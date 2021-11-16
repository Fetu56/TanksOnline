using LoginPass;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public class Client
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
            socket.Connect(iPEndPoint);
            Form1 log = new Form1(socket);
            log.ShowDialog();
            if (!log.Logged)
            {
                MessageBox.Show("Cannot login to the server");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
            process.Start();
        }

        private void ClientStartThread()
        {
            try
            {
                while (socket.Connected)
                {
                   
                }
            }
            catch (Exception)
            {}
            Console.Read();
        }
        public void SendTank(TanksLib.Tank tank)
        {
            socket.Send(Encoding.Unicode.GetBytes(JsonSerializer.Serialize<TanksLib.Tank>(tank)));
        }
        public void Send(string str)
        {
            socket.Send(Encoding.Unicode.GetBytes(str));
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
        public static string GetString(Socket sc)
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