using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;
using System.Drawing;
using System.ComponentModel;
using System.Threading;

namespace SUOnlineRisk
{
    /*
     * This player does not implement anything. It only relays message from input to the output.
     * All actions should be determined by GUI with an actual human player.
     * The class only serves to provide a common interface of the Player class.
     * */
    public class Human: Player
    {
        // constructor
        public Human(string UserName, Color ArmyColor, Map map)
            : base(UserName, ArmyColor, map)
        {
        }

        /*
         * Simply pass the message.
         */ 
        RiskMessage GenericAction(RiskMessage incoming)
        {
            RiskMessage outgoing = (RiskMessage) incoming.Clone();
            return outgoing;
        }

        override public RiskMessage Initialize(RiskMessage incoming)
        {
            return GenericAction(incoming);
        }
        override public RiskMessage Distribute(RiskMessage incoming)
        {
            return GenericAction(incoming);
        }

        override public RiskMessage NewArmies(RiskMessage incoming)
        {
            //collect the new armies
            return GenericAction(incoming);
        }

        override public RiskMessage TradeCard(RiskMessage incoming)
        {
            return GenericAction(incoming);
        }
        override public RiskMessage AdditionalArmies(RiskMessage incoming)
        {
            //collect the new additional armies - obtained by trading in cards
            return GenericAction(incoming);
        }
        override public RiskMessage Attack(RiskMessage incoming)
        {
            return GenericAction(incoming);
        }

        override public RiskMessage Roll(RiskMessage incoming)
        {
            return GenericAction(incoming);
        }

        override public RiskMessage AttackOutcome(RiskMessage incoming)
        {
            return GenericAction(incoming);
        }

        override public RiskMessage Conquer(RiskMessage incoming)
        {
            return GenericAction(incoming);
        }

        override public RiskMessage ReinforcementCard(RiskMessage incoming)
        {
            return GenericAction(incoming);
        }
        override public RiskMessage Reinforce(RiskMessage incoming)
        {
            return GenericAction(incoming);
        }
        override public RiskMessage Fortify(RiskMessage incoming)
        {
            return GenericAction(incoming);
        }
    }
}
