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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.engineComboBox = new System.Windows.Forms.ComboBox();
            this.deleteCheckBox = new System.Windows.Forms.CheckBox();
            this.dataStartRowNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.columnNamesRowNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.engineLabel = new System.Windows.Forms.Label();
            this.columnNamesRowLabel = new System.Windows.Forms.Label();
            this.dataStartRowLabel = new System.Windows.Forms.Label();
            this.calcFormulasCheckBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.subdivideLabel = new System.Windows.Forms.Label();
            this.onlyLabel = new System.Windows.Forms.Label();
            this.ignoreLabel = new System.Windows.Forms.Label();
            this.ignoreColumnsLabel = new System.Windows.Forms.Label();
            this.subdivideTextBox = new System.Windows.Forms.TextBox();
            this.onlyTextBox = new System.Windows.Forms.TextBox();
            this.ignoreTextBox = new System.Windows.Forms.TextBox();
            this.ignoreColumnsTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.mainTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataStartRowNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnNamesRowNumericUpDown)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 270F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.mainTableLayoutPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(684, 181);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTableLayoutPanel.ColumnCount = 2;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTableLayoutPanel.Controls.Add(this.engineComboBox, 1, 0);
            this.mainTableLayoutPanel.Controls.Add(this.deleteCheckBox, 0, 3);
            this.mainTableLayoutPanel.Controls.Add(this.dataStartRowNumericUpDown, 1, 2);
            this.mainTableLayoutPanel.Controls.Add(this.columnNamesRowNumericUpDown, 1, 1);
            this.mainTableLayoutPanel.Controls.Add(this.engineLabel, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.columnNamesRowLabel, 0, 1);
            this.mainTableLayoutPanel.Controls.Add(this.dataStartRowLabel, 0, 2);
            this.mainTableLayoutPanel.Controls.Add(this.calcFormulasCheckBox, 0, 4);
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 5;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(264, 175);
            this.mainTableLayoutPanel.TabIndex = 1;
            // 
            // engineComboBox
            // 
            this.engineComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.engineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.engineComboBox.FormattingEnabled = true;
            this.engineComboBox.Location = new System.Drawing.Point(135, 3);
            this.engineComboBox.Name = "engineComboBox";
            this.engineComboBox.Size = new System.Drawing.Size(126, 20);
            this.engineComboBox.TabIndex = 1;
            // 
            // deleteCheckBox
            // 
            this.deleteCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteCheckBox.AutoSize = true;
            this.mainTableLayoutPanel.SetColumnSpan(this.deleteCheckBox, 2);
            this.deleteCheckBox.Location = new System.Drawing.Point(3, 108);
            this.deleteCheckBox.Name = "deleteCheckBox";
            this.deleteCheckBox.Size = new System.Drawing.Size(258, 29);
            this.deleteCheckBox.TabIndex = 7;
            this.deleteCheckBox.Text = "yml→xlsx変換時に行削除を行う\r\n(--delete)";
            this.deleteCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataStartRowNumericUpDown
            // 
            this.dataStartRowNumericUpDown.Location = new System.Drawing.Point(135, 73);
            this.dataStartRowNumericUpDown.Name = "dataStartRowNumericUpDown";
            this.dataStartRowNumericUpDown.Size = new System.Drawing.Size(38, 19);
            this.dataStartRowNumericUpDown.TabIndex = 5;
            // 
            // columnNamesRowNumericUpDown
            // 
            this.columnNamesRowNumericUpDown.Location = new System.Drawing.Point(135, 38);
            this.columnNamesRowNumericUpDown.Name = "columnNamesRowNumericUpDown";
            this.columnNamesRowNumericUpDown.Size = new System.Drawing.Size(37, 19);
            this.columnNamesRowNumericUpDown.TabIndex = 3;
            // 
            // engineLabel
            // 
            this.engineLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.engineLabel.AutoSize = true;
            this.engineLabel.Location = new System.Drawing.Point(71, 0);
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
            this.columnNamesRowLabel.Location = new System.Drawing.Point(5, 35);
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
            this.dataStartRowLabel.Location = new System.Drawing.Point(28, 70);
            this.dataStartRowLabel.Name = "dataStartRowLabel";
            this.dataStartRowLabel.Size = new System.Drawing.Size(101, 24);
            this.dataStartRowLabel.TabIndex = 4;
            this.dataStartRowLabel.Text = "データ開始行\r\n(--data-start-row)";
            this.dataStartRowLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // calcFormulasCheckBox
            // 
            this.calcFormulasCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calcFormulasCheckBox.AutoSize = true;
            this.mainTableLayoutPanel.SetColumnSpan(this.calcFormulasCheckBox, 2);
            this.calcFormulasCheckBox.Location = new System.Drawing.Point(3, 143);
            this.calcFormulasCheckBox.Name = "calcFormulasCheckBox";
            this.calcFormulasCheckBox.Size = new System.Drawing.Size(258, 29);
            this.calcFormulasCheckBox.TabIndex = 8;
            this.calcFormulasCheckBox.Text = "yml→xlsx変換時に数式キャッシュを再計算する\r\n(--calc-formulas)";
            this.calcFormulasCheckBox.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.ignoreColumnsTextBox, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.ignoreTextBox, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.onlyTextBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.subdivideLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.onlyLabel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.ignoreLabel, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.ignoreColumnsLabel, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.subdivideTextBox, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(273, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(408, 175);
            this.tableLayoutPanel2.TabIndex = 9;
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
            this.onlyLabel.Location = new System.Drawing.Point(105, 0);
            this.onlyLabel.Name = "onlyLabel";
            this.onlyLabel.Size = new System.Drawing.Size(96, 24);
            this.onlyLabel.TabIndex = 1;
            this.onlyLabel.Text = "このシートのみ変換\r\n--only";
            // 
            // ignoreLabel
            // 
            this.ignoreLabel.AutoSize = true;
            this.ignoreLabel.Location = new System.Drawing.Point(207, 0);
            this.ignoreLabel.Name = "ignoreLabel";
            this.ignoreLabel.Size = new System.Drawing.Size(84, 24);
            this.ignoreLabel.TabIndex = 2;
            this.ignoreLabel.Text = "このシートを無視\r\n--ignore";
            // 
            // ignoreColumnsLabel
            // 
            this.ignoreColumnsLabel.AutoSize = true;
            this.ignoreColumnsLabel.Location = new System.Drawing.Point(309, 0);
            this.ignoreColumnsLabel.Name = "ignoreColumnsLabel";
            this.ignoreColumnsLabel.Size = new System.Drawing.Size(96, 24);
            this.ignoreColumnsLabel.TabIndex = 3;
            this.ignoreColumnsLabel.Text = "このカラム名を無視\r\n--ignore-columns";
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
            this.subdivideTextBox.Size = new System.Drawing.Size(96, 144);
            this.subdivideTextBox.TabIndex = 4;
            // 
            // onlyTextBox
            // 
            this.onlyTextBox.AcceptsReturn = true;
            this.onlyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.onlyTextBox.Location = new System.Drawing.Point(105, 28);
            this.onlyTextBox.Multiline = true;
            this.onlyTextBox.Name = "onlyTextBox";
            this.onlyTextBox.Size = new System.Drawing.Size(96, 144);
            this.onlyTextBox.TabIndex = 5;
            // 
            // ignoreTextBox
            // 
            this.ignoreTextBox.AcceptsReturn = true;
            this.ignoreTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ignoreTextBox.Location = new System.Drawing.Point(207, 28);
            this.ignoreTextBox.Multiline = true;
            this.ignoreTextBox.Name = "ignoreTextBox";
            this.ignoreTextBox.Size = new System.Drawing.Size(96, 144);
            this.ignoreTextBox.TabIndex = 6;
            // 
            // ignoreColumnsTextBox
            // 
            this.ignoreColumnsTextBox.AcceptsReturn = true;
            this.ignoreColumnsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ignoreColumnsTextBox.Location = new System.Drawing.Point(309, 28);
            this.ignoreColumnsTextBox.Multiline = true;
            this.ignoreColumnsTextBox.Name = "ignoreColumnsTextBox";
            this.ignoreColumnsTextBox.Size = new System.Drawing.Size(96, 144);
            this.ignoreColumnsTextBox.TabIndex = 7;
            // 
            // SettingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 181);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SettingDialog";
            this.Text = "設定";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.mainTableLayoutPanel.PerformLayout();
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
    }
}