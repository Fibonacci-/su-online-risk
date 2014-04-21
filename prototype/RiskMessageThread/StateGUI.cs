﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SUOnlineRisk;
using System.Threading;

namespace SURiskGUI
{
    public partial class StateGUI : Form
    {
        //MainState state;
        Map map;
        Player player;
        Territory moveFrom;
        Territory moveTo;
        Territory attackFrom;
        Territory attackTo;
        //int count = 0;
        //int moves = 0;
        //int j;
        string mapfilename = @"C:\\SimpleRisk.map";
        string humanName = "Me";
        Dictionary<Territory, Label> labelMap = new Dictionary<Territory, Label>();
        //Label stateLabel = new Label();
        //Label armyLabel = new Label();
        //StateSelector selectorDlg;
        SharedMessage shared; //for synchronizing GUI with background worker.
        int numArmyAvailable; //keep the number of amies yet to be placed in Reinforcement.
        List<ArmyPlacement> newArmies = new List<ArmyPlacement>();
        int numDiceRolled; //used during conquering as we need to move at least this many armies.
        public StateGUI()
        {
            InitializeComponent();
            shared = new SharedMessage(humanName);
            backgroundWorker1.RunWorkerAsync();
            if (File.Exists(mapfilename) == false)
            {
                MessageBox.Show(mapfilename + " does not exist.");
                Application.Exit();
            }
            map = Map.loadMap(mapfilename);
            //player = new Player("Gary", Color.Red, map);
            player = new Human(humanName, Color.Red, map, shared, backgroundWorker1);
            this.pictureBox1.Image = map.getBitmap();

            foreach (Territory t in map.getAllTerritories())
            {
                Label ta = new Label();
                labelMap[t] = ta;
                ta.AutoSize = true;
                this.Controls.Add(ta);
                ta.BringToFront();
            }
            updateTerritoryLabels();

            //player.Territories = new List<Territory>();
            /*stateLabel.Left = 700 * pictureBox1.Width / map.getBitmap().Width - pictureBox1.Width / 30;
            stateLabel.Top = 600 * pictureBox1.Height / map.getBitmap().Height - pictureBox1.Height / 30;
            numInitialArmies = 10;
            stateLabel.Text = "Start";
            stateLabel.AutoSize = true;
            this.Controls.Add(stateLabel);
            stateLabel.BringToFront();*/
            //state = MainState.Unknown;

            //selectorDlg = new StateSelector();
        }

        /*
         * This utility function update the labels on each territory based on the information in the Map.
         * */
        void updateTerritoryLabels()
        {
            if (map == null) return;

            foreach (Territory t in map.getAllTerritories())
            {
                int tx = t.returnX();
                int ty = t.returnY();
                int wx = tx * pictureBox1.Width / map.getBitmap().Width - pictureBox1.Width / 30;
                int wy = ty * pictureBox1.Height / map.getBitmap().Height - pictureBox1.Height / 30;
                Label ta = labelMap[t];
                ta.Left = wx;
                ta.Top = wy;
                ta.Text = t.getOwner() + " " + t.numArmies;
            }
        }

        /*
         * this utility function returns a Territory at a click point (given as Point p).
         * */
        Territory clickedTerritory(Point p)
        {
            int margin = 30;
            foreach (Territory t in map.getAllTerritories())
            {
                //checks to see if the user clicked in a territory's parameters
                if (((p.X > t.returnX() - margin) && (p.X < t.returnX() + margin)) && ((p.Y > t.returnY() - margin) && (p.Y < t.returnY() + margin)))
                {
                    return t;
                }
            }
            return null;
        }

        //Keeps the labels where they should be in the picture when the window is resized
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (map == null) return;
            updateTerritoryLabels();
            //stateLabel.Left = 700 * pictureBox1.Width / map.getBitmap().Width - pictureBox1.Width / 30;
            //stateLabel.Top = 600 * pictureBox1.Height / map.getBitmap().Height - pictureBox1.Height / 30;
        }

        //Depending on the current state of the game, handles what happens when a player clicks a territory
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.shared.bWaitForUser == false) return;

            //gets where the user clicked
            Point p = new Point();
            p.X = e.Location.X * map.getBitmap().Width / pictureBox1.Width;
            p.Y = e.Location.Y * map.getBitmap().Height / pictureBox1.Height;
            //Initialize: select unoccupied territories to occupy
            //allows the user to occupy 4 armies initially. 
            //This wil be changed to deal with taking turns in the final product
            MainState state = shared.message.state;
            Territory t = clickedTerritory(p);
            if (state == MainState.Initialize)
            {
                if (t != null)
                {
                    if (t.getOwner() == "unoccupied")
                    {
                        if (MessageBox.Show("You selected " + t.getName() + ". Is this correct?",
                            "Yes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            lock (shared)
                            {
                                shared.message.territory_army.Add(new ArmyPlacement(t.getName(), humanName, 1));
                                shared.bWaitForUser = false;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("This territory is already occupied");
                    }
                }
            }
            //Distribute/reinforcements: add units to territories you own already
            else if (state == MainState.Distribute)
            {
                if (t != null)
                {
                    if (t.getOwner() == player.getName())
                    {
                        if (MessageBox.Show("You selected " + t.getName() + ". Is this correct?",
                            "Yes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            lock (shared)
                            {
                                shared.message.territory_army.Add(new ArmyPlacement(t.getName(), humanName, 1));
                                shared.bWaitForUser = false;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You do not own this territory and cannot place an army here");
                    }
                }
            }
            else if (state == MainState.Reinforce)
            {
                if (t != null)
                {
                    if (t.getOwner() == player.getName())
                    {
                        if (MessageBox.Show("You selected " + t.getName() + ". Is this correct?",
                            "Yes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            int k = newArmies.Count - numArmyAvailable;
                            newArmies[k].owner = player.getName();
                            newArmies[k].territory = t.getName();
                            newArmies[k].numArmies = 1;
                            numArmyAvailable--;
                            t.numArmies++; //update the local copy
                            updateTerritoryLabels(); //needed to update the local map, as we do not sent a message to the server yet.
                            if (numArmyAvailable <= 0)
                            {
                                lock (shared)
                                {
                                    foreach (ArmyPlacement a in newArmies)
                                    {
                                        shared.message.territory_army.Add(a);
                                    }
                                    newArmies.Clear();
                                    shared.bWaitForUser = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You do not own this territory and cannot place an army here");
                    }
                }
            }
            //fortify: state to move armies from one territory to another
            else if (state == MainState.Fortify)
            {
                if (t != null)
                {
                    if (t.getOwner() == player.getName())
                    {
                        moveFrom = t;
                    }
                    else
                    {
                        MessageBox.Show("You do not own this territory");
                    }
                }
            }
            //Attack: state to attack an enemy territory with one of your territories
            else if (state == MainState.Attack)
            {
                if (t != null && attackFrom == null)
                {
                    if (t.getOwner() == this.player.getName() && t.numArmies > 1)
                    {
                        attackFrom = t;
                    }
                    //need a minimum of 2 to attack. 
                    else if (t.numArmies <= 1)
                    {
                        MessageBox.Show("You cannot attack from this territory. You do not have sufficient units on this territory.");
                    }
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.shared.bWaitForUser == false) return;

            Point p = new Point();
            p.X = e.Location.X * map.getBitmap().Width / pictureBox1.Width;
            p.Y = e.Location.Y * map.getBitmap().Height / pictureBox1.Height;
            //fortify: state to move armies from one territory to another
            MainState state = shared.message.state;
            Territory t = clickedTerritory(p);
            if (state == MainState.Fortify)
            {
                if (t != null)
                {
                    if (t == moveFrom)
                    {
                        MessageBox.Show("You cannot move armies from a territory into that same territory");
                    }
                    else if (t.getOwner() != player.getName())
                    {
                        MessageBox.Show("You do not own this territory");
                    }
                    else if (MessageBox.Show("You want to move units from " + moveFrom.getName() + " to " + t.getName() + ". Is this correct?",
                            "Yes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        moveTo = t;
                        /*
                         * TK. I have disabled this for now.
                        FortifyMessage move = new FortifyMessage(moveFrom.numArmies - 1);
                        if (move.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            t.numArmies = t.numArmies + move.getArmiesToMove();
                            moveFrom.numArmies = moveFrom.numArmies - move.getArmiesToMove();
                            Label owner = labelMap[t];
                            owner.Text = t.getName() + " " + t.getOwner() + " " + t.numArmies;
                            Label owner2 = labelMap[moveFrom];
                            owner2.Text = moveFrom.getName() + " " + moveFrom.getOwner() + " " + moveFrom.numArmies;
                            moves++;
                            bDone = true;
                        }*/
                        lock (shared)
                        {
                            shared.message.territory_army.Add(new ArmyPlacement(moveTo.getName(), player.nickname, 1));
                            shared.message.territory_army.Add(new ArmyPlacement(moveFrom.getName(), player.nickname, -1));
                            shared.bWaitForUser = false;
                        }
                    }
                }
            }
            //Attack: state to attack an enemy territory with one of your territories
            if (state == MainState.Attack)
            {
                if (t != null && attackFrom != null)
                {
                    if (t.getOwner() == attackFrom.getOwner())
                    {
                        MessageBox.Show("You cannot attack this territory. You own this territory. Please select a territory to attack from and a territory to attack.");
                        //pop-up an error message.
                    }
                    else if(attackFrom.isNeighbor(t) == false)
                    {
                        MessageBox.Show("You cannot attack this territory. This is not a neighbor of " + attackFrom.getName() +".");
                    }
                    else
                    {
                        if (MessageBox.Show("Do you want to attack from " + attackFrom.getName() + " to " + t.getName() + "?",
                            "Yes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            //opens new form for attacking
                            attackTo = t;
                            lock (shared)
                            {
                                shared.message.from = attackFrom.getName();
                                shared.message.to = attackTo.getName();
                                shared.bWaitForUser = false;
                            }
                        }
                    }
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            using (RiskMessageThread.ServiceReference1.RiskServerClient proxy = new RiskMessageThread.ServiceReference1.RiskServerClient())
            {
                string[] names = {humanName, "Hal", "Watson" };
                Color[] colors = {Color.Red, Color.Blue, Color.Green};
                List<RiskClient> clients = new List<RiskClient>();
                
                int gameid = proxy.newGame(names[0], mapfilename);
                for(int i=0; i<names.Length; ++i)
                {
                    clients.Add(new RiskClient());
                    if(i==0)
                    {
                        //give the GUI map.
                        clients[i].player = this.player; // new Human(names[i], colors[i], map, shared, backgroundWorker1);
                    }
                    else
                    {
                        Map map2 = Map.loadMap(this.mapfilename); //give a separate map
                        clients[i].player = new Computer(names[i], colors[i], map2);
                    }
                    proxy.joinGame(names[i], gameid);
                }
                int ret = proxy.startGame(names[0], gameid);
                int count = 0;
                bool bRunning = true;
                while (bRunning)
                {
                    Console.Out.WriteLine("Iteration: " + ++count);
                    for (int i = 0; i < clients.Count; ++i)
                    {
                        //get the fresh map from the Server
                        {
                            RiskMessage outgoing = new RiskMessage(MainState.GetMap, clients[i].name);
                            RiskMessage resp = proxy.Request(outgoing);
                            resp.state = MainState.Update;
                            if (clients[i].player is Human)
                            {
                                Console.Out.WriteLine("Reporting progress with " + resp.state);
                                backgroundWorker1.ReportProgress(0, resp);
                            }
                            clients[i].Request(resp); //update the Map at the Client side.
                        }
                        //check for what to do
                        {
                            RiskMessage outgoing = new RiskMessage(MainState.Unknown, clients[i].name);
                            RiskMessage resp = proxy.Request(outgoing); //Get what to do
                            if (resp.state == MainState.Over)
                            {
                                bRunning = false;
                                break;
                            }
                            if (resp.state != MainState.Idle)
                            {
                                Console.Out.WriteLine("\t" + clients[i].name + ": " + resp.state);
                                if (clients[i].player is Human)
                                {
                                    Console.Out.WriteLine("Reporting progress with " + resp.state);
                                    shared.bWaitForUser = true; //Let the Human wait for GUI response
                                    backgroundWorker1.ReportProgress(0, resp);
                                }
                                RiskMessage resp2 = clients[i].Request(resp); //do whatever being told to do
                                RiskMessage resp3 = proxy.Request(resp2); //update the Map at the Server side
                            }
                        }
                    }
                }
                RiskMessage msg = new RiskMessage(MainState.Over, names[0]);
                this.backgroundWorker1.ReportProgress(100, msg);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RiskMessage msg = (RiskMessage)e.UserState;
            if (msg.state == MainState.Update)
            {
                updateTerritoryLabels();
            }
            else
            {
                if (msg.state == MainState.Initialize)
                {
                    MessageBox.Show("You are in " + msg.state + " mode. Click an unoccupied territory to occupy");
                }
                else if (msg.state == MainState.Distribute)
                {
                    MessageBox.Show("You are in " + msg.state + " mode. Click your territory to add an army");
                }
                else if (msg.state == MainState.TradeCard)
                {
                    MessageBox.Show("You are in " + msg.state + " mode. This mode has not been implemented");
                    lock (shared)
                    {
                        shared.bWaitForUser = false;
                    }
                }
                else if (msg.state == MainState.NewArmies)
                {
                    newArmies.Clear();
                    foreach (ArmyPlacement p in msg.territory_army)
                    {
                        if (p.territory == "any")
                        {
                            newArmies.Add(p);
                        }
                    }
                    numArmyAvailable = newArmies.Count();
                    MessageBox.Show("You are in " + msg.state + " mode.\nYou collected " + numArmyAvailable + " armies.");
                    this.numArmyLabel.Text = "Available armies: " + numArmyAvailable;
                    lock (shared)
                    {
                        shared.bWaitForUser = false;
                    }
                }
                else if (msg.state == MainState.Reinforce)
                {
                    MessageBox.Show("You are in " + msg.state + " mode. Select your territory to reinforce.");
                }
                else if (msg.state == MainState.Attack)
                {
                    if (MessageBox.Show("Do you want to attack?", "Attack", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    {
                        lock (shared)
                        {
                            shared.bWaitForUser = false;
                            shared.message.state = MainState.AttackDone;
                        }
                    }
                    else
                    {
                        attackFrom = null;
                        attackTo = null;
                        MessageBox.Show("You are in " + msg.state + " mode. Drag mouse to specify an attack.");
                    }
                }
                else if (msg.state == MainState.Roll)
                {
                    MessageBox.Show("A battle between " + msg.from + " and " + msg.to + " begins.");
                    DiceForm Attack = null;
                    //first figure out if you are attacking or being attacked.
                    Territory from = map.getTerritory(shared.message.from);
                    Territory to = map.getTerritory(shared.message.to);
                    if (from == null || to == null)
                    {
                        //this should never happens...
                        throw new NullReferenceException("Null territories for attacking. Cannot continue");
                    }
                    if (from.getOwner() == player.nickname) //you are the attacker
                    {
                        Attack = new DiceForm(from, Math.Min(3, from.numArmies - 1));
                    }
                    else if (to.getOwner() == player.nickname) //you are the defender
                    {
                        Attack = new DiceForm(to, Math.Min(2, to.numArmies));
                    }
                    if (Attack.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string result = "You rolled ";
                        foreach (int n in Attack.numberAttacker)
                        {
                            result += n + ", ";
                        }
                        numDiceRolled = Attack.numberAttacker.Length;
                        MessageBox.Show(result);
                    }
                    lock (shared)
                    {
                        shared.message.roll = (int[])Attack.numberAttacker.Clone();
                        shared.bWaitForUser = false;
                    }
                }
                else if (msg.state == MainState.AttackOutcome)
                {
                    MessageBox.Show("Attacker lost " + -msg.territory_army[0].numArmies + " and Defender lost " + -msg.territory_army[1].numArmies);
                    shared.bWaitForUser = false; //nothing to be done.
                }
                else if (msg.state == MainState.AttackDone)
                {
                    shared.bWaitForUser = false;
                }
                else if (msg.state == MainState.Conquer)
                {
                    //in this phase, we need to decide how many armies to move from the territory attacked from to the territory conquered.
                    //in the shared message's territory_army, there are two ArmyPlacement: [0] is the attacker and [1] is the conquered.
                    //we can move as many as attacker's armies - 1 to the conqured. 
                    //for now, we only move 1 army.
                    lock (shared)
                    {
                        shared.message.territory_army[1].numArmies = numDiceRolled; //this is the minimum amount we have to move.
                        shared.message.territory_army[0].numArmies -= numDiceRolled;
                        MessageBox.Show("You won! Moving one army from " + shared.message.territory_army[0].territory + " to " + shared.message.territory_army[1].territory);
                        shared.bWaitForUser = false;
                    }
                }
                else if (msg.state == MainState.ReinforcementCard)
                {
                    MessageBox.Show("You are in " + msg.state + " mode. This mode has not been implemented");
                    lock (shared)
                    {
                        shared.bWaitForUser = false;
                    }
                }
                else if (msg.state == MainState.Fortify)
                {
                    //check there are more than one territory
                    int count = map.getAllTerritories().FindAll(x => x.getOwner() == player.nickname).Count();
                    if (count > 1)
                    {
                        MessageBox.Show("You are in " + msg.state + " mode. Drag mouse to specify fortification.");
                    }
                    else
                    {
                        lock (shared)
                        {
                            shared.bWaitForUser = false;
                        }
                    }
                }
                else
                {
                    //This should not happen... except 'over'?
                    MessageBox.Show("You are in " + msg.state + " mode.");
                    lock (shared)
                    {
                        shared.bWaitForUser = false;
                    }
                }
                this.stateLabel.Text = msg.state.ToString();
                this.BringToFront();
            }
        }
    }
}