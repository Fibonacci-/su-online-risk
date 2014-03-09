using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SUOnlineRisk;
using System.Collections.Generic;
namespace LewisGUIExample
{
    public partial class MainForm : Form
    {
        Map map;
        string mapfilename = @"..\..\SimpleRisk.map";
        bool selectedTerr = false;
        Territory attackFrom;
        List<Player> players;
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
            players = new List<Player>();
            players.Add(new Player("Attacker", Color.Blue, map));
            players.Add(new Player("Defender", Color.Red, map));
            for (int i = 0; i < map.getAllTerritories().Count; i++)
            {
                if (i <= (map.getAllTerritories().Count / 2))
                {
                    map.getOneTerritory(i).setOwner("Attacker");
                }
                else
                {
                    map.getOneTerritory(i).setOwner("Defender");
                }
            }
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = new Point();
            p.X = e.Location.X * map.getBitmap().Width / pictureBox1.Width;
            p.Y = e.Location.Y * map.getBitmap().Height / pictureBox1.Height;
            //MessageBox.Show("Clicked at(" + p.X + ", " + p.Y + ")");
            if (selectedTerr == false)
            {
                foreach (Territory t in map.getAllTerritories())
                {
                    if (((p.X > t.returnX() - 10) && (p.X < t.returnX() + 10)) && ((p.Y > t.returnY() - 10) && (p.Y < t.returnY() + 10)))
                    {
                        foreach (Player thisp in players)
                        {
                            string name = thisp.nickname;
                            if (t.getOwner() == name)
                            {
                                if (MessageBox.Show("You selected " + t.getName() + ". Is this correct?",
                                    "Yes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    MessageBox.Show("Please select an adjacent enemy territory to attack.");
                                    selectedTerr = true;
                                    attackFrom = t;
                                }
                            }
                        }
                        break;

                    }
                }
            }
            else
            {
                foreach (Territory t in attackFrom.returnNeighbors())
                {
                    if (((p.X > t.returnX() - 10) && (p.X < t.returnX() + 10)) && ((p.Y > t.returnY() - 10) && (p.Y < t.returnY() + 10)))
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
                            MessageBox.Show("You cannot attack this territory. You own this territory.");
                            //pop-up an error message.
                        }
                        break;
                    }
                }
            }
        }

        //attack button
        private void button1_Click(object sender, EventArgs e)
        {
            DiceForm f2 = new DiceForm();
            f2.setOptionAttack();
            f2.ShowDialog();
        }

        //defend button
        private void button2_Click(object sender, EventArgs e)
        {
            DiceRollDefend f3 = new DiceRollDefend();
            f3.setOptionDefend();
            f3.ShowDialog();
        }

    }

}
