using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RiskServer
{
    class Program
    {

        static void Main(string[] args)
        {
            Socket client;
            TcpListener listener;

            try
            {
                //start main TCP listener in thread
                //used for new incoming connections
                Console.WriteLine("Starting TCP listener...");
                //from any IP
                listener = new TcpListener(IPAddress.Any, 2000);
                listener.Start();

                //continuously listen for incoming connections,
                //then start a new clienthandler thread
                var clientListenThread = new Thread(() =>
                    {
                        while (true)
                        {
                            Console.WriteLine("Listening for connection...");
                            client = listener.AcceptSocket();
                            IPEndPoint ip = client.RemoteEndPoint as IPEndPoint;
                            Console.WriteLine("Connection accepted from " + ip.Address);
                            var clientHandlerThread = new Thread(() =>
                                {
                                    new ClientHandler(client);
                                });
                            clientHandlerThread.Start();
                        }
                    });

                clientListenThread.Start();
                //after client listening thread has been created and started, wait for text commands
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Ready for command . . .");
                        String command = Console.ReadLine();
                        //quit server
                        if (command.Equals("stop"))
                        {
                            listener.Stop();
                            Environment.Exit(0);
                        }
                        //send message to everyone
                        else if (command.Equals("broadcast"))
                        {
                            Console.WriteLine("Message to broadcast to clients: ");
                            String message = Console.ReadLine();
                            PlayerList.Instance.broadcast(message);
                        }
                        //send message to desired player
                        else if (command.Equals("player message"))
                        {
                            Console.WriteLine("Enter player ID");
                            String playerID = Console.ReadLine();
                            Player player = PlayerList.Instance.findPlayer(playerID);
                            if (player != null)
                            {
                                Console.WriteLine("Found player " + player.getName() + ". Enter message");
                                String message = Console.ReadLine();
                                player.sendMessage(message);
                            }
                            else
                            {
                                Console.WriteLine("Couldn't find player with id " + playerID);
                            }
                        }
                        //list all currently connected players 
                        else if (command.Equals("list players"))
                        {
                            List<Player> playerlist = PlayerList.Instance.getList();
                            foreach (Player player in playerlist)
                            {
                                Console.WriteLine(player.getName());
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e);
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }

        }
    }
}
