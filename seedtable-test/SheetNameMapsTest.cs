using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

using SeedTable;

namespace seedtable_test {
    public class SheetNameMapsTest {
        public static IEnumerable<object[]> Examples() {
            yield return new object[] { "fooX.xlsx", "fooX", "foos" };
            yield return new object[] { "fooX.xlsx", "fooY", null };
            yield return new object[] { "bar2.xlsx", "barX", "bars" };
            yield return new object[] { "bar2.xlsx", "barY", null };
            yield return new object[] { "my.xlsx", "bazbaz", "baz" };
            yield return new object[] { "my.xlsx", "bazbaz2", null };
            yield return new object[] { "my2.xlsx", "bazbaz", "baz2" };
            yield return new object[] { "my2.xlsx", "bazbaz2", null };
        }

        public static IEnumerable<string> sheetNameMapStrs = new List<string> {
            "foos:fooX",
            "bars:barX",
            "baz:my.xlsx/bazbaz",
            "baz2:my*.xlsx/bazbaz",
        };

        [Theory]
        [MemberData(nameof(Examples))]
        public void Parse(string fileName, string sheetName, string yamlTableName) {
            var sheetNameMaps = SheetNameMaps.FromMixed(sheetNameMapStrs);
            var sheetNameMap = sheetNameMaps.Find(fileName, sheetName);
            Assert.Equal(yamlTableName, sheetNameMap == null ? null : sheetNameMap.YamlTableName);
        }
    }
}
