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
using Yad.Properties;
using Yad.Properties.Client;
using System.Collections;
using Yad.Net.Common;
using Yad.Net.Client;
using Yad.UI.Client;
using System.Runtime.InteropServices;

namespace Yad.Engine.Client {
	static partial class GameGraphics {

		#region events
		public static event EventHandler GameGraphicsChanged;
		#endregion

		#region private members
		static GameLogic _gameLogic;
		static GameForm _gameForm;
		static SimpleOpenGlControl _map;
		static SimpleOpenGlControl _miniMap;
		static bool init = false;

		#region z-constants
		/// <summary>
		/// Defines z-depth of an object type.
		/// </summary>
		const float _depthMap = 0.0f,
					_depthSpice = 0.1f,
					_depthSlab = 0.2f,
					_depthBuilding = 0.3f,
					_depthSandworm = 0.35f,
					_depthUnit = 0.4f,
					_depthUnitAddons = 0.5f,
					_depthAmmos = 0.55f,
					_depthSelection = 0.6f,
					_depthFogOfWar = 0.7f,
					_depthMouseSelection = 0.8f,
					_depthPlacingBuilding = 0.9f;
		#endregion

		static RectangleF _defaultUV = new RectangleF(0, 0, 1, 1);

		#region texture offsets
		/// <summary>
		/// Used for defining textures,
		/// ie. a tank's textures:
		/// picture = tankID + _pictureOffset;
		/// texture = tankID + _textureOffset;
		/// turret = tankID + _turretOffset;
		/// Of course, we must remeber not to define more than 100 objects (units, buildings, etc)
		/// because the id's will overlap :P
		/// </summary>
		const short _offsetPicture = 100, _offsetTurret = 200, _offsetSpecialAnimation = 300, _offsetTexture = 1000;
		#endregion

		#region funny constants
		/// <summary>
		/// These are just stupid, very often used constants.
		/// </summary>
		const float half = 1.0f / 2.0f, oneThird = 1.0f / 3.0f, oneFourth = 0.25f, oneFifth = 0.2f, oneEight = 0.125f, oneSixteenth = 0.0625f, oneEleventh = 1.0f / 11.0f;
		#endregion

		#region drawing-related members
		/// <summary>
		/// SimpleOpenGLControl's size
		/// </summary>
		static System.Drawing.Size viewport;// = new System.Drawing.Size();

		/// <summary>
		/// Map view.
		/// </summary>
		static ClipRectangle mapClip;// = new ClipRectangle();

		/// <summary>
		/// Offset used for map scrolling.
		/// </summary>
		static PointF offset;// = new PointF(0, 0);

		/// <summary>
		/// Minimum zoom, so that there is no black area on the map.
		/// </summary>
		static float minimumZoom;// = 1;
		static float zoom,// = 1.0f, 
			zoomStep;// = 3.5f;
		static float relativeZoom;

		/// <summary>
		/// Used for drawing textures
		/// </summary>
		//static VertexData vertexData = new VertexData();
		static VertexDataArray vertexDataArray;
		#endregion

		#endregion

		#region private methods

		/// <summary>
		/// Converse one bitmap to another with giver colour theme
		/// </summary>
		/// <param name="color"></param>
		/// <param name="bmp"></param>
		/// <returns></returns>
		public static Bitmap convertColour(Color color, Bitmap sourceBmp) {
			Bitmap bmp = new Bitmap(sourceBmp);
			byte a = 3, r = 2, g = 1, b = 0, temp;
			//Graphics g = Graphics.FromImage(bmp);
			Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			System.Drawing.Imaging.BitmapData bmpData =
				bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
				bmp.PixelFormat);
			IntPtr ptr = bmpData.Scan0;
			int bytes = bmp.Width * bmp.Height * 4;
			byte[] rgbValues = new byte[bytes];
			System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

			//color conversion
			for (int counter = 0; counter < rgbValues.Length; counter += 4) {
				if (rgbValues[counter + a] != 0 && rgbValues[counter + g] == 0 && rgbValues[counter + b] == 0) {
					temp = rgbValues[counter + r];
					rgbValues[counter + r] = (byte)((double)temp / 255 * color.R);
					rgbValues[counter + g] = (byte)((double)temp / 255 * color.G);
					rgbValues[counter + b] = (byte)((double)temp / 255 * color.B);
				}
			}
			//conversion end
			System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
			bmp.UnlockBits(bmpData);

			return bmp;
		}


		/// <summary>
		/// Creates 32-bit texture using bitmap "filename" and binds it to "id" so that
		/// it can be used by OpenGL to render objects.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="filename"></param>
		private static void Create32bTexture(int id, string filename) {
			Bitmap bitmap = new Bitmap(filename);
			Create32bTexture(id, bitmap);
		}

		/// <summary>
		/// Remember that bitmap should have dimensions 2^x / 2^y.
		/// Use GameGraphics.LoadBitmap( ) for bitmap loading.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="bitmap"></param>
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

			minimumZoom = Math.Max((float)viewport.Width / (float)_gameLogic.Simulation.Map.Width, (float)viewport.Height / (float)_gameLogic.Simulation.Map.Height);
			minimumZoom = Math.Max(minimumZoom, 16f);

			UpdateZoom();
			//UpdateClip();
		}

		private static void UpdateZoom() {
			if (zoom < minimumZoom) {
				zoom = minimumZoom;
			}

			relativeZoom = zoom / minimumZoom;

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

		private static void DrawElementFromLeftBottom(float x, float y, float z, float width, float height, int texture, RectangleF uv, bool useOffset) {
			//Gl.glEnable(Gl.GL_TEXTURE_2D);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);

			//init uv mapping for texture
			//vertexDataArray.uv[0] = uv.Left; vertexDataArray.uv[1] = uv.Bottom;
			//vertexDataArray.uv[2] = uv.Right; vertexDataArray.uv[3] = uv.Bottom;
			//vertexDataArray.uv[4] = uv.Right; vertexDataArray.uv[5] = uv.Top;
			//vertexDataArray.uv[6] = uv.Left; vertexDataArray.uv[7] = uv.Top;

			setQuadFromLeftBottom(0, x, y, z, width, height);
			setQuadUV(0, uv.Left, uv.Bottom, uv.Right, uv.Bottom, uv.Right, uv.Top, uv.Left, uv.Top);

			if (useOffset) {
				applyQuadOffset(0, 1);
			}

			if (Settings.Default.UseSafeRendering) {
				Gl.glBegin(Gl.GL_TRIANGLE_FAN);
				int i2 = 0, i3 = 0;
				for (int i = 0; i < 4; i++) {
					Gl.glTexCoord2f(vertexDataArray.uv[i2], vertexDataArray.uv[i2 + 1]);
					Gl.glVertex3f(vertexDataArray.vertices[i3], vertexDataArray.vertices[i3 + 1], vertexDataArray.vertices[i3 + 2]);
					i2 += 2;
					i3 += 3;
				}
				Gl.glEnd();
				return;
			}
			Gl.glDrawElements(Gl.GL_TRIANGLE_FAN, 4, Gl.GL_UNSIGNED_SHORT, vertexDataArray.IndicesPtr);
		}

		private static void DrawElementFromMiddle(float x, float y, float z, float width, float height, int texture, RectangleF uv, bool useOffset) {
			float w2 = width / 2.0f;
			float h2 = height / 2.0f;
			//Gl.glPushMatrix();
			//Gl.glTranslatef(x + moveX, y + moveY, 0);            
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);

			vertexDataArray.uv[0] = uv.Left; vertexDataArray.uv[1] = uv.Bottom;
			vertexDataArray.uv[2] = uv.Right; vertexDataArray.uv[3] = uv.Bottom;
			vertexDataArray.uv[4] = uv.Right; vertexDataArray.uv[5] = uv.Top;
			vertexDataArray.uv[6] = uv.Left; vertexDataArray.uv[7] = uv.Top;

			vertexDataArray.vertices[0] = x + 0.5f - w2;
			vertexDataArray.vertices[1] = y + 0.5f - h2;
			vertexDataArray.vertices[2] = z;
			vertexDataArray.vertices[3] = x + 0.5f + w2;
			vertexDataArray.vertices[4] = y + 0.5f - h2;
			vertexDataArray.vertices[5] = z;
			vertexDataArray.vertices[6] = x + 0.5f + w2;
			vertexDataArray.vertices[7] = y + 0.5f + h2;
			vertexDataArray.vertices[8] = z;
			vertexDataArray.vertices[9] = x + 0.5f - w2;
			vertexDataArray.vertices[10] = y + 0.5f + h2;
			vertexDataArray.vertices[11] = z;

			if (useOffset) {
				vertexDataArray.vertices[0] -= offset.X;
				vertexDataArray.vertices[1] -= offset.Y;
				//vertexDataArray.vertices[2] = z;
				vertexDataArray.vertices[3] -= offset.X;
				vertexDataArray.vertices[4] -= offset.Y;
				//vertexDataArray.vertices[5] = z;
				vertexDataArray.vertices[6] -= offset.X;
				vertexDataArray.vertices[7] -= offset.Y;
				//vertexDataArray.vertices[8] = z;
				vertexDataArray.vertices[9] -= offset.X;
				vertexDataArray.vertices[10] -= offset.Y;
				//vertexDataArray.vertices[11] = z;
			}
			if (Settings.Default.UseSafeRendering) {
				Gl.glBegin(Gl.GL_TRIANGLE_FAN);
				int i2 = 0, i3 = 0;
				for (int i = 0; i < 4; i++) {
					Gl.glTexCoord2f(vertexDataArray.uv[i2], vertexDataArray.uv[i2 + 1]);
					Gl.glVertex3f(vertexDataArray.vertices[i3], vertexDataArray.vertices[i3 + 1], vertexDataArray.vertices[i3 + 2]);
					i2 += 2;
					i3 += 3;
				}
				Gl.glEnd();
				return;
			}
			Gl.glDrawElements(Gl.GL_TRIANGLE_FAN, 4, Gl.GL_UNSIGNED_SHORT, vertexDataArray.IndicesPtr);
		}

		private static void UpdateOffsetY() {
			if (offset.Y < mapClip.Height / 2.0f) {
				offset.Y = mapClip.Height / 2.0f;
			}

			if (offset.Y > _gameLogic.Simulation.Map.Height - mapClip.Height / 2.0f) {
				offset.Y = _gameLogic.Simulation.Map.Height - mapClip.Height / 2.0f;
			}
		}

		private static void UpdateOffsetX() {
			if (offset.X < mapClip.Width / 2.0f) {
				offset.X = mapClip.Width / 2.0f;
			}

			if (offset.X > _gameLogic.Simulation.Map.Width - mapClip.Width / 2.0f) {
				offset.X = _gameLogic.Simulation.Map.Width - mapClip.Width / 2.0f;
			}
		}

		private static void Notify() {
			if (GameGraphicsChanged != null) {
				GameGraphicsChanged(null, null);
			}
		}
		#endregion

		#region texture init

		enum MainTextures : int {
			Map = 1,
			SelectionRectangle = 2,
			FogOfWar = 3,
			ThinSpice = 4, ThickSpice = 5, Rocket = 6, Bullet = 7, Sonic = 8, Building1x1 = 9, Building2x2 = 10, Building2x3 = 11, Building3x3 = 12
		}

		public static void InitTextures(Simulation simulation) {
			//Map
			_miniMap.MakeCurrent();
			Create32bTexture((int)MainTextures.Map, MapTextureGenerator.GenerateBitmap(simulation.Map));

			_map.MakeCurrent();
			Create32bTexture((int)MainTextures.Map, MapTextureGenerator.GenerateBitmap(simulation.Map));
			//Selection rectangle
			Bitmap selectionRect = new Bitmap(Path.Combine(Settings.Default.UI, "Selection.png"));
			Create32bTexture((int)MainTextures.SelectionRectangle, LoadBitmap(selectionRect));
			//FogOfWar texture
			Bitmap fow = new Bitmap(Path.Combine(Settings.Default.Terrain, "Hidden.png"));
			Create32bTexture((int)MainTextures.FogOfWar, LoadBitmap(fow));
			Bitmap thinSpice = new Bitmap(Path.Combine(Settings.Default.Terrain, "Spice.png"));
			Create32bTexture((int)MainTextures.ThinSpice, LoadBitmap(thinSpice));
			Bitmap thickSpice = new Bitmap(Path.Combine(Settings.Default.Terrain, "ThickSpice.png"));
			Create32bTexture((int)MainTextures.ThickSpice, LoadBitmap(thickSpice));

			Bitmap bullet = new Bitmap(Path.Combine(Settings.Default.Bullets, "Medium.png"));
			Create32bTexture((int)MainTextures.Bullet, LoadBitmap(bullet));

			Bitmap rocket = new Bitmap(Path.Combine(Settings.Default.Bullets, "Rocket.png"));
			Create32bTexture((int)MainTextures.Rocket, LoadBitmap(rocket));

			Bitmap sonic = new Bitmap(Path.Combine(Settings.Default.Bullets, "Sonic.png"));
			Create32bTexture((int)MainTextures.Sonic, LoadBitmap(sonic));

			Bitmap building1x1 = new Bitmap(Path.Combine(Settings.Default.Structures, "BuildingTurret.png"));
			Create32bTexture((int)MainTextures.Building1x1, LoadBitmap(building1x1));

			Bitmap building2x2 = new Bitmap(Path.Combine(Settings.Default.Structures, "Building22.png"));
			Create32bTexture((int)MainTextures.Building2x2, LoadBitmap(building2x2));

			Bitmap building2x3 = new Bitmap(Path.Combine(Settings.Default.Structures, "Building23.png"));
			Create32bTexture((int)MainTextures.Building2x3, LoadBitmap(building2x3));

			Bitmap building3x3 = new Bitmap(Path.Combine(Settings.Default.Structures, "Building33.png"));
			Create32bTexture((int)MainTextures.Building3x3, LoadBitmap(building3x3));

			GameSettingsWrapper gameSettings = GlobalSettings.Wrapper;

			#region player-color specific
			PlayerInfo[] players = ClientPlayerInfo.GetAllPlayers();

			foreach (UnitTankData o in gameSettings.Tanks) {
				String tankBasePath = Path.Combine(Settings.Default.Units, o.Name + "Base.png");
				Bitmap tankBaseSource = new Bitmap(tankBasePath);

				foreach (PlayerInfo p in players) {
					Bitmap tankBaseColoured = GameGraphics.convertColour(p.Color, tankBaseSource);
					Bitmap tankBaseColouredCorrected = LoadBitmap(tankBaseColoured);
					Create32bTexture(o.TypeID + p.Id * _offsetTexture, tankBaseColouredCorrected);
				}
				//lets assume that turrets are non-coloured ;p
				String tankTurretPath = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap tankTurretSource = new Bitmap(tankTurretPath);
				Bitmap tankTurretTexture = LoadBitmap(tankTurretSource);
				Create32bTexture(o.TypeID + _offsetTurret, tankTurretTexture);

			}

			foreach (UnitTrooperData o in gameSettings.Troopers) {
				String file = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap source = new Bitmap(file);

				foreach (PlayerInfo p in players) {
					Bitmap colouredBmp = GameGraphics.convertColour(p.Color, source);
					Bitmap texture = LoadBitmap(colouredBmp);
					Create32bTexture(o.TypeID + p.Id * _offsetTexture, texture);
				}
			}

			foreach (UnitHarvesterData o in gameSettings.Harvesters) {
				String file = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap source = new Bitmap(file);

				foreach (PlayerInfo p in players) {
					Bitmap colouredBmp = GameGraphics.convertColour(p.Color, source);
					Bitmap texture = LoadBitmap(colouredBmp);
					Create32bTexture(o.TypeID + p.Id * _offsetTexture, texture);
				}

				file = Path.Combine(Settings.Default.Units, o.Name + "Sand.png");
				Bitmap specialTexture = LoadBitmap(source);
				Create32bTexture(o.TypeID + _offsetSpecialAnimation, specialTexture);
			}

			foreach (UnitMCVData o in gameSettings.MCVs) {
				String file = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap source = new Bitmap(file);

				foreach (PlayerInfo p in players) {
					Bitmap colouredBmp = GameGraphics.convertColour(p.Color, source);
					Bitmap texture = LoadBitmap(colouredBmp);
					Create32bTexture(o.TypeID + p.Id * _offsetTexture, texture);
				}
			}

			foreach (BuildingData o in gameSettings.Buildings) {
				//Load texture for map
				String file = Path.Combine(Settings.Default.Structures, o.Name + ".png");
				Bitmap source = new Bitmap(file);

				foreach (PlayerInfo p in players) {
					Bitmap colouredBmp = GameGraphics.convertColour(p.Color, source);
					Bitmap texture = LoadBitmap(colouredBmp);
					Create32bTexture(o.TypeID + p.Id * _offsetTexture, texture);
				}

				//Load picture (currently not needed)
				/*
				String file = Path.Combine(Settings.Default.Pictures, o.Name + ".png");
				Bitmap texture = LoadBitmap(file);
				Create32bTexture(o.TypeID + _pictureOffset, texture);
				 */
			}
			#endregion


			/*foreach (AmmoData o in gameSettings.Ammos) {
			}*/

			foreach (RaceData o in gameSettings.Races) {
				String file = Path.Combine(Settings.Default.Pictures, o.Name + ".png");
				Bitmap source = new Bitmap(file);
				Bitmap texture = LoadBitmap(source);
				Create32bTexture(o.TypeID + _offsetPicture, texture);
			}


			foreach (UnitSandwormData o in gameSettings.Sandworms) {
				String file = Path.Combine(Settings.Default.Units, o.Name + ".png");
				Bitmap source = new Bitmap(file);
				Bitmap texture = LoadBitmap(source);
				Create32bTexture(o.TypeID + _offsetTexture, texture);
			}
		}

		private static int Correct(int size) {
			double y = Math.Ceiling(Math.Log(size, 2));
			return (int)Math.Pow(2, y);
		}

		public static Bitmap LoadBitmap(Bitmap tmp) {
			Bitmap bmp = new Bitmap(Correct(tmp.Width), Correct(tmp.Height), PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(bmp);
			/*
			g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			 */
			g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.DrawImage(tmp, 0, 0, bmp.Width, bmp.Height);
			g.Dispose();
			return bmp;
		}

		#endregion

		#region drawing
		public static void Draw() {
			if (!init) return;

			Map map = _gameLogic.Simulation.Map;

			_miniMap.MakeCurrent();
			DrawMinimap();

			_map.MakeCurrent();
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glLoadIdentity();

			#region map
			DrawElementFromLeftBottom(0, 0, _depthMap, map.Width, map.Height, 1, _defaultUV, true);
			#endregion

			#region spice
			DrawSpice(map);
			#endregion

			#region slabs
			//TODO: when slabs are not buildings anymore - draw'em like fog of war
			#endregion

			#region sandworms
			foreach (UnitSandworm s in _gameLogic.Simulation.Sandworms.Values) {
				DrawSandworm(s);
			}
			#endregion sandworms

			#region players' data (units & buildings)
			ICollection<Player> players = _gameLogic.Simulation.GetPlayers();
			foreach (Player p in players) {
				List<Building> buildings = p.GetAllBuildings();

				foreach (Building b in buildings) {
					DrawBuilding(b);
				}
			}

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
						case BoardObjectClass.UnitTank:
							DrawTank((UnitTank)u);
							break;
						case BoardObjectClass.UnitTrooper:
							DrawTrooper((UnitTrooper)u);
							break;
					}
				}
			}

			foreach (Player p in players) {
				List<Ammo> ammos = p.GetAllAmmos();
				foreach (Ammo a in ammos) {
					DrawAmmo(a);
				}

			}
			#endregion

			float percentage;

			#region selected objects
			List<Unit> selectedUnits = new List<Unit>(_gameLogic.SelectedUnits);
			foreach (Unit u in selectedUnits) {
				PointF realPos = CountRealPosition(u);
				float size = u.getSize();
				if (!NeedsDrawing(realPos.X, realPos.Y, size, size)) {
					continue;
				}
				Player p;
				if (!_gameLogic.Simulation.Players.TryGetValue(u.ObjectID.PlayerID, out p)) {
					continue;
				}
				Color playerColor = p.Color;
				DrawSelectionRectangle(realPos.X, realPos.Y, _depthSelection, size, size, true, true, playerColor);
				percentage = ((float)u.Health) / u.getMaxHealth();
				Gl.glColor3f(1 - percentage, percentage, 0);
				DrawBar(realPos.X, realPos.Y, _depthSelection, size, size, percentage, true, oneSixteenth);

				if (u.BoardObjectClass == BoardObjectClass.UnitHarvester) {
					UnitHarvester h = u as UnitHarvester;
					percentage = ((float)h.Spice) / (float)h.HarvesterData.Capacity;
					Gl.glColor3f(1, 1 - percentage, 0);
					DrawBar(realPos.X, realPos.Y, _depthSelection, size, size, percentage, true, oneFourth + oneEight);
				}
			}

			Building selB = _gameLogic.SelectedBuilding;
			if (selB != null) {
				if (NeedsDrawing(selB.Position.X, selB.Position.Y, selB.Width, selB.Height)) {
					Player p;
					if (_gameLogic.Simulation.Players.TryGetValue(selB.ObjectID.PlayerID, out p)) {
						DrawSelectionRectangle(selB.Position.X, selB.Position.Y, _depthSelection, selB.Width, selB.Height, false, true, p.Color);
						percentage = ((float)selB.Health) / (float)selB.getMaxHealth();
						Gl.glColor3f(1 - percentage, percentage, 0);
						DrawBar(selB.Position.X, selB.Position.Y, _depthSelection, selB.Width, selB.Height, percentage, false, oneSixteenth);
					}
				}
			}

			Gl.glColor3f(1, 1, 1);

			#endregion

			#region placing building
			if (_gameForm.IsCreatingBuilding) {
				short id = _gameForm.CreatingBuildingId;
				BuildingData bd;
				if (GlobalSettings.Wrapper.buildingsMap.TryGetValue(id, out bd)) {
					Position p = TranslateMousePosition(_gameForm.getMousePositionInMapView());
					if (_gameLogic.Simulation.Map.ContainsPosition(p)) {
						if (Building.CheckBuildPosition(bd, p, _gameLogic.Simulation.Map, _gameLogic.CurrentPlayer.Id)) {
							Gl.glColor4f(0, 1, 0, 0.75f);
						} else {
							Gl.glColor4f(1, 0, 0, 0.75f);
						}
						DrawBuilding(bd, p.X, p.Y, _depthPlacingBuilding, _gameLogic.CurrentPlayer.Id);
						Gl.glColor4f(1, 1, 1, 1);
					}
				}
			}
			#endregion

			#region fow
			DrawFogOfWar();
			#endregion

			#region selection
			DrawMouseSelection();
			#endregion

			//DrawSomeShit(map);

			_map.SwapBuffers();
		}

		private static void DrawSomeShit(Map map) {
			Building b;
			Unit u;
			Gl.glDisable(Gl.GL_TEXTURE_2D);
			for (int x = 0; x < map.Width; x++) {
				for (int y = 0; y < map.Height; y++) {


					if (map.Buildings[x, y].Count > 0) {
						b = map.Buildings[x, y].First.Value;

						Gl.glColor3f(1.0f, 1.0f, 1.0f);
						DrawRectangle(x, y, _depthSelection, 1, 1, true);

					} else if (map.Units[x, y].Count > 0) {
						u = map.Units[x, y].First.Value;
						if (map.Units[x, y].Count > 1) {
							Gl.glColor3f(1.0f, 0.0f, 0.0f);
						} else if (u.Position.Equals(new Position(x, y))) {
							Gl.glColor3f(1.0f, 1.0f, 0.0f);
						} else
							Gl.glColor3f(1.0f, 1.0f, 1.0f);
						DrawRectangle(x, y, _depthSelection, 1, 1, true);
					}
				}
			}
			Gl.glEnable(Gl.GL_TEXTURE_2D);
		}



		private static bool[,] _minimapZBuffer;
		private static PointF minimapOffset;

		private static void DrawMinimap() {
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			if (!_gameLogic.isOutpostOperating()) {
				_miniMap.SwapBuffers();
				return;
			}
			Gl.glLoadIdentity();

			PointF mainOffset = offset;
			offset = minimapOffset;

			Map map = _gameLogic.Simulation.Map;

			DrawElementFromLeftBottom(0, 0, _depthMap, map.Width, map.Height, (int)MainTextures.Map, _defaultUV, true);

			//prawdopodobnie szybsze ni¿ tworzenie KOPII list jednostek i budynków i iterowanie po nich (krótsze listy ale z kopiowaniem)
			bool[,] fow = _gameLogic.CurrentPlayer.FogOfWar;

			Gl.glDisable(Gl.GL_TEXTURE_2D);

			Color color;

			for (int x = 0; x < map.Width; x++) {
				for (int y = 0; y < map.Height; y++) {
					_minimapZBuffer[x, y] = false;
				}
			}

			color = Color.Black;
			Gl.glColor3f(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);

			DrawMinimapFogOfWar(map, fow);

			Building b;
			Unit u;

			for (int x = 0; x < map.Width; x++) {
				for (int y = 0; y < map.Height; y++) {
					if (_minimapZBuffer[x, y]) {
						continue;
					}

					if (map.Buildings[x, y].Count > 0) {
						b = map.Buildings[x, y].First.Value;
						if (b.Position.X < x || b.Position.Y < y) {
							continue; // znaczy siê, ¿e by³ ju¿ wybudowany
						}

						color = _gameLogic.Simulation.Players[b.ObjectID.PlayerID].Color;
						Gl.glColor3f(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
						DrawRectangle(x, y, _depthBuilding, b.Width, b.Height, true);
						for (int i = 0; i < b.Width; i++) {
							for (int j = 0; j < b.Height; j++) {
								_minimapZBuffer[x + i, y + j] = true;
							}
						}
					} else if (map.Units[x, y].Count > 0) {
						u = map.Units[x, y].First.Value;
						color = _gameLogic.Simulation.Players[u.ObjectID.PlayerID].Color;
						Gl.glColor3f(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
						DrawRectangle(x, y, _depthUnit, 1, 1, true);
						_minimapZBuffer[x, y] = true;
					}
				}
			}

			DrawSelectionRectangle(mainOffset.X, mainOffset.Y, _depthSelection, mapClip.Width, mapClip.Height, true, true, Color.White);

			Gl.glEnable(Gl.GL_TEXTURE_2D);
			Gl.glColor3f(1, 1, 1);

			offset = mainOffset;

			_miniMap.SwapBuffers();
		}

		private static void DrawMinimapFogOfWar(Map map, bool[,] fow) {
			int quadCounter = 0;
			for (int x = 0; x < map.Width; x++) {
				for (int y = 0; y < map.Height; y++) {
					if (fow[x, y]) {
						setQuadFromLeftBottom(quadCounter, x, y, _depthFogOfWar, 1, 1);
						_minimapZBuffer[x, y] = true;
						quadCounter++;
					}
				}
			}
			applyQuadOffset(0, quadCounter);
			DrawVerticesArray(quadCounter);
		}

		static void applyQuadOffset(int firstQuadIndex, int quadCount) {
			int begin = firstQuadIndex * 12;
			int end = begin + 12 * quadCount;
			for (int i = begin; i < end; i += 3) {
				vertexDataArray.vertices[i] -= offset.X;
				vertexDataArray.vertices[i + 1] -= offset.Y;
			}
		}

		static void setQuadUV(int quadIndex,
								float u1, float v1,
								float u2, float v2,
								float u3, float v3,
								float u4, float v4) {
			int idx = quadIndex * 8;
			vertexDataArray.uv[idx + 0] = u1; vertexDataArray.uv[idx + 1] = v1;
			vertexDataArray.uv[idx + 2] = u2; vertexDataArray.uv[idx + 3] = v2;
			vertexDataArray.uv[idx + 4] = u3; vertexDataArray.uv[idx + 5] = v3;
			vertexDataArray.uv[idx + 6] = u4; vertexDataArray.uv[idx + 7] = v4;
		}

		static void setQuadFromLeftBottom(int quadIndex, float x, float y, float z, float width, float height) {
			int vertexIndex = quadIndex * 12;
			vertexDataArray.vertices[vertexIndex] = x;
			vertexDataArray.vertices[vertexIndex + 1] = y;
			vertexDataArray.vertices[vertexIndex + 2] = z;
			vertexDataArray.vertices[vertexIndex + 3] = x + width;
			vertexDataArray.vertices[vertexIndex + 4] = y;
			vertexDataArray.vertices[vertexIndex + 5] = z;
			vertexDataArray.vertices[vertexIndex + 6] = x + width;
			vertexDataArray.vertices[vertexIndex + 7] = y + height;
			vertexDataArray.vertices[vertexIndex + 8] = z;
			vertexDataArray.vertices[vertexIndex + 9] = x;
			vertexDataArray.vertices[vertexIndex + 10] = y + height;
			vertexDataArray.vertices[vertexIndex + 11] = z;
		}

		private static void DrawRectangle(float x, float y, float z, float width, float height, bool useOffset) {
			setQuadFromLeftBottom(0, x, y, z, width, height);
			if (useOffset) {
				applyQuadOffset(0, 4);
			}

			vertexDataArray.uv[0] = 0; vertexDataArray.uv[1] = 0;
			vertexDataArray.uv[2] = 1; vertexDataArray.uv[3] = 0;
			vertexDataArray.uv[4] = 1; vertexDataArray.uv[5] = 1;
			vertexDataArray.uv[6] = 0; vertexDataArray.uv[7] = 1;

			if (Settings.Default.UseSafeRendering) {
				Gl.glBegin(Gl.GL_QUADS);
				int i2 = 0, i3 = 0;
				for (int i = 0; i < 4; i++) {
					//Gl.glTexCoord2f(vertexDataArray.uv[i2], vertexDataArray.uv[i2 + 1]);
					Gl.glVertex3f(vertexDataArray.vertices[i3], vertexDataArray.vertices[i3 + 1], vertexDataArray.vertices[i3 + 2]);
					i2 += 2;
					i3 += 3;
				}
				Gl.glEnd();
			} else {
				Gl.glDrawElements(Gl.GL_QUADS, 4, Gl.GL_UNSIGNED_SHORT, vertexDataArray.IndicesPtr);
			}
		}

		private static void DrawVerticesArray(int quadCount) {

			if (Settings.Default.UseSafeRendering) {
				Gl.glBegin(Gl.GL_QUADS);
				int i2 = 0, i3 = 0;
				for (int i = 0; i < quadCount * 4; i++) {
					//Gl.glTexCoord2f(vertexDataArray.uv[i2], vertexDataArray.uv[i2 + 1]);
					Gl.glVertex3f(vertexDataArray.vertices[i3], vertexDataArray.vertices[i3 + 1], vertexDataArray.vertices[i3 + 2]);
					i2 += 2;
					i3 += 3;
				}
				Gl.glEnd();
			} else {
				Gl.glDrawElements(Gl.GL_QUADS, 4 * quadCount, Gl.GL_UNSIGNED_SHORT, vertexDataArray.IndicesPtr);
			}
		}

		private static void DrawMouseSelection() {
			if (!_gameForm.IsSelecting) {
				return;
			}
			Position start = _gameForm.SelectionStart;
			Position end = _gameForm.SelectionEnd;
			short xMin = Math.Min(start.X, end.X);
			short xMax = Math.Max(start.X, end.X);
			short yMin = Math.Min(start.Y, end.Y);
			short yMax = Math.Max(start.Y, end.Y);
			DrawSelectionRectangle(xMin, yMin, _depthMouseSelection, xMax - xMin + 1, yMax - yMin + 1, false, true, _gameLogic.CurrentPlayer.Color);
		}

		private static void DrawFogOfWar() {
			Player p = _gameLogic.Simulation.Players[_gameLogic.CurrentPlayer.Id];
			bool[,] fogOfWar = p.FogOfWar;
			for (int x = 0; x < fogOfWar.GetLength(0); x++) {
				for (int y = 0; y < fogOfWar.GetLength(1); y++) {
					if (fogOfWar[x, y] == false || !NeedsDrawing(x, y, 1, 1)) {
						continue;
					}
					int fogIndex = MapTextureGenerator.FindFogFrame(fogOfWar, x, y);
					RectangleF fowUV = new RectangleF(oneSixteenth * (float)fogIndex + oneSixteenth * oneSixteenth, oneSixteenth - 2 * oneSixteenth * oneSixteenth, oneSixteenth - 2 * oneSixteenth * oneSixteenth, 1 - oneEight);
					DrawElementFromLeftBottom(x, y, _depthFogOfWar, 1, 1, (int)MainTextures.FogOfWar, fowUV, true);
				}
			}
		}

		private static void DrawSpice(Map map) {
			for (int x = 0; x < map.Width; x++) {
				for (int y = 0; y < map.Height; y++) {
					if (!NeedsDrawing(x, y, 1, 1) || map.Spice[x, y] == 0) {
						continue;
					}
					int index = MapTextureGenerator.FindSpiceFrame(map.Spice, x, y);
					int texture = (map.Spice[x, y] >= 10) ? (int)MainTextures.ThickSpice : (int)MainTextures.ThinSpice;
					RectangleF spiceUV = new RectangleF(oneSixteenth * index, 0, oneSixteenth, 1);
					DrawElementFromLeftBottom(x, y, _depthSpice, 1, 1, texture, spiceUV, true);
				}
			}
		}

		private static void DrawSelectionRectangle(float x, float y, float z, float width, float height, bool fromMiddle, bool useOffset, Color color) {

			if (fromMiddle) {
				float w2 = width / 2.0f;
				float h2 = height / 2.0f;

				//left bottom
				vertexDataArray.vertices[0] = x + 0.5f - w2;
				vertexDataArray.vertices[1] = y + 0.5f - h2;
				vertexDataArray.vertices[2] = z;

				//right bottom
				vertexDataArray.vertices[3] = x + 0.5f + w2;
				vertexDataArray.vertices[4] = y + 0.5f - h2;
				vertexDataArray.vertices[5] = z;

				//right top
				vertexDataArray.vertices[6] = x + 0.5f + w2;
				vertexDataArray.vertices[7] = y + 0.5f + h2;
				vertexDataArray.vertices[8] = z;


				vertexDataArray.vertices[9] = x + 0.5f - w2;
				vertexDataArray.vertices[10] = y + 0.5f + h2;
				vertexDataArray.vertices[11] = z;
			} else {
				vertexDataArray.vertices[0] = x;
				vertexDataArray.vertices[1] = y;
				vertexDataArray.vertices[2] = z;
				vertexDataArray.vertices[3] = x + width;
				vertexDataArray.vertices[4] = y;
				vertexDataArray.vertices[5] = z;
				vertexDataArray.vertices[6] = x + width;
				vertexDataArray.vertices[7] = y + height;
				vertexDataArray.vertices[8] = z;
				vertexDataArray.vertices[9] = x;
				vertexDataArray.vertices[10] = y + height;
				vertexDataArray.vertices[11] = z;
			}

			if (useOffset) {
				vertexDataArray.vertices[0] -= offset.X;
				vertexDataArray.vertices[1] -= offset.Y;
				//vertexDataArray.vertices[2] = z;
				vertexDataArray.vertices[3] -= offset.X;
				vertexDataArray.vertices[4] -= offset.Y;
				//vertexDataArray.vertices[5] = z;
				vertexDataArray.vertices[6] -= offset.X;
				vertexDataArray.vertices[7] -= offset.Y;
				//vertexDataArray.vertices[8] = z;
				vertexDataArray.vertices[9] -= offset.X;
				vertexDataArray.vertices[10] -= offset.Y;
				//vertexDataArray.vertices[11] = z;
			}

			vertexDataArray.uv[0] = 0; vertexDataArray.uv[1] = 0;
			vertexDataArray.uv[2] = 1; vertexDataArray.uv[3] = 0;
			vertexDataArray.uv[4] = 1; vertexDataArray.uv[5] = 1;
			vertexDataArray.uv[6] = 0; vertexDataArray.uv[7] = 1;

			//Gl.glBindTexture(Gl.GL_TEXTURE_2D, (int)MainTextures.MalaPierdolonaKropka);
			Gl.glDisable(Gl.GL_TEXTURE_2D);
			Gl.glColor3f(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);

			//Gl.glLineWidth(relativeZoom);

			if (Settings.Default.UseSafeRendering) {
				Gl.glBegin(Gl.GL_LINE_LOOP);
				int i2 = 0, i3 = 0;
				for (int i = 0; i < 4; i++) {
					//Gl.glTexCoord2f(vertexDataArray.uv[i2], vertexDataArray.uv[i2 + 1]);
					Gl.glVertex3f(vertexDataArray.vertices[i3], vertexDataArray.vertices[i3 + 1], vertexDataArray.vertices[i3 + 2]);
					i2 += 2;
					i3 += 3;
				}
				Gl.glEnd();
			} else {
				Gl.glDrawElements(Gl.GL_LINE_LOOP, 4, Gl.GL_UNSIGNED_SHORT, vertexDataArray.IndicesPtr);
			}

			Gl.glEnable(Gl.GL_TEXTURE_2D);
			Gl.glColor3f(1, 1, 1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="health">Between 0 and 1</param>
		private static void DrawBar(float x, float y, float z, float width, float height, float percentage, bool forUnit, float offsetY) {

			float healthBarWidth = (width + half) * percentage;
			float healthBarWidth2 = healthBarWidth / 2.0f;
			float h2 = height / 2.0f;
			float barHeight = oneFourth;

			if (forUnit) {
				//left bottom
				vertexDataArray.vertices[0] = x + 0.5f - healthBarWidth2 - offset.X;
				vertexDataArray.vertices[1] = y + 0.5f + h2 - offset.Y + offsetY;
				vertexDataArray.vertices[2] = z;

				//right bottom
				vertexDataArray.vertices[3] = x + 0.5f + healthBarWidth2 - offset.X;
				vertexDataArray.vertices[4] = y + 0.5f + h2 - offset.Y + offsetY;
				vertexDataArray.vertices[5] = z;

				//right top
				vertexDataArray.vertices[6] = x + 0.5f + healthBarWidth2 - offset.X;
				vertexDataArray.vertices[7] = y + 0.5f + h2 - offset.Y + offsetY + barHeight;
				vertexDataArray.vertices[8] = z;

				//left top
				vertexDataArray.vertices[9] = x + 0.5f - healthBarWidth2 - offset.X;
				vertexDataArray.vertices[10] = y + 0.5f + h2 - offset.Y + offsetY + barHeight;
				vertexDataArray.vertices[11] = z;
			} else {
				float w2 = width / 2.0f;

				//left bottom
				vertexDataArray.vertices[0] = x + w2 - healthBarWidth2 - offset.X;
				vertexDataArray.vertices[1] = y + height - offset.Y + offsetY;
				vertexDataArray.vertices[2] = z;

				//right bottom
				vertexDataArray.vertices[3] = x + w2 + healthBarWidth2 - offset.X;
				vertexDataArray.vertices[4] = y + height - offset.Y + offsetY;
				vertexDataArray.vertices[5] = z;

				//right top
				vertexDataArray.vertices[6] = x + w2 + healthBarWidth2 - offset.X;
				vertexDataArray.vertices[7] = y + height - offset.Y + offsetY + barHeight;
				vertexDataArray.vertices[8] = z;

				//left top
				vertexDataArray.vertices[9] = x + w2 - healthBarWidth2 - offset.X;
				vertexDataArray.vertices[10] = y + height - offset.Y + offsetY + barHeight;
			}

			Gl.glDisable(Gl.GL_TEXTURE_2D);
			//Gl.glColor3f(1 - percentage, percentage, 0);

			if (Settings.Default.UseSafeRendering) {
				Gl.glBegin(Gl.GL_POLYGON);
				int i2 = 0, i3 = 0;
				for (int i = 0; i < 4; i++) {
					Gl.glVertex3f(vertexDataArray.vertices[i3], vertexDataArray.vertices[i3 + 1], vertexDataArray.vertices[i3 + 2]);
					i2 += 2;
					i3 += 3;
				}
				Gl.glEnd();
			} else {
				Gl.glDrawElements(Gl.GL_POLYGON, 4, Gl.GL_UNSIGNED_SHORT, vertexDataArray.IndicesPtr);
			}

			Gl.glEnable(Gl.GL_TEXTURE_2D);
			//Gl.glColor3f(1, 1, 1);
		}

		#region Helpers
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

		private static bool NeedsDrawing(float x, float y, float width, float height) {
			if (x + width < mapClip.Left + offset.X)
				return false;
			if (x > mapClip.Right + offset.X)
				return false;
			if (y > mapClip.Top + offset.Y)
				return false;
			if (y + height < mapClip.Bottom + offset.Y)
				return false;
			return true;
		}

		#endregion

		private static void DrawTrooper(UnitTrooper o) {
			PointF realPos = CountRealPosition(o);
			if (!NeedsDrawing(realPos.X, realPos.Y, 0.5f, 0.5f)) {
				return;
			}
			float frame = o.RemainingTurnsInMove % 3;

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

			DrawElementFromMiddle(realPos.X, realPos.Y, _depthUnit, 0.5f, 0.5f, o.TypeID + o.ObjectID.PlayerID * _offsetTexture, uv, true);
		}

		private static void DrawTank(UnitTank o) {
			PointF realPos = CountRealPosition(o);
			if (!NeedsDrawing(realPos.X, realPos.Y, 1, 1)) {
				return;
			}
			Direction d = o.Direction;
			Direction t = o.TurretDirection;
			RectangleF uvBase = VehicleUVChooser(d);
			DrawElementFromLeftBottom(realPos.X, realPos.Y, _depthUnit, 1, 1, o.TypeID + o.ObjectID.PlayerID * _offsetTexture, uvBase, true);

			RectangleF uvTurret = VehicleUVChooser(t);
			DrawElementFromMiddle(realPos.X, realPos.Y, _depthUnitAddons, 1, 1, o.TypeID + _offsetTurret, uvTurret, true);
		}


		private static RectangleF VehicleUVChooser(Direction d) {
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
			return uv;


		}

		private static RectangleF AmmoUVChooser(Direction d) {
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
			//uv.Width = oneSixteenth;
			return uv;
		}

		private static void DrawAmmo(Ammo a) {
			//a.LastPosition
			PointF realPos = new PointF(a.Position.X, a.Position.Y);//
			switch (a.Type) {
				case AmmoType.None:
					break;
				case AmmoType.Bullet:
					DrawElementFromMiddle(realPos.X, realPos.Y, _depthAmmos, 0.1f, 0.1f, (int)MainTextures.Bullet, _defaultUV, true);
					break;
				case AmmoType.Rocket:
					RectangleF uv = AmmoUVChooser(a.Direction);
					DrawElementFromMiddle(realPos.X, realPos.Y, _depthAmmos, 1, 1, (int)MainTextures.Rocket, uv, true);
					break;
				case AmmoType.Sonic:
					DrawElementFromMiddle(realPos.X, realPos.Y, _depthAmmos, 1, 1, (int)MainTextures.Sonic, _defaultUV, true);
					break;
				default:
					break;
			}

		}

		private static RectangleF AmmoUVChooser(Ammo a) {
			RectangleF uv = VehicleUVChooser(a.Direction);
			uv.Width = oneSixteenth;
			return uv;
		}

		private static void DrawSandworm(UnitSandworm o) {
			PointF realPos = CountRealPosition(o);
			if (!NeedsDrawing(realPos.X, realPos.Y, 1, 1)) {
				return;
			}
			//float frame = 0;// o.RemainingTurnsInMove % 5;
			DrawElementFromLeftBottom(realPos.X, realPos.Y, _depthSandworm, 1, 1, o.TypeID + _offsetTexture, _defaultUV, true);
		}

		private static void DrawMCV(UnitMCV o) {
			PointF realPos = CountRealPosition(o);
			if (!NeedsDrawing(realPos.X, realPos.Y, 1.5f, 1.5f)) {
				return;
			}
			Direction d = o.Direction;

			RectangleF uv = VehicleUVChooser(d);
			DrawElementFromMiddle(realPos.X, realPos.Y, _depthUnit, 1.5f, 1.5f, o.TypeID + o.ObjectID.PlayerID * _offsetTexture, uv, true);
		}

		private static void DrawHarvester(UnitHarvester o) {
			PointF realPos = CountRealPosition(o);
			if (!NeedsDrawing(realPos.X, realPos.Y, 1.5f, 1.5f)) {
				return;
			}
			Direction d = o.Direction;
			RectangleF uv = VehicleUVChooser(d);
			DrawElementFromMiddle(realPos.X, realPos.Y, _depthUnit, 1.5f, 1.5f, o.TypeID + o.ObjectID.PlayerID * _offsetTexture, uv, true);

			//TODO: draw sand animation
		}

		private static void DrawBuilding(Building o) {
			if (!NeedsDrawing(o.Position.X, o.Position.Y, o.Width, o.Height)) {
				return;
			}

			if (o.State == Building.BuildingState.constructing) {
				int tex;
				if (o.Width == 1 && o.Height == 1) {
					tex = (int)MainTextures.Building1x1;
				} else if (o.Width == 2 && o.Height == 2) {
					tex = (int)MainTextures.Building2x2;
				} else if (o.Width == 3 && o.Height == 2) {
					tex = (int)MainTextures.Building2x3;
				} else {
					tex = (int)MainTextures.Building3x3;
				}
				DrawElementFromLeftBottom(o.Position.X, o.Position.Y, _depthBuilding, o.Width, o.Height, tex, _defaultUV, true);
				return;
			}

			BuildingData bd = o.BuildingData;
			float y = bd.TextureAnimationFramesCount;
			float x = bd.TextureSpecialActionFramesCount;
			RectangleF uv;
			if (bd.IsTurret) {
				uv = VehicleUVChooser(o.Direction);
			} else if (bd.TypeID == _wallID) {
				uv = WallUVChooser(o);
			} else {
				uv = new RectangleF(0, 1.0f / y, 1.0f / x, 1.0f / y);
			}

			DrawElementFromLeftBottom(o.Position.X, o.Position.Y, _depthBuilding, o.Width, o.Height, o.TypeID + o.ObjectID.PlayerID * _offsetTexture, uv, true);
		}

		private static RectangleF WallUVChooser(Building o) {
			int index = MapTextureGenerator.GetWallTextureIndex(o.Position, _gameLogic.Simulation.Map);
			RectangleF uv = new RectangleF(index * oneEleventh, 0, oneEleventh, 1);
			return uv;
		}

		private static void DrawBuilding(BuildingData bd, float x, float y, float depth, short playerID) {
			if (!NeedsDrawing(x, y, bd.Size.X, bd.Size.Y)) {
				return;
			}
			float cy = bd.TextureAnimationFramesCount;
			float cx = bd.TextureSpecialActionFramesCount;
			RectangleF uv;
			if (bd.IsTurret) {
				uv = VehicleUVChooser(Direction.North);
			} else {
				uv = new RectangleF(0, 1.0f / cy, 1.0f / cx, 1.0f / cy);
			}

			DrawElementFromLeftBottom(x, y, depth, bd.Size.X, bd.Size.Y, bd.TypeID + playerID * _offsetTexture, uv, true);
		}
		#endregion

		static int _wallID; //needs special drawing

		#region public methods
		public static void InitGL(GameLogic gLogic, GameForm gForm, SimpleOpenGlControl map, SimpleOpenGlControl miniMap) {
			viewport = new System.Drawing.Size();
			mapClip = new ClipRectangle();
			offset = new PointF(0, 0);
			minimumZoom = 1;
			zoom = 1.0f;
			zoomStep = 3.5f;
			relativeZoom = 0;

			_map = map;
			_miniMap = miniMap;
			_gameLogic = gLogic;
			_gameForm = gForm;
			_gameLogic.Simulation.onTurnEnd += new SimulationHandler(GameGraphics.Notify);
			vertexDataArray = new VertexDataArray(4 * _gameLogic.Simulation.Map.Width * _gameLogic.Simulation.Map.Height); // * 4, bo zakadamy, ze dla kazdego pola planszy sa 4 vertexy

			_wallID = GlobalSettings.Wrapper.namesToIds["Wall"];

			//Gl.glEnable(Gl.GL_LINE_SMOOTH);

			initGLminimap();
			initGLmap();

			Application.Idle += new EventHandler(Application_Idle);

			init = true;			
		}

		static void Application_Idle(object sender, EventArgs e) {
			while (NativeMethods.NativeMethods.ApplicationIsIdle) {
				//UpdateFrame();
				Draw();
			}
		}	
		
		public static void DeinitGL() {
			Application.Idle -= new EventHandler(Application_Idle);

			init = false;
			/*
			_gameLogic = null;
			_gameForm = null;
			_map = null;
			_miniMap = null;
			 */
		}

		private static void initGLminimap() {
			Map map = _gameLogic.Simulation.Map;

			_miniMap.MakeCurrent();
			_minimapZBuffer = new bool[map.Width, map.Height];

			Gl.glDisable(Gl.GL_LIGHTING);

			Gl.glEnable(Gl.GL_TEXTURE_2D);                                      // Enable Texture Mapping
			//Gl.glEnable(Gl.GL_BLEND);
			//Gl.glShadeModel(Gl.GL_SMOOTH);                                      // Enable Smooth Shading
			Gl.glClearColor(0, 0, 0, 0);                                     // Black Background
			Gl.glClearDepth(1);                                                 // Depth Buffer Setup
			//Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
			//Gl.glDepthFunc(Gl.GL_LEQUAL);                                       // The Type Of Depth Testing To Do
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
			//Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

			Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, vertexDataArray.VerticesPtr);
			Gl.glTexCoordPointer(2, Gl.GL_FLOAT, 0, vertexDataArray.UVPtr);
			Gl.glEnable(Gl.GL_VERTEX_ARRAY);
			Gl.glEnable(Gl.GL_TEXTURE_COORD_ARRAY);


			Gl.glViewport(0, 0, _miniMap.Width, _miniMap.Height);

			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();

			float maxDim = Math.Max(map.Width, map.Height);
			Gl.glOrtho(-maxDim / 2.0f, maxDim / 2.0f, -maxDim / 2.0f, maxDim / 2.0f, -1, 1);
			minimapOffset = new PointF(map.Width / 2.0f, map.Height / 2.0f);

			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
		}

		private static void initGLmap() {
			_map.MakeCurrent();
			setGLparams();
			UpdateViewport();
		}

		private static void setGLparams() {
			Gl.glDisable(Gl.GL_LIGHTING);

			Gl.glEnable(Gl.GL_TEXTURE_2D);                                      // Enable Texture Mapping
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glShadeModel(Gl.GL_SMOOTH);                                      // Enable Smooth Shading
			Gl.glClearColor(0, 0, 0, 0);                                     // Black Background
			Gl.glClearDepth(1);                                                 // Depth Buffer Setup
			//Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LEQUAL);                                       // The Type Of Depth Testing To Do
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

			Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, vertexDataArray.VerticesPtr);
			Gl.glTexCoordPointer(2, Gl.GL_FLOAT, 0, vertexDataArray.UVPtr);
			Gl.glEnable(Gl.GL_VERTEX_ARRAY);
			Gl.glEnable(Gl.GL_TEXTURE_COORD_ARRAY);
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
		public static Position TranslateMousePosition(Point p) {
			Position pn = new Position(
			(short)(((float)p.X) / zoom + offset.X + mapClip.X),
			(short)(((float)(viewport.Height - p.Y - 1)) / zoom + offset.Y + mapClip.Y));
			return pn;
		}

		public static Position TranslateMinimapMousePosition(Point p) {
			Map m = _gameLogic.Simulation.Map;
			float maxDim = Math.Max(m.Width, m.Height);
			float zoom = ((float)maxDim) / (float)_miniMap.Width;
			Position pn = new Position(
			(short)(((float)p.X) * zoom - maxDim / 2.0f),
			(short)(((float)(_miniMap.Height - p.Y - 1)) * zoom - maxDim / 2.0f));
			return pn;

			/*
			float xRatio = ((float)p.X) / (float)_miniMap.Width;
			float yRatio = 1.0f - ((float)p.Y) / (float)_miniMap.Height;
			int x = (int)(xRatio * maxDim - (maxDim / 2.0f));
			int y = (int)(yRatio * maxDim - (maxDim / 2.0f));

			Position pn = new Position(x, y);
			return pn;
			 */
		}

		public static void centerOn(Position p) {
			Map m = _gameLogic.Simulation.Map;
			offset.X = p.X;
			offset.Y = p.Y;

			UpdateOffsetX();
			UpdateOffsetY();
		}
	}
}
