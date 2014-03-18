using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;

namespace SUOnlineRisk
{
    class Message
    {
        public MainState state;
        public string playerName;
        public string from; //name of a territory to attack from
        public string to;   //name of a territory to attack to
        public int armyGained; //the number of armies gained after attack or collection
        public int armyLost; //the number
        public int[] roll = new int[3]; //used during attacking to see which party won.
        ReinforcementCard[] cards; //used at the end of the attacking phase to receive it, and the collection phase to use them.
        public bool bDone; //true if this is the last iteration of the operation
        public Message(MainState state)
        {
            this.state = state;
        }
    }
}
