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

namespace Client.Engine.GameGraphics {
	class GameGraphics {
		private static GameGraphics gameGraphics = null;

		public static GameGraphics GetInstance() {
			if (gameGraphics == null) {
				gameGraphics = new GameGraphics();
			}
			return gameGraphics;
		}

		public event EventHandler GameGraphicsChanged;

		/// <summary>
		/// Map dimension (in tiles)
		/// </summary>
		int mapWidth, mapHeight;

		/// <summary>
		/// Zoomed map dimensions
		/// </summary>
		float mapZoomedWidth = 1, mapZoomedHeight = 1;

		/// <summary>
		/// SimpleOpenGLControl's size
		/// </summary>
		int viewWidth = 1, viewHeight = 1;

		/// <summary>
		/// Rotation angles
		/// </summary>
		float lookAtAngleX, lookAtAngleY;

		/// <summary>
		/// Map scrolling offset
		/// </summary>
		float offsetX, offsetY;

		/// <summary>
		/// Biggest dimension of the map
		/// </summary>
		int maxMagnitude = 1;

		float zoom = 1.0f, zoomStep = 0.1f;

		/// <summary>
		/// Used for drawing textures
		/// </summary>
		VertexData vertexData = new VertexData();

		private GameGraphics() {
			//this.InitGL();
		}

		public void InitGL() {
			Gl.glEnable(Gl.GL_TEXTURE_2D);                                      // Enable Texture Mapping
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glShadeModel(Gl.GL_SMOOTH);                                      // Enable Smooth Shading
			Gl.glClearColor(0, 0, 0, 0);                                     // Black Background
			Gl.glClearDepth(1);                                                 // Depth Buffer Setup
			Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LEQUAL);                                       // The Type Of Depth Testing To Do                        
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

			UpdateViewport();

			lookAtAngleY = lookAtAngleX = 0;
			offsetX = - mapWidth / 2 + 0.5f;
			offsetY = - mapHeight / 2 + 0.5f;

			vertexData.vertex[2] = 0.0f;
			vertexData.vertex[5] = 0.0f;
			vertexData.vertex[8] = 0.0f;
			vertexData.vertex[11] = 0.0f;
			vertexData.indices[0] = 0;
			vertexData.indices[1] = 1;
			vertexData.indices[2] = 2;
			vertexData.indices[3] = 3;

			//UV mapping doesn't change
			vertexData.uv[0] = 0; vertexData.uv[1] = 0;
			vertexData.uv[2] = 1; vertexData.uv[3] = 0;
			vertexData.uv[4] = 1; vertexData.uv[5] = 1;
			vertexData.uv[6] = 0; vertexData.uv[7] = 1;

			Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, vertexData.vertex);
			Gl.glTexCoordPointer(2, Gl.GL_FLOAT, 0, vertexData.uv);
			Gl.glEnable(Gl.GL_VERTEX_ARRAY);
			Gl.glEnable(Gl.GL_TEXTURE_COORD_ARRAY);
		}

		public void InitTextures() {
			//TODO:
			//For all units from config file:
			// getID, getTexturePath, create32btexture

			// Create32bTexture(Texture.Indoor, Path.Combine(Resources.GraphicsPath, Resources.indoorTileBmp));
			Bitmap map = new Bitmap(32*64, 32*32, PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(map);
			g.Clear(Color.Yellow);
			g.FillRectangle(Brushes.Green, 0, 0, 32, 32);
            g.FillRectangle(Brushes.Blue, 128, 128, 32, 32);
            g.FillRectangle(Brushes.Red, 256 + 64, 128 + 96, 32, 32);
			g.Dispose();
			Create32bTexture(1, map);
		}

		/// <summary>
		/// Creates 32-bit texture using bitmap "filename" and binds it to "id" so that
		/// it can be used by OpenGL to render objects.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="filename"></param>
		private void Create32bTexture(int id, string filename) {
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

		private void Create32bTexture(int id, Bitmap bitmap) {
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

		private void UpdateViewport() {
            //float m = (float)viewHeight / (float)viewWidth;
            //float n = (float)mapWidth / (float)mapHeight;
			Gl.glViewport(0, 0, this.viewWidth, this.viewHeight);
			Gl.glMatrixMode(Gl.GL_PROJECTION);                                  // Select The Projection Matrix
			Gl.glLoadIdentity();                                                // Reset The Projection Matrix
			Gl.glOrtho(-(0.5f * mapWidth), (0.5f * mapWidth), (0.5f * mapHeight), -(0.5f * mapHeight), -100, 100);
			//Glu.gluPerspective(60.0f, 1.0f, 1.0f, 10000.0f);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);                                   // Select The Modelview Matrix
			Gl.glLoadIdentity();                                                // Reset The Modelview Matrix            
		}

		public void Zoom(int zoomDiff) {
			zoom += zoomStep * zoomDiff;
			if (zoom <= zoomStep)
				zoom = zoomStep;

			this.mapZoomedWidth = 1.0f + (float)(mapWidth - 1) * zoom;
			this.mapZoomedHeight = 1.0f + (float)(mapHeight - 1) * zoom;

			InfoLog.WriteInfo("Zooming: " + zoom + " " + mapZoomedWidth + " " + mapZoomedHeight, EPrefix.GameGraphics); 

			Notify();
		}

		//TODO: may be outdated
		private bool ShouldOmitDrawing(int x, int y) {
			return ((x + offsetX + 1 < -mapZoomedWidth * 0.5f) ||
					   (x + offsetX - 1 > mapZoomedWidth * 0.5f) ||
					   (y + offsetY + 1 < -mapZoomedHeight * 0.5f) ||
					   (y + offsetY - 1 > mapZoomedHeight * 0.5f));
		}

		public void Draw() {
			Gl.glClearColor(0, 0, 0, 0);
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

			Gl.glLoadIdentity();
			/*
			Glu.gluLookAt(0, zoom * 10.0f, zoom * -20.0f,
							0, 0, 0,
							0, -1, 0);//0.5f * Math.Sqrt(2), 0.5f * Math.Sqrt(2));
			Gl.glRotated(lookAtAngleX, 1.0d, 0.0d, 0.0d);
			Gl.glRotated(lookAtAngleY, 0.0d, 1.0d, 0.0d);
			*/
			Gl.glColor4f(1, 1, 1, 1);
			DrawElement(0, 0, 1, offsetX, offsetY, mapWidth, mapHeight, 0.0f);
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

		private void DrawElement(int x, int y, int texture, float offsetx, float offsety, int width, int height, float z) {
			z = -z;
			width--;
			height--;
			//Gl.glPushMatrix();
			//Gl.glTranslatef(x + moveX, y + moveY, 0);            
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);
			//Gl.glBegin(Gl.GL_TRIANGLE_FAN);            
			vertexData.vertex[0] = -0.5f + x + offsetx;
			vertexData.vertex[1] = -0.5f + y + offsety;
			vertexData.vertex[2] = z;
			vertexData.vertex[3] = 0.5f + x + offsetx + width;
			vertexData.vertex[4] = -0.5f + y + offsety;
			vertexData.vertex[5] = z;
			vertexData.vertex[6] = 0.5f + x + offsetx + width;
			vertexData.vertex[7] = 0.5f + y + offsety + height;
			vertexData.vertex[8] = z;
			vertexData.vertex[9] = -0.5f + x + offsetx;
			vertexData.vertex[10] = 0.5f + y + offsety + height;
			vertexData.vertex[11] = z;

			Gl.glDrawElements(Gl.GL_TRIANGLE_FAN, 4, Gl.GL_UNSIGNED_SHORT, vertexData.intPointers[2]);
		}

		public void RotateX(float amount) {
			lookAtAngleX += amount;

			Notify();
		}

		public void RotateY(float amount) {
			lookAtAngleY += amount;

			Notify();
		}

		public void RotationReset() {
			lookAtAngleX = lookAtAngleY = 0;

			Notify();
		}

		public void TranslateX(float amount) {
			offsetX += amount;

			Notify();
		}

		public void TranslateY(float amount) {
			offsetY += amount;

			Notify();
		}

		public void TranslationReset() {
			offsetX = offsetY = 0;

			Notify();
		}

		public void SetMapSize(int width, int height) {
			Console.Out.WriteLine("Setting map size");

			this.mapWidth = width;
			this.mapHeight = height;

			maxMagnitude = Math.Max(this.mapWidth, this.mapHeight);
		}

		public void SetViewSize(int width, int height) {
			this.viewWidth = width;
			this.viewHeight = height;

			UpdateViewport();
		}

		private void Notify() {
			if (GameGraphicsChanged != null)
				GameGraphicsChanged(null, null);
		}
	}
}
