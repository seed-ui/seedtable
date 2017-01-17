using TonNurako.GC;
using TonNurako.Widgets;
using TonNurako.Widgets.Xm;

namespace XmSeedtable
{
    partial class SeedTableX11
    {
        private void Sinatra() {
            mainLayout = this.Layout;

            this.RealizedEvent += SeedTableGUI_Load;

            // seed
            tableLayoutPanel1 = new TonNurako.Widgets.Xm.Form();
            tableLayoutPanel1.TopAttachment = AttachmentType.Form;
            tableLayoutPanel1.LeftAttachment =
            tableLayoutPanel1.RightAttachment = AttachmentType.Form;
            mainLayout.Children.Add(tableLayoutPanel1);

            seedPathLabel = new TonNurako.Widgets.Xm.Label();
            seedPathLabel.LabelString = "seedファイル";
            seedPathTextBox = new TonNurako.Widgets.Xm.Text();
            seedPathTextBox.Width = 168;
            seedPathTextBox.ValueChangedEvent += seedPathTextBox_TextChanged;

            seedPathButton = new TonNurako.Widgets.Xm.PushButton();
            seedPathButton.LabelString = "開く...";
            seedPathButton.ActivateEvent += seedPathButton_Click;
            seedPathButton.RightAttachment = AttachmentType.Form;

            tableLayoutPanel1.Children.Add(seedPathLabel);
            tableLayoutPanel1.Children.Add(seedPathTextBox);
            tableLayoutPanel1.Children.Add(seedPathButton);

            seedPathTextBox.RightAttachment = AttachmentType.Widget;
            seedPathTextBox.RightWidget = seedPathButton;
            seedPathTextBox.LeftAttachment = AttachmentType.Widget;
            seedPathTextBox.LeftWidget = seedPathLabel;

            // setting
            tableLayoutPanel2 = new TonNurako.Widgets.Xm.Form();
            tableLayoutPanel2.TopAttachment = AttachmentType.Widget;
            tableLayoutPanel2.TopWidget = tableLayoutPanel1;
            tableLayoutPanel2.LeftAttachment =
            tableLayoutPanel2.RightAttachment = AttachmentType.Form;
            mainLayout.Children.Add(tableLayoutPanel2);

            settingPathLabel = new TonNurako.Widgets.Xm.Label();
            settingPathLabel.LabelString = "設定ファイル";
            settingPathTextBox = new TonNurako.Widgets.Xm.Text();
            settingPathTextBox.Width = 168;
            settingPathTextBox.ValueChangedEvent += settingPathTextBox_TextChanged;

            settingPathButton = new TonNurako.Widgets.Xm.PushButton();
            settingPathButton.LabelString = "開く...";
            settingPathButton.ActivateEvent += settingPathButton_Click;
            settingPathButton.RightAttachment = AttachmentType.Form;

            tableLayoutPanel2.Children.Add(settingPathLabel);
            tableLayoutPanel2.Children.Add(settingPathTextBox);
            tableLayoutPanel2.Children.Add(settingPathButton);

            settingPathTextBox.RightAttachment = AttachmentType.Widget;
            settingPathTextBox.RightWidget = settingPathButton;
            settingPathTextBox.LeftAttachment = AttachmentType.Widget;
            settingPathTextBox.LeftWidget = settingPathLabel;

            var xlay = new TonNurako.Widgets.Xm.Form();
            xlay.TopAttachment = AttachmentType.Widget;
            xlay.TopWidget = tableLayoutPanel2;
            xlay.LeftAttachment =
            xlay.RightAttachment =
            xlay.BottomAttachment = AttachmentType.Form;

            mainLayout.Children.Add(xlay);

            yamlToExcelGroupBox = new TonNurako.Widgets.Xm.Frame();
            yamlToExcelGroupBox.Width = 139;
            yamlToExcelGroupBox.Height = 139;

            var label1 = new Label();
            label1.FrameConstraint.ChildType = FrameChildType.TitleChild;
            label1.LabelString = "yml -> xlsx";
            yamlToExcelGroupBox.Children.Add(label1);
            xlay.Children.Add(yamlToExcelGroupBox);

            excelToYamlGroupBox = new TonNurako.Widgets.Xm.Frame();
            excelToYamlGroupBox.Width = 139;
            excelToYamlGroupBox.Height = 139;
            excelToYamlGroupBox.EntryVerticalAlignment = ContentsAlignment.Center;
            excelToYamlGroupBox.LeftAttachment = AttachmentType.Widget;
            excelToYamlGroupBox.LeftWidget = yamlToExcelGroupBox;
            excelToYamlGroupBox.TopAttachment = AttachmentType.Form;
            excelToYamlGroupBox.RightAttachment = AttachmentType.Form;
            excelToYamlGroupBox.BottomAttachment = AttachmentType.Form;

            var label2 = new Label();
            label2.FrameConstraint.ChildType = FrameChildType.TitleChild;
            label2.LabelString = "xlsx -> yml";
            excelToYamlGroupBox.Children.Add(label2);
            xlay.Children.Add(excelToYamlGroupBox);
            yamlToExcelGroupBox.LeftAttachment = AttachmentType.Form;
            yamlToExcelGroupBox.BottomAttachment = AttachmentType.Form;
            yamlToExcelGroupBox.TopAttachment = AttachmentType.Form;

            yamlToExcelArea = new TonNurako.Widgets.Xm.PushButton();
            yamlToExcelArea.LabelPixmap =
                Pixmap.FromBuffer(this, global::XmSeedtable.Properties.Resources.yamlToExcel);
            yamlToExcelArea.LabelType = LabelType.Pixmap;
            yamlToExcelGroupBox.Children.Add(yamlToExcelArea);

            excelToYamlArea = new TonNurako.Widgets.Xm.PushButton();
            excelToYamlArea.LabelPixmap =
                Pixmap.FromBuffer(this, global::XmSeedtable.Properties.Resources.excelToYaml);
            excelToYamlArea.LabelType = LabelType.Pixmap;
            excelToYamlArea.ActivateEvent += excelToYamlArea_Click;

            excelToYamlGroupBox.Children.Add(excelToYamlArea);
            ///yamlToExcelArea.LabelPixmap =
            ///    TonNurako.GC.XImage.FromBitmap(this, );


        }
        private TonNurako.Widgets.Xm.Form mainLayout;
        private TonNurako.Widgets.Xm.Form tableLayoutPanel2;
        private TonNurako.Widgets.Xm.Form tableLayoutPanel1;
        private TonNurako.Widgets.Xm.Label seedPathLabel;
        private TonNurako.Widgets.Xm.Text seedPathTextBox;
        private TonNurako.Widgets.Xm.PushButton seedPathButton;
        private TonNurako.Widgets.Xm.Label settingPathLabel;
        private TonNurako.Widgets.Xm.Text settingPathTextBox;
        private TonNurako.Widgets.Xm.PushButton settingPathButton;
        private TonNurako.Widgets.Xm.FileSelectionDialog seedFolderBrowserDialog;
        private TonNurako.Widgets.Xm.FileSelectionDialog  settingOpenFileDialog;
        private TonNurako.Widgets.Xm.FileSelectionDialog  excelFolderBrowserDialog;
        private TonNurako.Widgets.Xm.Frame yamlToExcelGroupBox;
        private TonNurako.Widgets.Xm.PushButton yamlToExcelArea;
        private  TonNurako.Widgets.Xm.Frame excelToYamlGroupBox;
        private TonNurako.Widgets.Xm.PushButton excelToYamlArea;
        private TonNurako.Widgets.Xm.FileSelectionDialog excelOpenFileDialog;
    }
}
