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
        public ArmyPlacement(string t="unknown", string o="na", int n=0)
        {
            territory = t;
            owner = o;
            numArmies = n;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public override bool Equals(object obj)
        {
            ArmyPlacement p = obj as ArmyPlacement;
            if (p == null) return false;
            if (this.numArmies != p.numArmies) return false;
            else if (this.owner != p.owner) return false;
            else if (this.territory != p.territory) return false;
            else return true;
        }
    }
}
