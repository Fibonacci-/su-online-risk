using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project
{
    class Continent
    {
        // Variables.
        protected string name;
        protected int unitBonus;
        protected List<Territory> territories;

        // Constructor.
        private Continent(string name, int unit, List<Territory> ter)
        {
            this.name = name;
            this.unitBonus = unit;
            this.territories = ter;
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

        // Getting territory list.
        public List<Territory> getTerritories() { return (territories); }

        // Checkking for a specific territory.
        public bool isTerritory(Territory czech) { return (territories.Contains(czech)); }
    }
}
