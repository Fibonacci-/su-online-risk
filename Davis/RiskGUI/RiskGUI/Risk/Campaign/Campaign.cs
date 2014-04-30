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
    public partial class Campaign : Form
    {
        public Campaign()
        {
            InitializeComponent();
        }

        private void backButton(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nextButton(object sender, EventArgs e)
        {
            this.Close();
            Map map = new Map();
            map.Show();
            
        }
    }
}
