using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;
using System.Drawing;
using System.Threading;

namespace ClientDemo2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ClientDemo2.ServiceReference1.RiskMessageServiceClient proxy = new ClientDemo2.ServiceReference1.RiskMessageServiceClient())
            {
                Console.Out.Write("Your name: ");
                string name = Console.ReadLine();
                proxy.Logon(name, "blablablah");
                MainState state = MainState.Idle;
                int gameid;
                if(name == "admin")
                {
                    gameid = proxy.createGame(1);
                    Console.Out.WriteLine("A game " + gameid + " is created.");
                    proxy.joinGame(name, gameid);
                    Console.Out.Write("Hit ENTER to start the game: ");
                    Console.In.ReadLine();
                    proxy.startGame(name, gameid);
                }
                else
                {
                    Console.Out.Write("Type the game id: ");
                    string input = Console.In.ReadLine();
                    gameid = Int32.Parse(input);
                    if(proxy.joinGame(name, gameid)==false)
                    {
                        Console.Error.WriteLine("Wrong game id. Bye.");
                        return;
                    }
                }
                while(true)
                {
                    RiskMessage msg = new RiskMessage(state, name);
                    RiskMessage resp = proxy.Request(msg);
                    Console.Out.WriteLine(msg.state + " => " + resp.state + " " + resp.playerName);
                    state = resp.state;
                    if(state == MainState.Over)
                    {
                        break;
                    }
                    Thread.Sleep(500);
                }
                Console.WriteLine("Press <ENTER> to terminate server.");
                Console.ReadLine();
            }
        }
    }
}
