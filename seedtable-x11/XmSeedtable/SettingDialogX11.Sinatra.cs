using System;
using System.Collections.Generic;
using System.Text;
using TonNurako.Widgets.Xm;

namespace XmSeedtable
{
    public partial class SettingDialogX11
    {
        delegate Widget Delegaty(Widget parent);
        struct LeftControls {
            public string Label;
            public Delegaty Delegaty;

            public LeftControls(string l, Delegaty d) {
                Label = l;
                Delegaty = d;
            }
        }


        private Widget WgtEngine(Widget rc) {
            engineComboBox = new SimpleOptionMenu();
            var engine = new List<string>();
            int i = 0;
            int v = 0;
            foreach(SeedTable.CommonOptions.Engine e in  Enum.GetValues(typeof(SeedTable.CommonOptions.Engine))) {
                engine.Add(e.ToString());
                if (Options.engine == e) {
                    v = i;
                }
                i++;
            }
            engineComboBox.ButtonSet = v;
            engineComboBox.ButtonCount = engine.Count;
            engineComboBox.Buttons = engine.ToArray();
            engineComboBox.SimpleEvent += (x,y) => {
                Options.engine = (SeedTable.CommonOptions.Engine)y.SelectedIndex;
            };
            return engineComboBox;
        }


        private Widget WgtColumn(Widget rc) {
            columnNamesRowNumericUpDown = new SimpleSpinBox();
            columnNamesRowNumericUpDown.SpinBoxChildType = SpinBoxChildType.Numeric;
            columnNamesRowNumericUpDown.MinimumValue = 0;
            columnNamesRowNumericUpDown.MaximumValue = 999;
            columnNamesRowNumericUpDown.Columns = 4;
            columnNamesRowNumericUpDown.Position = Options.columnNamesRow;

            columnNamesRowNumericUpDown.ValueChangedEvent += (x,y) => {
                Options.columnNamesRow = columnNamesRowNumericUpDown.Position;
            };
            return columnNamesRowNumericUpDown;
        }


        private Widget WgtDataRow(Widget rc) {
            dataStartRowNumericUpDown = new SimpleSpinBox();
            dataStartRowNumericUpDown.SpinBoxChildType = SpinBoxChildType.Numeric;
            dataStartRowNumericUpDown.MinimumValue = 0;
            dataStartRowNumericUpDown.MaximumValue = 999;
            dataStartRowNumericUpDown.Columns = 4;
            dataStartRowNumericUpDown.Position = Options.dataStartRow;

            dataStartRowNumericUpDown.ValueChangedEvent += (x,y) => {
                Options.dataStartRow = dataStartRowNumericUpDown.Position;
            };

            return dataStartRowNumericUpDown;
        }

        private Widget WgtText(Widget rc, out Text rb, IEnumerable<string> val) {
            rb = new Text();
            rb.EditMode = EditMode.Multi;
            rb.Rows = 10;
            rb.Columns = 10;
            StringBuilder b = new StringBuilder();
            foreach(var s in val) {
                b.Append(s).Append("\n");
            }
            rb.Value = b.ToString().TrimEnd();

            return rb;
        }

        private void Sinatra() {
            var form = new Form();
            form.MarginHeight = 2;
            form.MarginWidth = 2;
            this.Children.Add(form);

            var cs = new LeftControls[] {
                new LeftControls( "エンジン\n(--engine)", WgtEngine),
                new LeftControls( "カラム名行\n(--column-names-row)", WgtColumn),
                new LeftControls( "データ開始行\n(--data-start-row)", WgtDataRow),
            };

            var vb6 = new Delegaty[] {
                    (X) => {
                        var msc = new SimpleCheckBox();
                        deleteCheckBox = new ToggleButtonGadget();
                        deleteCheckBox.LabelString = "yml→xlsx変換時に行削除を行う\n(--delete)";
                        deleteCheckBox.Set = Options.delete ? ToggleButtonState.Set : ToggleButtonState.Unset;
                        deleteCheckBox.ValueChangedEvent += (x,y) => {
                            Options.delete = deleteCheckBox.Set == ToggleButtonState.Set;
                        };
                        msc.Children.Add(deleteCheckBox);
                        return msc;
                    },
                    (X) => {
                        var msc = new SimpleCheckBox();
                        calcFormulasCheckBox = new ToggleButtonGadget();
                        calcFormulasCheckBox.LabelString = "yml→xlsx変換時に数式キャッシュを再計算する\n(--calc-formulas)";
                        calcFormulasCheckBox.Set = Options.calcFormulas ? ToggleButtonState.Set : ToggleButtonState.Unset;
                        calcFormulasCheckBox.ValueChangedEvent += (x,y) => {
                            Options.calcFormulas = deleteCheckBox.Set == ToggleButtonState.Set;
                        };
                        msc.Children.Add(calcFormulasCheckBox);
                        return msc;
                    }
            };

            var rc = new RowColumn();
            rc.Packing = Packing.Column;
            rc.NumColumns = cs.Length+1;
            rc.Orientation = Orientation.Horizontal;
            rc.IsAligned = true;
            rc.EntryAlignment = Alignment.End;
            form.Children.Add(rc);
            foreach(var c in cs) {
                var l = new LabelGadget();
                l.LabelString = c.Label;
                rc.Children.Add(l);
                if(null != c.Delegaty) {
                    var x = c.Delegaty(rc);
                    rc.Children.Add(x);
                }
            }
            rc.TopAttachment = AttachmentType.Form;
            rc.LeftAttachment = AttachmentType.Form;

            var rd = new RowColumn();
            rd.NumColumns = vb6.Length+1;
            rd.Orientation = Orientation.Vertical;
            rd.IsAligned = true;
            rd.EntryAlignment = Alignment.End;
            form.Children.Add(rd);
            foreach(var c in vb6) {
                var x = c(rd);
                rd.Children.Add(x);
            }
            rd.TopAttachment = AttachmentType.Widget;
            rd.TopWidget = rc;

            var vb5 = new LeftControls[] {
                new LeftControls( "yml分割設定\n--subdivide", (x) =>{
                        return WgtText(x, out subdivideTextBox, Options.subdivide);
                    }),
                new LeftControls( "このシートのみ変換\n--only", (x) =>{
                        return WgtText(x, out onlyTextBox, Options.only);
                    }),
                new LeftControls( "このシートを無視\n--ignore", (x) =>{
                        return WgtText(x, out ignoreTextBox, Options.ignore);
                    }),
                new LeftControls( "このカラム名を無視\n--ignore-columns", (x) =>{
                        return WgtText(x, out ignoreColumnsTextBox, Options.ignoreColumns);
                    }),
                new LeftControls( "YAMLにするカラム\n--yaml-columns", (x) =>{
                        return WgtText(x, out yamlColumnsTextBox, Options.yamlColumns);
                    }),
            };
            var re = new Form();
            re.FractionBase = vb5.Length;
            form.Children.Add(re);

            re.TopAttachment = AttachmentType.Widget;
            re.TopWidget = rd;
            re.BottomAttachment = AttachmentType.Form;
            re.RightAttachment = AttachmentType.Form;
            for (int i = 0; i <vb5.Length; ++i) {
                var c = vb5[i];
                var frx = new Form();
                frx.LeftAttachment = AttachmentType.Position;
                frx.LeftPosition = i;
                frx.RightAttachment = AttachmentType.Position;
                frx.RightPosition = i+1;
                frx.TopAttachment =
                frx.BottomAttachment = AttachmentType.Form;

                var k = new Label();
                k.LabelString = c.Label;
                frx.Children.Add(k);

                var t = c.Delegaty(re);
                frx.Children.Add(t);
                re.Children.Add(frx);
                t.TopAttachment = AttachmentType.Widget;
                t.TopWidget = k;
                t.LeftAttachment =
                t.RightAttachment =
                t.BottomAttachment = AttachmentType.Form;
            }

            var buttonBase = new RowColumn();
            buttonBase.TopAttachment = AttachmentType.Form;
            buttonBase.RightAttachment = AttachmentType.Form;
            buttonBase.Orientation = Orientation.Horizontal;
            form.Children.Add(buttonBase);


            okButton = new PushButton();
            okButton.LabelString = "保存";
            okButton.ShowAsDefault = true;
            okButton.Alignment = Alignment.Center;

            okButton.ActivateEvent += (z,p) => {
                SaveOptions();
                this.Destroy();
            };
            buttonBase.Children.Add(okButton);

            discardButton = new PushButton();
            discardButton.TopAttachment = AttachmentType.Widget;
            discardButton.TopWidget = okButton;
            discardButton.RightAttachment = AttachmentType.Form;
            discardButton.LabelString = "取消";
            discardButton.Alignment = Alignment.Center;
            discardButton.ActivateEvent += (x,y) => {
                this.Destroy();
            };
            buttonBase.Children.Add(discardButton);

            // 読み取り専用
            if (!Changable) {
                deleteCheckBox.Sensitive              =
                calcFormulasCheckBox.Sensitive        =
                yamlColumnsTextBox.Sensitive          =
                ignoreColumnsTextBox.Sensitive        =
                ignoreTextBox.Sensitive               =
                onlyTextBox.Sensitive                 =
                subdivideTextBox.Sensitive            =
                dataStartRowNumericUpDown.Sensitive   =
                engineComboBox.Sensitive              =
                okButton.Sensitive                    =
                columnNamesRowNumericUpDown.Sensitive = false;
            }
        }
        ToggleButtonGadget deleteCheckBox;
        ToggleButtonGadget calcFormulasCheckBox;
        Text yamlColumnsTextBox;
        Text ignoreColumnsTextBox;
        Text ignoreTextBox;
        Text onlyTextBox;
        Text subdivideTextBox;
        SimpleSpinBox dataStartRowNumericUpDown;
        SimpleOptionMenu engineComboBox;
        SimpleSpinBox columnNamesRowNumericUpDown;
        TonNurako.Widgets.Xm.PushButton okButton;
        TonNurako.Widgets.Xm.PushButton discardButton;

    }
}
