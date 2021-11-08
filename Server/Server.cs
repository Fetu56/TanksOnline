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
            bool joined = false;
            for(int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].clients.Where(x => x.tankClient != null  && x.tankClient.hp > 0 && x.socket.Connected).Count() < 4)
                {
                    rooms[i].clients.Add(new Client(socketClient));
                    joined = true;
                    Console.WriteLine($"Registrated user join room #{i}. Info: ip - {socketClient.RemoteEndPoint}, protocol {socketClient.ProtocolType}");
                    socketClient.Send(Encoding.Unicode.GetBytes(rooms[i].clients.Count.ToString() + ','));
                    return;
                }
            }
            if(!joined)
            {
                Room room = new Room();
                room.clients.Add(new Client(socketClient));
                Console.WriteLine($"Registrated user join NEW room #{rooms.Count}. Info: ip - {socketClient.RemoteEndPoint}, protocol {socketClient.ProtocolType}");
                socketClient.Send(Encoding.Unicode.GetBytes(room.clients.Count.ToString() + ','));
                rooms.Add(room);
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