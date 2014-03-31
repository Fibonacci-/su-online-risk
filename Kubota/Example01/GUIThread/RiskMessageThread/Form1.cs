using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace RiskMessageThread
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.attackButton.Enabled = false;
            this.drawButton.Enabled = false;
            this.doneButton.Enabled = false;
            this.placeButton.Enabled = false;
            this.moveButton.Enabled = false;
            this.tradeButton.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(3000);
                this.backgroundWorker1.ReportProgress(0);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }
    }
}
