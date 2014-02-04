using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Project
{
    class Player
    {
        //list of variables
        public String nickname;
        public Color ArmyColor;
        public int numOfTerritories;
        public int ArmySize;
        public List<Territory> Territories;
        public List<ReinforcementCard> ReinforcementCards;
        public List<Army> armies;

        public Player(String username, Color ArmyColor)
        {
            nickname = username;
            this.ArmyColor = ArmyColor;
            this.numOfTerritories = 0;
            this.ArmySize = 0;
        }

        public String getName()
        {
            return nickname;
        }
        public Color getColor()
        {
            return ArmyColor;
        }
        public List<Territory> getTerritories()
        {
            return Territories;
        }
        public List<ReinforcementCard> getCards()
        {
            return ReinforcementCards;
        }

        public void addTerritory(Territory newTerr, Army army, int units)
        {
            Territories.Add(newTerr);
            armies.Add(army);
            army.territory = newTerr;
            army.units = units;
        }
        public void addCard(ReinforcementCard newCard)
        {
            ReinforcementCards.Add(newCard);
        }

        public bool addArmy(Territory t, int armyUnits)
        {
			//if the player already owns the territory
			if(Territories.Contains(t))
			{
			//go through each army and check if its on the desired territory, add units to it
				for(int i = 0; i < armies.Count; i++)
				{
					if(armies[i].territory == t)
					{
						armies[i].units += armyUnits;
					}
				}
				return true;
			}
			return false;
        }

        public bool removeArmy(Territory t, int armyUnits)
        {
			//if the player already owns the territory
			if(Territories.Contains(t))
			{
			//go through each army and check if its on the desired territory, remove units from it
				for(int i = 0; i < armies.Count; i++)
				{
					if(armies[i].territory == t && armyUnits != armies[i].units)
					{
						armies[i].units -= armyUnits;
						
					}
				}
				return true;
			}
			return false;
        }
        public Boolean removeTerritory(Territory remTerr)
        {
            //check if array contains the string if so remove it, otherwise return
            if (Territories.Contains(remTerr) == true)
            {
                Territories.Remove(remTerr);
                return true;
            }
            for (int i = 0; i < armies.Count; ++i)
            {
                if (armies[i].territory == remTerr)
                {
                    this.ArmySize -= armies[i].units;
					armies.Remove(armies[i]);
                    break;
                }
            }
            return false;
        }
        public Boolean removeCard(ReinforcementCard remCard)
        {
            //check if array contains the string if so remove it, otherwise return
            if (ReinforcementCards.Contains(remCard) == true)
            {
                ReinforcementCards.Remove(remCard);
                return true;
            }

            return false;
        }
    }
}
