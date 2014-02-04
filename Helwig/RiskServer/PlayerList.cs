using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RiskServer
{
    class PlayerList
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
        
        public Player findPlayer(String playerID)
        {
            foreach (Player player in playerList)
            {
                if (player.getID().Equals(playerID))
                {
                    return player;
                }
            }
            return null;
        }

        public Boolean updatePlayer(String playerID, Socket newClient)
        {
            try
            {
                Player player = findPlayer(playerID);
                player.setSocket(newClient); 
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
            return false;
        }

        public List<Player> getList()
        {
            return playerList;
        }
    }
}
