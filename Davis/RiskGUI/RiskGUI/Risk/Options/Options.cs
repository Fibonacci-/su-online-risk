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
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        private void backButton(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
