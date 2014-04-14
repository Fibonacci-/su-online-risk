using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SUOnlineRisk
{
    public class Player
    {
        //list of variables
        public String nickname;
        public Color ArmyColor;
        public int numOfTerritories;
        public int ArmySize;
        public List<Territory> Territories;
        public List<ReinforcementCard> ReinforcementCards;
        public List<Army> armies;
        public MainState state; // maintains which state the player is in: e.g. Attack, Reinforce, Conquer, etc.

        public Map map;

        public Player(String username, Color ArmyColor, Map map)
        {
            nickname = username;
            this.ArmyColor = ArmyColor;
            this.numOfTerritories = 0;
            this.ArmySize = 0;
            this.map = map;
            Territories = new List<Territory>();
            ReinforcementCards = new List<ReinforcementCard>();
            armies = new List<Army>();
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


        /*
         * The following virtual methods can be override to provide custom behavior for a bot and a human player.
         */
        virtual public Message Update(Message incoming)
        {
            //Since the Player shares the same map with the Server, nothing needs to be done
            //Else do something like this.
            /*if(incoming is ArmyPlacementMessage)
            {
                foreach (ArmyPlacement a in ((ArmyPlacementMessage)incoming).territory_army)
                {
                    map.getTerritory(a.territory).numArmies += a.numArmies;
                }
            }*/
            return new Message(MainState.Update, nickname);  //acknowledgement only
        }
        virtual public Message Initialize(Message incoming)
        {
            ArmyPlacementMessage message = new ArmyPlacementMessage(MainState.Initialize, nickname);
            //Figure out which territory to place an army.
            Territory selected = null;
            message.territory_army.Add(new ArmyPlacement(selected.getName(), incoming.playerName, 1));
            return message;
        }
        virtual public Message Distribute(Message incoming)
        {
            ArmyPlacementMessage message = new ArmyPlacementMessage(MainState.Distribute, nickname);
            //Figure out which territories to place the remaining armies.
            Territory selected = null;
            //message.territory_army.Add(new ArmyPlacement(selected.getName(), 2));
            //message.territory_army.Add(new ArmyPlacement(selected.getName(), 3));
            //message.territory_army.Add(new ArmyPlacement(selected.getName(), 4));
            return message;
        }

        virtual public Message NewArmies(Message message)
        {
            if (message is ArmyPlacementMessage)
            {
                //collect the new armies
            }
            return new Message(MainState.NewArmies, nickname);  //acknowledgement only
        }

        virtual public Message TradeCard(Message message)
        {
            //decide if you want to trade in reinforcement cards.
            TradeCardMessage outgoing = new TradeCardMessage(MainState.TradeCard, nickname);
            outgoing.cardIds = new int[3];
            return outgoing;
        }
        virtual public Message AdditionalArmies(Message message)
        {
            if (message is ArmyPlacementMessage)
            {
                //collect the new additional armies - obtained by trading in cards
            }
            return new Message(MainState.AdditionalArmies, nickname); //acknowledgement only.
        }
        virtual public Message Attack(Message incoming)
        {
            //If attacking
            {
                AttackMessage message = new AttackMessage(MainState.Attack, nickname);
                //Figure out from which territory and to which territory the attck will be made.
                message.from = null; //set the territory to attack from
                message.to = null; //set the territory to attack to
                return message;
            }
            //Else attack is done
            {
                AttackDoneMessage message = new AttackDoneMessage(MainState.AttackDone, nickname);
                return message;
            }
        }

        virtual public Message Roll(Message incoming)
        {
            RollMessage message = new RollMessage(MainState.Roll, nickname);
            //Roll dice and return the result in.
            message.roll = new int[2]; //the size dependent on how many dice to use
            Random rg = new Random();
            for (int i = 0; i < message.roll.Length; ++i)
            {
                message.roll[i] = rg.Next(1, 7);
            }
            return message;
        }

        virtual public Message Conquer(Message incoming)
        {
            if(incoming is ArmyPlacementMessage)
            {
                int numArmyToMove = 1; //decide on how many armies to move
                //the first ArmyPlacement is From territory and
                ((ArmyPlacementMessage)incoming).territory_army[0].numArmies = -numArmyToMove;
                //the second ArmyPlacement is To territory.
                ((ArmyPlacementMessage)incoming).territory_army[1].numArmies = numArmyToMove;
            }
            return new Message(MainState.Update, nickname);  //acknowledgement only
        }

        virtual public Message ReinforcementCard(Message message)
        {
            if (message is ReinforcementCardMessage)
            {
                //store a reinforcement card
            }
            return new Message(MainState.ReinforcementCard, nickname);
        }
        virtual public Message Reinforce(Message message)
        {
            //determine how new armies (both new and additional) are to be placed.
            //store the information in the message.
            ArmyPlacementMessage outgoing = new ArmyPlacementMessage(MainState.Reinforce, nickname);
            //outgoing.territory_army; //store the placement in this list.
            return outgoing;
        }
        virtual public Message Fortify(Message message)
        {
            //determine how you want to re-distribute the armies.
            //store the information in the message. Use negative if armies are taken away and positive if they are added.
            ArmyPlacementMessage outgoing = new ArmyPlacementMessage(MainState.Reinforce, nickname);
            //outgoing.territory_army; //store the redistribution in this list.
            return outgoing;
        }
    }
}
