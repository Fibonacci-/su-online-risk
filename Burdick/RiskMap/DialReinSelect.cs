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
    public partial class DialReinSelect : Form
    {
        // Variables
        public bool wasOkay = false;
        public ReinforcementCard selected;

        // Constructionator
        public DialReinSelect(List<ReinforcementCard> cards)
        {
            InitializeComponent();
            for (int i = 0; i < cards.Count; i++) comboBox1.Items.Add(cards[i]);
        }

        // O - K ! ! !
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("You need to select a reinforcement card.");
            }
            else
            {
                this.selected = (ReinforcementCard)comboBox1.SelectedItem;
                this.wasOkay = true;
                Close();
            }
        }

        // Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
