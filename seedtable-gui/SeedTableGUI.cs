using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SeedTable;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace seedtable_gui {
    public partial class SeedTableGUI : Form {
        public SeedTableGUI() {
            InitializeComponent();
            yamlToExcelArea.AllowDrop = true;
            excelToYamlArea.AllowDrop = true;
        }

        private void mainLayout_Paint(object sender, PaintEventArgs e) {

        }

        private void SeedTableGUI_Load(object sender, EventArgs e) {
            RestoreFormValues();
            RestorePersonalFormValues();
        }

        private void seedPathButton_Click(object sender, EventArgs e) {
            seedFolderBrowserDialog.SelectedPath = SeedPath;
            if (seedFolderBrowserDialog.ShowDialog() == DialogResult.OK) {
                SeedPath = seedFolderBrowserDialog.SelectedPath;
            }
        }

        private void settingPathButton_Click(object sender, EventArgs e) {
            settingOpenFileDialog.FileName = Path.GetFileName(SettingPath);
            settingOpenFileDialog.InitialDirectory = Path.GetDirectoryName(SettingPath);
            if (settingOpenFileDialog.ShowDialog() == DialogResult.OK) {
                SettingPath = settingOpenFileDialog.FileName;
            }
        }

        private void seedPathTextBox_DragEnter(object sender, DragEventArgs e) {
            DragEnterBase(e);
        }

        private void settingPathTextBox_DragEnter(object sender, DragEventArgs e) {
            DragEnterBase(e);
        }

        private void seedPathTextBox_DragDrop(object sender, DragEventArgs e) {
            var directory = GetDropedDirectory(e);
            if (directory != null) SeedPath = directory;
        }

        private void settingPathTextBox_DragDrop(object sender, DragEventArgs e) {
            var file = GetDropedFile(e);
            if (file != null) SettingPath = file;
        }

        private void seedPathTextBox_TextChanged(object sender, EventArgs e) {
            SaveFormValues();
        }

        private void settingPathTextBox_TextChanged(object sender, EventArgs e) {
            SaveFormValues();
        }

        private void yamlToExcelArea_DragEnter(object sender, DragEventArgs e) {
            DragEnterBase(e);
        }

        private void excelToYamlArea_DragEnter(object sender, DragEventArgs e) {
            DragEnterBase(e);
        }

        private void yamlToExcelArea_DragDrop(object sender, DragEventArgs e) {
            YamlToExcel(GetDropedExcel(e));
        }

        private void excelToYamlArea_DragDrop(object sender, DragEventArgs e) {
            ExcelToYaml(GetDropedExcel(e));
        }

        private void yamlToExcelArea_DoubleClick(object sender, EventArgs e) {
            YamlToExcel(GetTemplateExcelsFromDialog());
        }

        private void excelToYamlArea_DoubleClick(object sender, EventArgs e) {
            ExcelToYaml(GetDataExcelsFromDialog());
        }

        private void YamlToExcel(string[] fileNames) {
            if (fileNames == null) return;
            var fileBaseNames = fileNames.Select(fileName => Path.GetFileName(fileName));
            var fileDirNames = fileNames.Select(fileName => Path.GetDirectoryName(fileName));
            var fileDirName = fileDirNames.First();
            if (!fileDirNames.All(_fileDirName => fileDirName == _fileDirName)) {
                MessageBox.Show("同じフォルダにあるxlsxファイルのみにして下さい", "エラー");
                return;
            }
            var setting = LoadSetting();
            if (setting == null) return;
            dataExcelFolderBrowserDialog.SelectedPath = DataExcelsDirectoryPath;
            if (dataExcelFolderBrowserDialog.ShowDialog() == DialogResult.OK) {
                DataExcelsDirectoryPath = dataExcelFolderBrowserDialog.SelectedPath;
            } else {
                return;
            }
            var options = new ToOptions();
            options.files = fileBaseNames;
            options.seedInput = SeedPath;
            options.xlsxInput = fileDirName;
            options.output = DataExcelsDirectoryPath;
            options.columnNamesRow = setting.columnNamesRow;
            options.dataStartRow = setting.dataStartRow;
            options.engine = setting.engine;
            options.ignoreColumns = setting.ignoreColumns;
            options.ignore = setting.ignore;
            options.only = setting.only;
            options.subdivide = setting.subdivide;
            options.versionColumn = setting.versionColumn;
            options.requireVersion = setting.requireVersion;
            options.delete = setting.delete;
            options.calcFormulas = setting.calcFormulas;
            var dialog = new YamlToExcelDialog(options);
            dialog.ShowDialog();
        }

        private void ExcelToYaml(string[] fileNames) {
            if (fileNames == null) return;
            var setting = LoadSetting();
            if (setting == null) return;
            var options = new FromOptions();
            options.files = fileNames;
            options.input = ".";
            options.output = SeedPath;
            options.columnNamesRow = setting.columnNamesRow;
            options.dataStartRow = setting.dataStartRow;
            options.engine = setting.engine;
            options.ignoreColumns = setting.ignoreColumns;
            options.ignore = setting.ignore;
            options.only = setting.only;
            options.subdivide = setting.subdivide;
            options.versionColumn = setting.versionColumn;
            options.requireVersion = setting.requireVersion;
            var dialog = new ExcelToYamlDialog(options);
            dialog.ShowDialog();
        }

        private void DragEnterBase(DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        private string[] GetDropedExcel(DragEventArgs e) {
            var fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            if (!fileNames.All(fileName => Path.GetExtension(fileName) == ".xlsx")) {
                MessageBox.Show("xlsxファイルだけを指定して下さい", "エラー");
                return null;
            }
            return fileNames;
        }

        private string GetDropedFile(DragEventArgs e) {
            var fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            if (fileNames.Count() != 1 || !File.Exists(fileNames.First())) {
                MessageBox.Show("1ファイルだけを指定して下さい", "エラー");
                return null;
            }
            return fileNames.First();
        }

        private string GetDropedDirectory(DragEventArgs e) {
            var fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            if (fileNames.Count() != 1 || !Directory.Exists(fileNames.First())) {
                MessageBox.Show("1フォルダだけを指定して下さい", "エラー");
                return null;
            }
            return fileNames.First();
        }

        private string[] GetDataExcelsFromDialog() {
            string[] fileNames = null;
            dataExcelOpenFileDialog.InitialDirectory = DataExcelsDirectoryPath;
            if (dataExcelOpenFileDialog.ShowDialog() == DialogResult.OK) {
                fileNames = dataExcelOpenFileDialog.FileNames;
                if (fileNames.Count() > 0) DataExcelsDirectoryPath = Path.GetDirectoryName(fileNames.First());
            }
            return fileNames;
        }

        private string[] GetTemplateExcelsFromDialog() {
            string[] fileNames = null;
            templateExcelOpenFileDialog.InitialDirectory = TemplateExcelsDirectoryPath;
            if (templateExcelOpenFileDialog.ShowDialog() == DialogResult.OK) {
                fileNames = templateExcelOpenFileDialog.FileNames;
                if (fileNames.Count() > 0) TemplateExcelsDirectoryPath = Path.GetDirectoryName(fileNames.First());
            }
            return fileNames;
        }

        private string SeedPath {
            get { return seedPathTextBox.Text; }
            set { seedPathTextBox.Text = value; }
        }

        private string SettingPath {
            get { return settingPathTextBox.Text; }
            set { settingPathTextBox.Text = value; }
        }

        private string DataExcelsDirectoryPath {
            get { return _DataExcelsDirectoryPath; }
            set {
                _DataExcelsDirectoryPath = value;
                SavePersonalFormValues();
            }
        }
        private string _DataExcelsDirectoryPath;

        private string TemplateExcelsDirectoryPath {
            get { return _TemplateExcelsDirectoryPath; }
            set {
                _TemplateExcelsDirectoryPath = value;
                SavePersonalFormValues();
            }
        }
        private string _TemplateExcelsDirectoryPath;

        private ToOptions LoadSetting() {
            if (SettingPath == null || SettingPath.Length == 0) {
                MessageBox.Show("設定ファイルを指定して下さい", "エラー");
                return null;
            }
            if (!File.Exists(SettingPath)) {
                MessageBox.Show("指定された設定ファイルがありません", "エラー");
                return null;
            }
            var yaml = File.ReadAllText(SettingPath);
            var builder = new DeserializerBuilder();
            builder.WithNamingConvention(new HyphenatedNamingConvention());
            var deserializer = builder.Build();
            var options = deserializer.Deserialize<ToOptions>(yaml);
            if (options == null) {
                MessageBox.Show("設定ファイルが空です", "エラー");
                return null;
            }
            return options;
        }

        private void SaveFormValues() {
            var yaml = new Serializer().Serialize(new FormValues(SeedPath, SettingPath));
            File.WriteAllText(FormValuesPath, yaml);
        }

        private void RestoreFormValues() {
            if (!File.Exists(FormValuesPath)) return;
            var yaml = File.ReadAllText(FormValuesPath);
            var formValues = new Deserializer().Deserialize<FormValues>(yaml);
            SeedPath = formValues.SeedPath;
            SettingPath = formValues.SettingPath;
        }

        private void SavePersonalFormValues() {
            var yaml = new Serializer().Serialize(new PersonalFormValues(DataExcelsDirectoryPath, TemplateExcelsDirectoryPath));
            File.WriteAllText(PersonalFormValuesPath, yaml);
        }

        private void RestorePersonalFormValues() {
            if (!File.Exists(PersonalFormValuesPath)) return;
            var yaml = File.ReadAllText(PersonalFormValuesPath);
            var personalFormValues = new Deserializer().Deserialize<PersonalFormValues>(yaml);
            DataExcelsDirectoryPath = personalFormValues.DataExcelsDirectoryPath;
            TemplateExcelsDirectoryPath = personalFormValues.TemplateExcelsDirectoryPath;
        }

        private string FormValuesPath {
            get { return Path.Combine(Application.StartupPath, FormValuesFile); }
        }
        private const string FormValuesFile = "settings.yml";

        private string PersonalFormValuesPath {
            get { return Path.Combine(Application.StartupPath, PersonalFormValuesFile); }
        }
        private const string PersonalFormValuesFile = "personal_settings.yml";
    }
}
