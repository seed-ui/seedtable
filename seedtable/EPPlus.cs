using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

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

            public SeedTableBase GetSeedTable(string sheetName, int columnNamesRowIndex = 2, int dataStartRowIndex = 3, IEnumerable<string> ignoreColumnNames = null, string keyColumnName = "id", string versionColumnName = null) {
                try {
                    return new SeedTable(Document, Document.Workbook.Worksheets[sheetName], columnNamesRowIndex, dataStartRowIndex, ignoreColumnNames, keyColumnName, versionColumnName);
                } catch (KeyNotFoundException exception) {
                    throw new SheetNotFoundException($"sheet [{sheetName}] not found in [{Document.File}]", exception);
                }
            }

            public void Save() => Document.Save();

            public void SaveAs(string file) => Document.SaveAs(new FileInfo(file));

            public void Calculate() => Document.Workbook.Calculate();

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
            const string FillPatternTypeKey = "$fill-pattern-type";
            const string FillBackgroundColorThemeKey = "$fill-background-color-theme";
            const string FillBackgroundColorTintKey = "$fill-background-color-tint";
            const string FillBackgroundColorRgbKey = "$fill-background-color-rgb";

            ExcelPackage Document;
            ExcelWorksheet Worksheet;
            List<Color> themeColorList = new List<Color>();

            public List<SeedTableColumn> Columns { get; } = new List<SeedTableColumn>();
            public int VersionColumnIndex { get; private set; }
            public int IdColumnIndex { get; private set; }

            public SeedTable(ExcelPackage document, ExcelWorksheet worksheet, int columnNamesRowIndex = 2, int dataStartRowIndex = 3, IEnumerable<string> ignoreColumnNames = null, string keyColumnName = "id", string versionColumnName = null) : base(columnNamesRowIndex, dataStartRowIndex, ignoreColumnNames, keyColumnName, versionColumnName) {
                Document = document;
                Worksheet = worksheet;
                GetColumns();
            }

            void GetColumns() {
                foreach(var columnIndex in Enumerable.Range(1, Worksheet.Dimension.Columns + 1)) {
                    var cell = Worksheet.Cells[ColumnNamesRowIndex, columnIndex];
                    var value = cell.GetValue<string>();
                    if (value == null || value.Length == 0) continue;
                    if (IgnoreColumnNames.Contains(value)) continue;
                    if (VersionColumnName == value) {
                        VersionColumnIndex = columnIndex;
                    } else {
                        if (KeyColumnName == value) IdColumnIndex = columnIndex;
                        Columns.Add(new SeedTableColumn(value, columnIndex));
                    }
                }
                CheckColumns();
            }

            void CheckColumns() {
                if (IdColumnIndex == 0) Errors.Add(new NoIdColumnException($"key column [{KeyColumnName}] not found [{SheetName}]"));
                var columnNames = new HashSet<string>();
                foreach(var column in Columns) {
                    var columnName = column.Name;
                    if (columnNames.Contains(columnName)) Errors.Add(new DuplicateColumnNameException($"Duplicate column name [{columnName}] found in sheet [{SheetName}]"));
                    columnNames.Add(columnName);
                }
            }

            public override string SheetName { get { return Worksheet.Name; } }

            public override DataDictionaryList ExcelToData(string requireVersion = "") {
                var table
                    = Enumerable.Range(DataStartRowIndex, Worksheet.Dimension.Rows)
                        .Select(rowIndex =>
                        {
                            var valuesDictionary = GetCellValuesDictionary(rowIndex);

                            AddRowStyleFill(rowIndex, valuesDictionary);

                            return valuesDictionary;
                        }).ToArray();

                return new DataDictionaryList(table, KeyColumnName);
            }

            void AddRowStyleFill(int rowIndex, Dictionary<string, object> valuesDictionary)
            {
                var fill = Worksheet.Row(rowIndex).Style.Fill;
                if (!string.IsNullOrEmpty(fill.BackgroundColor.Theme))
                {
                    valuesDictionary.Add(FillPatternTypeKey, fill.PatternType);
                    valuesDictionary.Add(FillBackgroundColorThemeKey, fill.BackgroundColor.Theme);
                    valuesDictionary.Add(FillBackgroundColorTintKey, fill.BackgroundColor.Tint);
                }
                else if (!string.IsNullOrEmpty(fill.BackgroundColor.Rgb))
                {
                    valuesDictionary.Add(FillPatternTypeKey, fill.PatternType);
                    valuesDictionary.Add(FillBackgroundColorRgbKey, fill.BackgroundColor.Rgb);
                }
            }

            Dictionary<string, object> GetCellValuesDictionary(int rowIndex) => Columns.ToDictionary(column => column.Name, column => Worksheet.Cells[rowIndex, column.Index].Value);

            public override void DataToExcel(DataDictionaryList data, bool delete = false) {
                SetupThemeColor();

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
                            if (rowData.TryGetValue(column.Name, out var value)) {
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

                        SetRowStyleFill(rowIndex, rowData);

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

            void SetupThemeColor() {
                var getXmlFromUri = typeof(ExcelPackage).GetMethod("GetXmlFromUri", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
                var theme1uri = new Uri("/xl/theme/theme1.xml", UriKind.Relative);
                var theme1xml = (XmlDocument)getXmlFromUri.Invoke(Document, new object[] { theme1uri });

                foreach (XmlElement element in theme1xml.GetElementsByTagName("a:sysClr")) {
                    var colorString = element.GetAttribute("lastClr");
                    var color = Color.FromArgb(int.Parse("FF" + colorString, System.Globalization.NumberStyles.AllowHexSpecifier));
                    themeColorList.Add(color);
                }

                foreach (XmlElement element in theme1xml.GetElementsByTagName("a:srgbClr")) {
                    var colorString = element.GetAttribute("val");
                    var color = Color.FromArgb(int.Parse("FF" + colorString, System.Globalization.NumberStyles.AllowHexSpecifier));
                    themeColorList.Add(color);
                }
            }

            void SetRowStyleFill(int rowIndex, IDictionary<string, object> rowData)
            {
                try {
                    var row = Worksheet.Row(rowIndex);
                    if (rowData.TryGetValue(FillPatternTypeKey, out var fillPatternType)) {
                        row.Style.Fill.PatternType = (OfficeOpenXml.Style.ExcelFillStyle)Enum.Parse(typeof(OfficeOpenXml.Style.ExcelFillStyle), fillPatternType.ToString());
                    } else {
                        // パターンタイプが無ければ色を設定できないので、処理を終了する
                        return;
                    }

                    if (rowData.TryGetValue(FillBackgroundColorThemeKey, out var backgroundColorTheme)) {
                        var baseColor = themeColorList[int.Parse(backgroundColorTheme.ToString())];
                        float tint = 0.0f;
                        if (rowData.TryGetValue(FillBackgroundColorTintKey, out var backgroundColorTint)) {
                            tint = float.Parse(backgroundColorTint.ToString());
                        }

                        var hue = baseColor.GetHue();
                        var saturation = baseColor.GetSaturation();
                        var brightness = baseColor.GetBrightness();

                        if (tint > 0) {
                            brightness = brightness * (1.0f - tint) - tint;
                        } else if (tint < 0) {
                            brightness *= (1.0f + tint);
                        }

                        var color = HlsToRgb(hue, saturation, brightness);

                        row.Style.Fill.BackgroundColor.SetColor(color);
                    } else if (rowData.TryGetValue(FillBackgroundColorRgbKey, out var backgroundColorRgb)) {
                        var color = Color.FromArgb(int.Parse(backgroundColorRgb.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier));
                        row.Style.Fill.BackgroundColor.SetColor(color);
                    }
                } catch (Exception e) {
                    Errors.Add(e);
                }
            }

            Color HlsToRgb(float hue, float saturation, float brightness) {
                float s = saturation;
                float l = brightness;

                float r1, g1, b1;
                if (s == 0) {
                    r1 = l;
                    g1 = l;
                    b1 = l;
                } else {
                    float h = hue / 60f;
                    int i = (int)Math.Floor(h);
                    float f = h - i;
                    //float c = (1f - Math.Abs(2f * l - 1f)) * s;
                    float c = (l < 0.5f) ? 2f * s * l : 2f * s * (1f - l);
                    float m = l - c / 2f;
                    float p = c + m;
                    //float x = c * (1f - Math.Abs(h % 2f - 1f));
                    float q = (i % 2 == 0) ? q = l + c * (f - 0.5f) : q = l - c * (f - 0.5f); // q = x + m

                    switch (i) {
                        case 0:
                            r1 = p;
                            g1 = q;
                            b1 = m;
                            break;
                        case 1:
                            r1 = q;
                            g1 = p;
                            b1 = m;
                            break;
                        case 2:
                            r1 = m;
                            g1 = p;
                            b1 = q;
                            break;
                        case 3:
                            r1 = m;
                            g1 = q;
                            b1 = p;
                            break;
                        case 4:
                            r1 = q;
                            g1 = m;
                            b1 = p;
                            break;
                        case 5:
                            r1 = p;
                            g1 = m;
                            b1 = q;
                            break;
                        default:
                            throw new ArgumentException(
                                "色相の値が不正です。", "hsl");
                    }
                }

                return Color.FromArgb((int)Math.Round(r1 * 255f), (int)Math.Round(g1 * 255f), (int)Math.Round(b1 * 255f));
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
