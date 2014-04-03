using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ServiceDemo
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class MessageService : IMessageService
    {
        public SUOnlineRisk.RiskMessage request(SUOnlineRisk.RiskMessage m)
        {
            Console.WriteLine(m.playerName + " " + m.state);
            SUOnlineRisk.RiskMessage ack = new SUOnlineRisk.RiskMessage(SUOnlineRisk.MainState.AttackDone, m.playerName + "'s namesis");
            return ack;
        }
    }
}
