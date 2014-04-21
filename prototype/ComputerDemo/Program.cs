using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;

namespace ComputerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Map map = new Map(@"..\..\SimpleRiskMap.png"); //We will be using the same map - this needs to be changed
            Map map = Map.loadMap(@"..\..\SimpleRisk.map");
            Player[] players = new Player[3];
            RiskClient[] clients = new RiskClient[3];
            RiskSequencer server = new RiskSequencer();
            server.map = map;
            server.generateReinforcementCards(map);
            string[] names = { "Tom", "Amy", "Jack" };
            System.Drawing.Color[] colors = {System.Drawing.Color.Red, System.Drawing.Color.Green, System.Drawing.Color.Blue };
            for (int i = 0; i < 3; ++i)
            {
                Map map2 = Map.loadMap(@"..\..\SimpleRisk.map"); // give each player their own copy of the map
                players[i] = new Computer(names[i], colors[i], map2);
                clients[i] = new RiskClient();
                clients[i].player = players[i];
                clients[i].server = server;
                server.addClient(clients[i]);
            }
            {
                bool bRunning = true;
                server.Start();

                int count = 0;
                while (bRunning)
                {
                    Console.Out.WriteLine("Iteration: " + ++count);
                    for (int i = 0; i < clients.Length; ++i)
                    {
                        //get the fresh map from the Server
                        {
                            RiskMessage outgoing = new RiskMessage(MainState.GetMap, clients[i].name);
                            RiskMessage resp = server.Request(outgoing);
                            resp.state = MainState.Update;
                            clients[i].Request(resp); //update the Map at the Client side.
                        }
                        //check for what to do
                        {
                            RiskMessage outgoing = new RiskMessage(MainState.Unknown, clients[i].name);
                            RiskMessage resp = server.Request(outgoing); //Get what to do
                            if (resp.state == MainState.Over)
                            {
                                bRunning = false;
                                break;
                            }
                            if (resp.state != MainState.Idle)
                            {
                                Console.Out.WriteLine("\t" + clients[i].name + ": " + resp.state);
                                RiskMessage resp2 = clients[i].Request(resp); //do whatever being told to do
                                RiskMessage resp3 = server.Request(resp2); //update the Map at the Server side
                            }
                        }
                    }
                }
                Console.Out.WriteLine("Game over");
            }

            Console.Out.WriteLine(" ");
        }
    }
}
