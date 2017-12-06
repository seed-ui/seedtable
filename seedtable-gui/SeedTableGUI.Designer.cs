namespace seedtable_gui {
    partial class SeedTableGUI {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeedTableGUI));
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.yamlToExcelGroupBox = new System.Windows.Forms.GroupBox();
            this.yamlToExcelArea = new System.Windows.Forms.PictureBox();
            this.excelToYamlGroupBox = new System.Windows.Forms.GroupBox();
            this.excelToYamlArea = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.seedPathLabel = new System.Windows.Forms.Label();
            this.seedPathTextBox = new System.Windows.Forms.TextBox();
            this.seedPathButton = new System.Windows.Forms.Button();
            this.settingPathTextBox = new System.Windows.Forms.TextBox();
            this.settingPathButton = new System.Windows.Forms.Button();
            this.settingButton = new System.Windows.Forms.Button();
            this.seedFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.settingOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.dataExcelFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.dataExcelOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.templateExcelOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mainLayout.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.yamlToExcelGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yamlToExcelArea)).BeginInit();
            this.excelToYamlGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.excelToYamlArea)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.AutoSize = true;
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainLayout.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.mainLayout.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 2;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(296, 228);
            this.mainLayout.TabIndex = 0;
            this.mainLayout.Paint += new System.Windows.Forms.PaintEventHandler(this.mainLayout_Paint);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.yamlToExcelGroupBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.excelToYamlGroupBox, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 83);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(290, 142);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // yamlToExcelGroupBox
            // 
            this.yamlToExcelGroupBox.Controls.Add(this.yamlToExcelArea);
            this.yamlToExcelGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.yamlToExcelGroupBox.Location = new System.Drawing.Point(3, 3);
            this.yamlToExcelGroupBox.Name = "yamlToExcelGroupBox";
            this.yamlToExcelGroupBox.Size = new System.Drawing.Size(139, 136);
            this.yamlToExcelGroupBox.TabIndex = 4;
            this.yamlToExcelGroupBox.TabStop = false;
            this.yamlToExcelGroupBox.Text = "yml → xlsx";
            // 
            // yamlToExcelArea
            // 
            this.yamlToExcelArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.yamlToExcelArea.Image = global::seedtable_gui.Properties.Resources.yamlToExcel;
            this.yamlToExcelArea.Location = new System.Drawing.Point(3, 15);
            this.yamlToExcelArea.Name = "yamlToExcelArea";
            this.yamlToExcelArea.Size = new System.Drawing.Size(133, 118);
            this.yamlToExcelArea.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.yamlToExcelArea.TabIndex = 3;
            this.yamlToExcelArea.TabStop = false;
            this.yamlToExcelArea.DragDrop += new System.Windows.Forms.DragEventHandler(this.yamlToExcelArea_DragDrop);
            this.yamlToExcelArea.DragEnter += new System.Windows.Forms.DragEventHandler(this.yamlToExcelArea_DragEnter);
            this.yamlToExcelArea.DoubleClick += new System.EventHandler(this.yamlToExcelArea_DoubleClick);
            // 
            // excelToYamlGroupBox
            // 
            this.excelToYamlGroupBox.Controls.Add(this.excelToYamlArea);
            this.excelToYamlGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.excelToYamlGroupBox.Location = new System.Drawing.Point(148, 3);
            this.excelToYamlGroupBox.Name = "excelToYamlGroupBox";
            this.excelToYamlGroupBox.Size = new System.Drawing.Size(139, 136);
            this.excelToYamlGroupBox.TabIndex = 5;
            this.excelToYamlGroupBox.TabStop = false;
            this.excelToYamlGroupBox.Text = "xlsx → yml";
            // 
            // excelToYamlArea
            // 
            this.excelToYamlArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.excelToYamlArea.Image = global::seedtable_gui.Properties.Resources.excelToYaml;
            this.excelToYamlArea.Location = new System.Drawing.Point(3, 15);
            this.excelToYamlArea.Name = "excelToYamlArea";
            this.excelToYamlArea.Size = new System.Drawing.Size(133, 118);
            this.excelToYamlArea.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.excelToYamlArea.TabIndex = 4;
            this.excelToYamlArea.TabStop = false;
            this.excelToYamlArea.DragDrop += new System.Windows.Forms.DragEventHandler(this.excelToYamlArea_DragDrop);
            this.excelToYamlArea.DragEnter += new System.Windows.Forms.DragEventHandler(this.excelToYamlArea_DragEnter);
            this.excelToYamlArea.DoubleClick += new System.EventHandler(this.excelToYamlArea_DoubleClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.seedPathLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.seedPathTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.seedPathButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.settingPathTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.settingPathButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.settingButton, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(290, 74);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // seedPathLabel
            // 
            this.seedPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.seedPathLabel.AutoSize = true;
            this.seedPathLabel.Location = new System.Drawing.Point(3, 6);
            this.seedPathLabel.Name = "seedPathLabel";
            this.seedPathLabel.Size = new System.Drawing.Size(52, 24);
            this.seedPathLabel.TabIndex = 0;
            this.seedPathLabel.Text = "seedフォルダ";
            // 
            // seedPathTextBox
            // 
            this.seedPathTextBox.AllowDrop = true;
            this.seedPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.seedPathTextBox.Location = new System.Drawing.Point(61, 9);
            this.seedPathTextBox.Name = "seedPathTextBox";
            this.seedPathTextBox.Size = new System.Drawing.Size(168, 19);
            this.seedPathTextBox.TabIndex = 1;
            this.seedPathTextBox.TextChanged += new System.EventHandler(this.seedPathTextBox_TextChanged);
            this.seedPathTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.seedPathTextBox_DragDrop);
            this.seedPathTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.seedPathTextBox_DragEnter);
            // 
            // seedPathButton
            // 
            this.seedPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.seedPathButton.Location = new System.Drawing.Point(235, 7);
            this.seedPathButton.Name = "seedPathButton";
            this.seedPathButton.Size = new System.Drawing.Size(52, 23);
            this.seedPathButton.TabIndex = 2;
            this.seedPathButton.Text = "開く";
            this.seedPathButton.UseVisualStyleBackColor = true;
            this.seedPathButton.Click += new System.EventHandler(this.seedPathButton_Click);
            // 
            // settingPathTextBox
            // 
            this.settingPathTextBox.AllowDrop = true;
            this.settingPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.settingPathTextBox.Location = new System.Drawing.Point(61, 46);
            this.settingPathTextBox.Name = "settingPathTextBox";
            this.settingPathTextBox.Size = new System.Drawing.Size(168, 19);
            this.settingPathTextBox.TabIndex = 4;
            this.settingPathTextBox.TextChanged += new System.EventHandler(this.settingPathTextBox_TextChanged);
            this.settingPathTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.settingPathTextBox_DragDrop);
            this.settingPathTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.settingPathTextBox_DragEnter);
            // 
            // settingPathButton
            // 
            this.settingPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.settingPathButton.Location = new System.Drawing.Point(235, 44);
            this.settingPathButton.Name = "settingPathButton";
            this.settingPathButton.Size = new System.Drawing.Size(52, 23);
            this.settingPathButton.TabIndex = 5;
            this.settingPathButton.Text = "開く";
            this.settingPathButton.UseVisualStyleBackColor = true;
            this.settingPathButton.Click += new System.EventHandler(this.settingPathButton_Click);
            // 
            // settingButton
            // 
            this.settingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.settingButton.Location = new System.Drawing.Point(3, 44);
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(52, 23);
            this.settingButton.TabIndex = 6;
            this.settingButton.Text = "設定";
            this.settingButton.UseVisualStyleBackColor = true;
            this.settingButton.Click += new System.EventHandler(this.settingButton_Click);
            // 
            // seedFolderBrowserDialog
            // 
            this.seedFolderBrowserDialog.Description = "seedフォルダ";
            this.seedFolderBrowserDialog.ShowNewFolderButton = false;
            // 
            // settingOpenFileDialog
            // 
            this.settingOpenFileDialog.Filter = "YAMLファイル(*.yml,*.yaml)|*.yml;*.yaml|JSONファイル(*.json)|*.json";
            this.settingOpenFileDialog.ReadOnlyChecked = true;
            this.settingOpenFileDialog.Title = "設定ファイルを開く";
            // 
            // dataExcelFolderBrowserDialog
            // 
            this.dataExcelFolderBrowserDialog.Description = "Excelファイルを保存するフォルダ";
            // 
            // dataExcelOpenFileDialog
            // 
            this.dataExcelOpenFileDialog.Filter = "Excelファイル|*.xlsx;*.xlsm";
            this.dataExcelOpenFileDialog.Multiselect = true;
            this.dataExcelOpenFileDialog.ReadOnlyChecked = true;
            // 
            // templateExcelOpenFileDialog
            // 
            this.templateExcelOpenFileDialog.Filter = "Excelファイル|*.xlsx;*.xlsm";
            this.templateExcelOpenFileDialog.Multiselect = true;
            this.templateExcelOpenFileDialog.ReadOnlyChecked = true;
            // 
            // SeedTableGUI
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 228);
            this.Controls.Add(this.mainLayout);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SeedTableGUI";
            this.Text = "SeedTableGUI";
            this.Load += new System.EventHandler(this.SeedTableGUI_Load);
            this.mainLayout.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.yamlToExcelGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.yamlToExcelArea)).EndInit();
            this.excelToYamlGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.excelToYamlArea)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label seedPathLabel;
        private System.Windows.Forms.TextBox seedPathTextBox;
        private System.Windows.Forms.Button seedPathButton;
        private System.Windows.Forms.TextBox settingPathTextBox;
        private System.Windows.Forms.Button settingPathButton;
        private System.Windows.Forms.FolderBrowserDialog seedFolderBrowserDialog;
        private System.Windows.Forms.OpenFileDialog settingOpenFileDialog;
        private System.Windows.Forms.FolderBrowserDialog dataExcelFolderBrowserDialog;
        private System.Windows.Forms.GroupBox yamlToExcelGroupBox;
        private System.Windows.Forms.PictureBox yamlToExcelArea;
        private System.Windows.Forms.GroupBox excelToYamlGroupBox;
        private System.Windows.Forms.PictureBox excelToYamlArea;
        private System.Windows.Forms.OpenFileDialog dataExcelOpenFileDialog;
        private System.Windows.Forms.OpenFileDialog templateExcelOpenFileDialog;
        private System.Windows.Forms.Button settingButton;
    }
}