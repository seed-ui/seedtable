using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SeedTable;

namespace seedtable_gui {
    public partial class YamlToExcelDialog : BaseForm {
        ToOptions Options { get; }

        public YamlToExcelDialog(ToOptions options) : base() {
            InitializeComponent();
            Options = options;
        }

        private void YamlToExcelDialog_Shown(object sender, EventArgs e) {
            seedToExcelBackgroundWorker.RunWorkerAsync(Options);
        }

        private void okButton_Click(object sender, EventArgs e) {
            Close();
        }

        private void seedToExcelBackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            var worker = sender as BackgroundWorker;
            SeedTableInterface.InformationMessageEventHandler handler =
                (string message) => worker.ReportProgress(0, message);
            SeedTableInterface.InformationMessageEvent += handler;
            try {
                SeedTableInterface.SeedToExcel((ToOptions)e.Argument);
                e.Result = true;
            } catch (SeedTableInterface.CannotContinueException) {
                e.Result = false;
            }
            SeedTableInterface.InformationMessageEvent -= handler;
        }

        private void WriteInfo(string message) {
            infoTextBox.AppendText(message + Environment.NewLine);
        }

        private void seedToExcelBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            WriteInfo((string)e.UserState);
        }

        private void seedToExcelBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if ((bool)e.Result == false) {
                MessageBox.Show("処理が失敗しました");
            }
            okButton.Enabled = true;
        }
    }
}
