using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using CommandLine;

namespace SeedTable {

    public class CommonOptions {
        public enum Engine {
            OpenXml,
            ClosedXML,
            EPPlus,
        }

        [Value(0, Required = true, HelpText = "xlsx files")]
        public IEnumerable<string> files { get; set; }

        [Option('o', "output", Default = ".", HelpText = "output directory")]
        public string output { get; set; } = ".";

        [Option('S', "subdivide", Separator = ',', HelpText = "subdivide rules : [(pre cut):]sheet-name[:(post cut)]")]
        public IEnumerable<string> subdivide { get; set; } = new List<string> { };

        [Option('I', "ignore", Separator = ',', HelpText = "ignore sheet names")]
        public IEnumerable<string> ignore { get; set; } = new List<string> { };
        
        [Option('O', "only", Separator = ',', HelpText = "only sheet names")]
        public IEnumerable<string> only { get; set; } = new List<string> { };
        
        [Option('M', "mapping", Separator = ',', HelpText = "sheet names mapping : (seed table name):(excel sheet name)")]
        public IEnumerable<string> mapping { get; set; } = new List<string> { };

        [Option('R', "require-version", Default = "", HelpText = "require version (with version column)")]
        public string requireVersion { get; set; } = "";

        [Option('v', "version-column", HelpText = "version column")]
        public string versionColumn { get; set; }

        [Option('y', "yaml-columns", Separator = ',', HelpText = "yaml columns")]
        public IEnumerable<string> yamlColumns { get; set; } = new List<string> { };

        [Option('n', "ignore-columns", Separator = ',', HelpText = "ignore columns")]
        public IEnumerable<string> ignoreColumns { get; set; } = new List<string> { };

        [Option('f', "format", Separator = ',', HelpText = "format")]
        public SeedYamlFormat format { get; set; } = SeedYamlFormat.Hash;

        [Option("column-names-row", Default = 2, HelpText = "column names row index")]
        public int columnNamesRow { get; set; } = 2;

        [Option("data-start-row", Default = 3, HelpText = "data start row index")]
        public int dataStartRow { get; set; } = 3;

        [Option('e', "engine", Default = Engine.OpenXml, HelpText = "parser engine")]
        public virtual Engine engine { get; set; }

        [Option('d', "delete", Default = false, HelpText = "delete data which is not exists in source")]
        public bool delete { get; set; } = false;

        [Option("seed-extension", Default = ".yml", HelpText = "seed file extension")]
        public virtual string seedExtension { get; set; } = ".yml";
    }

    [Verb("from", HelpText ="Yaml from Excel")]
    public class FromOptions : CommonOptions {
        [Option('e', "engine", Default = Engine.OpenXml, HelpText = "parser engine")]
        public override Engine engine { get; set; } = Engine.OpenXml;

        // [Option('d', "stdout", Default = false, HelpText = "output one sheets to stdout")]
        // public bool stdout { get; set; }

        [Option('i', "input", Default = ".", HelpText = "input directory")]
        public string input { get; set; } = ".";
    }

    [Verb("to", HelpText = "Yaml to Excel")]
    public class ToOptions : CommonOptions {
        [Option('e', "engine", Default = Engine.EPPlus, HelpText = "parser engine")]
        public override Engine engine { get; set; } = Engine.EPPlus;

        [Option('s', "seed-input", Default = ".", HelpText = "seed input directory")]
        public string seedInput { get; set; } = ".";

        [Option('x', "xlsx-input", Default = ".", HelpText = "xlsx input directory")]
        public string xlsxInput { get; set; } = ".";

        [Option('c', "calc-formulas", Default = false, HelpText = "calculate all formulas and store results to cache fields")]
        public bool calcFormulas { get; set; } = false;
    }

    class MainClass {
        public static void Main(string[] args) {
            SeedTableInterface.InformationMessageEvent += (message) => Console.Error.WriteLine(message);
            var options = CommandLine.Parser.Default.ParseArguments<FromOptions, ToOptions>(args);
            try {
                options.MapResult(
                    (FromOptions opts) => SeedTableInterface.ExcelToSeed(opts),
                    (ToOptions opts) => SeedTableInterface.SeedToExcel(opts),
                    error => true
                    );
            } catch (SeedTableInterface.CannotContinueException) {
                Environment.Exit(1);
            }
        }
    }

    public class SeedTableInterface {
        public delegate void InformationMessageEventHandler(string message);
        public static event InformationMessageEventHandler InformationMessageEvent = delegate { };
        static void WriteInfo(string message) => InformationMessageEvent(message);

        public class CannotContinueException : InvalidOperationException { }

        public static bool SeedToExcel(ToOptions options) {
            Log("engine", options.engine);
            Log("output-directory", options.output);
            var startTime = DateTime.Now;
            var previousTime = startTime;
            foreach (var file in options.files) {
                var filePath = Path.Combine(options.xlsxInput, file);
                Log(file);
                Log("  full-path", filePath);
                CheckFileExists(filePath);
                switch (options.engine) {
                    case ToOptions.Engine.OpenXml:
                        using (var excelData = OpenXml.ExcelData.FromFile(filePath, true)) {
                            var parseFinishTime = DateTime.Now;
                            DurationLog("  parse-time", previousTime, parseFinishTime);
                            previousTime = SeedToExcelCore(excelData, file, options, startTime, parseFinishTime);
                        }
                        break;
                    case ToOptions.Engine.ClosedXML:
                        using (var excelData = ClosedXML.ExcelData.FromFile(filePath)) {
                            var parseFinishTime = DateTime.Now;
                            DurationLog("  parse-time", previousTime, parseFinishTime);
                            previousTime = SeedToExcelCore(excelData, file, options, startTime, parseFinishTime);
                        }
                        break;
                    case ToOptions.Engine.EPPlus:
                        using (var excelData = EPPlus.ExcelData.FromFile(filePath)) {
                            var parseFinishTime = DateTime.Now;
                            DurationLog("  parse-time", previousTime, parseFinishTime);
                            previousTime = SeedToExcelCore(excelData, file, options, startTime, parseFinishTime);
                        }
                        break;
                }
            }
            DurationLog("total", startTime, DateTime.Now);
            return true;
        }

        static DateTime SeedToExcelCore(IExcelData excelData, string file, ToOptions options, DateTime startTime, DateTime previousTime) {
            Log("  sheets");
            var sheetsConfig = new SheetsConfig(options.only, options.ignore, null, options.mapping);
            foreach (var sheetName in excelData.SheetNames) {
                var yamlTableName = sheetsConfig.YamlTableName(sheetName);
                if (yamlTableName == sheetName) {
                    Log($"    {yamlTableName}");
                } else {
                    Log($"    {yamlTableName} -> {sheetName}");
                }
                if (!sheetsConfig.IsUseSheet(sheetName)) {
                    Log("      ignore", "skip");
                    continue;
                }
                var seedTable = GetSeedTable(excelData, sheetName, options);
                if (seedTable.Errors.Count != 0) {
                    continue;
                }
                YamlData yamlData = null;
                try {
                    yamlData = YamlData.ReadFrom(yamlTableName, options.seedInput, options.seedExtension);
                } catch (FileNotFoundException exception) {
                    Log("      skip", $"seed file [{exception.FileName}] not found");
                    continue;
                }
                try {
                    seedTable.DataToExcel(yamlData.Data, options.delete);
                } catch (IdParseException exception) {
                    WriteInfo($"      ERROR: {exception.Message}");
                    throw new CannotContinueException();
                }
                var now = DateTime.Now;
                DurationLog("      write-time", previousTime, now);
                previousTime = now;
            }
            // 数式を再計算して結果をキャッシュする
            if (options.calcFormulas && excelData is EPPlus.ExcelData) ((EPPlus.ExcelData)excelData).Calculate();
            if (options.output.Length == 0) {
                excelData.Save();
                Log("  write-path", "overwrite");
            } else {
                var writePath = Path.Combine(options.output, file);
                Log("  write-path", writePath);
                if (!Directory.Exists(options.output)) Directory.CreateDirectory(options.output);
                excelData.SaveAs(writePath);
            }
            var end = DateTime.Now;
            DurationLog("  write-time", previousTime, end);
            return end;
        }

        public static bool ExcelToSeed(FromOptions options) {
            Log("engine", options.engine);
            Log("output-directory", options.output);
            var startTime = DateTime.Now;
            var previousTime = startTime;
            foreach (var file in options.files) {
                var filePath = Path.Combine(options.input, file);
                Log(file);
                Log("  full-path", filePath);
                CheckFileExists(filePath);
                switch (options.engine) {
                    case FromOptions.Engine.OpenXml:
                        using (var excelData = OpenXml.ExcelData.FromFile(filePath, false)) {
                            var parseFinishTime = DateTime.Now;
                            DurationLog("  parse-time", startTime, parseFinishTime);
                            previousTime = ExcelToSeedCore(excelData, options, previousTime, parseFinishTime);
                        }
                        break;
                    case FromOptions.Engine.ClosedXML:
                        using (var excelData = ClosedXML.ExcelData.FromFile(filePath)) {
                            var parseFinishTime = DateTime.Now;
                            DurationLog("  parse-time", startTime, parseFinishTime);
                            previousTime = ExcelToSeedCore(excelData, options, previousTime, parseFinishTime);
                        }
                        break;
                    case FromOptions.Engine.EPPlus:
                        using (var excelData = EPPlus.ExcelData.FromFile(filePath)) {
                            var parseFinishTime = DateTime.Now;
                            DurationLog("  parse-time", startTime, parseFinishTime);
                            previousTime = ExcelToSeedCore(excelData, options, previousTime, parseFinishTime);
                        }
                        break;
                }
            }
            DurationLog("total", startTime, DateTime.Now);
            return true;
        }

        static DateTime ExcelToSeedCore(IExcelData excelData, FromOptions options, DateTime startTime, DateTime previousTime) {
            Log("  sheets");
            var sheetsConfig = new SheetsConfig(options.only, options.ignore, options.subdivide, options.mapping);
            foreach (var sheetName in excelData.SheetNames) {
                var yamlTableName = sheetsConfig.YamlTableName(sheetName);
                if (yamlTableName == sheetName) {
                    Log($"    {yamlTableName}");
                } else {
                    Log($"    {yamlTableName} <- {sheetName}");
                }
                if (!sheetsConfig.IsUseSheet(sheetName)) {
                    Log("      ignore", "skip");
                    continue;
                }
                var subdivide = sheetsConfig.subdivide(yamlTableName);
                var seedTable = GetSeedTable(excelData, sheetName, options);
                if (seedTable.Errors.Count != 0) {
                    continue;
                }
                new YamlData(
                    seedTable.ExcelToData(options.requireVersion),
                    subdivide.NeedSubdivide,
                    subdivide.CutPrefix,
                    subdivide.CutPostfix,
                    options.format,
                    options.delete,
                    options.yamlColumns
                ).WriteTo(
                    yamlTableName,
                    options.output,
                    options.seedExtension
                );
                var now = DateTime.Now;
                DurationLog("      write-time", previousTime, now);
                previousTime = now;
            }
            return previousTime;
        }

        static SeedTableBase GetSeedTable(IExcelData excelData, string sheetName, CommonOptions options) {
            var seedTable = excelData.GetSeedTable(sheetName, options.columnNamesRow, options.dataStartRow, options.ignoreColumns, options.versionColumn);
            if (seedTable.Errors.Count != 0) {
                var skipExceptions = seedTable.Errors.Where(error => error is NoIdColumnException);
                if (skipExceptions.Count() != 0) {
                    foreach (var error in skipExceptions) {
                        WriteInfo($"      skip: {error.Message}");
                    }
                } else {
                    foreach(var error in seedTable.Errors) {
                        WriteInfo($"      ERROR: {error.Message}");
                    }
                    throw new CannotContinueException();
                }
            }
            return seedTable;
        }

        static void CheckFileExists(string file) {
            if (!File.Exists(file)) {
                WriteInfo($"file not found [{file}]");
                throw new CannotContinueException();
            }
        }

        static void Log(string prefix, object value = null) {
            WriteInfo($"{prefix}: {value}");
        }

        static void DurationLog(string prefix, DateTime start, DateTime end) {
            WriteInfo($"{prefix}: {(end - start).TotalMilliseconds} ms");
        }

        class SheetsConfig {
            public SheetsConfig(IEnumerable<string> only, IEnumerable<string> ignore, IEnumerable<string> subdivide = null, IEnumerable<string> mapping = null) {
                var subdivideSheetNames = SheetNameWithSubdivides.FromMixed(subdivide);
                OnlySheetNames = SheetNameWithSubdivides.FromMixed(only);
                IgnoreSheetNames = SheetNameWithSubdivides.FromMixed(ignore);
                SubdivideRules = new SheetNameWithSubdivides(subdivideSheetNames.Concat(OnlySheetNames));
                yamlToExcelMapping = mapping.Select(map => map.Split(':')).ToDictionary(map => map[0], map => map[1]);
                excelToYamlMapping = yamlToExcelMapping.ToDictionary(map => map.Value, map => map.Key);
            }

            SheetNameWithSubdivides SubdivideRules;
            SheetNameWithSubdivides IgnoreSheetNames;
            SheetNameWithSubdivides OnlySheetNames;
            Dictionary<string, string> yamlToExcelMapping;
            Dictionary<string, string> excelToYamlMapping;

            public bool IsUseSheet(string sheetName) {
                if (IgnoreSheetNames.Contains(sheetName)) return true;
                if (OnlySheetNames.Count != 0 && !OnlySheetNames.Contains(sheetName)) return false;
                return true;
            }

            public SheetNameWithSubdivide subdivide(string sheetName) {
                var subdivideRule = SubdivideRules.Find(sheetName);
                return subdivideRule ?? new SheetNameWithSubdivide(sheetName);
            }

            public string ExcelSheetName(string yamlTableName) {
                string excelSheetName;
                if (yamlToExcelMapping.TryGetValue(yamlTableName, out excelSheetName)) {
                    return excelSheetName;
                } else {
                    return yamlTableName;
                }
            }

            public string YamlTableName(string excelSheetName) {
                string yamlTableName;
                if (excelToYamlMapping.TryGetValue(excelSheetName, out yamlTableName)) {
                    return yamlTableName;
                } else {
                    return excelSheetName;
                }
            }
        }

        class SheetNameWithSubdivides : List<SheetNameWithSubdivide> {
            public static SheetNameWithSubdivides FromMixed(IEnumerable<string> mixedSheetNames = null) {
                return mixedSheetNames == null ?
                    new SheetNameWithSubdivides() :
                    new SheetNameWithSubdivides(
                        mixedSheetNames.Select(mixedSheetName => SheetNameWithSubdivide.FromMixed(mixedSheetName))
                    );
            }

            public SheetNameWithSubdivides() : base() { }

            public SheetNameWithSubdivides(IEnumerable<SheetNameWithSubdivide> sheetNameWithSubdivides) : base(
                sheetNameWithSubdivides.Where(
                    sheetNameWithSubdivide => sheetNameWithSubdivide.MatchType == SheetNameWithSubdivide.SheetNameMatchType.Exact
                ).Concat(
                    sheetNameWithSubdivides.Where(
                        sheetNameWithSubdivide => sheetNameWithSubdivide.MatchType == SheetNameWithSubdivide.SheetNameMatchType.Wildcard
                    )
                ).Concat(
                    sheetNameWithSubdivides.Where(
                        sheetNameWithSubdivide => sheetNameWithSubdivide.MatchType == SheetNameWithSubdivide.SheetNameMatchType.All
                    ).Take(1)
                )
            ) { }

            public SheetNameWithSubdivide Find(string sheetName) {
                return Find(sheetNameWithSubdivide => sheetNameWithSubdivide.IsMatch(sheetName));
            }

            public bool Contains(string sheetName) {
                return Find(sheetName) != null;
            }
        }

        class SheetNameWithSubdivide {
            public static SheetNameWithSubdivide FromMixed(string mixedSheetName) {
                var result = Regex.Match(mixedSheetName, @"^(?:(\d+):)?(.+?)(?::(\d+))?$");
                if (!result.Success) throw new Exception($"{mixedSheetName} is wrong sheet name and subdivide rule definition");
                var cutPrefixStr = result.Groups[1].Value;
                var sheetName = result.Groups[2].Value;
                var cutPostfixStr = result.Groups[3].Value;
                var needSubdivide = cutPrefixStr.Length != 0 || cutPostfixStr.Length != 0;
                var cutPrefix = cutPrefixStr.Length == 0 ? 0 : Convert.ToInt32(cutPrefixStr);
                var cutPostfix = cutPostfixStr.Length == 0 ? 0 : Convert.ToInt32(cutPostfixStr);
                return new SheetNameWithSubdivide(sheetName, needSubdivide, cutPrefix, cutPostfix);
            }

            public string SheetName { get; }
            public bool NeedSubdivide { get; }
            public int CutPrefix { get; }
            public int CutPostfix { get; }
            public SheetNameMatchType MatchType { get; }
            private Regex SheetNameMatcher { get; }

            public SheetNameWithSubdivide(string sheetName, bool needSubdivide = false, int cutPrefix = 0, int cutPostfix = 0) {
                SheetName = sheetName;
                NeedSubdivide = needSubdivide;
                CutPrefix = cutPrefix;
                CutPostfix = cutPostfix;
                if (SheetName == "*") {
                    MatchType = SheetNameMatchType.All;
                } else if (SheetName.Contains("*") || SheetName.Contains("?")) {
                    SheetNameMatcher = new Regex("^" + Regex.Escape(sheetName).Replace(@"\*", ".*").Replace(@"\?", ".") + "$");
                    MatchType = SheetNameMatchType.Wildcard;
                } else {
                    MatchType = SheetNameMatchType.Exact;
                }
            }

            public bool IsMatch(string sheetName) {
                switch (MatchType) {
                    case SheetNameMatchType.Exact:
                        return sheetName == SheetName;
                    case SheetNameMatchType.Wildcard:
                        return SheetNameMatcher.IsMatch(sheetName);
                    default:
                        return true;
                }
            }

            public enum SheetNameMatchType {
                All = 0,
                Wildcard,
                Exact,
            }
        }
    }
}
