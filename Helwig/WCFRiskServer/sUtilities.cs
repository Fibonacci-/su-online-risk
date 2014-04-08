using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database_Controller;
using MySql.Data.MySqlClient;
using SUOnlineRisk;

namespace WCFRiskServer
{
    class sUtilities
    {
        //init singleton
        private static sUtilities instance;
        private Controller controller;
        private MySqlConnection connection;
        private List<Game> gameList;
        private List<User> userList;

        private sUtilities() 
        {
            controller = new Controller();
            connection = controller.Connect("lunacyarts.com", "3306", "ablivion", "rab313222");
            gameList = new List<Game>();
            userList = new List<User>();
        }
        public static sUtilities Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new sUtilities();
                }
                return instance;
            }
        }

        //game-specific things

        public void addGame(Game game)
        {
            gameList.Add(game);
        }

        public List<Game> listGames()
        {
            return gameList;
        }

        public Boolean removeGame(Game game)
        {
            return gameList.Remove(game);
        }

        public Game findGame(int gameID)
        {
            foreach (Game g in gameList)
            {
                if (g.getID() == gameID)
                {
                    return g;
                }
            }
            return null;
        }

        //player things
        public void addUser(User user)
        {
            userList.Add(user);
        }

        public Boolean remUser(string username)
        {
            return userList.Remove(findUser(username));
        }

        public User findUser(String username)
        {
            foreach (User u in userList)
            {
                if (String.Compare(u.getUserName(), username, false) == 0)
                {
                    return u;
                }
            }
            return null;
        }

        public Game findPlayer(string username)
        {
            foreach (Game g in gameList)
            {
                List<Player> playerList = g.getPlayerList();
                foreach (Player p in playerList)
                {
                    if (String.Compare(p.getName(), username, false) == 0)
                    {
                        return g;
                    }
                }
            }
            return null;
        }

        //DB stuff

        public int Login(string username, string password)
        {
            int c = controller.Login(connection, username, password);
            return c;
        }

        public void closeConnection()
        {
            connection.Close();
        }

        public Boolean addChat(string username, string message, int gameID)
        {
            return controller.insert_chat(connection, username, message, gameID);
        }
        //Create_Usr(conection, username, hashpass)

        public Boolean createUser(string username, string hashpass)
        {
            return controller.Create_Usr(connection, username, hashpass);
        }
        //not doing these one yet
        //chatroom_recall(connection, gameID)
        //playerchat_recall(connection, string username)

        //create_game(connection, string[] users, string game_name);
        public int createGame(string[] userList, string gameName)
        {
            return controller.create_game(connection, userList, gameName);
        }
        //premature_close(connection, players[], gameid);

        public void unfinishedClose(Game game)
        {

        }
    }
}
