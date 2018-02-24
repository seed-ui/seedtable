using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

using SeedTable;

namespace seedtable_test {
    public class FromToTestBase : IDisposable {
        private readonly List<CommonOptions> OptionsList = new List<CommonOptions>();

        protected virtual FromOptions BuildFromOptions(string input = null, string output = null) {
            return BuildFromOptions(new string[] { Paths.SourceExcelName }, input, output);
        }

        protected virtual FromOptions BuildFromOptions(IEnumerable<string> files, string input = null, string output = null) {
            var options = new FromOptions();
            options.files = files;
            options.engine = CommonOptions.Engine.EPPlus;
            options.yamlColumns = new string[] { "data_yaml" };
            options.input = input ?? Paths.TestResourcesPath;
            options.output = output ?? Paths.DestinationSeedPath();
            OptionsList.Add(options);
            return options;
        }

        protected void Prepare(FromOptions options) {
            SeedTableInterface.ExcelToSeed(options);
        }

        protected virtual ToOptions BuildToOptions(string seedInput, string xlsxInput, string output, bool calcFormulas = true) {
            return BuildToOptions(new string[] { Paths.SourceExcelName }, seedInput, xlsxInput, output, calcFormulas);
        }

        protected virtual ToOptions BuildToOptions(IEnumerable<string> files, string seedInput, string xlsxInput, string output, bool calcFormulas = true) {
            var options = new ToOptions();
            options.files = files;
            options.engine = CommonOptions.Engine.EPPlus;
            options.calcFormulas = calcFormulas;
            options.xlsxInput = xlsxInput;
            options.seedInput = seedInput;
            options.output = output;
            OptionsList.Add(options);
            return options;
        }

        protected void Prepare(ToOptions options) {
            SeedTableInterface.SeedToExcel(options);
        }

        protected IEnumerable<Dictionary<string, object>> GetYamlData(string path) {
            return YamlData.YamlToData(File.ReadAllText(path)).Table;
        }

        public void Dispose() {
            foreach (var output in OptionsList.Select(options => options.output).Distinct()) {
                Directory.Delete(output, true);
            }
        }
    }
}
