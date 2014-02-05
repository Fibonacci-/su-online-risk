using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Army
    {
        public Player owner;
        public int units;
        public Territory territory;
 
        public Army(Player player, int u, Territory t)
        {
            owner = player;
            units = u;
            territory = t;
        }
        public Player getOwner()
        {
            return owner;
        }
        public int getArmyCount(int art, int cav, int inf)
        {
            return units;//cav worth 5 inf and art worth 10 inf, need to adjust for that
        }
        public Territory getTerritory() // confused about how or better yet what exactly to do here. 
        {
            return territory;
        }
        public void setTerritory(Territory t)
        {
            territory = t;
        }
    }
}
