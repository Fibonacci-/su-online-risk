using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;

namespace ClientDemo2
{
    class LocalClient
    {
        Message incoming;
        Message outgoing;
        public Player player
        {
            get;
            set;
        }
        public LocalServer server
        {
            get;
            set;
        }
        public string name
        {
            get { return player.nickname; }
        }
        public MainState state
        {
            get { return player.state; }
        }
        public RiskMessage Request(RiskMessage incoming)
        {
            RiskMessage outgoing = null;
            MainState state = incoming.state;
            player.state = state;
            if (state == MainState.Update) //this is for updating the map
            {
                outgoing = player.Update(incoming);
            }
            else if (state == MainState.Initialize)
            {
                outgoing = player.Initialize(incoming);
            }
            else if (state == MainState.Distribute)
            {
                outgoing = player.Distribute(incoming);
            }
            else if (state == MainState.NewArmies)
            {
                outgoing = player.NewArmies(incoming);
            }
            else if (state == MainState.TradeCard)
            {
                outgoing = player.TradeCard(incoming);
            }
            else if (state == MainState.AdditionalArmies)
            {
                outgoing = player.AdditionalArmies(incoming);
            }
            else if (state == MainState.Reinforce)
            {
                outgoing = player.Reinforce(incoming);
            }
            else if (state == MainState.Attack)
            {
                outgoing = player.Attack(incoming);
            }
            else if (state == MainState.Roll)
            {
                outgoing = player.Roll(incoming);
            }
            else if (state == MainState.ReinforcementCard)
            {
                outgoing = player.ReinforcementCard(incoming);
            }
            else if (state == MainState.Fortify)
            {
                outgoing = player.Fortify(incoming);
            }
            else if (state == MainState.Start)
            {
                outgoing = incoming;
            }
            return outgoing;
        }
    }
}
