using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example01
{
    public partial class Form1 : Form
    {
        String mode;
        public Form1()
        {
            InitializeComponent();
            mode = "Uninitialized";
            updateStatusLabel();
        }
        private void updateStatusLabel()
        {
            this.toolStripStatusLabel1.Text = mode;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            int count = Int32.Parse(this.label1.Text);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                count++;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (count > 0)
                {
                    count--;
                }
            }
            this.label1.Text = count.ToString();
        }

        private void doneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = "Done";
            updateStatusLabel();
        }
    }
}
