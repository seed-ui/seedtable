using System;
using System.Text.RegularExpressions;

namespace SeedTable {
    public class SheetNameMap {
        public static SheetNameMap FromMixed(string mixedName) {
            var result = Regex.Match(mixedName, @"^([^:/]+):(?:([^:/]+)/)?([^:/]+)$");
            if (!result.Success) throw new Exception($"{mixedName} is wrong mapping rule definition");
            var yamlTableName = result.Groups[1].Value;
            var fileName = result.Groups[2].Value;
            if (fileName.Length == 0) fileName = "*";
            var sheetName = result.Groups[3].Value;
            return new SheetNameMap(yamlTableName, fileName, sheetName);
        }

        public string YamlTableName { get; }
        public Wildcard FileName { get; } = null;
        public string SheetName { get; }

        public SheetNameMap(
            string yamlTableName,
            string fileName,
            string sheetName
        ) {
            YamlTableName = yamlTableName;
            FileName = new Wildcard(fileName);
            SheetName = sheetName;
        }

        public bool IsMatch(string fileName, string sheetName) {
            return FileName.IsMatch(fileName) && SheetName == sheetName;
        }
    }
}
