using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmSeedtable {
    public class FormValuesX11 {
        public string SeedPath { get; set; }
        public string SettingPath { get; set; }
        public string SourcePath { get; set; }
        public string YamlToExcelTargetFolder { get; set; }

        public FormValuesX11() { }
        public FormValuesX11(string seedPath, string settingPath, string sourcePath, string yamlToExcelTargetFolder) {
            SeedPath = seedPath;
            SettingPath = settingPath;
            SourcePath = sourcePath;
            YamlToExcelTargetFolder = yamlToExcelTargetFolder;
        }
    }
}
