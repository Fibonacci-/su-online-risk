using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RiskMap
{
    public partial class DialTerrInfo : Form
    {
        // Variables
        public bool wasOkay = false;
        public string newName;
        public Continent newCont;
        public List<Territory> newNeighbors = new List<Territory>();

        // Constructor
        public DialTerrInfo(List<Continent> conts, List<Territory> terrs)
        {
            InitializeComponent();
            for (int i = 0; i < conts.Count; i++) {
                comboBox1.Items.Add(conts.ElementAt(i));
            }
            for (int i = 0; i < terrs.Count; i++)
            {
                checkedListBox1.Items.Add(terrs.ElementAt(i));
            }
        }

        // Loading neighbors.
        public void loadNeighbors(List<Territory> temp) {
            for (int i = 0; i < checkedListBox1.SelectedItems.Count; i++) {
            }
        }

        // Selecting a continent
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Guess I never actually ended up using this.
        }

        // Selecting neighbors
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Or this.
        }
        
        // Okay
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("A territory needs a continent.");
            }
            else
            {
                this.newName = textBox1.Text;
                this.newCont = (Continent)comboBox1.SelectedItem;
                this.wasOkay = true;
                for (int i = 0; i < temp.Count; i++) {
                    newNeighbors.Add();
                }
                Close();
            }
        }

        // Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Setting for editing
        public void setForEdit(string name, int owner) {
            textBox1.Text = name;
            comboBox1.SelectedIndex = owner;
        }
    }
}
