using System.IO;

namespace seedtable_test {
    public static class Paths {
        public static readonly string SourceExcelName = "seedtable_example.xlsx";
        public static readonly string TestResourcesPath = Path.Combine("../../test-resources");
        public static readonly string SourceExcelPath = Path.Combine(TestResourcesPath, SourceExcelName);
        public static readonly string SourceSeedPath = Path.Combine(TestResourcesPath, "seeds");
        public static readonly string ToSeedPath = Path.Combine(TestResourcesPath, "seeds_to");
        public static readonly string ToUnionSeedPath = Path.Combine(TestResourcesPath, "seeds_to_union");
        public static readonly string ToDeleteSeedPath = Path.Combine(TestResourcesPath, "seeds_to_delete");
        public static readonly string DestinationBasePath = Path.Combine("../../test-tmp");
        public static string DestinationSeedPath(string seedPath = null) {
            return Path.Combine(DestinationBasePath, seedPath ?? Path.GetRandomFileName());
        }
    }
}
