using System.Collections.Generic;
using System.Linq;

namespace SeedTable {
    class SheetNameOnFileNames : List<SheetNameOnFileName> {
        public static SheetNameOnFileNames FromMixed(IEnumerable<string> mixedNames = null) {
            return
                mixedNames == null ?
                new SheetNameOnFileNames() :
                new SheetNameOnFileNames(mixedNames.Select(mixedName => SheetNameOnFileName.FromMixed(mixedName)));
        }

        public SheetNameOnFileNames() : base() { }

        public SheetNameOnFileNames(IEnumerable<SheetNameOnFileName> sheetNameOnFileName) : base(sheetNameOnFileName) { }

        // file/sheet指定が存在して完全マッチ、あるいは指定が存在しない場合true
        public bool IsUseSheet(string fileName, string sheetName) {
            var sheetNameOnFileName = this.FirstOrDefault(_sheetNameOnFileName => _sheetNameOnFileName.SheetName.IsMatch(sheetName));
            if (sheetNameOnFileName == null) return true; // 指定がない
            if (sheetNameOnFileName.FileName.IsMatch(fileName)) return true; // 完全マッチ
            return false;
        }
    }
}
