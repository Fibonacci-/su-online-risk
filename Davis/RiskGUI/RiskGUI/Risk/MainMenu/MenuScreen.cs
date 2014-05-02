using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RiskGUI
{
    public partial class MenuScreen : Form
    {
        public MenuScreen()
        {
            InitializeComponent();
        }


        private void campaign(object sender, EventArgs e)
        {
            Campaign campaignSettings = new Campaign();
            campaignSettings.Show();
        }

        private void multiplayer(object sender, EventArgs e)
        {
            Multiplayer multiplayerSettings = new Multiplayer();
            multiplayerSettings.Show();
        }

        private void profile(object sender, EventArgs e)
        {
            Profile createProfile = new Profile();
            createProfile.Show();
        }

        private void credits(object sender, EventArgs e)
        {
            Credits credits = new Credits();
            credits.Show();
        }

        private void exit(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
