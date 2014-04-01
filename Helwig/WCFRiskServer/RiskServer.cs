using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MySql.Data.MySqlClient;
using Database_Controller;
using SUOnlineRisk;

namespace WCFRiskServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class RiskServer : IRiskServer
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public Boolean Login(string username, string password)
        {
            sUtilities u = sUtilities.Instance;
            User user = new User(username, password);
            int i = u.Login(user.getUserName(), user.getPassWord());
            if (i == 0)
            {
                u.addUser(user);
                return true;
            }
            return false;

        }

        public Boolean newUser(string username, string hashpass)
        {
            return sUtilities.Instance.createUser(username, hashpass);
        }

        public Boolean chatMessage(string username, string chatmessage, int gameID)
        {
            sUtilities u = sUtilities.Instance;
            return u.addChat(username, chatmessage, gameID);
        }
        //TODO
        public void sendSystemMessage(int gameID, Message message)
        {
            //clients should be polling server every few seconds
            //set current message in utilities
            Game g = sUtilities.Instance.findGame(gameID);
            if (g != null)
            {
                g.currentMessage = message;
            }
        }

        public void logoff(string username)
        {
            //remove from game
            Game g = sUtilities.Instance.findPlayer(username);
            g.remPlayer(username);
        }

        public List<int> findGames()
        {
            List<Game> lg = sUtilities.Instance.listGames();
            List<int> l = new List<int>();
            foreach (Game g in lg)
            {
                l.Add(g.getID());
            }
            return l;
        }

        public Boolean joinGame(string username, int gameID)
        {
            Game g = sUtilities.Instance.findGame(gameID);
            return g.addPlayer(new Player(username, System.Drawing.Color.Red, g.getMap()));
        }

        public int startGame(string username, int gameID)
        {
            Game g = sUtilities.Instance.findPlayer(username);
            return g.startGame();
        }

        public int newGame(string username, string mapname)
        {
            //do some kind of authentication with userlist
            Player p = new Player(username,System.Drawing.Color.Red,Map.loadMap(mapname));
            Game g = new Game(new Random().Next(1000), p, Map.loadMap(mapname));

            sUtilities.Instance.addGame(g);
            return g.getID();
        }

        public Map getMap(int gameID, string mapname)
        {
            return sUtilities.Instance.findGame(gameID).getMap();
        }
    }
}
