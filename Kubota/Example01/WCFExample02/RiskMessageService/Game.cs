using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUOnlineRisk;

namespace RiskMessageService
{
    class Game
    {
        class PlayerAvatar
        {
            public string name;
            public List<ArmyPlacement> changes;
        }
        static int MaxNumPlayers = 6;

        List<PlayerAvatar> players = new List<PlayerAvatar>();
        PlayerAvatar currentPlayer;
        public bool gameOn = false;
        MainState state;
        int gameID;
        Map map;
        int tics; //counter - only for DEBUGGING.

        public bool Start()
        {
            if(gameOn==true)
            {
                return false;
            }
            else if (players.Count() == 0)
            {
                return false;
            }
            else
            {
                tics = 0;
                currentPlayer = players[0];
                state = MainState.Idle;
                gameOn = true;
                return true;
            }
        }

        public bool addPlayer(string name)
        {
            if(players.Count >= MaxNumPlayers)
            {
                return false;
            }
            else
            {
                PlayerAvatar player = new PlayerAvatar();
                player.name = name;
                player.changes = new List<ArmyPlacement>();
                players.Add(player);
                return true;
            }
        }

        /*
         * This method update the map from the message.
         */
        public void Update(RiskMessage msg)
        {
            //IMPLEMENT updating the map

            //store any changes to pass on to each player.
            foreach(PlayerAvatar player in players)
            {
                if(player != currentPlayer)
                {
                    player.changes.AddRange(msg.territory_army);
                }
            }
        }

        /*
         * This method figures out the next state given the current state and the message.
         */
        public RiskMessage Next(RiskMessage msg)
        {
            if (gameOn)
            {
                if (tics >= 100)
                {
                    return new RiskMessage(MainState.Over, msg.playerName);
                }
                else if (msg.playerName == currentPlayer.name)
                {
                    tics++;
                    if (state == MainState.Idle)
                    {
                        state = MainState.NewArmies;
                    }
                    else if (state == MainState.NewArmies)
                    {
                        state = MainState.Reinforce;
                    }
                    else if (state == MainState.Reinforce)
                    {
                        state = MainState.Attack;
                    }
                    else if (state == MainState.Attack)
                    {
                        state = MainState.AttackDone;
                    }
                    else if (state == MainState.AttackDone)
                    {
                        state = MainState.Fortify;
                    }
                    else if (state == MainState.Fortify)
                    {
                        state = MainState.Idle;
                        //the end of turn for this player. Activate the next player.
                        currentPlayer = players[(players.FindIndex(x => x == currentPlayer) + 1) % players.Count()];
                        Console.Out.WriteLine("Switch the player to " + currentPlayer);
                    }
                    else
                    {
                        state = MainState.Unknown;
                    }
                    return new RiskMessage(state, msg.playerName);
                }
                else
                {
                    //tell this player any changes made to the map by the currently active player
                    PlayerAvatar player = players.Find(x => x.name == msg.playerName);
                    RiskMessage reply = new RiskMessage(MainState.Update, msg.playerName);
                    reply.territory_army = player.changes;
                    player.changes.Clear();
                    return reply;
                }
            }
            else
            {
                return new RiskMessage(MainState.Idle, msg.playerName);
            }
        }
    }
}
