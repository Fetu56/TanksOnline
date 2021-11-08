using System;
using System.Collections.Generic;
using System.Drawing;
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
                    if (clients.Where(x => x!= null && x.needToRef).Count() > 0)
                    {
                        Thread.Sleep(10);
                        clients.ForEach(x => x.tankClient.bulletref -= 20) ;
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
            CheckShoots();
            SendMsgToAll(JsonSerializer.Serialize<List<Tank>>(tanks));
            //Console.WriteLine($"Data sended to users, {tanks.Where(x => x != null).Count()} of tanks.");
            clients.ForEach(x => { x.needToRef = false; x.shoot = false; });
        }
        private void CheckShoots()
        {
            clients.Where(x => x.shoot).ToList().ForEach(Shoot);
        }
        private void Shoot(Client client)
        {
            Point point = new Point();

            switch (client.tankClient.rotate)
            {
                case Rotate.DOWN:
                    point = new Point(0, 24);
                    break;
                case Rotate.UP:
                    point = new Point(0, -24);
                    break;
                case Rotate.LEFT:
                    point = new Point(-24, 0);
                    break;
                case Rotate.RIGHT:
                    point = new Point(24, 0);
                    break;
            }
            Point shootCords = new Point(client.tankClient.cords.X, client.tankClient.cords.Y);
            List<Tank> tanks = new List<Tank>();
            clients.ForEach(x => tanks.Add(x.socket.Connected && x.tankClient.hp > 0 && x != client ? x.tankClient : null));
            for(int i = 0; i < 200 && shootCords.X > 0 && shootCords.Y > 0 && shootCords.X < 500 && shootCords.Y < 500; i++)
            {
                Tank tank = Tank.CheckCollision(tanks, shootCords, 50, true);
                if(tank != null)
                {
                    try
                    {
                        tanks.Where(x => x != null && x.id == tank.id).First().hp -= 25;
                    }
                    catch(Exception) { }
                    break;
                }
                shootCords = new Point(shootCords.X + point.X, shootCords.Y + point.Y);
            }
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
