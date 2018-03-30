using System.Collections.Generic;
using System.Linq;

namespace SeedTable {
    public class SheetNameMaps : List<SheetNameMap> {
        public static SheetNameMaps FromMixed(IEnumerable<string> mixedNames = null) {
            return
                mixedNames == null ?
                new SheetNameMaps() :
                new SheetNameMaps(mixedNames.Select(mixedName => SheetNameMap.FromMixed(mixedName)));
        }

        public SheetNameMaps() : base() { }

        public SheetNameMaps(IEnumerable<SheetNameMap> sheetNameMaps) : base(
            sheetNameMaps.OrderByDescending(sheetNameMap => (int)sheetNameMap.FileName.MatchType) // ワイルドカードの判定優先順位を低くする
        ) { }

        // file/sheet指定が存在して完全マッチ、あるいは指定が存在しない場合true
        public SheetNameMap Find(string fileName, string sheetName) {
            return Find(sheetNameMap => sheetNameMap.IsMatch(fileName, sheetName));
        }

        public bool Contains(string fileName, string sheetName) {
            return Find(fileName, sheetName) != null;
        }
    }
}
