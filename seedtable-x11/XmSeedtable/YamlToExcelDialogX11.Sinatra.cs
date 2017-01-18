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

            var sc = new ScrolledWindow();
            form.Children.Add(sc);

            textBox = new Text();
            textBox.EditMode = EditMode.Multi;

            textBox.TopAttachment = AttachmentType.Form;
            textBox.LeftAttachment = AttachmentType.Form;
            textBox.RightAttachment = AttachmentType.Form;
            sc.Children.Add(textBox);

            okButton = new PushButton();
            okButton.LabelString = "閉じる";
            okButton.BottomAttachment = AttachmentType.Form;
            okButton.LeftAttachment = AttachmentType.Form;
            okButton.RightAttachment = AttachmentType.Form;
            okButton.ActivateEvent += (z,p) => {
                this.Destroy();
            };
            form.Children.Add(okButton);
            sc.BottomAttachment = AttachmentType.Widget;
            sc.BottomWidget = okButton;
        }
        private TonNurako.Widgets.Xm.PushButton okButton;
        private TonNurako.Widgets.Xm.Text textBox;
    }
}
