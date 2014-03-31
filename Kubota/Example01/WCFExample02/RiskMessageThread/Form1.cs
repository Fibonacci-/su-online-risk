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
using RiskMessageService;
using SUOnlineRisk;

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
            using (RiskMessageThread.ServiceReference1.RiskMessageServiceClient proxy = new RiskMessageThread.ServiceReference1.RiskMessageServiceClient())
            {
                LoginDialog dlg = new LoginDialog();
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string name = dlg.getLogin();
                    string passwd = dlg.getPassword();
                    proxy.Logon(name, passwd);
                    MainState state = MainState.Idle;
                    int gameid;
                    if (name == "admin")
                    {
                        gameid = proxy.createGame(1);
                        proxy.joinGame(name, gameid);
                        MessageBox.Show("GameID is " + gameid + ". Click OK to start the game");
                        proxy.startGame(name, gameid);
                    }
                    else
                    {
                        GameIDInput dlg2 = new GameIDInput();
                        int gameid2 = 0;
                        if (dlg2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            gameid2 = dlg2.id();
                        }
                        if (proxy.joinGame(name, gameid2) == false)
                        {
                            Console.Error.WriteLine("Wrong game id. Bye.");
                            return;
                        }
                    }
                    while (true)
                    {
                        RiskMessage msg = new RiskMessage(state, name);
                        RiskMessage resp = proxy.Request(msg);
                        //Console.Out.WriteLine(msg.state + " => " + resp.state + " " + resp.playerName);
                        state = resp.state;
                        backgroundWorker1.ReportProgress(0, resp);
                        if (state == MainState.Over)
                        {
                            break;
                        }
                        Thread.Sleep(500);
                    }
                }
            }
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RiskMessage msg = (RiskMessage)e.UserState;
            this.toolStripStatusLabel1.Text = msg.state + " " + msg.playerName;
        }
    }
}
