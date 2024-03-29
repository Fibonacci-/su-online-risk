﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace SUOnlineRisk
{
    class Player
    {
        string name;
        Message request;
        Message acknowledge;
        bool bPending;
        public bool bRunning
        {
            get;
            private set;
        }
        public Client client
        {
            get;
            set;
        }
        public Player(string name)
        {
            this.name = name;
            client = null;
            bPending = false;
            bRunning = false;
        }
        public void Request(Message message)
        {
            lock (this)
            {
                this.request = message;
                this.bPending = true;
            }
        }
        public void Run()
        {
            if(client == null)
            {
                Console.Error.WriteLine("There is no client.");
                return;
            }
            bRunning = true;
            while (true)
            {
                while(bPending==false)
                {
                    Thread.Sleep(300);
                }
                lock (this)
                {
                    //it is your turn. Make a whatever move.
                    bPending = false;
                    Console.Out.WriteLine(name + ">> doing work at state = " + request.state);
                    acknowledge = request;
                    //let the client knows what you did.
                    client.Accept(acknowledge);
                    if (acknowledge.state == MainState.Over)
                    {
                        break;
                    }
                }
            }
            bRunning = false;
        }
    }
}
