using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;

namespace SeedTable {
    class ClosedExcelData {
        private XLWorkbook workbook;

        public ClosedExcelData(XLWorkbook workbook) {
            this.workbook = workbook;
        }

        public IEnumerable<string> SheetNames {
            get { return this.workbook.Worksheets.Select(worksheet => worksheet.Name); }
        }

        private IXLWorksheet WorkSheet(string sheet_name) {
            try {
                return this.workbook.Worksheets.First(_sheet => _sheet.Name == sheet_name);
            } catch (Exception exception) {
                throw new InvalidOperationException(string.Format("シート[{0}]が見つかりません", sheet_name), exception);
            }
        }

        public ClosedSeedTable GetSeedTable(string sheet_name, int column_row = 2) {
            return new ClosedSeedTable(this.WorkSheet(sheet_name), column_row);
        }
    }

    class ClosedSeedTable {
        public IXLWorksheet worksheet { get; private set; }
        public int column_row { get; private set; }
        public ClosedSeedTable(IXLWorksheet worksheet, int column_row = 2) {
            this.worksheet = worksheet;
            this.column_row = column_row;
        }

        public string SheetName {
            get { return this.worksheet.Name; }
        }

        public IXLCells ColumnCells {
            get { return this._ColumnCells = this._ColumnCells ?? worksheet.Row(this.column_row).Cells(); }
        }
        private IXLCells _ColumnCells;

        public Dictionary<string, int> Columns {
            get {
                return this._Columns = this._Columns ?? this.ColumnCells
                    .Where(cell => cell.GetValue<string>() != "")
                    .Where(cell => cell.GetValue<string>() != "dummy")
                    .ToDictionary(cell => cell.GetValue<string>(), cell => cell.Address.ColumnNumber);
            }
        }
        private Dictionary<string, int> _Columns;

        public IEnumerable<string> ColumnValues {
            get {
                return this.ColumnCells.Select(cell => cell.GetValue<string>());
            }
        }

        public int IDColumn {
            get { return this.Columns.First(column => column.Key == "id").Value; }
        }

        public int ColumnIndex(string column_name) {
            return this.Columns[column_name];
        }

        public void DataToExcel(DataDictionaryList data, bool delete = false) {

            if (this.ColumnCells.First().GetValue<string>() != "id") {
                throw new NotSupportedException("2行目の先頭がidでないので[" + this.SheetName + "]は扱えません");
            }

            var data_dic = data.ToDictionaryDictionary();
            var ids = new HashSet<string>(data_dic.Keys);
            Console.Error.WriteLine(string.Join("|", ids));
            var rest_ids = new HashSet<string>(data_dic.Keys);
            worksheet.Rows().Skip(this.column_row).ForEach(row => {
                var id = "data" + row.Cell(this.IDColumn).GetValue<string>();
                Console.Error.WriteLine(id + ids.Contains(id));
                if (ids.Contains(id)) {
                    var row_data = data_dic[id];
                    row_data.ForEach(col_data => {
                        var cell = row.Cell(this.Columns[col_data.Key]);
                        Console.Error.WriteLine(col_data.Key + ": " + col_data.Value);
                        if (!cell.HasFormula) cell.SetValue<string>(col_data.Value != null ? col_data.Value : "");
                    });
                    rest_ids.Remove(id);
                }
            });
            /*
            rows.Where(row => row.RowIndex >= 3).Select(row => {
                var cols = this.GetCellValuesDictionary(row, use_cols, columns);
                if (ids.Contains(cols["id"])) {

                }
                return 1;
            });
            */
        }

        public DataDictionaryList ExcelToData() {
            var table = worksheet.Rows().Skip(this.column_row)
                .Select(row => this.GetRowValuesDictionary(row));
            return new DataDictionaryList(table);
        }

        private Dictionary<string, string> GetRowValuesDictionary(IXLRow row) {
            return this.Columns.ToDictionary(column => column.Key, column => {
                //Console.WriteLine(row.RangeAddress);
                //Console.WriteLine(row.Cell(column.Value).FormulaA1);
                //Console.WriteLine(row.Cell(column.Value).Value);
                return row.Cell(column.Value).GetValue<string>();
            });
            //return this.Columns.ToDictionary(column => column.Key, column => row.Cell(column.Value).GetValue<string>());
        }
    }
}