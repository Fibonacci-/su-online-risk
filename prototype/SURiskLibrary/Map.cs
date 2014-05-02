using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace SUOnlineRisk
{
    [Serializable]
    public class Map
    {
        // Variables.
        public List<Continent> continents;
        public List<Territory> territories;
        public List<ReinforcementCard> cards;
        public string fileName;
        public Bitmap bitmap;

        // Constructor.
        public Map(string file)
        {
            this.fileName = file;
            this.continents = new List<Continent>();
            this.territories = new List<Territory>();
            try
            {
                bitmap = new Bitmap(this.fileName);
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.Error.WriteLine("Map: File " + this.fileName + " does not exists.");
            }
            try
            {
                bitmap = new Bitmap(this.fileName);
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.Error.WriteLine("Map: File " + this.fileName + " does not exists.");
            }
        }

        // Other constructor?
        public Map(Bitmap bitmap)
        {
            this.fileName = "Not given";
            this.continents = new List<Continent>();
            this.territories = new List<Territory>();
            this.bitmap = (Bitmap) bitmap.Clone();
        }

        // Adding a continent.
        public void addContinent(Continent c)
        {
            continents.Add(c);
        }

        // Adding a territory.
        public bool addTerritory(Territory t, Continent c)
        {
            if(continents.Exists(x => x.getName()==c.getName()))
            {
                Continent continent = continents.Find(x=>x.getName()==c.getName());
                continent.addTerritory(t);
                territories.Add(t);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Adding a reinforcement card.
        public void addCard(ReinforcementCard temp) {
            cards.Add(temp);
        }

        /*
         * find a territory by its name.
         * return null if there is no such territory.
         */
        public Territory getTerritory(string name)
        {
            if(territories.Exists(x=> x.getName()==name))
            {
                return territories.Find(x => x.getName() == name);
            }
            else
            {
                return null;
            }
        }

        // Returning a specific continent (given a position in the list).
        public Continent getOneContinent(int i) { return continents.ElementAt(i); }

        // Returning a specific territory (given a position in the list).
        public Territory getOneTerritory(int i) { return territories.ElementAt(i); }

        // Returning all continents.
        public List<Continent> getAllContinents() { return continents; }

        // Returning all territories.
        public List<Territory> getAllTerritories() { return territories; }

        // Returning the bitmap.
        public Bitmap getBitmap() { return bitmap; }

        // Setting the bitmap.
        public void setBitmap(Bitmap temp) { this.bitmap = temp; }

        // Returning all reinforcement cards.
        public List<ReinforcementCard> getAllCards() { return cards; }

        // Returning a specific reinforcement card (given a position in the list).
        public ReinforcementCard getCard(int i) { return cards.ElementAt(i); }

        // Saving the map.
        public void saveMap(string path)
        {
            Stream stream = null;
            try
            {
                stream = File.Create(path);
            }
            catch(System.IO.DirectoryNotFoundException)
            {
                Console.Error.WriteLine("Path, " + path + ", does not exist.");
            }
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(stream, this); 
            }
            catch(System.Runtime.Serialization.SerializationException)
            {
                Console.Error.WriteLine("Serialization failed. Could not save the map");
            }
            finally
            {
                stream.Close();
            }
        }

        // Loading the necessary files and deciphering the mapcode.
        public static Map loadMap(string path)
        {
            Stream stream = null;
            try
            {
                stream = File.OpenRead(path);
            }
            catch(Exception ex)
            {
                if(ex is System.IO.DirectoryNotFoundException || ex is System.IO.FileNotFoundException)
                {
                    Console.Error.WriteLine("Path, " + path + ", does not exist.");
                }
                throw;
            }
            BinaryFormatter formatter = new BinaryFormatter();
            Map map = null;
            try
            {
                map = (Map) formatter.Deserialize(stream); 
            }
            catch(System.Runtime.Serialization.SerializationException)
            {
                //Console.Error.WriteLine("Serialization failed. Could not save the map");
                throw;         
            }
            finally
            {
                stream.Close();
            }
            return map;
        }
    }
}