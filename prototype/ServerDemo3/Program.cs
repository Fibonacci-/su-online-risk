using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ServerDemo3
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(WCFRiskServer.RiskServer)))
            {
                host.Open();
                Console.WriteLine("Press <ENTER> to terminate server.");
                Console.ReadLine();
            }
        }
    }
}
