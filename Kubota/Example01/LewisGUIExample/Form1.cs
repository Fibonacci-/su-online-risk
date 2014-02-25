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

namespace LewisGUIExample
{
    public partial class Form1 : Form
    {
        Map map;
        string mapfilename = @"..\..\..\MapStuff\SimpleRisk.map";
        Dictionary<Territory,Label> territoryLabels;
        public Form1()
        {
            InitializeComponent();
            if(File.Exists(mapfilename) == false)
            {
                MessageBox.Show(mapfilename + " does not exist.");
                Application.Exit();
            }
            map = Map.loadMap(mapfilename);
            this.pictureBox1.Image = map.getBitmap();
            this.Controls.Remove(this.pictureBox1);
            territoryLabels = new Dictionary<Territory,Label>();
            foreach(Territory t in map.getAllTerritories())
            {
                Label label = new Label();
                label.Text = t.getOwner() + ": " + t.numArmies;
                //label.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                label.Name = t.getOwner();
                label.Size = new System.Drawing.Size(100, 20);
                this.Controls.Add(label);
                territoryLabels[t] = label;
            }
            relocateLabel();
            this.Controls.Add(this.pictureBox1);
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = new Point();
            p.X = e.Location.X * map.getBitmap().Width / pictureBox1.Width;
            p.Y = e.Location.Y * map.getBitmap().Height / pictureBox1.Height;
            //MessageBox.Show("Clicked at(" + p.X + ", " + p.Y + ")");
            float rad = 10;
            foreach (Territory t in map.getAllTerritories())
            {
                float dx = p.X - t.returnX();
                float dy = p.Y - t.returnY();
                if (Math.Abs(dx) < rad && Math.Abs(dy) < rad)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        MessageBox.Show("Attacking from territory " + t.getName() + " owned by " + t.getOwner()+ ".");
                    }
                    else if(e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        MessageBox.Show("Attacking to territory " + t.getName() + " owned by " + t.getOwner() + ".");
                    }
                    break;
                }
            }
            
        }

        void relocateLabel()
        {
            foreach(Territory t in territoryLabels.Keys)
            {
                int x = t.returnX() * pictureBox1.Width / map.getBitmap().Width;
                int y = t.returnY() * pictureBox1.Height / map.getBitmap().Height;
                territoryLabels[t].Location = new Point(x, y + 40);
            }

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            relocateLabel();
        }
    }
}
