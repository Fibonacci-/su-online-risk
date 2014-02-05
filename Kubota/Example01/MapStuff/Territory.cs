using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiskMap
{
    [Serializable]
    class Territory
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

        // Constructor.
        private Territory(int x, int y, string name, Continent continent, List<Territory> links)
        {
            this.x = x;
            this.y = y;
            this.name = name;
            this.continent = continent;
            this.neighbors = links;
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
    }
}