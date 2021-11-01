using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TanksLib;

namespace ClientGame
{
    public partial class Form1 : Form
    {
        int id;
        Client.Client client;
        public List<Tank> tanks { get; private set; }
        const int TankSize = 50;
        Image tankimg = Image.FromFile("tank.png");
        Task game;
        public Form1()
        {
            tanks = new List<Tank>();
            client = new Client.Client();
            client.Start();
            InitializeComponent();
            while(id == 0 || id == null)
            {
                try
                {
                    id = int.Parse(client.GetString().Split(',')[0]);
                }
                catch { }
                
            }
            
            client.SendTank(new Tank() { name = "fety-"+id.ToString(), cords = new Point(100, 100), speed = 5, id = id});
            this.KeyDown += FormKeyDown;
            game = new Task(GameManager);
            game.Start();
        }

        

        private void GameManager()
        {

            while(!client.process.IsCompleted)
            {
                try
                {
                    UpdateData();
                }
                catch { }
            }
        }
        private void UpdateData()
        {
            string json = client.GetString();
            tanks.Clear();
            try
            {
                tanks = JsonSerializer.Deserialize<List<Tank>>(json);
                for (int i = 0; i < tanks.Count; i++)
                {
                    if (gameField.Controls.Count <= i)
                    {
                        Image img = new Bitmap(tankimg);

                        switch (tanks[i].rotate)
                        {
                            case Rotate.DOWN:
                                img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                break;
                            case Rotate.LEFT:
                                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                break;
                            case Rotate.RIGHT:
                                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                break;
                        }
                        this.gameField.Invoke((MethodInvoker)delegate {

                            gameField.Controls.Add(new PictureBox() { BackgroundImage = img, BackgroundImageLayout = ImageLayout.Zoom, Location = tanks[i].cords, Size = new Size(TankSize, TankSize) });
                            gameField.Controls.OfType<PictureBox>().ToList().ForEach(x => x.Paint += Tank_Paint);

                        });




                    }
                    else
                    {
                        gameField.Controls[i].Location = new Point(tanks[i].cords.X, tanks[i].cords.Y);
                        Image img = new Bitmap(tankimg);
                        switch (tanks[i].rotate)
                        {
                            case Rotate.DOWN:
                                img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                break;
                            case Rotate.LEFT:
                                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                break;
                            case Rotate.RIGHT:
                                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                break;
                        }
                        gameField.Controls[i].BackgroundImage = img;
                    }
                }
            }
            catch { }
            gameField.Refresh();





                //GC.Collect();
        }
        private void FormKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (tanks != null && tanks.Count >= id)
                {
                    int idt = id - 1;
                    Image img = new Bitmap(tankimg);
                    Point lastpos = tanks[idt].cords;
                    switch (e.KeyCode)
                    {
                        case Keys.A:
                            tanks[idt].rotate = Rotate.LEFT;
                            tanks[idt].cords = new Point(tanks[idt].cords.X > tanks[idt].speed ? tanks[idt].cords.X - tanks[idt].speed : tanks[idt].cords.X, tanks[idt].cords.Y);
                            break;
                        case Keys.D:
                            tanks[idt].rotate = Rotate.RIGHT;
                            tanks[idt].cords = new Point(tanks[idt].cords.X < gameField.Width - tanks[idt].speed - TankSize ? tanks[idt].cords.X + tanks[idt].speed : tanks[idt].cords.X, tanks[idt].cords.Y);
                            break;
                        case Keys.W:
                            tanks[idt].rotate = Rotate.UP;
                            tanks[idt].cords = new Point(tanks[idt].cords.X, tanks[idt].cords.Y > tanks[idt].speed ? tanks[idt].cords.Y - tanks[idt].speed : tanks[idt].cords.Y);
                            break;
                        case Keys.S:
                            tanks[idt].rotate = Rotate.DOWN;
                            tanks[idt].cords = new Point(tanks[idt].cords.X, tanks[idt].cords.Y < gameField.Height - tanks[idt].speed - TankSize ? tanks[idt].cords.Y + tanks[idt].speed : tanks[idt].cords.Y);
                            break;
                        default:
                            return;
                    }
                    switch (tanks[idt].rotate)
                    {
                        case Rotate.DOWN:
                            img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case Rotate.LEFT:
                            img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                        case Rotate.RIGHT:
                            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                    }
                    Tank tank = tanks[idt];
                    if (lastpos != tank.cords)
                    {
                        client.SendTank(tank);
                    }



                }
            }
            catch { }
           
            GC.Collect();
            Thread.Sleep(100);
        }
        private void Tank_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                if(tanks.Count > 0)
                {
                    try
                    {
                        e.Graphics.DrawString(tanks[gameField.Controls.IndexOf((Control)sender)].name, myFont, Brushes.Black, new PointF(0, 0));
                    }
                    catch { }
                }    
                    
            }
        }

    }
}
