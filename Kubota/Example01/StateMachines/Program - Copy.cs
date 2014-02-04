using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace StateMachines
{
    class Program
    {
        static void Main(string[] args)
        {
            Player[] players = new Player[6];
            Thread[] playerThreads = new Thread[players.Length];
            for(int i=0; i<players.Length; ++i)
            {
                players[i] = new Player("Player " + (i+1));
                playerThreads[i] = new Thread(players[i].Run);
                playerThreads[i].Start();
            }
            while(true)
            {
                string input = Console.In.ReadLine();
                try
                {
                    int id = Int32.Parse(input);
                    if (id >= 0 && id < players.Length)
                    {
                        players[id].Send(new Message());
                    }
                }
                catch(FormatException ex)
                {
                    Console.Error.WriteLine("Failed to parse.");
                    Console.Error.WriteLine(ex);
                }
            }
        }
    }
}
