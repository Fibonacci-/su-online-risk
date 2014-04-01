using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SUOnlineRisk;

namespace WCFRiskServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IRiskServer
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here

        [OperationContract]
        Boolean Login(string username, string password);

        [OperationContract]
        Boolean newUser(string username, string hashpass);

        [OperationContract]
        Boolean chatMessage(string username, string chatmessage, int gameID);

        [OperationContract]
        void sendSystemMessage(int gameID, Message message);

        //todo
        [OperationContract]
        void logoff(string username);//maybe

        [OperationContract]
        List<int> findGames();

        [OperationContract]
        Boolean joinGame(string username, int gameID);

        [OperationContract]
        int startGame(string username, int gameID);//returns new gameID to use

        [OperationContract]
        int newGame(string username, string mapname);

        [OperationContract]
        Map getMap(int gameID, string mapname);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "WCFRiskServer.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
