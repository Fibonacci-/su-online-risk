using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            if(input.Equals("gameStart"))
            {
                myGame.beginGame(this);
            }
        }

        Socket client;
        String userName;
        String pid;
        String authcode;
        Game myGame;
        Boolean host;
        Boolean inGame;

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

        public void sendMessage(String message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            client.Send(data);
        }

        public String getID()
        {
            return pid;
        }

        public String getName()
        {
            return userName;
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
    }
}
