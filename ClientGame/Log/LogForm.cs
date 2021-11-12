using ClientGame.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginPass
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        public bool Logged = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void log_Click(object sender, EventArgs e)
        {
            if(CheckLogIn(textEm.Text, textPass.Text))
            {
                MessageBox.Show("Welcome!", "Success auth.", MessageBoxButtons.OK);
                Logged = true;
            }
            else
            {
                MessageBox.Show("Try again!", "Failed auth.", MessageBoxButtons.OK);
            }
        }

        private bool CheckLogIn(string log, string pas)
        {
            bool res = false;
            if(File.Exists("data.txt"))
            {
                List<string> data = File.ReadAllLines("data.txt").ToList();

                res = data.Where(x => x.Split(' ')[0] == log).Where(x => x.Split(' ')[1] == Cash.ComputeSha256Hash(pas)).Count() > 0 ? true : false;

                GC.Collect();
            }
            return res;
        }
        private string GetEmailByLog(string log)
        {
            if (File.Exists("data.txt"))
            {
                List<string> data = File.ReadAllLines("data.txt").ToList();

                return data.Find(x => x.Split(' ')[0] == log).Split(' ')[2];

                GC.Collect();
            }
            return null;
        }

        private void reg_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            new Reg().ShowDialog();
            this.Visible = true;
        }

        private void resetPass_Click(object sender, EventArgs e)
        {
            //reset pass
            string log = InputBox.ShowInputDialog("Login input");
            string email = GetEmailByLog(log);
            if(email != null)
            {
                string code = random.Next(100, 999).ToString();
                EmailSend.Send(email, code, log);
                if (code == InputBox.ShowInputDialog("Input code"))
                {
                    string pass = InputBox.ShowInputDialog("New pass");
                    if(pass == InputBox.ShowInputDialog("Comfirm pass"))
                    {
                        List<string> data = File.ReadAllLines("data.txt").ToList();
                        data[data.FindIndex(x => x.Split(' ')[0] == log)] = log + " " + Cash.ComputeSha256Hash(pass) + " " + email;
                        File.WriteAllLines("data.txt", data.ToArray());
                    }
                }
                else
                {
                    MessageBox.Show("Неверный код!");
                }
            }

        }
    }
    
}
