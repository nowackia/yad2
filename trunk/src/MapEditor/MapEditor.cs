using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Client.Board;
using System.IO;

namespace MapEditor
{
    public partial class MapEditor : Form
    {
        int width, height;
        TileType[,] map = null;
        ToolStripButton currentTileTypeButton = null;
        TileType currentTileType;
        Bitmap bmp = null;
        int tileWidth = 16;
        int mapWidth, mapHeight;
        bool painting = false;

        public MapEditor()
        {
            InitializeComponent();

            this.rockButton.Tag = TileType.Rock;
            this.sandButton.Tag = TileType.Sand;
            this.mountainButton.Tag = TileType.Mountain;

            this.rockButton.PerformClick();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateDialog createDialog = new CreateDialog();
            if (createDialog.ShowDialog() == DialogResult.OK)
            {
                width = createDialog.MapWidth;
                height = createDialog.MapHeight;

                map = new TileType[width, height];
                mapWidth = width * tileWidth;
                mapHeight = height * tileWidth;
                bmp = new Bitmap(tileWidth * width, tileWidth * height);
                this.pictureBox1.Width = mapWidth;
                this.pictureBox1.Height = mapHeight;
                this.pictureBox1.Image = bmp;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        map[x, y] = TileType.Sand;
                    }
                }
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.Yellow);
            }
        }

        private void SetTile(int x, int y, TileType tt)
        {
            Rectangle rc = new Rectangle(tileWidth * x, tileWidth * y, tileWidth, tileWidth);
            map[x, y] = tt;
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(GetBrush(tt), rc);
            //g.Dispose();
            this.pictureBox1.Invalidate(rc);
        }

        private Brush GetBrush(TileType tt)
        {
            switch (tt)
            {
                case TileType.Mountain:
                    return Brushes.Brown;
                case TileType.Rock:
                    return Brushes.Gray;
                case TileType.Sand:
                default:
                    return Brushes.Yellow;
            }
        }



        private void terrainButtonClicked(object sender, EventArgs e)
        {
            if (currentTileTypeButton != null)
                currentTileTypeButton.Checked = false;

            ToolStripButton src = sender as ToolStripButton;
            src.Checked = true;

            currentTileType = (TileType)src.Tag;
            currentTileTypeButton = src;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!painting)
                return;

            if (e.X < 0 || e.X >= mapWidth || e.Y < 0 || e.Y >= mapHeight)
                return;

            int x = e.X / tileWidth;
            int y = e.Y / tileWidth;
            SetTile(x, y, currentTileType);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                painting = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                painting = false;
        }

        private void MapEditor_Load(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        sw.Write((int)map[x, y]);
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }
        }
    }
}