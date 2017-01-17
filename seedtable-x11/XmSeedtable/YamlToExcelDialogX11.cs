using System;
using TonNurako.Data;
using TonNurako.Widgets;
using TonNurako.Widgets.Xm;

using SeedTable;

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