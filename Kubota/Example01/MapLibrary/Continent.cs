using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiskMap
{
    [Serializable]
    public class Continent
    {
        // Variables.
        protected string name;
        protected int unitBonus;
        protected List<Territory> territories;
        protected int id;

        // Constructor.
        public Continent(string name, int unit, int id, List<Territory> ter = null)
        {
            this.name = name;
            this.unitBonus = unit;
            this.id = id;
            if (ter != null)
            {
                this.territories = ter;
            }
            else
            {
                this.territories = new List<Territory>();
            }
        }

        // Getting name.
        public string getName() { return (name); }

        // Getting unit bonus.
        public int getBonus() { return (unitBonus); }

        // Checking to see if the whole continent has been captured by a single player.
        public bool isCaptured()
        {
            // Will work on this later.
            return (false);
        }

        //adding a territory
        public void addTerritory(Territory t)
        {
            territories.Add(t);
        }

        // Getting territory list.
        public List<Territory> getTerritories() { return (territories); }

        // Checkking for a specific territory.
        public bool isTerritory(Territory czech) { return (territories.Contains(czech)); }
    }
}
