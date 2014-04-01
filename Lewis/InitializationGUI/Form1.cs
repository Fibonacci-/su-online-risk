using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SUOnlineRisk;

namespace InitializationGUI
{
    public partial class Form1 : Form
    {
        Map map;
        Player player;
        Territory moveFrom;
        Territory moveTo;
        int count = 0;
        int numInitialArmies;
        int moves = 0;
        int j;
        string mapfilename = @"..\..\SimpleRisk.map";
        Dictionary<Territory, Label> labelMap = new Dictionary<Territory, Label>();
        Label state = new Label();
        public Form1()
        {
            InitializeComponent();
            if (File.Exists(mapfilename) == false)
            {
                MessageBox.Show(mapfilename + " does not exist.");
                Application.Exit();
            }
            map = Map.loadMap(mapfilename);
            player = new Player("Gary", Color.Red, map);
            this.pictureBox1.Image = map.getBitmap();

            foreach (Territory t in map.getAllTerritories())
            {
                int tx = t.returnX();
                int ty = t.returnY();
                int wx = tx * pictureBox1.Width / map.getBitmap().Width - pictureBox1.Width/30;
                int wy = ty * pictureBox1.Height / map.getBitmap().Height - pictureBox1.Height / 30;
                Label ta = new Label();
                ta.Left = wx;
                ta.Top = wy;
                ta.Text = t.getName() + " " + t.getOwner() + " " + t.numArmies;
                ta.AutoSize = true;
                this.Controls.Add(ta);
                ta.BringToFront();

                labelMap[t] = ta;
            }
            //Take out this line
            player.Territories = new List<Territory>();
            state.Left = 700 * pictureBox1.Width / map.getBitmap().Width - pictureBox1.Width / 30;
            state.Top = 600 * pictureBox1.Height / map.getBitmap().Height - pictureBox1.Height / 30;
            numInitialArmies = 10;
            state.Text = "Initialization: " + numInitialArmies;
            state.AutoSize = true;
            this.Controls.Add(state);
            state.BringToFront();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (map == null) return;

            foreach (Territory t in map.getAllTerritories())
            {
                int tx = t.returnX();
                int ty = t.returnY();
                int wx = tx * pictureBox1.Width / map.getBitmap().Width - pictureBox1.Width / 30;
                int wy = ty * pictureBox1.Height / map.getBitmap().Height - pictureBox1.Height / 30;
                Label ta = labelMap[t];
                ta.Left = wx;
                ta.Top = wy;
            }
            state.Left = 700 * pictureBox1.Width / map.getBitmap().Width - pictureBox1.Width / 30;
            state.Top = 600 * pictureBox1.Height / map.getBitmap().Height - pictureBox1.Height / 30;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //gets where the user clicked
            Point p = new Point();
            p.X = e.Location.X * map.getBitmap().Width / pictureBox1.Width;
            p.Y = e.Location.Y * map.getBitmap().Height / pictureBox1.Height;
            //allows the user to occupy 4 armies initially. 
            //This wil be changed to deal with taking turns in the final product
            if (count < 4)
            {
                foreach (Territory t in map.getAllTerritories())
                {
                    if (count == map.getAllTerritories().Count())
                    {
                        break;
                    }
                    //checks to see if the user clicked in a territory's parameters
                    if (((p.X > t.returnX() - 30) && (p.X < t.returnX() + 30)) && ((p.Y > t.returnY() - 30) && (p.Y < t.returnY() + 30)))
                    {
                        if (t.getOwner() == "unoccupied")
                        {
                            if (MessageBox.Show("You selected " + t.getName() + ". Is this correct?",
                                "Yes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                t.setOwner(player.getName());
                                t.numArmies = t.numArmies + 1;
                                Label owner = labelMap[t];
                                owner.Text = t.getName() + " " + t.getOwner() + " " + t.numArmies;
                                Army army = new Army(player, 1, t);
                                player.addTerritory(t, army, 1);
                                count++;
                                numInitialArmies--;
                                state.Text = "Initialization: " + numInitialArmies;
                                if (count == 4)
                                {
                                    state.Text = "Distribution: " + numInitialArmies;
                                }
                            }
                            break;
                        }
                        else
                        {
                            MessageBox.Show("This territory is already occupied");
                        }
                    }
                }
            }
            else if (numInitialArmies > 0)
            {
                foreach (Territory t in map.getAllTerritories())
                {
                    if (((p.X > t.returnX() - 30) && (p.X < t.returnX() + 30)) && ((p.Y > t.returnY() - 30) && (p.Y < t.returnY() + 30)))
                    {
                        if (t.getOwner() == player.getName())
                        {
                            if (MessageBox.Show("You selected " + t.getName() + ". Is this correct?",
                                "Yes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                t.numArmies = t.numArmies + 1;
                                numInitialArmies--;
                                Label update = labelMap[t];
                                update.Text = t.getName() + " " + t.getOwner() + " " + t.numArmies;
                                state.Text = "Distribution: " + numInitialArmies;
                                if (numInitialArmies == 0)
                                {
                                    state.Text = "Fortify";
                                    MessageBox.Show("Click the territory you want to move units from, drag it to the territory you want to move them to, and release the mouse");
                                    moves++;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("You do not own this territory and cannot place an army here");
                        }
                        break;
                    }
                }
            }
            else if (moves == 1)
            {
                foreach (Territory t in map.getAllTerritories())
                {
                    if (((p.X > t.returnX() - 30) && (p.X < t.returnX() + 30)) && ((p.Y > t.returnY() - 30) && (p.Y < t.returnY() + 30)))
                    {
                        if (t.getOwner() != player.getName())
                        {
                            MessageBox.Show("You do not own this territory");
                            break;
                        }
                        else
                        {
                            moveFrom = t;
                        }
                    }
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Point p = new Point();
            p.X = e.Location.X * map.getBitmap().Width / pictureBox1.Width;
            p.Y = e.Location.Y * map.getBitmap().Height / pictureBox1.Height;
            if (moves == 1)
            {
                foreach (Territory t in map.getAllTerritories())
                {
                    if (((p.X > t.returnX() - 30) && (p.X < t.returnX() + 30)) && ((p.Y > t.returnY() - 30) && (p.Y < t.returnY() + 30)))
                    {
                        if (t == moveFrom)
                        {
                            MessageBox.Show("You cannot move armies from a territory into that same territory");
                        }
                        else if (t.getOwner() != player.getName())
                        {
                            MessageBox.Show("You do not own this territory");
                        }
                        else if (MessageBox.Show("You want to move units from " + moveFrom.getName() + " to " + t.getName() + ". Is this correct?",
                                "Yes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            moveTo = t;
                            FortifyMessage move = new FortifyMessage(moveFrom.numArmies - 1);
                            if (move.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                t.numArmies = t.numArmies + move.getArmiesToMove();
                                moveFrom.numArmies = moveFrom.numArmies - move.getArmiesToMove();
                                Label owner = labelMap[t];
                                owner.Text = t.getName() + " " + t.getOwner() + " " + t.numArmies;
                                Label owner2 = labelMap[moveFrom];
                                owner2.Text = moveFrom.getName() + " " + moveFrom.getOwner() + " " + moveFrom.numArmies;
                                moves++;
                                state.Text = "Next Turn";
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}
