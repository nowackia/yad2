using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Client.Board;
using Yad.Board.Common;
using Client.Properties;

namespace Client.Engine.GameGraphics {
	static class MapTextureGenerator {
		static int textureSize = 16;
        static Bitmap[] bmps = null;
        /*
        private static void loadTextures()
        {
            if (bmps !=null)
                return;
            bmps = new Bitmap[TextureFiles.Count];
            for (int i = 0; i < TextureFiles.Count; i++)
            {
                bmps[i] = new Bitmap(Path.Combine(Path.GetFullPath(Settings.Default.Terrain), TextureFiles.getFileName((ETextures)i)));
            }
         }
        */
        public static Bitmap GenerateBitmap()
        {
			//loadTextures();
	        Bitmap bmp = new Bitmap(Map.Width * textureSize, Map.Height * textureSize, PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(bmp);
			for (int y = 0; y < Map.Height; y++) {
				for (int x = 0; x < Map.Width; x++) {
					g.DrawImage(bmps[(int)Map.Tiles[x, y]],new Rectangle(textureSize*x,textureSize*y, textureSize,textureSize), new Rectangle (0,0,textureSize,textureSize),GraphicsUnit.Pixel);
				}
			}
			return bmp;
		}
	}
}
