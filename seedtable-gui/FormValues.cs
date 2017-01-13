using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seedtable_gui {
    public class FormValues {
        public string SeedPath { get; set; }
        public string SettingPath { get; set; }

        public FormValues() { }
        public FormValues(string seedPath, string settingPath) {
            SeedPath = seedPath;
            SettingPath = settingPath;
        }
    }
}
