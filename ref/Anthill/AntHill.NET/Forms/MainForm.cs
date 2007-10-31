using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AntHill.NET.Forms;
using Tao.OpenGl;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace AntHill.NET
{
    public partial class MainForm : Form
    {
        private float sceneWidth, sceneHeight; //OpenGL *scene* size, *not* control's size
        private float offsetX = 0, offsetY = 0; //scrollbar offset
        private int maxMagnitude = 0;
        private bool scrolling = false, rotating = false;
        private ConfigForm cf = null;        
        private Point mousePos;
        private double lookAtAngleX, lookAtAngleY;
        
        Counter counter = new Counter();
        public bool done = false;

        private bool InitGL()
        {
            Gl.glEnable(Gl.GL_TEXTURE_2D);                                      // Enable Texture Mapping
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glShadeModel(Gl.GL_SMOOTH);                                      // Enable Smooth Shading
            Gl.glClearColor(0, 0, 0, 0);                                     // Black Background
            Gl.glClearDepth(1);                                                 // Depth Buffer Setup
            Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
            Gl.glDepthFunc(Gl.GL_LEQUAL);                                       // The Type Of Depth Testing To Do                        
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            ReSizeGLScene(maxMagnitude, maxMagnitude, true);

            InitDrawing();
            return true;
        }

        public MainForm()
        {
            InitializeComponent();

            openGLControl.InitializeContexts();
            InitGL();

            try
            {
                AHGraphics.Init();
            }
            catch
            {
                MessageBox.Show(Properties.Resources.errorGraphics);
                throw new Exception();
            }

            cf = new ConfigForm();

            this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
        }

        void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (rightPanel.Enabled == false) return;

            int zoom = magnitudeBar.Value + (e.Delta * (magnitudeBar.Maximum - magnitudeBar.Minimum) / 1200);
            if (zoom > magnitudeBar.Maximum) zoom = magnitudeBar.Maximum;
            if (zoom < magnitudeBar.Minimum) zoom = magnitudeBar.Minimum;
            magnitudeBar.Value = zoom;

            magnitudeBar_Scroll(null, null);
        }

        private void ReSizeGLScene(float width, float height, bool updateViewport)
        {
            if (updateViewport)            
                Gl.glViewport(0, 0, openGLControl.Width, openGLControl.Height);                            
            Gl.glMatrixMode(Gl.GL_PROJECTION);                                  // Select The Projection Matrix
            Gl.glLoadIdentity();                                                // Reset The Projection Matrix
            //Gl.glOrtho(-(0.5f * width), (0.5f * width), (0.5f * height), -(0.5f * height), -100, 100);
            Glu.gluPerspective(60.0f, (float)openGLControl.Width / (float)openGLControl.Height, 1.0f, 10000.0f);
            sceneWidth = width;
            sceneHeight = height;
            Gl.glMatrixMode(Gl.GL_MODELVIEW);                                   // Select The Modelview Matrix
            Gl.glLoadIdentity();                                                // Reset The Modelview Matrix            
        }

        private void loadData(object sender, EventArgs e)
        {
            pauseButton_Click(this, null);
            this.Resize -= new EventHandler(UpdateMap);

            string name;
            if (simulationXMLopenFileDialog.ShowDialog() == DialogResult.OK)
            {                
                name = simulationXMLopenFileDialog.FileName;
                XmlReaderWriter reader = new XmlReaderWriter();
                try
                {
                    reader.ReadMe(name);
                    Simulation.DeInit();
                    Simulation.Init(new Map(AntHillConfig.mapColCount, AntHillConfig.mapRowCount, AntHillConfig.tiles));

                }
                catch
                {
                    openGLControl.Invalidate();
                    rightPanel.Enabled = false;

                    MessageBox.Show(Properties.Resources.errorInitialization);
                    return;
                }

                rightPanel.Enabled = true;                
                
                timer.Interval = speedBar.Maximum - speedBar.Value + speedBar.Minimum;

                this.Resize += new EventHandler(UpdateMap);
                if (Simulation.simulation.Map.Width > Simulation.simulation.Map.Height)
                    maxMagnitude = Simulation.simulation.Map.Width;
                else
                    maxMagnitude = Simulation.simulation.Map.Height;
                offsetX = -(Simulation.simulation.Map.Width >> 1) + 0.5f;
                offsetY = -(Simulation.simulation.Map.Height >> 1) + 0.5f;
                magnitudeBar.Maximum = 50 * Simulation.simulation.Map.Height;
                AntHillConfig.curMagnitude = ((float)magnitudeBar.Value) / 1000.0f;
                vScrollBar1.Minimum = 0;
                vScrollBar1.LargeChange = 1;
                vScrollBar1.Maximum = 10 * Simulation.simulation.Map.Height + vScrollBar1.LargeChange;
                vScrollBar1.Value = 10 * (Simulation.simulation.Map.Height >> 1);
                vScrollBar1.Enabled = true;
                hScrollBar1.Minimum = 0;
                hScrollBar1.LargeChange = 1;
                hScrollBar1.Maximum = 10 * Simulation.simulation.Map.Width + hScrollBar1.LargeChange;
                hScrollBar1.Value = 10 * (Simulation.simulation.Map.Width >> 1);
                hScrollBar1.Enabled = true;
                RecalculateUI(true);
                openGLControl.Invalidate();
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            doTurnButton.Enabled = false;
            startButton.Enabled = false;
            btnStop.Enabled = true;
            btnReset.Enabled = true;

            timer.Start();
            //((ISimulationUser)Simulation.simulation).Start();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            doTurnButton.Enabled = true;
            startButton.Enabled = true;
            btnStop.Enabled = false;
            btnReset.Enabled = true;                        
        }

        private void doTurnButton_Click(object sender, EventArgs e)
        {
            //doTurnButton.Enabled = true;
            //startButton.Enabled = true;
            btnReset.Enabled = true;
            //btnStop.Enabled = false;

            if (((ISimulationUser)Simulation.simulation).DoTurn() == false)
            {
                openGLControl.Invalidate();
                MessageBox.Show(Properties.Resources.SimulationFinished);
            }

            openGLControl.Invalidate();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            timer.Stop();

            startButton.Enabled = true;
            btnReset.Enabled = true;
            doTurnButton.Enabled = true;
            btnStop.Enabled = false;

            //((ISimulationUser)Simulation.simulation).Pause();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (((ISimulationUser)Simulation.simulation).DoTurn() == false)
            {
                timer.Stop();
                MessageBox.Show(Properties.Resources.SimulationFinished);
            }
            counter.RoundTick();
            //this.Text = "AntHill.NET - fps:" + counter.FPS + " rps:" + counter.RPS;
            this.Text = "AntHill.NET - fps/rps:" + counter.RPS;
            openGLControl.Invalidate();
        }

        private void buttonShowConfig_Click(object sender, EventArgs e)
        {
            cf.RefreshData();
            cf.Show();
        }
        
        private void UpdateMap(object sender, EventArgs e)
        {
            RecalculateUI(true);            
            openGLControl.Invalidate();
        }

        private void RecalculateUI(bool recalculateViewport)
        {
            float mapWidth = Simulation.simulation.Map.Width,
                mapHeight = Simulation.simulation.Map.Height;

            float magnitude = ((float)magnitudeBar.Value) / ((float)magnitudeBar.Maximum);
            float x = 1.0f + (float)(Simulation.simulation.Map.Width - 1) * magnitude;
            float y = 1.0f + (float)(Simulation.simulation.Map.Height - 1) * magnitude;            

            ReSizeGLScene(x, y, recalculateViewport);
        }

        private void speedBar_Scroll(object sender, EventArgs e)
        {
            //timer.Interval = speedBar.Value;
            timer.Interval = 10000 / (speedBar.Value);
        }
        
        private void magnitudeBar_Scroll(object sender, EventArgs e)
        {
            AntHillConfig.curMagnitude = ((float)magnitudeBar.Value) / 1000.0f;
            cf.RefreshData();
            RecalculateUI(false);
            openGLControl.Invalidate();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Simulation.DeInit();
            Simulation.Init(new Map(AntHillConfig.mapColCount, AntHillConfig.mapRowCount, AntHillConfig.tiles));

            startButton.Enabled = true;
            //btnReset.Enabled = true;
            doTurnButton.Enabled = true;
            btnStop.Enabled = false;

            openGLControl.Invalidate();
        }        

        private void Scrolled(object sender, EventArgs e)
        {
            openGLControl.Invalidate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            done = true;
        }

        private bool ShouldOmitDrawing(int x, int y)
        {            
            return ((x + offsetX + 1 < -sceneWidth * 0.5f) ||
                       (x + offsetX - 1 > sceneWidth * 0.5f) ||
                       (y + offsetY + 1 < -sceneHeight * 0.5f) ||
                       (y + offsetY - 1 > sceneHeight * 0.5f));
        }

        private void openGLControl_Paint(object sender, PaintEventArgs ea)
        {
            counter.FrameTick();
            
            //Gl.glClearColor(0, 0, 0, 0);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            if (!cbVisualize.Checked) return;

            Gl.glLoadIdentity();
            Glu.gluLookAt(  0, AntHillConfig.curMagnitude * 10.0f, AntHillConfig.curMagnitude * -20.0f, 
                            0, 0, 0, 
                            0, -1, 0);//0.5f * Math.Sqrt(2), 0.5f * Math.Sqrt(2));
            Gl.glRotated(lookAtAngleX, 1.0d, 0.0d, 0.0d);
            Gl.glRotated(lookAtAngleY, 0.0d, 1.0d, 0.0d);
            if (Simulation.simulation == null) return;
                        
            Map map = Simulation.simulation.Map;

            int signal;

            Gl.glColor4f(1, 1, 1, 1);
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    //if (ShouldOmitDrawing(x, y)) continue;
                    DrawElement(x, y, map.GetTile(x, y).GetTexture(), Dir.N, offsetX, offsetY, 1, 1, 0.0f);
                }
            }

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    //if (ShouldOmitDrawing(x, y)) continue;

                    if ((signal = map.MsgCount[x, y].GetCount(MessageType.FoodLocalization)) > 0)
                    {
                        Gl.glColor4f(1, 1, 1, (float)(signal + AntHillConfig.signalInitialAlpha) / AntHillConfig.signalHighestDensity);
                        DrawElement(x, y, (int)AHGraphics.Texture.MessageFoodLocation, Dir.N, offsetX, offsetY, 1, 1, 0.01f);
                    }
                    if ((signal = map.MsgCount[x, y].GetCount(MessageType.QueenInDanger)) > 0)
                    {
                        Gl.glColor4f(1, 1, 1, (float)(signal + AntHillConfig.signalInitialAlpha) / AntHillConfig.signalHighestDensity);
                        DrawElement(x, y, (int)AHGraphics.Texture.MessageQueenInDanger, Dir.N, offsetX, offsetY, 1, 1, 0.01f);
                    }
                    if ((signal = map.MsgCount[x, y].GetCount(MessageType.QueenIsHungry)) > 0)
                    {
                        Gl.glColor4f(1, 1, 1, (float)(signal + AntHillConfig.signalInitialAlpha) / AntHillConfig.signalHighestDensity);
                        DrawElement(x, y, (int)AHGraphics.Texture.MessageQueenIsHungry, Dir.N, offsetX, offsetY, 1, 1, 0.01f);
                    }
                    if ((signal = map.MsgCount[x, y].GetCount(MessageType.SpiderLocalization)) > 0)
                    {
                        Gl.glColor4f(1, 1, 1, (float)(signal + AntHillConfig.signalInitialAlpha) / AntHillConfig.signalHighestDensity);
                        DrawElement(x, y, (int)AHGraphics.Texture.MessageSpiderLocation, Dir.N, offsetX, offsetY, 1, 1, 0.01f);
                    }
                }
            }

            Gl.glColor4f(1, 1, 1, 1);
            Creature e;
            Food f;
            LIList<Ant>.Enumerator enumerator = Simulation.simulation.ants.GetEnumerator();
            while (enumerator.MoveNext())
            {
                e = enumerator.Current;
                //if (ShouldOmitDrawing(e.Position.X, e.Position.Y)) continue;
                DrawElement(e.Position.X, e.Position.Y, e.GetTexture(), e.Direction, offsetX, offsetY, 1, 1, 0.02f);
            }
            LIList<Spider>.Enumerator enumeratorSpider = Simulation.simulation.spiders.GetEnumerator();
            while (enumeratorSpider.MoveNext())
            {
                e = enumeratorSpider.Current;
                //if (ShouldOmitDrawing(e.Position.X, e.Position.Y)) continue;
                DrawElement(e.Position.X, e.Position.Y, e.GetTexture(), e.Direction, offsetX, offsetY, 1, 1, 0.02f);
            }
            LIList<Food>.Enumerator enumeratorFood = Simulation.simulation.food.GetEnumerator();
            while (enumeratorFood.MoveNext())
            {
                f = enumeratorFood.Current;
                //if (ShouldOmitDrawing(f.Position.X, f.Position.Y)) continue;
                DrawElement(f.Position.X, f.Position.Y, f.GetTexture(), Dir.N, offsetX, offsetY, 1, 1, 0.015f);
            }

            e = Simulation.simulation.queen;
            if (e != null)// && !ShouldOmitDrawing(e.Position.X, e.Position.Y))
                DrawElement(e.Position.X, e.Position.Y, e.GetTexture(), e.Direction, offsetX, offsetY, 1, 1, 0.025f);
            
            //deszcz
            Rain rain = Simulation.simulation.rain;
            if (rain != null)
            {
                DrawElement(rain.Position.X, rain.Position.Y, rain.GetTexture(), Dir.N, offsetX, offsetY, AntHillConfig.rainWidth, AntHillConfig.rainWidth, 1.0f);                
            }
        }        
        class VertexData
        {                       
            public float []vertex = new float[3 * 4];
            public float []uv = new float[2 * 4];
            public UInt16[] indices = new UInt16[4];            
            public IntPtr []intPointers = new IntPtr[3];
            public GCHandle[] handles = new GCHandle[3];
            public VertexData()
            {
                handles[0] = GCHandle.Alloc(vertex, GCHandleType.Pinned);
                handles[1] = GCHandle.Alloc(uv, GCHandleType.Pinned);
                handles[2] = GCHandle.Alloc(indices, GCHandleType.Pinned);
                for (int i = 0; i < 3; i++)
                    intPointers[i] = handles[i].AddrOfPinnedObject();
            }
            ~VertexData()
            {
                for (int i = 0; i < 3; i++)
                    handles[i].Free();
            }
        }
        private VertexData vertexData = new VertexData();
        private void InitDrawing()
        {
            lookAtAngleY = lookAtAngleX = 0;

            vertexData.vertex[2] = 0.0f;
            vertexData.vertex[5] = 0.0f;
            vertexData.vertex[8] = 0.0f;
            vertexData.vertex[11] = 0.0f;
            vertexData.indices[0] = 0;
            vertexData.indices[1] = 1;
            vertexData.indices[2] = 2;
            vertexData.indices[3] = 3;

            Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, vertexData.vertex);
            Gl.glTexCoordPointer(2, Gl.GL_FLOAT, 0, vertexData.uv);
            Gl.glEnable(Gl.GL_VERTEX_ARRAY);
            Gl.glEnable(Gl.GL_TEXTURE_COORD_ARRAY);
            startingTick = Environment.TickCount;
        }
        int startingTick;
        private void DrawElement(int x, int y, int texture, Dir direction, float moveX, float moveY, int width, int height, float z)
        {
            z = -z;
            width--;
            height--;
            //Gl.glPushMatrix();
            //Gl.glTranslatef(x + moveX, y + moveY, 0);            
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);
            //Gl.glBegin(Gl.GL_TRIANGLE_FAN);            
            vertexData.vertex[0] = -0.5f + x + moveX;
            vertexData.vertex[1] = -0.5f + y + moveY;
            vertexData.vertex[2] = z;            
            vertexData.vertex[3] = 0.5f + x + moveX + width;
            vertexData.vertex[4] = -0.5f + y + moveY;
            vertexData.vertex[5] = z;
            vertexData.vertex[6] = 0.5f + x + moveX + width;
            vertexData.vertex[7] = 0.5f + y + moveY + height;
            vertexData.vertex[8] = z;                        
            vertexData.vertex[9] = -0.5f + x + moveX;
            vertexData.vertex[10] = 0.5f + y + moveY + height;
            vertexData.vertex[11] = z;
            switch (direction)
            {
                case Dir.N:
                    vertexData.uv[0] = 0; vertexData.uv[1] = 0;
                    vertexData.uv[2] = 1; vertexData.uv[3] = 0;
                    vertexData.uv[4] = 1; vertexData.uv[5] = 1;
                    vertexData.uv[6] = 0; vertexData.uv[7] = 1;
                    break;
                case Dir.E:
                    vertexData.uv[2] = 0; vertexData.uv[3] = 0;
                    vertexData.uv[4] = 1; vertexData.uv[5] = 0;
                    vertexData.uv[6] = 1; vertexData.uv[7] = 1;
                    vertexData.uv[0] = 0; vertexData.uv[1] = 1;
                    break;
                case Dir.S:
                    vertexData.uv[4] = 0; vertexData.uv[5] = 0;
                    vertexData.uv[6] = 1; vertexData.uv[7] = 0;
                    vertexData.uv[0] = 1; vertexData.uv[1] = 1;
                    vertexData.uv[2] = 0; vertexData.uv[3] = 1;
                    break;
                case Dir.W:
                    vertexData.uv[6] = 0; vertexData.uv[7] = 0;
                    vertexData.uv[0] = 1; vertexData.uv[1] = 0;
                    vertexData.uv[2] = 1; vertexData.uv[3] = 1;
                    vertexData.uv[4] = 0; vertexData.uv[5] = 1;
                    break;
            }

            //Gl.glPushMatrix();            
            if (magicCheckBox.Checked)
            {
                Gl.glPushMatrix();
                float f = (float)(Environment.TickCount - startingTick) * 0.00001f;
                Gl.glRotatef(f * (x + Simulation.simulation.Map.Width * y), 1, 1, 1);
            }
            Gl.glDrawElements(Gl.GL_TRIANGLE_FAN,4 ,Gl.GL_UNSIGNED_SHORT, vertexData.intPointers[2]);
            if (magicCheckBox.Checked)            
                Gl.glPopMatrix();            
        }                

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            offsetX = -((float)hScrollBar1.Value / 10);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            offsetY = -((float)vScrollBar1.Value / 10); 
        }


        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mousePos = e.Location;
                scrolling = true;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                mousePos = e.Location;
                rotating = true;
            }
        }        

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                scrolling = false;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                rotating = false;
            }
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (scrolling)
            {
                int hVal = hScrollBar1.Value - (e.X - mousePos.X);
                int vVal = vScrollBar1.Value - (e.Y - mousePos.Y);
                hScrollBar1.Value = Math.Min(hScrollBar1.Maximum - hScrollBar1.LargeChange, Math.Max(0, hVal));
                vScrollBar1.Value = Math.Min(vScrollBar1.Maximum - vScrollBar1.LargeChange, Math.Max(0, vVal));
                offsetX = -((float)hScrollBar1.Value / 10);
                offsetY = -((float)vScrollBar1.Value / 10);
                openGLControl.Invalidate();
                mousePos = e.Location;
            }
            else if (rotating)
            {
                lookAtAngleX += ((double)(e.Y - mousePos.Y)) * 0.1;
                lookAtAngleY -= ((double)(e.X - mousePos.X)) * 0.1;
                openGLControl.Invalidate();
                mousePos = e.Location;
            }
        }

        private void openGLControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                lookAtAngleX -= 2;
                openGLControl.Invalidate();
            }
            else if (e.KeyCode == Keys.S)
            {
                lookAtAngleX += 2;
                openGLControl.Invalidate();
            }
            else if (e.KeyCode == Keys.A)
            {
                lookAtAngleY += 2;
                openGLControl.Invalidate();
            }
            else if (e.KeyCode == Keys.D)
            {
                lookAtAngleY -= 2;
                openGLControl.Invalidate();
            }
        }
    }
}
