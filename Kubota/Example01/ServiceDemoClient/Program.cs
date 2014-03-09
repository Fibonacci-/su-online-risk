using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ServiceDemoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            EndpointAddress ep = new EndpointAddress("http://localhost:8000/MessageService/ServiceDemo");
            ServiceDemo.IMessageService proxy = ChannelFactory<ServiceDemo.IMessageService>.CreateChannel(new BasicHttpBinding(), ep);
            //string s = proxy.HelloIndigo();
            SUOnlineRisk.RiskMessage m = new SUOnlineRisk.RiskMessage(SUOnlineRisk.MainState.Conquer, "Gomachan");
            SUOnlineRisk.RiskMessage ack = proxy.request(m);
            Console.WriteLine(ack.playerName + " " + ack.state);
            Console.WriteLine("Press <ENTER> to terminate client.");
            Console.ReadLine();
        }
    }
}
