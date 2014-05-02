using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SUOnlineRisk;

namespace RiskMap
{
    public partial class DialContInfo : Form
    {
        // Variables
        protected bool wasOkay = false;
        protected string newName;
        protected int newBonus;

        // Constructionator
        public DialContInfo()
        {
            InitializeComponent();
        }

        // Canceling
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Confirming
        private void button1_Click(object sender, EventArgs e)
        {
            int val = 0;
            try
            {
                val = Int32.Parse(textBox2.Text);
                this.newBonus = val;
                this.newName = textBox1.Text;
                this.wasOkay = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("You need to type a number.");
            }
        }

        // Returning
        public bool returnOkay() { return wasOkay; }
        public String returnName() { return newName; }
        public int returnBonus() { return newBonus; }

        // Setting for editing
        public void setForEdit(string name, int bonus) {
            textBox1.Text = name;
            textBox2.Text = bonus.ToString();
        }

        // Nobody
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Do we really need a placeholder for these?
        }

        // cares
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Okay apparently we do otherwise the program will bitch about it. Great. Thanks.
        }
    }
}