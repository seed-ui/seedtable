using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace seedtable {
    class ExcelData {
        private SpreadsheetDocument document;
        private WorkbookPart workbook_part;
        //! シート中の文字列データが一括して格納されているテーブル
        private SharedStringTable string_table;
        //! シート
        private IEnumerable<Sheet> sheets;
        public int column_row { get; private set; }

        public ExcelData(SpreadsheetDocument document, int column_row = 2) {
            this.document = document;
            this.column_row = column_row;
            this.workbook_part = document.WorkbookPart;
            this.string_table = this.workbook_part.SharedStringTablePart.SharedStringTable;
            this.sheets = workbook_part.Workbook.Descendants<Sheet>();
        }

        public IEnumerable<string> SheetNames {
            get { return this.sheets.Select(sheet => sheet.Name.Value); }
        }

        public DataDictionaryList ExcelToData(string sheet_name) {
            var worksheet = this.GetWorksheet(sheet_name);
            var rows = worksheet.Descendants<Row>();
            var columns_row = this.GetHeaderRow(rows);

            var column_names = this.GetCellValues(columns_row);
            var column_places = this.GetCellColumnPlaces(columns_row).ToList();
            var use_cols = this.GetUseColumnPlaces(column_names, column_places);
            var columns = this.GetCellValues(columns_row, use_cols).ToList();
            var table = rows.Where(row => row.RowIndex >= this.column_row + 1).Select(row => this.GetCellValuesDictionary(row, use_cols, columns));
            return new DataDictionaryList(table);
        }

        private Row GetHeaderRow(IEnumerable<Row> rows) {
            return rows.First(row => row.RowIndex == this.column_row);
        }

        private IEnumerable<string> GetUseColumnPlaces(IEnumerable<string> column_names, List<string> column_places) {
            return column_names.Select((col, index) => new { col, index }).Where(elem => elem.col != "dummy" && elem.col != "").Select(elem => column_places[elem.index]);
        }

        private Sheet GetSheet(string sheet_name) {
            try {
                return this.sheets.First(_sheet => _sheet.Name == sheet_name);
            } catch (Exception exception) {
                throw new InvalidOperationException(string.Format("シート[{0}]が見つかりません", sheet_name), exception);
            }
        }

        private Worksheet GetWorksheet(string sheet_name) {
            var sheet = this.GetSheet(sheet_name);
            var worksheet_part = (WorksheetPart)workbook_part.GetPartById(sheet.Id);
            return worksheet_part.Worksheet;
        }

        private static readonly HashSet<string> text_localnames = new HashSet<string> { "r", "t" };

        private string GetCellValue(Cell cell) {
            if (cell == null) return null;
            // 数式
            if (cell.CellFormula != null) {
                //Console.WriteLine("FORMULA: "+cell.CellFormula.Text+" -> "+ cell.CellValue.Text);
                return cell.CellValue.Text;
            }
            // SharedString以外
            if (cell.DataType == null || cell.DataType != CellValues.SharedString || this.string_table == null) {
                return cell.InnerText;
            }
            // SharedString
            var element = string_table.ElementAt(int.Parse(cell.InnerText));
            return element.Aggregate("", (str, child) => str + (text_localnames.Contains(child.LocalName) ? child.InnerText : ""));
        }

        private IEnumerable<string> GetCellPlaces(Row row) {
            return row.Select(cell => ((Cell)cell).CellReference.Value);
        }

        private static readonly Regex column_place_junk_re = new Regex(@"\d+");

        private IEnumerable<string> GetCellColumnPlaces(Row row) {
            return this.GetCellPlaces(row).Select(value => column_place_junk_re.Replace(value, ""));
        }

        private IEnumerable<string> GetCellValues(Row row) {
            return row.Select(cell => this.GetCellValue((Cell)cell));
        }

        private IEnumerable<string> GetCellValues(Row row, IEnumerable<string> use_cols) {
            var cells = row.Select(cell => (Cell)cell)
                .Select(cell => new { cell, col = cell.CellReference.Value.Replace(row.RowIndex.ToString(), "") })
                .ToDictionary(cell => cell.col, cell => cell.cell);
            return use_cols.Select(col => cells.ContainsKey(col) ? cells[col] : null).Select(cell => this.GetCellValue(cell));
        }

        private Dictionary<string, string> GetCellValuesDictionary(Row row, IEnumerable<string> use_cols, List<string> columns) {
            return this.GetCellValues(row, use_cols)
                .Select((value, index) => new { value, index })
                .ToDictionary(elem => columns[elem.index], elem => elem.value);
        }
    }
}