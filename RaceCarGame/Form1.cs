using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaceCarGame
{
    public partial class Form1 : Form
    {
        static public int Counter = 0;
        static int randomNumber2;
        static int score = 0;
        static int Game_length = 0;
        int[,] LeftArray = new int[50, 2];      // x and y of left line
        Point[] lPoints = new Point[50];
        Point[] rPoints = new Point[50];
//        int[,] RightArray = new int[10, 2];     // x and y of right line
        Random rnd = new Random();
        int x, y;
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Location = new Point(200, 700);
            x = pictureBox1.Left;
            y = pictureBox1.Top;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen blackPen = new Pen(Color.Black, 3);
            Counter++;

            if (Counter == 1)
            {
                int randomNumber = rnd.Next(100, 200 + 1);
                for (int i = 0; i < lPoints.Length; i++)
                {
                    LeftArray[i, 0] = randomNumber;
                    LeftArray[i, 1] = i * this.Height / (lPoints.Length);

                    Point LeftPoint = new Point(LeftArray[i, 0], LeftArray[i, 1]);
                    Point RightPoint = new Point(LeftArray[i, 0] + pictureBox1.Width * 3, LeftArray[i, 1]);
                    lPoints[i] = LeftPoint;
                    rPoints[i] = RightPoint;
                }
                e.Graphics.DrawLines(blackPen, lPoints);
                e.Graphics.DrawLines(blackPen, rPoints);
            }
            else
            {
                e.Graphics.DrawString($"{Game_length:#,0}m", new Font("Arial", 20), Brushes.Blue, this.Width * 3 / 4, this.Height-100);
                if (Game_length++ >= 1000 )
                {
                    Game_length--;
                    e.Graphics.DrawString($"Your Score: {score:#,0}", new Font("Arial", 20), Brushes.Blue, this.Width/2, 10);
                    timer1.Enabled = false;
                    return;
                }
                if ((LeftArray[lPoints.Length - 1, 0] <= x) && ((x + pictureBox1.Width) < LeftArray[lPoints.Length - 1, 0] + pictureBox1.Width * 3))
                {   // not crashed.
                }
                else
                {
                    Game_length--;
                    e.Graphics.DrawLines(blackPen, lPoints);
                    e.Graphics.DrawLines(blackPen, rPoints);
                    SolidBrush redBrush = new SolidBrush(Color.Red);
                    e.Graphics.DrawString("BOOM", new Font("Arial", 60), redBrush, 10, 10);
                    e.Graphics.DrawString($"Your Score: {score:#,0}", new Font("Arial", 20), Brushes.Red, this.Width/2, 10);
                    score -= 10;
                    return;
                }
                
                for (int i = lPoints.Length - 1; 0 < i; --i)
                {
                    LeftArray[i, 0] = LeftArray[i - 1, 0];
                    LeftArray[i, 1] = i * this.Height / (lPoints.Length);
                    Point lPointNext = new Point(LeftArray[i, 0], LeftArray[i, 1]);
                    Point rPointNext = new Point(LeftArray[i, 0] + pictureBox1.Width * 3, LeftArray[i, 1]);
                    lPoints[i] = lPointNext;
                    rPoints[i] = rPointNext;
                }
                if (Counter % 50 == 0)
                {
                    randomNumber2 += rnd.Next(-5, 5 + 1);
                    if (randomNumber2 == 0)
                    {
                        randomNumber2 = (((int)rnd.Next(0, 1 + 1) == 0) ? -1 : 1);
                    }
                }
                else if (Counter % 20 == 0)
                {
                    randomNumber2 += rnd.Next(-2, 2 + 1);
                    if (randomNumber2 == 0)
                    {
                        randomNumber2 = (((int)rnd.Next(0, 1 + 1) == 0) ? -1 : 1);
                    }
                }
                if (LeftArray[0, 0] + randomNumber2 < 10)
                {
                    LeftArray[0, 0] = 10;
                    randomNumber2 = 1;
                }
                else if (this.Width - 50 - pictureBox1.Width * 3 < LeftArray[0, 0]+randomNumber2)
                {
                    LeftArray[0, 0] = this.Width - 50 - pictureBox1.Width * 3;
                    randomNumber2 = -1;
                }
                else
                {
                    LeftArray[0, 0] += randomNumber2;
                }

                LeftArray[0, 1] = 0;
                lPoints[0] = new Point(LeftArray[0, 0], LeftArray[0, 1]);
                rPoints[0] = new Point(LeftArray[0, 0]+ pictureBox1.Width * 3, LeftArray[0, 1]);
                e.Graphics.DrawLines(blackPen, lPoints);
                e.Graphics.DrawLines(blackPen, rPoints);
                // crash check
                if ((LeftArray[lPoints.Length-1,0] <= x) && ((x+pictureBox1.Width) < LeftArray[lPoints.Length-1,0] + pictureBox1.Width * 3)   )
                {   // not crashed.
                    timer1.Enabled = true;
                    timer1.Interval = rnd.Next(20, 100 + 1);
                    score += 10;
                    e.Graphics.DrawString($"Your Score: {score:#,0}", new Font("Arial", 20), Brushes.Blue, this.Width/2, 10);
                }
                else
                {
                    //                    timer1.Enabled = false;
                    timer1.Interval = 100;
                    SolidBrush redBrush = new SolidBrush(Color.Red);
                    e.Graphics.DrawString("BOOM", new Font("Arial", 60), redBrush, 10, 10);
                    score -= 10;
                    e.Graphics.DrawString($"Your Score: {score:#,0}", new Font("Arial", 20), Brushes.Red, this.Width/2, 10);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            x = pictureBox1.Location.X;
            y = pictureBox1.Location.Y;
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (x < this.Size.Width - pictureBox1.Width)
                    {
                        x += 2;
                    }
                    pictureBox1.Location = new System.Drawing.Point(x, y);
                    break;
                case Keys.Left:
                    if (x > 2)
                    {
                        x -= 2;
                    }
                    pictureBox1.Location = new System.Drawing.Point(x, y);
                    break;
                default:
                    break;
            }
            this.Refresh();
        }
    }
}
