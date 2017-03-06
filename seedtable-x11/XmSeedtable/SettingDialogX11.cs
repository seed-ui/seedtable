using System;
using TonNurako.Widgets.Xm;

using SeedTable;
using System.Threading.Tasks;

namespace XmSeedtable
{
    public partial class SettingDialogX11 :
        TonNurako.Widgets.Xm.DialogShell
    {

        ToOptions Options { get; }
        public bool Status {get; private set;} = false;
        public SettingDialogX11(ToOptions options) : base() {
            Options = options;
            this.AllowAutoManage = false;
            this.AllowShellResize = true;
            this.Title = "Setty";
            //this.DeleteResponse = DeleteResponse.DoNothing;
            this.CreatePopupChildEvent += (x,y) => {
                Sinatra();
            };
            this.PopdownEvent += (x,y) => {
                Console.WriteLine("destroy");
                this.Destroy();
            };

        }

        public void delegaty(FromOptions e) {
            this.AppContext.Invoke(()=>{
                okButton.Sensitive = false;
            });
            SeedTableInterface.InformationMessageEventHandler handler =
                (string message) => {
                    Console.WriteLine(message);
                    this.AppContext.Invoke(()=>{
                        textBox.Insert(message + "\n", textBox.CursorPosition);
                    });
                };
            SeedTableInterface.InformationMessageEvent += handler;
            try {
                SeedTableInterface.ExcelToSeed(e);
                Status = true;
            } catch (SeedTableInterface.CannotContinueException) {
                Status = false;
            }
            finally {
                this.AppContext.Invoke(()=>{
                    okButton.Sensitive = true;
                });
                SeedTableInterface.InformationMessageEvent -= handler;
            }

        }
    }
}