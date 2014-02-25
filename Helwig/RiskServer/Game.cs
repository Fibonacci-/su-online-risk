using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUOnlineRisk;

namespace RiskServer
{
    class Game
    {
        //constructor
        public Game(String gameID, Map map, Player host)
        {
            //list to keep track of players in game
            playerList = new List<Player>();
            this.gameID = gameID;
            this.map = map;
            addPlayer(host);
            Console.WriteLine("Made new game " + this.gameID + " with host player " + this.hostPlayer.getName());
        }
        List<Player> playerList;
        Player hostPlayer;
        String gameID;
        Map map;

        //add player to game, providing maximum players has not been reached.
        //if there's nobody in the game, set player as host
        public void addPlayer(Player player)
        {
            if (playerList.Count == 0)
            {
                Console.WriteLine("Added player " + player.getName() + " as host for game " + this.getID());
                playerList.Add(player);
                hostPlayer = player;
                player.setHost();
            }
            else if (playerList.Count >= 6)
            {
                //TODO change this eventually
                player.sendMessage("Game is full");
                Console.WriteLine("Can't add player " + player.getName() + " to game " + this.getID() + ". Too many players");
            }
            else
            {
                if (!checkPlayerInGame(player))
                {
                    playerList.Add(player);
                }
                else
                {
                    Console.WriteLine("Error: Player " + player.getName() + " is already in game " + this.gameID);
                }

            }
        }

        Boolean checkPlayerInGame(Player player)
        {
            foreach (Player p in playerList)
            {
                if (p == player) return true;
            }
            return false;
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

        public void getMessage(String message)
        {

        }

        //this is where the magic happens
        internal void beginGame(Player host)
        {
            //can only be called by host player
            if (host.Equals(hostPlayer) && playerList.Count() > 1)
            {
                Console.WriteLine("Starting game " + this.getID());
                //shuffle player list to emulate a dice roll
                Random rng = new Random();
                playerList.OrderBy(a => rng.Next());
                //broadcast player order
                String message = "ord";
                foreach (Player player in playerList)
                {
                    message += "," + player.getID();
                    player.startGame();
                }
                this.broadcast(message);
                //add a certain number of armies to each player
                if (playerList.Count() == 2)
                {
                    foreach (Player player in playerList)
                    {
                        player.addUnusedInfantry(40);
                        player.sendMessage("ua,40");
                    }
                    // TODO add a neutral army with 40 pieces
                }
                else
                {
                    //add so many army pieces depending on number of people playing
                    int calculatedToAdd = 50 - (playerList.Count() * 5);
                    foreach (Player player in playerList)
                    {
                        player.addUnusedInfantry(calculatedToAdd);
                        player.sendMessage("ua," + calculatedToAdd);

                    }
                }

                //Let clients know phase is changing
                broadcast("tpp");
                claimTerritories();
            }
            else
            {
                host.sendMessage("err");
            }

        }

        private void claimTerritories()
        {
            //TODO while true, check if all territories are claimed
            //Can't do until I have a territory framework
            foreach (Player player in playerList)
            {
                player.sendMessage("tpr,1");
                String message = player.getMessage();
            }
        }


    }
    class DBConnect
    {
        //Roger's connection goes here
        
    }
    
}
