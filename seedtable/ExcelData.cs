using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SeedTable {
    namespace OpenXml {
        class SeedTableCell {
            public Cell Cell { get; }
            SharedStringTableWithIndex SharedStringTable { get; }

            public SeedTableCell(Cell cell, SharedStringTableWithIndex sharedStringTable) {
                Cell = cell;
                SharedStringTable = sharedStringTable;
            }

            static readonly Regex ColumnPlaceJunkRegex = new Regex(@"\d+", RegexOptions.Compiled);

            static readonly Regex RowPlaceJunkRegex = new Regex(@"\D+", RegexOptions.Compiled);

            public string Reference { get { return Cell.CellReference.Value; } }

            public string Column() => Column(Cell);

            public int Row() => Row(Cell);

            public object Value() => Value(Cell, SharedStringTable);

            public string ValueString() => ValueString(Cell, SharedStringTable);

            public void SetValue(string text) => SetValue(Cell, text, SharedStringTable);

            public static string Column(Cell cell) => ColumnPlaceJunkRegex.Replace(cell.CellReference.Value, "", 1);

            public static int Row(Cell cell) => Convert.ToInt32(RowPlaceJunkRegex.Replace(cell.CellReference.Value, "", 1));

            static readonly HashSet<string> TextLocalnames = new HashSet<string> { "r", "t" };

            public static string ValueString(Cell cell, SharedStringTableWithIndex sharedStringTable) {
                if (cell == null) return null;
                // 数式
                if (cell.CellFormula != null) {
                    //Console.WriteLine("FORMULA: "+cell.CellFormula.Text+" -> "+ cell.CellValue.Text);
                    return cell.CellValue.Text;
                }
                // SharedString以外
                if (cell.DataType == null || cell.DataType != CellValues.SharedString || sharedStringTable == null) {
                    return cell.InnerText;
                }
                // SharedString
                return sharedStringTable.SharedString(int.Parse(cell.InnerText));
            }

            public static object Value(Cell cell, SharedStringTableWithIndex sharedStringTable) {
                if (cell == null || cell.CellValue == null) {
                    return null;
                } else if (cell.DataType == null) {
                    return cell.CellValue.InnerText;
                } else {
                    string text = cell.CellFormula == null ? cell.CellValue.InnerText : cell.CellValue.Text;
                    switch (cell.DataType.Value) {
                        case CellValues.InlineString:
                            return text;
                        case CellValues.SharedString:
                            return sharedStringTable.SharedString(int.Parse(text));
                        case CellValues.String:
                            return text;
                        case CellValues.Boolean:
                            return text.Trim() == "0";
                        case CellValues.Date:
                            return DateTime.Parse(text);
                        case CellValues.Error:
                            return new Exception(text);
                        case CellValues.Number:
                            return double.Parse(text);
                        default:
                            return null;
                    }
                }
            }

            public static void SetValue(Cell cell, string text, SharedStringTableWithIndex sharedStringTable) {
                var index = sharedStringTable.SetSharedString(text == null ? "" : text);
                cell.CellValue = new CellValue(index.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            }

            public static int ColumnStrToIndex(string column) {
                column = column.ToUpper();
                long columnIndex = 0;
                foreach (char c in column) {
                    columnIndex = columnIndex * 26 + (c - 'A' + 1);
                }
                return (int)columnIndex;
            }

            public static string ColumnIndexToStr(int columnIndex) {
                var columnChars = new List<int>();
                while (columnIndex != 0) {
                    columnChars.Add(columnIndex % 26);
                    columnIndex /= 26;
                }
                return string.Concat(columnChars.Reverse<int>().Select(columnChar => Convert.ToChar(columnChar + 'A' - 1)));
            }
        }

        class SeedTableRow {
            public Row Row { get; }
            SharedStringTableWithIndex SharedStringTable { get; }

            public SeedTableRow(Row row, SharedStringTableWithIndex sharedStringTable) {
                Row = row;
                SharedStringTable = sharedStringTable;
            }

            public UInt32Value RowIndex { get { return Row.RowIndex; } }

            public IEnumerable<SeedTableCell> Cells() => Row.Select(cell => new SeedTableCell((Cell)cell, SharedStringTable));

            public IEnumerable<SeedTableCell> Cells(IEnumerable<string> useCols) {
                var rowIndexStr = RowIndex.ToString();
                var cells = Cells().ToDictionary(cell => cell.Reference.Replace(rowIndexStr, ""), cell => cell);
                return useCols.Select(col => cells.ContainsKey(col) ? cells[col] : null);
            }
        }

        class SeedTableColumn {
            public string Name { get; }
            public string Index { get; }
            public SeedTableColumn(string name, string index) {
                Name = name;
                Index = index;
            }
        }

        class SeedTable : SeedTableBase {
            public override string SheetName { get; }
            Sheet Sheet { get; }
            Worksheet Worksheet { get; }
            SharedStringTableWithIndex SharedStringTable { get; }

            public List<SeedTableColumn> Columns { get; } = new List<SeedTableColumn>();
            public string VersionColumnIndex { get; private set; }
            public string IdColumnIndex { get; private set; }

            Dictionary<string, object> DefaultRowData;

            public SeedTable(string sheetName, Sheet sheet, Worksheet worksheet, SharedStringTableWithIndex sharedStringTable, int columnNamesRowIndex = 2, int dataStartRowIndex = 3, IEnumerable<string> ignoreColumnNames = null, string versionColumnName = null) : base(columnNamesRowIndex, dataStartRowIndex, ignoreColumnNames, versionColumnName) {
                SheetName = sheetName;
                Sheet = sheet;
                Worksheet = worksheet;
                SharedStringTable = sharedStringTable;

                GetColumns();
            }

            void GetColumns() {
                var columnNameCells = ColumnNamesRow().Cells();
                foreach(var cell in columnNameCells) {
                    var value = cell.ValueString();
                    if (IgnoreColumnNames.Contains(value)) continue;
                    if (VersionColumnName == value) {
                        VersionColumnIndex = cell.Column();
                    } else {
                        var columnIndex = cell.Column();
                        if ("id" == value) IdColumnIndex = columnIndex;
                        Columns.Add(new SeedTableColumn(value, columnIndex));
                    }
                }
                CheckColumns();
                if (Errors.Count == 0) DefaultRowData = Columns.ToDictionary(column => column.Name, column => (object)null);
            }

            void CheckColumns() {
                if (IdColumnIndex == null) Errors.Add(new NoIdColumnException($"id column not found [{SheetName}]"));
                var columnNames = new HashSet<string>();
                foreach(var column in Columns) {
                    var columnName = column.Name;
                    if (columnNames.Contains(columnName)) Errors.Add(new DuplicateColumnNameException($"Duplicate column name [{columnName}] found in sheet [{SheetName}]"));
                    columnNames.Add(columnName);
                }
            }

            public IEnumerable<string> ColumnNames() => Columns.Select(column => column.Name);

            public IEnumerable<string> ColumnIndexes() => Columns.Select(column => column.Index);

            public IEnumerable<SeedTableRow> Rows() => Worksheet.Descendants<Row>().Select(row => new SeedTableRow(row, SharedStringTable));

            public SeedTableRow ColumnNamesRow() => new SeedTableRow(Worksheet.Descendants<Row>().First(row => row.RowIndex == this.ColumnNamesRowIndex), SharedStringTable);

            public override DataDictionaryList ExcelToData(string requireVersion = "") {
                var rows = Worksheet.Descendants<Row>();
                var table = rows
                    .Where(row => row.RowIndex >= DataStartRowIndex)
                    .Select(row => GetCellValuesDictionary(row));
                return new DataDictionaryList(table);
            }

            Dictionary<string, object> GetCellValuesDictionary(Row row) {
                var columnIndexToName = ColumnIndexToName(row.RowIndex.ToString());
                var restColumnCount = Columns.Count;
                var cellValues = new Dictionary<string, object>(DefaultRowData);
                foreach (Cell cell in row) {
                    string columnName;
                    if (columnIndexToName.TryGetValue(cell.CellReference.Value, out columnName)) {
                        --restColumnCount;
                        cellValues[columnName] = SeedTableCell.Value(cell, SharedStringTable);
                        // カラム数分たまったら読み出しを打ち切る
                        if (restColumnCount == 0) break;
                    }
                }
                return cellValues;
            }

            Dictionary<string, string> ColumnIndexToName(string rowIndexStr) {
                // {"A10" => "id", "B10" => "name", "D10" => "description"} のような連想配列を作る
                return Columns.ToDictionary(column => column.Index + rowIndexStr, column => column.Name);
            }

            public override void DataToExcel(DataDictionaryList data, bool delete = false) {
                var indexedData = data.IndexById();
                var restIds = new HashSet<string>(indexedData.Keys);
                var indexedRows = new Dictionary<string, Row>();
                var rows = Worksheet.Descendants<Row>();
                foreach (var row in rows.Where(row => row.RowIndex >= ColumnNamesRowIndex + 1)) {
                    var idCellReference = IdColumnIndex + row.RowIndex.ToString();
                    var idCell = (Cell)row.First(cell => ((Cell)cell).CellReference.Value == idCellReference);
                    var id = SeedTableCell.ValueString(idCell, SharedStringTable);
                    indexedRows[id] = row;
                    Dictionary<string, object> rowData;
                    if (indexedData.TryGetValue(id, out rowData)) {
                        restIds.Remove(id);
                        var columnIndexToName = ColumnIndexToName(row.RowIndex.ToString());
                        foreach (Cell cell in row) {
                            // 数式セルは飛ばす
                            if (cell.CellFormula != null) continue;
                            string columnName;
                            if (columnIndexToName.TryGetValue(cell.CellReference.Value, out columnName)) {
                                object value;
                                if (rowData.TryGetValue(columnName, out value)) {
                                    SeedTableCell.SetValue(cell, Convert.ToString(value), SharedStringTable);
                                }
                            }
                        }
                    }
                }
            }
        }

        class SharedStringTableWithIndex {
            public SharedStringTable SharedStringTable { get; }
            WorkbookPart WorkbookPart { get; }

            private Dictionary<string, uint> Index = new Dictionary<string, uint>();

            public SharedStringTableWithIndex(SharedStringTable sharedStringTable, WorkbookPart workbookPart) {
                SharedStringTable = sharedStringTable;
                WorkbookPart = workbookPart;

                MakeIndex();
            }

            void MakeIndex() {
                uint index = 0;
                foreach (var item in SharedStringTable) {
                    Index[item.InnerText] = index;
                    ++index;
                }
            }

            static readonly HashSet<string> TextLocalnames = new HashSet<string> { "r", "t" };

            public string SharedString(int index) {
                var element = SharedStringTable.ElementAt(index);
                return element.Aggregate("", (str, child) => str + (TextLocalnames.Contains(child.LocalName) ? child.InnerText : ""));
            }

            public uint SetSharedString(string text) {
                uint index;
                if (TryGetSharedStringIndex(text, out index)) return index;
                SharedStringTable.AppendChild(new SharedStringItem(new Text(text)));
                SharedStringTable.Save();
                index = SharedStringTable.Count.Value - 1;
                Index[text] = index;
                return index;
            }

            bool TryGetSharedStringIndex(string text, out uint index) {
                return Index.TryGetValue(text, out index);
            }

            // TODO:Narazaka: clean unused strings
            public void CleanSharedStringTable() {
                var unusedIds = new HashSet<int>(Enumerable.Range(0, (int)SharedStringTable.Count.Value));
                foreach(var worksheet in WorkbookPart.GetPartsOfType<WorksheetPart>().Select(part => part.Worksheet)) {
                    foreach (var cell in worksheet.GetFirstChild<SheetData>().Descendants<Cell>()) {
                        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString) {
                        }
                    }

                }
            }
        }

        class ExcelData : IExcelData {
            SpreadsheetDocument Document { get; }
            IEnumerable<Sheet> Sheets { get; }
            //! シート中の文字列データが一括して格納されているテーブル
            SharedStringTableWithIndex SharedStringTable { get; }

            public static ExcelData FromFile(string file, bool isEditable) => new ExcelData(SpreadsheetDocument.Open(file, isEditable));

            public ExcelData(SpreadsheetDocument document) {
                Document = document;
                Sheets = WorkbookPart.Workbook.Descendants<Sheet>();
                SharedStringTable = new SharedStringTableWithIndex(WorkbookPart.SharedStringTablePart.SharedStringTable, WorkbookPart);
            }

            WorkbookPart WorkbookPart { get { return this.Document.WorkbookPart; } }

            public IEnumerable<string> SheetNames { get { return this.Sheets.Select(sheet => sheet.Name.Value); } }

            Sheet Sheet(string sheetName) {
                try {
                    return this.Sheets.First(sheet => sheet.Name == sheetName);
                } catch (InvalidOperationException exception) {
                    throw new SheetNotFoundException($"sheet [{sheetName}] not found", exception);
                }
            }

            Worksheet Worksheet(string sheetName) => ((WorksheetPart)WorkbookPart.GetPartById(Sheet(sheetName).Id)).Worksheet;

            public SeedTableBase GetSeedTable(string sheetName, int columnNamesRowIndex = 2, int dataStartRowIndex = 3, IEnumerable<string> ignoreColumnNames = null, string versionColumnName = null) {
                return new SeedTable(sheetName, Sheet(sheetName), Worksheet(sheetName), SharedStringTable, columnNamesRowIndex, dataStartRowIndex, ignoreColumnNames, versionColumnName);
            }

            public void Save() {
                Document.WorkbookPart.Workbook.Save();
            }

            public void SaveAs(string file) {
                using (var newDocument = SpreadsheetDocument.Create(file, Document.DocumentType)) {
                    // 中身を消す
                    newDocument.DeleteParts(newDocument.GetPartsOfType<OpenXmlPart>());
                    // 元ファイルから全情報をコピー
                    foreach (var part in Document.GetPartsOfType<OpenXmlPart>()) {
                        newDocument.AddPart(part);
                    }
                    newDocument.WorkbookPart.Workbook.Save();
                }
            }

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
    }
}
