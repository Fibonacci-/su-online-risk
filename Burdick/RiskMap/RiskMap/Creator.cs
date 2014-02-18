using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RiskMap
{
    public partial class Creator : Form
    {
        // Lists
        List<Continent> listContinents = new List<Continent>();
        List<Territory> listTerritories = new List<Territory>();

        // Constructor
        public Creator()
        {
            InitializeComponent();
        }

        ////////////////////////////////////
        // LOADING AND SAVING AND WHATNOT //
        ////////////////////////////////////

        // Opening image
        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog tempDialogue = new OpenFileDialog();
            tempDialogue.Filter = "Images|*.png;*.gif;*.jpg;*.jpeg;*.bmp";
            tempDialogue.Title = "You are to selection one image files ! ! !";
            if (tempDialogue.ShowDialog() == DialogResult.OK)
            {
                Bitmap tempBitmap = new Bitmap(tempDialogue.OpenFile());
                pictureBox1.Width = tempBitmap.Width;
                pictureBox1.Height = tempBitmap.Height;
                pictureBox1.Image = tempBitmap;
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        // TRY OUR NEW INTER-CONTINENTAL BREAKFAST HAHAHA I AM THE KING OF JOKES //
        ///////////////////////////////////////////////////////////////////////////

        // Creating new continent
        private void newContinentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialContInfo temp = new DialContInfo();
            temp.Visible = false;
            temp.ShowDialog();
            if (temp.returnOkay()) addContinent(temp.returnName(), temp.returnBonus());
        }

        // Editing continent
        private void editContinentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.editContinentToolStripMenuItem.DropDownItems.Clear();
            for (int i = 0; i < listContinents.Count; i++)
            {
                this.editContinentToolStripMenuItem.DropDownItems.Add(listContinents.ElementAt(i).getName(), null, new EventHandler(EditMenuContinent));
            }
        }
        private void EditMenuContinent(object sender, EventArgs e) {
            int contAt = editContinentToolStripMenuItem.DropDownItems.IndexOf((ToolStripMenuItem)sender);
            Continent tempCont = listContinents.ElementAt(contAt);
            DialContInfo temp = new DialContInfo();
            temp.setForEdit(tempCont.getName(), tempCont.getBonus());
            temp.Visible = false;
            temp.ShowDialog();
            if (temp.returnOkay()) {
                tempCont.newName(temp.returnName());
                tempCont.newBonus(temp.returnBonus());
            }
        }

        // Adding continent
        private void addContinent(String name, int bonus) { listContinents.Add(new Continent(name, bonus)); }

        //////////////////////////////////
        // ULTIMTAE TERRITORIAL CONTROL //
        //////////////////////////////////

        // Creating new territory
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (listContinents.Count == 0) {
                MessageBox.Show("You need to create at least one continent before you can create a territory.");
            }
            else {
                int tempX = MousePosition.X;
                int tempY = MousePosition.Y;
                DialTerrInfo temp = new DialTerrInfo(listContinents, listTerritories);
                temp.Visible = false;
                temp.ShowDialog();
                if (temp.wasOkay) {
                    addTerritory(tempX, tempY, temp.newName, temp.newCont);
                    listTerritories.ElementAt(listTerritories.Count - 1).neighNew(temp.newNeighbors);
                }
            }
        }

        // Editing territory
        private void editTerritoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.editTerritoryToolStripMenuItem.DropDownItems.Clear();
            for (int i = 0; i < listTerritories.Count; i++)
            {
                this.editTerritoryToolStripMenuItem.DropDownItems.Add(listTerritories.ElementAt(i).getName(), null, new EventHandler(EditMenuTerritory));
            }
        }
        private void EditMenuTerritory(object sender, EventArgs e)
        {
            int terrAt = editTerritoryToolStripMenuItem.DropDownItems.IndexOf((ToolStripMenuItem)sender);
            Territory tempCont = listTerritories.ElementAt(terrAt);
            DialTerrInfo temp = new DialTerrInfo(listContinents, listTerritories);
            temp.setForEdit(tempCont.getName(), listContinents.IndexOf(tempCont.returnContinent()));
            temp.Visible = false;
            temp.ShowDialog();
            if (temp.wasOkay) {
                tempCont.newName(temp.newName);
                tempCont.newCont(temp.newCont);
                tempCont.neighNew(temp.newNeighbors);
            }
        }

        // Adding territory
        private void addTerritory(int x, int y, String name, Continent cont) { listTerritories.Add(new Territory(x, y, name, cont, null)); }
    }
}