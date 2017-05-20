using System.IO;

namespace seedtable_test {
    public static class Paths {
        public static readonly string TestResourcesPath = Path.Combine("../../test-resources");
        public static readonly string SourceExcelPath = Path.Combine(TestResourcesPath, "seedtable_example.xlsx");
        public static readonly string SourceSeedPath = Path.Combine(TestResourcesPath, "seeds");
        public static readonly string DestinationBasePath = Path.Combine("../../test-tmp");
        public static string DestinationSeedPath(string seedPath = null) {
            return Path.Combine(DestinationBasePath, seedPath ?? Path.GetRandomFileName());
        }
    }
}
