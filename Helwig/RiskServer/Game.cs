using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiskServer
{
    class Game
    {
        //constructor
        public Game(String gameID) 
        {
            //list to keep track of players in game
            playerList = new List<Player>();
            this.gameID = gameID;
            
        }
        List<Player> playerList;
        Player hostPlayer;
        String gameID;
        
        //add player to game, providing maximum players has not been reached.
        //if there's nobody in the game, set player as host
        public void addPlayer(Player player)
        {
            if (playerList.Count == 0)
            {
                playerList.Add(player);
                hostPlayer = player;
                player.setHost();
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

        //remove player from game.
        //if player to be removed is the host, set a new host
        public void remPlayer(Player player)
        {
            if (!playerList.Remove(player))
            {
                Console.WriteLine("Could not remove player.");
            }
            else
            {
                Console.WriteLine("Removed player " + player.getID() + " from game " + this.getID() + ".");
                if (hostPlayer == player && playerList.Count() != 0)
                {
                    hostPlayer = playerList[0];
                    playerList[0].setHost();
                }
            }
            
        }

        public String getID()
        {
            return gameID;
        }

        //handles sending a string to each player in the game
        void broadcast(String message)
        {
            foreach (Player player in playerList)
            {
                player.sendMessage(message);
            }
        }

        //this is where the magic happens
        internal void beginGame(Player host)
        {
            //can only be called by host player
            if (host.Equals(hostPlayer))
            {
                //shuffle player list to emulate a dice roll
                Random rng = new Random();
                playerList.OrderBy(a => rng.Next());
                //broadcast player order
                String message = "ord";
                foreach (Player player in playerList)
                {
                    message += "," + player.getID();
                }
                this.broadcast(message);
            }

        }

        
    }
}
