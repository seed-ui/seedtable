using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TonNurako.Data;
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

        public override void ShellCreated()
        {
            Sinatra();
        }

        private void SeedTableGUI_Load(object sender, EventArgs e) {
            RestoreFormValues();
        }

        private void excelToYamlArea_Click(object sender, TonNurako.Events.PushButtonEventArgs e) {

        }


        private void seedPathButton_Click(object sender, TonNurako.Events.PushButtonEventArgs e) {
            var d = new FileSelectionDialog();
            d.FileTypeMask = FileTypeMask.Directory;
            d.DialogStyle = DialogStyle.FullApplicationModal;
            d.PathMode = PathMode.Relative;

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

        private void settingPathButton_Click(object sender, TonNurako.Events.PushButtonEventArgs e) {
            var d = new FileSelectionDialog();
            d.FileTypeMask = FileTypeMask.Regular;
            d.DialogStyle = DialogStyle.FullApplicationModal;
            d.PathMode = PathMode.Relative;

            d.OkEvent += (x,y) => {
                SettingPath = System.IO.Path.Combine(d.Directory, d.TextString);
                d.Destroy();
            };
            d.CancelEvent += (x,y) => {
                d.Destroy();
            };
            this.Layout.Children.Add(d);
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

        private string SeedPath {
            get { return seedPathTextBox.Value; }
            set { seedPathTextBox.Value = value; }
        }

        private string SettingPath {
            get { return settingPathTextBox.Value; }
            set { settingPathTextBox.Value = value; }
        }

        private ToOptions LoadSetting() {
            if (SettingPath == null || SettingPath.Length == 0) {
                ShowMessageBox("設定ファイルを指定して下さい", "エラー");
                return null;
            }
            if (!File.Exists(SettingPath)) {
                ShowMessageBox("指定された設定ファイルがありません", "エラー");
                return null;
            }
            var yaml = File.ReadAllText(SettingPath);
            var builder = new DeserializerBuilder();
            builder.WithNamingConvention(new HyphenatedNamingConvention());
            var deserializer = builder.Build();
            var options = deserializer.Deserialize<ToOptions>(yaml);
            if (options == null) {
                ShowMessageBox("設定ファイルが空です", "エラー");
                return null;
            }
            return options;
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
            d.CancelEvent += (x,y) => {
                d.Destroy();
            };
            d.OkEvent += (x,y) => {
                var targetFolder = System.IO.Path.Combine(d.Directory, d.TextString);

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
                d.Destroy();

                var dialog = new YamlToExcelDialogX11(options);
                this.Children.Add(dialog);
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
            options.ignore = setting.ignore;
            options.only = setting.only;
            options.subdivide = setting.subdivide;
            options.versionColumn = setting.versionColumn;
            options.requireVersion = setting.requireVersion;

            var dialog = new ExcelToYamlDialogX11(options);
            this.Children.Add(dialog);
            dialog.Popup(GrabOption.Exclusive);
        }


        private void SaveFormValues() {
            var yaml = new Serializer().Serialize(new FormValuesX11(SeedPath, SettingPath));
            File.WriteAllText(FormValuesPath, yaml);
        }

        private void RestoreFormValues() {
            if (!File.Exists(FormValuesPath)) return;
            var yaml = File.ReadAllText(FormValuesPath);
            var formValues = new Deserializer().Deserialize<FormValuesX11>(yaml);
            SeedPath = formValues.SeedPath;
            SettingPath = formValues.SettingPath;
        }

        private string FormValuesPath {
            get { return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), FormValuesFile); }
        }
        private const string FormValuesFile = "settings.yml";
    }
}
