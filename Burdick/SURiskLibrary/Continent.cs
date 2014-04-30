using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUOnlineRisk
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

        // Adding a territory.
        public void addTerritory(Territory t)
        {
            territories.Add(t);
        }

        // Getting territory list.
        public List<Territory> getTerritories() { return (territories); }

        // Checkking for a specific territory.
        public bool isTerritory(Territory czech) { return (territories.Contains(czech)); }

        // To string.
        public override string ToString()
        {
            return name;
        }

        // New name.
        public void newName(string nue) { this.name = nue; }

        // New bonus.
        public void newBonus(int nue) { this.unitBonus = nue; }
    }
}
