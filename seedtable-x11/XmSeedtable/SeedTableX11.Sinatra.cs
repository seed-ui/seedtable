using TonNurako.GC;
using TonNurako.Widgets;
using TonNurako.Widgets.Xm;

namespace XmSeedtable
{
    partial class SeedTableX11
    {
        private void Sinatra() {
            mainLayout = this.Layout;
            mainLayout.Width = 640;

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


            // source
            var tableLayoutPanel3 = new TonNurako.Widgets.Xm.Form();
            tableLayoutPanel3.TopAttachment = AttachmentType.Widget;
            tableLayoutPanel3.TopWidget = tableLayoutPanel2;
            tableLayoutPanel3.LeftAttachment =
            tableLayoutPanel3.RightAttachment = AttachmentType.Form;
            mainLayout.Children.Add(tableLayoutPanel3);

            var sourceGroup = new TonNurako.Widgets.Xm.Frame();
            sourceGroup.TopAttachment =
            sourceGroup.LeftAttachment =
            sourceGroup.RightAttachment =
            sourceGroup.BottomAttachment = AttachmentType.Form;

            var sourceGroupTitle = new Label();
            sourceGroupTitle.FrameConstraint.ChildType = FrameChildType.TitleChild;
            sourceGroupTitle.LabelString = "変換元";
            sourceGroup.Children.Add(sourceGroupTitle);
            tableLayoutPanel3.Children.Add(sourceGroup);

            var srcInnner = new Form();
            sourceGroup.Children.Add(srcInnner);

            var sourceLabel = new TonNurako.Widgets.Xm.Label();
            sourceLabel.LabelString = "フォルーダー";
            sourceTextBox = new TonNurako.Widgets.Xm.Text();
            sourceTextBox.Width = 168;

            var sourceButton = new TonNurako.Widgets.Xm.PushButton();
            sourceButton.LabelString = "開く...";
            sourceButton.ActivateEvent += sourceButton_Click;
            sourceButton.RightAttachment = AttachmentType.Form;

            srcInnner.Children.Add(sourceLabel);
            srcInnner.Children.Add(sourceButton);
            srcInnner.Children.Add(sourceTextBox);

            fileListBox = new ScrolledList();
            fileListBox.Height = 139;
            fileListBox.TopWidget = sourceTextBox;
            fileListBox.TopAttachment = AttachmentType.Widget;
            fileListBox.LeftAttachment =
            fileListBox.RightAttachment =
            fileListBox.BottomAttachment = AttachmentType.Form;
            srcInnner.Children.Add(fileListBox);

            sourceTextBox.RightAttachment = AttachmentType.Widget;
            sourceTextBox.RightWidget = sourceButton;
            sourceTextBox.LeftAttachment = AttachmentType.Widget;
            sourceTextBox.LeftWidget = sourceLabel;

            // ボタム
            var xlay = new TonNurako.Widgets.Xm.Form();
            xlay.TopAttachment = AttachmentType.Widget;
            xlay.TopWidget = tableLayoutPanel3;
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
            excelToYamlGroupBox.BottomAttachment = AttachmentType.Form;

            var label2 = new Label();
            label2.FrameConstraint.ChildType = FrameChildType.TitleChild;
            label2.LabelString = "xlsx -> yml";
            excelToYamlGroupBox.Children.Add(label2);
            xlay.Children.Add(excelToYamlGroupBox);
            yamlToExcelGroupBox.LeftAttachment = AttachmentType.Form;
            yamlToExcelGroupBox.BottomAttachment = AttachmentType.Form;
            yamlToExcelGroupBox.TopAttachment = AttachmentType.Form;

            yamlToExcelPixmap = Pixmap.FromBuffer(this, global::XmSeedtable.Properties.Resources.yamlToExcel);
            this.ToolkitResources.RetainCustomObject(yamlToExcelPixmap);

            yamlToExcelArea = new TonNurako.Widgets.Xm.PushButton();
            yamlToExcelArea.LabelType = LabelType.Pixmap;
            yamlToExcelArea.LabelPixmap = yamlToExcelPixmap;

            yamlToExcelArea.ActivateEvent += yamlToExcelArea_Click;
            yamlToExcelGroupBox.Children.Add(yamlToExcelArea);

            excelToYamlPixmap = Pixmap.FromBuffer(this, global::XmSeedtable.Properties.Resources.excelToYaml);
            this.ToolkitResources.RetainCustomObject(excelToYamlPixmap);

            excelToYamlArea = new TonNurako.Widgets.Xm.PushButton();
            excelToYamlArea.LabelType = LabelType.Pixmap;
            excelToYamlArea.LabelPixmap = excelToYamlPixmap;

            excelToYamlArea.ActivateEvent += excelToYamlArea_Click;

            excelToYamlGroupBox.Children.Add(excelToYamlArea);
        }

        private Pixmap yamlToExcelPixmap;
        private Pixmap excelToYamlPixmap;

        private TonNurako.Widgets.Xm.Form mainLayout;
        private TonNurako.Widgets.Xm.Form tableLayoutPanel2;
        private TonNurako.Widgets.Xm.Form tableLayoutPanel1;
        private TonNurako.Widgets.Xm.Label seedPathLabel;
        private TonNurako.Widgets.Xm.Text seedPathTextBox;
        private TonNurako.Widgets.Xm.PushButton seedPathButton;
        private TonNurako.Widgets.Xm.Label settingPathLabel;
        private TonNurako.Widgets.Xm.Text settingPathTextBox;
        private TonNurako.Widgets.Xm.PushButton settingPathButton;
        private TonNurako.Widgets.Xm.Frame yamlToExcelGroupBox;
        private TonNurako.Widgets.Xm.PushButton yamlToExcelArea;
        private  TonNurako.Widgets.Xm.Frame excelToYamlGroupBox;
        private TonNurako.Widgets.Xm.PushButton excelToYamlArea;

        private TonNurako.Widgets.Xm.Text sourceTextBox;
        private TonNurako.Widgets.Xm.ScrolledList fileListBox;

    }
}
