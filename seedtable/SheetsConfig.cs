using System.Collections.Generic;
using System.Linq;

namespace SeedTable {
    class SheetsConfig {
        public SheetsConfig(
            IEnumerable<string> only,
            IEnumerable<string> ignore,
            IEnumerable<string> subdivide = null,
            IEnumerable<string> primary = null,
            IEnumerable<string> mapping = null,
            IEnumerable<string> alias = null
        ) {
            var subdivideSheetNames = SheetNameWithSubdivides.FromMixed(subdivide);
            OnlySheetNames = SheetNameWithSubdivides.FromMixed(only);
            IgnoreSheetNames = SheetNameWithSubdivides.FromMixed(ignore);
            SubdivideRules = new SheetNameWithSubdivides(subdivideSheetNames.Concat(OnlySheetNames));
            PrimarySheetNames = SheetNameOnFileNames.FromMixed(primary);
            excelToYamlMapping = SheetNameMaps.FromMixed(mapping);
            excelToYamlAlias = SheetNameMaps.FromMixed(alias);
        }

        SheetNameWithSubdivides SubdivideRules;
        SheetNameWithSubdivides IgnoreSheetNames;
        SheetNameWithSubdivides OnlySheetNames;
        SheetNameOnFileNames PrimarySheetNames;
        SheetNameMaps excelToYamlMapping;
        SheetNameMaps excelToYamlAlias;

        // onOperationはFrom | Toだと正しく動作しない
        public bool IsUseSheet(string fileName, string excelSheetName, string yamlTableName, OnOperation onOperation) {
            if (IgnoreSheetNames.Contains(fileName, yamlTableName, onOperation)) return false;
            if (OnlySheetNames.Count != 0 && !OnlySheetNames.Contains(fileName, yamlTableName, onOperation)) return false;
            if (onOperation.HasFlag(OnOperation.From)) {
                // TODO: primaryでない的なnoticeを出したほうが良い
                if (!PrimarySheetNames.IsUseSheet(fileName, yamlTableName)) return false;
                // エイリアス設定先のシートはfrom時変換されない
                if (excelToYamlAlias.Contains(fileName, excelSheetName)) return false;
            }
            return true;
        }

        public SheetNameWithSubdivide subdivide(string fileName, string sheetName, OnOperation onOperation) {
            var subdivideRule = SubdivideRules.Find(fileName, sheetName, onOperation);
            return subdivideRule ?? new SheetNameWithSubdivide(fileName, sheetName);
        }

        public string YamlTableName(string fileName, string excelSheetName) {
            var sheetNameMap = excelToYamlMapping.Find(fileName, excelSheetName) ?? excelToYamlAlias.Find(fileName, excelSheetName);
            return sheetNameMap == null ? excelSheetName : sheetNameMap.YamlTableName;
        }
    }
}
