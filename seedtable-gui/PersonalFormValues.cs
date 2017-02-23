using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seedtable_gui {
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
