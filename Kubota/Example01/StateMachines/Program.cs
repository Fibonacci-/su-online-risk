using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SUOnlineRisk
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            Thread serverThread = new Thread(server.Run);

            for (int i = 0; i < 3; ++i)
            {
                Player player = new Player("Player " + (i+1));
                Thread playerThread = new Thread(player.Run);

                Client client = new Client(server);
                server.addClient(client);
                Thread clientThread = new Thread(client.Run);
                player.client = client;
                client.player = player;

                clientThread.Start();
                playerThread.Start();
            }
            serverThread.Start();

            Console.In.ReadLine();
            server.Stop();
        }
    }
}
