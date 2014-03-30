using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SUOnlineRisk;

namespace WCFRiskServer
{
    public class Game
    {
        //states
        public Game(int gameID, Player hostPlayer, Map map)
        {
            this.gameID = gameID;
            playerList = new List<Player>();
            this.map = map;
        }

        int gameID;
        Map map;
        List<Player> playerList;
        public Message currentMessage;

        public int getID()
        {
            return gameID;
        }

        public Map getMap()
        {
            return this.map;
        }

        public Boolean addPlayer(Player player)
        {
            if(playerList.Count() < 6)
            {
                Color c = Color.Red;
                Player p = new Player(player.getName(),c,map);
                playerList.Add(p);
                return true;
            }
            return false;
        }

        public Boolean remPlayer(string userName)
        {
            foreach (Player p in playerList)
            {
                if (String.Compare(p.getName(), userName, false) == 0)
                {
                    return playerList.Remove(p);
                }
            }
            return false;
        }

        public List<Player> getPlayerList()
        {
            return playerList;
        }

        public int startGame()
        {
            //phil's game logic code
            return 0;
        }

    }
}
