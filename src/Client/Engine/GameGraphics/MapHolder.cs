using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Yad.Board.Common;
using Yad.Engine.GameGraphics.Client;
using Yad.Properties;
using Yad.Properties.Client;
using Yad.UI.Client;
using Yad.Engine.Client;

namespace Yad.Engine.GameGraphics.Client {
	static class MapTextureGenerator {
		static int textureSize = 16;
		static Bitmap[] bmps = null;
		private static ETextures[][] tileFrameMap = new ETextures[][]{
			new ETextures[]{ETextures.Mountain, ETextures.Mountain, ETextures.Mountain, ETextures.Mountain, ETextures.Mountain, (ETextures)0}, //left, right, upper, lower
			new ETextures[]{ETextures.Mountain, ETextures.Rock, ETextures.Rock, ETextures.Rock, ETextures.Rock, (ETextures)1},
			new ETextures[]{ETextures.Mountain, ETextures.Mountain, ETextures.Mountain, ETextures.Rock, ETextures.Rock, (ETextures)2},
			new ETextures[]{ETextures.Mountain, ETextures.Mountain,ETextures.Rock, ETextures.Rock,ETextures.Rock, (ETextures)3},
			new ETextures[]{ETextures.Mountain, ETextures.Rock,ETextures.Mountain,ETextures.Rock,ETextures.Rock, (ETextures)4},
			new ETextures[]{ETextures.Mountain, ETextures.Rock,ETextures.Rock, ETextures.Mountain, ETextures.Rock, (ETextures)5},
			new ETextures[]{ETextures.Mountain, ETextures.Rock,ETextures.Rock, ETextures.Rock,ETextures.Mountain, (ETextures)6},
			new ETextures[]{ETextures.Mountain, ETextures.Rock,ETextures.Rock, ETextures.Mountain, ETextures.Mountain, (ETextures)7},
			new ETextures[]{ETextures.Mountain, ETextures.Rock,ETextures.Mountain, ETextures.Mountain, ETextures.Mountain, (ETextures)8},
			new ETextures[]{ETextures.Mountain, ETextures.Mountain,ETextures.Rock, ETextures.Mountain, ETextures.Mountain, (ETextures)9},
			new ETextures[]{ETextures.Mountain, ETextures.Mountain, ETextures.Mountain, ETextures.Rock, ETextures.Mountain, (ETextures)10},
			new ETextures[]{ETextures.Mountain, ETextures.Mountain, ETextures.Mountain, ETextures.Mountain, ETextures.Rock, (ETextures)11},
			new ETextures[]{ETextures.Mountain, ETextures.Rock, ETextures.Mountain, ETextures.Rock, ETextures.Mountain, (ETextures)12},
			new ETextures[]{ETextures.Mountain, ETextures.Mountain, ETextures.Rock, ETextures.Mountain, ETextures.Rock, (ETextures)13},
			new ETextures[]{ETextures.Mountain, ETextures.Mountain, ETextures.Rock, ETextures.Rock, ETextures.Mountain, (ETextures)14},
			new ETextures[]{ETextures.Mountain, ETextures.Rock, ETextures.Mountain, ETextures.Mountain, ETextures.Rock, (ETextures)15},

			
			new ETextures[]{ETextures.Rock, ETextures.Sand, ETextures.Sand, ETextures.Sand, ETextures.Sand, (ETextures)1},
			new ETextures[]{ETextures.Rock, ETextures.Rock, ETextures.Rock, ETextures.Sand, ETextures.Sand, (ETextures)2},
			new ETextures[]{ETextures.Rock, ETextures.Rock,ETextures.Sand, ETextures.Sand,ETextures.Sand, (ETextures)3},
			new ETextures[]{ETextures.Rock, ETextures.Sand,ETextures.Rock,ETextures.Sand,ETextures.Sand, (ETextures)4},
			new ETextures[]{ETextures.Rock, ETextures.Sand,ETextures.Sand, ETextures.Rock, ETextures.Sand, (ETextures)5},
			new ETextures[]{ETextures.Rock, ETextures.Sand,ETextures.Sand, ETextures.Sand,ETextures.Rock, (ETextures)6},
			new ETextures[]{ETextures.Rock, ETextures.Sand,ETextures.Sand, ETextures.Rock, ETextures.Rock, (ETextures)7},
			new ETextures[]{ETextures.Rock, ETextures.Sand,ETextures.Rock, ETextures.Rock, ETextures.Rock, (ETextures)8},
			new ETextures[]{ETextures.Rock, ETextures.Rock,ETextures.Sand, ETextures.Rock, ETextures.Rock, (ETextures)9},
			new ETextures[]{ETextures.Rock, ETextures.Rock, ETextures.Rock, ETextures.Sand, ETextures.Rock, (ETextures)10},
			new ETextures[]{ETextures.Rock, ETextures.Rock, ETextures.Rock, ETextures.Rock, ETextures.Sand, (ETextures)11},
			new ETextures[]{ETextures.Rock, ETextures.Sand, ETextures.Rock, ETextures.Sand, ETextures.Rock, (ETextures)12},
			new ETextures[]{ETextures.Rock, ETextures.Rock, ETextures.Sand, ETextures.Rock, ETextures.Sand, (ETextures)13},
			new ETextures[]{ETextures.Rock, ETextures.Rock, ETextures.Sand, ETextures.Sand, ETextures.Rock, (ETextures)14},
			new ETextures[]{ETextures.Rock, ETextures.Sand, ETextures.Rock, ETextures.Rock, ETextures.Sand, (ETextures)15},
			new ETextures[]{ETextures.Rock, ETextures.Whatever, ETextures.Whatever, ETextures.Whatever, ETextures.Whatever, (ETextures)0}, //left, right, upper, lower
			new ETextures[]{ETextures.Sand, ETextures.Whatever,ETextures.Whatever,ETextures.Whatever,ETextures.Whatever, (ETextures)0}
			};
		private static short[][] frameMap = new short[][]{new short[]{(short)1, (short)1, (short)1, (short)1, (short)1, (short)0}, //left, right, upper, lower
			new short[]{(short)1, (short)0, (short)0, (short)0, (short)0, (short)1},
			new short[]{(short)1, (short)1, (short)1, (short)0, (short)0, (short)2},
			new short[]{(short)1, (short)1,(short)0, (short)0,(short)0, (short)3},
			new short[]{(short)1, (short)0,(short)1,(short)0,(short)0, (short)4},
			new short[]{(short)1, (short)0,(short)0, (short)1, (short)0, (short)5},
			new short[]{(short)1, (short)0,(short)0, (short)0,(short)1, (short)6},
			new short[]{(short)1, (short)0,(short)0, (short)1, (short)1, (short)7},
			new short[]{(short)1, (short)0,(short)1, (short)1, (short)1, (short)8},
			new short[]{(short)1, (short)1,(short)0, (short)1, (short)1, (short)9},
			new short[]{(short)1, (short)1, (short)1, (short)0, (short)1, (short)10},
			new short[]{(short)1, (short)1, (short)1, (short)1, (short)0, (short)11},
			new short[]{(short)1, (short)0, (short)1, (short)0, (short)1, (short)12},
			new short[]{(short)1, (short)1, (short)0, (short)1, (short)0, (short)13},
			new short[]{(short)1, (short)1, (short)0, (short)0, (short)1, (short)14},
			new short[]{(short)1, (short)0, (short)1, (short)1, (short)0, (short)15}
		};

		private static int FindFrame(Map map, int x, int y) {
			int result;
			if (x < 0 || y < 0 || x >= map.Width || y >= map.Height)
				throw new MapHolderException("Incorrect map position");

			ETextures tileLeft, tileRight, tileUpper, tileLower;

			if (x > 0)
				tileLeft = (ETextures)map.Tiles[x - 1, y];
			else
				tileLeft = ETextures.Whatever;

			if (x < map.Width - 1)
				tileRight = (ETextures)map.Tiles[x + 1, y];
			else
				tileRight = ETextures.Whatever;

			if (y < map.Height - 1)
				tileUpper = (ETextures)map.Tiles[x, y + 1];
			else
				tileUpper = ETextures.Whatever;

			if (y > 0)
				tileLower = (ETextures)map.Tiles[x, y - 1];
			else
				tileLower = ETextures.Whatever;

			for (int i = 0; i < tileFrameMap.Length; i++) {
				if ((result = Match(tileFrameMap[i], (ETextures)map.Tiles[x, y], tileLeft, tileRight, tileLower, tileUpper)) >= 0)
					return result;
			}
			throw new MapHolderException("Bitmap frame not found");

		}

		private static int Match(ETextures[] tileType, ETextures center, ETextures tileLeft, ETextures tileRight, ETextures tileLower, ETextures tileUpper) {
			if (tileLeft == ETextures.Whatever)
				tileLeft = center;
			if (tileRight == ETextures.Whatever)
				tileRight = center;
			if (tileUpper == ETextures.Whatever)
				tileUpper = center;
			if (tileLower == ETextures.Whatever)
				tileLower = center;
			if (tileType[0] == center && (tileType[1] == tileLeft || tileType[1] == ETextures.Whatever) && (tileType[2] == tileRight || tileType[2] == ETextures.Whatever) && (tileType[3] == tileUpper || tileType[3] == ETextures.Whatever) && (tileType[4] == tileLower || tileType[4] == ETextures.Whatever))
				return (int)tileType[5];
			else
				return -1;
		}

		private static void LoadTextures() {
			try {
				if (bmps != null)
					return;
				bmps = new Bitmap[TextureFiles.Count];
				for (int i = 0; i < TextureFiles.Count; i++) {
					bmps[i] = new Bitmap(Path.Combine(Path.GetFullPath(Settings.Default.Terrain), TextureFiles.getFileName((ETextures)i)));
				}
			} catch (Exception e) {
				throw new MapHolderException(e);
			}
		}


		public static Bitmap GenerateBitmap(Map map) {
			if (!map.CheckConststency())
				throw new MapHolderException("Map is inconsistent");
			LoadTextures();

			int oldWidth = map.Width * textureSize, oldHeight = map.Height * textureSize;
			Bitmap bmp = new Bitmap(oldWidth, oldHeight, PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(bmp);

			for (int y = 0; y < map.Height; y++) {
				for (int x = 0; x < map.Width; x++) {
					Bitmap sourceBitmap = bmps[(int)map.Tiles[x, y]]; //all the possible tile's variations are here, spaced horizontally

					Rectangle destRect = new Rectangle(textureSize * x, textureSize * (map.Height - y - 1), textureSize, textureSize);
					Rectangle sourceRect = new Rectangle(sourceBitmap.Height * FindFrame(map, x, y), 0, sourceBitmap.Height, sourceBitmap.Height);

					g.DrawImage(sourceBitmap, destRect, sourceRect, GraphicsUnit.Pixel);
				}
			}
			g.Dispose();

			//scale bitmap to whole bitmap with dimensions = 2^y
			int newWidth = Correct(oldWidth), newHeight = Correct(oldHeight);
			Bitmap res = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);
			Graphics g2 = Graphics.FromImage(res);
			g2.DrawImage(bmp, 0, 0, newWidth, newHeight);
			return res;
		}

		private static int Correct(int size) {
			double y = Math.Ceiling(Math.Log(size, 2));
			return (int)Math.Pow(2, y);
		}

		public static int FindFogFrame(bool[,] FogOfWar, int x, int y) {
			int result;
			int width = FogOfWar.GetLength(0),
				height = FogOfWar.GetLength(1);
			if (x < 0 || y < 0 || x >= width || y >= height)
				throw new MapHolderException("Incorrect map position");

			bool fogLeft, fogRight, fogUpper, fogLower;

			if (x > 0)
				fogLeft = FogOfWar[x - 1, y];
			else
				fogLeft = FogOfWar[x, y];

			if (x < width - 1)
				fogRight = FogOfWar[x + 1, y];
			else
				fogRight = FogOfWar[x, y];

			if (y < height - 1)
				fogUpper = FogOfWar[x, y + 1];
			else
				fogUpper = FogOfWar[x, y];

			if (y > 0)
				fogLower = FogOfWar[x, y - 1];
			else
				fogLower = FogOfWar[x, y];
			for (int i = 0; i < tileFrameMap.Length; i++) {
				if ((result = MatchFog(frameMap[i], FogOfWar[x, y], fogLeft, fogRight, fogLower, fogUpper)) >= 0)
					return result;
			}
			throw new MapHolderException("Bitmap frame not found");

		}

		public static int FindSpiceFrame(int[,] map, int x, int y) {
			int result;
			int width = map.GetLength(0),
				height = map.GetLength(1);
			if (x < 0 || y < 0 || x >= width || y >= height)
				throw new MapHolderException("Incorrect map position");

			int left, right, upper, lower;

			if (x > 0)
				left = map[x - 1, y];
			else
				left = map[x, y];

			if (x < width - 1)
				right = map[x + 1, y];
			else
				right = map[x, y];

			if (y < height - 1)
				upper = map[x, y + 1];
			else
				upper = map[x, y];

			if (y > 0)
				lower = map[x, y - 1];
			else
				lower = map[x, y];
			for (int i = 0; i < frameMap.Length; i++) {
				if ((result = MatchSpice(frameMap[i], map[x, y], left, right, lower, upper)) >= 0)
					return result;
			}
			return -1;

		}

		private static short MatchSpice(short[] spiceFrameMap, int center, int left, int right, int lower, int upper) {
			if ((center>0) == (spiceFrameMap[0]>0) && (left>0) == (spiceFrameMap[1]>0) && (right>0) == (spiceFrameMap[2]>0) && (upper>0) == (spiceFrameMap[3]>0) && (lower>0) == (spiceFrameMap[4]>0))
				return spiceFrameMap[5];
			return -1;
		}

		private static short MatchFog(short[] fogFrameMap, bool center, bool fogLeft, bool fogRight, bool fogLower, bool fogUpper) {
			if (Convert.ToInt16(center) == fogFrameMap[0] && Convert.ToInt16(fogLeft) == fogFrameMap[1] && Convert.ToInt16(fogRight) == fogFrameMap[2] && Convert.ToInt16(fogUpper) == fogFrameMap[3] && Convert.ToInt16(fogLower) == fogFrameMap[4])
				return fogFrameMap[5];
			return -1;
		}

	}
}
