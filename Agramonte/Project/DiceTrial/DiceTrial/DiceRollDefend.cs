using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LewisGUIExample
{
    public partial class DiceRollDefend : Form
    {
        const int NumDiceDefender = 2;
        int[] numberDefender;
        Random rg;
        int[] dieChoiceDefender;
        public DiceRollDefend()
        {
            InitializeComponent();
            numberDefender = new int[NumDiceDefender];
            dieChoiceDefender = new int[] { 1, 2 };
            rg = new Random();
            setDieBlank();
        }
        #region Rolls for Attackers and Defenders
        //using random it sets a number for each of the 3 die for attacker
        void RollDefender2()
        {
            for (int i = 0; i < NumDiceDefender; ++i)
            {
                numberDefender[i] = rg.Next(1, 7);
            }
        }

        void RollDefender1()
        {
            for (int i = 0; i < NumDiceDefender - 1; ++i)
            {
                numberDefender[i] = rg.Next(1, 7);
            }
        }
        #endregion

        #region Set Die Images
        //sets the image of each die in the pictureboxes based on the numbers from Roll() 

        void updatePicturesDefender2()
        {
            PictureBox[] boxes = { this.pictureBox1, this.pictureBox2 };
            string[] picnames = { "dice_1.png", "dice_2.png", "dice_3.png", "dice_4.png", "dice_5.png", "dice_6.png" };
            for (int i = 0; i < NumDiceDefender; ++i)
            {
                //since in an array need to load picname - 1 to match value of number
                boxes[i].Load(@"..\..\Resources\" + picnames[numberDefender[i] - 1]);
            }
        }

        void updatePicturesDefender1()
        {
            PictureBox[] boxes = { this.pictureBox1 };
            string[] picnames = { "dice_1.png", "dice_2.png", "dice_3.png", "dice_4.png", "dice_5.png", "dice_6.png" };
            for (int i = 0; i < NumDiceDefender - 1; ++i)
            {
                //since in an array need to load picname - 1 to match value of number
                boxes[i].Load(@"..\..\Resources\" + picnames[numberDefender[i] - 1]);
            }
        }

        //sets the die to a blank set to start so there is an image
        void setDieBlank()
        {
            PictureBox[] boxes = { this.pictureBox1, this.pictureBox2 };
            string picname = "dice_blank.png";
            for (int i = 0; i < NumDiceDefender; ++i)
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

        public void setOptionDefend()
        {
            comboBox1.DataSource = dieChoiceDefender;
        }

        //provides a number for the 2 Defenders die and displays them 
        private void buttonRoll1_Click(object sender, EventArgs e)
        {
            //Will update all if the user chooses to have all 3 die rolled. Otherwise no update is done.
            //Roll is inside to avoid advancing the random numbers unless they are to be used.
            if (comboBox1.SelectedValue.Equals((object)NumDiceDefender))
            {
                if (pictureBox2.Visible == false)
                {
                    pictureBox2.Show();
                }

                RollDefender2();
                updatePicturesDefender2();
            }
            //lastly since one is the only other option roll 1 and disable the others
            else
            {
                RollDefender1();
                pictureBox2.Hide();
                updatePicturesDefender1();
            }
        }
            
    }
}
