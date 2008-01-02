using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Yad.Board;
using Yad.AI.General;

namespace AStarTest
{
    public partial class FormMain : Form
    {
        const int tileWidth = 20;
        const int tileHeight = 20;
        const int mapWidth = 64;
        const int mapHeight = 64;
        int[,] tiles = null;
        int OCCUPIED = -1;
        int EMPTY = 0;
        Position start = Position.Invalid;
        Position goal = Position.Invalid;
        int goals = 0;
        TestInput ti = null;
        Queue<Position> q = new Queue<Position>();
        public FormMain()
        {
            InitializeComponent();
            pictureBox1.Size = new Size(mapWidth * tileWidth, mapHeight * tileHeight);
            tiles = new int[mapWidth, mapHeight];
            ti = new TestInput(tiles);
            ti.IsMoveable += new TestInput.MoveCheckDelegate(IsMoveable);

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int x = 0; x < mapWidth; ++x)
                for (int y = 0; y < mapHeight; ++y)
                {

                    Rectangle rect = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
                    if (tiles[x, y] == OCCUPIED)
                        g.FillRectangle(Brushes.Red, rect);
                    g.DrawRectangle(Pens.Black, rect);
                   
                }
            if (!start.Equals(Position.Invalid))
            {
                Rectangle rect = new Rectangle(start.X * tileWidth, start.Y * tileHeight, tileWidth, tileHeight);
                g.FillEllipse(Brushes.Blue, rect);
            }
            if (!goal.Equals(Position.Invalid))
            {
                Rectangle rect = new Rectangle(goal.X * tileWidth, goal.Y * tileHeight, tileWidth, tileHeight);
                g.FillEllipse(Brushes.Orange, rect);
            }
            
            if (q.Count != 0)
            {
                Position[] p = q.ToArray();
                for (int i = 0; i < p.Length ; ++i)
                {
                    Rectangle rect = new Rectangle(p[i].X * tileWidth, p[i].Y * tileHeight, tileWidth, tileHeight);
                    g.FillEllipse(Brushes.Green, rect);
                }
            }
            /*Point pt1 = new Point(0,0);
            Point pt2 = new Point(0,0);
            for (int i = 0; i < mapWidth; ++i)
            {
                pt1.X = pt2.X = i * tileWidth;
                pt1.Y = 0;
                pt2.Y = mapHeight * (tileHeight);
                g.DrawLine(Pens.Black, pt1, pt2);
            }*/
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.Location.X / tileWidth;
            int y = e.Location.Y / tileHeight;
            if (e.Button == MouseButtons.Left)
            {
                if (tiles[x, y] == OCCUPIED)
                    tiles[x, y] = 0;
                else
                    tiles[x, y] = OCCUPIED;
            }
            else
            {
                if (this.start.X == x && this.start.Y == y)
                {
                    goal = Position.Invalid;
                    start = Position.Invalid;
                    goals = 0;
                    pictureBox1.Refresh();
                    return;
                }
                if (this.goal.X == x && this.goal.Y == y)
                {
                    goal = Position.Invalid;
                    if (goals == 2)
                        goals--;
                    pictureBox1.Refresh();
                    return;
                }
                if (this.start.Equals(Position.Invalid))
                {
                    start = new Position(x, y);
                    goals++;
                    pictureBox1.Refresh();
                    return;
                }
                else
                {
                    if (goals < 2 && this.goal.Equals(Position.Invalid))
                    {
                        goal = new Position(x, y);
                        goals++;
                        pictureBox1.Refresh();
                        return;
                    }
                }
                
            }
            pictureBox1.Refresh();
        }

        public bool IsMoveable(short x, short y, int[,] tiles)
        {
            if (tiles[x, y] == EMPTY)
                return true;
            return false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.ti.Start = start;
            this.ti.Goal = goal;
            q = AStar.Search<Position>(ti);
            if (null == q)
                q = new Queue<Position>();
            pictureBox1.Refresh();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            q.Clear();
            pictureBox1.Refresh();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < mapWidth; ++x)
                for (int y = 0; y < mapHeight; ++y)
                    tiles[x, y] = EMPTY;
            pictureBox1.Refresh();
        }


    }
}