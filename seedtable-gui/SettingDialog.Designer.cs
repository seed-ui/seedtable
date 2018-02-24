namespace seedtable_gui {
    partial class SettingDialog {
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.engineComboBox = new System.Windows.Forms.ComboBox();
            this.basicOptionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataStartRowNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.columnNamesRowNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.engineLabel = new System.Windows.Forms.Label();
            this.columnNamesRowLabel = new System.Windows.Forms.Label();
            this.dataStartRowLabel = new System.Windows.Forms.Label();
            this.seedExtensionTextBox = new System.Windows.Forms.TextBox();
            this.seedExtensionLabel = new System.Windows.Forms.Label();
            this.calcFormulasCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteCheckBox = new System.Windows.Forms.CheckBox();
            this.formatLabel = new System.Windows.Forms.Label();
            this.formatComboBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ignoreTextBox = new System.Windows.Forms.TextBox();
            this.onlyTextBox = new System.Windows.Forms.TextBox();
            this.subdivideLabel = new System.Windows.Forms.Label();
            this.onlyLabel = new System.Windows.Forms.Label();
            this.ignoreLabel = new System.Windows.Forms.Label();
            this.subdivideTextBox = new System.Windows.Forms.TextBox();
            this.yamlColumnsTextBox = new System.Windows.Forms.TextBox();
            this.yamlColumnsLabel = new System.Windows.Forms.Label();
            this.ignoreColumnsLabel = new System.Windows.Forms.Label();
            this.ignoreColumnsTextBox = new System.Windows.Forms.TextBox();
            this.aliasLabel = new System.Windows.Forms.Label();
            this.aliasTextBox = new System.Windows.Forms.TextBox();
            this.mappingLabel = new System.Windows.Forms.Label();
            this.primaryLabel = new System.Windows.Forms.Label();
            this.mappingTextBox = new System.Windows.Forms.TextBox();
            this.primaryTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.mainTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.basicOptionsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataStartRowNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnNamesRowNumericUpDown)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.mainTableLayoutPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1161, 254);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTableLayoutPanel.ColumnCount = 2;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.mainTableLayoutPanel.Controls.Add(this.engineComboBox, 1, 0);
            this.mainTableLayoutPanel.Controls.Add(this.dataStartRowNumericUpDown, 1, 2);
            this.mainTableLayoutPanel.Controls.Add(this.columnNamesRowNumericUpDown, 1, 1);
            this.mainTableLayoutPanel.Controls.Add(this.engineLabel, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.columnNamesRowLabel, 0, 1);
            this.mainTableLayoutPanel.Controls.Add(this.dataStartRowLabel, 0, 2);
            this.mainTableLayoutPanel.Controls.Add(this.seedExtensionTextBox, 1, 3);
            this.mainTableLayoutPanel.Controls.Add(this.seedExtensionLabel, 0, 3);
            this.mainTableLayoutPanel.Controls.Add(this.calcFormulasCheckBox, 0, 6);
            this.mainTableLayoutPanel.Controls.Add(this.deleteCheckBox, 0, 5);
            this.mainTableLayoutPanel.Controls.Add(this.formatLabel, 0, 4);
            this.mainTableLayoutPanel.Controls.Add(this.formatComboBox, 1, 4);
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 7;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28816F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(244, 248);
            this.mainTableLayoutPanel.TabIndex = 1;
            // 
            // engineComboBox
            // 
            this.engineComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.engineComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.basicOptionsBindingSource, "engine", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.engineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.engineComboBox.FormattingEnabled = true;
            this.engineComboBox.Location = new System.Drawing.Point(149, 3);
            this.engineComboBox.Name = "engineComboBox";
            this.engineComboBox.Size = new System.Drawing.Size(92, 20);
            this.engineComboBox.TabIndex = 1;
            // 
            // basicOptionsBindingSource
            // 
            this.basicOptionsBindingSource.DataSource = typeof(SeedTable.BasicOptions);
            // 
            // dataStartRowNumericUpDown
            // 
            this.dataStartRowNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.basicOptionsBindingSource, "dataStartRow", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dataStartRowNumericUpDown.Location = new System.Drawing.Point(149, 73);
            this.dataStartRowNumericUpDown.Name = "dataStartRowNumericUpDown";
            this.dataStartRowNumericUpDown.Size = new System.Drawing.Size(38, 19);
            this.dataStartRowNumericUpDown.TabIndex = 5;
            // 
            // columnNamesRowNumericUpDown
            // 
            this.columnNamesRowNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.basicOptionsBindingSource, "columnNamesRow", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.columnNamesRowNumericUpDown.Location = new System.Drawing.Point(149, 38);
            this.columnNamesRowNumericUpDown.Name = "columnNamesRowNumericUpDown";
            this.columnNamesRowNumericUpDown.Size = new System.Drawing.Size(37, 19);
            this.columnNamesRowNumericUpDown.TabIndex = 3;
            // 
            // engineLabel
            // 
            this.engineLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.engineLabel.AutoSize = true;
            this.engineLabel.Location = new System.Drawing.Point(85, 0);
            this.engineLabel.Name = "engineLabel";
            this.engineLabel.Size = new System.Drawing.Size(58, 24);
            this.engineLabel.TabIndex = 0;
            this.engineLabel.Text = "エンジン\r\n(--engine)";
            this.engineLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // columnNamesRowLabel
            // 
            this.columnNamesRowLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.columnNamesRowLabel.AutoSize = true;
            this.columnNamesRowLabel.Location = new System.Drawing.Point(19, 35);
            this.columnNamesRowLabel.Name = "columnNamesRowLabel";
            this.columnNamesRowLabel.Size = new System.Drawing.Size(124, 24);
            this.columnNamesRowLabel.TabIndex = 2;
            this.columnNamesRowLabel.Text = "カラム名行\r\n(--column-names-row)";
            this.columnNamesRowLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dataStartRowLabel
            // 
            this.dataStartRowLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataStartRowLabel.AutoSize = true;
            this.dataStartRowLabel.Location = new System.Drawing.Point(42, 70);
            this.dataStartRowLabel.Name = "dataStartRowLabel";
            this.dataStartRowLabel.Size = new System.Drawing.Size(101, 24);
            this.dataStartRowLabel.TabIndex = 4;
            this.dataStartRowLabel.Text = "データ開始行\r\n(--data-start-row)";
            this.dataStartRowLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // seedExtensionTextBox
            // 
            this.seedExtensionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seedExtensionTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.basicOptionsBindingSource, "seedExtension", true));
            this.seedExtensionTextBox.Location = new System.Drawing.Point(149, 108);
            this.seedExtensionTextBox.Name = "seedExtensionTextBox";
            this.seedExtensionTextBox.Size = new System.Drawing.Size(92, 19);
            this.seedExtensionTextBox.TabIndex = 10;
            // 
            // seedExtensionLabel
            // 
            this.seedExtensionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.seedExtensionLabel.AutoSize = true;
            this.seedExtensionLabel.Location = new System.Drawing.Point(34, 105);
            this.seedExtensionLabel.Name = "seedExtensionLabel";
            this.seedExtensionLabel.Size = new System.Drawing.Size(109, 24);
            this.seedExtensionLabel.TabIndex = 9;
            this.seedExtensionLabel.Text = "seedファイルの拡張子(--seed-extension)";
            this.seedExtensionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // calcFormulasCheckBox
            // 
            this.calcFormulasCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calcFormulasCheckBox.AutoSize = true;
            this.mainTableLayoutPanel.SetColumnSpan(this.calcFormulasCheckBox, 2);
            this.calcFormulasCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.basicOptionsBindingSource, "calcFormulas", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.calcFormulasCheckBox.Location = new System.Drawing.Point(3, 213);
            this.calcFormulasCheckBox.Name = "calcFormulasCheckBox";
            this.calcFormulasCheckBox.Size = new System.Drawing.Size(238, 32);
            this.calcFormulasCheckBox.TabIndex = 8;
            this.calcFormulasCheckBox.Text = "yml→xlsx変換時に数式キャッシュを再計算\r\n(--calc-formulas)";
            this.calcFormulasCheckBox.UseVisualStyleBackColor = true;
            // 
            // deleteCheckBox
            // 
            this.deleteCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteCheckBox.AutoSize = true;
            this.mainTableLayoutPanel.SetColumnSpan(this.deleteCheckBox, 2);
            this.deleteCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.basicOptionsBindingSource, "delete", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.deleteCheckBox.Location = new System.Drawing.Point(3, 178);
            this.deleteCheckBox.Name = "deleteCheckBox";
            this.deleteCheckBox.Size = new System.Drawing.Size(238, 29);
            this.deleteCheckBox.TabIndex = 7;
            this.deleteCheckBox.Text = "変換元にないデータを削除する\r\n(--delete)";
            this.deleteCheckBox.UseVisualStyleBackColor = true;
            // 
            // formatLabel
            // 
            this.formatLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.formatLabel.AutoSize = true;
            this.formatLabel.Location = new System.Drawing.Point(54, 140);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(89, 24);
            this.formatLabel.TabIndex = 11;
            this.formatLabel.Text = "yamlのフォーマット\r\n(--format)";
            this.formatLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // formatComboBox
            // 
            this.formatComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formatComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.basicOptionsBindingSource, "format", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.formatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatComboBox.FormattingEnabled = true;
            this.formatComboBox.Location = new System.Drawing.Point(149, 143);
            this.formatComboBox.Name = "formatComboBox";
            this.formatComboBox.Size = new System.Drawing.Size(92, 20);
            this.formatComboBox.TabIndex = 12;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.50025F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.50025F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.50025F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.50025F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.50025F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.50025F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.49524F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.50328F));
            this.tableLayoutPanel2.Controls.Add(this.ignoreTextBox, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.onlyTextBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.subdivideLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.onlyLabel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.ignoreLabel, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.subdivideTextBox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.yamlColumnsTextBox, 7, 1);
            this.tableLayoutPanel2.Controls.Add(this.yamlColumnsLabel, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.ignoreColumnsLabel, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.ignoreColumnsTextBox, 6, 1);
            this.tableLayoutPanel2.Controls.Add(this.aliasLabel, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.aliasTextBox, 5, 1);
            this.tableLayoutPanel2.Controls.Add(this.mappingLabel, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.primaryLabel, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.mappingTextBox, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.primaryTextBox, 3, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(253, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(905, 248);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // ignoreTextBox
            // 
            this.ignoreTextBox.AcceptsReturn = true;
            this.ignoreTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ignoreTextBox.Location = new System.Drawing.Point(229, 28);
            this.ignoreTextBox.Multiline = true;
            this.ignoreTextBox.Name = "ignoreTextBox";
            this.ignoreTextBox.Size = new System.Drawing.Size(107, 217);
            this.ignoreTextBox.TabIndex = 6;
            // 
            // onlyTextBox
            // 
            this.onlyTextBox.AcceptsReturn = true;
            this.onlyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.onlyTextBox.Location = new System.Drawing.Point(116, 28);
            this.onlyTextBox.Multiline = true;
            this.onlyTextBox.Name = "onlyTextBox";
            this.onlyTextBox.Size = new System.Drawing.Size(107, 217);
            this.onlyTextBox.TabIndex = 5;
            // 
            // subdivideLabel
            // 
            this.subdivideLabel.AutoSize = true;
            this.subdivideLabel.Location = new System.Drawing.Point(3, 0);
            this.subdivideLabel.Name = "subdivideLabel";
            this.subdivideLabel.Size = new System.Drawing.Size(71, 24);
            this.subdivideLabel.TabIndex = 0;
            this.subdivideLabel.Text = "yml分割設定\r\n--subdivide";
            // 
            // onlyLabel
            // 
            this.onlyLabel.AutoSize = true;
            this.onlyLabel.Location = new System.Drawing.Point(116, 0);
            this.onlyLabel.Name = "onlyLabel";
            this.onlyLabel.Size = new System.Drawing.Size(106, 24);
            this.onlyLabel.TabIndex = 1;
            this.onlyLabel.Text = "このテーブルのみ変換\r\n--only";
            // 
            // ignoreLabel
            // 
            this.ignoreLabel.AutoSize = true;
            this.ignoreLabel.Location = new System.Drawing.Point(229, 0);
            this.ignoreLabel.Name = "ignoreLabel";
            this.ignoreLabel.Size = new System.Drawing.Size(94, 24);
            this.ignoreLabel.TabIndex = 2;
            this.ignoreLabel.Text = "このテーブルを無視\r\n--ignore";
            // 
            // subdivideTextBox
            // 
            this.subdivideTextBox.AcceptsReturn = true;
            this.subdivideTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subdivideTextBox.Location = new System.Drawing.Point(3, 28);
            this.subdivideTextBox.Multiline = true;
            this.subdivideTextBox.Name = "subdivideTextBox";
            this.subdivideTextBox.Size = new System.Drawing.Size(107, 217);
            this.subdivideTextBox.TabIndex = 4;
            // 
            // yamlColumnsTextBox
            // 
            this.yamlColumnsTextBox.AcceptsReturn = true;
            this.yamlColumnsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.yamlColumnsTextBox.Location = new System.Drawing.Point(794, 28);
            this.yamlColumnsTextBox.Multiline = true;
            this.yamlColumnsTextBox.Name = "yamlColumnsTextBox";
            this.yamlColumnsTextBox.Size = new System.Drawing.Size(108, 217);
            this.yamlColumnsTextBox.TabIndex = 9;
            // 
            // yamlColumnsLabel
            // 
            this.yamlColumnsLabel.AutoSize = true;
            this.yamlColumnsLabel.Location = new System.Drawing.Point(794, 0);
            this.yamlColumnsLabel.Name = "yamlColumnsLabel";
            this.yamlColumnsLabel.Size = new System.Drawing.Size(107, 24);
            this.yamlColumnsLabel.TabIndex = 8;
            this.yamlColumnsLabel.Text = "YAML扱いするカラム \r\n--yaml-columns";
            // 
            // ignoreColumnsLabel
            // 
            this.ignoreColumnsLabel.AutoSize = true;
            this.ignoreColumnsLabel.Location = new System.Drawing.Point(681, 0);
            this.ignoreColumnsLabel.Name = "ignoreColumnsLabel";
            this.ignoreColumnsLabel.Size = new System.Drawing.Size(96, 24);
            this.ignoreColumnsLabel.TabIndex = 3;
            this.ignoreColumnsLabel.Text = "このカラム名を無視\r\n--ignore-columns";
            // 
            // ignoreColumnsTextBox
            // 
            this.ignoreColumnsTextBox.AcceptsReturn = true;
            this.ignoreColumnsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ignoreColumnsTextBox.Location = new System.Drawing.Point(681, 28);
            this.ignoreColumnsTextBox.Multiline = true;
            this.ignoreColumnsTextBox.Name = "ignoreColumnsTextBox";
            this.ignoreColumnsTextBox.Size = new System.Drawing.Size(107, 217);
            this.ignoreColumnsTextBox.TabIndex = 7;
            // 
            // aliasLabel
            // 
            this.aliasLabel.AutoSize = true;
            this.aliasLabel.Location = new System.Drawing.Point(568, 0);
            this.aliasLabel.Name = "aliasLabel";
            this.aliasLabel.Size = new System.Drawing.Size(88, 24);
            this.aliasLabel.TabIndex = 12;
            this.aliasLabel.Text = "エイリアスシート名\r\n--alias";
            // 
            // aliasTextBox
            // 
            this.aliasTextBox.AcceptsReturn = true;
            this.aliasTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aliasTextBox.Location = new System.Drawing.Point(568, 28);
            this.aliasTextBox.Multiline = true;
            this.aliasTextBox.Name = "aliasTextBox";
            this.aliasTextBox.Size = new System.Drawing.Size(107, 217);
            this.aliasTextBox.TabIndex = 13;
            // 
            // mappingLabel
            // 
            this.mappingLabel.AutoSize = true;
            this.mappingLabel.Location = new System.Drawing.Point(455, 0);
            this.mappingLabel.Name = "mappingLabel";
            this.mappingLabel.Size = new System.Drawing.Size(105, 24);
            this.mappingLabel.TabIndex = 10;
            this.mappingLabel.Text = "ymlとシート名の対応\r\n--mapping";
            // 
            // primaryLabel
            // 
            this.primaryLabel.AutoSize = true;
            this.primaryLabel.Location = new System.Drawing.Point(342, 0);
            this.primaryLabel.Name = "primaryLabel";
            this.primaryLabel.Size = new System.Drawing.Size(57, 24);
            this.primaryLabel.TabIndex = 14;
            this.primaryLabel.Text = "優先シート\r\n--primary";
            // 
            // mappingTextBox
            // 
            this.mappingTextBox.AcceptsReturn = true;
            this.mappingTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mappingTextBox.Location = new System.Drawing.Point(455, 28);
            this.mappingTextBox.Multiline = true;
            this.mappingTextBox.Name = "mappingTextBox";
            this.mappingTextBox.Size = new System.Drawing.Size(107, 217);
            this.mappingTextBox.TabIndex = 11;
            // 
            // primaryTextBox
            // 
            this.primaryTextBox.AcceptsReturn = true;
            this.primaryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.primaryTextBox.Location = new System.Drawing.Point(342, 28);
            this.primaryTextBox.Multiline = true;
            this.primaryTextBox.Name = "primaryTextBox";
            this.primaryTextBox.Size = new System.Drawing.Size(107, 217);
            this.primaryTextBox.TabIndex = 15;
            // 
            // SettingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 254);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SettingDialog";
            this.Text = "設定";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.mainTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.basicOptionsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataStartRowNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnNamesRowNumericUpDown)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.ComboBox engineComboBox;
        private System.Windows.Forms.CheckBox deleteCheckBox;
        private System.Windows.Forms.NumericUpDown dataStartRowNumericUpDown;
        private System.Windows.Forms.NumericUpDown columnNamesRowNumericUpDown;
        private System.Windows.Forms.Label engineLabel;
        private System.Windows.Forms.Label columnNamesRowLabel;
        private System.Windows.Forms.Label dataStartRowLabel;
        private System.Windows.Forms.CheckBox calcFormulasCheckBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox ignoreColumnsTextBox;
        private System.Windows.Forms.TextBox ignoreTextBox;
        private System.Windows.Forms.TextBox onlyTextBox;
        private System.Windows.Forms.Label subdivideLabel;
        private System.Windows.Forms.Label onlyLabel;
        private System.Windows.Forms.Label ignoreLabel;
        private System.Windows.Forms.Label ignoreColumnsLabel;
        private System.Windows.Forms.TextBox subdivideTextBox;
        private System.Windows.Forms.BindingSource basicOptionsBindingSource;
        private System.Windows.Forms.Label yamlColumnsLabel;
        private System.Windows.Forms.TextBox yamlColumnsTextBox;
        private System.Windows.Forms.Label mappingLabel;
        private System.Windows.Forms.TextBox mappingTextBox;
        private System.Windows.Forms.Label seedExtensionLabel;
        private System.Windows.Forms.TextBox seedExtensionTextBox;
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.ComboBox formatComboBox;
        private System.Windows.Forms.TextBox aliasTextBox;
        private System.Windows.Forms.Label aliasLabel;
        private System.Windows.Forms.TextBox primaryTextBox;
        private System.Windows.Forms.Label primaryLabel;
    }
}