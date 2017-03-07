namespace XmSeedtable {
    public class PersonalFormValuesX11 {
        public string DataExcelsDirectoryPath { get; set; }
        public string TemplateExcelsDirectoryPath { get; set; }

        public PersonalFormValuesX11() { }
        public PersonalFormValuesX11(string dataExcelsDirectoryPath, string templateExcelsDirectoryPath) {
            DataExcelsDirectoryPath = dataExcelsDirectoryPath;
            TemplateExcelsDirectoryPath = templateExcelsDirectoryPath;
        }
    }
}
