using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SUOnlineRisk;

namespace RiskServer
{
    class Player
    {
        //constructor
        public Player(Socket client, String name, String PID, String authCode)
        {
            this.client = client;
            this.userName = name;
            this.pid = PID;
            this.authcode = authCode;
            this.inGame = false;
            this.host = false;
            this.unusedInfantry = 0;
            this.connectionAttempts = 0;
            startListening();
        }

        
        private void startListening()
        {
            //new thread for listening to client
            Thread dataCollection = new Thread(() =>
            {
                //same general implementation as ClientHandler
                Console.WriteLine("Starting player listening thread");
                while (true)
                {
                    byte[] data = new byte[1000];
                    int size2 = 0;
                    String gotData;
                    try
                    {
                        int size = client.Receive(data);
                        size2 = size;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e);
                        break;
                    }
                    if (size2 != 0)
                    {
                        Console.WriteLine("Recieved data (Player " + this.getID() + "): ");
                        char[] toParse = new char[1000];
                        for (int i = 0; i < size2; i++)
                        {
                            Console.Write(Convert.ToChar(data[i]));
                            toParse[i] = Convert.ToChar(data[i]);
                        }
                        gotData = new String(toParse);
                        Console.WriteLine();
                        dataParse(gotData);
                    }
                }
            });
            try
            {
                dataCollection.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
        }

        private void dataParse(string input)
        {
            try
            {
                //use straight input string unless section demands use of split string
                String[] parsed = input.Split(',');
                if (string.Compare(input, "startMyGame", true) == 0 && inGame == false)
                {
                    Console.WriteLine("Starting game " + myGame.getID());
                    myGame.beginGame(this);
                    inGame = true;
                }
                else if (string.Compare(input, "makeGame", true) == 0)
                {
                    //asks GameList to make a game with 
                    Console.WriteLine("Player " + this.getName() + " creating new game");
                    myGame = GameList.Instance.newGame(this, "SimpleRisk.map");
                    Console.WriteLine("Player " + this.getName() + " recieved game " + myGame.getID() + " OK.");
                    //TODO send player message confirming game join
                }
                else if (string.Compare(parsed[0], "joinGame", true) == 0)
                {
                    myGame = GameList.Instance.findGame(parsed[1]);
                    if (myGame == null)
                    {
                        Console.WriteLine("Couldn't find game " + parsed[1]);
                        //TODO send player specific error message: game outdated?
                    }
                    else
                    {
                        myGame.addPlayer(this);
                    }
                }
                else
                {
                    Console.WriteLine("Command not recognized (Player)");
                    currentMessage = input;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing data from Player " + this.getName() + ". Error message:");
                Console.WriteLine(e);
                this.sendMessage("err");
            }
        }

        Socket client;
        String currentMessage;
        String userName;
        String pid;
        String authcode;
        Game myGame;
        Boolean host;
        Boolean inGame;
        int unusedInfantry;
        int MaxConnectionAttempts = 2;
        int connectionAttempts;

        //returns next data string sent by client
        public String getMessage()
        {
            currentMessage = null;
            while (currentMessage == null) { };
            String returnMessage = currentMessage;
            currentMessage = null;
            return returnMessage;
        }

        public void setGame(Game game, Boolean host){
            myGame = game;
        }

        public void setHost()
        {
            host = true;
            this.sendMessage("host");
        }

        public void leaveGame()
        {
            myGame.remPlayer(this);
        }

        public void sendMessage(Message message)
        {
            try
            {
                client.Send(data);
                connectionAttempts = 0;
            }
            catch (Exception e)
            {
                e.ToString();
                Console.WriteLine("Looks like player " + this.getName() + " has been disconnected.");
                connectionAttempts++;
                if (connectionAttempts >= MaxConnectionAttempts)
                {
                    PlayerList.Instance.remPlayer(this);
                    Console.WriteLine("Ending connection to player " + this.getName());
                    try
                    {
                        this.client.Shutdown(SocketShutdown.Both);
                        this.client.Close();
                    }
                    catch (Exception e2)
                    {
                        Console.WriteLine("Could not close connection to player " + this.getName() + ". It's probably already closed.");
                        e2.ToString();
                    }
                }
            }
        }

        public void sendMessage(String message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            try
            {
                client.Send(data);
                connectionAttempts = 0;
            }
            catch (Exception e)
            {
                e.ToString();
                Console.WriteLine("Looks like player " + this.getName() + " has been disconnected.");
                connectionAttempts++;
                if (connectionAttempts >= MaxConnectionAttempts)
                {
                    PlayerList.Instance.remPlayer(this);
                    Console.WriteLine("Ending connection to player " + this.getName());
                    try
                    {
                        this.client.Shutdown(SocketShutdown.Both);
                        this.client.Close();
                    }
                    catch (Exception e2)
                    {
                        Console.WriteLine("Could not close connection to player " + this.getName() + ". It's probably already closed.");
                        e2.ToString();
                    }
                }
            }
        }

        public String getID()
        {
            return pid;
        }

        public String getName()
        {
            return userName;
        }

        public void subtractUnusedInfantry(int num)
        {
            this.unusedInfantry -= num;
            if (unusedInfantry < 0)
            {
                unusedInfantry = 0;
            }
        }

        public void addUnusedInfantry(int num)
        {
            unusedInfantry += num;
        }

        public int numUnusedInfantry()
        {
            return unusedInfantry;
        }

        public void setSocket(Socket s){
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            this.client = s;
            startListening();
        }

        public String getAuth()
        {
            return authcode;
        }

        public void startGame()
        {
            inGame = true;
        }
    }
}
