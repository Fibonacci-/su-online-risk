using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;
using System.Threading;
using System.ServiceModel;

namespace RiskMessageService
{
    public class RiskMessageClient
    {
        public RiskMessageClient(string name)
        {
            this.name = name;
        }
        public string name;
        public RiskMessage Request(RiskMessage incoming)
        {
            RiskMessage outgoing = null;
            MainState state = incoming.state;
            return outgoing;
        }

        //client's thread.
        public void Run()
        {
            /*EndpointAddress ep = new EndpointAddress("http://localhost:8000/MessageService/ServiceDemo");
            ServiceDemo.IMessageService proxy = ChannelFactory<ServiceDemo.IMessageService>.CreateChannel(new BasicHttpBinding(), ep);
            SUOnlineRisk.RiskMessage m = new SUOnlineRisk.RiskMessage(SUOnlineRisk.MainState.Conquer, "Gomachan");
            SUOnlineRisk.RiskMessage ack = proxy.request(m);*/

            EndpointAddress ep = new EndpointAddress("http://localhost:8000/RiskMessageServer/RiskMessageServer");
            IRiskMessageService server = ChannelFactory<IRiskMessageService>.CreateChannel(new BasicHttpBinding(), ep);

            RiskMessage msg = new RiskMessage(MainState.Idle, this.name);
            while(true)
            {
                RiskMessage outgoing = server.Request(msg);
                Console.Out.WriteLine(name + ": " + outgoing.state + " " + outgoing.playerName);
                msg = outgoing;
                Thread.Sleep(100);                
            }
        }
    }
}
