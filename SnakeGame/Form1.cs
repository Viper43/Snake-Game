using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        int col = 50, row = 25, score = 0, front = 0, back = 0, dx = 0, dy = 0;
        Piece[] snake = new Piece[1250];
        List<int> available = new List<int>();
        bool[,] visit;
        Random random = new Random();

        Timer timer = new Timer();
        public Form1()
        {
            InitializeComponent();
            initial();
            LaunchTimer();
        }

        private void LaunchTimer()
        {
            timer.Interval = 50;
            timer.Tick += move;
            timer.Start();
            return;
        }


        private void snake_Keydown(object sender, KeyEventArgs e)
        {
            dx = dy = 0;
            switch(e.KeyCode)
            {
                case Keys.Right:
                    dx = 20;
                    break;
                case Keys.Left:
                    dx = -20;
                    break;
                case Keys.Up:
                    dy = 20;
                    break;
                case Keys.Down:
                    dy = -20;
                    break;
            }
        }

        private void move(object sender, EventArgs e)
        {
            int x = snake[front].Location.X, y = snake[front].Location.Y;
            if (dx == 0 || dy == 0) return;
            if( GameOver(x + dx, y + dy) )
            {
                timer.Stop();
                MessageBox.Show("Game Over............");
                return;
            }
            if ( collisionFood( x + dx, y + dy))
            {
                score += 1;
                scorelabel.Text = "Score : " + score.ToString();
                if (hits( (y + dy) / 20, (x + dx) / 20) ) return;
                Piece head = new Piece(x + dx, y + dy);
                front = (front - 1 + 1250) % 1250;
                snake[front] = head;
                visit[head.Location.Y / 20, head.Location.X / 20] = true;
                Controls.Add(head);
                randomFood();
            }
            else
            {
                if (hits((y + dy) / 20, (x + dx) / 20)) return;
                visit[snake[back].Location.Y / 20, snake[back].Location.X / 20] = false;
                front = (front - 1 + 1250) % 1250;
                snake[front] = snake[back];
                snake[front].Location = new Point(x + dx, y + dy);
                back = (back - 1 + 1250) % 1250;
                visit[(y + dy) / 20, (x + dx) / 20] = true;
            }
        }

        private void randomFood()
        {
            available.Clear();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; i < row; i++)
                {
                    if (!visit[i, j]) available.Add(i * col + j);
                }
            }
            int idx = random.Next(available.Count) % available.Count();
            foodlabel.Left = (available[idx] * 20) % Width;
            foodlabel.Top = (available[idx] * 20) / Width * 20;
        }
        private bool hits(int x, int y)
        {
            if ( visit[x, y] )
            {
                timer.Stop();
                MessageBox.Show("Snake hits the body...");
                return true;
            }
            return false;
        }

        private bool collisionFood(int x, int y)
        {
            return x == foodlabel.Location.X && y == foodlabel.Location.Y;
        }

        private bool GameOver(int x, int y)
        {
            return x < 0 || y < 0 || x > 980 || y > 480;
        }

        private void initial()
        {
            visit = new bool[row, col];
            Piece head = new Piece((random.Next() % col) * 20, (random.Next() % row) * 20);
            foodlabel.Location = new Point((random.Next() % col) * 20, (random.Next() % row) * 20);
            for(int i = 0; i < row; i++)
            {
                for (int j = 0; i < row; i++)
                {
                    visit[i, j] = false;
                    available.Add(i * col + j);
                }
                visit[head.Location.Y / 20, head.Location.X / 20] = true;
                available.Remove(head.Location.Y / 20 * col + head.Location.X / 20);
                Controls.Add(head);
                snake[front] = head;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
