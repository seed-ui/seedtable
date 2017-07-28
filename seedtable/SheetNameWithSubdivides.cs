using System.Collections.Generic;
using System.Linq;

namespace SeedTable {
    class SheetNameWithSubdivides : List<SheetNameWithSubdivide> {
        public static SheetNameWithSubdivides FromMixed(IEnumerable<string> mixedNames = null) {
            return mixedNames == null ?
                new SheetNameWithSubdivides() :
                new SheetNameWithSubdivides(
                    mixedNames.Select(mixedName => SheetNameWithSubdivide.FromMixed(mixedName))
                );
        }

        public SheetNameWithSubdivides() : base() { }

        public SheetNameWithSubdivides(IEnumerable<SheetNameWithSubdivide> sheetNameWithSubdivides) : base(
            sheetNameWithSubdivides.OrderBy(
                sheetNameWithSubdivide =>
                    - (int)sheetNameWithSubdivide.FileName.MatchType - 10 * (int)sheetNameWithSubdivide.SheetName.MatchType
            )
        ) { }

        public SheetNameWithSubdivide Find(string fileName, string sheetName, OnOperation onOperation) {
            return Find(sheetNameWithSubdivide => sheetNameWithSubdivide.IsMatch(fileName, sheetName, onOperation));
        }

        public bool Contains(string fileName, string sheetName, OnOperation onOperation) {
            return Find(fileName, sheetName, onOperation) != null;
        }
    }
}
