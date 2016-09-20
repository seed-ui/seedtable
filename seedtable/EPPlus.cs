using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;

namespace SeedTable {
    namespace EPPlus {
        class ExcelData : IExcelData {
            ExcelPackage Document { get; }

            public static ExcelData FromFile(string file) {
                var fileInfo = new FileInfo(file);
                if (!fileInfo.Exists) throw new FileNotFoundException(null, file);
                return new ExcelData(new ExcelPackage(fileInfo));
            }

            public ExcelData(ExcelPackage document) {
                Document = document;
            }

            public IEnumerable<string> SheetNames { get { return Document.Workbook.Worksheets.Select(sheet => sheet.Name); } }

            public SeedTableBase GetSeedTable(string sheetName, int columnNamesRowIndex = 2, int dataStartRowIndex = 3, IEnumerable<string> ignoreColumnNames = null, string versionColumnName = null) {
                try {
                    return new SeedTable(Document.Workbook.Worksheets[sheetName], columnNamesRowIndex, dataStartRowIndex, ignoreColumnNames, versionColumnName);
                } catch (KeyNotFoundException exception) {
                    throw new SheetNotFoundException($"sheet [{sheetName}] not found in [{Document.File}]", exception);
                }
            }

            public void Save() => Document.Save();

            public void SaveAs(string file) => Document.SaveAs(new FileInfo(file));

            bool disposed = false;

            ~ExcelData() {
                this.Dispose(false);
            }

            public void Dispose() {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool isDisposing) {
                if (disposed) return;
                if (isDisposing) Document.Dispose();
                disposed = true;
            }
        }

        class SeedTable : SeedTableBase {
            ExcelWorksheet Worksheet;

            public List<SeedTableColumn> Columns { get; } = new List<SeedTableColumn>();
            public int VersionColumnIndex { get; private set; }
            public int IdColumnIndex { get; private set; }

            public SeedTable(ExcelWorksheet worksheet, int columnNamesRowIndex = 2, int dataStartRowIndex = 3, IEnumerable<string> ignoreColumnNames = null, string versionColumnName = null) : base(columnNamesRowIndex, dataStartRowIndex, ignoreColumnNames, versionColumnName) {
                Worksheet = worksheet;
                GetColumns();
            }

            void GetColumns() {
                foreach(var columnIndex in Enumerable.Range(1, Worksheet.Dimension.Columns)) {
                    var cell = Worksheet.Cells[ColumnNamesRowIndex, columnIndex];
                    var value = cell.GetValue<string>();
                    if (IgnoreColumnNames.Contains(value)) continue;
                    if (VersionColumnName == value) {
                        VersionColumnIndex = columnIndex;
                    } else {
                        if ("id" == value) IdColumnIndex = columnIndex;
                        Columns.Add(new SeedTableColumn(value, columnIndex));
                    }
                }
                CheckColumns();
            }

            void CheckColumns() {
                if (IdColumnIndex == 0) Errors.Add(new NoIdColumnException($"id column not found [{SheetName}]"));
                var columnNames = new HashSet<string>();
                foreach(var column in Columns) {
                    var columnName = column.Name;
                    if (columnNames.Contains(columnName)) Errors.Add(new DuplicateColumnNameException($"Duplicate column name [{columnName}] found in sheet [{SheetName}]"));
                    columnNames.Add(columnName);
                }
            }

            public override string SheetName { get { return Worksheet.Name; } }

            public override DataDictionaryList ExcelToData(string requireVersion = "") {
                var table = Enumerable.Range(DataStartRowIndex, Worksheet.Dimension.Rows).Select(rowIndex => GetCellValuesDictionary(rowIndex));
                return new DataDictionaryList(table);
            }

            Dictionary<string, object> GetCellValuesDictionary(int rowIndex) => Columns.ToDictionary(column => column.Name, column => Worksheet.Cells[rowIndex, column.Index].Value);

            public override void DataToExcel(DataDictionaryList data, bool delete = false) {
                var indexedData = data.IndexById();
                var idIndexes = new List<IdIndex>();
                var restIds = new HashSet<string>(indexedData.Keys);
                // replace
                foreach (var rowIndex in Enumerable.Range(DataStartRowIndex, Worksheet.Dimension.Rows)) {
                    var id = Worksheet.Cells[rowIndex, IdColumnIndex].GetValue<string>();
                    if (id == null || id == "") continue;
                    idIndexes.Add(new IdIndex(id, rowIndex));
                    Dictionary<string, object> rowData;
                    if (indexedData.TryGetValue(id, out rowData)) {
                        restIds.Remove(id);
                        foreach (var column in Columns) {
                            object value;
                            if (rowData.TryGetValue(column.Name, out value)) {
                                var cell = Worksheet.Cells[rowIndex, column.Index];
                                if (cell.Formula != null && cell.Formula.Length != 0) continue;
                                cell.Value = value;
                            }
                        }
                    }
                }
                // add
                var restIdGroups = GetReversedIdGroups(
                    idIndexes.Select(idIndex => {
                        try {
                            return long.Parse(idIndex.Id);
                        } catch (FormatException exception) {
                            throw new IdParseException(idIndex.Id, exception);
                        }
                    }).ToList(),
                    restIds.Select(id => {
                        try {
                            return long.Parse(id);
                        } catch (FormatException exception) {
                            throw new IdParseException(id, exception);
                        }
                    }).ToList()
                );
                var doAdd = restIdGroups.Count != 0;
                List<string> lastRestIdGroup = null;
                if (restIdGroups.TryGetValue("0", out lastRestIdGroup)) {
                    restIdGroups.Remove("0");
                }
                var endColumnIndex = Worksheet.Dimension.Columns;
                // 逆順に処理
                idIndexes.Reverse();
                foreach (var idIndex in idIndexes) {
                    List<string> idGroup;
                    if (restIdGroups.TryGetValue(idIndex.Id, out idGroup)) {
                        var previousRowIndex = idIndex.Index;
                        var newRowIndex = previousRowIndex + 1;
                        // 行を挿入数分増やす
                        Worksheet.InsertRow(newRowIndex, idGroup.Count, previousRowIndex);
                        // 直上行
                        var previousRow = Worksheet.Cells[previousRowIndex, 1, previousRowIndex, endColumnIndex];
                        // idGroupはID逆順
                        var rowIndexOffset = idGroup.Count - 1;
                        foreach (var id in idGroup) {
                            var rowIndex = newRowIndex + rowIndexOffset;
                            // 直上行の値をコピー (数式等のため)
                            previousRow.Copy(Worksheet.Cells[rowIndex, 1]);
                            var rowData = indexedData[id];
                            foreach (var column in Columns) {
                                object value;
                                if (rowData.TryGetValue(column.Name, out value)) {
                                    var cell = Worksheet.Cells[rowIndex, column.Index];
                                    if (cell.Formula != null && cell.Formula.Length != 0) continue;
                                    cell.Value = value;
                                }
                            }
                            --rowIndexOffset;
                        }
                    }
                }
                // 最初の行には異なった処理が必要
                if (lastRestIdGroup != null) {
                    Worksheet.InsertRow(DataStartRowIndex, lastRestIdGroup.Count, DataStartRowIndex);
                    var newRowIndex = DataStartRowIndex;
                    var nextRowIndex = DataStartRowIndex + lastRestIdGroup.Count;
                    var nextRow = Worksheet.Cells[nextRowIndex, 1, nextRowIndex, endColumnIndex];
                    var rowIndexOffset = lastRestIdGroup.Count - 1;
                    foreach (var id in lastRestIdGroup) {
                        var rowIndex = newRowIndex + rowIndexOffset;
                        nextRow.Copy(Worksheet.Cells[rowIndex, 1]);
                        var rowData = indexedData[id];
                        foreach (var column in Columns) {
                            object value;
                            if (rowData.TryGetValue(column.Name, out value)) {
                                var cell = Worksheet.Cells[rowIndex, column.Index];
                                if (cell.Formula != null && cell.Formula.Length != 0) continue;
                                cell.Value = value;
                            }
                        }
                        --rowIndexOffset;
                    }
                }
                // delete
                if (delete) {
                    var allIds = new HashSet<string>(indexedData.Keys);
                    // 行追加がある場合はインデックスの対応を再走査する
                    if (doAdd) {
                        idIndexes = new List<IdIndex>();
                        foreach (var rowIndex in Enumerable.Range(DataStartRowIndex, Worksheet.Dimension.Rows)) {
                            var id = Worksheet.Cells[rowIndex, IdColumnIndex].GetValue<string>();
                            if (id == null || id == "") continue;
                            idIndexes.Add(new IdIndex(id, rowIndex));
                        }
                    }
                    var toDeleteIndexes = idIndexes.Where(idIndex => !allIds.Contains(idIndex.Id)).Select(idIndex => idIndex.Index).ToList();
                    toDeleteIndexes.Sort();
                    toDeleteIndexes.Reverse();
                    foreach (var rowIndex in toDeleteIndexes) {
                        Worksheet.DeleteRow(rowIndex);
                    }
                }
            }

            Dictionary<string, List<string>> GetReversedIdGroups(List<long> existIds, List<long> restIds) {
                // 逆順にする理由:
                //   - 行挿入時逆順に挿入するとインデックスの移動を気にしなくとも良くなる
                //   - 境界判定の最終比較値が0になるので扱いやすい
                //   - 基本的にID末尾への追加が多いと想定されるので走査が少なくてすむ可能性が高い
                existIds.Sort();
                existIds.Reverse();
                existIds.Add(0);
                restIds.Sort();
                restIds.Reverse();

                var restIdGroups = new Dictionary<string, List<string>>();
                List<string> currentRestIdGroup = null;
                var existIdsIndex = 0;
                var baseId = existIds[existIdsIndex];
                var initialValue = true;
                foreach(var id in restIds) {
                    if (id < baseId) {
                        do baseId = existIds[++existIdsIndex];
                        while (id < baseId);
                        initialValue = true;
                    }
                    if (initialValue) {
                        currentRestIdGroup = new List<string>();
                        restIdGroups.Add(baseId.ToString(), currentRestIdGroup);
                        initialValue = false;
                    }
                    currentRestIdGroup.Add(id.ToString());
                }
                return restIdGroups;
            }

            protected struct IdIndex {
                public string Id;
                public int Index;
                public IdIndex(string id, int index) {
                    Id = id;
                    Index = index;
                }
            }
        }

        class SeedTableColumn {
            public string Name { get; }
            public int Index { get; }
            public SeedTableColumn(string name, int index) {
                Name = name;
                Index = index;
            }
        }
    }
}
