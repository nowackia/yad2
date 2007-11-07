using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Tao.OpenGl;
using Tao.Platform.Windows;
using System.IO;
using Client.Properties;
using Client.Log;
using Client.Engine.GameGraphics;

namespace Client.Engine.GameGraphics {
    static class GameGraphics {
        public static event EventHandler GameGraphicsChanged;

        /// <summary>
        /// Map dimension (in tiles)
        /// </summary>
        static Size map = new Size();

        /// <summary>
        /// SimpleOpenGLControl's size
        /// </summary>
		static Size viewport = new Size();

        /// <summary>
        /// Map view.
        /// </summary>
		static ClipRectangle mapClip = new ClipRectangle();

        /// <summary>
        /// Minimum zoom, so that there is no black area on the map.
        /// </summary>
		static float minimumZoom = 1;

		static float zoom = 1.0f, zoomStep = 8.5f;

        /// <summary>
        /// Used for drawing textures
        /// </summary>
		static VertexData vertexData = new VertexData();

		public static void InitGL() {
            Gl.glEnable(Gl.GL_TEXTURE_2D);                                      // Enable Texture Mapping
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glShadeModel(Gl.GL_SMOOTH);                                      // Enable Smooth Shading
            Gl.glClearColor(0, 0, 0, 0);                                     // Black Background
            Gl.glClearDepth(1);                                                 // Depth Buffer Setup
            Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
            Gl.glDepthFunc(Gl.GL_LEQUAL);                                       // The Type Of Depth Testing To Do                        
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            vertexData.indices[0] = 0;
            vertexData.indices[1] = 1;
            vertexData.indices[2] = 2;
            vertexData.indices[3] = 3;

            Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, vertexData.vertex);
            Gl.glTexCoordPointer(2, Gl.GL_FLOAT, 0, vertexData.uv);
            Gl.glEnable(Gl.GL_VERTEX_ARRAY);
            Gl.glEnable(Gl.GL_TEXTURE_COORD_ARRAY);

            UpdateViewport();
        }

		public static void InitTextures() {
            //TODO:
            //For all units from config file:
            // getID, getTexturePath, create32btexture

            // Create32bTexture(Texture.Indoor, Path.Combine(Resources.GraphicsPath, Resources.indoorTileBmp));
            MapHolder mh = new MapHolder("Resources/Maps/test.map");
            Create32bTexture(1, mh.ToBitmap());
        }

        /// <summary>
        /// Creates 32-bit texture using bitmap "filename" and binds it to "id" so that
        /// it can be used by OpenGL to render objects.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="filename"></param>
		private static void Create32bTexture(int id, string filename) {
            Bitmap bitmap = new Bitmap(filename);
            Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, id);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);//_MIPMAP_NEAREST);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8, bitmap.Width, bitmap.Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
            //Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, 4, bitmap.Width, bitmap.Height, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

            bitmap.UnlockBits(bitmapData);
        }

		private static void Create32bTexture(int id, Bitmap bitmap) {
            int width = bitmap.Width;
            int height = bitmap.Height;
            Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, id);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);//_MIPMAP_NEAREST);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8, width, height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
            //Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, 4, bitmap.Width, bitmap.Height, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

            bitmap.UnlockBits(bitmapData);
        }

		private static void UpdateViewport() {
            minimumZoom = Math.Max((float)viewport.Width / map.Width, (float)viewport.Height / map.Height);
            if (zoom < minimumZoom)
                zoom = minimumZoom;
            /*
            mapClip.Left = -(0.5f * viewWidth) / zoom;
            mapClip.Right = (0.5f * viewWidth) / zoom;
            mapClip.Bottom = -(0.5f * viewHeight) / zoom;
            mapClip.Top = (0.5f * viewHeight / zoom);
            */
            mapClip.Left = 0;
            mapClip.Width = viewport.Width / zoom;
            mapClip.Bottom = 0;
            mapClip.Height = viewport.Height / zoom;

            Gl.glViewport(0, 0, viewport.Width, viewport.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glOrtho(mapClip.Left, mapClip.Right, mapClip.Bottom, mapClip.Top, -1, 1);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }

		public static void Draw() {
            Gl.glClearColor(0, 0, 0, 0);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glLoadIdentity();

            Gl.glColor4f(1, 1, 1, 1);
            DrawElementFromLeftBottom(0, 0, 0, map.Width, map.Height, 1, new RectangleF(0,0,1,1));

            /*
            // Draw map first
            for (int x = 0; x < map.Width; x++) {
                for (int y = 0; y < map.Height; y++) {
                    //if (ShouldOmitDrawing(x, y)) continue;
                    DrawElement(x, y, map.GetTile(x, y).GetTexture(), offsetX, offsetY, 1, 1, 0.0f);
                }
            }
            Gl.glColor4f(1, 1, 1, (float)(signal + AntHillConfig.signalInitialAlpha) / AntHillConfig.signalHighestDensity);
            DrawElement(x, y, (int)AHGraphics.Texture.MessageQueenInDanger, offsetX, offsetY, 1, 1, 0.01f);
            Gl.glColor4f(1, 1, 1, 1);
            DrawElement(rain.Position.X, rain.Position.Y, rain.GetTexture(), offsetX, offsetY, AntHillConfig.rainWidth, AntHillConfig.rainWidth, 1.0f);
             */
        }

		private static void DrawElementFromLeftBottom(float x, float y, float z, float width, float height, int texture, RectangleF uv) {
            //Gl.glPushMatrix();
            //Gl.glTranslatef(x + moveX, y + moveY, 0);

			//init texture
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);

			//init uv mapping for texture
			vertexData.uv[0] = uv.Left; vertexData.uv[1] = uv.Bottom;
            vertexData.uv[2] = uv.Right; vertexData.uv[3] = uv.Bottom;
            vertexData.uv[4] = uv.Right; vertexData.uv[5] = uv.Top;
            vertexData.uv[6] = uv.Left; vertexData.uv[7] = uv.Top;

            vertexData.vertex[0] = x - mapClip.Left;
            vertexData.vertex[1] = y - mapClip.Bottom;
            vertexData.vertex[2] = z;
            vertexData.vertex[3] = x + width - mapClip.Left;
			vertexData.vertex[4] = y - mapClip.Bottom;
            vertexData.vertex[5] = z;
			vertexData.vertex[6] = x + width - mapClip.Left;
			vertexData.vertex[7] = y + height - mapClip.Bottom;
            vertexData.vertex[8] = z;
			vertexData.vertex[9] = x - mapClip.Left;
			vertexData.vertex[10] = y + height - mapClip.Bottom;
            vertexData.vertex[11] = z;

			if (Properties.Settings.Default.UseSafeRendering) {
				Gl.glDrawElements(Gl.GL_TRIANGLE_FAN, 4, Gl.GL_UNSIGNED_SHORT, vertexData.intPointers[2]);
				return;
			}

			Gl.glBegin(Gl.GL_TRIANGLE_FAN);
			int i2=0, i3=0;
			for (int i = 0; i < 4; i++) {
				Gl.glTexCoord2f(vertexData.uv[i2], vertexData.uv[i2 + 1]);
				Gl.glVertex3f(vertexData.vertex[i3], vertexData.vertex[i3+1], vertexData.vertex[i3+2]);
				i2 += 2;
				i3 += 3;
			}
			Gl.glEnd();
        }

		private static void DrawElementFromMiddle(float x, float y, float z, float width, float height, int texture) {
            float w2 = width / 2.0f;
            float h2 = height / 2.0f;
            //Gl.glPushMatrix();
            //Gl.glTranslatef(x + moveX, y + moveY, 0);            
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);
            //Gl.glBegin(Gl.GL_TRIANGLE_FAN);            
			vertexData.vertex[0] = x + 0.5f - w2 - mapClip.Left;
			vertexData.vertex[1] = y + 0.5f - h2 - mapClip.Bottom;
            vertexData.vertex[2] = z;
			vertexData.vertex[3] = x + 0.5f - mapClip.Left + width;
			vertexData.vertex[4] = y + 0.5f - h2 - mapClip.Bottom;
            vertexData.vertex[5] = z;
			vertexData.vertex[6] = x + 0.5f - mapClip.Left + width;
			vertexData.vertex[7] = y - 0.5f - mapClip.Bottom + height;
            vertexData.vertex[8] = z;
			vertexData.vertex[9] = x + 0.5f - w2 - mapClip.Left;
			vertexData.vertex[10] = y + 0.5f - mapClip.Bottom + height;
            vertexData.vertex[11] = z;

            Gl.glDrawElements(Gl.GL_TRIANGLE_FAN, 4, Gl.GL_UNSIGNED_SHORT, vertexData.intPointers[2]);
        }

		public static void Zoom(int zoomDiff) {
            zoom += zoomStep * zoomDiff;
            if (zoom <= zoomStep)
                zoom = zoomStep;

            InfoLog.WriteInfo("Zooming: " + zoom, EPrefix.GameGraphics);

            UpdateViewport();

            Notify();
        }

		public static void TranslateX(float amount) {
            mapClip.X += amount;

            if (mapClip.X < 0) {
                mapClip.X = 0;
            }

            if (mapClip.Right > map.Width) {
                mapClip.Right = map.Width;
            }

            InfoLog.WriteInfo("TranslatingX: " + mapClip.X, EPrefix.GameGraphics);

            Notify();
        }

		public static void TranslateY(float amount) {
            mapClip.Y += amount;

            if (mapClip.Bottom < 0) {
                mapClip.Bottom = 0;
            }

            if (mapClip.Top > map.Height) {
                mapClip.Top = map.Height;
            }

            InfoLog.WriteInfo("TranslatingY: " + mapClip.Y, EPrefix.GameGraphics);

            Notify();
        }

		public static void TranslationReset() {
            mapClip.X = 0;
            mapClip.Y = mapClip.Height;

            Notify();
        }

		public static void SetMapSize(int width, int height) {
            Console.Out.WriteLine("Setting map size");

            map.Width = width;
            map.Height = height;

            minimumZoom = Math.Max(map.Width, map.Height);
        }

		public static void SetViewSize(int width, int height) {
            viewport.Width = width;
            viewport.Height = height;

            UpdateViewport();
        }

		static void Notify() {
            //if (GameGraphicsChanged != null)
            GameGraphicsChanged(null, null);
        }
    }
}
