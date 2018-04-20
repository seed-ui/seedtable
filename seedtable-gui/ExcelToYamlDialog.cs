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
    public partial class ExcelToYamlDialog : BaseForm {
        FromOptions Options { get; }

        public ExcelToYamlDialog(FromOptions options) : base() {
            InitializeComponent();
            Options = options;
        }

        private void ExcelToYamlDialog_Shown(object sender, EventArgs e) {
            excelToSeedBackgroundWorker.RunWorkerAsync(Options);
        }

        private void okButton_Click(object sender, EventArgs e) {
            Close();
        }

        private void excelToSeedBackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            var worker = sender as BackgroundWorker;
            SeedTableInterface.InformationMessageEventHandler handler =
                (string message) => worker.ReportProgress(0, message);
            SeedTableInterface.InformationMessageEvent += handler;
            try {
                SeedTableInterface.ExcelToSeed((FromOptions)e.Argument);
                e.Result = true;
            } catch (SeedTableInterface.CannotContinueException) {
                e.Result = false;
            }
            SeedTableInterface.InformationMessageEvent -= handler;
        }

        private void WriteInfo(string message) {
            infoTextBox.AppendText(message + Environment.NewLine);
        }

        private void excelToSeedBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            WriteInfo((string)e.UserState);
        }

        private void excelToSeedBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if ((bool)e.Result == false) {
                MessageBox.Show("処理が失敗しました");
            }
            okButton.Enabled = true;
        }
    }
}
