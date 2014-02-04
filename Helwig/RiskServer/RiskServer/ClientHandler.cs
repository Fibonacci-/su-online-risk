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
            this.client = client;
            var dataCollection = new Thread(() =>
                {
                    while (true)
                    {
                        byte[] data = new byte[1000];
                        int size2 = 0;
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
                            if (this.parse(toParse))
                            {
                                break;
                            }
                            Console.WriteLine();
                        }
                    }
                });
            dataCollection.Start();
        }

        byte[] loginOK = Encoding.ASCII.GetBytes("l,1");
        byte[] loginInvalid = Encoding.ASCII.GetBytes("l,0");

        //returns true if clientHandler no longer needs to listen to the socket
        private Boolean parse(char[] toParse)
        {
            String recievedData = new String(toParse);
            String[] parsing = recievedData.Split(',');
            if (parsing[0].Equals("1"))
            {
                byte[] byteData = Encoding.ASCII.GetBytes("1");
                client.Send(byteData);
                return false;
            }
            else if (parsing[0].Equals("0"))
            {
                client.Shutdown(SocketShutdown.Both);
                client.Disconnect(true);
                return true;
            }
            else if (parsing[0].Equals("l"))
            {
                Boolean b = true;
                //Boolean b = Login.check(parsing[1], parsing[2]);
                if (b)
                {
                    client.Send(loginOK);
                    String newID = generateID(6);
                    Console.WriteLine("Player ID for " + parsing[1] + " is " + newID);
                    Player player = new Player(client, parsing[1], newID);
                    PlayerList.Instance.addPlayer(player);
                    return true;
                }
                else
                {
                    client.Send(loginInvalid);
                    return false;
                }
            }
            else
            {
                byte[] byteData = Encoding.ASCII.GetBytes("0");
                client.Send(byteData);
                return false;
            }
        }

        public String generateID(int size)
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
