using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUOnlineRisk
{
    public enum ReinforcemenCardUnit { Infantry, Cavalry, Artillery, Wild };
    [Serializable]
    public class ReinforcementCard
    {
        public String TerritoryName;
        public ReinforcemenCardUnit UnitType;
        public int CardId;
        public ReinforcementCard(String Territory, ReinforcemenCardUnit Unit)
        {
            TerritoryName = Territory;
            UnitType = Unit;
            CardId = _Counter;
            _Counter++;
        }
        private static int _Counter = 0;
        public static int numArmies(ReinforcemenCardUnit type)
        {
            if (type == ReinforcemenCardUnit.Infantry) return 1;
            else if (type == ReinforcemenCardUnit.Cavalry) return 5;
            else if (type == ReinforcemenCardUnit.Artillery) return 10;
            else return 0;
        }

        public override bool Equals(object obj)
        {
            ReinforcementCard card = obj as ReinforcementCard;
            if (card == null) return false;
            if (this.CardId != card.CardId) return false;
            else if (this.TerritoryName != card.TerritoryName) return false;
            else if (this.UnitType != card.UnitType) return false;
            else return true;
        }
    }
}
