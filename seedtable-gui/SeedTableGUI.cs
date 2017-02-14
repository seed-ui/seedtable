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
        }

        private void seedPathButton_Click(object sender, EventArgs e) {
            if (seedFolderBrowserDialog.ShowDialog() == DialogResult.OK) {
                SeedPath = seedFolderBrowserDialog.SelectedPath;
            }
        }

        private void settingPathButton_Click(object sender, EventArgs e) {
            if (settingOpenFileDialog.ShowDialog() == DialogResult.OK) {
                SettingPath = settingOpenFileDialog.FileName;
            }
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
            YamlToExcel(GetExcelsFromDialog());
        }

        private void excelToYamlArea_DoubleClick(object sender, EventArgs e) {
            ExcelToYaml(GetExcelsFromDialog());
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
            string targetFolder;
            if (excelFolderBrowserDialog.ShowDialog() == DialogResult.OK) {
                targetFolder = excelFolderBrowserDialog.SelectedPath;
            } else {
                return;
            }
            var options = new ToOptions();
            options.files = fileBaseNames;
            options.seedInput = SeedPath;
            options.xlsxInput = fileDirName;
            options.output = targetFolder;
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

        private string[] GetExcelsFromDialog() {
            string[] fileNames = null;
            if (excelOpenFileDialog.ShowDialog() == DialogResult.OK) {
                fileNames = excelOpenFileDialog.FileNames;
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

        private string FormValuesPath {
            get { return Path.Combine(Application.StartupPath, FormValuesFile); }
        }
        private const string FormValuesFile = "settings.yml";
    }
}
