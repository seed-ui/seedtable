using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TonNurako.Data;
using TonNurako.Widgets;
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
            var cb = new SimpleOptionMenu();
            List<string> engine = new List<string>();
            foreach(SeedTable.CommonOptions.Engine e in  Enum.GetValues(typeof(SeedTable.CommonOptions.Engine))) {
                engine.Add(e.ToString());
            }
            cb.ButtonCount = engine.Count;
            cb.Buttons = engine.ToArray();
            return cb;
        }

        private Widget WgtColumn(Widget rc) {
            var cb = new SimpleSpinBox();
            cb.SpinBoxChildType = SpinBoxChildType.Numeric;
            cb.MinimumValue = 2;
            cb.Columns = 4;

            return cb;
        }

        private Widget WgtDataRow(Widget rc) {
            var cb = new SimpleSpinBox();
            cb.SpinBoxChildType = SpinBoxChildType.Numeric;
            cb.MinimumValue = 3;
            cb.Columns = 4;
            return cb;
        }

        private Widget WgtText(Widget rc) {
            var cb = new Text();
            cb.EditMode = EditMode.Multi;
            return cb;
        }

        private void Sinatra() {
            var form = new Form();
            form.Width = 640;
            form.Height = 320;
            //form.ResizePolicy = ResizePolicy.Grow;
            form.MarginHeight = 10;
            form.MarginWidth = 10;
            this.Children.Add(form);

            var cs = new LeftControls[] {
                new LeftControls( "エンジン\n(--engine)", WgtEngine),
                new LeftControls( "カラム名行\n(--column-names-row)", WgtColumn),
                new LeftControls( "データ開始行\n(--data-start-row)", WgtDataRow),
                /*new LeftControls( "yml→xlsx変換時に行削除を行う\r\n(--delete)",
                    (X) => {
                        var k = new ToggleButtonGadget();
                        k.LabelString = "";
                        return k;
                    }),
                new LeftControls( "yml→xlsx変換時に数式キャッシュを再計算する\r\n(--calc-formulas)",
                    (X) => {
                        var k = new ToggleButtonGadget();
                        k.LabelString = "";
                        return k;
                    }),*/
            };

            var vb6 = new Delegaty[] {
                    (X) => {
                        var msc = new SimpleCheckBox();
                        var k = new ToggleButtonGadget();
                        k.LabelString = "yml→xlsx変換時に行削除を行う\n(--delete)";
                        msc.Children.Add(k);
                        return msc;
                    },
                    (X) => {
                        var msc = new SimpleCheckBox();
                        var k = new ToggleButtonGadget();
                        k.LabelString = "yml→xlsx変換時に数式キャッシュを再計算する\n(--calc-formulas)";
                        msc.Children.Add(k);
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
            //rd.BottomAttachment = AttachmentType.Form;


            var vb5 = new LeftControls[] {
                new LeftControls( "yml分割設定\n--subdivide", WgtText),
                new LeftControls( "このシートのみ変換\n--only", WgtText),
                new LeftControls( "このシートを無視\n--ignore", WgtText),
                new LeftControls( "このカラム名を無視\n--ignore-columns", WgtText),
            };
            var re = new Form();
            //re.BorderWidth = 2;
            //re.NumColumns = vb5.Length+1;
            //re.Orientation = Orientation.Horizontal;
            //re.IsAligned = true;
            //re.EntryAlignment = Alignment.End;
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
                //frx.BorderWidth = 2;

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

            okButton = new PushButton();
            okButton.LabelString = "閉じる";
            okButton.TopAttachment = AttachmentType.Form;
            okButton.RightAttachment = AttachmentType.Form;
            okButton.ShowAsDefault = true;

            okButton.ActivateEvent += (z,p) => {
                this.Destroy();
            };
            form.Children.Add(okButton);

            //sc.BottomAttachment = AttachmentType.Widget;
            //sc.BottomWidget = okButton;
        }
        private TonNurako.Widgets.Xm.PushButton okButton;
        private TonNurako.Widgets.Xm.Text textBox;
    }
}
