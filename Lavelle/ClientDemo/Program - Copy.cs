using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StateMachines;
using RiskMap;

namespace ClientDemo
{
    class Server
    {
        Message incoming;
        Message outgoing;
        List<Client> clients;
        Client current; //currently active client/player
        List<MainState> states; //state of each client

        //these are used during the attack phase
        Client attacker; 
        Client defender; 
        int[] attackerRoll;
        int[] defenderRoll;

        //right now the same map is shared among the server and players. For implementation across remote computers, 
        //this needs to be changed.
        public Map map
        {
            get;
            set;
        }

        //constructor
        public Server()
        {
            clients = new List<Client>();
            current = null;
        }
        public void addClient(Client client)
        {
            clients.Add(client);
            states.Add(MainState.Start);
            if (current == null) current = client;
        }
        public void Acknowledge(Message message)
        {
            incoming = message;
            int idx = clients.FindIndex(x => x == current);
            states[idx] = incoming.state;
            if (incoming.state == MainState.Initialize)
            {
                Territory t = map.getTerritory(incoming.to);
                if (t.getOwner() == "unoccupied")
                {
                    t.setOwner(incoming.playerName);
                    t.numArmies++;
                }
            }
            else if (incoming.state == MainState.Distribute)
            {
                Territory t = map.getTerritory(incoming.to);
                if (t.getOwner() == incoming.playerName)
                {
                    t.numArmies += incoming.armyCounts;
                }
            }
            else if (incoming.state == MainState.Collect)
            {
            }
            else if (incoming.state == MainState.Attack)
            {
                Territory from = map.getTerritory(incoming.from);
                Territory to = map.getTerritory(incoming.to);
                if (from.getOwner() == incoming.playerName && to.getOwner() != incoming.playerName)
                {
                    if (from.isNeighbor(to) && to.isNeighbor(from))
                    {
                        attacker = clients.Find(x => x.name == from.getOwner());
                        defender = clients.Find(x => x.name == to.getOwner());
                        attackerRoll = null;
                        defenderRoll = null;
                    }
                }
            }
            else if(incoming.state == MainState.Roll)
            {
                if(incoming.playerName == attacker.name)
                {
                    attackerRoll = (int[]) incoming.roll.Clone();
                    Array.Sort(attackerRoll);
                }
                else
                {
                    defenderRoll = (int[])incoming.roll.Clone();
                    Array.Sort(defenderRoll);
                }
            }
            else if (incoming.state == MainState.Fortify)
            {
                Territory from = map.getTerritory(incoming.from);
                Territory to = map.getTerritory(incoming.to);
                if (from.getOwner() == incoming.playerName && to.getOwner() != incoming.playerName)
                {
                    if (from.isNeighbor(to) && to.isNeighbor(from))
                    {
                        if (from.numArmies >= incoming.armyCounts)
                        {
                            to.numArmies += incoming.armyCounts;
                            from.numArmies -= incoming.armyCounts;
                        }
                    }
                }
            }
        }
        public Message Request()
        {
            if(outgoing == null)
            {
                outgoing = new Message(MainState.Initialize); //very first message
            }
            else
            {
                //find out the current state
                int idx = clients.FindIndex(x => x == current);
                MainState state = states[idx];
                if(state == MainState.Initialize)
                {
                    //go to the next client. But we need to check if there are any more territories left
                }
                else if(state == MainState.Distribute)
                {
                    //go to the next client.
                }
                else if(state == MainState.Collect)
                {
                    //go to the attack phase
                }
                else if(state == MainState.Attack)
                {
                    //if done, go to the fortify phase.
                    //else go to the rolll phase.
                }
                else if(state == MainState.Roll)
                {
                    //find the winner. Update the armies.
                    //go back to the attack phase.
                }
                else if(state == MainState.Fortify)
                {
                    //go to the next client at the collect phase.
                }
                else if(state == MainState.Start)
                {
                    //do nothing?
                }
                else if(state == MainState.Update)
                {
                    //do nothing?
                }
            }
            return outgoing;
        }
    }
    class Player
    {
        public Map map
        {
            get;
            set;
        }
        public string name
        {
            get;
            private set;
        }
        public Player(string n)
        {
            name = n;
        }
        public void Update(Message message)
        {
            //update the armies using
            Territory t = map.getTerritory(message.to);
            Territory f = map.getTerritory(message.from);
            int counts = message.armyCounts;
        }
        public Message Initialize()
        {
            Message message = new Message(MainState.Initialize);
            //Figure out which territory to place an army, and set message.to.
            message.armyCounts = 1; //should be 1
            return message;
        }
        public Message Attack()
        {
            Message message = new Message(MainState.Attack);
            //Figure out from which territory and to which territory the attck will be made.
            //Set message.to and message.from.
            message.armyCounts = 1; //should be 1
            message.bDone = true; //set this to true if done attacking, false otherwise.
            return message;
        }
        public Message Roll()
        {
            Message message = new Message(MainState.Roll);
            //Roll dice and return the result in.
            message.roll[0] = 1;
            message.roll[1] = 2;
            message.roll[2] = 0; //not being used
            return message;
        }
        public Message Distribute() { return new Message(MainState.Distribute); }
        public Message Collect() { return new Message(MainState.Collect); }
        public Message Reinforce() { return new Message(MainState.Reinforce); }
        public Message Fortify() { return new Message(MainState.Fortify); }
    }
    class Client
    {
        public Player player
        {
            get;
            set;
        }
        public Server server
        {
            get;
            set;
        }
        public string name
        {
            get { return player.name; }
        }
        public void Send(Message message)
        {
            //send the message to the server
            server.Acknowledge(message);
        }
        public Message Receive()
        {
            //receive a message from the server.
            Message message = server.Request();
            return message;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Map map = Map.loadMap(@"..\..\SimpleRisk.map"); //We will be using the same map - this needs to be changed
            Player[] players = new Player[3];
            Client[] clients = new Client[3];
            Message[] request = new Message[3];
            Message[] acknowledge = new Message[3];
            Server server = new Server();
            server.map = map;
            string[] names = { "Tom", "Amy", "Jack" };
            for (int i = 0; i < 3; ++i)
            {
                players[i] = new Player(names[i]);
                players[i].map = map;
                clients[i] = new Client();
                request[i] = new Message(MainState.Start);
                acknowledge[i] = new Message(MainState.Start);
                clients[i].player = players[i];
                clients[i].server = server;
                server.addClient(clients[i]);
            }
            while (true)
            {
                for (int i = 0; i < 3; ++i)
                {
                    request[i] = clients[i].Receive();
                    if (request[i].state == MainState.Start)
                    {
                        acknowledge[i].state = MainState.Start; 
                    }
                    else if (request[i].state == MainState.Initialize)
                    {
                        acknowledge[i] = players[i].Initialize();
                    }
                    else if (request[i].state == MainState.Distribute)
                    {
                        acknowledge[i] = players[i].Distribute();
                    }
                    else if (request[i].state == MainState.Collect)
                    {
                        acknowledge[i] = players[i].Collect();
                    }
                    else if (request[i].state == MainState.Attack)
                    {
                        acknowledge[i] = players[i].Attack();
                    }
                    else if (request[i].state == MainState.Roll)
                    {
                        acknowledge[i] = players[i].Roll();
                    }
                    else if (request[i].state == MainState.Fortify)
                    {
                        acknowledge[i] = players[i].Fortify();
                    }
                    else if (request[i].state == MainState.Over)
                    {
                        break;
                    }
                    clients[i].Send(acknowledge[i]);
                }
            }
        }
    }
}
