using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

using SeedTable;

namespace seedtable_test {
    public class FromTest : FromTestBase {
        [Fact]
        public void EqualToExample() {
            var options = BuildOptions();
            Prepare(options);

            foreach (var sourcePath in Directory.GetFiles(Paths.SourceSeedPath)) {
                var filename = Path.GetFileName(sourcePath);
                var destinationPath = Path.Combine(options.output, filename);
                Assert.True(File.Exists(destinationPath));
                var sourceContent = File.ReadAllText(sourcePath);
                var destinationContent = File.ReadAllText(destinationPath);
                Assert.Equal(sourceContent, destinationContent);
            }
        }
    }
}
