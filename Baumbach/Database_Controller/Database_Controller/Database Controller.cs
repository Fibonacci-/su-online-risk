using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

//When using any function below Connect and Input_Form, make sure to open your conenction before hand and close the connection after each function

namespace Database_Controller
{
    public class Database_Controller
    {
        public MySqlConnection Input_Form()
            //Command Line login for a database use only once.
        {
            Console.Out.Write("Please enter the database location: ");
            string url = Console.In.ReadLine();
            Console.Out.Write("\nPlease enter port number or hit enter to default: ");
            string port = Console.In.ReadLine();
            if (port == "")
            {
                port = "3306";
            }
            string username = "";
            while (username == "")
            {
                Console.Out.Write("\nPlease enter your database admin username: ");
                username = Console.In.ReadLine();
            }
            string password = "";
            while (password == "")
            {
                Console.Out.Write("\nPlease enter your databae admin password: ");
                password = Console.In.ReadLine();
            }
            Console.Out.WriteLine("\n\nThank you, attempting to connect to database...");
            MySqlConnection connection = Connect(url, port, username, password);
            return connection;
        }
        public MySqlConnection Connect(string url, string port, string username, string password)
            //Used to create a connection string to database, for easier access use Input_Form().
        {
            string MyConnectionString = "datasource=" + url + ";port=" + port + ";username=" + username + ";password=" + password + ";";
            MySqlConnection connection = new MySqlConnection(MyConnectionString);
            try
            {
                Console.Out.WriteLine("\nAttempting to connect to specified server.");
                connection.Open();
                Console.Out.WriteLine("\nConnection Successful");
            }
            catch
            {
                Console.Out.WriteLine("I'm sorry but your connection was not successful.");
            }
            return connection;
        }
        public int Login(MySqlConnection connection, string usrname, string hashpass)
            //Attempts to log user in with ha
            //------------------------------------
            //Login Return Codes
            //------------------------------------
            //Error Code 0 = Correct Credentials
            //Error Code 1 = User does not exist
            //Error Code 2 = Incorrect Password
            //Error Code 3 = Connection Issue
            //------------------------------------
        {
            try
            {
                int ErrorCode = 0;
                string checkusr = "SELECT * FROM `risk`.`user_table`";
                MySqlCommand usrcheck = new MySqlCommand(checkusr, connection);
                MySqlDataReader dataReader = usrcheck.ExecuteReader();

                while (dataReader.Read())
                {
                    if (String.Equals((dataReader["user_name"] + ""), usrname, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        if (String.Equals((dataReader["user_pwd"] + ""), hashpass, StringComparison.OrdinalIgnoreCase) == true)
                        {
                            Console.Out.WriteLine("User name and password correct in new form of checker.");
                            dataReader.Close();
                            ErrorCode = 0;
                            string updatelastlog = "UPDATE `risk`.`user_table` SET last_login = NOW() WHERE user_name = '"+usrname+"'";
                            MySqlCommand lastlog = new MySqlCommand(updatelastlog, connection);
                            lastlog.ExecuteNonQuery();
                            return ErrorCode;
                        }
                        else
                        {
                            Console.Out.WriteLine("Found User name, pass wrong.");
                            dataReader.Close();
                            ErrorCode = 2;
                            return ErrorCode;
                        }
                    }
                }
                dataReader.Close();
                Console.Out.WriteLine("I am sorry but the user name you attempted to use does not exist.");
                ErrorCode = 1;
                return ErrorCode;
            }
            catch
            {
                Console.Out.WriteLine("\nSorry but the action preformed was unssucessful. Check your connection.");
                int ErrorCode = 3;
                return ErrorCode;
            }
        }
        public Boolean Create_Usr(MySqlConnection connection, string usrname, string hashpass)
            //Takes username, connection, and hashed password and checks values against the Database, if no values, creates a new user.
        {
            try
            {
                string checkusr = "SELECT * FROM `risk`.`user_table`";
                MySqlCommand usrcheck = new MySqlCommand(checkusr, connection);
                MySqlDataReader dataReader = usrcheck.ExecuteReader();
                
                while (dataReader.Read())
                {
                    if (String.Equals((dataReader["user_name"] + ""), usrname, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Console.Out.WriteLine("Username already exists.");
                        dataReader.Close();
                        return false;
                    }
                }
                dataReader.Close();

            }

            catch { Console.Out.WriteLine("\nSorry but the action preformed was unssucessful. Check your connection."); return false; }
            
            try
            {
                string usrstring = "INSERT INTO `risk`.`user_table` (`user_name`, `user_pwd`) VALUES ('" + usrname + "', '" + hashpass + "');";
                MySqlCommand usercreate = new MySqlCommand(usrstring);
                usercreate.Connection = connection;
                usercreate.ExecuteNonQuery();
                Console.Out.WriteLine("\nSuccess, User Created!");
                return true;
            }
            catch { Console.Out.Write("User Creation was unsuccessful."); return false; }
            
        }
        public Boolean insert_chat(MySqlConnection connection, string username, string chatmessage, int gameid)
            //Collects connection, username, chat message, and the game where it origionated from and inserts it into database.
        {
            try
            {
                string insert = "INSERT INTO `risk`.`chat_log` VALUES('"+gameid+"','"+username+"',NOW(),'"+chatmessage+"','')";
                MySqlCommand chatinsert = new MySqlCommand(insert);
                chatinsert.Connection = connection;
                chatinsert.ExecuteNonQuery();
                return true;
            }
            catch {return false; }
            
        }
        public List<string> chatroom_recall(MySqlConnection connection, int gameid)
            //Recalls messages based on the game they origionated from.
        {
            MySqlDataReader dataReader;
            List<string> log = new List<string>();
            try
            {
                string callchat = "SELECT * FROM `risk`.`chat_log`";
                MySqlCommand call = new MySqlCommand(callchat, connection);
                dataReader = call.ExecuteReader();
                while (dataReader.Read())
                {
                    if (Convert.ToInt32(dataReader["chat_id"]) == gameid)
                    {
                        log.Add("Message ID: " + dataReader["message_id"] + "[" + dataReader["date_stamp"] + "], User: " + dataReader["user_id"] + ", Sent: " + dataReader["text_body"]);
                    }
                }
                dataReader.Close();
            }
            catch { throw; }
            dataReader.Close();
            return log;
        }
        public List<string> playerchat_recall(MySqlConnection connection, string username)
            //Recalls messages based on the player who they origionated from.
        {
            MySqlDataReader dataReader;
            List<string> log = new List<string>();
            try
            {
                string callchat = "SELECT * FROM `risk`.`chat_log`";
                MySqlCommand call = new MySqlCommand(callchat, connection);
                dataReader = call.ExecuteReader();
                while (dataReader.Read())
                {
                    if (String.Equals((dataReader["user_id"] + ""), username, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        log.Add("Message ID: " + dataReader["message_id"] + " [" + dataReader["date_stamp"] + "]  In Game: " + dataReader["chat_id"] + username + ", Sent: " + dataReader["text_body"]);
                    }
                }
                dataReader.Close();
            }
            catch { throw; }
            dataReader.Close();
            return log;
        }
        public int create_game(MySqlConnection connection, string[] users, string game_name)
            //Creates a game log in database and returns the gameid.
            //User list must be 6 values long.
            //Use null if no player is present.
        {
            for (int i = 0; i < 6; i++)
            {
                if (users[i] == null) { users[i] = ""; }
                else { }
            }
            Int32 game_id = 0;
            try
            {
                string gamecr = "INSERT INTO risk.game_log VALUES('', NOW(), '', @user1, @user2, @user3, @user4, @user5, @user6, '', @name); SELECT LAST_INSERT_ID()";
                MySqlCommand creategame = new MySqlCommand(gamecr, connection);

                creategame.Parameters.AddWithValue("@user1", users[0]);
                creategame.Parameters.AddWithValue("@user2", users[1]);
                creategame.Parameters.AddWithValue("@user3", users[2]);
                creategame.Parameters.AddWithValue("@user4", users[3]);
                creategame.Parameters.AddWithValue("@user5", users[4]);
                creategame.Parameters.AddWithValue("@user6", users[5]);
                creategame.Parameters.AddWithValue("@name", game_name);

                game_id = Convert.ToInt32(creategame.ExecuteScalar());
                Console.Out.WriteLine("\nSuccess, lobby data created!");
            }
            catch
            {
                throw;
                return game_id;
            }
            return game_id;
        }
        public Boolean no_finish(MySqlConnection connection, int gameid)
            //Ties loose ends in database if a game is not finished.
        {
            try
            {
                string closecmd = "UPDATE `risk`.`game_log` SET game_end = NOW( ) WHERE game_id = @game_id";
                MySqlCommand close = new MySqlCommand(closecmd, connection);
                close.Parameters.AddWithValue("@game_id", gameid);
                close.ExecuteNonQuery();
                Boolean succeed = playerupdate(connection, gameid, null);
                if (succeed == false)
                {
                    Console.Out.WriteLine("Player Wins and Games Played update failed...Sorry man...");
                }
            }
            catch
            {
                throw;
                return false;
            }
            Console.Out.WriteLine("Updates succeeded. Player totals updated and game closed.");
            return true;
        }
        public Boolean game_finished(MySqlConnection connection, int gameid, string winner)
            //Proper close for a game that has finished. Updates player and game log list values.
           {
            try
            {
                string closecmd = "UPDATE `risk`.`game_log` SET game_end = NOW( ) AND winner = @uname WHERE game_id = @game_id";
                MySqlCommand close = new MySqlCommand(closecmd, connection);
                close.Parameters.AddWithValue("@game_id", gameid);
                close.Parameters.AddWithValue("@uname", winner);
                close.ExecuteNonQuery();
                Boolean succeed = playerupdate(connection, gameid, winner);
                if (succeed == false)
                {
                    Console.Out.WriteLine("Player Wins and Games Played update failed...Sorry man...");
                }
            }
            catch
            {
                throw;
                return false;
            }
            Console.Out.WriteLine("Updates succeeded. Player totals updated and game closed.");
            return true;
        }
        private Boolean playerupdate(MySqlConnection connection, int gameid, string winner)
            //Updates player total wins and games played.
        {
            string[] plist = new string[6];
            string gamescan = "SELECT * FROM `risk`.`game_log`";
            MySqlDataReader dataReader;
            MySqlCommand scan = new MySqlCommand(gamescan, connection);
            try
            {
                dataReader = scan.ExecuteReader();

                while (dataReader.Read())
                {
                    if (gameid == Convert.ToInt32(dataReader["game_id"]))
                    {
                        plist[0] = Convert.ToString(dataReader["lobby_leader"]);
                        plist[1] = Convert.ToString(dataReader["user2"]);
                        plist[2] = Convert.ToString(dataReader["user3"]);
                        plist[3] = Convert.ToString(dataReader["user4"]);
                        plist[4] = Convert.ToString(dataReader["user5"]);
                        plist[5] = Convert.ToString(dataReader["user6"]);
                        dataReader.Close();
                        break;
                    }
                }
            }
            catch
            {
                throw;
                dataReader.Close();
                return false;
            }
            try
            {
                foreach (string p in plist)
                {
                    string player_update = "UPDATE `risk`.`user_table` SET games_played = games_played + 1 WHERE user_name = @uname";
                    MySqlCommand pupdate = new MySqlCommand(player_update, connection);
                    pupdate.Parameters.AddWithValue("@uname", p);
                    pupdate.ExecuteNonQuery();
                }
                if (winner != null)
                {
                    string player_update = "UPDATE `risk`.`user_table` SET wins = wins + 1 WHERE user_name = @uname";
                    MySqlCommand pupdate = new MySqlCommand(player_update, connection);
                    pupdate.Parameters.AddWithValue("@uname", winner);
                    pupdate.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
                return false;
            }
            return true;
        }
        public int message_save(MySqlConnection connection, string message, int gameid)
            //Saves game state messages to the Server.
        {
            Int32 message_id = 0;
            try
            {
                string script = "INSERT INTO risk.action_log VALUES(@game_id, @message, ''); SELECT LAST_INSERT_ID()";
                MySqlCommand cr_message = new MySqlCommand(script, connection);

                cr_message.Parameters.AddWithValue("@game_id", gameid);
                cr_message.Parameters.AddWithValue("@message", message);

                message_id = Convert.ToInt32(cr_message.ExecuteScalar());
                Console.Out.WriteLine("Message Added");
            }
            catch
            {
                throw;
                return 0;
            }
            return message_id;
        }
        public string get_message(MySqlConnection connection, int messageid)
            //Returns game state messages from Server.
        {
            MySqlDataReader dataReader;
            string message_save = null;
            try
            {
                string callchat = "SELECT * FROM `risk`.`action_log`";
                MySqlCommand call = new MySqlCommand(callchat, connection);
                dataReader = call.ExecuteReader();
                while (dataReader.Read())
                {
                    if (Convert.ToInt32(dataReader["message_id"]) == messageid)
                    {
                        message_save = Convert.ToString(dataReader["message"]);
                        dataReader.Close();
                        return message_save;
                    }
                }
                dataReader.Close();
            }
            catch { throw; dataReader.Close(); return message_save; }
            dataReader.Close();
            return message_save;
        }
    }
}
