using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;
using System.ServiceModel;

namespace RiskMessageService
{
    public class RiskMessageServer : IRiskMessageService
    {
        class PlayerAvatar
        {
            public PlayerAvatar(string player)
            {
                this.player = player;
                changes = new List<ArmyPlacement>();
                active = false;
            }
            public List<ArmyPlacement> changes;
            public string player;
            public bool active;
        }

        List<PlayerAvatar> clients = new List<PlayerAvatar>();
        PlayerAvatar current; //currently active client/player
        MainState state; //current state


        //right now the same map is shared among the server and players. For implementation across remote computers, 
        //this needs to be changed.
        public Map map
        {
            get;
            set;
        }

        public void addClient(string player)
        {
            PlayerAvatar avatar = new PlayerAvatar(player);
            clients.Add(avatar);
            if (current == null) current = avatar;
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
         * this method takes a RiskRiskMessage from a client/player and update the map and other variables accordingly.
         */
        public Queue<RiskMessage> Update(RiskMessage message)
        {
            Queue<RiskMessage> queue = new Queue<RiskMessage>();
            MainState state = message.state;
            if(state == MainState.Idle || state == MainState.Update || state == MainState.Unknown)
            {

            }
            else
            {

            }
            return queue;
        }
        /*
         * This method takes a RiskMessage from a client and decide what is the next phase.
         * Create outgoing RiskMessages and put them in the outgoing queue.
         */
        public Queue<RiskMessage> Next(MainState currentState)
        {
            Queue<RiskMessage> queue = new Queue<RiskMessage>();
            return queue;
        }
        public RiskMessage Request(RiskMessage msg)
        {
            if(msg.playerName != current.player)
            {
                RiskMessage outgoing = new RiskMessage(MainState.Update, msg.playerName);
                if (clients.Exists(x => x.player == msg.playerName))
                {
                    PlayerAvatar avatar = clients.Find(x => x.player == msg.playerName);
                    outgoing.territory_army = avatar.changes;
                    avatar.changes.Clear();
                    return outgoing;
                }
                else
                {
                    PlayerAvatar avatar = new PlayerAvatar(msg.playerName);
                    clients.Add(avatar);
                    return outgoing;
                }
            }
            else
            {
                MainState state = msg.state;

                Update(msg);
                
                if (state == MainState.Idle)
                {
                    return new RiskMessage(MainState.AdditionalArmies, msg.playerName);
                }
                else if (state == MainState.NewArmies)
                {
                    return new RiskMessage(MainState.Reinforce, msg.playerName);
                }
                else if (state == MainState.Reinforce)
                {
                    return new RiskMessage(MainState.Attack, msg.playerName);
                }
                else if (state == MainState.Attack)
                {
                    return new RiskMessage(MainState.AttackDone, msg.playerName);
                }
                else if (state == MainState.AttackDone)
                {
                    return new RiskMessage(MainState.Fortify, msg.playerName);
                }
                else if (state == MainState.Fortify)
                {
                    current = clients[(clients.FindIndex(x => x == current) + 1) % clients.Count];
                    return new RiskMessage(MainState.Idle, msg.playerName);
                }
                else
                {
                    return new RiskMessage(MainState.Unknown, msg.playerName);
                }
            }
        }
        public void Run()
        {
            //start the service
            using (ServiceHost host = new ServiceHost(typeof(RiskMessageServer), new Uri("http://localhost:8000/RiskMessageService")))
            {
                host.AddServiceEndpoint(typeof(IRiskMessageService), new BasicHttpBinding(), "RiskMessageService");
                host.Open();
                Console.WriteLine("Press <ENTER> to terminate server.");
                Console.ReadLine();
            }
            /*using (ServiceHost host = new ServiceHost(typeof(ServiceDemo.MessageService), new Uri("http://localhost:8000/MessageService")))
            {
                host.AddServiceEndpoint(typeof(ServiceDemo.IMessageService), new BasicHttpBinding(), "ServiceDemo");
                host.Open();
                Console.WriteLine("Press <ENTER> to terminate the service host");
                Console.ReadLine();
            }*/
        }
    }
}
