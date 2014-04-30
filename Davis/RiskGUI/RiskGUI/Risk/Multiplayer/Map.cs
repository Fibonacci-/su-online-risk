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
    public partial class Map : Form
    {
        public Map()
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
    }
}
