﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUOnlineRisk
{
    //general purpose message - a base class for other messages.
    [Serializable]
    public class Message: ICloneable
    {
        public MainState state;
        public string playerName;
        public Message(MainState state, string name)
        {
            this.state = state;
            this.playerName = name;
        }
        virtual public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    //describe description of a battle.
    [Serializable]
    public class AttackMessage : Message, ICloneable
    {
        public AttackMessage(MainState state, string name) : base(state, name) { }

        public string from; //name of a territory to attack from
        public string to;   //name of a territory to attack to
        override public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    //indicates the end of attack phase.
    [Serializable]
    public class AttackDoneMessage : Message, ICloneable
    {
        public AttackDoneMessage(MainState state, string name) : base(state, name) { }
        override public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    //Give a reinforcement card.
    [Serializable]
    public class ReinforcementCardMessage : Message, ICloneable
    {
        public ReinforcementCardMessage(MainState state, string name) : base(state, name) { }
        public ReinforcementCard card; //given if a territory has been conquered.
        override public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    //describe how dice roll is requested and how the roll ended up. The length of the arry indicateds the number of dice being used.
    [Serializable]
    public class RollMessage : Message, ICloneable
    {
        public RollMessage(MainState state, string name, Boolean attacking = true) : base(state, name) { attacker = attacking; }
        public int[] roll; //contains the result of dice roll. Numbers outside 1 and 6 are ignored.
        public Boolean attacker;
        override public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    //used to submit reinforcement cards
    [Serializable]
    public class TradeCardMessage : Message, ICloneable
    {
        public TradeCardMessage(MainState state, string name) : base(state, name) { }
        public int[] cardIds; //used to cash in reinforcement cards
        override public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    //this messageis used for Initialization, Distribution, NewArmies, AdditionalArmies, Conquer, Fortification, and Update
    [Serializable]
    public class ArmyPlacementMessage: Message, ICloneable
    {
        public ArmyPlacementMessage(MainState state, string name) : base(state, name) { territory_army = new List<ArmyPlacement>(); }
        public List<ArmyPlacement> territory_army; //each item describe how armies are placed. Use 'any' if the armies can be placed anywhere. 
        override public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    [Serializable]
    public class RiskMessage: Message
    {
        //public MainState state;
        //public string playerName;
        public string from; //name of a territory to attack from
        public string to;   //name of a territory to attack to
        public List<ReinforcementCard> card; //given if a territory has been conquered.
        public int[] roll; //contains the result of dice roll. Numbers outside 1 and 6 are ignored.
        public Boolean attacker; //identify if the roller is attacker or not
        public int[] cardIds; //used to cash in reinforcement cards
        public List<ArmyPlacement> territory_army; //each item describe how armies are placed. Use 'any' if the armies can be placed anywhere. 
        public RiskMessage(MainState state, string name): base(state, name)
        {
            //this.state = state;
            //this.playerName = name;
            territory_army = new List<ArmyPlacement>();
            card = new List<ReinforcementCard>();
        }
        override public object Clone()
        {
            return this.MemberwiseClone();
        }
        public override bool Equals(object obj)
        {
            RiskMessage msg = obj as RiskMessage;
            if(msg == null)
            {
                return false;
            }
            if (this.attacker != msg.attacker) return false;
            else if (this.from != msg.from) return false;
            else if (this.playerName != msg.playerName) return false;
            else if (this.state != msg.state) return false;
            else if (this.to != msg.to) return false;
            //now compare items in each container
            if (this.card.Count != msg.card.Count) return false;
            for (int i = 0; i < this.card.Count; ++i)
            {
                if (this.card[i].Equals(msg.card[i]) == false) return false;
            }
            if (this.cardIds == null && msg.cardIds != null) return false;
            else if (this.cardIds != null && msg.cardIds == null) return false;
            else if (this.cardIds != null && msg.cardIds != null)
            {
                if (this.cardIds.Length != msg.cardIds.Length) return false;
                for (int i = 0; i < this.cardIds.Length; ++i)
                {
                    if (this.cardIds[i] != msg.cardIds[i]) return false;
                }
            }
            if (this.roll == null && msg.roll != null) return false;
            else if (this.roll != null && msg.roll == null) return false;
            else if (this.roll != null && msg.roll != null)
            {
                if (this.roll.Length != msg.roll.Length) return false;
                for (int i = 0; i < this.roll.Length; ++i)
                {
                    if (this.roll[i] != msg.roll[i]) return false;
                }
            }
            if (this.territory_army.Count != msg.territory_army.Count) return false;
            for (int i = 0; i < this.territory_army.Count; ++i)
            {
                if (this.territory_army[i].Equals(msg.territory_army[i]) == false) return false;
            }
            return true;
        }
    }
}
