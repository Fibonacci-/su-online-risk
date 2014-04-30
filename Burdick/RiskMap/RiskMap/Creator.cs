using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SUOnlineRisk;

namespace RiskMap
{
    public partial class Creator : Form
    {
        // Lists
        List<Continent> listContinents = new List<Continent>();
        List<Territory> listTerritories = new List<Territory>();
        List<ReinforcementCard> listCards = new List<ReinforcementCard>();

        // Constructor
        public Creator()
        {
            InitializeComponent();
        }

        ////////////////////////////////////
        // LOADING AND SAVING AND WHATNOT //
        ////////////////////////////////////

        // Saving map
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog tempDialogue = new SaveFileDialog();
            tempDialogue.Filter = "Risk Map|*.xml";
            tempDialogue.Title = "Save Map";
            if (tempDialogue.ShowDialog() == DialogResult.OK)
            {
                Map tempMap = new Map("");
                for (int i = 0; i < listContinents.Count; i++) tempMap.addContinent(listContinents.ElementAt(i));
                for (int i = 0; i < listTerritories.Count; i++)
                {
                    Territory t = listTerritories[i];
                    tempMap.addTerritory(t, t.returnContinent());
                    ReinforcementCard card = listCards[i];
                    tempMap.addCard(card);
                }
                tempMap.saveMap(tempDialogue.FileName);
            }
        }

        // Loading an existing map
        private void loadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog tempDialogue = new OpenFileDialog();
            tempDialogue.Filter = "Risk Map|*.xml";
            tempDialogue.Title = "Load Map";
            if (tempDialogue.ShowDialog() == DialogResult.OK)
            {
                listTerritories.Clear();
                listContinents.Clear();
                listCards.Clear();
                Map temp;
                temp = Map.loadMap(tempDialogue.FileName);
                editContinentToolStripMenuItem.DropDownItems.Clear();
                for (int i = 0; i < temp.getAllContinents().Count; i++)
                {
                    listContinents.Add(temp.getOneContinent(i));
                    editContinentToolStripMenuItem.DropDownItems.Add(listContinents.ElementAt(listContinents.Count - 1).getName(), null, new EventHandler(EditMenuContinent));
                }
                editTerritoryToolStripMenuItem.DropDownItems.Clear();
                for (int i = 0; i < temp.getAllTerritories().Count; i++)
                {
                    listTerritories.Add(temp.getOneTerritory(i));
                    editTerritoryToolStripMenuItem.DropDownItems.Add(listTerritories.ElementAt(listTerritories.Count - 1).getName(), null, new EventHandler(EditMenuTerritory));
                }
                for (int i = 0; i < temp.getAllCards(0).Count; i++)
                {
                    listCards.Add(temp.getCard(i));
                }
            }
        }
        
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
                //pictureBox1.Top = 50;
                //pictureBox1.Left = 20;
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
            // old
            /*this.editContinentToolStripMenuItem.DropDownItems.Clear();
            for (int i = 0; i < listContinents.Count; i++)
            {
                this.editContinentToolStripMenuItem.DropDownItems.Add(listContinents.ElementAt(i).getName(), null, new EventHandler(EditMenuContinent));
            }*/
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
        private void addContinent(String name, int bonus) {
            listContinents.Add(new Continent(name, bonus, listContinents.Count + 1));
            editContinentToolStripMenuItem.DropDownItems.Add(listContinents.ElementAt(listContinents.Count - 1).getName(), null, new EventHandler(EditMenuContinent));
        }

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
                
                int tempX = MousePosition.X - pictureBox1.Left;
                int tempY = MousePosition.Y - pictureBox1.Top - this.menuStrip1.Location.Y;
                //int tempX = MousePosition.X * mapBitmap.Width / pictureBox1.Width;
                //int tempY = MousePosition.Y * mapBitmap.Height / pictureBox1.Height;
                MessageBox.Show("You clicked " + tempX + ", " + tempY);
                DialTerrInfo temp = new DialTerrInfo(listContinents, listTerritories);
                temp.Visible = false;
                temp.ShowDialog();
                if (temp.wasOkay) {
                    addTerritory(tempX, tempY, temp.newName, temp.newCont);
                    Territory t = listTerritories.ElementAt(listTerritories.Count - 1);
                    t.neighNew(temp.newNeighbors);
                    ReinforcemenCardUnit unit;
                    if ((listTerritories.Count - 1) % 3 == 0)
                    {
                        unit = ReinforcemenCardUnit.Infantry;
                    }
                    else if ((listTerritories.Count - 1) % 3 == 1)
                    {
                        unit = ReinforcemenCardUnit.Cavalry;
                    }
                    else
                    {
                        unit = ReinforcemenCardUnit.Artillery;
                    }
                    ReinforcementCard card = new ReinforcementCard(t.getName(), unit);
                    listCards.Add(card);
                }
            }
        }

        // Editing territory
        private void editTerritoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // old
            /*this.editTerritoryToolStripMenuItem.DropDownItems.Clear();
            for (int i = 0; i < listTerritories.Count; i++)
            {
                this.editTerritoryToolStripMenuItem.DropDownItems.Add(listTerritories.ElementAt(i).getName(), null, new EventHandler(EditMenuTerritory));
            }*/
        }
        private void EditMenuTerritory(object sender, EventArgs e)
        {
            int terrAt = editTerritoryToolStripMenuItem.DropDownItems.IndexOf((ToolStripMenuItem)sender);
            Territory tempCont = listTerritories.ElementAt(terrAt);
            DialTerrInfo temp = new DialTerrInfo(listContinents, listTerritories);
            temp.setForEdit(tempCont.getName(), listContinents.IndexOf(tempCont.returnContinent()));
            for (int i = 0; i < listTerritories.Count; i++)
            {
                if (tempCont.returnNeighbors().IndexOf(listTerritories.ElementAt(i)) > -1) temp.checkBox(i);
            }
            temp.Visible = false;
            temp.ShowDialog();
            if (temp.wasOkay) {
                tempCont.newName(temp.newName);
                tempCont.newCont(temp.newCont);
                tempCont.neighNew(temp.newNeighbors);
            }
        }

        // Adding territory
        private void addTerritory(int x, int y, String name, Continent cont) {
            listTerritories.Add(new Territory(x, y, name, listTerritories.Count + 1, cont, null));
            editTerritoryToolStripMenuItem.DropDownItems.Add(listTerritories.ElementAt(listTerritories.Count - 1).getName(), null, new EventHandler(EditMenuTerritory));
        }
    }
}