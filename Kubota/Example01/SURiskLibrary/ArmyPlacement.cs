using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUOnlineRisk
{
    /*
     * this is a utility class used in some of messages to describe how armies are moved. To move n armies from Territory A to B, 
     * use two ArmyPlacement objects: one with (A, n) and the other (B, -n).
     */
    [Serializable]
    public class ArmyPlacement : ICloneable
    {
        public string territory;
        public string owner;
        public int numArmies;
        public ArmyPlacement()
        {
            territory = "unknown";
            owner = "na";
            numArmies = 0;
        }
        public ArmyPlacement(string t, string o, int n)
        {
            territory = t;
            owner = o;
            numArmies = n;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
