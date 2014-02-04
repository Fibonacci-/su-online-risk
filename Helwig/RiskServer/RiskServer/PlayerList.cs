using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RiskServer
{
    class PlayerList : List<Socket>
    {
        private static PlayerList instance;
        private static List<Player> playerList;
        private Random random;

        private PlayerList() 
        {
            playerList = new List<Player>();
            random = new Random();
        }

        public static PlayerList Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerList();
                }
                return instance;
            }
        }

        public void broadcast(String message)
        {
            int count = 0;
            foreach (Player player in playerList)
            {
                player.sendMessage(message);
                count++;
            }
            Console.WriteLine("Broadcast send to " + count + " connected players.");
        }

        public void addPlayer(Player player)
        {
            playerList.Add(player);
            Console.WriteLine("PlayerList recieved " + player.getName());
        }

        public Random getRandom()
        {
            return random;
        }
        
    }
}
