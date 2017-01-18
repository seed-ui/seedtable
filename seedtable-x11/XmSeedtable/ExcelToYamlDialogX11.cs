using System;
using TonNurako.Widgets.Xm;

using SeedTable;
using System.Threading.Tasks;

namespace XmSeedtable
{
    public partial class ExcelToYamlDialogX11 :
        TonNurako.Widgets.Xm.DialogShell
    {

        FromOptions Options { get; }
        public bool Status {get; private set;} = false;
        public ExcelToYamlDialogX11(FromOptions options) : base() {
            Options = options;
            this.AllowAutoManage = false;
            this.DeleteResponse = DeleteResponse.DoNothing;
            this.CreatePopupChildEvent += (x,y) => {
                Sinatra();
                new Task(()=> {
                    delegaty(options);
                }).Start();
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