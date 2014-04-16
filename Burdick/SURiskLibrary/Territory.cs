using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUOnlineRisk
{
    [Serializable]
    public class Territory
    {
        // Variables.
        protected int x;
        protected int y;
        protected int radius = 5;
        protected Continent continent;
        protected List<Territory> neighbors;
        protected string name;
        protected int id;
        protected string owner;
        public int numArmies
        {
            get;
            set;
        }

        // Constructor.
        public Territory(int x, int y, string name, int id, Continent continent, List<Territory> links = null)
        {
            this.x = x;
            this.y = y;
            this.name = name;
            this.id = id;
            this.continent = continent;
            if (links == null)
            {
                this.neighbors = new List<Territory>();
            }
            else
            {
                this.neighbors = links;
            }
            this.owner = "unoccupied";
        }

        // Getting X.
        public int returnX() { return x; }

        // Getting Y.
        public int returnY() { return y; }

        // Getting radius for some reason.
        public int returnRadius() { return radius; }

        // Getting list of neighbors.
        public List<Territory> returnNeighbors() { return neighbors; }

        // Getting continent.
        public Continent returnContinent() { return continent; }

        // Checking if neighbor.
        public bool isNeighbor(Territory check) { return neighbors.Contains(check); }

        // Checking if continent.
        public bool isContinent(Continent check) { return (check == continent); }

        // Getting name.
        public string getName() { return (name); }

        //add a neighbor
        public bool addNeighbor(Territory t)
        {
            if (neighbors.Contains(t)) return false;
            else
            {
                neighbors.Add(t);
                return true;
            }
        }

        public string getOwner()
        {
            return owner;
        }
        public void setOwner(string owner)
        {
            this.owner = owner;
        }

        // To string
        public override string ToString()
        {
            return name;
        }

        // New name.
        public void newName(string nue) { this.name = nue; }

        // New continent.
        public void newCont(Continent nue) { this.continent = nue; }

        // Clearing neighbors.
        public void neighClear() { neighbors.Clear(); }

        // Some new neighbors just moved next door, Billy. They have a boy just about your age.
        //public void neighNew(Territory nue) { neighbors.Add(nue); }
        public void neighNew(List<Territory> nue)
        {
            neighClear();
            for (int i = 0; i < nue.Count; i++) this.neighbors.Add(nue.ElementAt(i));
        }
    }
}