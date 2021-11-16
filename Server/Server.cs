using Server.LogReg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TanksLib;


namespace Server
{
    class Server
    {
        Socket socket;
        IPEndPoint iPEndPoint;
        List<Room> rooms;
        Task senderMsg;
        bool work = true;
        public Server()
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            rooms = new List<Room>();

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

                while (work)
                {
                    Connected();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            rooms.ForEach(x => x.DisconnectAll());

        }
        private void Connected()
        {
            Socket socketClient = socket.Accept();
            Task task = new Task(() => ConnectUser(socketClient));
            task.Start();
        }
        private void ConnectUser(Socket socket)
        {
            bool joined = false;
            AuthUser(socket);
                for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].clients.Where(x => x.tankClient != null && x.tankClient.hp > 0 && x.socket.Connected).Count() < 4)
                {
                    rooms[i].clients.Add(new Client(socket));
                    joined = true;
                    Console.WriteLine($"Registrated user join room #{i}. Info: ip - {socket.RemoteEndPoint}, protocol {socket.ProtocolType}");
                    socket.Send(Encoding.Unicode.GetBytes(rooms[i].clients.Count.ToString() + ','));
                    return;
                }
            }
            if (!joined)
            {
                Room room = new Room();
                room.clients.Add(new Client(socket));
                Console.WriteLine($"Registrated user join NEW room #{rooms.Count}. Info: ip - {socket.RemoteEndPoint}, protocol {socket.ProtocolType}");
                socket.Send(Encoding.Unicode.GetBytes(room.clients.Count.ToString() + ','));
                rooms.Add(room);
            }
        }

        private void AuthUser(Socket socket)
        {
            while (true)
            {
                string data = GetString(socket);
                if (data.StartsWith("log"))
                {
                    if(File.Exists("data.txt"))
                    {
                        if (CheckData.CheckLogIn(data.Split()[1], data.Split()[2]))
                        {
                            socket.Send(Encoding.Unicode.GetBytes("suc"));
                            break;
                        }
                        else
                        {
                            socket.Send(Encoding.Unicode.GetBytes("inc"));
                        }
                    }
                }
                else if (data.StartsWith("reg"))
                {
                    CheckData.RegIn(data.Split()[1], data.Split()[2], data.Split()[3], socket);
                }
                else if (data.StartsWith("reset"))
                {
                    CheckData.CheckReset(data.Split()[1], socket);
                }
            }
        }



        private void msgSender()
        {
            string msg;
            while (true)
            {
                try
                {
                    Console.WriteLine("Выберите действие:\n[1] - отключение\n[0] - выход:");
                    msg = Console.ReadLine();
                    Option option = Option.nullOp;
                    switch (msg)
                    {
                        case "1":
                            option = Option.disconnect;
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
                        if (rooms.Count > 0)
                        {
                            Console.WriteLine("Выберите комнату, к которой применить действие. Для выбора всех введите \"!\".");
                            for (int i = 0; i < rooms.Count; i++)
                            {
                                Console.WriteLine(i + ". " + rooms[i]);
                            }
                            string get = Console.ReadLine();
                            switch (option)
                            {
                                case Option.disconnect:
                                    if (get.StartsWith("!"))
                                    {
                                        rooms.ForEach(x => x.DisconnectAll());
                                    }
                                    else
                                    {
                                        int id = int.Parse(get);
                                        rooms[id].DisconnectAll();
                                    }
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