using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

using SeedTable;

namespace seedtable_test {
    public class SubdivideOptionTest {
        public static IEnumerable<object[]> Examples() {
            yield return new object[] { "foos", "NO_NAME", "foos", false, 0, 0, "id", OnOperation.From | OnOperation.To };
            yield return new object[] { "foo.xlsx/foos", "foo.xlsx", "foos", false, 0, 0, "id", OnOperation.From | OnOperation.To };
            yield return new object[] { "foos:0", "NO_NAME", "foos", true, 0, 0, "id", OnOperation.From | OnOperation.To };
            yield return new object[] { "0:foos", "NO_NAME", "foos", true, 0, 0, "id", OnOperation.From | OnOperation.To };
            yield return new object[] { "foos:1", "NO_NAME", "foos", true, 0, 1, "id", OnOperation.From | OnOperation.To };
            yield return new object[] { "1:foos", "NO_NAME", "foos", true, 1, 0, "id", OnOperation.From | OnOperation.To };
            yield return new object[] { "2:foos:2", "NO_NAME", "foos", true, 2, 2, "id", OnOperation.From | OnOperation.To };
            yield return new object[] { "foos@NO_OPTION", "NO_NAME", "foos", false, 0, 0, "id", OnOperation.From | OnOperation.To };
            yield return new object[] { "foos@from", "NO_NAME", "foos", false, 0, 0, "id", OnOperation.From };
            yield return new object[] { "foos@TO", "NO_NAME", "foos", false, 0, 0, "id", OnOperation.To };
            yield return new object[] { "foos@key=foo_id", "NO_NAME", "foos", false, 0, 0, "foo_id", OnOperation.From | OnOperation.To };
            yield return new object[] { "foos@from@key=foo_id", "NO_NAME", "foos", false, 0, 0, "foo_id", OnOperation.From };
            yield return new object[] { "1:foo.xlsx/foos:0@from@key=foo_id", "foo.xlsx", "foos", true, 1, 0, "foo_id", OnOperation.From };
        }

        [Theory]
        [MemberData(nameof(Examples))]
        public void Parse(string subdivideStr, string fileName, string sheetName, bool needSubdivide, int cutPrefix, int cutPostfix, string keyColumnName, OnOperation oonOperation) {
            var subdivide = SheetNameWithSubdivide.FromMixed(subdivideStr);
            Assert.True(subdivide.FileName.IsMatch(fileName));
            Assert.True(subdivide.SheetName.IsMatch(sheetName));
            Assert.Equal(subdivide.NeedSubdivide, needSubdivide);
            Assert.Equal(subdivide.CutPrefix, cutPrefix);
            Assert.Equal(subdivide.CutPostfix, cutPostfix);
            Assert.Equal(subdivide.KeyColumnName, keyColumnName);
            Assert.Equal(subdivide.OnOperation, oonOperation);
        }
    }
}
