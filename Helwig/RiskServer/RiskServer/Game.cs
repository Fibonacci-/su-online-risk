using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiskServer
{
    class Game
    {
        public Game() 
        {
            playerList = new List<Player>();
            
        }
        List<Player> playerList;
        

        public void addPlayer(Player player)
        {
            if (playerList.Count >= 6)
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
        }
    }
}
