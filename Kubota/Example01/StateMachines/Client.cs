using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace StateMachines
{
    class Client
    {
        Message acknowledge;
        Message request;
        Server server;
        bool bAck;
        bool bPending;
        public bool bRunning
        {
            get;
            private set;
        }
        public Player player
        {
            get;
            set;
        }
        public Client(Server server)
        {
            this.server = server;
            player = null;
            bAck = false;
            bPending = false;
        }
        //from the server
        public void Request(Message message)
        {
            lock (this)
            {
                this.request = message;
                bPending = true;
            }
        }
        //from the player
        public void Accept(Message message)
        {
            lock (this)
            {
                bAck = true;
                acknowledge = message;
            }
        }
        public void Run()
        {
            if (player == null)
            {
                Console.Error.WriteLine("There is no player.");
                return;
            }
            if (server == null)
            {
                Console.Error.WriteLine("There is no server.");
                return;
            }
            bRunning = true;
            while (true)
            {
                while(bPending == false)
                {
                    Thread.Sleep(100);
                }
                bPending = false;
                player.Request(request);
                while(bAck == false)
                {
                    Thread.Sleep(100);
                }
                bAck = false;
                server.Accept(this, acknowledge);
                if(player.bRunning == false)
                {
                    break;
                }
            }
            bRunning = false;
        }
    }
}
