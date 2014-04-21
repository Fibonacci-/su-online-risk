using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;

namespace SUOnlineRisk
{
    public class RiskSequencer
    {
        List<RiskClient> clients = new List<RiskClient>();
        List<ReinforcementCard> reinforcementCards; //a whole set of reinforcement cards.
        List<ReinforcementCard> usedPiles = new List<ReinforcementCard>();
        //Dictionary<MockClient, MainState> states = new Dictionary<MockClient, MainState>();

        RiskClient current; //currently active client/player
        MainState currentState; //current state

        //these are used during the attack phase
        RiskClient attacker;
        RiskClient defender;
        Territory attackFrom;
        Territory attackTo;
        int[] attackerRoll;
        int[] defenderRoll;
        //set true when a territory is conquered.
        bool bConquered = false;
        // set true when a player is eliminated.
        bool bEliminated = false;
        bool bCardsTraded = false;
        List<ArmyPlacement> battleResult = new List<ArmyPlacement>(); //TK. used to tell participants the outcome of a battle

        //used during the initialization phase
        int initialized = 0;
        // used during distribution phase
        int distributed = 0;

        //the number of armies to be awarded in sequence
        static int[] tradedArmyCounts = { 4, 6, 8, 10, 12, 15 };
        //this track how many times reinforcement cards are traded. This will be used to get the number of armies to be awarded.
        int tradedCount = 0;
        List<ReinforcementCard> tradedCards = new List<ReinforcementCard>();

        //right now the same map is shared among the server and players. For implementation across remote computers, 
        //this needs to be changed.
        public Map map
        {
            get;
            set;
        }

        /*
         * Generate ReinforcementCards for each territory in the map.
         */
        public void generateReinforcementCards(Map map)
        {
            reinforcementCards = new List<ReinforcementCard>();
            List<Territory> territories = map.getAllTerritories();
            int n = 0;
            foreach (Territory t in map.getAllTerritories())
            {
                ReinforcementCard r = null;
                switch (n)
                {
                    case (0):
                        r = new ReinforcementCard(t.getName(), ReinforcemenCardUnit.Infantry);
                        break;
                    case (1):
                        r = new ReinforcementCard(t.getName(), ReinforcemenCardUnit.Cavalry);
                        break;
                    case (2):
                        r = new ReinforcementCard(t.getName(), ReinforcemenCardUnit.Artillery);
                        break;
                }
                reinforcementCards.Add(r);
                n = ++n % 3;
            }
        }

        /*
         * Check if the entire map is wipied out by a single player.
         */
        bool gameOver(Map map)
        {
            String winner = null;
            Boolean endGame = true;
            foreach (Territory t in map.getAllTerritories())
            {
                if (winner == null) winner = t.getOwner();
                if (t.getOwner() != winner) endGame = false;
                if (t.getOwner() == "unoccupied") endGame = false;
            }
            return endGame;
        }

        public void addClient(RiskClient client)
        {
            clients.Add(client);
            //states[client] = MainState.Start;
            if (current == null) current = client;
        }
        void updateMap(List<ArmyPlacement> placement)
        {
            foreach (ArmyPlacement a in placement)
            {
                Territory t = map.getTerritory(a.territory);
                t.numArmies += a.numArmies;
                t.setOwner(a.owner);
            }
        }

        /*
         * this method figures out how many armies the player receives based on the number of territories occupied by the player
         * and continents occupied by the player.
         */
        int numArmiesCollected(string playerName)
        {
            int terrs = 0;
            foreach (Territory T in map.getAllTerritories())
            {
                if (T.getOwner() == playerName)
                {
                    terrs++;
                }
            }
            int numCollect = terrs / 3;
            if (numCollect < 3) numCollect = 3;
            foreach (Continent C in map.getAllContinents())
            {
                bool owned = true;
                foreach (Territory T in C.getTerritories())
                {
                    if (T.getOwner() != playerName) owned = false;
                }
                if (owned)
                {
                    numCollect += C.getBonus();
                }
            }
            return numCollect;
        }
        /*
         * this method takes a message from a client/player and update the map and other variables accordingly.
         */
        public void Update(RiskMessage message)
        {
            List<ArmyPlacement> changes = new List<ArmyPlacement>();
            RiskClient client = clients.Find(x => x.name == message.playerName);
            if(client == null)
            {
                throw new NullReferenceException("Player " + message.playerName + " does not exist.");
            }
            //int idx = clients.FindIndex(x => x == current);

            //TK. When Client is on a remote Server, the state change from Attack to AttackDone does not propagate to the Server.
            //We need to take care of the change right here.
            if(client.state == MainState.Attack && message.state == MainState.AttackDone)
            {
                client.state = MainState.AttackDone;
            }

            if (client.state == MainState.Initialize || client.state == MainState.Distribute)
            {
                if (message.territory_army.Count > 0) // false for distribution, why??
                {
                    if (client.state == MainState.Initialize)
                    {
                        initialized++;
                    }
                    else // client.state == MainState.Distribute
                    {
                        distributed++;
                    }
                    changes.Add(message.territory_army[0]);
                }
            }
            else if (client.state == MainState.Reinforce || client.state == MainState.Fortify)
            {
                foreach (ArmyPlacement a in message.territory_army)
                {
                    changes.Add(a);
                }
            }
            else if (client.state == MainState.Conquer)
            {
                foreach (ArmyPlacement p in message.territory_army)
                {
                    changes.Add(p);
                }
                bConquered = true;
            }
            else if (client.state == MainState.NewArmies)
            {
                //no change to the map
            }
            else if (client.state == MainState.TradeCard)
            {
                if (message.cardIds != null) //there is a set of cards to be traded for armies.
                {
                    //                    foreach (int id in message.cardIds)
                    //                    {
                    //                        ReinforcementCard card = usedPiles.Find(x => x.CardId == id);
                    //                        ArmyPlacement a = new ArmyPlacement();
                    //                        a.numArmies = ReinforcementCard.numArmies(card.UnitType);
                    //                        a.territory = card.TerritoryName;
                    //                        changes.Add(a);
                    //                        tradedCards.Add(card);
                    //                    }
                    //                    tradedCount++;
                    bCardsTraded = true;
                    foreach (int id in message.cardIds)
                    {
                        ReinforcementCard card = usedPiles.Find(x => x.CardId == id);
                        tradedCards.Add(card);
                    }
                }
            }
            else if (client.state == MainState.AdditionalArmies)
            {
                //no change to the map
            }
            else if (client.state == MainState.Attack)
            {
                Territory from = map.getTerritory(message.from);
                Territory to = map.getTerritory(message.to);
                Console.Out.WriteLine("owners: " + from.getOwner() + " " + to.getOwner());
                Console.Out.WriteLine("Battle: " + from.getName() + "[" + from.getOwner() + "] => " + to.getName() + "[" + to.getOwner() + "]");
                if (from.getOwner() != current.name)
                {
                    Console.Out.WriteLine("Player attacking from a territory they don't own");
                }
                if (from.getOwner() == message.playerName && to.getOwner() != message.playerName)
                {
                    if (from.isNeighbor(to) && to.isNeighbor(from))
                    {
                        attackFrom = from;
                        attackTo = to;
                        attacker = clients.Find(x => x.name == from.getOwner());
                        defender = clients.Find(x => x.name == to.getOwner());
                        if (attacker == null || defender == null)
                        {
                            Console.Out.WriteLine("MockServer::Update - Attack state - null attacker or defender");
                        }
                    }
                }
            }
            else if (client.state == MainState.AttackDone)
            {
                //erase them so that we do not mistakenly start another battle
                attackFrom = null;
                attackTo = null;
                attacker = null;
                defender = null;
                attackerRoll = null;
                defenderRoll = null;
            }
            else if (client.state == MainState.Roll)
            {
                if (message.playerName == attacker.name)
                {
                    attackerRoll = (int[])message.roll.Clone();
                    Array.Sort(attackerRoll);
                }
                else if (message.playerName == defender.name)
                {
                    defenderRoll = (int[])message.roll.Clone();
                    Array.Sort(defenderRoll);
                }
                if (attackerRoll != null && defenderRoll != null)
                {
                    //Decide how many armies are lost from each side 
                    int attackerLoss = 0; //set this one
                    int defenderLoss = 0; //set this one
                    int aIndex = attackerRoll.Length - 1;
                    for (int i = defenderRoll.Length - 1; i > -1; i--)
                    {
                        if (aIndex == -1) break;
                        if (attackerRoll[aIndex] > defenderRoll[i])
                        {
                            defenderLoss++;
                        }
                        else attackerLoss++;

                        if (defenderLoss == attackTo.numArmies)
                        {
                            break; // no more armies left in the defending territory
                        }
                    }
                    ArmyPlacement a = new ArmyPlacement(attackFrom.getName(), attacker.name, -attackerLoss);
                    ArmyPlacement b = new ArmyPlacement(attackTo.getName(), defender.name, -defenderLoss);
                    changes.Add(a);
                    changes.Add(b);
                    battleResult.Clear();
                    battleResult.Add(a);
                    battleResult.Add(b);
                }
            }

            //go ahead and update the map
            updateMap(changes);

            /*
             * TK
             * Having server broadcasting changes requires non-http (peer-to-peer) protocol and harder to implement.
             * Now, each client (player) is responsible for requesting the updated Map from the server.
             * /
            //create broadcast messages to all the players - including the current player who initiated the change
            /*Queue<RiskMessage> queue = new Queue<RiskMessage>();
            foreach(MockClient client in clients)
            {
                RiskMessage a = new RiskMessage(MainState.Update, client.name);
                foreach(ArmyPlacement p in changes)
                {
                    a.territory_army.Add((ArmyPlacement)p.Clone());
                }
                queue.Enqueue(a);
            }
            return queue;*/
        }
        /*
         * This method takes a message from a client and decide what is the next phase.
         * Create outgoing messages and put them in the outgoing queue.
         */
        public RiskMessage Next(RiskMessage incoming)
        {
            RiskClient client = clients.Find(x => x.name == incoming.playerName);
            if(client == null) //TK. Make sure the client exists.
            {
                throw new NullReferenceException("Player " + incoming.playerName + " does not exist.");
            }
            RiskMessage outgoing = null;
            if (gameOver(map))
            {
                outgoing = new RiskMessage(MainState.Over, incoming.playerName);
            }
            else if (incoming.playerName != current.name)
            {
                //if this message is from inactive player, and this player is under attack.
                if (defender != null && incoming.playerName == defender.name && current.state == MainState.Roll)
                {
                    defender.state = MainState.Roll;
                    outgoing = new RiskMessage(MainState.Roll, defender.name);
                    outgoing.from = this.attackFrom.getName();
                    outgoing.to = this.attackTo.getName();
                }
                else
                {
                    outgoing = new RiskMessage(MainState.Idle, incoming.playerName);
                }
            }
            else //active player
            {
                int idx = clients.FindIndex(x => x == current);
                if (client.state == MainState.Initialize)
                {
                    if (initialized < map.getAllTerritories().Count) //continue initialization
                    {
                        outgoing = new RiskMessage(MainState.Initialize, current.name);
                        current = clients[(idx + 1) % clients.Count]; //go to the next player
                    }
                    else //move on to distribution
                    {
                        //everyone moves to Distribute
                        foreach(RiskClient c in clients)
                        {
                            c.state = MainState.Distribute;
                        }
                        currentState = MainState.Distribute;
                        outgoing = new RiskMessage(MainState.Distribute, current.name);
                        current = clients[(idx + 1) % clients.Count]; //go to the next player
                    }
                }
                else if (client.state == MainState.Distribute)
                {
                    if (distributed < ((10 - clients.Count) * 5 * clients.Count - initialized)) // continue distribution
                    {
                        outgoing = new RiskMessage(MainState.Distribute, current.name);
                        current = clients[(idx + 1) % clients.Count]; // go to the next player
                    }
                    else // move on to the next player's TradeCard phase
                    {
                        currentState = MainState.TradeCard;
                        //the current player moves to TradeCard. Others go Idle.
                        foreach (RiskClient c in clients)
                        {
                            if (c == current)
                            {
                                c.state = MainState.TradeCard;
                            }
                            else
                            {
                                c.state = MainState.Idle;
                            }
                        }
                        outgoing = new RiskMessage(MainState.TradeCard, current.name);
                        //RiskClient next = clients[(idx + 1) % clients.Count]; // go to the next player 
                    }
                }
                else if (client.state == MainState.TradeCard)//reinforcement cards have been submitted.
                {
                    //still collect phase
                    int collected = 0;
                    if (bEliminated)
                    {
                        client.state = MainState.Reinforce;
                        outgoing = new RiskMessage(MainState.Reinforce, current.name);
                    }
                    else
                    {
                        client.state = MainState.NewArmies;
                        outgoing = new RiskMessage(MainState.NewArmies, current.name);
                        collected = numArmiesCollected(current.name);
                    }
                    if (bCardsTraded) // TODO: test this condition!!!
                    {
                        //Figure out how many armies should be awarded
                        if (tradedCount < tradedArmyCounts.Length)
                        {
                            collected += tradedArmyCounts[tradedCount];
                            tradedCount++;
                        }
                        else
                        {
                            collected += tradedArmyCounts[tradedArmyCounts.Length - 1] + 5 * (tradedCount - tradedArmyCounts.Length);
                            tradedCount++;
                        }
                        bCardsTraded = false;
                    }
                    for (int i = 0; i < collected; i++)
                    {
                        outgoing.territory_army.Add(new ArmyPlacement("any", current.name, 1));
                    }
                }
                else if (client.state == MainState.NewArmies)
                {
                    client.state = MainState.Reinforce;
                    outgoing = new RiskMessage(MainState.Reinforce, current.name);
                }
                //else if (client.state == MainState.AdditionalArmies)
                //{
                //    RiskMessage outgoing = new RiskMessage(MainState.Reinforce, current.name);
                //    queue.Enqueue(outgoing);
                //}
                else if (client.state == MainState.Reinforce)
                {
                    if (bEliminated)
                    {
                        bEliminated = false;
                    }
                    //still collect phase
                    client.state = MainState.Attack;
                    outgoing = new RiskMessage(MainState.Attack, current.name);
                }
                else if (client.state == MainState.Attack)
                {
                    attackerRoll = null;
                    defenderRoll = null;

                    outgoing = new RiskMessage(MainState.Roll, attacker.name);
                    outgoing.from = this.attackFrom.getName();
                    outgoing.to = this.attackTo.getName();
                    outgoing.attacker = true;
                    client.state = MainState.Roll;
                }
                else if (client.state == MainState.Roll)
                {
                    if (attackerRoll != null && defenderRoll != null) //end of the battle
                    {
                        client.state = MainState.AttackOutcome;
                        defender.state = MainState.Idle;
                        outgoing = new RiskMessage(MainState.AttackOutcome, current.name);
                        //TK. Inform the outcome
                        outgoing.territory_army = new List<ArmyPlacement>();
                        foreach(ArmyPlacement p in battleResult)
                        {
                            outgoing.territory_army.Add(p);
                        }
                    }
                    else
                    {
                        //wait for both rolls to arrive.
                        //                    Message outgoing = new Message(MainState.Idle, current.name);
                        //                    queue.Enqueue(outgoing);
                        outgoing = new RiskMessage(MainState.Idle, current.name);
                    }
                }
                else if (client.state == MainState.AttackOutcome)
                {
                    if (attackTo.numArmies == 0)
                    {
                        client.state = MainState.Conquer;
                        outgoing = new RiskMessage(MainState.Conquer, current.name);
                        outgoing.territory_army.Add(new ArmyPlacement(attackFrom.getName(), attacker.name, 0));
                        outgoing.territory_army.Add(new ArmyPlacement(attackTo.getName(), attacker.name, 0));
                        outgoing.from = this.attackFrom.getName();
                        outgoing.to = this.attackTo.getName();
                    }
                    else
                    {
                        client.state = MainState.Attack;
                        outgoing = new RiskMessage(MainState.Attack, current.name);
                    }
                }
                else if (client.state == MainState.Conquer)
                {
                    if (map.getAllTerritories().FindAll(t => t.getOwner() == defender.name).Count == 0)
                    {
                        // the defender has been eliminated
                        bEliminated = true;
                        client.state = MainState.ReinforcementCard;
                        outgoing = new RiskMessage(MainState.ReinforcementCard, current.name);
                        outgoing.card = defender.player.ReinforcementCards;
                    }
                    else
                    {
                        client.state = MainState.Attack;
                        outgoing = new RiskMessage(MainState.Attack, current.name);
                    }
                }
                //else if (client.state == MainState.Eliminate)
                //{
                //    // TODO: this
                //}
                else if (client.state == MainState.AttackDone)
                {
                    if (bConquered)
                    {
                        client.state = MainState.ReinforcementCard;
                        outgoing = new RiskMessage(MainState.ReinforcementCard, current.name);
                        if (reinforcementCards.Count == 0)
                        {
                            foreach (ReinforcementCard card in usedPiles)
                            {
                                reinforcementCards.Add(card);
                            }
                            usedPiles.Clear();
                        }
                        if (reinforcementCards.Count != 0)
                        {
                            ReinforcementCard c = reinforcementCards[0];
                            outgoing.card.Add(c);
                            reinforcementCards.Remove(c);
                            usedPiles.Add(c);
                        }
                        bConquered = false;
                    }
                    else
                    {
                        client.state = MainState.Fortify;
                        outgoing = new RiskMessage(MainState.Fortify, current.name);
                    }
                }
                else if (client.state == MainState.ReinforcementCard)
                {
                    if (bEliminated) // we are in the middle of eliminating a player
                    {
                        client.state = MainState.TradeCard;
                        outgoing = new RiskMessage(MainState.TradeCard, current.name);
                    }
                    else
                    {
                        client.state = MainState.Fortify;
                        outgoing = new RiskMessage(MainState.Fortify, current.name);
                    }
                }
                else if (client.state == MainState.Fortify)
                {
                    //end of this player's turn. 
                    /*
                     * TK - What if the next two players are already out?
                     * */
                    /*RiskClient next = clients[(idx + 1) % clients.Count()];
                    // check that the next player still occupies territories
                    if (map.getAllTerritories().FindAll(t => t.getOwner() == next.name).Count == 0)
                    {
                        next = clients[(idx + 2) % clients.Count()];
                    }*/
                    current.state = MainState.Idle;
                    outgoing = new RiskMessage(MainState.Idle, current.name); //TK.
                    RiskClient next = null;
                    for (int i = 1; i < clients.Count(); ++i)
                    {
                        RiskClient c = clients[(idx + i) % clients.Count()];
                        if (map.getAllTerritories().FindAll(t => t.getOwner() == c.name).Count > 0)
                        {
                            next = c;
                            break;
                        }
                    }
                    if (next != null)
                    {
                        //Move to the next player
                        current = next;
                        current.state = MainState.TradeCard;
                    }
                    else
                    {
                        //game over.
                    }
                }
                else if (client.state == MainState.Start)
                {
                    //the game is about to start. Let the first one to initialize
                    //current = clients[0]; //TK. This should be done when the game starts before the main loop
                    //Everyone goes Initialize
                    foreach(RiskClient c in clients)
                    {
                        c.state = MainState.Initialize;
                    }
                    outgoing = new RiskMessage(MainState.Initialize, current.name);
                    current = clients[(idx + 1) % clients.Count]; //go to the next player
                }
                else //if (client.state == MainState.Idle) //TK. Put all other cases in here. Return Unknown. We should take care of this at the Client side as this should not happen.
                {
                    //nothing to do
                    outgoing = new RiskMessage(MainState.Unknown, current.name);
                }
            }
            return outgoing;
        }

        public RiskMessage Request(RiskMessage msg)
        {
            if(msg.state == MainState.Unknown)
            {
                return Next(msg);
            }
            else if(msg.state == MainState.GetMap)
            {
                List<ArmyPlacement> territory_army = new List<ArmyPlacement>();
                foreach (Territory t in map.getAllTerritories())
                {
                    territory_army.Add(new ArmyPlacement(t.getName(), t.getOwner(), t.numArmies));
                }
                RiskMessage outgoing = new RiskMessage(MainState.GetMap, msg.playerName);
                outgoing.territory_army = territory_army;
                return outgoing;
            }
            else 
            {
                Update(msg); //update the map
                return msg;
            }
        }

        public void Start()
        {
            this.currentState = MainState.Start; //begin the game.
            this.current = clients[0];
        }

    }
}
