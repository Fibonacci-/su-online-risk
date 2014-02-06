using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiceTrial
{
    public partial class Form1 : Form
    {
        //Dice _Dice;
        const int NumDice = 3;
        int[] numbers;
        Random rg;
        public Form1()
        {
            InitializeComponent();
            numbers = new int[NumDice];
            rg = new Random();
            Roll();
            updatePictures();
        }

        void Roll()
        {
            for (int i = 0; i < NumDice; ++i)
            {
                numbers[i] = rg.Next(1, 7);
            }
        }

        void updatePictures()
        {
            PictureBox[] boxes = { this.pictureBox1, this.pictureBox2, this.pictureBox3 };
            string[] picnames = { "dice_1.png", "dice_2.png", "dice_3.png", "dice_4.png", "dice_5.png", "dice_6.png" };
            for (int i = 0; i < NumDice; ++i)
            {
                boxes[i].Load(@"..\..\Resources\" + picnames[numbers[i] - 1]);
            }
        }

        private void InitializeDice()
        {
            //_Dice = new Dice();
            //_Dice.RollingChanged += OnDiceRollingChanged;
            // _Dice.Rolled += OnDiceRolled;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void OnDiceRolled(object sender, EventArgs e)
        {
            buttonRoll.Enabled = true;
        }
  

        private void buttonRoll_Click(object sender, EventArgs e)
        {
            Roll();
            updatePictures();

        }
    }

}
