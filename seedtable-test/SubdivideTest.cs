using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

using SeedTable;

namespace seedtable_test {
    public class SubdivideTest : FromTestBase {
        private const string SheetName = "foo_bars";
        private readonly IEnumerable<Dictionary<string, object>> AllData;

        public SubdivideTest() {
            var options = BuildOptions();
            Prepare(options);
            var data = YamlData.YamlToData(File.ReadAllText(Path.Combine(options.output, $"{SheetName}.yml")));
            AllData = data.Table.OrderBy(record => record["id"]);
        }

        protected FromOptions BuildOptions(int? preCut = null, int? postCut = null) {
            var options = base.BuildOptions();
            options.only = new string[] { SheetName };
            var preCutStr = preCut == null ? "" : $"{preCut}:";
            var postCutStr = postCut == null ? "" : $":{postCut}";
            options.subdivide = new string[] { $"{preCutStr}{SheetName}{postCutStr}" };
            return options;
        }

        private static readonly int[] AllIds = new int[] {
            10001,
            10008,
            10015,
            10022,
            10029,
            10036,
            10043,
            10050,
            10057,
            10064,
            10071,
            10078,
            10085,
            10092,
            10099,
            10106,
            10113,
            10120,
            10127,
            10134,
            10141,
            10148,
            10155,
            10162,
            10169,
            10176,
            10183,
            10190,
            10197,
            10204,
            10211,
            10218,
        };

        private static readonly IEnumerable<string> AllIdsStr = AllIds.Select(id => id.ToString());

        public static IEnumerable<object[]> Examples() {
            yield return new object[] { null, 0, AllIdsStr };
            yield return new object[] { 0, null, AllIdsStr };
            yield return new object[] { 0, 0, AllIdsStr };
            yield return new object[] { null, 3, new string[] { "10" } };
            yield return new object[] { null, 5, new string[] { null } };
            yield return new object[] { null, 9, new string[] { null } };
            yield return new object[] { 1, null, AllIdsStr.Select(id => id.Substring(1)) };
            yield return new object[] { 5, null, new string[] { null } };
            yield return new object[] { 6, null, new string[] { null } };
            yield return new object[] { 0, 3, new string[] { "10" } };
            yield return new object[] { 1, 0, AllIdsStr.Select(id => id.Substring(1)) };
            yield return new object[] { 1, 2, new string[] { "00", "01", "02" } };
            yield return new object[] { 2, 2, new string[] { "0", "1", "2" } };
            yield return new object[] { 3, 2, new string[] { null } };
            yield return new object[] { 3, 3, new string[] { null } };
        }

        private readonly Func<string, int> IdComparator = id => id == null ? 0 : int.Parse(id);

        [Theory]
        [MemberData(nameof(Examples))]
        private void Check(int? preCut, int? postCut, IEnumerable<string> separatedIds) {
            var options = BuildOptions(preCut, postCut);
            Prepare(options);

            var destPath = Path.Combine(options.output, SheetName);
            Assert.True(Directory.Exists(destPath));
            var separatedAllData = new List<IEnumerable<Dictionary<string, object>>>();
            var separatedAllIds = new List<string>();
            foreach (var filePath in Directory.GetFiles(destPath)) {
                var separatedId = Path.GetFileNameWithoutExtension(filePath).Substring(4);
                separatedAllIds.Add(separatedId.Length == 0 ? null : separatedId);
                var data = YamlData.YamlToData(File.ReadAllText(filePath));
                separatedAllData.Add(data.Table);
            }
            var combinedAllData = separatedAllData.SelectMany(data => data).OrderBy(record => record["id"]);
            Assert.Equal(AllData, combinedAllData);
            Assert.Equal(separatedAllIds.OrderBy(IdComparator), separatedIds.OrderBy(IdComparator));
        }
    }
}
