using System;
using System.Text.RegularExpressions;

namespace SeedTable {
    class SheetNameOnFileName {
        public static SheetNameOnFileName FromMixed(string mixedName) {
            var separated = mixedName.Split('/');
            return new SheetNameOnFileName(separated[0], separated[1]);
        }

        public Wildcard FileName { get; } = null;
        public Wildcard SheetName { get; }

        public SheetNameOnFileName(string fileName, string sheetName) {
            FileName = new Wildcard(fileName);
            SheetName = new Wildcard(sheetName);
        }
    }
}
