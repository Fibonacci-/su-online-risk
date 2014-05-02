using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;

namespace SUOnlineRisk
{
    public class RiskClient
    {
        RiskMessage incoming;
        RiskMessage outgoing;
        public Player player
        {
            get;
            set;
        }
        public RiskSequencer server
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
            set { player.state = value; }
        }
        public RiskMessage Request(RiskMessage incoming)
        {
            RiskMessage outgoing = null;
            MainState state = incoming.state;
            //player.state = state;
            if (state == MainState.Initialize)
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
            else if (state == MainState.Reinforce)
            {
                outgoing = player.Reinforce(incoming);
            }
            else if (state == MainState.Attack)
            {
                outgoing = player.Attack(incoming);
            }
            else if (state == MainState.AttackDone)
            {
                outgoing = player.AttackDone(incoming);
            }
            else if (state == MainState.AttackOutcome)
            {
                outgoing = player.AttackOutcome(incoming);
            }
            else if (state == MainState.Roll)
            {
                outgoing = player.Roll(incoming);
            }
            else if (state == MainState.Conquer)
            {
                outgoing = player.Conquer(incoming);
            }
            //else if (state == MainState.Eliminate)
            //{
            //    // TODO: this
            //}
            else if (state == MainState.ReinforcementCard)
            {
                outgoing = player.ReinforcementCard(incoming);
            }
            else if (state == MainState.Fortify)
            {
                outgoing = player.Fortify(incoming);
            }
            else if (state == MainState.Update)
            {
                //UpdateMap(incoming.territory_army); //TK. I think this is redundant to player.Update.
                outgoing = player.Update(incoming);
                //outgoing = new RiskMessage(MainState.Update, incoming.playerName);
            }
            else if (state == MainState.Start)
            {
                outgoing = incoming;
            }
            return outgoing;
        }
        
        public void UpdateMap(List<ArmyPlacement> placement)
        {
            foreach(ArmyPlacement a in placement)
            {
                Territory t = player.map.getTerritory(a.territory);
                t.numArmies = a.numArmies;
                t.setOwner(a.owner);
            }
        }
    }
}
