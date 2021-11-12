using ClientGame.Log;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginPass
{
    public partial class Reg : Form
    {
        Random random = new Random();
        public Reg()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void reg_Click(object sender, EventArgs e)
        {
            if(textPass.Text == textPassConf.Text && textLog.Text.Length >= 3 && textEm.Text.Length >= 3 && textPass.Text.Length >= 3)
            {
                if(RegIn(textLog.Text, textPass.Text))
                {
                    MessageBox.Show("Successfull auth!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Problems with registration.");
                }
            }
            else
            {
                MessageBox.Show("Incorrect passwords!");
            }
        }
        private bool RegIn(string log, string pas)
        {
            bool res = false;
            if(File.Exists("data.txt"))
            {
                if (File.ReadAllLines("data.txt").ToList().Where(x => x.Split(' ')[0] == log).Count() == 0)
                {
                    try
                    {
                        string code = random.Next(100, 999).ToString();
                        EmailSend.Send(textEm.Text, code, textLog.Text);
                        if (code == InputBox.ShowInputDialog("Input code"))
                        {
                            File.AppendAllText("data.txt", $"{log} {Cash.ComputeSha256Hash(pas)} {textEm.Text}\n");
                            res = true;
                        }

                        
                    }catch { }
                }
            }
            else
            {
                try
                {
                    string code = random.Next(100, 999).ToString();
                    EmailSend.Send(textEm.Text, code, textLog.Text);
                    if (code == InputBox.ShowInputDialog("Input code"))
                    {
                        File.Create("data.txt").Close();
                        File.AppendAllText("data.txt", $"{log} {Cash.ComputeSha256Hash(pas)} {textEm.Text}\n");
                        res = true;
                    }
                }
                catch { }

                
            }
            GC.Collect();
            return res;
        }
    }
}
