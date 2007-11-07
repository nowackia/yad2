using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Client.Engine.GameGraphics
{
    class MapHolder
    {
        #region private fields
        private Bitmap bmp;
        List<List<int>> list = new List<List<int>>();
        private Graphics g;
        private int maxWidth;
        #endregion

        #region constructors
        public MapHolder(string mapFilePath)
        {
            int c;
            int value;
            List<int> tempList = null;
            try
            {
                StreamReader sr = new StreamReader(mapFilePath);
                while (!sr.EndOfStream)
                {
                    c = sr.Read();
                    if (tempList == null)
                    {
                        tempList = new List<int>();
                        maxWidth = 0;
                    }
                    if ((char)c == '\n')
                    {
                        list.Add(tempList);
                        tempList = null;
                        continue;
                    }
                    else if (Int32.TryParse(((char)c).ToString(), out value))
                    {
                        tempList.Add(value);
                        if (tempList.Count > maxWidth)
                        {
                            maxWidth = tempList.Count;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new MapHolderException(ex);
            }
        }
        public MapHolder() { }
        #endregion

        public Bitmap ToBitmap()
        {
            bmp = new Bitmap(maxWidth * 32, list.Count * 32, PixelFormat.Format32bppArgb);
            g = Graphics.FromImage(bmp);
            for (int y = 0; y < list.Count; y++)
            {
                for (int x = 0; x < list[y].Count; x++)
                {
                    //TODO PR: tutaj ładowanie bitmap na mape.
                    g.FillRectangle(new SolidBrush(Color.FromArgb(80 * list[y][x], 80 * list[y][x], 80 * list[y][x])), new Rectangle(32 * x, 32 * y, 32, 32));

                }
            }
            return bmp;
        }
    }
}
