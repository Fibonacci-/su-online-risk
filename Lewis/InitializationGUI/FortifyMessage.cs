﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InitializationGUI
{
    public partial class FortifyMessage : Form
    {
        int k;
        int max;
        int armiesToMove = 0;
        public FortifyMessage(int max)
        {
            InitializeComponent();
            this.max = max;
            numericUpDown1.Maximum = max;
        }

        private void FortifyMessage_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            armiesToMove = (int)numericUpDown1.Value;
        }

        public int getArmiesToMove()
        {
            return armiesToMove;
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
