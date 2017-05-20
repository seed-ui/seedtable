using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

using SeedTable;

namespace seedtable_test {
    public class ToTest : FromToTestBase {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EqualToExample(bool delete) {
            var toOptions = BuildToOptions(Paths.ToSeedPath, Paths.TestResourcesPath, Paths.DestinationSeedPath());
            toOptions.delete = delete;
            var fromOptions = BuildFromOptions(toOptions.output, toOptions.output);
            Prepare(toOptions);
            Prepare(fromOptions);

            var comparePath = delete ? Paths.ToDeleteSeedPath : Paths.ToUnionSeedPath;
            foreach (var sourcePath in Directory.GetFiles(comparePath)) {
                var filename = Path.GetFileName(sourcePath);
                var destinationPath = Path.Combine(fromOptions.output, filename);
                Assert.True(File.Exists(destinationPath));
                var sourceContent = YamlData.YamlToData(File.ReadAllText(sourcePath)).Table;
                var destinationContent = YamlData.YamlToData(File.ReadAllText(destinationPath)).Table;
                Assert.Equal(sourceContent, destinationContent);
            }
        }
    }
}
