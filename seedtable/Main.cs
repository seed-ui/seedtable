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

        [Option('S', "subdivide", Separator = ',', HelpText = "subdivide rules")]
        public IEnumerable<string> subdivide { get; set; } = new List<string> { };

        [Option('I', "ignore", Separator = ',', HelpText = "ignore sheet names")]
        public IEnumerable<string> ignore { get; set; } = new List<string> { };
        
        [Option('O', "only", Separator = ',', HelpText = "only sheet names")]
        public IEnumerable<string> only { get; set; } = new List<string> { };

        [Option('R', "require-version", Default = "", HelpText = "require version (with version column)")]
        public string requireVersion { get; set; } = "";

        [Option('v', "version-column", HelpText = "version column")]
        public string versionColumn { get; set; }

        [Option('y', "yaml-columns", Separator = ',', HelpText = "yaml columns")]
        public IEnumerable<string> yamlColumns { get; set; } = new List<string> { };

        [Option('n', "ignore-columns", Separator = ',', HelpText = "ignore columns")]
        public IEnumerable<string> ignoreColumns { get; set; } = new List<string> { };

        [Option("column-names-row", Default = 2, HelpText = "column names row index")]
        public int columnNamesRow { get; set; } = 2;

        [Option("data-start-row", Default = 3, HelpText = "data start row index")]
        public int dataStartRow { get; set; } = 3;

        [Option('e', "engine", Default = Engine.OpenXml, HelpText = "parser engine")]
        public virtual Engine engine { get; set; }
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

        [Option('d', "delete", Default = false, HelpText = "delete enabled")]
        public bool delete { get; set; } = false;

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
            var sheetsConfig = new SheetsConfig(options.only, options.ignore);
            foreach (var sheetName in excelData.SheetNames) {
                Log($"    {sheetName}");
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
                    yamlData = YamlData.ReadFrom(sheetName, options.seedInput);
                } catch (FileNotFoundException exception) {
                    Log("      skip", $"seed file [{exception.FileName}] not found");
                    continue;
                }
                try {
                    seedTable.DataToExcel(yamlData.data, options.delete);
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
            var sheetsConfig = new SheetsConfig(options.only, options.ignore, options.subdivide);
            foreach (var sheetName in excelData.SheetNames) {
                Log($"    {sheetName}");
                if (!sheetsConfig.IsUseSheet(sheetName)) {
                    Log("      ignore", "skip");
                    continue;
                }
                var subdivide = sheetsConfig.subdivide(sheetName);
                var seedTable = GetSeedTable(excelData, sheetName, options);
                if (seedTable.Errors.Count != 0) {
                    continue;
                }
                new YamlData(seedTable.ExcelToData(options.requireVersion)).WriteTo(sheetName, options.output, subdivide.NeedSubdivide, subdivide.CutPrefix, subdivide.CutPostfix, yamlColumnNames: options.yamlColumns);
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
            public SheetsConfig(IEnumerable<string> only, IEnumerable<string> ignore, IEnumerable<string> subdivide = null) {
                var subdivideSheetNameWithSubdivides = subdivide == null ? new SheetNameWithSubdivide[] { } : subdivide.Select((sheetName) => SheetNameWithSubdivide.FromMixed(sheetName));
                var onlySheetNameWithSubdivides = only.Select((sheetName) => SheetNameWithSubdivide.FromMixed(sheetName));
                foreach(var sheetNameWithSubdivide in onlySheetNameWithSubdivides.Concat(subdivideSheetNameWithSubdivides)) {
                    subdivideRules[sheetNameWithSubdivide.ToString()] = sheetNameWithSubdivide;
                }
                onlySheetNames = new HashSet<string>(onlySheetNameWithSubdivides.Select((sheetNameWithSubdivide) => sheetNameWithSubdivide.ToString()));
                ignoreSheetNames = new HashSet<string>(ignore.Select((sheetName) => SheetNameWithSubdivide.FromMixed(sheetName).ToString()));
            }

            Dictionary<string, SheetNameWithSubdivide> subdivideRules = new Dictionary<string, SheetNameWithSubdivide>();
            HashSet<string> onlySheetNames;
            HashSet<string> ignoreSheetNames;

            public bool IsUseSheet(string sheetName) {
                if (ignoreSheetNames.Contains(sheetName)) return false;
                if (onlySheetNames.Count != 0 && !onlySheetNames.Contains(sheetName)) return false;
                return true;
            }

            public SheetNameWithSubdivide subdivide(string sheetName) {
                return subdivideRules.ContainsKey(sheetName) ? subdivideRules[sheetName] : new SheetNameWithSubdivide(sheetName);
            }
        }

        class SheetNameWithSubdivide {
            public SheetNameWithSubdivide(string sheetName, bool needSubdivide = false, int cutPrefix = 0, int cutPostfix = 0) {
                this.SheetName = sheetName;
                this.NeedSubdivide = needSubdivide;
                this.CutPrefix = cutPrefix;
                this.CutPostfix = cutPostfix;
            }

            public string SheetName { get; }
            public bool NeedSubdivide { get; }
            public int CutPrefix { get; }
            public int CutPostfix { get; }

            public override string ToString() => this.SheetName;

            public static SheetNameWithSubdivide FromMixed(string sheetNameMixed) {
                var result = Regex.Match(sheetNameMixed, @"^(?:(\d+):)?(.+?)(?::(\d+))?$");
                if (!result.Success) throw new Exception($"{sheetNameMixed} is wrong sheet name and subdivide rule definition");
                var cutPrefix = result.Groups[1].Value;
                var sheetName = result.Groups[2].Value;
                var cutPostfix = result.Groups[3].Value;
                var needSubdivide = cutPrefix.Length != 0 || cutPostfix.Length != 0;
                return new SheetNameWithSubdivide(
                    sheetName,
                    needSubdivide,
                    cutPrefix.Length == 0 ? 0 : Convert.ToInt32(cutPrefix),
                    cutPostfix.Length == 0 ? 0 :Convert.ToInt32(cutPostfix)
                    );
            }
        }
    }
}
