using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server
{
    class Client
    {
        public Socket socket { get; private set; }
        public Task clientTask;
        public TanksLib.Tank tankClient { get; set; }
        public bool needToRef = false;
        public Client(Socket socketg)
        {
            socket = socketg;
            clientTask = new Task(ConnectedClientSupport);
            clientTask.Start();
        }
        public void Disconnect()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        public void Send(string msg)
        {
            socket.Send(Encoding.Unicode.GetBytes(msg));
        }
        public override string ToString()
        {
            return $"Client with ip {socket.RemoteEndPoint} is " + (socket.Connected ? "connected" : "not connected");
        }
        private void ConnectedClientSupport()
        {
            while (socket.Connected)
            {
               string data = this.GetString();
               TanksLib.Tank tank = JsonSerializer.Deserialize<TanksLib.Tank>(data);
               tankClient = tank;
                needToRef = true;
                Console.WriteLine(data);
            }
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
