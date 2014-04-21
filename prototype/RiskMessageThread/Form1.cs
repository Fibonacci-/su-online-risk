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
using SUOnlineRisk;

namespace RiskMessageThread
{
    public partial class Form1 : Form
    {
        bool bWaitForUser;
        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();
            bWaitForUser = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            using (RiskMessageThread.ServiceReference1.RiskServerClient proxy = new RiskMessageThread.ServiceReference1.RiskServerClient())
            {
                MainState state = MainState.Idle;
                string name = "admin"; 
                while (true)
                {
                    RiskMessage msg = new RiskMessage(state, name);
                    RiskMessage resp = proxy.Request(msg);
                    //Console.Out.WriteLine(msg.state + " => " + resp.state + " " + resp.playerName);
                    state = resp.state;
                    if (state == MainState.Over)
                    {
                        break;
                    }
                    lock (this)
                    {
                        bWaitForUser = true;
                    }
                    backgroundWorker1.ReportProgress(0, resp);
                    while (bWaitForUser == true)
                    {
                        Thread.Sleep(500);
                    }
                }
            }
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RiskMessage msg = (RiskMessage)e.UserState;
            this.toolStripStatusLabel1.Text = msg.state + " " + msg.playerName;
            MessageBox.Show("Stopped right here.");
            lock (this)
            {
                bWaitForUser = false;
            }
        }
    }
}
