using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SUOnlineRisk
{
    public class Computer : Player
    {
        // private data
        private Random rand;
        int numDiceRolled;
        List<ArmyPlacement> newArmies;

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

        public override RiskMessage Update(RiskMessage incoming)
        {
//            System.Console.Out.WriteLine("Computer::Update ");
//            Console.Out.WriteLine("     Computer::Update for " + nickname);
            foreach (ArmyPlacement p in incoming.territory_army)
            {
                Territory selected = map.getTerritory(p.territory);
                if (selected == null)
                {
                    Console.Out.WriteLine("Computer::Update - selected null territory");
                }
                selected.numArmies += p.numArmies;
                selected.setOwner(p.owner);
            }
            Console.Out.Write("Update " + nickname + ":\t");
            foreach (Territory t in this.map.getAllTerritories())
            {
                Console.Out.Write(t.getOwner() + ":" + t.numArmies + "\t");
            }
            Console.Out.WriteLine();
            return incoming;
        }

        public override RiskMessage Initialize(RiskMessage incoming)
        {
            RiskMessage outgoing = new RiskMessage(MainState.Initialize, nickname);
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
                outgoing.territory_army.Add(new ArmyPlacement(selected.getName(), nickname, 1));
            }
            return outgoing;
        }

        public override RiskMessage Distribute(RiskMessage incoming)
        {
            RiskMessage message = new RiskMessage(MainState.Distribute, nickname);
            // choose a random territory from Territories and place the army there
            // ------------------------------------------------------------------
            List<Territory> owned = map.getAllTerritories().FindAll(x => x.getOwner() == nickname);
            Territory selected = owned.ElementAt(rand.Next(0, owned.Count));
            // ------------------------------------------------------------------
            // place one army in the selected territory
//            addArmy(selected, 1);
            // put a new ArmyPlacement in the outgoing message
            message.territory_army.Add(new ArmyPlacement(selected.getName(), nickname, 1));
            return message;
        }

        public override RiskMessage TradeCard(RiskMessage message)
        {
            RiskMessage outgoing = new RiskMessage(MainState.TradeCard, nickname);
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

        public override RiskMessage NewArmies(RiskMessage message)
        {
            // collect the new armies - save the list of ArmyPlacements in newArmies
            newArmies = message.territory_army;
            return new RiskMessage(MainState.NewArmies, nickname);  //acknowledgement only
        }

        public override RiskMessage Reinforce(RiskMessage message)
        {
            RiskMessage outgoing = new RiskMessage(MainState.Reinforce, nickname);
            foreach (ArmyPlacement p in newArmies)
            {
                Territory selected;
                // the armies may HAVE to be placed on a specific territory (bonus from cards whose territory I own)
                if (p.territory != "any")
                {
                    selected = map.getTerritory(p.territory);
                    if (selected == null)
                    {
                        Console.Out.WriteLine("Computer::Reinforce - null territory selected");
                    }
                }
                else // choose a random territory
                {
                    // ------------------------------------------------------------------
                    List<Territory> owned = map.getAllTerritories().FindAll(x => x.getOwner() == nickname);
                    selected = owned.ElementAt(rand.Next(0, owned.Count));
                    // ------------------------------------------------------------------
                }
//              selected.numArmies += p.numArmies;
                outgoing.territory_army.Add(new ArmyPlacement(selected.getName(), nickname, p.numArmies));
            }
            newArmies.Clear();
            return outgoing;
        }

        public override RiskMessage Attack(RiskMessage incoming)
        {
            List<Territory> owned = map.getAllTerritories().FindAll(t => t.getOwner() == nickname);
            List<RiskMessage> possibleAttacks = new List<RiskMessage>();
            foreach (Territory t in owned)
            {
                if (t.numArmies > 1) // cannot attack with only 1 army
                {
                    List<Territory> neighbors = t.returnNeighbors();
                    foreach (Territory n in neighbors)
                    {
                        if (n.getOwner() != nickname) // if someone else owns this neighbor
                        {
                            if (n.numArmies < t.numArmies) // and the neighbor has fewer armies than this territory
                            {
                                // maybe attack it
                                possibleAttacks.Add(new RiskMessage(MainState.Attack, nickname));
                                possibleAttacks.Last().from = t.getName();
                                possibleAttacks.Last().to = n.getName();
                            }
                        }
                    }
                }
            }

            // decide which attack to make, if any
            int m = rand.Next(0, 2);
            if (nickname == "Tom")
            {
                m = 1;
            }
            if (m==0 || possibleAttacks.Count == 0)
            {
                RiskMessage message = new RiskMessage(MainState.AttackDone, nickname);
                state = MainState.AttackDone;
                return message;
            }
            else
            {
                // pick a random attack from the possibleAttacks list
                state = MainState.Attack;
                RiskMessage outgoing = possibleAttacks.ElementAt(rand.Next(0, possibleAttacks.Count));
                if (outgoing == null || map.getTerritory(outgoing.from).getOwner() != nickname)
                {
                    Console.Out.WriteLine("Computer::Attack - null outgoing message or attacking from unowned territory");
                }
                return outgoing;
            }
        }

        public override RiskMessage Roll(RiskMessage incoming)
        {
            RiskMessage outgoing = new RiskMessage(MainState.Roll, nickname);
            // if attacking, roll 3 dice or one less than the number of armies attacking
            if (map.getTerritory(incoming.from).getOwner() == nickname) // i own the attacking territory -> i am the attacker
            {
                int armies = map.getTerritory(incoming.from).numArmies;
                if (armies > 3)
                {
                    outgoing.roll = new int[3];
                    numDiceRolled = 3;
                }
                else if (armies == 3)
                {
                    outgoing.roll = new int[2];
                    numDiceRolled = 2;
                }
                else if (armies == 2)
                {
                    outgoing.roll = new int[1];
                    numDiceRolled = 1;
                }
                if(armies < 2 || incoming.from == null || map.getTerritory(incoming.from) == null)
                {
                    Console.Out.WriteLine("Stop");
                }
            }
            // if defending, roll 2 dice
            else
            {
                int armies = map.getTerritory(incoming.to).numArmies;
                if (armies > 1)
                {
                    outgoing.roll = new int[2];
                    numDiceRolled = 2;
                }
                else if (armies == 1)
                {
                    outgoing.roll = new int[1];
                    numDiceRolled = 1;
                }
                else
                {
                    Console.Out.WriteLine("Stop");
                }
            }

            for (int i = 0; i < outgoing.roll.Length; i++)
            {
                outgoing.roll[i] = rand.Next(1, 7);
            }

            return outgoing;
        }

        public override RiskMessage AttackOutcome(RiskMessage incoming)
        {
            return Update(incoming);
        }

        public override RiskMessage Conquer(RiskMessage incoming)
        {
            RiskMessage outgoing = new RiskMessage(MainState.Conquer, nickname);
            // find the conquered territory
            Territory conquered = map.getTerritory(incoming.territory_army[1].territory);

            // decide how many armies to move
            int totalArmies = map.getTerritory(incoming.territory_army[0].territory).numArmies;
            int numArmyToMove = rand.Next(numDiceRolled, totalArmies);

            // the first ArmyPlacement is From territory (attacker)
            outgoing.territory_army.Add(new ArmyPlacement(incoming.from, nickname, -numArmyToMove));

            // the second ArmyPlacement is To territory (newly conquered)
            outgoing.territory_army.Add(new ArmyPlacement(incoming.to, nickname, numArmyToMove));
            return outgoing;
        }

        public override RiskMessage ReinforcementCard(RiskMessage message)
        {
            if (message.card != null)
            {
                addCard(message.card);
            }
            return new RiskMessage(MainState.ReinforcementCard, nickname);
        }

        public override RiskMessage Fortify(RiskMessage message)
        {
            RiskMessage outgoing = new RiskMessage(MainState.Fortify, nickname);
            // i don't see a good reason to redistribute for the random implementation
            return outgoing;
        }

    }
}
