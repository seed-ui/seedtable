using System;
using System.Text.RegularExpressions;

namespace SeedTable {
    public class SheetNameWithSubdivide {
        public static SheetNameWithSubdivide FromMixed(string mixedName) {
            var result = Regex.Match(mixedName, @"^(?:(\d+):)?(?:([^:@]+)/)?([^:/@]+)(?::(\d+))?(?:@(.*))?$");
            if (!result.Success) throw new Exception($"{mixedName} is wrong sheet name and subdivide rule definition");
            var cutPrefixStr = result.Groups[1].Value;
            var fileName = result.Groups[2].Value;
            if (fileName.Length == 0) fileName = "*";
            var sheetName = result.Groups[3].Value;
            var cutPostfixStr = result.Groups[4].Value;
            var options = result.Groups[5].Value == null ? new string[] { } : result.Groups[5].Value.Split('@');
            var onOperation = OnOperation.From | OnOperation.To;
            var keyColumnName = "id";
            int? columnNamesRow = null;
            int? dataStartRow = null;
            string subdivideFilename = null;
            foreach (var option in options) {
                if (Regex.IsMatch(option, $"^(?:from|to)$", RegexOptions.IgnoreCase)) {
                    Enum.TryParse(option, true, out onOperation);
                } else if (option.StartsWith("key=")) {
                    keyColumnName = option.Substring(4);
                } else if (option.StartsWith("column-names-row=")) {
                    columnNamesRow = int.Parse(option.Substring(17));
                } else if (option.StartsWith("data-start-row=")) {
                    dataStartRow = int.Parse(option.Substring(15));
                } else if (option.StartsWith("subdivide-filename=")) {
                    subdivideFilename = option.Substring(19);
                }
            }
            var needSubdivide = cutPrefixStr.Length != 0 || cutPostfixStr.Length != 0;
            var cutPrefix = cutPrefixStr.Length == 0 ? 0 : Convert.ToInt32(cutPrefixStr);
            var cutPostfix = cutPostfixStr.Length == 0 ? 0 : Convert.ToInt32(cutPostfixStr);
            return new SheetNameWithSubdivide(fileName, sheetName, needSubdivide, cutPrefix, cutPostfix, keyColumnName, columnNamesRow, dataStartRow, subdivideFilename, onOperation);
        }

        public Wildcard FileName { get; } = null;
        public Wildcard SheetName { get; }
        public bool NeedSubdivide { get; }
        public int CutPrefix { get; }
        public int CutPostfix { get; }
        public string KeyColumnName { get; }
        public int? ColumnNamesRow { get; }
        public int? DataStartRow { get; }
        public string SubdivideFilename { get; }
        public OnOperation OnOperation { get; }

        public SheetNameWithSubdivide(
            string fileName,
            string sheetName,
            bool needSubdivide = false,
            int cutPrefix = 0,
            int cutPostfix = 0,
            string keyColumnName = "id",
            int? columnNamesRow = null,
            int? dataStartRow = null,
            string subdivideFilename = null,
            OnOperation onOperation = OnOperation.From | OnOperation.To
        ) {
            FileName = new Wildcard(fileName);
            SheetName = new Wildcard(sheetName);
            NeedSubdivide = needSubdivide;
            CutPrefix = cutPrefix;
            CutPostfix = cutPostfix;
            KeyColumnName = keyColumnName;
            ColumnNamesRow = columnNamesRow;
            DataStartRow = dataStartRow;
            SubdivideFilename = subdivideFilename;
            OnOperation = onOperation;
        }

        public bool IsMatch(string fileName, string sheetName, OnOperation onOperation) {
            return FileName.IsMatch(fileName) && SheetName.IsMatch(sheetName) && OnOperation.HasFlag(onOperation);
        }
    }
}
