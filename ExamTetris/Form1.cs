using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamTetris
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Panel GamePanel;
        List<List<int>> gameList = new List<List<int>>() { 
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };
        bool gameActive = false;
        bool squareIsFall = true;
        Point coordCurrentBox;
        Random random = new Random();
        List<Color> colors = new List<Color>() { Color.Red, Color.Green, Color.Blue, Color.Yellow};
        public Form1()
        {
            InitializeComponent();

            bmp = new Bitmap(panelGame.ClientSize.Width, panelGame.ClientSize.Height);
            Button button4 = new Button();
            button4.Location = new Point(0, 0);

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, panelGame, new object[] { true });

            panelGame.Paint += PanelGame_Paint;
            
        }

        private void PanelGame_Paint(object sender, PaintEventArgs e)
        {
            bmp = new Bitmap(panelGame.ClientSize.Width, panelGame.ClientSize.Height);

            SolidBrush sBrush = new SolidBrush(Color.White);
            Graphics graph = Graphics.FromImage(bmp);
            //graph.FillRectangle(sBrush, panelGame.ClientRectangle);

            RenderMesh();
            if(gameActive)
            {
                DrawPlayingField();
            }

            e.Graphics.DrawImageUnscaled(bmp, Point.Empty);
        }

        private void RenderMesh()
        {
            Graphics graphics = Graphics.FromImage(bmp);
            int width = panelGame.ClientSize.Width;
            int height = panelGame.ClientSize.Height;
            //MessageBox.Show(width.ToString(), height.ToString());
            int stepX = width / 10;
            int stepY = height / 10;
            Point p1, p2;
            //Vertical
            for(int x = 0; x <= stepX*10; x+=stepX) 
            {
                p1 = new Point(x, 0);
                p2 = new Point(x, stepY * 10);
                graphics.DrawLine(Pens.Black, p1, p2);
            }
            //Horizontal
            for (int y = 0; y <= stepY*10; y += stepY)
            {
                p1 = new Point(0, y);
                p2 = new Point(stepX * 10, y);
                graphics.DrawLine(Pens.Black, p1, p2);
            }
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            gameList = new List<List<int>>() {
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};
            gameActive = true;
            squareIsFall = true;
            timer.Interval = 500;
            timer.Start();
            panelGame.Invalidate();
            panelGame.Focus();
        }

        private Point GenerateNewBox()
        {
            int position = random.Next(0, 10);
            gameList[0][position] = random.Next(1, 5);
            return new Point(position, 0);
        }
        private void ChangeCurrentCoordY()
        {
            int valueOfBox = gameList[coordCurrentBox.Y][coordCurrentBox.X];
            gameList[coordCurrentBox.Y][coordCurrentBox.X] = 0;
            coordCurrentBox.Y += 1;
            gameList[coordCurrentBox.Y][coordCurrentBox.X] = valueOfBox;
        }

        private void ChangeCurrentCoordX(int sign)
        {
            int valueOfBox = gameList[coordCurrentBox.Y][coordCurrentBox.X];
            gameList[coordCurrentBox.Y][coordCurrentBox.X] = 0;
            coordCurrentBox.X += sign;
            gameList[coordCurrentBox.Y][coordCurrentBox.X] = valueOfBox;
        }

        private void DrawPlayingField()
        {
            Graphics graphics = Graphics.FromImage(bmp);
            SolidBrush brush;
            int width = panelGame.ClientSize.Width;
            int height = panelGame.ClientSize.Height;
            int stepX = width / 10;
            int stepY = height / 10;
            if (gameActive) 
            {
                for(int i = 0; i < gameList.Count; i++) 
                {
                    for(int j=0; j < gameList[i].Count; j++)
                    {
                        if (gameList[i][j] != 0)
                        {
                            brush = new SolidBrush(colors[gameList[i][j] - 1]);
                            graphics.FillRectangle(brush, stepX * j, stepY * i, stepX, stepY);
                        }
                    }
                }
            
            }
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            if (squareIsFall)
            {
                coordCurrentBox = GenerateNewBox();
                squareIsFall = false;
            }
            else
            {
                if ((coordCurrentBox.Y + 1) < 10 && gameList[coordCurrentBox.Y + 1][coordCurrentBox.X] == 0)
                {
                    ChangeCurrentCoordY();
                }
                else
                {
                    squareIsFall = true;
                }
            }

            panelGame.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(gameActive)
            {
                if((e.KeyCode == Keys.A || e.KeyCode == Keys.Left) && (coordCurrentBox.X - 1) >= 0 && gameList[coordCurrentBox.Y][coordCurrentBox.X -1] == 0) 
                {
                    ChangeCurrentCoordX(-1);
                    panelGame.Invalidate();
                }
                else if((e.KeyCode == Keys.D || e.KeyCode == Keys.Right) && (coordCurrentBox.X + 1) < 10 && gameList[coordCurrentBox.Y][coordCurrentBox.X+1] == 0)
                {
                    ChangeCurrentCoordX(1);
                    panelGame.Invalidate();
                }
            }
        }
    }
}
