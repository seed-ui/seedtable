using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seedtable_egui.Data {
    public class FormValues {
        public string SeedPath { get; set; }
        public string SettingPath { get; set; }
        public string SourcePath { get; set; } // FormValuesX11 で作られた設定との互換
        public string YamlToExcelTargetFolder { get; set; } // FormValuesX11 で作られた設定との互換

        public FormValues() { }
        public FormValues(string seedPath, string settingPath) {
            SeedPath = seedPath;
            SettingPath = settingPath;
        }
    }
}
