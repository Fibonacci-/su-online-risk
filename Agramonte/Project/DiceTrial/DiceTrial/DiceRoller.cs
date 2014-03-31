using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SUOnlineRisk;

namespace LewisGUIExample
{
    public partial class DiceForm : Form
    {

        const int NumDiceAttacker = 3;
        int[] numberAttacker;
        Random rg;
        int[] dieChoiceAttacker;
        Player roller; 

        public DiceForm()
        {
            InitializeComponent();
            numberAttacker = new int[NumDiceAttacker];
            dieChoiceAttacker = new int[] { 1, 2, 3 };
            rg = new Random();
            setDieBlank();
            
        }

        #region Rolls for Attackers and Defenders
        //using random it sets a number for each of the 3 die for attacker
        void RollAttacker3()
        {
            for (int i = 0; i < NumDiceAttacker; ++i)
            {
                numberAttacker[i] = rg.Next(1, 7);
            }
        }

        //using random to set the value for 2 die, leaves the 3rd die unchanged
        void RollAttacker2()
        {
            for (int i = 0; i < NumDiceAttacker - 1; ++i)
            {
                numberAttacker[i] = rg.Next(1, 7);
            }
        }

        //using random to set the value for 1 die, leaves the 2nd and 3rd die unchanged
        void RollAttacker1()
        {
            for (int i = 0; i < NumDiceAttacker - 2; ++i)
            {
                numberAttacker[i] = rg.Next(1, 7);
            }
        }

        #endregion

        #region Set Die Images
        //sets the image of each die in the pictureboxes based on the numbers from Roll() 
        //USE WHEN ROLLING ALL FOR ATTACKER!!
        void updatePicturesAttacker3()
        {
            PictureBox[] boxes = { this.pictureBox1, this.pictureBox2, this.pictureBox3 };
            string[] picnames = { "dice_1.png", "dice_2.png", "dice_3.png", "dice_4.png", "dice_5.png", "dice_6.png" };
            for (int i = 0; i < NumDiceAttacker; ++i)
            {
                //since in an array need to load picname - 1 to match value of number
                boxes[i].Load(@"..\..\Resources\" + picnames[numberAttacker[i] - 1]);
            }
        }

        void updatePicturesAttacker2()
        {
            PictureBox[] boxes = { this.pictureBox1, this.pictureBox2 };
            string[] picnames = { "dice_1.png", "dice_2.png", "dice_3.png", "dice_4.png", "dice_5.png", "dice_6.png" };
            for (int i = 0; i < 2; ++i)
            {
                //since in an array need to load picname - 1 to match value of number
                boxes[i].Load(@"..\..\Resources\" + picnames[numberAttacker[i] - 1]);
            }
        }

        void updatePicturesAttacker1()
        {
            PictureBox[] boxes = { this.pictureBox1 };
            string[] picnames = { "dice_1.png", "dice_2.png", "dice_3.png", "dice_4.png", "dice_5.png", "dice_6.png" };
            for (int i = 0; i < 1; ++i)
            {
                //since in an array need to load picname - 1 to match value of number
                boxes[i].Load(@"..\..\Resources\" + picnames[numberAttacker[i] - 1]);
            }
        }

        //sets the die to a blank set to start so there is an image
        void setDieBlank()
        {
            PictureBox[] boxes = { this.pictureBox1, this.pictureBox2, this.pictureBox3 };
            string picname = "dice_blank.png";
            for (int i = 0; i < NumDiceAttacker; ++i)
            {
                //since in an array need to load picname - 1 to match value of number
                boxes[i].Load(@"..\..\Resources\" + picname);
            }
        }

        #endregion

        #region Compare Attacker and Defender Rolls
        void compareOneSet()
        {

        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void setOptionAttack()//Player player)
        {
            comboBox1.DataSource = dieChoiceAttacker;
            //roller = player;
        }

        //provides a number for the 3 attackers die and displays them 
        private void buttonRoll_Click(object sender, EventArgs e)
        {

            //Will update all if the user chooses to have all 3 die rolled. Otherwise no update is done.
            //Roll is inside to avoid advancing the random numbers unless they are to be used.
                if (comboBox1.SelectedValue.Equals((object)NumDiceAttacker))
                {
                    if (pictureBox2.Visible == false || pictureBox3.Visible == false)
                    {
                        pictureBox2.Show();
                        pictureBox3.Show();
                    }
                    RollAttacker3();
                    updatePicturesAttacker3();
                }
                //When roll is set to 2 disable a roll and hide the last die
                else if (comboBox1.SelectedValue.Equals((object)(NumDiceAttacker - 1)))
                {
                    if (pictureBox2.Visible == false)
                    {
                        pictureBox2.Show();
                    }

                    RollAttacker2();
                    pictureBox3.Hide();
                    updatePicturesAttacker2();
                }
                //lastly since one is the only other option roll 1 and disable the others
                else
                {
                    RollAttacker1();
                    pictureBox2.Hide();
                    pictureBox3.Hide();
                    updatePicturesAttacker1();
                }

            }
            
    }

}
