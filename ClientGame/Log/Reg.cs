using ClientGame.Log;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace LoginPass
{
    public partial class Reg : Form
    {
        Random random = new Random();
        Socket socket;
        public Reg(Socket socketcl)
        {
            InitializeComponent();
            socket = socketcl;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void reg_Click(object sender, EventArgs e)
        {
            if(textPass.Text == textPassConf.Text && textLog.Text.Length >= 3 && textEm.Text.Length >= 3 && textPass.Text.Length >= 3)
            {
                if(RegSd())
                {
                    MessageBox.Show("Successfull auth!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Problems with registration.");
                }
            }
            else if(textPass.Text != textPassConf.Text)
            {
                MessageBox.Show("Incorrect passwords!");
            }
            else
            {
                MessageBox.Show("Check your data!");
            }
        }
        private bool RegSd()
        {
            bool res = false;
            socket.Send(Encoding.Unicode.GetBytes($"reg {textLog.Text} {textPass.Text} {textEm.Text}"));
            if (Client.Client.GetString(socket).StartsWith("code"))
            {
                socket.Send(Encoding.Unicode.GetBytes(InputBox.ShowInputDialog("Code input")));
                if(Client.Client.GetString(socket).StartsWith("sucr"))
                {
                    res = true;
                }
            }
            return res;
        }
        
    }
}
