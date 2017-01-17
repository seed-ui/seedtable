using System;
using TonNurako.Data;
using TonNurako.Widgets;
using TonNurako.Widgets.Xm;

using SeedTable;

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
            };
        }

        public void delegaty(FromOptions e) {
            SeedTableInterface.InformationMessageEventHandler handler =
                (string message) => {
                    Console.WriteLine(message);
                };
            SeedTableInterface.InformationMessageEvent += handler;
            try {
                SeedTableInterface.ExcelToSeed(e);
                Status = true;
            } catch (SeedTableInterface.CannotContinueException) {
                Status = false;
            }
            SeedTableInterface.InformationMessageEvent -= handler;
        }
    }
}