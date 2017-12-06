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
            if (SettingPath != null && SettingPath.Length > 0) {
                settingOpenFileDialog.FileName = Path.GetFileName(SettingPath);
                settingOpenFileDialog.InitialDirectory = Path.GetDirectoryName(SettingPath);
            }
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

        private void settingButton_Click(object sender, EventArgs e) {
            var setting = LoadSetting(false) ?? new ToOptions();
            var settingReadOnly = SettingReadOnly();
            var dialog = new SettingDialog(setting, !settingReadOnly);
            dialog.ShowDialog();
            if (!settingReadOnly) SaveSetting(setting);
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
            var options = new ToOptions() {
                files = fileBaseNames,
                seedInput = SeedPath,
                xlsxInput = fileDirName,
                output = DataExcelsDirectoryPath,
                columnNamesRow = setting.columnNamesRow,
                dataStartRow = setting.dataStartRow,
                engine = setting.engine,
                ignoreColumns = setting.ignoreColumns,
                format = setting.format,
                ignore = setting.ignore,
                only = setting.only,
                subdivide = setting.subdivide,
                mapping = setting.mapping,
                alias = setting.alias,
                versionColumn = setting.versionColumn,
                requireVersion = setting.requireVersion,
                delete = setting.delete
            };
            options.seedExtension = options.seedExtension;
            options.calcFormulas = setting.calcFormulas;
            var dialog = new YamlToExcelDialog(options);
            dialog.ShowDialog();
        }

        private void ExcelToYaml(string[] fileNames) {
            if (fileNames == null) return;
            var setting = LoadSetting();
            if (setting == null) return;
            var options = new FromOptions() {
                files = fileNames,
                input = ".",
                output = SeedPath,
                columnNamesRow = setting.columnNamesRow,
                dataStartRow = setting.dataStartRow,
                engine = setting.engine,
                ignoreColumns = setting.ignoreColumns,
                format = setting.format,
                yamlColumns = setting.yamlColumns,
                ignore = setting.ignore,
                only = setting.only,
                subdivide = setting.subdivide,
                primary = setting.primary,
                mapping = setting.mapping,
                alias = setting.alias,
                versionColumn = setting.versionColumn,
                requireVersion = setting.requireVersion,
                delete = setting.delete
            };
            options.seedExtension = options.seedExtension;
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
            if (!fileNames.All(fileName => AllowExtensions.Contains(Path.GetExtension(fileName)))) {
                MessageBox.Show("xlsxまたはxlsmファイルだけを指定して下さい", "エラー");
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

        private const string DefaultSettingFile = "options.yml";

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

        private ToOptions LoadSetting(bool showAlert = true) {
            if (SettingPath == null || SettingPath.Length == 0) {
                if (showAlert) MessageBox.Show("設定ファイルを指定して下さい", "エラー");
                return null;
            }
            if (!File.Exists(SettingPath)) {
                if (showAlert) MessageBox.Show("指定された設定ファイルがありません", "エラー");
                return null;
            }
            var yaml = File.ReadAllText(SettingPath);
            var builder = new DeserializerBuilder();
            builder.WithNamingConvention(new HyphenatedNamingConvention());
            var deserializer = builder.Build();
            var options = deserializer.Deserialize<ToOptions>(yaml);
            return options ?? new ToOptions();
        }

        private void SaveSetting(ToOptions options) {
            // 使わないパス情報は空にして出力しないようにする
            options.output = null;
            options.seedInput = null;
            options.xlsxInput = null;
            var builder = new SerializerBuilder();
            builder.WithNamingConvention(new HyphenatedNamingConvention());
            var serializer = builder.Build();
            var yaml = serializer.Serialize(options);
            if (SettingPath == null || SettingPath.Length == 0) SettingPath = Path.Combine(Application.StartupPath, DefaultSettingFile);
            File.WriteAllText(SettingPath, yaml);
        }

        private bool SettingReadOnly() {
            return File.Exists(SettingReadOnlyPath);
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

        private string SettingReadOnlyPath {
            get { return Path.Combine(Application.StartupPath, SettingReadOnlyFile); }
        }
        private const string SettingReadOnlyFile = "options.readonly";

        private static HashSet<string> AllowExtensions = new HashSet<string> {".xlsx", ".xlsm"};
    }
}
