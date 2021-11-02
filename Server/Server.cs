using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TanksLib;
using Timer = System.Timers.Timer;

namespace Server
{
    class Server
    {
        Socket socket;
        IPEndPoint iPEndPoint;
        List<Client> clients;
        Task senderMsg;
        bool work = true;
        public Server()
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clients = new List<Client>();
            Timer timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RefreshActions();
        }

        public Server(string ip, int port)
        {
            try
            {
                iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            }
            catch (Exception)
            {
                iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            }

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start()
        {
            Console.WriteLine($"Start server [{iPEndPoint.Address}]...");
            try
            {
                socket.Bind(iPEndPoint);
                socket.Listen(10);
                senderMsg = new Task(msgSender);
                senderMsg.Start();
                Task refresher = new Task(Refresh);
                refresher.Start();
                while (work)
                {
                    Connected();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            DisconnectAll();

        }
        private void Connected()
        {
            Socket socketClient = socket.Accept();
            if(clients.Where(x => x.tankClient.hp > 0 && x.socket.Connected).Count() < 4)
            {
                clients.Add(new Client(socketClient));
                Console.WriteLine($"Registrated user join. Info: ip - {socketClient.RemoteEndPoint}, protocol {socketClient.ProtocolType}");
                socketClient.Send(Encoding.Unicode.GetBytes(clients.Count.ToString() + ','));
            }
            

        }

        public void Refresh()
        {
            while(!Environment.HasShutdownStarted)
            {
                try
                {
                    if (clients.Where(x => x.needToRef).Count() > 0)
                    {
                        Thread.Sleep(10);
                        RefreshActions();
                    }
                }
                catch(Exception ex) { Console.WriteLine(ex); }
            }
            
        }
        public void RefreshActions()
        {
            List<Tank> tanks = new List<Tank>();
            clients.ForEach(x => tanks.Add(x.socket.Connected && x.tankClient.hp > 0 ? x.tankClient : null));
            SendMsgToAll(JsonSerializer.Serialize<List<Tank>>(tanks));
            Console.WriteLine($"Data sended to users, {tanks.Where(x => x != null).Count()} of tanks.");
            clients.ForEach(x => x.needToRef = false);
        }
        public void DisconnectAll()
        {
            clients.ForEach(x => x.Disconnect());
            clients.Clear();
        }
        private void msgSender()
        {
            string msg;
            while (true)
            {
                try
                {
                    Console.WriteLine("Выберите действие:\n[1] - отключение\n[2] - обновить\n[0] - выход:");
                    msg = Console.ReadLine();
                    Option option = Option.nullOp;
                    switch (msg)
                    {
                        case "1":
                            option = Option.disconnect;
                            break;
                        case "2":
                            option = Option.startProg;
                            break;
                        case "0":
                            option = Option.exit;
                            break;
                        default:
                            option = Option.nullOp;
                            break;
                    }
                    if (option == Option.exit)
                    {
                        work = false;
                        Environment.Exit(0);
                        break;
                    }
                    else if (option != Option.nullOp)
                    {
                        if (clients.Count > 0)
                        {
                            Console.WriteLine("Выберите клиента, к которому применить действие. Для выбора всех введите \"!\".");
                            for (int i = 0; i < clients.Count; i++)
                            {
                                Console.WriteLine(i + ". " + clients[i]);
                            }
                            string get = Console.ReadLine();
                            switch (option)
                            {
                                case Option.disconnect:
                                    if (get.StartsWith("!"))
                                    {
                                        SendMsgToAll("→disconnect@");
                                        DisconnectAll();
                                    }
                                    else
                                    {
                                        int id = int.Parse(get);
                                        clients[id].Send("→disconnect@");
                                        clients[id].Disconnect();
                                    }
                                    break;
                                case Option.startProg:
                                    RefreshActions();
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Недостаточное количество пользователей.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private void SendMsgToAll(string msg)
        {
            for(int i = 0; i < clients.Count; i++)
            {
                try
                {
                    clients[i].Send(msg);
                }
                catch { }
            }
        }
        private string GetString(Socket sc)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int bytes = 0;
            byte[] data = new byte[256];
            do
            {
                bytes = sc.Receive(data);
                stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (sc.Available > 0);
            return stringBuilder.ToString();
        }

    }
}