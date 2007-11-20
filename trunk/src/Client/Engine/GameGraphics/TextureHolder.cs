using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Yad.Engine.Common;
using Yad.Properties.Common;
using System.IO;

namespace Yad.Engine.GameGraphics
{
	static class TextureHolder
	{
		public const short PLAYERS_NUM = 8;
		public static Color[] playerColors = new Color[PLAYERS_NUM] {Color.Green, Color.Blue, Color.Red, Color.Pink, Color.Yellow, Color.Violet, Color.SpringGreen, Color.Khaki};
		static Bitmap[,] bitmaps;

		private static void initializeAll()
		{
			if (bitmaps == null)
			{
				bitmaps = new Bitmap[PLAYERS_NUM, UnitTextures.Count];
				for (int i = 0; i < PLAYERS_NUM; i++)
					for (int j = 0; j < UnitTextures.Count; j++)
					{
						bitmaps[i, j] = convertColour(playerColors[i], new Bitmap(Path.Combine(Settings.Default.Units, UnitTextures.getFileName((EUnitTexture)j))));
						
						//bitmap colour conversion
					}
			}
		}

		public static Bitmap convertColour(Color color, Bitmap bmp)
		{
			byte a =3, r = 2, g = 1, b = 0, temp;
			//Graphics g = Graphics.FromImage(bmp);
			Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			System.Drawing.Imaging.BitmapData bmpData =
				bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
				bmp.PixelFormat);
			IntPtr ptr = bmpData.Scan0;
			int bytes = bmp.Width * bmp.Height * 3;
			byte[] rgbValues = new byte[bytes];
			System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
			
			//color conversion
			for (int counter = 0; counter < rgbValues.Length; counter += 4)
			{
				if (rgbValues[counter + a] != 0 && rgbValues[counter + g] == 0 && rgbValues[counter + b] == 0)
				{
					temp = rgbValues[counter + r];
					rgbValues[counter + r] = (byte)((double)temp/255*color.R); 
					rgbValues[counter + g] = (byte)((double)temp/255*color.G);
					rgbValues[counter + b] = (byte)((double)temp / 255 * color.B);
				}
			}
			//conversion end
			System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
			bmp.UnlockBits(bmpData);

			return bmp;
		}


		public static Bitmap getBitmap(int x, EUnitTexture y)
		{
			if (bitmaps == null)
				initializeAll();
			return bitmaps[x, (int)y];
		}


	}
}
