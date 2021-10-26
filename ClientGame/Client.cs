using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        Socket socket;
        IPEndPoint iPEndPoint;
        public Task process { get; private set; }
        public Client()
        {
           
            iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start()
        {
            process = new Task(ClientStartThread);
            process.Start();
        }

        private void ClientStartThread()
        {
            try
            {
                socket.Connect(iPEndPoint);
                while (socket.Connected)
                {
                   
                }
            }
            catch (Exception ex)
            {

            }
            Console.Read();
        }
        public void SendTank(TanksLib.Tank tank)
        {
            socket.Send(Encoding.Unicode.GetBytes(JsonSerializer.Serialize<TanksLib.Tank>(tank)));
        }
        public string GetString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int bytes = 0;
            byte[] data = new byte[256];
            do
            {
                bytes = socket.Receive(data);
                stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (socket.Available > 0);
            return stringBuilder.ToString();
        }

    }
}   