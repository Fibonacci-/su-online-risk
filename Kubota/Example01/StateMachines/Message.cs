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
        //int roll;
        //ReinforcementCard card;
        //
        public Message(MainState state)
        {
            this.state = state;
        }
    }
}
