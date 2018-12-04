using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace SeedTable {
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
            var fileName = Path.GetFileName(file);
            var sheetsConfig = new SheetsConfig(options.only, options.ignore, options.subdivide, null, options.mapping, options.alias);
            var yamlDataCache = new Dictionary<string, YamlData>(); // aliasのため同テーブルはキャッシュする
            foreach (var sheetName in excelData.SheetNames) {
                var yamlTableName = sheetsConfig.YamlTableName(fileName, sheetName);
                if (yamlTableName == sheetName) {
                    Log($"    {yamlTableName}");
                } else {
                    Log($"    {yamlTableName} -> {sheetName}");
                }
                if (!sheetsConfig.IsUseSheet(fileName, sheetName, yamlTableName, OnOperation.To)) {
                    Log("      ignore", "skip");
                    continue;
                }
                var subdivide = sheetsConfig.subdivide(fileName, yamlTableName, OnOperation.To);
                var seedTable = GetSeedTable(excelData, sheetName, options, subdivide);
                if (seedTable.Errors.Count != 0) {
                    continue;
                }
                YamlData yamlData = null;
                if (!yamlDataCache.TryGetValue(yamlTableName, out yamlData)) {
                    try {
                        yamlData = YamlData.ReadFrom(yamlTableName, options.seedInput, options.seedExtension, subdivide.KeyColumnName);
                        yamlDataCache[yamlTableName] = yamlData;
                    } catch (FileNotFoundException exception) {
                        Log("      skip", $"seed file [{exception.FileName}] not found");
                        continue;
                    }
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
                            previousTime = ExcelToSeedCore(excelData, file, options, previousTime, parseFinishTime);
                        }
                        break;
                    case FromOptions.Engine.ClosedXML:
                        using (var excelData = ClosedXML.ExcelData.FromFile(filePath)) {
                            var parseFinishTime = DateTime.Now;
                            DurationLog("  parse-time", startTime, parseFinishTime);
                            previousTime = ExcelToSeedCore(excelData, file, options, previousTime, parseFinishTime);
                        }
                        break;
                    case FromOptions.Engine.EPPlus:
                        using (var excelData = EPPlus.ExcelData.FromFile(filePath)) {
                            var parseFinishTime = DateTime.Now;
                            DurationLog("  parse-time", startTime, parseFinishTime);
                            previousTime = ExcelToSeedCore(excelData, file, options, previousTime, parseFinishTime);
                        }
                        break;
                }
            }
            DurationLog("total", startTime, DateTime.Now);
            return true;
        }

        static DateTime ExcelToSeedCore(IExcelData excelData, string file, FromOptions options, DateTime startTime, DateTime previousTime) {
            Log("  sheets");
            var fileName = Path.GetFileName(file);
            var sheetsConfig = new SheetsConfig(options.only, options.ignore, options.subdivide, options.primary, options.mapping, options.alias);
            foreach (var sheetName in excelData.SheetNames) {
                var yamlTableName = sheetsConfig.YamlTableName(fileName, sheetName);
                if (yamlTableName == sheetName) {
                    Log($"    {yamlTableName}");
                } else {
                    Log($"    {yamlTableName} <- {sheetName}");
                }
                if (!sheetsConfig.IsUseSheet(fileName, sheetName, yamlTableName, OnOperation.From)) {
                    Log("      ignore", "skip");
                    continue;
                }
                var subdivide = sheetsConfig.subdivide(fileName, yamlTableName, OnOperation.From);
                var seedTable = GetSeedTable(excelData, sheetName, options, subdivide);
                if (seedTable.Errors.Count != 0) {
                    continue;
                }
                new YamlData(
                    seedTable.ExcelToData(options.requireVersion),
                    subdivide.NeedSubdivide,
                    subdivide.CutPrefix,
                    subdivide.CutPostfix,
                    subdivide.SubdivideFilename,
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

        static SeedTableBase GetSeedTable(IExcelData excelData, string sheetName, CommonOptions options, SheetNameWithSubdivide subdivide) {
            var seedTable = excelData.GetSeedTable(sheetName, subdivide.ColumnNamesRow ?? options.columnNamesRow, subdivide.DataStartRow ?? options.dataStartRow, options.ignoreColumns, subdivide.KeyColumnName, options.versionColumn);
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
    }
}
