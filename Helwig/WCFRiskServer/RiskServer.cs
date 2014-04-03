using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MySql.Data.MySqlClient;
using Database_Controller;

namespace WCFRiskServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class RiskServer : IRiskServer
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public Boolean Login(string user, string hashpass)
        {
            sUtilities u = sUtilities.Instance;
            int i = u.Login(user, hashpass);
            if (i == 0)
            {
                return true;
            }
            return false;

        }

        public Boolean chatMessage(string username, string chatmessage, int gameID)
        {
            sUtilities u = sUtilities.Instance;
            return u.addChat(username, chatmessage, gameID);
        }
    }
}
