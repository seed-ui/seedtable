using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

using SeedTable;

namespace seedtable_test {
    public class AliasTest : FromToTestBase {
        private const string ExcelName = "seedtable_example_alias.xlsx";
        private readonly IEnumerable<string> Alias = new string[] { "foos:foos_ref", "foos:foos_ref2" };

        [Fact]
        private void FromWithAlias() {
            var options = BuildFromOptions(new string[] { ExcelName });
            options.alias = Alias;
            Prepare(options);
            var files = Directory.GetFiles(options.output).Select(path => Path.GetFileName(path)).OrderBy(file => file);
            Assert.Equal(files, new string[] { "bars.yml", "foos.yml" });
        }

        [Fact]
        private void ToWithAlias() {
            var toOptions = BuildToOptions(new string[] { ExcelName }, Paths.AliasSeedPath, Paths.TestResourcesPath, Paths.DestinationSeedPath());
            toOptions.alias = Alias;
            toOptions.delete = true;
            var fromOptions = BuildFromOptions(new string[] { ExcelName }, toOptions.output, toOptions.output);
            Prepare(toOptions);
            Prepare(fromOptions);

            var sourceData = GetYamlData(Path.Combine(Paths.AliasSeedPath, "foos.yml"));
            foreach (var sheetName in new string[] { "foos", "foos_ref", "foos_ref2" }) {
                var data = GetYamlData(Path.Combine(toOptions.output, $"{sheetName}.yml"));
                Assert.Equal(sourceData, data);
            }
        }
    }
}
