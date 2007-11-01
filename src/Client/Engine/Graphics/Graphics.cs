using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Tao.OpenGl;

namespace Client.Engine.Graphics
{
    static class Graphics
    {
        /*
         * TODO:
         * Z pliku konfiguracyjnego wyci¹gn¹æ nazwy jednostek,
         * do³¹czyæ im int ID i po tym identyfikowaæ
         */

        /// <summary>
        /// Initializes all textures
        /// </summary>
        static public void Init()
        {
//            Create32bTexture(Texture.Indoor, Path.Combine(Resources.GraphicsPath, Resources.indoorTileBmp));
        }

        /// <summary>
        /// Creates 32-bit texture using bitmap "filename" and binds it to "id" so that
        /// it can be used by OpenGL to render objects.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="filename"></param>
        private static void Create32bTexture(int id, string filename)
        {
            Bitmap bitmap = new Bitmap(filename);
            Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, id);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);//_MIPMAP_NEAREST);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8, bitmap.Width, bitmap.Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
            //Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, 4, bitmap.Width, bitmap.Height, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

            bitmap.UnlockBits(bitmapData);
            //bitmap.Dispose();
        }
    }
}
