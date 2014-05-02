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
    public partial class CampaignMap : Form
    {
        public CampaignMap()
        {
            InitializeComponent();
        }

        private void backButton(object sender, EventArgs e)
        {
            Campaign campaign = new Campaign();
            campaign.Show();
            this.Close();
        }

        private void playButton(object sender, EventArgs e)
        {
            Game game = new Game();
            game.Show();
        }
    }
}
