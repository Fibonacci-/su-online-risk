using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SURiskGUI
{
    public partial class GameIDInput : Form
    {
        public GameIDInput()
        {
            InitializeComponent();
        }
        public int id()
        {
            int gameid = -1;
            try
            {
                gameid = Int32.Parse(this.textBox1.Text);
            }
            catch (System.FormatException ex)
            {
                MessageBox.Show(this.textBox1.Text + " is not a number.");
                gameid = -1;
            }
            return gameid;
        }
    }
}
