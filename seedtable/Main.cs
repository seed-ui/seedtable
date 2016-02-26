using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
using CommandLine;

namespace seedtable {
    [Verb("from", HelpText ="Yaml from Excel")]
    class FromOptions {
        public enum Engine {
            OpenXml,
            ClosedXml,
        }

        [Value(0, Required = true, HelpText = "Excel file name")]
        public string excel_name { get; set; }
        
        [Value(1, HelpText = "sheet names (default = all)")]
        public IEnumerable<string> sheet_names { get; set; }

        [Option('o', "output", Default = ".", HelpText = "output directory")]
        public string directory { get; set; }

        [Option('e', "engine", Default = Engine.OpenXml, HelpText = "parser engine")]
        public Engine engine { get; set; }
    }

    [Verb("to", HelpText = "Yaml to Excel")]
    class ToOptions {
        [Value(0, Required = true, HelpText = "Excel file name")]
        public string excel_name { get; set; }

        [Value(1, HelpText = "sheet names (default = all) ex. sheetname--3 = subdivide 3")]
        public IEnumerable<string> sheet_names { get; set; }

        [Option('i', "input", Default = ".", HelpText = "input directory")]
        public string directory { get; set; }

        [Option('o', "output", Default = "", HelpText = "output xlsx name (or overwrite)")]
        public string output_excel_name { get; set; }

        [Option('d', "delete", Default = false, HelpText = "delete enabled")]
        public bool delete { get; set; }
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
            using (var workbook = new XLWorkbook(options.excel_name)) {
                var excel = new ClosedExcelData(workbook);
                (options.sheet_names.Count() > 0 ? options.sheet_names : excel.SheetNames).ForEach(
                    sheet_name => excel.GetSeedTable(SheetNameOriginal(sheet_name)).DataToExcel(YamlData.ReadFrom(SheetNameOriginal(sheet_name), options.directory).data, options.delete)
                    );
                if (options.output_excel_name.Length == 0) {
                    workbook.Save();
                } else {
                    workbook.SaveAs(options.output_excel_name);
                }
            }
            return true;
        }

        static bool ExcelToSeed(FromOptions options) {
            if (options.engine == FromOptions.Engine.OpenXml) {
                using (var document = SpreadsheetDocument.Open(options.excel_name, false)) {
                    var excel = new ExcelData(document);
                    (options.sheet_names.Count() > 0 ? options.sheet_names : excel.SheetNames).ForEach(
                        sheet_name => new YamlData(excel.ExcelToData(SheetNameOriginal(sheet_name)))
                            .WriteTo(SheetNameOriginal(sheet_name), options.directory, SheetNamePreCut(sheet_name), SheetNamePostCut(sheet_name))
                        );
                }
            } else {
                using (var workbook = new XLWorkbook(options.excel_name)) {
                    var excel = new ClosedExcelData(workbook);
                    (options.sheet_names.Count() > 0 ? options.sheet_names : excel.SheetNames).ForEach(
                        sheet_name => new YamlData(excel.GetSeedTable(SheetNameOriginal(sheet_name)).ExcelToData())
                            .WriteTo(SheetNameOriginal(sheet_name), options.directory, SheetNamePreCut(sheet_name), SheetNamePostCut(sheet_name))
                        );
                }
            }
            return true;
        }

        static string SheetNameOriginal(string sheet_name) {
            return Regex.Replace(Regex.Replace(sheet_name, @"^\d+--", ""), @"--\d+$", "");
        }

        static int SheetNamePreCut(string sheet_name) {
            var result = Regex.Match(sheet_name, @"^(\d+)--");
            return result.Success ? Convert.ToInt32(result.Groups[1].Value) : 0;
        }

        static int SheetNamePostCut(string sheet_name) {
            var result = Regex.Match(sheet_name, @"--(\d+)$");
            return result.Success ? Convert.ToInt32(result.Groups[1].Value) : 0;
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
