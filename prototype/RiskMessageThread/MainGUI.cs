using System;
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
    public partial class MainGUI : Form
    {
        //proxy for communicating with a server.
        RiskMessageThread.ServiceReference1.RiskServerClient proxy = null;

        bool bOwner; //true if you started a game.
        int gameid = -1;

        //MainState state;
        Map map;
        Player player;
        Territory moveFrom;
        Territory moveTo;
        Territory attackFrom;
        Territory attackTo;

        string mapfilename = @"C:\\SimpleRisk.map";

        Dictionary<Territory, Label> labelMap = new Dictionary<Territory, Label>();
        SharedMessage shared = null; //for synchronizing GUI with background worker.
        int numArmyAvailable; //keep the number of amies yet to be placed in Reinforcement.
        int numArmyDistribution = 0; //the number of armies yet to be distributed - used only in the distribution phase.
        int armyToMove; //number of armies movable for fortify
        List<ArmyPlacement> newArmies = new List<ArmyPlacement>();
        int numDiceRolled; //used during conquering as we need to move at least this many armies.
        public MainGUI()
        {
            InitializeComponent();
            string mapfilename;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mapfilename = openFileDialog1.FileName;
                map = Map.loadMap(openFileDialog1.FileName);
            }
            else
            {
                MessageBox.Show("Good bye.");
                CleanUp();
                return;
            }
            if (map == null)
            {
                MessageBox.Show("Failed to obtain a map.");
                CleanUp();
                return;
            }

            LoginDialog login = new LoginDialog();
            if(login.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if(login.isComputer())
                {
                    player = new Computer(login.getLogin(), Color.Red, map);
                }
                else
                {
                    player = new Human(login.getLogin(), Color.Red, map);
                }
            }
            else
            {
                MessageBox.Show("Good bye.");
                CleanUp();
                return;
            }
            shared = new SharedMessage(player.nickname);

            /*
             * Connect to the server and get the game.
             * */
            try
            {
                proxy = new RiskMessageThread.ServiceReference1.RiskServerClient();
                {
                    if (MessageBox.Show("Do you want to start a new game?", "Question", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        gameid = proxy.newGame(player.nickname, Path.GetFileName(mapfilename));
                        if (gameid == 0)
                        {
                            MessageBox.Show("Failed to create a new game.");
                            CleanUp();
                            return;
                        }
                        MessageBox.Show("A new game has been created. The id is " + gameid);
                        bOwner = true;
                        if (proxy.joinGame(player.nickname, gameid) == false)
                        {
                            MessageBox.Show("Failed to join the game.");
                            CleanUp();
                            return;
                        }
                    }
                    else
                    {
                        GameIDInput gameInput = new GameIDInput();
                        if (gameInput.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            gameid = gameInput.id();
                            if (proxy.joinGame(player.nickname, gameid) == false)
                            {
                                MessageBox.Show("Failed to join the game.");
                                CleanUp();
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Good bye.");
                            CleanUp();
                            return;
                        }
                    }
                    shared = new SharedMessage(player.nickname);

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

                }
                this.Text = player.nickname + " - " + gameid;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                CleanUp();
                return;
            }
            backgroundWorker1.RunWorkerAsync();
        }

        void CleanUp()
        {
            if(proxy != null)
            {
                proxy.Close();
            }
            Load += (s, e) => Close();
        }


        /*
         * This utility function computes the number of armies to be have at the beginning of the distribution phase.
         * This function should be called only after the initialization so that all the territories are properly occupied.
         * */
        int computeDistributionArmies()
        {
            //first count the number of players in the game by looking through distinct owners in the map.
            HashSet<string> names = new HashSet<string>();
            foreach (Territory t in map.getAllTerritories())
            {
                names.Add(t.getOwner());
            }
            //int total = ((10 - names.Count) * 5 * names.Count - map.getAllTerritories().Count); //a secret formula.
            int total = 1 * names.Count; //TK - for testing purpose.
            return total / names.Count;
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

        void updateNumArmyLabel(int n)
        {
            numArmyLabel.Text = "Available armies: " + n;
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
        }

        //Depending on the current state of the game, handles what happens when a player clicks a territory
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.shared.bWaitForUser == false) return;
            MainState state = shared.message.state;

            //allow to cancel operations (Attack and Fortify) by clicking right mouse button
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (state == MainState.Attack)
                {
                    if (MessageBox.Show("Do you want to stop attacking?", "Question", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        lock (shared)
                        {
                            //by setting the attacking territory to "none", we signal the server to cancel the attack.
                            shared.message.from = "none";
                            shared.message.state = MainState.AttackDone;
                            shared.bWaitForUser = false;
                        }
                    }
                }
                else if (state == MainState.Fortify)
                {
                    if (MessageBox.Show("Do you want to skip fortification?", "Question", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        lock (shared)
                        {
                            shared.bWaitForUser = false;
                        }
                    }
                }
            }
            //gets where the user clicked
            Point p = new Point();
            p.X = e.Location.X * map.getBitmap().Width / pictureBox1.Width;
            p.Y = e.Location.Y * map.getBitmap().Height / pictureBox1.Height;
            //Initialize: select unoccupied territories to occupy
            //allows the user to occupy 4 armies initially. 
            //This wil be changed to deal with taking turns in the final product
            Territory t = clickedTerritory(p);
            if (state == MainState.Initialize)
            {
                if (t != null)
                {
                    if (t.getOwner() == "unoccupied")
                    {
                        if (MessageBox.Show("You selected " + t.getName() + ". Is this correct?",
                            "Confirmation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            lock (shared)
                            {
                                shared.message.territory_army.Add(new ArmyPlacement(t.getName(), player.nickname, 1));
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
                            "Confirmation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            numArmyDistribution--;
                            updateNumArmyLabel(numArmyDistribution);
                            lock (shared)
                            {
                                shared.message.territory_army.Add(new ArmyPlacement(t.getName(), player.nickname, 1));
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
                            "Confirmation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            int k = newArmies.Count - numArmyAvailable;
                            newArmies[k].owner = player.getName();
                            newArmies[k].territory = t.getName();
                            newArmies[k].numArmies = 1;
                            numArmyAvailable--;
                            updateNumArmyLabel(numArmyAvailable);
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
                //numArmyLabel.Text = "Available armies: ";
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
                            "Confirmation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        moveTo = t;
                        FortifyMessage move = new FortifyMessage(moveFrom.numArmies - 1, 0);

                        if (move.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            armyToMove = move.getArmiesToMove();
                        }
                        lock (shared)
                        {
                            shared.message.territory_army.Add(new ArmyPlacement(moveTo.getName(), player.nickname, armyToMove));
                            shared.message.territory_army.Add(new ArmyPlacement(moveFrom.getName(), player.nickname, -armyToMove));
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
                            "Confirmation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
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

        /*
         * This version implements a game playing against two computers.
         * */
        /*private void backgroundWorker1_AgainstComputers(object sender, DoWorkEventArgs e)
        {
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
                            clients[i].Request(resp); //update the Map at the Client side.
                            if (clients[i].player is Human)
                            {
                                Console.Out.WriteLine("Reporting progress with " + resp.state);
                                lock (shared)
                                {
                                    shared.bWaitForUser = true; //Let the Human wait for GUI response
                                }
                                backgroundWorker1.ReportProgress(0, resp); //update the GUI map
                                while (shared.bWaitForUser == true)
                                {
                                    Thread.Sleep(10);
                                }
                            }
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
                                    lock (shared)
                                    {
                                        shared.bWaitForUser = true; //Let the Human wait for GUI response
                                        shared.message = (RiskMessage) resp.Clone(); //shared message lives across threads
                                    }
                                    backgroundWorker1.ReportProgress(0, shared.message);
                                    while(shared.bWaitForUser == true)
                                    {
                                        Thread.Sleep(10);
                                    }
                                    lock(shared)
                                    {
                                        resp = (RiskMessage) shared.message.Clone(); //copy back
                                    }
                                }
                                RiskMessage resp2 = clients[i].Request(resp); //do whatever being told to do
                                RiskMessage resp3 = proxy.Request(resp2); //update the Map at the Server side
                            }
                            else
                            {
                                Thread.Sleep(300);
                            }
                        }
                    }
                }
                RiskMessage msg = new RiskMessage(MainState.Over, names[0]);
                this.backgroundWorker1.ReportProgress(100, msg);
            }
        }*/

        /*
         * This version implements a game playing against other remote players.
         * */
        private void backgroundWorker1_AgainstRemotePlayers(object sender, DoWorkEventArgs e)
        {
            if (bOwner)
            {
                MessageBox.Show("Click OK to start this game.");
                int ret = proxy.startGame(player.nickname, gameid);
                string[] names = proxy.getPlayerList(gameid);
                string namelist = "";
                foreach (string n in names)
                {
                    namelist += n + ", ";
                }
                if (ret < 0)
                {
                    MessageBox.Show("Failed to start the game...\n" + namelist);
                    CleanUp();
                    return;
                }
                else
                {
                    MessageBox.Show("Following players are in the game:\n" + namelist);
                }
            }

            RiskClient client = new RiskClient();
            client.player = this.player;
            bool bRunning = true;
            RiskMessage prev1 = null, prev2 = null; //store previous messages so that identical messages can be ignored.
            while (bRunning)
            {
                //get the fresh map from the Server
                {
                    RiskMessage outgoing = new RiskMessage(MainState.GetMap, client.name);
                    RiskMessage resp = proxy.Request(outgoing);
                    if (resp.Equals(prev1) == false)
                    {
                        RiskMessage resp2 = (RiskMessage) resp.Clone();
                        resp2.state = MainState.Update;
                        client.Request(resp2); //update the Map at the Client side.
                        if (client.player is Human)
                        {
                            Console.Out.WriteLine("Reporting progress with " + resp2.state);
                            lock (shared)
                            {
                                shared.bWaitForUser = true; //Let the Human wait for GUI response
                            }
                            backgroundWorker1.ReportProgress(0, resp2); //update the GUI map
                            while (shared.bWaitForUser == true)
                            {
                                Thread.Sleep(10);
                            }
                        }
                    }
                    prev1 = (RiskMessage) resp.Clone();
                }
                //check for what to do
                {
                    RiskMessage outgoing = new RiskMessage(MainState.Unknown, client.name);
                    RiskMessage resp = proxy.Request(outgoing); //Get what to do
                    if (resp.Equals(prev2) == false)
                    {
                        if (resp.state == MainState.Over)
                        {
                            bRunning = false;
                            break;
                        }
                        if (resp.state != MainState.Idle)
                        {
                            Console.Out.WriteLine("\t" + client.name + ": " + resp.state);
                            if (client.player is Human)
                            {
                                Console.Out.WriteLine("Reporting progress with " + resp.state);
                                lock (shared)
                                {
                                    shared.bWaitForUser = true; //Let the Human wait for GUI response
                                    shared.message = (RiskMessage)resp.Clone(); //shared message lives across threads
                                }
                                backgroundWorker1.ReportProgress(0, shared.message);
                                while (shared.bWaitForUser == true)
                                {
                                    Thread.Sleep(10);
                                }
                                lock (shared)
                                {
                                    resp = (RiskMessage)shared.message.Clone(); //copy back
                                }
                            }
                            RiskMessage resp2 = client.Request(resp); //do whatever being told to do
                            RiskMessage resp3 = proxy.Request(resp2); //update the Map at the Server side
                            proxy.Request(new RiskMessage(MainState.Acknowledge, client.name)); //end this turn. possibly move to the next player
                        }
                    }
                    prev2 = (RiskMessage) resp.Clone();
                    Thread.Sleep(300);
                }
            }
            RiskMessage msg = new RiskMessage(MainState.Over, client.name);
            this.backgroundWorker1.ReportProgress(100, msg);
        }

        /*
         * Solicit action from the user according to the current state of the game.
         * Set shared.bWaitForUser to false once the action is done.
         * If it requires mouse action, do not set shared.bWaitForUser to false yet. 
         * Wait until the mouse action is done.
         * */
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RiskMessage msg = (RiskMessage)e.UserState;
            if (msg.state == MainState.Update)
            {
                updateTerritoryLabels();
                lock (shared)
                {
                    shared.bWaitForUser = false;
                }
            }
            else
            {
                if (msg.state == MainState.Initialize)
                {
                    MessageBox.Show(player.nickname + ". You are in " + msg.state + " mode. Click an unoccupied territory to occupy");
                }
                else if (msg.state == MainState.Distribute)
                {
                    if (numArmyDistribution == 0)
                    {
                        numArmyDistribution = computeDistributionArmies();
                    }
                    MessageBox.Show(player.nickname + ". You have " + numArmyDistribution + " armies to distribute. Click your territory to add an army");
                    updateNumArmyLabel(numArmyDistribution);
                }
                else if (msg.state == MainState.TradeCard)
                {
                    List<ReinforcementCard> cards = null;
                    List<ReinforcementCard> hands = player.getCards();
                    if (hands.Count >= 3)
                    {
                        TradeCardGUI trade = new TradeCardGUI(hands);
                        trade.Text = "Trade card - " + player.nickname;
                        if (trade.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            cards = trade.getReinforcementCards();
                            if (cards.Count == 3) //since TradeCardGUI does not perform sufficient error check, I have to do it here...
                            {
                                player.removeCard(cards[0]);
                                player.removeCard(cards[1]);
                                player.removeCard(cards[2]);
                            }
                        }
                    }
                    lock (shared)
                    {
                        if (cards != null && cards.Count == 3) //cards have been selected.
                        {
                            shared.message.cardIds = new int[3];
                            for (int i = 0; i < 3; ++i)
                            {
                                shared.message.cardIds[i] = cards[i].CardId;
                            }
                        }
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
                    MessageBox.Show(player.nickname + ". You are in " + msg.state + " mode.\nYou collected " + numArmyAvailable + " armies.");
                    updateNumArmyLabel(numArmyAvailable);
                    lock (shared)
                    {
                        shared.bWaitForUser = false;
                    }
                }
                else if (msg.state == MainState.Reinforce)
                {
                    MessageBox.Show(player.nickname + ". You are in " + msg.state + " mode. Select your territory to reinforce.");
                }
                else if (msg.state == MainState.Attack)
                {
                    if (MessageBox.Show(player.nickname + ". Do you want to attack?", "Attack", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
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
                        MessageBox.Show(player.nickname + ". You are in " + msg.state + " mode. Drag mouse to specify an attack.");
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
                        Console.Out.WriteLine("Null territories for attacking. Cannot continue");
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
                        string result = player.nickname + ". You rolled ";
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
                    MessageBox.Show(player.nickname + ". Attacker lost " + -msg.territory_army[0].numArmies + " and Defender lost " + -msg.territory_army[1].numArmies);
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
                        armyToMove = numDiceRolled;
                        if (MessageBox.Show("Would you like to move more than the minimum amount of armies?",
                               "Confirmation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            FortifyMessage move = new FortifyMessage(attackFrom.numArmies - 1, numDiceRolled);
                            if (move.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                armyToMove = move.getArmiesToMove();
                            }
                        }
                        shared.message.territory_army[1].numArmies = armyToMove; //this is the minimum amount we have to move.
                        shared.message.territory_army[0].numArmies -= armyToMove;

                        MessageBox.Show(player.nickname + ". You won! Moving army from " + shared.message.territory_army[0].territory + " to " + shared.message.territory_army[1].territory);
                        shared.bWaitForUser = false;
                    }
                }
                else if (msg.state == MainState.ReinforcementCard)
                {
                    string s = "";
                    foreach(ReinforcementCard card in msg.card)
                    {
                        player.addCard(card);
                        s += card.TerritoryName + " " + card.UnitType + "\n";
                    }
                    if(msg.card.Count > 0)
                    {
                        MessageBox.Show(player.nickname + ". You received:\n" + s);
                    }
                    else
                    {
                        MessageBox.Show(player.nickname + ". You did not get any reinforcement card. Why? Darn it.");
                    }
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
                        MessageBox.Show(player.nickname + ". You are in " + msg.state + " mode. Drag mouse to specify fortification.\nUse right mouse to skip fortification.");
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
                    MessageBox.Show(player.nickname + ". You are in " + msg.state + " mode.");
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
