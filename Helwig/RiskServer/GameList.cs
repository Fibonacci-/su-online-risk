using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskServer
{
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
                if(checkID.Equals(gameID))
                {
                    return game;
                }
            }
            return null;

        }

        public void newGame(Player host)
        {
            Game game = new Game(ClientHandler.generateID(6));
            gameList.Add(game);
            game.addPlayer(host);
        }
    }
}
