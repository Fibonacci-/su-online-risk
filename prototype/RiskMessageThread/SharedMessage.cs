using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;

namespace SUOnlineRisk
{
    /*
     * This utility class is used to synchronize GUI thread and background worker thread in StateGUI.cs.
     * */
    public class SharedMessage
    {
        public RiskMessage message;
        public bool bWaitForUser;
        public SharedMessage(string name)
        {
            message = new RiskMessage(MainState.Unknown, name);
            bWaitForUser = false;
        }
    }
}
