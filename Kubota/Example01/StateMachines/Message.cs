using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachines
{
    class Message
    {
        public MainState state;
        //Territory from;
        //Territory to;
        //int armyCounts;
        //int roll; //used during attacking to see which party won.
        //ReinforcementCard[] cards; //used at the end of the attacking phase to receive it, and the collection phase to use them.
        //bool bDone; //true if this is the last iteration of the operation
        public Message(MainState state)
        {
            this.state = state;
        }
    }
}
