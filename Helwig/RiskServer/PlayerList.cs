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
        //singleton class
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

        //send message to all connected players
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

        //adds player to playerlist
        public void addPlayer(Player player)
        {
            playerList.Add(player);
            Console.WriteLine("PlayerList recieved " + player.getName());
        }

        //returns Random object
        public Random getRandom()
        {
            return random;
        }

        //rolls dice
        public int diceRoll()
        {
            //returns random int between 1 and 6
            return (this.getRandom().Next(1, 7));
        }
        
        //find connected player, given player ID
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

        //find and update player socket
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

        //I don't know why this is here, I'm pretty sure it's never used
        public List<Player> getList()
        {
            return playerList;
        }
    }
}
