﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;

namespace ClientDemo
{
    class MockServer
    {
        List<MockClient> clients = new List<MockClient>();
        List<ReinforcementCard> reinforcementCards; //a whole set of reinforcement cards.
        List<ReinforcementCard> usedPiles = new List<ReinforcementCard>();

        MockClient current; //currently active client/player
        MainState state; //current state

        //these are used during the attack phase
        MockClient attacker;
        MockClient defender;
        Territory attackFrom;
        Territory attackTo;
        int[] attackerRoll;
        int[] defenderRoll;
        //set true when a territory is conquered.
        bool bConquered = false;

        //used during the initialization phase
        int initialized = 0;

        //the number of armies to be awarded in sequence
        static int[] tradedArmyCounts = {4, 6, 8, 10, 12, 15};
        //this track how many times reinforcement cards are traded. This will be used to get the number of armies to be awarded.
        int tradedCount = 0;
        List<ReinforcementCard> tradedCards = null;

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
        List<ReinforcementCard> generateReinforcementCards(Map map)
        {
            List<ReinforcementCard> cards = new List<ReinforcementCard>();
            /* THIS NEEDS TO BE COMPLETEd.
             */
            return cards;
        }

        /*
         * Check if the entire map is wipied out by a single player.
         */
        bool gameOver(Map map)
        {
            /* THIS NEEDS TO BE COMPLETEd.
             */
            return false;
        }

        public void addClient(MockClient client)
        {
            clients.Add(client);
            if (current == null) current = client;
        }
        void updateMap(List<ArmyPlacement> placement)
        {
            foreach(ArmyPlacement a in placement)
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
        int numArmiesCollected(Map map, string playerName)
        {
            //NOT YET IMPLEMENTED.
            return 0;
        }
        /*
         * this method takes a message from a client/player and update the map and other variables accordingly.
         */
        public Queue<Message> Update(Message message)
        {
            List<ArmyPlacement> changes = new List<ArmyPlacement>();
            int idx = clients.FindIndex(x => x == current);
            state = message.state;
            if (message.state == MainState.Initialize)
            {
                if (message is ArmyPlacementMessage)
                {
                    ArmyPlacementMessage message2 = (ArmyPlacementMessage)message;
                    if (message2.territory_army.Count > 0)
                    {
                        initialized++;
                        changes.Add(message2.territory_army[0]);
                    }
                }
            }
            else if (message.state == MainState.Distribute || message.state == MainState.Reinforce || message.state == MainState.Fortify)
            {
                if (message is ArmyPlacementMessage)
                {
                    foreach (ArmyPlacement a in ((ArmyPlacementMessage)message).territory_army)
                    {
                        changes.Add(a);
                    }
                }
            }
            if (message.state == MainState.Conquer)
            {
                if (message is ArmyPlacementMessage)
                {
                    ArmyPlacementMessage message2 = (ArmyPlacementMessage)message;
                    foreach (ArmyPlacement p in message2.territory_army)
                    {
                        changes.Add(p);
                    }
                    bConquered = true;
                }
            }
            else if (message.state == MainState.NewArmies)
            {
                //no change to the map
            }
            else if (message.state == MainState.TradeCard)
            {
                if (message is TradeCardMessage)
                {
                    TradeCardMessage message2 = (TradeCardMessage)message;
                    foreach (int id in message2.cardIds)
                    {
                        ReinforcementCard card = usedPiles.Find(x => x.CardId == id);
                        ArmyPlacement a = new ArmyPlacement();
                        a.numArmies = ReinforcementCard.numArmies(card.UnitType);
                        a.territory = card.TerritoryName;
                        changes.Add(a);
                        tradedCards.Add(card);
                    }
                    tradedCount++;
                }
            }
            else if (message.state == MainState.AdditionalArmies)
            {
                //no change to the map
            }
            else if (message.state == MainState.Attack)
            {
                if (message is AttackMessage)
                {
                    AttackMessage message2 = (AttackMessage)message;
                    Territory from = map.getTerritory(message2.from);
                    Territory to = map.getTerritory(message2.to);
                    if (from.getOwner() == message2.playerName && to.getOwner() != message2.playerName)
                    {
                        if (from.isNeighbor(to) && to.isNeighbor(from))
                        {
                            attackFrom = from;
                            attackTo = to;
                            attacker = clients.Find(x => x.name == from.getOwner());
                            defender = clients.Find(x => x.name == to.getOwner());
                        }
                    }
                }
            }
            else if (message.state == MainState.AttackDone)
            {
                if (message is AttackDoneMessage)
                {
                    AttackDoneMessage message2 = (AttackDoneMessage)message;
                    //erase them so that we do not mistakenly start another battle
                    attackFrom = null;
                    attackTo = null;
                    attacker = null;
                    defender = null;
                    attackerRoll = null;
                    defenderRoll = null;
                }
            }
            else if (message.state == MainState.Roll)
            {
                if (message is RollMessage)
                {
                    RollMessage message2 = (RollMessage)message;
                    if (message2.playerName == attacker.name)
                    {
                        attackerRoll = (int[])message2.roll.Clone();
                        Array.Sort(attackerRoll);
                    }
                    else
                    {
                        defenderRoll = (int[])message2.roll.Clone();
                        Array.Sort(defenderRoll);
                    }
                    if (attackerRoll != null && defenderRoll != null)
                    {
                        //Decide how many armies are lost from each side 
                        int attackerLoss = 0; //set this one
                        int defenderLoss = 0; //set this one
                        ArmyPlacement a = new ArmyPlacement(attackFrom.getName(), attacker.name, attackerLoss);
                        ArmyPlacement b = new ArmyPlacement(attackTo.getName(), defender.name, defenderLoss);
                        changes.Add(a); 
                        changes.Add(b);
                    }
                }
            }

            //go ahead and update the map
            updateMap(changes);

            //create broadcast messages to all the players - including the current player who initiated the change
            Queue<Message> queue = new Queue<Message>();
            foreach(MockClient client in clients)
            {
                ArmyPlacementMessage a = new ArmyPlacementMessage(MainState.Update, client.name);
                foreach(ArmyPlacement p in changes)
                {
                    a.territory_army.Add((ArmyPlacement)p.Clone());
                }
                queue.Enqueue(a);
            }
            return queue;
        }
        /*
         * This method takes a message from a client and decide what is the next phase.
         * Create outgoing messages and put them in the outgoing queue.
         */
        public Queue<Message> Next(MainState currentState)
        {
            Queue<Message> queue = new Queue<Message>();
            int idx = clients.FindIndex(x => x == current);
            if(gameOver(map))
            {
                foreach(MockClient client in clients)
                {
                    Message outgoing = new Message(MainState.Over, client.name);
                    queue.Enqueue(outgoing);
                }
            }
            else if (currentState == MainState.Initialize)
            {
                if (initialized < map.getAllTerritories().Count) //continue initialization
                {
                    current = clients[(idx + 1) % clients.Count]; //go to the next player
                    Message outgoing = new Message(MainState.Initialize, current.name); 
                    queue.Enqueue(outgoing);
                }
                else //move on to distribution
                {
                    current = clients[0];
                    Message outgoing = new Message(MainState.Distribute, current.name);
                    queue.Enqueue(outgoing);
                }
            }
            else if (currentState == MainState.Distribute)
            {
                if (idx == clients.Count() - 1) //go to the next phase
                {
                    current = clients[0];
                    ArmyPlacementMessage outgoing = new ArmyPlacementMessage(MainState.NewArmies, current.name);
                    int collected = numArmiesCollected(map, current.name);
                    outgoing.territory_army.Add(new ArmyPlacement("any", current.name, collected));
                    queue.Enqueue(outgoing);
                }
                else //move to the next player and continue Distribution
                {
                    current = clients[idx + 1];
                    Message outgoing = new Message(MainState.Distribute, current.name);
                    queue.Enqueue(outgoing);
                }
            }
            else if (currentState == MainState.NewArmies)
            {
                ArmyPlacementMessage outgoing = new ArmyPlacementMessage(MainState.TradeCard, current.name);
                queue.Enqueue(outgoing);
            }
            else if (currentState == MainState.TradeCard)//reinforcement cards have been submitted.
            {
                //still collect phase
                ArmyPlacementMessage outgoing = new ArmyPlacementMessage(MainState.AdditionalArmies, current.name);
                int collected = 0; 
                //Figure out how many armies should be awarded
                if(tradedCount <= tradedArmyCounts.Length)
                {
                    collected = tradedArmyCounts[tradedCount - 1];
                }
                else
                {
                    collected = tradedArmyCounts[tradedArmyCounts.Length - 1] + 5 * (tradedCount - tradedArmyCounts.Length);
                }
                outgoing.territory_army.Add(new ArmyPlacement("any", current.name, collected));
                queue.Enqueue(outgoing);
            }
            else if (currentState == MainState.AdditionalArmies)
            {
                Message outgoing = new Message(MainState.Reinforce, current.name);
                queue.Enqueue(outgoing);
            }
            else if (currentState == MainState.Reinforce)
            {
                //still collect phase
                Message outgoing = new Message(MainState.Attack, current.name);
                queue.Enqueue(outgoing);
            }
            else if (currentState == MainState.Attack)
            {
                attackerRoll = null;
                defenderRoll = null;
                Message outgoing = new Message(MainState.Roll, attacker.name);
                queue.Enqueue(outgoing);
                Message outgoing2 = new Message(MainState.Roll, defender.name);
                queue.Enqueue(outgoing2);
            }
            else if (currentState == MainState.Roll)
            {
                if (attackerRoll != null && defenderRoll != null) //end of the battle
                {
                    if (attackTo.numArmies == 0)
                    {
                        ArmyPlacementMessage outgoing = new ArmyPlacementMessage(MainState.Conquer, current.name);
                        outgoing.territory_army.Add(new ArmyPlacement(attackFrom.getName(), attacker.name, 0));
                        outgoing.territory_army.Add(new ArmyPlacement(attackTo.getName(), attacker.name, 0));
                        queue.Enqueue(outgoing);
                    }
                    else
                    {
                        Message outgoing = new Message(MainState.Attack, current.name);
                        queue.Enqueue(outgoing);
                    }
                }
                else
                {
                    //wait for both rolls to arrive.
                }
            }
            else if (currentState == MainState.Conquer)
            {
                Message outgoing = new Message(MainState.Attack, current.name);
                queue.Enqueue(outgoing);
            }
            else if (currentState == MainState.AttackDone)
            {
                if(bConquered)
                {
                    ReinforcementCardMessage outgoing = new ReinforcementCardMessage(MainState.ReinforcementCard, current.name);
                    outgoing.card = reinforcementCards[0];
                    reinforcementCards.Remove(outgoing.card);
                    queue.Enqueue(outgoing);
                    bConquered = false;
                }
                else
                {
                    Message outgoing = new Message(MainState.Fortify, current.name);
                    queue.Enqueue(outgoing);
                }
            }
            else if (currentState == MainState.ReinforcementCard)
            {
                Message outgoing = new Message(MainState.Fortify, current.name);
                queue.Enqueue(outgoing);
            }
            else if (currentState == MainState.Fortify)
            {
                //end of this player's turn. Move to the next player
                current = clients[(idx + 1) % clients.Count()];
                Message outgoing = new Message(MainState.NewArmies, current.name);
                queue.Enqueue(outgoing);
            }
            else if (currentState == MainState.Start)
            {
                //anything to do?
            }
            return queue;
        }
        public void Run()
        {
            Queue<Message> incomingQueue = new Queue<Message>();
            Message message = new Message(MainState.Initialize, clients[0].name);
            incomingQueue.Enqueue(message);
            while (incomingQueue.Count > 0)
            {
                Message incoming = incomingQueue.Dequeue();
                MainState state = incoming.state;

                Queue<Message> queue = Update(incoming);
                foreach (Message m in queue)
                {
                    MockClient c = clients.Find(x => x.name == m.playerName);
                    c.Request(m);
                }

                Queue<Message> queue2 = Next(state); //figure out the next phase
                foreach (Message m in queue2)
                {
                    MockClient c = clients.Find(x => x.name == m.playerName);
                    Message msg = c.Request(m);
                    incomingQueue.Enqueue(msg);
                }
            }
        }
    }
    class MockPlayer: Player
    {
        public Map map
        {
            get;
            set;
        }
        public MockPlayer(string n, System.Drawing.Color color): base(n, color)
        {
        }
    }
    class MockClient
    {
        Message incoming;
        Message outgoing;
        public MockPlayer player
        {
            get;
            set;
        }
        public MockServer server
        {
            get;
            set;
        }
        public string name
        {
            get { return player.nickname; }
        }
        public MainState state
        {
            get { return player.state; }
        }
        public Message Request(Message incoming)
        {
            Message outgoing = null;
            MainState state = incoming.state;
            player.state = state;
            if (state == MainState.Update) //this is for updating the map
            {
                outgoing = player.Update(incoming);
            }
            else if (state == MainState.Initialize)
            {
                outgoing = player.Initialize(incoming);
            }
            else if (state == MainState.Distribute)
            {
                outgoing = player.Distribute(incoming);
            }
            else if (state == MainState.NewArmies)
            {
                outgoing = player.NewArmies(incoming);
            }
            else if (state == MainState.TradeCard)
            {
                outgoing = player.TradeCard(incoming);
            }
            else if (state == MainState.AdditionalArmies)
            {
                outgoing = player.AdditionalArmies(incoming);
            }
            else if (state == MainState.Reinforce)
            {
                outgoing = player.Reinforce(incoming);
            }
            else if (state == MainState.Attack)
            {
                outgoing = player.Attack(incoming);
            }
            else if (state == MainState.Roll)
            {
                outgoing = player.Roll(incoming);
            }
            else if (state == MainState.ReinforcementCard)
            {
                outgoing = player.ReinforcementCard(incoming);
            }
            else if (state == MainState.Fortify)
            {
                outgoing = player.Fortify(incoming);
            }
            else if (state == MainState.Start)
            {
                outgoing = incoming;
            }
            return outgoing;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map(@"..\..\SimpleRiskMap.png"); //We will be using the same map - this needs to be changed
            MockPlayer[] players = new MockPlayer[3];
            MockClient[] clients = new MockClient[3];
            MockServer server = new MockServer();
            server.map = map;
            string[] names = { "Tom", "Amy", "Jack" };
            System.Drawing.Color[] colors = {System.Drawing.Color.Red, System.Drawing.Color.Green, System.Drawing.Color.Blue };
            for (int i = 0; i < 3; ++i)
            {
                players[i] = new MockPlayer(names[i], colors[i]);
                players[i].map = map;
                clients[i] = new MockClient();
                clients[i].player = players[i];
                clients[i].server = server;
                server.addClient(clients[i]);
            }
            server.Run();
        }
    }
}
