using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiskServer
{
    class Game
    {
        public Game(String gameID) 
        {
            playerList = new List<Player>();
            this.gameID = gameID;
            
        }
        List<Player> playerList;
        Player hostPlayer;
        String gameID;
        

        public void addPlayer(Player player)
        {
            if (playerList.Count == 0)
            {
                playerList.Add(player);
                hostPlayer = player;
            }
            else if (playerList.Count >= 6)
            {
                player.sendMessage("Game is full");
            }
            else
            {
                playerList.Add(player);

            }
        }

        public void remPlayer(Player player)
        {
            if (!playerList.Remove(player))
            {
                Console.WriteLine("Could not remove player.");
            }
            else
            {
                Console.WriteLine("Removed player " + player.getID() + " from game " + this.getID() + ".");
            }
        }

        public String getID()
        {
            return gameID;
        }

        void broadcast(String message)
        {
            foreach (Player player in playerList)
            {
                player.sendMessage(message);
            }
        }
    }
}
