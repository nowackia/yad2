using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Yad.Board.Common;
using System.Runtime.Serialization.Formatters.Binary;
//194.29.178.173
namespace Yad.MapEditor
{
    public partial class MapEditor : Form
    {
        enum Mode
        {
            Drawing,
            PointSelecing,
            SpiceAdding,
            None
        }


        int width, height;
        //TileType[,] map = null;
        MapData map = null;
        ToolStripButton currentTileTypeButton = null;
        Mode currentMode = Mode.Drawing;
        TileType currentTileType;
        Bitmap bmp = null;
        int tileWidth = 16;
        int mapWidth, mapHeight;
        bool painting = false;
        List<Point> startPoints = new List<Point>();

        Brush StartPointBrush = Brushes.LightBlue;
        Brush ThickSpiceBrush = Brushes.DarkRed;
        Brush ThinSpiceBrush = Brushes.Salmon;

        private const int MaxStartPointsNo = 8;
        private const int TickSpiceNo = 10;
        private int SpiceToAddAmount = 0;

        int brushSize = 1;

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
                startPoints.Clear();
                width = createDialog.MapWidth;
                height = createDialog.MapHeight;

                map = new MapData(width, height);

                InitUI();

                //Graphics g = Graphics.FromImage(bmp);
                //g.Clear(Color.Yellow);
            }
        }

        private void InitUI()
        {
            mapWidth = width * tileWidth;
            mapHeight = height * tileWidth;
            bmp = new Bitmap(tileWidth * width, tileWidth * height);
            this.pictureBox1.Width = mapWidth;
            this.pictureBox1.Height = mapHeight;
            this.pictureBox1.Image = bmp;
            for (int x = 0; x < width; ++x)
                for (int y = 0; y < height; ++y)
                    DrawTile(x, y);
        }

        private void SetTile(int x, int y, TileType tt)
        {
            Rectangle rc = new Rectangle(tileWidth * x, tileWidth * y, tileWidth, tileWidth);
            map[x][y].Type = tt;
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(GetBrush(tt), rc);
            //g.Dispose();
            this.pictureBox1.Invalidate(rc);
        }
        Font ft = new Font("Arial", 10);
        private void DrawTile(int x, int y)
        {
            int left = tileWidth * x;
            int top = tileWidth * y;
            Rectangle rc = new Rectangle(left, top, tileWidth, tileWidth);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(GetBrush(map[x][y].Type), rc);
            if (map[x][y].IsSpiceThin)
                g.FillRectangle(ThinSpiceBrush, rc);
            else
                if (map[x][y].IsSpiceThick)
                    g.FillRectangle(ThickSpiceBrush, rc);
            if (map[x][y].IsSpice)
                g.DrawString(map[x][y].SpiceNo.ToString(), ft, Brushes.Black, new PointF(left, top)); 
            if (startPoints.Contains(new Point(x,y)))
                g.FillEllipse(StartPointBrush, rc);
                
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
                    return Brushes.Yellow;
            }
            return Brushes.Yellow;
        }



        private void terrainButtonClicked(object sender, EventArgs e)
        {
            ToolStripButton src = ModeChange(sender);
            currentMode = Mode.Drawing;
            currentTileType = (TileType)src.Tag;
            currentTileTypeButton = src;
        }

        private ToolStripButton ModeChange(object sender)
        {
            if (currentTileTypeButton != null)
                currentTileTypeButton.Checked = false;
            if (btnStartPoint.Checked == true)
                btnStartPoint.Checked = false;

            ToolStripButton src = sender as ToolStripButton;
            src.Checked = true;
            return src;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

                if (!painting)
                    return;

                if (e.X < 0 || e.X >= mapWidth || e.Y < 0 || e.Y >= mapHeight)
                    return;

                int x = e.X / tileWidth;
                int y = e.Y / tileWidth;
                SetTile(x, y);
        }
        
        private void SetTile(int x, int y)
        {
            int half = (int)((brushSize)/ 2);
            if (brushSize == 1)
            {
                ApplyTile(x, y);
                return;
            }
            for (int i = x - half; i < x + half; ++i)
                for (int j = y - half; j < y + half; ++j)
                {
                    int px = Math.Min(Math.Max(0, i), this.width-1);
                    int py = Math.Min(Math.Max(0, j), this.height-1);
                    ApplyTile(px, py);
                }
        }

        private void ApplyTile(int x, int y)
        {
            switch (currentMode)
            {
                case Mode.SpiceAdding:
                    ApplySpiceAdd(x, y);
                    break;
                case Mode.Drawing:
                    ApplyCurrentType(x, y);
                    break;
            }
        }
        private void ApplyCurrentType(int x, int y)
        {
             map[x][y].Type = currentTileType;
             DrawTile(x, y);
        }

        private void ApplySpiceAdd(int x, int y)
        {
            map[x][y].SpiceNo += SpiceToAddAmount;
            DrawTile(x, y);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentMode == Mode.Drawing || currentMode == Mode.SpiceAdding)
            {
                if (e.Button == MouseButtons.Left)
                    painting = true;
                if (e.Button == MouseButtons.Right)
                {
                    painting = true;
                    SpiceToAddAmount = -SpiceToAddAmount;
                }
                
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentMode == Mode.Drawing || currentMode == Mode.SpiceAdding)
            {
                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                    painting = false;
                if (SpiceToAddAmount < 0)
                    SpiceToAddAmount = -SpiceToAddAmount;
                    
            }
        }

        private void MapEditor_Load(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (startPoints.Count != MaxStartPointsNo)
            {
                MessageBox.Show("All 8 start points must be defined", "Error");
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream stream = File.Open(sfd.FileName, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, startPoints);
                bformatter.Serialize(stream, map);
                stream.Close();
            }
        }

        private void btnStartPoint_Click(object sender, EventArgs e)
        {
            if (currentTileTypeButton != null)
                currentTileTypeButton.Checked = false;

            ToolStripButton src = sender as ToolStripButton;
            src.Checked = true;
            currentTileTypeButton = null;
            currentMode = Mode.PointSelecing;
        }
        private void RedrawStartPoints()
        {
            Graphics g = Graphics.FromImage(bmp);
            foreach (Point p in startPoints)
            {
                Rectangle rc = new Rectangle(p.X * tileWidth, p.Y * tileWidth, tileWidth, tileWidth);
                g.FillEllipse(Brushes.OrangeRed, rc);
                pictureBox1.Invalidate(rc);

            }
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (currentMode == Mode.PointSelecing)
            {
             
                int x = e.X / tileWidth;
                int y = e.Y / tileWidth;
                foreach (Point pt in startPoints)
                {
                    if (pt.X == x && pt.Y == y)
                    {
                        startPoints.Remove(pt);
                        DrawTile(x, y);
                        return;
                        
                    }
                }
                if (startPoints.Count == MaxStartPointsNo)
                {
                    MessageBox.Show("Maximum number of start points reached!", "Error");
                    return;
                }
                Point p = new Point(x, y);
                startPoints.Add(p);
                DrawTile(x, y);

            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void MapEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '+')
                brushSize = brushSize == 10 ? brushSize : brushSize + 1;
            if (e.KeyChar == '-')
                brushSize = brushSize == 1 ? brushSize : brushSize - 1;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ToolStripButton tsb = ModeChange(sender);
            if (tsb == btnThickSpice)
                SpiceToAddAmount = 10;
            else
                SpiceToAddAmount = 1;
            currentTileTypeButton = tsb;
            currentMode = Mode.SpiceAdding;

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = ofd.FileName;
                FileStream fs = File.Open(fileName, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                startPoints = (List<Point>)bf.Deserialize(fs);
                map = (MapData)bf.Deserialize(fs);
                for (int x = 0; x < map.Width; ++x)
                    for (int y = 0; y < map.Height; ++y)
                        map[x][y].SpiceNo = Math.Abs(map[x][y].SpiceNo);
                width = map.Width;
                height = map.Height;
                InitUI();
            }
            
        }

        
     


    }
}