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
    public class Human: Player
    {
        BackgroundWorker worker;
        SharedMessage shared; //used to communicate with the GUI
        // constructor
        public Human(string UserName, Color ArmyColor, Map map, SharedMessage shared, BackgroundWorker worker)
            : base(UserName, ArmyColor, map)
        {
            this.shared = shared;
            this.worker = worker;
        }

        /*
         * This method defines a generic handling of the message.
         * 1. it copies the message to the message shared between GUI and worker thread.
         * 2. wait for the user action by setting bWaitForUser bool variable.
         * 3. return the shared message.
         * GUI needs to use the state to take proper action. 
         * The result of the GUI needs to be stored into the shared message content.
         */ 
        RiskMessage GenericAction(RiskMessage incoming)
        {
            lock (shared)
            {
                shared.message = (RiskMessage) incoming.Clone(); //create a clean slate message
                //shared.bWaitForUser = true; //We do this in the main loop
                //worker.ReportProgress(0, shared.message);
            }
            while (shared.bWaitForUser)
            {
                Thread.Sleep(100);
            }
            return shared.message;
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
