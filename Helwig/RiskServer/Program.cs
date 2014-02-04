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
                Console.WriteLine("Starting TCP listener...");
                
                
                    listener = new TcpListener(IPAddress.Any, 2000);
                    listener.Start();
                
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
                    while (true)
                    {
                        Console.WriteLine("Ready for command . . .");
                        String command = Console.ReadLine();
                        if (command.Equals("stop"))
                        {
                            listener.Stop();
                            Environment.Exit(0);
                        }
                        else if (command.Equals("broadcast"))
                        {
                            Console.WriteLine("Message to broadcast to clients: ");
                            String message = Console.ReadLine();
                            PlayerList.Instance.broadcast(message);
                        }
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
                        } else if(command.Equals("list players")) {
                            List<Player> playerlist = PlayerList.Instance.getList();
                            foreach (Player player in playerlist)
                            {
                                Console.WriteLine(player.getName());
                            }
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
