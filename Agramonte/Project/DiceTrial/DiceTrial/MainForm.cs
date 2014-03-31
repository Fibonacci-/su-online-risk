using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SUOnlineRisk;

namespace LewisGUIExample
{
    public partial class MainForm : Form
    {
        Map map;
        string mapfilename = @"..\..\SimpleRisk.map";
        bool selectedTerr = false;
        Territory attackFrom;
        public MainForm()
        {
            InitializeComponent();
            if (File.Exists(mapfilename) == false)
            {
                MessageBox.Show(mapfilename + " does not exist.");
                Application.Exit();
            }
            map = Map.loadMap(mapfilename);
            this.pictureBox1.Image = map.getBitmap();
            map.addPlayer(new Player("Attacker", Color.Blue));
            map.addPlayer(new Player("Defender", Color.Red));
            for (int i = 0; i < map.getAllTerritories().Count; i++)
            {
                if (i < (map.getAllTerritories().Count / 2))
                {
                    map.getOneTerritory(i).setOwner("Attacker");
                }
                else
                {
                    map.getOneTerritory(i).setOwner("Defender");
                }
            }
        }

        //getting message to display after opening form
        private void MainForm_Shown(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Click and hold on territory to attack from. Release mouse on territory you wish to attack.");
        }

        //player selects territory to attack
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Point p = new Point();
            p.X = e.Location.X * map.getBitmap().Width / pictureBox1.Width;
            p.Y = e.Location.Y * map.getBitmap().Height / pictureBox1.Height;
            // if an attacking territory was not properly selected, skip the loop
            if (attackFrom == null)
            {
                selectedTerr = false;
            }

            //checking for a territory based on mouse location on user release
            if (attackFrom != null)
            {
                foreach (Territory t in attackFrom.returnNeighbors())
                {
                    if (selectedTerr == false)
                    {
                        continue;
                    }
                    if (((p.X > t.returnX() - 20) && (p.X < t.returnX() + 20)) && ((p.Y > t.returnY() - 20) && (p.Y < t.returnY() + 20)))
                    {
                        if (t.getOwner() != attackFrom.getOwner())
                        {
                            if (MessageBox.Show("You selected " + t.getName() + ". Is this correct?",
                                "Yes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                //opens new form for attacking
                                selectedTerr = false;
                                DiceForm Attack = new DiceForm();
                                Attack.setOptionAttack();
                                Attack.ShowDialog();
                            }
                        }
                        else
                        {
                            MessageBox.Show("You cannot attack this territory. You own this territory. Please select a territory to attack from and a territory to attack.");
                            //pop-up an error message.
                        }

                        break;
                    }
                }
            }
        }

        //player selects territory to attack from
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = new Point();
            p.X = e.Location.X * map.getBitmap().Width / pictureBox1.Width;
            p.Y = e.Location.Y * map.getBitmap().Height / pictureBox1.Height;
            attackFrom = null;

            //check for an army to attack from based on user input. mouse pressed down. 
            if (selectedTerr == false)
            {
                foreach (Territory t in map.getAllTerritories())
                {
                    if (((p.X > t.returnX() - 20) && (p.X < t.returnX() + 20)) && ((p.Y > t.returnY() - 20) && (p.Y < t.returnY() + 20)))
                    {

                        string name = "Attacker";
                        if (t.getOwner() == name && t.numArmies > 1)
                        {
                            Console.Write(t.getName());

                                selectedTerr = true;
                                attackFrom = t;
                        }
                         //need a minimum of 2 to attack. 
                        else if (t.numArmies <= 1)
                        {
                            MessageBox.Show("You cannot attack from this territory. You do not have sufficient units on this territory.");
                        }
                    }
                }
            }
        }

    }

}
