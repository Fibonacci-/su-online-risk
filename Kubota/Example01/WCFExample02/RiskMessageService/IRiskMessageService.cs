using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SUOnlineRisk;

namespace RiskMessageService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IRiskMessageService
    {
        // TODO: Add your service operations here
        [OperationContract]
        RiskMessage Request(RiskMessage msg);

        [OperationContract]
        bool Logon(string name, string password);

        [OperationContract]
        int createGame(int mapId);

        [OperationContract]
        bool joinGame(string name, int gameId);

        [OperationContract]
        bool startGame(string name, int gameId);
    }
}
