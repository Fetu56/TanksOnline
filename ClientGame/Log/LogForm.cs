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
            socketCl.Send(Encoding.Unicode.GetBytes(InputBox.ShowInputDialog("Code input")));
            this.Visible = false;
            new Reg(socketCl).ShowDialog();
            this.Visible = true;
        }

        private void resetPass_Click(object sender, EventArgs e)
        {
            string log = InputBox.ShowInputDialog("Login input");
            socketCl.Send(Encoding.Unicode.GetBytes($"reset {log}"));
            if(Client.Client.GetString(socketCl).StartsWith("code"))
            {
                socketCl.Send(Encoding.Unicode.GetBytes(InputBox.ShowInputDialog("Code input")));
                if (Client.Client.GetString(socketCl).StartsWith("newpass"))
                {
                    socketCl.Send(Encoding.Unicode.GetBytes(Cash.ComputeSha256Hash(InputBox.ShowInputDialog("New password"))));
                }
                else
                {
                    MessageBox.Show("Error with code!");
                }
            }
            else
            {
                MessageBox.Show("Error!");
            }

        }
    }
    
}
