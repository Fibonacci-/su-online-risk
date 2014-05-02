using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using SUOnlineRisk;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SURiskGUI
{
    /*
     * Gets the cards that will be removed from the players hand
     * when they are traded in
     * Actual removal of cards from hand happens in StateGUI
     * This is throwing an error at the moment in StateGui
     * */
    public partial class TradeCardGUI : Form
    {
        List<ReinforcementCard> ReinforcementCards;
        List<ReinforcementCard> selectedCards;
        string boxCombination; 
        CheckBox[] boxes;
        
        public TradeCardGUI(List<ReinforcementCard> Cards)
        {
            InitializeComponent();
            boxes = new CheckBox[] {checkBox1, checkBox2, checkBox3, checkBox4, checkBox5};
            ReinforcementCards = Cards;
            this.selectedCards = new List<ReinforcementCard>();
            if(ReinforcementCards.ElementAtOrDefault(0) != null)
            {
                if(ReinforcementCards[0].UnitType == ReinforcemenCardUnit.Infantry)
                {
                    pictureBox1.Image = global::RiskMessageThread.Properties.Resources.infantryBlue;
                }
                else if (ReinforcementCards[0].UnitType == ReinforcemenCardUnit.Cavalry)
                {
                    pictureBox1.Image = global::RiskMessageThread.Properties.Resources.cavalryBlue;
                }
                else if (ReinforcementCards[0].UnitType == ReinforcemenCardUnit.Artillery)
                {
                    pictureBox1.Image = global::RiskMessageThread.Properties.Resources.artilleryBlue;
                }
                label1.Text = ReinforcementCards[0].TerritoryName + " " + ReinforcementCards[0].UnitType.ToString();
            }
            else
            {
                label1.Text = "";
                checkBox1.Enabled = false;
                checkBox1.Visible = false;
                pictureBox1.Visible = false;
            }
            if (ReinforcementCards.ElementAtOrDefault(1) != null)
            {
                if (ReinforcementCards[1].UnitType == ReinforcemenCardUnit.Infantry)
                {
                    pictureBox2.Image = global::RiskMessageThread.Properties.Resources.infantryBlue;
                }
                else if (ReinforcementCards[1].UnitType == ReinforcemenCardUnit.Cavalry)
                {
                    pictureBox2.Image = global::RiskMessageThread.Properties.Resources.cavalryBlue;
                }
                else if (ReinforcementCards[1].UnitType == ReinforcemenCardUnit.Artillery)
                {
                    pictureBox2.Image = global::RiskMessageThread.Properties.Resources.artilleryBlue;
                }
                label2.Text = ReinforcementCards[1].TerritoryName + " " + ReinforcementCards[1].UnitType.ToString();
            }
            else
            {
                label2.Text = "";
                checkBox2.Enabled = false;
                checkBox2.Visible = false;
                pictureBox2.Visible = false;
            }
            if (ReinforcementCards.ElementAtOrDefault(2) != null)
            {
                if (ReinforcementCards[2].UnitType == ReinforcemenCardUnit.Infantry)
                {
                    pictureBox3.Image = global::RiskMessageThread.Properties.Resources.infantryBlue;
                }
                else if (ReinforcementCards[2].UnitType == ReinforcemenCardUnit.Cavalry)
                {
                    pictureBox3.Image = global::RiskMessageThread.Properties.Resources.cavalryBlue;
                }
                else if (ReinforcementCards[2].UnitType == ReinforcemenCardUnit.Artillery)
                {
                    pictureBox3.Image = global::RiskMessageThread.Properties.Resources.artilleryBlue;
                }
                label3.Text = ReinforcementCards[2].TerritoryName + " " + ReinforcementCards[2].UnitType.ToString();
            }
            else
            {
                label3.Text = "";
                checkBox3.Enabled = false;
                checkBox3.Visible = false;
                pictureBox3.Visible = false;
            }
            if (ReinforcementCards.ElementAtOrDefault(3) != null)
            {
                if (ReinforcementCards[3].UnitType == ReinforcemenCardUnit.Infantry)
                {
                    pictureBox4.Image = global::RiskMessageThread.Properties.Resources.infantryBlue;
                }
                else if (ReinforcementCards[3].UnitType == ReinforcemenCardUnit.Cavalry)
                {
                    pictureBox4.Image = global::RiskMessageThread.Properties.Resources.cavalryBlue;
                }
                else if (ReinforcementCards[3].UnitType == ReinforcemenCardUnit.Artillery)
                {
                    pictureBox4.Image = global::RiskMessageThread.Properties.Resources.artilleryBlue;
                }
                label4.Text = ReinforcementCards[3].TerritoryName + " " + ReinforcementCards[3].UnitType.ToString();
            }
            else
            {
                label4.Text = "";
                checkBox4.Enabled = false;
                checkBox4.Visible = false;
                pictureBox4.Visible = false;
            }
            if (ReinforcementCards.ElementAtOrDefault(4) != null)
            {
                if (ReinforcementCards[4].UnitType == ReinforcemenCardUnit.Infantry)
                {
                    pictureBox5.Image = global::RiskMessageThread.Properties.Resources.infantryBlue;
                }
                else if (ReinforcementCards[4].UnitType == ReinforcemenCardUnit.Cavalry)
                {
                    pictureBox5.Image = global::RiskMessageThread.Properties.Resources.cavalryBlue;
                }
                else if (ReinforcementCards[4].UnitType == ReinforcemenCardUnit.Artillery)
                {
                    pictureBox5.Image = global::RiskMessageThread.Properties.Resources.artilleryBlue;
                }
                label5.Text = ReinforcementCards[4].TerritoryName + " " + ReinforcementCards[4].UnitType.ToString();
            }
            else
            {
                label5.Text = "";
                checkBox5.Enabled = false;
                checkBox5.Visible = false;
                pictureBox5.Visible = false;
            }
        }

        string getBinary(bool box)
        {
            string bin;
            if(box)
            {
                bin = "1";
            }
            else{
                bin = "0";
            }
            return bin;
        }

        int countChecked()
        {
            int count = 0;
            foreach(CheckBox c in boxes)
            {
                if(c.Checked)
                {
                    count++;
                }
            }
            return count;
        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int count = countChecked();
            if(count > 3)
            {
                MessageBox.Show("You have three cards already selected");
                checkBox1.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            int count = countChecked();
            if (count > 3)
            {
                MessageBox.Show("You have three cards already selected");
                checkBox2.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            int count = countChecked();
            if (count > 3)
            {
                MessageBox.Show("You have three cards already selected");
                checkBox3.Checked = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            int count = countChecked();
            if (count > 3)
            {
                MessageBox.Show("You have three cards already selected");
                checkBox4.Checked = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            int count = countChecked();
            if (count > 3)
            {
                MessageBox.Show("You have three cards already selected");
                checkBox5.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*foreach (CheckBox c in boxes)
            {
                boxCombination += getBinary(c.Checked);
            }
            switch(boxCombination)
            {
                case "11100" :
                    Cards.Add(ReinforcementCards[0]);
                    Cards.Add(ReinforcementCards[1]);
                    Cards.Add(ReinforcementCards[2]);
                    break;

                case "01110":
                    Cards.Add(ReinforcementCards[1]);
                    Cards.Add(ReinforcementCards[2]);
                    Cards.Add(ReinforcementCards[3]);
                    break;

                case "00111":
                    Cards.Add(ReinforcementCards[2]);
                    Cards.Add(ReinforcementCards[3]);
                    Cards.Add(ReinforcementCards[4]);
                    break;

                case "10110":
                    Cards.Add(ReinforcementCards[0]);
                    Cards.Add(ReinforcementCards[2]);
                    Cards.Add(ReinforcementCards[3]);
                    break;

                case "10011":
                    Cards.Add(ReinforcementCards[0]);
                    Cards.Add(ReinforcementCards[3]);
                    Cards.Add(ReinforcementCards[4]);
                    break;

                case "01011":
                    Cards.Add(ReinforcementCards[1]);
                    Cards.Add(ReinforcementCards[3]);
                    Cards.Add(ReinforcementCards[4]);
                    break;

                case "01101":
                    Cards.Add(ReinforcementCards[1]);
                    Cards.Add(ReinforcementCards[2]);
                    Cards.Add(ReinforcementCards[4]);
                    break;

                case "10101":
                    Cards.Add(ReinforcementCards[0]);
                    Cards.Add(ReinforcementCards[2]);
                    Cards.Add(ReinforcementCards[4]);
                    break;

                case "11001":
                    Cards.Add(ReinforcementCards[0]);
                    Cards.Add(ReinforcementCards[1]);
                    Cards.Add(ReinforcementCards[4]);
                    break;

                case "11010":
                    Cards.Add(ReinforcementCards[0]);
                    Cards.Add(ReinforcementCards[1]);
                    Cards.Add(ReinforcementCards[3]);
                    break;

                default :
                    MessageBox.Show("This is not an acceptable combination of cards");
                    break;
            }*/
            for(int i=0; i<5; ++i)
            {
                if(boxes[i].Checked)
                {
                    selectedCards.Add(this.ReinforcementCards[i]);
                }
            }
            if(selectedCards.Count != 3)
            {
                MessageBox.Show("You need to select 3 cards.");
            }
            else 
            {
                //check if types are all the same or all different
                HashSet<ReinforcemenCardUnit> units = new HashSet<ReinforcemenCardUnit>();
                foreach(ReinforcementCard card in selectedCards)
                {
                    units.Add(card.UnitType);
                }
                if (units.Count == 1 || units.Count == 3)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("This is not an acceptable combination of cards. Cards have to be all the same type or different types.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        public List<ReinforcementCard> getReinforcementCards()
        {
            return selectedCards;
        }

        private void TradeCardGUI_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
