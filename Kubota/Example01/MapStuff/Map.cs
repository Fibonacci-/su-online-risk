using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiskMap
{
    [Serializable]
    class Map
    {
        // Variables.
        protected List<Continent> continents;
        protected List<Territory> territories;
        protected string fileName;
        //protected imageFile;

        // Constructor.
        private Map(string file)
        {
            // Note:    File string does not include extensions. This is so we only need one
            //          variable, and also because the filename of the image and code are
            //          going to be the same.

            this.fileName = file;
            // Will be done next week.
        }

        // Returning a specific continent.
        public Continent getOneContinent(int i) { return continents.ElementAt(i); }

        // Returning a specific territory.
        public Territory getOneTerritory(int i) { return territories.ElementAt(i); }

        // Returning all continents.
        public List<Continent> getAllContinents() { return continents; }

        // Returning all territories.
        public List<Territory> getAllTerritories() { return territories; }

        // Saving the map.
        protected void saveMap()
        {
            // Will work on this later.
        }

        // Loading the necessary files and deciphering the mapcode.
        protected bool loadMap()
        {
            // Loading files.
            if (!File.Exists(fileName + ".txt") || !File.Exists(fileName + ".png")) return false;
            else
            {
                FileStream fileRead = File.OpenRead(fileName + ".txt");
                // Load image file later.
            }

            // Deserializing map code.

            // Ending.
            return true;
        }
    }
}