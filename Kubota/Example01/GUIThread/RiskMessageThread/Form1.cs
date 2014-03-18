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
            RiskMessageThread.ServiceReference1.RiskMessageServiceClient proxy = new RiskMessageThread.ServiceReference1.RiskMessageServiceClient();
            while (true)
            {
                Thread.Sleep(3000);
                ServiceReference1.RiskMessage outgoing = (ServiceReference1.RiskMessage)new ServiceReference1.RiskMessage(); //(SUOnlineRisk.MainState.Idle, "Roger");
                ServiceReference1.RiskMessage msg = proxy.Request(outgoing);
                this.backgroundWorker1.ReportProgress(0);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //MessageBox.Show("Progress changed.");
            if(this.attackButton.Enabled == true)
            {
                this.attackButton.Enabled = false;
            }
            else
            {
                this.attackButton.Enabled = true;
            }
            //this.Invalidate();
        }
    }
}
