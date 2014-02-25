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
        string mapfilename = @"..\..\..\MapStuff\SimpleRisk.map";
        public Form1()
        {
            InitializeComponent();
            if (File.Exists(mapfilename) == false)
            {
                MessageBox.Show(mapfilename + " does not exist.");
                Application.Exit();
            }
            map = Map.loadMap(mapfilename);
            this.pictureBox1.Image = map.getBitmap();
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            Point p = new Point();
            p.X = e.Location.X * map.getBitmap().Width / pictureBox1.Width;
            p.Y = e.Location.Y * map.getBitmap().Height / pictureBox1.Height;
            //MessageBox.Show("Clicked at(" + p.X + ", " + p.Y + ")");
            foreach (Territory t in map.getAllTerritories())
            {
                if (((p.X > t.returnX() - 10) && (p.X < t.returnX() + 10)) && ((p.Y > t.returnY() - 10) && (p.Y < t.returnY() + 10)))
                {
                    //MessageBox.Show("Clicked at(" + p.X + ", " + p.Y + ")");
                    if (t.getOwner() == "unoccupied")
                    {
                        if (MessageBox.Show("You selected " + t.getName() + ". Is this correct?",
                            "Yes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            t.setOwner("gary");
                            t.numArmies = t.numArmies + 1;
                            MessageBox.Show("You now have " + t.numArmies + " on " + t.getName());
                        }
                        break;
                    }
                }
            }
        }
    }
}
