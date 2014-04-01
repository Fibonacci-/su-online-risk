using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SUOnlineRisk;

namespace RiskMessageService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class RiskMessageService : IRiskMessageService
    {
        static Dictionary<int, Game> games = new Dictionary<int, Game>();
        static Dictionary<string, Game> player2Game = new Dictionary<string, Game>();
        static List<string> players = new List<string>();
        static Random rgen = new Random();

        public RiskMessage Request(RiskMessage msg)
        {
            Game game = player2Game[msg.playerName];
            game.Update(msg);
            return game.Next(msg);
        }

        public bool Logon(string name, string password)
        {
            if(players.Exists(x => x==name))
            {
                return false;
            }
            else
            {
                //NEED TO ADD PASSWORD VERIFICATION
                players.Add(name);
                return true;
            }
        }

        public int createGame(int mapId)
        {
            /*if (games.ContainsKey(mapId))
            {
                return 0;
            }
            else*/
            {
                Game game = new Game();
                //for now, assign a random number. This number should come from DB.
                int gameid = rgen.Next(0, 100);
                games[gameid] = game;
                return gameid;
            }
        }

        public bool joinGame(string name, int gameId)
        {
            if (player2Game.ContainsKey(name))
            {
                //the player is already playing another game.
                return false;
            }
            else if(games.ContainsKey(gameId) == false)
            {
                //no such game exists.
                return false;
            }
            else 
            {
                Game game = games[gameId];
                if (game.gameOn)
                {
                    //the game has already started.
                    return false;
                }
                else
                {
                    player2Game[name] = games[gameId];
                    game.addPlayer(name);
                    return true;
                }
            }
        }

        public bool startGame(string name, int gameId)
        {
            Game game = player2Game[name];
            return game.Start();
        }
    }
}
