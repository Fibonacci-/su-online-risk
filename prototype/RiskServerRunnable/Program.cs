using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace RiskServerRunnable
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(WCFRiskServer.RiskServer)))
            {
                host.Open();
                Console.WriteLine("Enter 'stop' to terminate server");
                while (true)
                {
                    string s = Console.ReadLine();
                    if (String.Compare(s, "stop", true) == 0)
                    {
                        break;
                    }
                    Console.WriteLine("Didn't catch that");
                }
            }
        }
    }
}
