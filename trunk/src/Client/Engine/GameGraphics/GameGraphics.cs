using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Tao.OpenGl;
using Tao.Platform.Windows;
using System.IO;
using Yad.Log;
using Yad.Board.Common;
using Yad.Log.Common;
using Yad.Engine.GameGraphics.Client;
using System.Windows.Forms;
using Yad.Engine.Common;
using Yad.Board;
using Yad.Config.Common;
using Yad.Config;
using Yad.Properties.Client;
using Yad.Engine.GameGraphics.Renderes.Client;

namespace Yad.Engine.Client {
	static partial class GameGraphics {

		#region events
		public static event EventHandler GameGraphicsChanged;
		#endregion

		#region private members
		static Simulation simulation;

		const float mapDepth = 0.0f;
		const float buildingDepth = 0.1f;
		const float unitDepth = 0.2f;
		const float fogOfWarDepth = 0.3f;
		static RectangleF defaultUV = new RectangleF(0, 0, 1, 1);
		const short pictureOffset = 100;
		const short textureOffset = 200;
		const short turretOffset = 300;

		/// <summary>
		/// SimpleOpenGLControl's size
		/// </summary>
		static System.Drawing.Size viewport = new System.Drawing.Size();

		/// <summary>
		/// Map view.
		/// </summary>
		static ClipRectangle mapClip = new ClipRectangle();

		/// <summary>
		/// Offset used for map scrolling.
		/// </summary>
		static PointF offset = new PointF(0, 0);

		/// <summary>
		/// Minimum zoom, so that there is no black area on the map.
		/// </summary>
		static float minimumZoom = 1;

		static float zoom = 1.0f, zoomStep = 3.5f;

		/// <summary>
		/// Used for drawing textures
		/// </summary>
		static VertexData vertexData = new VertexData();

		#endregion

		#region private methods

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
			Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8, bitmap.Width, bitmap.Height, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
			//Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, 4, bitmap.Width, bitmap.Height, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

			bitmap.UnlockBits(bitmapData);
		}

		private static void Create32bTexture(int id, Bitmap bitmap) {
			int width = bitmap.Width;
			int height = bitmap.Height;
			Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			//MessageBoxEx.Show(this, bitmap.PixelFormat + " " + bitmap.PhysicalDimension);
			BitmapData bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			//Gl.glEnable(Gl.GL_TEXTURE_2D);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, id);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);//_MIPMAP_NEAREST);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
			Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8, width, height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
			//Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, 4, bitmap.Width, bitmap.Height, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

			bitmap.UnlockBits(bitmapData);
		}

		private static void UpdateViewport() {
			Gl.glViewport(0, 0, viewport.Width, viewport.Height);

			minimumZoom = Math.Max((float)viewport.Width / (float)simulation.Map.Width, (float)viewport.Height / (float)simulation.Map.Height);

			UpdateZoom();
			UpdateClip();
		}

		private static void UpdateZoom() {
			if (zoom < minimumZoom)
				zoom = minimumZoom;

			mapClip.Left = -viewport.Width / zoom / 2;
			mapClip.Width = viewport.Width / zoom;
			mapClip.Bottom = -viewport.Height / zoom / 2;
			mapClip.Height = viewport.Height / zoom;

			UpdateOffsetX();
			UpdateOffsetY();
			UpdateClip();
		}

		private static void UpdateClip() {
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Gl.glOrtho(mapClip.Left, mapClip.Right, mapClip.Bottom, mapClip.Top, -1, 1);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
		}

		public static void DrawElementFromLeftBottom(float x, float y, float z, float width, float height, int texture, RectangleF uv) {
			//Gl.glPushMatrix();
			//Gl.glTranslatef(x + moveX, y + moveY, 0);

			//init texture
			//Gl.glEnable(Gl.GL_TEXTURE_2D);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);

			//init uv mapping for texture
			vertexData.uv[0] = uv.Left; vertexData.uv[1] = uv.Bottom;
			vertexData.uv[2] = uv.Right; vertexData.uv[3] = uv.Bottom;
			vertexData.uv[4] = uv.Right; vertexData.uv[5] = uv.Top;
			vertexData.uv[6] = uv.Left; vertexData.uv[7] = uv.Top;

			vertexData.vertex[0] = x - offset.X;
			vertexData.vertex[1] = y - offset.Y;
			vertexData.vertex[2] = z;
			vertexData.vertex[3] = x + width - offset.X;
			vertexData.vertex[4] = y - offset.Y;
			vertexData.vertex[5] = z;
			vertexData.vertex[6] = x + width - offset.X;
			vertexData.vertex[7] = y + height - offset.Y;
			vertexData.vertex[8] = z;
			vertexData.vertex[9] = x - offset.X;
			vertexData.vertex[10] = y + height - offset.Y;
			vertexData.vertex[11] = z;

			if (Settings.Default.UseSafeRendering) {
				Gl.glDrawElements(Gl.GL_TRIANGLE_FAN, 4, Gl.GL_UNSIGNED_SHORT, vertexData.intPointers[2]);
				return;
			}

			Gl.glBegin(Gl.GL_TRIANGLE_FAN);
			int i2 = 0, i3 = 0;
			for (int i = 0; i < 4; i++) {
				Gl.glTexCoord2f(vertexData.uv[i2], vertexData.uv[i2 + 1]);
				Gl.glVertex3f(vertexData.vertex[i3], vertexData.vertex[i3 + 1], vertexData.vertex[i3 + 2]);
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
			vertexData.vertex[0] = x + 0.5f - w2;
			vertexData.vertex[1] = y + 0.5f - h2;
			vertexData.vertex[2] = z;
			vertexData.vertex[3] = x + 0.5f + width;
			vertexData.vertex[4] = y + 0.5f - h2;
			vertexData.vertex[5] = z;
			vertexData.vertex[6] = x + 0.5f + width;
			vertexData.vertex[7] = y - 0.5f + height;
			vertexData.vertex[8] = z;
			vertexData.vertex[9] = x + 0.5f - w2;
			vertexData.vertex[10] = y + 0.5f + height;
			vertexData.vertex[11] = z;

			Gl.glDrawElements(Gl.GL_TRIANGLE_FAN, 4, Gl.GL_UNSIGNED_SHORT, vertexData.intPointers[2]);
		}

		private static void UpdateOffsetY() {
			if (offset.Y < mapClip.Height / 2.0f) {
				offset.Y = mapClip.Height / 2.0f;
			}

			if (offset.Y > simulation.Map.Height - mapClip.Height / 2.0f) {
				offset.Y = simulation.Map.Height - mapClip.Height / 2.0f;
			}
		}

		private static void UpdateOffsetX() {
			if (offset.X < mapClip.Width / 2.0f) {
				offset.X = mapClip.Width / 2.0f;
			}

			if (offset.X > simulation.Map.Width - mapClip.Width / 2.0f) {
				offset.X = simulation.Map.Width - mapClip.Width / 2.0f;
			}
		}

		private static void Notify() {
			if (GameGraphicsChanged != null) {
				GameGraphicsChanged(null, null);
			}
		}
		#endregion

		#region texture init
		public static void InitTextures(Simulation simulation) {
			Create32bTexture(1, MapTextureGenerator.GenerateBitmap(simulation.Map));

			GameSettings gameSettings = simulation.GameSettingsWrapper.GameSettings;
			foreach (AmmoData o in gameSettings.AmmosData.AmmoDataCollection) {

			}

			foreach (BuildingData o in gameSettings.BuildingsData.BuildingDataCollection) {
				String file = Path.Combine(Settings.Default.Structures, o.Name + ".png");
				Bitmap texture = new Bitmap(file);
				Create32bTexture(o.TypeID + textureOffset, texture);
			}

			foreach (RaceData o in gameSettings.RacesData.RaceDataCollection) {
				String file = Path.Combine(Settings.Default.Pictures, o.Name + ".png");
				Bitmap texture = new Bitmap(file);
				Create32bTexture(o.TypeID + pictureOffset, texture);
			}

			foreach (UnitHarvesterData o in gameSettings.UnitHarvestersData.UnitHarvesterDataCollection) {
				String file = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap texture = new Bitmap(file);
				Create32bTexture(o.TypeID + textureOffset, texture);
			}

			foreach (UnitMCVData o in gameSettings.UnitMCVsData.UnitMCVDataCollection) {
				String file = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap texture = new Bitmap(file);
				Create32bTexture(o.TypeID + textureOffset, texture);
			}

			foreach (UnitSandwormData o in gameSettings.UnitSandwormsData.UnitSandwormDataCollection) {
				String file = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap texture = new Bitmap(file);
				Create32bTexture(o.TypeID + textureOffset, texture);
			}

			foreach (UnitTankData o in gameSettings.UnitTanksData.UnitTankDataCollection) {
				String file = Path.Combine(Settings.Default.Units, o.Name + "Base.png");
				Bitmap texture = new Bitmap(file);
				Create32bTexture(o.TypeID + textureOffset, texture);
			}

			foreach (UnitTrooperData o in gameSettings.UnitTroopersData.UnitTrooperDataCollection) {
				String file = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap texture = new Bitmap(file);
				Bitmap bmp = new Bitmap(Correct(texture.Width), Correct(texture.Height), PixelFormat.Format32bppArgb);
				Graphics g = Graphics.FromImage(bmp);
				g.DrawImage(texture, 0, 0, bmp.Width, bmp.Height);
				g.Dispose();
				Create32bTexture(o.TypeID + textureOffset, bmp);
			}
		}

		private static int Correct(int size) {
			double y = Math.Ceiling(Math.Log(size, 2));
			return (int)Math.Pow(2, y);
		}

		#endregion

		#region drawing
		public static void Draw() {
			Gl.glClearColor(0, 0, 0, 0);
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

			Gl.glLoadIdentity();

			//draw fogOfWar

			//Drawing map
			DrawElementFromLeftBottom(0, 0, mapDepth, simulation.Map.Width, simulation.Map.Height, 1, defaultUV);

			//Gl.glColor4f(1, 1, 1, (float)(signal + AntHillConfig.signalInitialAlpha) / AntHillConfig.signalHighestDensity);
			//DrawElement(x, y, (int)AHGraphics.Texture.MessageQueenInDanger, offsetX, offsetY, 1, 1, 0.01f);
			


			ICollection<Player> players = simulation.GetPlayers();
			foreach (Player p in players) {
				List<Unit> units = p.GetAllUnits();
				foreach (Unit u in units) {
					switch (u.BoardObjectClass) {
						case BoardObjectClass.UnitHarvester:
							DrawHarvester((UnitHarvester)u);
							break;
						case BoardObjectClass.UnitMCV:
							DrawMCV((UnitMCV)u);
							break;
						case BoardObjectClass.UnitSandworm:
							DrawSandworm((UnitSandworm)u);
							break;
						case BoardObjectClass.UnitTank:
							DrawTank((UnitTank)u);
							break;
						case BoardObjectClass.UnitTrooper:
							DrawTrooper((UnitTrooper)u);
							break;
					}
				}

				List<Building> buildings = p.GetAllBuildings();
				foreach (Building b in buildings) {
					DrawBuilding(b);
				}
			}
		}

		private static void DrawTrooper(UnitTrooper unitTrooper) {
			DrawElementFromLeftBottom(unitTrooper.Position.X, unitTrooper.Position.Y, unitDepth, 0.5f, 0.5f, unitTrooper.TypeID + textureOffset, new RectangleF(0, 0, 0.25f, 1/3.0f));
		}

		private static void DrawTank(UnitTank unitTank) {
			DrawElementFromLeftBottom(unitTank.Position.X, unitTank.Position.Y, unitDepth, 1, 1, unitTank.TypeID + textureOffset, new RectangleF(0,0,1.0f/8.0f ,1));
		}

		private static void DrawSandworm(UnitSandworm unitSandworm) {
			DrawElementFromLeftBottom(unitSandworm.Position.X, unitSandworm.Position.Y, unitDepth, 1, 1, unitSandworm.TypeID + textureOffset, defaultUV);
		}

		private static void DrawMCV(UnitMCV unitMCV) {
			DrawElementFromLeftBottom(unitMCV.Position.X, unitMCV.Position.Y, unitDepth, 1, 1, unitMCV.TypeID + textureOffset, defaultUV);
		}

		private static void DrawHarvester(UnitHarvester unitHarvester) {
			DrawElementFromLeftBottom(unitHarvester.Position.X, unitHarvester.Position.Y, unitDepth, 1, 1, unitHarvester.TypeID + textureOffset, defaultUV);
		}

		private static void DrawBuilding(Building b) {
			DrawElementFromLeftBottom(b.Position.X, b.Position.Y, buildingDepth, b.Size.X, b.Size.Y, b.TypeID + textureOffset, defaultUV);
		}
		#endregion

		#region public methods
		public static void InitGL(Simulation sim) {
			simulation = sim;
			simulation.onTurnEnd += new SimulationHandler(GameGraphics.Notify);

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

			//MessageBoxEx.Show(this, Gl.glGetString(Gl.GL_VERSION));
		}

		public static void Zoom(int zoomDiff) {
			zoom += zoomStep * zoomDiff;
			if (zoom <= zoomStep)
				zoom = zoomStep;

			//InfoLog.WriteInfo("Zooming: " + zoom, EPrefix.GameGraphics);

			UpdateViewport();

			Notify();
		}

		public static void OffsetX(float amount) {
			offset.X += amount;

			UpdateOffsetX();

			//InfoLog.WriteInfo("TranslatingX: " + mapClip.X, EPrefix.GameGraphics);

			Notify();
		}

		public static void OffsetY(float amount) {
			offset.Y += amount;

			UpdateOffsetY();
			//InfoLog.WriteInfo("TranslatingY: " + mapClip.Y, EPrefix.GameGraphics);

			Notify();
		}

		public static void SetViewSize(int width, int height) {
			viewport.Width = width;
			viewport.Height = height;

			UpdateViewport();
		}
		#endregion

		// translates screen mose coordinates to position on the map
		public static Position TranslateMousePosition(Point p)
		{
			Position pn = new Position(
			(short)(((float)p.X) / zoom + offset.X + mapClip.X),
			(short)(((float)(viewport.Height - p.Y - 1)) / zoom + offset.Y + mapClip.Y));
			return pn;
		}
	}
}
