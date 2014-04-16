using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SUOnlineRisk;

namespace RiskMap
{
    class Program
    {
        [STAThread]
        static void Main() {
            // Initializing creator
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Creator());

            // Testing map loading
            /*OpenFileDialog tempDialogue = new OpenFileDialog();
            tempDialogue.Filter = "Risk Map|*.xml";
            tempDialogue.Title = "Load Map";
            if (tempDialogue.ShowDialog() == DialogResult.OK) {
                Console.WriteLine("File selected");
                Map temp;
                Console.WriteLine("Map class created");
                temp = Map.loadMap(tempDialogue.FileName);
                Console.WriteLine("Map successfully loaded");
                for (int i = 0; i < temp.getAllContinents().Count; i++)
                    Console.WriteLine("Continent " + temp.getAllContinents()[i] + " present");
                for (int i = 0; i < temp.getAllTerritories().Count; i++)
                    Console.WriteLine("Territory " + temp.getAllTerritories()[i] + " present");
                for (int i = 0; i < temp.getAllCards(0).Count; i++)
                    Console.WriteLine("Reinforcement card for " + temp.getCard(i).TerritoryName + " with bonus " + temp.getCard(i).UnitType + " present");
            }*/
        }
    }
}
