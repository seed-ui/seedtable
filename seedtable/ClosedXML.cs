using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;

namespace SeedTable {
    namespace ClosedXML {
        class ExcelData : IExcelData {
            XLWorkbook Workbook;

            public static ExcelData FromFile(string file) => new ExcelData(new XLWorkbook(file));

            public ExcelData(XLWorkbook workbook) {
                Workbook = workbook;
            }

            public IEnumerable<string> SheetNames {
                get { return Workbook.Worksheets.Select(worksheet => worksheet.Name); }
            }

            public SeedTableBase GetSeedTable(string sheetName, int columnNamesRowIndex = 2, int dataStartRowIndex = 3, IEnumerable<string> ignoreColumnNames = null, string versionColumnName = null) {
                IXLWorksheet worksheet;
                try {
                    worksheet = Workbook.Worksheet(sheetName);
                } catch (Exception exception) {
                    throw new SheetNotFoundException($"sheet [{sheetName}] not found", exception);
                }
                return new SeedTable(worksheet, columnNamesRowIndex, dataStartRowIndex, ignoreColumnNames, versionColumnName);
            }

            public void Save() => Workbook.Save();

            public void SaveAs(string file) => Workbook.SaveAs(file);

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
                if (isDisposing) Workbook.Dispose();
                disposed = true;
            }
        }

        class SeedTable : SeedTableBase {
            public IXLWorksheet Worksheet { get; private set; }

            public List<SeedTableColumn> Columns { get; } = new List<SeedTableColumn>();
            public int VersionColumnIndex { get; private set; }
            public int IdColumnIndex { get; private set; }

            public SeedTable(IXLWorksheet worksheet, int columnNamesRowIndex = 2, int dataStartRowIndex = 3, IEnumerable<string> ignoreColumnNames = null, string versionColumnName = null) : base(columnNamesRowIndex, dataStartRowIndex, ignoreColumnNames, versionColumnName) {
                Worksheet = worksheet;

                GetColumns();
            }

            void GetColumns() {
                foreach (var cell in Worksheet.Row(ColumnNamesRowIndex).Cells()) {
                    var value = cell.GetValue<string>();
                    if (value == null || value.Length == 0) continue;
                    if (IgnoreColumnNames.Contains(value)) continue;
                    if (VersionColumnName == value) {
                        VersionColumnIndex = cell.Address.ColumnNumber;
                    } else {
                        var columnIndex = cell.Address.ColumnNumber;
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

            public override void DataToExcel(DataDictionaryList data, bool delete = false) {
                var indexedData = data.IndexById();
                var ids = new HashSet<string>(indexedData.Keys);
                var restIds = new HashSet<string>(indexedData.Keys);
                Worksheet.Rows().Skip(DataStartRowIndex - 1).ForEach(row => {
                    var id = row.Cell(IdColumnIndex).GetValue<string>();
                    if (ids.Contains(id)) {
                        var rowData = indexedData[id];
                        Columns.ForEach(column => {
                            var cell = row.Cell(column.Index);
                            var value = rowData[column.Name];
                            if (!cell.HasFormula) cell.SetValue<string>(value != null ? Convert.ToString(value) : "");
                        });
                        restIds.Remove(id);
                    }
                });
            }

            public override DataDictionaryList ExcelToData(string requireVersion = "") {
                var table = Worksheet.Rows().Skip(DataStartRowIndex - 1).Select(row => this.GetRowValuesDictionary(row));
                return new DataDictionaryList(table);
            }

            Dictionary<string, object> GetRowValuesDictionary(IXLRow row) => Columns.ToDictionary(column => column.Name, column => (row.Cell(column.Index).Value));
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
