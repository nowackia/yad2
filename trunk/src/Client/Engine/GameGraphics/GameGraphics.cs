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
using Yad.Engine.GameGraphics.Renderes.Client;
using Yad.Properties;

namespace Yad.Engine.Client {
	static partial class GameGraphics {

		#region events
		public static event EventHandler GameGraphicsChanged;
		#endregion

		#region private members
		static GameLogic gameLogic;

		const float mapDepth = 0.0f;
		const float slabDepth = 0.1f;
		const float buildingDepth = 0.2f;
		const float unitDepth = 0.3f;
		const float selectionDepth = 0.4f;
		const float fogOfWarDepth = 0.5f;
		static RectangleF defaultUV = new RectangleF(0, 0, 1, 1);
		const short pictureOffset = 100;
		const short textureOffset = 200;
		const short turretOffset = 300;
		const float oneThird = 1.0f / 3.0f;
		const float oneFourth = 0.25f;
		const float oneFifth = 0.2f;
		const float oneEight = 0.125f;

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

			minimumZoom = Math.Max((float)viewport.Width / (float)gameLogic.Simulation.Map.Width, (float)viewport.Height / (float)gameLogic.Simulation.Map.Height);

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

			if (offset.Y > gameLogic.Simulation.Map.Height - mapClip.Height / 2.0f) {
				offset.Y = gameLogic.Simulation.Map.Height - mapClip.Height / 2.0f;
			}
		}

		private static void UpdateOffsetX() {
			if (offset.X < mapClip.Width / 2.0f) {
				offset.X = mapClip.Width / 2.0f;
			}

			if (offset.X > gameLogic.Simulation.Map.Width - mapClip.Width / 2.0f) {
				offset.X = gameLogic.Simulation.Map.Width - mapClip.Width / 2.0f;
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
			Create32bTexture(2, LoadBitmap(Path.Combine(Settings.Default.UI, "Selection.png")));

			GameSettings gameSettings = simulation.GameSettingsWrapper.GameSettings;
			foreach (AmmoData o in gameSettings.AmmosData.AmmoDataCollection) {

			}

			foreach (BuildingData o in gameSettings.BuildingsData.BuildingDataCollection) {
				String file = Path.Combine(Settings.Default.Structures, o.Name + ".png");
				Bitmap texture = LoadBitmap(file);
				Create32bTexture(o.TypeID + textureOffset, texture);
			}

			foreach (RaceData o in gameSettings.RacesData.RaceDataCollection) {
				String file = Path.Combine(Settings.Default.Pictures, o.Name + ".png");
				Bitmap texture = LoadBitmap(file);
				Create32bTexture(o.TypeID + pictureOffset, texture);
			}

			foreach (UnitHarvesterData o in gameSettings.UnitHarvestersData.UnitHarvesterDataCollection) {
				String file = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap texture = LoadBitmap(file);
				Create32bTexture(o.TypeID + textureOffset, texture);
			}

			foreach (UnitMCVData o in gameSettings.UnitMCVsData.UnitMCVDataCollection) {
				String file = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap texture = LoadBitmap(file);
				Create32bTexture(o.TypeID + textureOffset, texture);
			}

			foreach (UnitSandwormData o in gameSettings.UnitSandwormsData.UnitSandwormDataCollection) {
				String file = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap texture = LoadBitmap(file);
				Create32bTexture(o.TypeID + textureOffset, texture);
			}

			foreach (UnitTankData o in gameSettings.UnitTanksData.UnitTankDataCollection) {
				String file = Path.Combine(Settings.Default.Units, o.Name + "Base.png");
				Bitmap texture = LoadBitmap(file);
				Create32bTexture(o.TypeID + textureOffset, texture);
			}

			foreach (UnitTrooperData o in gameSettings.UnitTroopersData.UnitTrooperDataCollection) {
				String file = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap bmp = LoadBitmap(file);
				Create32bTexture(o.TypeID + textureOffset, bmp);
			}
		}

		private static int Correct(int size) {
			double y = Math.Ceiling(Math.Log(size, 2));
			return (int)Math.Pow(2, y);
		}

		private static Bitmap LoadBitmap(String path) {
			Bitmap tmp = new Bitmap(path);
			Bitmap bmp = new Bitmap(Correct(tmp.Width), Correct(tmp.Height), PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(bmp);
			g.DrawImage(tmp, 0, 0, bmp.Width, bmp.Height);
			g.Dispose();
			return bmp;
		}

		#endregion

		#region drawing
		public static void Draw() {
			Gl.glClearColor(0, 0, 0, 0);
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

			Gl.glLoadIdentity();

			//draw fogOfWar

			//Drawing map
			DrawElementFromLeftBottom(0, 0, mapDepth, gameLogic.Simulation.Map.Width, gameLogic.Simulation.Map.Height, 1, defaultUV);

			//Drawing slab's
			//TODO

			//Gl.glColor4f(1, 1, 1, (float)(signal + AntHillConfig.signalInitialAlpha) / AntHillConfig.signalHighestDensity);
			//DrawElement(x, y, (int)AHGraphics.Texture.MessageQueenInDanger, offsetX, offsetY, 1, 1, 0.01f);


			ICollection<Player> players = gameLogic.Simulation.GetPlayers();
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
			foreach (Unit u in gameLogic.SelectedUnits) {
				PointF realPos = CountRealPosition(u);
				DrawElementFromLeftBottom(realPos.X, realPos.Y, selectionDepth, 1, 1, 2, defaultUV);
			}

			Building selB = gameLogic.SelectedBuilding;
			if (selB != null) {
				DrawElementFromLeftBottom(selB.Position.X, selB.Position.Y, selectionDepth, selB.Size.X, selB.Size.Y, 2, defaultUV);
			}
		}

		private static PointF CountRealPosition(Unit u) {
			float dx = u.Position.X - u.LastPosition.X;
			float dy = u.Position.Y - u.LastPosition.Y;

			float moveProgress = (float)u.RemainingTurnsInMove / (float)u.Speed;
			if (moveProgress >= 1) {
				moveProgress = 0;
			}

			//InfoLog.WriteInfo(dx.ToString() + " " + dy.ToString()+ " " + moveProgress.ToString(), EPrefix.GameGraphics);

			float x = ((float)u.Position.X) - dx * moveProgress;
			float y = ((float)u.Position.Y) - dy * moveProgress;
			return new PointF(x, y);
		}

		private static bool Test(Direction s, Direction t) {
			return ((s & t) != 0);
		}

		private static void DrawTrooper(UnitTrooper o) {
			PointF realPos = CountRealPosition(o);
			float frame = o.RemainingTurnsInMove % 3;
			
			// >^<v
			RectangleF uv = new RectangleF(0, (frame + 1.0f) * oneThird, oneFourth, oneThird);
			Direction d = o.Direction;

			if (Test(d, Direction.East)) {

			} else if (Test(d, Direction.West)) {
				uv.X = 2 * oneFourth;
			} else if (Test(d, Direction.North)) {
				uv.X = oneFourth;
			} else if (Test(d, Direction.South)) {
				uv.X = 3 * oneFourth;
			}

			DrawElementFromLeftBottom(realPos.X, realPos.Y, unitDepth, 0.5f, 0.5f, o.TypeID + textureOffset, uv);
		}

		private static void DrawTank(UnitTank o) {
			PointF realPos = CountRealPosition(o);
			Direction d = o.Direction;
			RectangleF uv = new RectangleF(oneEight, 0, oneEight, 1);

			if (Test(d, Direction.East)) {
				if (Test(d, Direction.North)) {
					//uv.X *= 1;
				} else if (Test(d, Direction.South)) {
					uv.X *= 7.0f;
				} else {
					uv.X = 0;
				}
			} else if (Test(d, Direction.West)) {
				if (Test(d, Direction.North)) {
					uv.X *= 3.0f;
				} else if (Test(d, Direction.South)) {
					uv.X *= 5.0f;
				} else {
					uv.X *= 4.0f;
				}
			} else /* center */ {
				if (Test(d, Direction.North)) {
					uv.X *= 2.0f;
				} else {
					uv.X *= 6.0f;
				}
			}

			DrawElementFromLeftBottom(realPos.X, realPos.Y, unitDepth, 1, 1, o.TypeID + textureOffset, uv);
		}

		private static void DrawSandworm(UnitSandworm o) {
			PointF realPos = CountRealPosition(o);
			float frame = o.RemainingTurnsInMove % 5;
			RectangleF uv = new RectangleF(0, (frame + 1.0f) * oneFifth, 1, oneFifth);
			DrawElementFromLeftBottom(realPos.X, realPos.Y, unitDepth, 1, 1, o.TypeID + textureOffset, uv);
		}

		private static void DrawMCV(UnitMCV o) {
			PointF realPos = CountRealPosition(o);
			Direction d = o.Direction;
			RectangleF uv = new RectangleF(oneEight, 0, oneEight, 1);

			if (Test(d, Direction.East)) {
				if (Test(d, Direction.North)) {
					//uv.X *= 1;
				} else if (Test(d, Direction.South)) {
					uv.X *= 7.0f;
				} else {
					uv.X = 0;
				}
			} else if (Test(d, Direction.West)) {
				if (Test(d, Direction.North)) {
					uv.X *= 3.0f;
				} else if (Test(d, Direction.South)) {
					uv.X *= 5.0f;
				} else {
					uv.X *= 4.0f;
				}
			} else /* center */ {
				if (Test(d, Direction.North)) {
					uv.X *= 2.0f;
				} else {
					uv.X *= 6.0f;
				}
			}

			DrawElementFromLeftBottom(realPos.X, realPos.Y, unitDepth, 1, 1, o.TypeID + textureOffset, uv);
		}

		private static void DrawHarvester(UnitHarvester o) {
			PointF realPos = CountRealPosition(o);
			Direction d = o.Direction;
			RectangleF uv = new RectangleF(oneEight, 0, oneEight, 1);

			if (Test(d, Direction.East)) {
				if (Test(d, Direction.North)) {
					//uv.X *= 1;
				} else if (Test(d, Direction.South)) {
					uv.X *= 7.0f;
				} else {
					uv.X = 0;
				}
			} else if (Test(d, Direction.West)) {
				if (Test(d, Direction.North)) {
					uv.X *= 3.0f;
				} else if (Test(d, Direction.South)) {
					uv.X *= 5.0f;
				} else {
					uv.X *= 4.0f;
				}
			} else /* center */ {
				if (Test(d, Direction.North)) {
					uv.X *= 2.0f;
				} else {
					uv.X *= 6.0f;
				}
			}

			DrawElementFromLeftBottom(realPos.X, realPos.Y, unitDepth, 1, 1, o.TypeID + textureOffset, uv);
		}

		private static void DrawBuilding(Building o) {
			//PointF realPos = CountRealPosition(o);
			DrawElementFromLeftBottom(o.Position.X, o.Position.Y, buildingDepth, o.Size.X, o.Size.Y, o.TypeID + textureOffset, defaultUV);
		}
		#endregion

		#region public methods
		public static void InitGL(GameLogic gLogic) {
			gameLogic = gLogic;
			gameLogic.Simulation.onTurnEnd += new SimulationHandler(GameGraphics.Notify);

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
