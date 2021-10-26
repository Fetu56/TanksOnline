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
            id = int.Parse(client.GetString().Split(',')[0]);
            client.SendTank(new Tank() { name = "fety-"+id.ToString(), cords = new Point(100, 100), speed = 5, id = id});
            this.KeyDown += FormKeyDown;
            game = new Task(GameManager);
            game.Start();
        }
        private void GameManager()
        {
            while(true)
            {
                UpdateData();
            }
        }
        private void UpdateData()
        {
            string json = client.GetString();
            tanks.Clear();
            gameField.Controls.Clear();
            try
            {
                tanks = JsonSerializer.Deserialize<List<Tank>>(json);
            }
            catch { }
            tanks.ForEach(x => {
                Image img = new Bitmap(tankimg);
                
                 switch (x.rotate)
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

                gameField.Controls.Add(new PictureBox() { BackgroundImage = img, BackgroundImageLayout = ImageLayout.Zoom, Location = x.cords, Size = new Size(TankSize, TankSize) });
            }
            );
            gameField.Controls.OfType<PictureBox>().ToList().ForEach(x => x.Paint += Tank_Paint);
            
                GC.Collect();
        }
        private void FormKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space) UpdateData();
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
                }
                switch (tanks[idt].rotate)
                {
                    case Rotate.DOWN:
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case Rotate.UP:
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
                if (gameField.Controls.Count > 0)
                {
                    this.Invoke((MethodInvoker)(()=> {
                        gameField.Controls.OfType<PictureBox>().ToList()[idt].BackgroundImage = img;
                        gameField.Controls.OfType<PictureBox>().ToList()[idt].Location = tanks[idt].cords;
                        gameField.Invalidate();
                    }));

                }
                else
                {
                    tanks.ForEach(x => {
                        Image img = new Bitmap(tankimg);

                        switch (x.rotate)
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

                        gameField.Controls.Add(new PictureBox() { BackgroundImage = img, BackgroundImageLayout = ImageLayout.Zoom, Location = x.cords, Size = new Size(TankSize, TankSize) });
                    }
           );
                    gameField.Controls.OfType<PictureBox>().ToList().ForEach(x => x.Paint += Tank_Paint);
                }
                
            }
            GC.Collect();
        }
        private void Tank_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                if(tanks.Count > 0)
                    e.Graphics.DrawString(tanks[gameField.Controls.IndexOf((Control)sender)].name, myFont, Brushes.Black, new PointF(0, 0));
            }
        }

    }
}
