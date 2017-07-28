using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

using SeedTable;

namespace seedtable_test {
    public class FromTest : FromToTestBase {
        [Fact]
        public void EqualToExample() {
            var options = BuildFromOptions();
            Prepare(options);

            foreach (var sourcePath in Directory.GetFiles(Paths.SourceSeedPath)) {
                var filename = Path.GetFileName(sourcePath);
                var destinationPath = Path.Combine(options.output, filename);
                Assert.True(File.Exists(destinationPath));
                var sourceContent = GetYamlData(sourcePath);
                var destinationContent = GetYamlData(destinationPath);
                Assert.Equal(sourceContent, destinationContent);
            }
        }
    }
}
