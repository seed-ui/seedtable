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

        private void Sinatra() {
            var form = new Form();
            form.Width = 640;
            form.Height = 320;
            form.ResizePolicy = ResizePolicy.Grow;
            this.Children.Add(form);

            var cs = new LeftControls[] {
                new LeftControls( "エンジン\n(--engine)", WgtEngine),
                new LeftControls( "カラム名行\n(--column-names-row)", WgtColumn),
                new LeftControls( "データ開始行\n(--data-start-row)", WgtDataRow),
                new LeftControls( "yml→xlsx変換時に行削除を行う\r\n(--delete)",
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
                    }),
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
            rc.BottomAttachment = AttachmentType.Form;



            okButton = new PushButton();
            okButton.LabelString = "閉じる";
            okButton.BottomAttachment = AttachmentType.Form;
            okButton.LeftAttachment = AttachmentType.Form;
            okButton.RightAttachment = AttachmentType.Form;
            okButton.ActivateEvent += (z,p) => {
                this.Destroy();
            };
            okButton.Visible = false;
            form.Children.Add(okButton);
            //sc.BottomAttachment = AttachmentType.Widget;
            //sc.BottomWidget = okButton;
        }
        private TonNurako.Widgets.Xm.PushButton okButton;
        private TonNurako.Widgets.Xm.Text textBox;
    }
}
