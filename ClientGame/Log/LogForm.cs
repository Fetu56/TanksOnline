using ClientGame.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace LoginPass
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        Socket socketCl;
        public bool Logged = false;
        public Form1(Socket socket)
        {
            InitializeComponent();
            socketCl = socket;
        }

        private void log_Click(object sender, EventArgs e)
        {
            socketCl.Send(Encoding.Unicode.GetBytes($"log {textEm.Text} {Cash.ComputeSha256Hash(textPass.Text)}"));  
            if (Client.Client.GetString(socketCl).StartsWith("suc"))
            {
                MessageBox.Show("Welcome!", "Success auth.", MessageBoxButtons.OK);
                Logged = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Try again!", "Failed auth.", MessageBoxButtons.OK);
            }
        }

       

        private void reg_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            new Reg(socketCl).ShowDialog();
            this.Visible = true;
        }

        private void resetPass_Click(object sender, EventArgs e)
        {
            //change
            //string log = InputBox.ShowInputDialog("Login input");
            //string email = GetEmailByLog(log);
            //if(email != null)
            //{
            //    string code = random.Next(100, 999).ToString();
            //    EmailSend.Send(email, code, log);
            //    if (code == InputBox.ShowInputDialog("Input code"))
            //    {
            //        string pass = InputBox.ShowInputDialog("New pass");
            //        if(pass == InputBox.ShowInputDialog("Comfirm pass"))
            //        {
            //            List<string> data = File.ReadAllLines("data.txt").ToList();
            //            data[data.FindIndex(x => x.Split(' ')[0] == log)] = log + " " + Cash.ComputeSha256Hash(pass) + " " + email;
            //            File.WriteAllLines("data.txt", data.ToArray());
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Неверный код!");
            //    }
            //}

        }
    }
    
}
