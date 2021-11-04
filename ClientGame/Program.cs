using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace ClientGame
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            if (CheckInternetConnection())
            {
                try
                {
                    if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\tanksOnline"))
                    {
                        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\tanksOnline");
                    }
                    if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\tanksOnline\tank.png"))
                    {
                        new WebClient().DownloadFile("https://iili.io/5cOMiB.png", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\tanksOnline\\tank.png");
                    }
                    if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\tanksOnline\brick.png"))
                    {
                        new WebClient().DownloadFile("https://iili.io/5cexdF.png", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\tanksOnline\\brick.png");
                    }
                }
                catch { }


                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
        static bool CheckInternetConnection()
        {
            return (new Ping().Send("google.com", 1000, new byte[32], new PingOptions()).Status == IPStatus.Success);
        }
    }
}
