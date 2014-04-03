using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SUOnlineRisk;

namespace ComputerImplementation02
{
    public class Computer : Player
    {
        // private data
        private Random rand;
        int numDiceRolled;
        List<ArmyPlacement> newArmies;

        // public data

        // constructor
        public Computer(string UserName, Color ArmyColor, Map map)
            : base(UserName, ArmyColor, map)
        {
            rand = new Random();
        }

        // public methods
        // override virtual methods in Player class:

        // PURPOSE: these methods are called by the Client during individual phases of gameplay
        // PARAMETERS: the incoming message provides information necessary for completing the phase
        //             the incoming message should be more specific for some methods
        // RETURN: the message that is returned is (usually) just an acknowledgement of the action that was just taken
        //         the MainState of the outgoing message should (usually) be the same as the MainState of the incoming message

        public override Message Update(Message incoming)
        {
            //Since the Player shares the same map with the Server, nothing needs to be done
            return base.Update(incoming);
        }

        public override Message Initialize(Message incoming)
        {
            ArmyPlacementMessage message = new ArmyPlacementMessage(MainState.Initialize, nickname);
            // Figure out which Territories are unoccupied
            List<Territory> ts = map.getAllTerritories();
            List<Territory> untaken = new List<Territory>();
            foreach(Territory t in ts)
            {
                if(t.getOwner() == "unoccupied")
                {
                    untaken.Add(t);
                }
            }
            if (untaken.Count>0)
            {
                // Choose a territory on which to place an army. (randomly)
                // ------------------------------------------------------------------
                Territory selected = untaken.ElementAt(rand.Next(0, untaken.Count));
                // ------------------------------------------------------------------
                // add the Territory to my list
                this.Territories.Add(selected);
                selected.setOwner(this.nickname);
                addArmy(selected, 1);
                message.territory_army.Add(new ArmyPlacement(selected.getName(), incoming.playerName, 1));
            }
            return message;
        }

        public override Message Distribute(Message incoming)
        {
            ArmyPlacementMessage message = new ArmyPlacementMessage(MainState.Distribute, nickname);
            // choose a random territory from Territories and place the army there
            // ------------------------------------------------------------------
            Territory selected = Territories.ElementAt(rand.Next(0, Territories.Count));
            // ------------------------------------------------------------------
            // place one army in the selected territory
            addArmy(selected, 1);
            // put a new ArmyPlacement in the outgoing message
            message.territory_army.Add(new ArmyPlacement(selected.getName(), nickname, 1));
            return message;
        }

        public override Message TradeCard(Message message)
        {
            TradeCardMessage outgoing = new TradeCardMessage(MainState.TradeCard, nickname);
            // only trade in cards when we have to
            if (ReinforcementCards.Count() >= 5)
            {
                outgoing.cardIds = new int[3];
                List<ReinforcementCard> toTrade = new List<ReinforcementCard>();
                // count the number of cards of each type
                int[] counter = new int[4];
                foreach (ReinforcementCard card in ReinforcementCards)
                {
                    if (card.UnitType == ReinforcemenCardUnit.Artillery)
                        counter[0]++;
                    else if (card.UnitType == ReinforcemenCardUnit.Cavalry)
                        counter[1]++;
                    else if (card.UnitType == ReinforcemenCardUnit.Infantry)
                        counter[2]++;
                    else
                        counter[3]++;
                }
                // if there are 3+ of any type (including any wilds), they are our set
                ReinforcemenCardUnit set = ReinforcemenCardUnit.Wild;
                if (counter[0] + counter[3] >= 3)
                    set = ReinforcemenCardUnit.Artillery;
                else if (counter[1] + counter[3] >= 3)
                    set = ReinforcemenCardUnit.Cavalry;
                else if (counter[2] + counter[3] >= 3)
                    set = ReinforcemenCardUnit.Infantry;

                // if there are not 3+ of any type, the set contains one of each type
                if (set == ReinforcemenCardUnit.Wild)
                {
                    Boolean bfound = false;
                    // find an artillery
                    foreach (ReinforcementCard card in ReinforcementCards)
                    {
                        if (card.UnitType == ReinforcemenCardUnit.Artillery)
                        {
                            outgoing.cardIds[0] = card.CardId;
                            toTrade.Add(card);
                            bfound = true;
                            break;
                        }
                    }
                    // if we couldn't find one, there must be a wild
                    if (!bfound)
                    {
                        foreach (ReinforcementCard card in ReinforcementCards)
                        {
                            if (card.UnitType == ReinforcemenCardUnit.Wild)
                            {
                                outgoing.cardIds[0] = card.CardId;
                                toTrade.Add(card);
                                break;
                            }
                        }
                    }

                    bfound = false;
                    // find a cavalry
                    foreach (ReinforcementCard card in ReinforcementCards)
                    {
                        if (card.UnitType == ReinforcemenCardUnit.Cavalry)
                        {
                            outgoing.cardIds[1] = card.CardId;
                            toTrade.Add(card);
                            bfound = true;
                            break;
                        }
                    }
                    // if we couldn't find one, there must be a wild
                    if (!bfound)
                    {
                        foreach (ReinforcementCard card in ReinforcementCards)
                        {
                            if (card.UnitType == ReinforcemenCardUnit.Wild)
                            {
                                outgoing.cardIds[1] = card.CardId;
                                toTrade.Add(card);
                                break;
                            }
                        }
                    }

                    bfound = false;
                    // find an infantry
                    foreach (ReinforcementCard card in ReinforcementCards)
                    {
                        if (card.UnitType == ReinforcemenCardUnit.Infantry)
                        {
                            outgoing.cardIds[2] = card.CardId;
                            toTrade.Add(card);
                            bfound = true;
                            break;
                        }
                    }
                    // if we couldn't find one, there must be a wild
                    if (!bfound)
                    {
                        foreach (ReinforcementCard card in ReinforcementCards)
                        {
                            if (card.UnitType == ReinforcemenCardUnit.Wild)
                            {
                                outgoing.cardIds[2] = card.CardId;
                                toTrade.Add(card);
                                break;
                            }
                        }
                    }
                }

                else // the set contains 3 of one type of card
                {
                    int i = 0;
                    foreach (ReinforcementCard card in ReinforcementCards)
                    {
                        if (card.UnitType == set)
                        {
                            outgoing.cardIds[i] = card.CardId;
                            i++;
                            toTrade.Add(card);
                        }
                        if (i == 3)
                            break;
                    }
                    if (i < 2)
                    {
                        foreach (ReinforcementCard card in ReinforcementCards)
                        {
                            if (card.UnitType == ReinforcemenCardUnit.Wild)
                            {
                                outgoing.cardIds[i] = card.CardId;
                                i++;
                                toTrade.Add(card);
                            }
                            if (i == 3)
                                break;
                        }
                    }
                }

                // remove the cards to be traded from ReinfocementCards
                foreach (ReinforcementCard card in toTrade)
                {
                    ReinforcementCards.Remove(card);
                }
            }
            return outgoing;
        }

        public override Message NewArmies(Message message)
        {
            if (message is ArmyPlacementMessage)
            {
                // collect the new armies - save the list of ArmyPlacements in newArmies
                newArmies = ((ArmyPlacementMessage)message).territory_army;
            }
            return new Message(MainState.NewArmies, nickname);  //acknowledgement only
        }

        public override Message Reinforce(Message message)
        {
            ArmyPlacementMessage outgoing = new ArmyPlacementMessage(MainState.Reinforce, nickname);
            foreach (ArmyPlacement p in newArmies)
            {
                Territory selected;
                // the armies may HAVE to be placed on a specific territory (bonus from cards whose territory I own)
                if (p.territory != "unoccupied")
                {
                    selected = map.getAllTerritories().Find(t => t.getName() == p.territory);
                }
                else // choose a random territory
                {
                    // ------------------------------------------------------------------
                    selected = Territories.ElementAt(rand.Next(0, Territories.Count));
                    // ------------------------------------------------------------------
                }
                selected.numArmies += p.numArmies;
                outgoing.territory_army.Add(new ArmyPlacement(selected.getName(), nickname, p.numArmies));
            }
            newArmies.Clear();
            return outgoing;
        }

        public override Message Attack(Message incoming)
        {
            List<AttackMessage> possibleAttacks = new List<AttackMessage>();
            foreach (Territory t in Territories)
            {
                List<Territory> neighbors = t.returnNeighbors();
                foreach (Territory n in neighbors)
                {
                    if (n.getOwner() != nickname) // if someone else owns this neighbor
                    {
                        if (n.numArmies < t.numArmies) // and the neighbor has fewer armies than this territory
                        {
                            // maybe attack it
                            possibleAttacks.Add(new AttackMessage(MainState.Attack, nickname));
                            possibleAttacks.Last().from = t.getName();
                            possibleAttacks.Last().to = n.getName();
                        }
                    }
                }
            }

            // decide which attack to make, if any
            int m = rand.Next(0, 2);
            if (m==0 || possibleAttacks.Count == 0)
            {
                AttackDoneMessage message = new AttackDoneMessage(MainState.AttackDone, nickname);
                state = MainState.AttackDone;
                return message;
            }
            else
            {
                // pick a random attack from the possibleAttacks list
                state = MainState.Attack;
                return possibleAttacks.ElementAt(rand.Next(possibleAttacks.Count));
            }
        }

        public override Message Roll(Message incoming)
        {
            RollMessage message = new RollMessage(MainState.Roll, nickname);
            // if attacking, roll 3 dice
            if (state == MainState.Attack)
            {
                message.roll = new int[3];
                numDiceRolled = 3;
            }
            // if defending, roll 2 dice
            else
            {
                message.roll = new int[2];
                numDiceRolled = 2;
            }

            for (int i = 0; i < message.roll.Length; i++)
            {
                message.roll[i] = rand.Next(1, 7);
            }

            return message;
        }

        public override Message AttackOutcome(Message incoming)
        {
            // update the number of armies on my territory as a result of the attack
            foreach(ArmyPlacement p in ((ArmyPlacementMessage)incoming).territory_army)
            {
                map.getAllTerritories().Find(t => t.getName() == p.territory).numArmies += p.numArmies;
            }
            return new Message(MainState.AttackOutcome, nickname); // acknowledgement
        }

        public override Message Conquer(Message incoming)
        {
            if (incoming is ArmyPlacementMessage)
            {
                int totalArmies = Territories.Find(t => t.getName() == ((ArmyPlacementMessage)incoming).territory_army[0].territory).numArmies;
                int numArmyToMove = rand.Next(numDiceRolled, totalArmies);
                // the first ArmyPlacement is From territory (attacker)
                ((ArmyPlacementMessage)incoming).territory_army[0].numArmies = -numArmyToMove;
                // the second ArmyPlacement is To territory (newly conquered)
                ((ArmyPlacementMessage)incoming).territory_army[1].numArmies = numArmyToMove;
                Territories.Add(Territories.Find(t => t.getName() == ((ArmyPlacementMessage)incoming).territory_army[1].territory));
                // TODO: did we actually move any armies?
                //       have we added the new territory to my list?
            }
            return new Message(MainState.Conquer, nickname);
        }

        public override Message ReinforcementCard(Message message)
        {
            if (message is ReinforcementCardMessage)
            {
                addCard(((ReinforcementCardMessage)message).card);
            }
            return new Message(MainState.ReinforcementCard, nickname);
        }

        public override Message Fortify(Message message)
        {
            ArmyPlacementMessage outgoing = new ArmyPlacementMessage(MainState.Fortify, nickname);
            // i don't see a good reason to redistribute for the random implementation
            return outgoing;
        }

    }
}
