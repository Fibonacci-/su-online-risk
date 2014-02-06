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
using RiskMap;

namespace LewisGUIExample
{
    public partial class Form1 : Form
    {
        Map map;
        string mapfilename = @"..\..\SimpleRisk.map";
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
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = new Point();
            p.X = e.Location.X * map.getBitmap().Width / pictureBox1.Width;
            p.Y = e.Location.Y * map.getBitmap().Height / pictureBox1.Height;
            MessageBox.Show("Clicked at(" + p.X + ", " + p.Y + ")");
        }
    }
}
