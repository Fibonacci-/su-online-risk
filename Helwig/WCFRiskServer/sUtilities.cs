using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database_Controller;
using MySql.Data.MySqlClient;

namespace WCFRiskServer
{
    class sUtilities
    {
        //init singleton
        private static sUtilities instance;
        private Controller controller;
        private MySqlConnection connection;
        private sUtilities() 
        {
            controller = new Controller();
            connection = controller.Connect("lunacyarts.com", "3306", "ablivion", "rab313222");
            
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

        public int Login(string username, string password)
        {
            int c = controller.Login(connection, username, password);
            return c;
        }

        public void closeConnection()
        {
            connection.Close();
        }

        public Boolean addChat(string user, string message, int gameID)
        {
            return controller.insert_chat(connection, user, message, gameID);
        }
        //Create_Usr(conection, username, hashpass)
        //chatroom_recall(connection, gameID)
        //playerchat_recall(connection, string username)
        //create_game(connection, string[] users, string game_name);
        //premature_close(connection, players[], gameid);
    }
}
