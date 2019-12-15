using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seedtable_egui.Data {
    public class PersonalFormValues {
        public string DataExcelsDirectoryPath { get; set; }
        public string TemplateExcelsDirectoryPath { get; set; }

        public PersonalFormValues() { }
        public PersonalFormValues(string dataExcelsDirectoryPath, string templateExcelsDirectoryPath) {
            DataExcelsDirectoryPath = dataExcelsDirectoryPath;
            TemplateExcelsDirectoryPath = templateExcelsDirectoryPath;
        }
    }
}
