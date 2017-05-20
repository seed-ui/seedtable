using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

using SeedTable;

namespace seedtable_test {
    public class FromTestBase : IDisposable {
        private readonly List<FromOptions> OptionsList = new List<FromOptions>();

        protected virtual FromOptions BuildOptions() {
            var options = new FromOptions();
            options.files = new string[] { Paths.SourceExcelPath };
            options.engine = CommonOptions.Engine.EPPlus;
            options.yamlColumns = new string[] { "data_yaml" };
            options.output = Paths.DestinationSeedPath();
            OptionsList.Add(options);
            return options;
        }

        protected void Prepare(FromOptions options) {
            SeedTableInterface.ExcelToSeed(options);
        }

        public void Dispose() {
            foreach (var options in OptionsList) {
                Directory.Delete(options.output, true);
            }
        }
    }
}
