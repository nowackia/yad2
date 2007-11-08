using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Client.Board;

namespace Client.Engine.GameGraphics {
	class MapTextureGenerator {
		static int textureSize = 16;

		public static Bitmap GenerateBitmap() {
			Bitmap bmp = new Bitmap(Map.Width * textureSize, Map.Height * textureSize, PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(bmp);
			for (int y = 0; y < Map.Height; y++) {
				for (int x = 0; x < Map.Width; x++) {
					//TODO PR: tutaj ładowanie bitmap na mape.
					g.FillRectangle(new SolidBrush(Color.FromArgb(80 * (int)Map.Tiles[x, y], 80 * (int)Map.Tiles[x, y], 80 * (int)Map.Tiles[x, y])), new Rectangle(textureSize * x, textureSize * y, textureSize, textureSize));
				}
			}
			return bmp;
		}
	}
}
