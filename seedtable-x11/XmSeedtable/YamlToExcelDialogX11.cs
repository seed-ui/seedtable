using System;
using TonNurako.Widgets.Xm;

using SeedTable;
using System.Threading.Tasks;

namespace XmSeedtable
{
    public partial class YamlToExcelDialogX11 :
        TonNurako.Widgets.Xm.DialogShell
    {

        ToOptions Options { get; }
        public bool Status {get; private set;} = false;
        public YamlToExcelDialogX11(ToOptions options) : base() {
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

        public void delegaty(ToOptions e) {
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
                SeedTableInterface.SeedToExcel(e);
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