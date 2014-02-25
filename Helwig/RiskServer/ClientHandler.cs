using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RiskServer
{
    class ClientHandler
    {
        public ClientHandler(Socket client)
        {
            //set client
            this.client = client;
            //start listening thread for client
            var dataCollection = new Thread(() =>
                {
                    while (true)
                    {
                        //buffer
                        byte[] data = new byte[1000];
                        //size2 is a workaround for a null check caused by size being defined
                        //inside the try/catch block
                        int size2 = 0;
                        try
                        {
                            //thread pauses here until client sends something
                            //if client disconnects, throws a SocketException
                            int size = client.Receive(data);
                            size2 = size;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e);
                            break;
                        }
                        //if data has been recieved
                        if (size2 != 0)
                        {
                            //handle recieved data
                            Console.WriteLine("Recieved data (CH): ");
                            char[] toParse = new char[1000];
                            for (int i = 0; i < size2; i++)
                            {
                                //write recieved data to console
                                Console.Write(Convert.ToChar(data[i]));
                                toParse[i] = Convert.ToChar(data[i]);
                            }
                            //\n
                            Console.WriteLine();
                            //parse recieved data using parse() method.
                            //if client can be passed to another class,
                            //this method returns true.
                            //false if ClientHandler needs to keep listening to the client.
                            size2 = 0;
                            if (this.parse(toParse))
                            {
                                break;
                            }
                            
                        }
                    }
                });
            //start thread
            dataCollection.Start();
        }

        //generic messages
        byte[] loginInvalid = Encoding.ASCII.GetBytes("l,0");
        byte[] genericLoginOK = Encoding.ASCII.GetBytes("l,1");
        //returns true if clientHandler no longer needs to listen to the socket
        private Boolean parse(char[] toParse)
        {
            //makes a string out of the char array
            String recievedData = new String(toParse);
            //splits the string
            String[] parsing = recievedData.Split(',');
            //just in case the string is empty for whatever reason
            try
            {
                //nice and simple ping function
                if (parsing[0].Equals("1"))
                {
                    byte[] byteData = Encoding.ASCII.GetBytes("1");
                    client.Send(byteData);
                    return false;
                }
                //client intends to disconnect
                else if (parsing[0].Equals("0"))
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Disconnect(true);
                    return true;
                }
                //requesting login
                else if (parsing[0].Equals("l"))
                {
                    Boolean b = true;
                    //Check login info against Roger here
                    //Boolean b = Login.check(parsing[1], parsing[2]);
                    if (b)
                    {
                        //authcode used for automatic relogin
                        String newAuthcode = generateID(3);
                        //ID will be from Roger's database
                        String newID = generateID(6);
                        //make login OK byte array to send
                        byte[] loginOK = Encoding.ASCII.GetBytes("l,1," + newID + "," + newAuthcode);
                        client.Send(loginOK);
                        Console.WriteLine("Player ID for " + parsing[1] + " is " + newID + ", authcode " + newAuthcode);
                        //make a new player and add it to the main list
                        Player player = new Player(client, parsing[1], newID, newAuthcode);
                        PlayerList.Instance.addPlayer(player);
                        //allow thread to exit
                        return true;
                    }
                    else
                    {
                        //send generic bad request
                        client.Send(loginInvalid);
                        return false;
                    }
                }
                //requesting re-login after dropped connection
                else if (parsing[0].Equals("req"))
                {
                    //retrieve information from previously split array and find requested player
                    Console.WriteLine("Looking for player");
                    String pid = parsing[1];
                    String authcode = parsing[2];
                    Player player = PlayerList.Instance.findPlayer(pid);
                    //findPlayer() returns null if player ID does not exist
                    if (player != null)
                    {
                        Console.WriteLine("Player " + player.getName() + " found.");
                        String playerauth = player.getAuth();
                        Console.WriteLine("Player authcode is " + playerauth + ". Recieved auth is " + authcode);
                        //make sure recieved authcode and stored authcode are the same
                        //for some reason, string.Equals(anotherString) always returns false in this situation
                        if (string.Compare(playerauth, authcode, true) == 0)
                        {
                            player.setSocket(client);
                            return true;
                        }
                        Console.WriteLine("Authcode incorrect!");
                    }
                    Console.WriteLine("Player not found.");
                    return false;
                }
                //if command isn't recognized, client most likely dropped connection and still thinks it's in a game
                //request re-login
                else
                {
                    Console.WriteLine("Client message not recognized. Requesting clarification");
                    try
                    {
                        byte[] byteData = Encoding.ASCII.GetBytes("req");
                        client.Send(byteData);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e);
                        return false;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
            return false;
        }

        //generate random string of given size
        public static String generateID(int size)
        {
            char[] buffer = new char[size];
            string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmopqrstuv1234567890";
            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[PlayerList.Instance.getRandom().Next(_chars.Length)];
            }
            return new string(buffer);
        }

        Socket client;
    }
}
