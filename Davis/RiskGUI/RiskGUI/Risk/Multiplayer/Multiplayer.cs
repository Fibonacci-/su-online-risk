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
    public partial class Multiplayer : Form
    {
        public Multiplayer()
        {
            InitializeComponent();
        }

        private void backButton(object sender, EventArgs e)
        {
            this.Close();
        }

        private void playButton(object sender, EventArgs e)
        {
            this.Close();
            Lobby lobby = new Lobby();
            lobby.Show();
        }

        private void mapButton(object sender, EventArgs e)
        {
            Map map = new Map();
            map.Show();
        }
    }
}
