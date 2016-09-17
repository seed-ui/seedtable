using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
using CommandLine;

namespace SeedTable {
    [Verb("from", HelpText ="Yaml from Excel")]
    class FromOptions {
        public enum Engine {
            OpenXml,
            ClosedXml,
        }

        [Value(0, Required = true, HelpText = "xlsx files")]
        public IEnumerable<string> files { get; set; }

        [Option('S', "subdivide", Separator = ',', HelpText = "subdivide rules")]
        public IEnumerable<string> subdivide { get; set; }

        [Option('I', "ignore", Separator = ',', HelpText = "ignore sheet names")]
        public IEnumerable<string> ignore { get; set; }
        
        [Option('O', "only", Separator = ',', HelpText = "only sheet names")]
        public IEnumerable<string> only { get; set; }

        [Option('i', "input", Default = ".", HelpText = "input directory")]
        public string input { get; set; }

        [Option('o', "output", Default = ".", HelpText = "output directory")]
        public string output { get; set; }

        [Option('d', "stdout", Default = false, HelpText = "output one sheets to stdout")]
        public bool stdout { get; set; }

        [Option('n', "ignore-columns", Separator = ',', HelpText = "ignore columns")]
        public IEnumerable<string> ignoreColumns { get; set; }

        [Option('e', "engine", Default = Engine.OpenXml, HelpText = "parser engine")]
        public Engine engine { get; set; }
    }

    [Verb("to", HelpText = "Yaml to Excel")]
    class ToOptions {
        public enum Engine {
            ClosedXml,
        }

        [Value(0, Required = true, HelpText = "xlsx files")]
        public IEnumerable<string> files { get; set; }

        [Option('I', "ignore", Separator = ',', HelpText = "ignore sheet names")]
        public IEnumerable<string> ignore { get; set; }
        
        [Option('O', "only", Separator = ',', HelpText = "only sheet names")]
        public IEnumerable<string> only { get; set; }

        [Option('i', "input", Default = ".", HelpText = "input directory")]
        public string input { get; set; }

        [Option('o', "output", Default = ".", HelpText = "output directory (or overwrite)")]
        public string output { get; set; }

        [Option('d', "delete", Default = false, HelpText = "delete enabled")]
        public bool delete { get; set; }

        [Option('e', "engine", Default = Engine.ClosedXml, HelpText = "parser engine")]
        public Engine engine { get; set; }
    }

    class MainClass {
        public static void Main (string[] args) {
            var options = CommandLine.Parser.Default.ParseArguments<FromOptions, ToOptions>(args);
            options.MapResult(
                (FromOptions opts) => ExcelToSeed(opts),
                (ToOptions opts) => SeedToExcel(opts),
                error => true
                );
        }

        static bool SeedToExcel(ToOptions options) {
            var sheetsConfig = new SheetsConfig(options.only, options.ignore);
            foreach (var file in options.files) {
                var filePath = Path.Combine(options.input, file);
                if (options.engine == ToOptions.Engine.ClosedXml) {
                    using (var workbook = new XLWorkbook(filePath)) {
                        var excel = new ClosedExcelData(workbook);
                        var sheetNames = excel.SheetNames.Where(sheetName => sheetsConfig.IsUseSheet(sheetName));
                        foreach (var sheetName in sheetNames) {
                            var yamlData = YamlData.ReadFrom(sheetName, options.input);
                            excel.GetSeedTable(sheetName).DataToExcel(yamlData.data, options.delete);
                        }
                        if (options.output.Length == 0) {
                            workbook.Save();
                        } else {
                            workbook.SaveAs(Path.Combine(options.output, file));
                        }
                    }
                }
            }
            return true;
        }

        static bool ExcelToSeed(FromOptions options) {
            var sheetsConfig = new SheetsConfig(options.only, options.ignore, options.subdivide);
            foreach (var file in options.files) {
                var filePath = Path.Combine(options.input, file);
                if (options.engine == FromOptions.Engine.OpenXml) {
                    using (var document = SpreadsheetDocument.Open(filePath, false)) {
                        var excel = new ExcelData(document);
                        var sheetNames = excel.SheetNames.Where(sheetName => sheetsConfig.IsUseSheet(sheetName));
                        foreach (var sheetName in sheetNames) {
                            var subdivide = sheetsConfig.subdivide(sheetName);
                            new YamlData(excel.ExcelToData(sheetName))
                                .WriteTo(sheetName, options.output, subdivide.CutPrefix, subdivide.CutPostfix);
                        }
                    }
                } else {
                    using (var workbook = new XLWorkbook(filePath)) {
                        var excel = new ClosedExcelData(workbook);
                        var sheetNames = excel.SheetNames.Where(sheetName => sheetsConfig.IsUseSheet(sheetName));
                        foreach (var sheetName in sheetNames) {
                            var subdivide = sheetsConfig.subdivide(sheetName);
                            new YamlData(excel.GetSeedTable(sheetName).ExcelToData())
                                .WriteTo(sheetName, options.output, subdivide.CutPrefix, subdivide.CutPostfix);
                        }
                    }
                }
            }
            return true;
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

    /*class DataTable {
        public List<string> columns { get; private set; }
        public IEnumerable<IEnumerable<string>> table { get; private set; }
        public DataTable(List<string> columns, IEnumerable<IEnumerable<string>> table) {
            this.columns = columns;
            this.table = table;
        }
        public DataDictionaryList ToDictionaryList() {
            var table = this.table.Select(row => row.Select((col, index) => new { col, index }).ToDictionary(elem => this.columns[elem.index], elem => elem.col));
            return new DataDictionaryList(table);
        }
    }*/
}
