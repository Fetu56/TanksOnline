using ClientGame.Log;
using LoginPass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Server.LogReg
{
    public static class CheckData
    {
        private static Random random = new Random();
        public static bool CheckLogIn(string log, string pas)
        {
            bool res = false;
            if (File.Exists("data.txt"))
            {
                List<string> data = File.ReadAllLines("data.txt").ToList();

                res = data.Where(x => x.Split(' ')[0] == log).Where(x => x.Split(' ')[1] == pas).Count() > 0 ? true : false;

                GC.Collect();
            }
            return res;
        }
        public static bool RegIn(string log, string pas, string email, Socket socket)
        {
            bool res = false;
            if (File.Exists("data.txt"))
            {
                if (File.ReadAllLines("data.txt").ToList().Where(x => x.Split(' ')[0] == log).Count() == 0)
                {
                    try
                    {
                        string code = random.Next(100, 999).ToString();
                        EmailSend.Send(email, code, log);
                        socket.Send(Encoding.Unicode.GetBytes("code"));
                        if (code == Client.GetString(socket))
                        {
                            File.AppendAllText("data.txt", $"{log} {Cash.ComputeSha256Hash(pas)} {email}\n");
                            res = true;

                            socket.Send(Encoding.Unicode.GetBytes("sucr"));
                        }
                        else
                        {
                            socket.Send(Encoding.Unicode.GetBytes("inc"));
                        }

                    }
                    catch { }
                }
            }
            else
            {
                try
                {
                    string code = random.Next(100, 999).ToString();
                    EmailSend.Send(email, code, log);
                    socket.Send(Encoding.UTF8.GetBytes("code"));
                    if (code == Client.GetString(socket))
                    {
                        File.Create("data.txt").Close();
                        File.AppendAllText("data.txt", $"{log} {Cash.ComputeSha256Hash(pas)} {email}\n");
                        res = true;
                        socket.Send(Encoding.Unicode.GetBytes("sucr"));
                    }
                    else
                    {
                        socket.Send(Encoding.Unicode.GetBytes("inc"));
                    }
                }
                catch { }


            }
            GC.Collect();
            return res;
        }
        public static string GetEmailByLog(string log)
        {
            if (File.Exists("data.txt"))
            {
                List<string> data = File.ReadAllLines("data.txt").ToList();

                GC.Collect();
                return data.Find(x => x.Split(' ')[0] == log).Split(' ')[2];
            }
            return null;
        }
        public static void ChangePass(string newPass, string email, string log)
        {
            if (File.Exists("data.txt"))
            {
                List<string> data = File.ReadAllLines("data.txt").ToList();
                data[data.FindIndex(x => x.Split(' ')[0] == log)] = log + " " + Cash.ComputeSha256Hash(newPass) + " " + email;
                File.WriteAllLines("data.txt", data.ToArray());
            }
        }
    }
}
