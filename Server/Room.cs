using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TanksLib;
using Timer = System.Timers.Timer;

namespace Server
{
    class Room
    {
        public List<Client> clients;
        public Room()
        {
            clients = new List<Client>();
            Timer timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;
            timer.Start();
            Task refresher = new Task(Refresh);
            refresher.Start();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RefreshActions();
        }
        public void Refresh()
        {
            while (!Environment.HasShutdownStarted)
            {
                try
                {
                    if (clients.Where(x => x.needToRef).Count() > 0)
                    {
                        Thread.Sleep(10);
                        RefreshActions();
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }

        }
        private void SendMsgToAll(string msg)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                try
                {
                    clients[i].Send(msg);
                }
                catch { }
            }
        }
        public void RefreshActions()
        {
            List<Tank> tanks = new List<Tank>();
            clients.ForEach(x => tanks.Add(x.socket.Connected && x.tankClient.hp > 0 ? x.tankClient : null));
            SendMsgToAll(JsonSerializer.Serialize<List<Tank>>(tanks));
            //Console.WriteLine($"Data sended to users, {tanks.Where(x => x != null).Count()} of tanks.");
            clients.ForEach(x => x.needToRef = false);
        }
        public void DisconnectAll()
        {
            SendMsgToAll("→disconnect@");
            clients.ForEach(x => x.Disconnect());
            clients.Clear();
        }
        public override string ToString()
        {
            return $"Room was visited with {clients.Count} players.";
        }
    }
}
