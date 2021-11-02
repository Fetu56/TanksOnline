using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        Image brickimg = Image.FromFile("brick.png");
        Task game;
        string json;
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
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.gameField.Paint += GameField_Paint;
            this.gameField.BackgroundImage = brickimg;
            client.SendTank(new Tank() { name = "PL-"+id.ToString(), cords = new Point(100, 100), speed = 15, id = id, bulletref = 0, hp = 100});
            this.KeyUp += FormKeyDown;
            game = new Task(GameManager);
            game.Start();
        }

        private void GameField_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.FillRectangle(Brushes.White, new Rectangle(10, 10, 485, 485));
        }

        private void GameManager()
        {

            while(!client.process.IsCompleted)
            {
                try
                {
                    json = client.GetString();
                    UpdateData();
                }
                catch { }
            }
        }
        private void UpdateData()
        {
            Thread.Sleep(5);
            tanks.Clear();
                tanks = JsonSerializer.Deserialize<List<Tank>>(json);
                for (int i = 0; i < tanks.Count; i++)
                {

                        if (gameField.Controls.Count <= i)
                        {
                            if (tanks[i] == null || tanks[i].hp < 0)
                            {
                                this.gameField.Invoke((MethodInvoker)delegate {

                                    gameField.Controls.Add(new PictureBox() { Visible = false});
                                    gameField.Controls.OfType<PictureBox>().ToList().ForEach(x => x.Paint += Tank_Paint);

                                });
                            }
                            else
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
                                gameField.Controls[i].BackColor = Color.White;
                            }
                            
                        }
                        else
                        {
                            if (tanks[i] == null || tanks[i].hp < 0)
                            {
                                gameField.Controls[i].Visible = false;
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

                                gameField.Controls[i].BackColor = Color.White;
                            }
                            
                        }
                   
                }
            gameField.Controls.OfType<PictureBox>().ToList().ForEach(x => x.Invalidate());

                //GC.Collect();
        }
        private void FormKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.A || e.KeyCode == Keys.D || e.KeyCode == Keys.S || e.KeyCode == Keys.W)
            {
                try
                {
                    List<Tank> tankTMP = new List<Tank>(tanks);
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
                        try
                        {
                            if (!Tank.CheckCollision(tankTMP, tankTMP[idt].cords, TankSize))
                            {
                                tankTMP[idt].cords = lastpos;
                            }
                        }
                        catch { }
                        switch (tankTMP[idt].rotate)
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
                        
                        if (lastpos != tankTMP[idt].cords)
                        {
                            client.SendTank(tankTMP[idt]);
                        }



                    }
                    else
                    {
                        UpdateData();
                    }
                }
                catch { }
            }
            else if(e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                Tank tank = tanks[id - 1];
                tank.bulletref = 100;
                client.SendTank(tank);
            }
            
           
            GC.Collect();
            Thread.Sleep(100);
        }
        private void Tank_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold))
            {
                if(tanks.Count > 0)
                {
                    try
                    {
                        if(tanks[gameField.Controls.IndexOf((Control)sender)].hp > 0)
                        {
                           
                            GraphicsPath p = new GraphicsPath();
                            p.AddString(
                                tanks[gameField.Controls.IndexOf((Control)sender)].name,   
                                myFont.FontFamily, 
                                (int)FontStyle.Regular,     
                               (float)15.2,    
                                new Point(3, 0),              
                                new StringFormat());    
                            e.Graphics.DrawPath(Pens.Black, p);
                            e.Graphics.DrawString(tanks[gameField.Controls.IndexOf((Control)sender)].name, myFont, Brushes.White, new PointF(3, 0));
                            e.Graphics.FillEllipse(new SolidBrush(takeHpClr(tanks[gameField.Controls.IndexOf((Control)sender)].hp)), new Rectangle(new Point(0, (int)(TankSize / 2.3)), new Size((int)(TankSize / 2.1), (int)(TankSize / 2.1))));
                            e.Graphics.DrawString(tanks[gameField.Controls.IndexOf((Control)sender)].hp.ToString(), myFont, Brushes.White, new PointF(-1, (int)(TankSize/2.3)+1));
                        }
                        
                    }
                    catch { }
                }    
                    
            }
        }
        private Color takeHpClr(int hp)
        {
            Color clr;
            if(hp > 80)
            {
                clr = Color.Green;
            }
            else if(hp >60)
            {
                clr = Color.YellowGreen;
            }
            else if (hp > 40)
            {
                clr = Color.Orange;
            }
            else if (hp > 20)
            {
                clr = Color.OrangeRed;
            }
            else
            {
                clr = Color.Red;
            }
            return clr;
        }

    }
}
