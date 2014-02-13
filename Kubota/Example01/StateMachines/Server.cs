using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SUOnlineRisk
{
    class Server
    {
        public List<Client> clients;
        List<Message> incoming;
        List<Message> outgoing;
        public List<bool> acks;
        bool bStop;
        public Server()
        {
            clients = new List<Client>();
            incoming = new List<Message>();
            outgoing = new List<Message>();
            acks = new List<bool>();
            bStop = false;
        }
        public bool addClient(Client client)
        {
            if(clients.Count < 6)
            {
                clients.Add(client);
                incoming.Add(new Message(MainState.Start));
                outgoing.Add(new Message(MainState.Start));
                acks.Add(false);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Accept(Client client, Message message)
        {
            lock (this)
            {
                int k = clients.IndexOf(client);
                if (k >= 0 && k < clients.Count)
                {
                    acks[k] = true;
                    incoming[k] = message;
                }
            }
        }
        public void Stop()
        {
            lock(this)
            {
                bStop = true;
            }
        }
        public void Run()
        {
            while (true)
            {
                int overCount = 0;
                for(int i=0; i<clients.Count; ++i)
                {
                    if(bStop)
                    {
                        outgoing[i].state = MainState.Over;
                    }
                    if (outgoing[i].state == MainState.Start)
                    {
                        outgoing[i].state = MainState.Initialize;
                    }
                    else if (outgoing[i].state == MainState.Initialize)
                    {
                        outgoing[i].state = MainState.Distribute;
                    }
                    else if (outgoing[i].state == MainState.Distribute)
                    {
                        outgoing[i].state = MainState.Collect;
                    }
                    else if (outgoing[i].state == MainState.Collect)
                    {
                        outgoing[i].state = MainState.Attack;
                    }
                    else if (outgoing[i].state == MainState.Attack)
                    {
                        outgoing[i].state = MainState.Fortify;
                    }
                    else if (outgoing[i].state == MainState.Fortify)
                    {
                        outgoing[i].state = MainState.Collect;
                    }
                    else if (outgoing[i].state == MainState.Over)
                    {
                        outgoing[i].state = MainState.Over;
                        overCount++;
                    }
                    acks[i] = false;
                    clients[i].Request(outgoing[i]);
                    while(clients[i].bRunning && acks[i] == false)
                    {
                        Thread.Sleep(100);
                    }
                }
                if(overCount == clients.Count)
                { 
                    break;
                }
            }
        }
    }
}
