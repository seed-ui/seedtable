using System;
using System.Linq;
using TonNurako.Widgets;
using TonNurako.Widgets.Xm;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

using SeedTable;
using System.IO;

namespace XmSeedtable
{
    public partial class SeedTableX11 :
        TonNurako.Widgets.LayoutWindow<TonNurako.Widgets.Xm.Form>
    {
        public SeedTableX11() {
        }

        public override void ShellCreated() {
            Sinatra();
        }

        private void SeedTableGUI_Load(object sender, EventArgs e) {
            RestoreFormValues();
            RestorePersonalFormValues();
        }

        private void seedPathButton_Click(object sender, TonNurako.Events.PushButtonEventArgs e) {
            var d = new FileSelectionDialog();
            d.FileTypeMask = FileTypeMask.Directory;
            d.DialogStyle = DialogStyle.FullApplicationModal;
            d.PathMode = PathMode.Relative;
            d.Directory = SeedPath;

            d.OkEvent += (x,y) => {
                SeedPath = System.IO.Path.Combine(d.Directory, d.TextString);
                d.Destroy();
            };
            d.CancelEvent += (x,y) => {
                d.Destroy();
            };
            this.Layout.Children.Add(d);
        }

        private void seedPathTextBox_TextChanged(object sender, EventArgs e) {
            SaveFormValues();
        }

        private void settingPathTextBox_TextChanged(object sender, EventArgs e) {
            SaveFormValues();
        }

        private void sourceTextBox_TextChanged(object sender, EventArgs e) {
            SaveFormValues();
            if (!Directory.Exists(SourcePath)) return;
            fileListBox.DeleteAllItems();
            var dpi = (from dir in (new DirectoryInfo(SourcePath)).EnumerateFiles()
                        where dir.Extension == ".xlsx"
                        select dir.ToString()).ToList();
            if (0 == dpi.Count) {
                ShowMessageBox($"このフォルダーにはExcelのファイルがないよう\n{SourcePath}", "エロー");
                return;
            }
            fileListBox.AddItems(
                (from w in dpi select Path.GetFileName(w)).ToArray(), 0, false);
        }

        private void settingPathButton_Click(object sender, TonNurako.Events.PushButtonEventArgs e) {
            var d = new FileSelectionDialog();
            d.FileTypeMask = FileTypeMask.Regular;
            d.DialogStyle = DialogStyle.FullApplicationModal;
            d.PathMode = PathMode.Relative;
            if (SettingPath != null && SettingPath.Length > 0) d.Directory = Path.GetDirectoryName(SettingPath);
            d.OkEvent += (x,y) => {
                SettingPath = System.IO.Path.Combine(d.Directory, d.TextString);
                d.Destroy();
            };
            d.CancelEvent += (x,y) => {
                d.Destroy();
            };
            this.Layout.Children.Add(d);
        }

        private void sourceButton_Click(object sender, TonNurako.Events.PushButtonEventArgs e) {
            var d = new FileSelectionDialog();
            d.FileTypeMask = FileTypeMask.Directory;
            d.DialogStyle = DialogStyle.FullApplicationModal;
            d.PathMode = PathMode.Relative;
            d.Directory = SourcePath;

            d.OkEvent += (x,y) => {
                SourcePath = System.IO.Path.Combine(d.Directory, d.TextString);
                d.Destroy();
            };
            d.CancelEvent += (x,y) => {
                d.Destroy();
            };
            this.Layout.Children.Add(d);
        }
        private void excelToYamlArea_Click(object sender, TonNurako.Events.PushButtonEventArgs e) {
            ExcelToYaml(SourceExcelFileNames());
        }

        private void yamlToExcelArea_Click(object sender, TonNurako.Events.PushButtonEventArgs e) {
            YamlToExcel(SourceExcelFileNames());
        }
        private void YamlToExcel(string[] fileNames) {
            if (fileNames == null) return;
            var fileBaseNames = fileNames.Select(fileName => Path.GetFileName(fileName));
            var fileDirNames = fileNames.Select(fileName => Path.GetDirectoryName(fileName));
            var fileDirName = fileDirNames.First();
            if (!fileDirNames.All(_fileDirName => fileDirName == _fileDirName)) {
                ShowMessageBox("同じフォルダにあるxlsxファイルのみにして下さい", "エラー");
                return;
            }
            var setting = LoadSetting();
            if (setting == null) {
                 return;
            }
            var d = new FileSelectionDialog();
            d.FileTypeMask = FileTypeMask.Directory;
            d.DialogStyle = DialogStyle.FullApplicationModal;
            d.PathMode = PathMode.Relative;
            d.Directory = YamlToExcelTargetFolder;
            d.CancelEvent += (x,y) => {
                d.Destroy();
            };
            d.OkEvent += (x,y) => {
                YamlToExcelTargetFolder = System.IO.Path.Combine(d.Directory, d.TextString);
                SaveFormValues();

                var options = new ToOptions();
                options.files = fileBaseNames;
                options.seedInput = SeedPath;
                options.xlsxInput = fileDirName;
                options.output = YamlToExcelTargetFolder;
                options.columnNamesRow = setting.columnNamesRow;
                options.dataStartRow = setting.dataStartRow;
                options.engine = setting.engine;
                options.ignoreColumns = setting.ignoreColumns;
                options.format = setting.format;
                options.ignore = setting.ignore;
                options.only = setting.only;
                options.subdivide = setting.subdivide;
                options.mapping = setting.mapping;
                options.alias = setting.alias;
                options.versionColumn = setting.versionColumn;
                options.requireVersion = setting.requireVersion;
                options.delete = setting.delete;
                options.seedExtension = setting.seedExtension;
                options.calcFormulas = setting.calcFormulas;
                d.Destroy();

                var dialog = new YamlToExcelDialogX11(options);
                this.Layout.Children.Add(dialog);
                dialog.Popup(GrabOption.Exclusive);
            };
            this.Layout.Children.Add(d);
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
            options.format = setting.format;
            options.yamlColumns = setting.yamlColumns;
            options.ignore = setting.ignore;
            options.only = setting.only;
            options.subdivide = setting.subdivide;
            options.primary = setting.primary;
            options.mapping = setting.mapping;
            options.alias = setting.alias;
            options.versionColumn = setting.versionColumn;
            options.requireVersion = setting.requireVersion;
            options.delete = setting.delete;
            options.seedExtension = setting.seedExtension;

            var dialog = new ExcelToYamlDialogX11(options);
            this.Layout.Children.Add(dialog);
            dialog.Popup(GrabOption.Exclusive);
        }

        private void ShowMessageBox(string message, string title) {
            var d = new ErrorDialog();
            d.DialogTitle = title;
            d.DialogStyle = DialogStyle.ApplicationModal;
            d.MessageString = message;
            d.OkLabelString = "わかった";

            d.WidgetCreatedEvent += (x, y) => {
                d.Items.Cancel.Visible = false;
                d.Items.Help.Visible = false;
            };

            this.Layout.Children.Add(d);
            d.Visible = true;
        }

        private string[] SourceExcelFileNames() {
            if (fileListBox.ItemCount == 0) return null;
            // 挙動がぁゃιぃので代替手段
            // var fileNames = fileListBox.SelectedItems.Select(fileName => Path.Combine(SourcePath == null ? "" : SourcePath, fileName)).ToArray();
            var fileNames = fileListBox.SelectedPositions.Select(index => Path.Combine(SourcePath == null ? "" : SourcePath, fileListBox.Items[index - 1])).ToArray();
            if (fileNames.Count() == 0) return null;
            return fileNames;
        }

        private string SeedPath {
            get { return seedPathTextBox.Value; }
            set { seedPathTextBox.Value = value; }
        }

        private string SettingPath {
            get { return settingPathTextBox.Value; }
            set { settingPathTextBox.Value = value; }
        }

        private string SourcePath {
            get { return sourceTextBox.Value; }
            set { sourceTextBox.Value = value; }
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

        private string YamlToExcelTargetFolder { get; set; }

        internal ToOptions LoadSetting(bool showAlert = true) {
            if (SettingPath == null || SettingPath.Length == 0) {
                if (showAlert) ShowMessageBox("設定ファイルを指定して下さい", "エラー");
                return null;
            }
            if (!File.Exists(SettingPath)) {
                if (showAlert) ShowMessageBox("指定された設定ファイルがありません", "エラー");
                return null;
            }
            var yaml = File.ReadAllText(SettingPath);
            var builder = new DeserializerBuilder();
            builder.WithNamingConvention(new HyphenatedNamingConvention());
            var deserializer = builder.Build();
            var options = deserializer.Deserialize<ToOptions>(yaml);
            if (options == null) {
               if (showAlert)  ShowMessageBox("設定ファイルが空です", "エラー");
                return null;
            }
            return options;
        }

        private const string DefaultSettingFile = "options.yml";

        internal void SaveSetting(ToOptions options) {
            // 使わないパス情報は空にして出力しないようにする
            options.output = null;
            options.seedInput = null;
            options.xlsxInput = null;
            var builder = new SerializerBuilder();
            builder.WithNamingConvention(new HyphenatedNamingConvention());
            var serializer = builder.Build();
            var yaml = serializer.Serialize(options);
            if (SettingPath == null || SettingPath.Length == 0) {
                // TODO: もう少しエレガントな方法は無いものか
                SettingPath =
                    Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), DefaultSettingFile);
            }
            File.WriteAllText(SettingPath, yaml);
        }

        public class SettingHandler {
            SeedTableX11 x11;
            public SettingHandler(SeedTableX11 x) {
                x11 = x;
            }
            public void Save(ToOptions opt) {
                x11.SaveSetting(opt);
            }
        }

        private bool SettingReadOnly() {
            return File.Exists(SettingReadOnlyPath);
        }

        private void settingButton_Click(object sender, EventArgs e) {
            var setting = LoadSetting(false);
            if (setting == null) {
                 setting = new ToOptions();
            }
            var settingReadOnly = SettingReadOnly();

            var dialog = new SettingDialogX11(setting, !settingReadOnly, new SettingHandler(this));
            this.Layout.Children.Add(dialog);
            dialog.Popup(GrabOption.Exclusive);
        }

        private void SaveFormValues() {
            var yaml = new Serializer().Serialize(new FormValuesX11(SeedPath, SettingPath, SourcePath, YamlToExcelTargetFolder));
            File.WriteAllText(FormValuesPath, yaml);
        }

        private void RestoreFormValues() {
            if (!File.Exists(FormValuesPath)) return;
            var yaml = File.ReadAllText(FormValuesPath);
            var formValues = new Deserializer().Deserialize<FormValuesX11>(yaml);
            SeedPath = formValues.SeedPath;
            SettingPath = formValues.SettingPath;
            SourcePath = formValues.SourcePath;
            YamlToExcelTargetFolder = formValues.YamlToExcelTargetFolder;
        }

        private void SavePersonalFormValues() {
            var yaml = new Serializer().Serialize(new PersonalFormValuesX11(DataExcelsDirectoryPath, TemplateExcelsDirectoryPath));
            File.WriteAllText(PersonalFormValuesPath, yaml);
        }

        private void RestorePersonalFormValues() {
            if (!File.Exists(PersonalFormValuesPath)) return;
            var yaml = File.ReadAllText(PersonalFormValuesPath);
            var personalFormValues = new Deserializer().Deserialize<PersonalFormValuesX11>(yaml);
            DataExcelsDirectoryPath = personalFormValues.DataExcelsDirectoryPath;
            TemplateExcelsDirectoryPath = personalFormValues.TemplateExcelsDirectoryPath;
        }
        private string FormValuesPath {
            get { return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), FormValuesFile); }
        }
        private const string FormValuesFile = "settings.yml";
        private string PersonalFormValuesPath {
            get { return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), PersonalFormValuesFile); }
        }
        private const string PersonalFormValuesFile = "personal_settings.yml";

        private string SettingReadOnlyPath {
            get { return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), SettingReadOnlyFile); }
        }
        private const string SettingReadOnlyFile = "options.readonly";
    }
}
