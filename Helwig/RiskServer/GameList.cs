using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;

namespace RiskServer
{
    //singleton class
    class GameList
    {
        private static GameList instance;
        private static List<Game> gameList;

        private GameList()
        {
            gameList = new List<Game>();
        }

        public static GameList Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameList();
                }
                return instance;
            }
        }

        //returns game given id, returns null if no game with id exists in list
        public Game findGame(String gameID)
        {
            foreach(Game game in gameList)
            {
                String checkID = game.getID();
                if(string.Compare(checkID,gameID, false) == 0)
                {
                    return game;
                }
            }
            return null;

        }

        //makes a new game
        public Game newGame(Player player, String mapFilePath)
        {
            Map map = SUOnlineRisk.Map.loadMap(mapFilePath);
            Game game = new Game(ClientHandler.generateID(6),map,player);
            gameList.Add(game);
            Console.WriteLine("GL made game " + game.getID() + " and added to game list.");
            return game;
        }
    }
}
