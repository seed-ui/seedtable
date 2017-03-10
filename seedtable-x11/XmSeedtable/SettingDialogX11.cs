using SeedTable;

namespace XmSeedtable
{
    public partial class SettingDialogX11 :
        TonNurako.Widgets.Xm.DialogShell
    {

        ToOptions Options { get; }
        SeedTableX11.SettingHandler XHandler {get;}

        bool Changable = false;
        public bool Status {get; private set;} = false;
        public SettingDialogX11(ToOptions options,bool changeable, SeedTableX11.SettingHandler handler) : base() {
            this.Width = 700;
            this.Height = 600;
            Options = options;
            XHandler = handler;
            Changable = changeable;
            this.AllowAutoManage = false;
            this.AllowShellResize = true;
            this.Title = "Settings";
            this.CreatePopupChildEvent += (x,y) => {
                Sinatra();
            };
            this.PopdownEvent += (x,p) => {
                this.Destroy();
            };
        }

        void SaveOptions() {
            if(!Changable) {
                return;
            }
            Options.subdivide = subdivideTextBox.Value.Split('\n');
            Options.ignoreColumns = ignoreColumnsTextBox.Value.Split('\n');
            Options.yamlColumns = yamlColumnsTextBox.Value.Split('\n');
            Options.mapping = mappingTextBox.Value.Split('\n');
            Options.ignore = ignoreTextBox.Value.Split('\n');
            Options.only = onlyTextBox.Value.Split('\n');
            XHandler.Save(Options);
        }
    }
}