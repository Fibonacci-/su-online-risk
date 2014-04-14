using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ServiceDemoHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(ServiceDemo.MessageService), new Uri("http://localhost:8000/MessageService")))
            {
                host.AddServiceEndpoint(typeof(ServiceDemo.IMessageService), new BasicHttpBinding(), "ServiceDemo");
                host.Open();
                Console.WriteLine("Press <ENTER> to terminate the service host");
                Console.ReadLine();
            }
        }
    }
}
