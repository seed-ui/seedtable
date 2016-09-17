using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SeedTable {
    class ExcelData {
        private SpreadsheetDocument Document { get; }
        private IEnumerable<Sheet> Sheets { get; }
        public int ColumnRow { get; }

        public ExcelData(SpreadsheetDocument document, int columnRow = 2) {
            this.Document = document;
            this.ColumnRow = columnRow;
            this.Sheets = this.WorkbookPart.Workbook.Descendants<Sheet>();
        }

        private WorkbookPart WorkbookPart { get { return this.Document.WorkbookPart; } }
        //! シート中の文字列データが一括して格納されているテーブル
        private SharedStringTable SharedStringTable { get { return this.WorkbookPart.SharedStringTablePart.SharedStringTable; } }

        public IEnumerable<string> SheetNames {
            get { return this.Sheets.Select(sheet => sheet.Name.Value); }
        }

        public DataDictionaryList ExcelToData(string sheetName) {
            var worksheet = this.Worksheet(sheetName);
            var rows = worksheet.Descendants<Row>();
            var columnsRow = this.GetHeaderRow(rows);

            var columnNames = this.GetCellValues(columnsRow);
            var columnPlaces = this.GetCellColumnPlaces(columnsRow).ToList();
            var useCols = this.GetUseColumnPlaces(columnNames, columnPlaces);
            var columns = this.GetCellValues(columnsRow, useCols).ToList();
            var table = rows.Where(row => row.RowIndex >= this.ColumnRow + 1).Select(row => this.GetCellValuesDictionary(row, useCols, columns));
            return new DataDictionaryList(table);
        }

        private Row GetHeaderRow(IEnumerable<Row> rows) => rows.First(row => row.RowIndex == this.ColumnRow);

        private IEnumerable<string> GetUseColumnPlaces(IEnumerable<string> columnNames, List<string> columnPlaces) {
            return columnNames.Select((col, index) => new { col, index }).Where(elem => elem.col != "dummy" && elem.col != "").Select(elem => columnPlaces[elem.index]);
        }

        private Sheet Sheet(string sheetName) {
            try {
                return this.Sheets.First(sheet => sheet.Name == sheetName);
            } catch (Exception exception) {
                throw new InvalidOperationException(string.Format("シート[{0}]が見つかりません", sheetName), exception);
            }
        }

        private Worksheet Worksheet(string sheetName) => ((WorksheetPart)WorkbookPart.GetPartById(Sheet(sheetName).Id)).Worksheet;

        private static readonly HashSet<string> textLocalnames = new HashSet<string> { "r", "t" };

        private string GetCellValue(Cell cell) {
            if (cell == null) return null;
            // 数式
            if (cell.CellFormula != null) {
                //Console.WriteLine("FORMULA: "+cell.CellFormula.Text+" -> "+ cell.CellValue.Text);
                return cell.CellValue.Text;
            }
            // SharedString以外
            if (cell.DataType == null || cell.DataType != CellValues.SharedString || this.SharedStringTable == null) {
                return cell.InnerText;
            }
            // SharedString
            var element = SharedStringTable.ElementAt(int.Parse(cell.InnerText));
            return element.Aggregate("", (str, child) => str + (textLocalnames.Contains(child.LocalName) ? child.InnerText : ""));
        }

        private IEnumerable<string> GetCellPlaces(Row row) => row.Select(cell => ((Cell)cell).CellReference.Value);

        private static readonly Regex columnPlaceJunkRe = new Regex(@"\d+");

        private IEnumerable<string> GetCellColumnPlaces(Row row) {
            return this.GetCellPlaces(row).Select(value => columnPlaceJunkRe.Replace(value, ""));
        }

        private IEnumerable<string> GetCellValues(Row row) {
            return row.Select(cell => this.GetCellValue((Cell)cell));
        }

        private IEnumerable<string> GetCellValues(Row row, IEnumerable<string> useCols) {
            var cells = row.Select(cell => (Cell)cell)
                .Select(cell => new { cell, col = cell.CellReference.Value.Replace(row.RowIndex.ToString(), "") })
                .ToDictionary(cell => cell.col, cell => cell.cell);
            return useCols.Select(col => cells.ContainsKey(col) ? cells[col] : null).Select(cell => this.GetCellValue(cell));
        }

        private Dictionary<string, string> GetCellValuesDictionary(Row row, IEnumerable<string> useCols, List<string> columns) {
            return this.GetCellValues(row, useCols)
                .Select((value, index) => new { value, index })
                .ToDictionary(elem => columns[elem.index], elem => elem.value);
        }
    }
}