using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TanksLib;

namespace ClientGame
{
    public partial class Form1 : Form
    {
        Client.Client client;
        public List<Tank> tanks { get; private set; }
        const int TankSize = 50;
        Image tankimg = Image.FromFile("tank.png");
        public Form1()
        {
            tanks = new List<Tank>();
            client = new Client.Client();
            InitializeComponent();
            client.Start();

            tanks.Add(new Tank() { name="fety", cords = new Point(100, 100), speed=5, id = int.Parse(client.GetString()) });
            tanks.ForEach(x=> gameField.Controls.Add(new PictureBox() {  BackgroundImage = tankimg, BackgroundImageLayout = ImageLayout.Zoom, Location = x.cords, Size = new Size(TankSize, TankSize) }));
            gameField.Controls.OfType<PictureBox>().ToList().ForEach(x => x.Paint += Tank_Paint);
            this.KeyDown += FormKeyDown;
        }

        private void FormKeyDown(object sender, KeyEventArgs e)
        {
            int id = 0;
            Image img = new Bitmap(tankimg);
            switch (e.KeyCode)  
            {
                case Keys.A:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    tanks[id].cords = new Point(tanks[id].cords.X > tanks[id].speed ? tanks[id].cords.X - tanks[id].speed : tanks[id].cords.X, tanks[id].cords.Y);
                    break;
                case Keys.D:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    tanks[id].cords = new Point(tanks[id].cords.X < gameField.Width - tanks[id].speed - TankSize ? tanks[id].cords.X + tanks[id].speed : tanks[id].cords.X, tanks[id].cords.Y);
                    break;
                case Keys.W:
                    tanks[id].cords = new Point(tanks[id].cords.X, tanks[id].cords.Y > tanks[id].speed ? tanks[id].cords.Y - tanks[id].speed : tanks[id].cords.Y);
                    break;
                case Keys.S:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    tanks[id].cords = new Point(tanks[id].cords.X, tanks[id].cords.Y < gameField.Height - tanks[id].speed - TankSize ? tanks[id].cords.Y + tanks[id].speed : tanks[id].cords.Y);
                    break;
            }
            gameField.Controls.OfType<PictureBox>().ToList()[id].BackgroundImage = img;
            gameField.Controls.OfType<PictureBox>().ToList()[id].Location = tanks[id].cords;
            gameField.Invalidate();
            GC.Collect();   
        }

        private void Tank_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString(tanks[gameField.Controls.IndexOf((Control)sender)].name, myFont, Brushes.Black, new PointF(0, 0));
            }
        }

        private void GameField_Validated(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
