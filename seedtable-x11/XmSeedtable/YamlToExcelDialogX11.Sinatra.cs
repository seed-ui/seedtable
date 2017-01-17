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
    public partial class YamlToExcelDialogX11
    {
        private void Sinatra() {
            var form = new Form();
            form.Width = 320;
            form.Height = 320;
            this.Children.Add(form);

            var text = new ScrolledText();
            text.Rows = 50;
            form.Children.Add(text);

            var ok = new PushButton();
            ok.TopAttachment = AttachmentType.Widget;
            ok.TopWidget = text;
            ok.ActivateEvent += (z,p) => {
                this.Destroy();
            };
            form.Children.Add(ok);
        }
    }
}
