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
        public Player(Socket client, String name, String PID)
        {
            this.client = client;
            this.userName = name;
            this.id = PID;
            startListening();
        }

        private void startListening()
        {
            var dataCollection = new Thread(() =>
            {
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
                    }
                    if (size2 != 0)
                    {
                        Console.WriteLine("Recieved data: ");
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
            dataCollection.Start();
        }

        private void dataParse(string input)
        {
            throw new NotImplementedException();
        }

        Socket client;
        String userName;
        String id;

        public void addToGame(Game game){
            game.addPlayer(this);
        }

        public void sendMessage(String message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            client.Send(data);
        }

        public String getID()
        {
            return id;
        }

        public String getName()
        {
            return userName;
        }
    }
}
