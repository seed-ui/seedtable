using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

using SeedTable;

namespace seedtable_test {
    public class PrimaryTest : FromToTestBase {
        private const string SheetName = "foos";

        [Theory]
        [InlineData("seedtable_example.xlsx")]
        [InlineData("seedtable_example_other.xlsx")]
        private void WithFileNameOnly(string fileName) {
            var options = BuildFromOptions(new string[] { "seedtable_example.xlsx", "seedtable_example_other.xlsx" });
            options.primary = new string[] { $"{fileName}/{SheetName}" };
            Prepare(options);

            var sourcePath = Path.Combine(fileName == "seedtable_example.xlsx" ? Paths.SourceSeedPath : Paths.OtherSeedPath, $"{SheetName}.yml");
            var destinationPath = Path.Combine(options.output, $"{SheetName}.yml");
            Assert.True(File.Exists(destinationPath));
            var sourceContent = GetYamlData(sourcePath);
            var destinationContent = GetYamlData(destinationPath);
            Assert.Equal(sourceContent, destinationContent);
        }
    }
}
