using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dune_2_Remade
{
    public class Camera
    {
        Vector4 viewRectangle;
        Point mapSize;
        public Rectangle mapWindowPosition;
        public SpriteBatch spriteBatch;
        private void Correct()
        {
            if (viewRectangle.Z < 1)
                viewRectangle.Z = 1;
            if (viewRectangle.W < 1)
                viewRectangle.W = 1;      
            if (viewRectangle.W > mapSize.X)
                viewRectangle.W = mapSize.X;
            if (viewRectangle.Z > mapSize.Y)
                viewRectangle.Z = mapSize.Y;
            if (viewRectangle.X + viewRectangle.W >= mapSize.X)
                viewRectangle.X = mapSize.X - viewRectangle.W;
            if (viewRectangle.Y + viewRectangle.Z >= mapSize.Y)
                viewRectangle.Y = mapSize.Y - viewRectangle.Z;
            if (viewRectangle.X < 0)
                viewRectangle.X = 0;
            if (viewRectangle.Y < 0)
                viewRectangle.Y = 0;      
      
        }
        public Camera(SpriteBatch spriteBatch, Vector4 viewRectangle, Point mapSize, Rectangle mapWindowPosition)
        {
            this.mapSize = mapSize;
            this.spriteBatch = spriteBatch;
            this.viewRectangle = viewRectangle;
            Correct();
            this.mapWindowPosition = mapWindowPosition;            
        }
        public void Move(Vector2 vector)
        {
            viewRectangle.X += vector.X;            
            viewRectangle.Y += vector.Y;
            Correct();
        }        
        public void SetMapViewingSize(Vector2 size)
        {
            viewRectangle.W = size.X;
            viewRectangle.Z = size.Y;
            Correct();
        }
        public void SetPosition(Point position)
        {
            viewRectangle.X = position.X;
            viewRectangle.Y = position.Y;
            Correct();
        }
        public Vector4 GetPosition()
        {
            return viewRectangle;
        }
        public void Zoom(float zoom)
        {
            if (zoom <= 0.0f)
                return;
            viewRectangle.W /= zoom;
            viewRectangle.Z /= zoom;
            if (viewRectangle.W > 30)
                viewRectangle.W = 30;
            if (viewRectangle.Z > 30)
                viewRectangle.Z = 30;
            Correct();
        }
        public void Center(Vector2 pos)
        {
            viewRectangle.X = pos.X - (viewRectangle.W / 2.0f);
            viewRectangle.Y = pos.Y - (viewRectangle.Z / 2.0f);
            Correct();
        }
        public Vector2 GetCenterPosition()
        {
            return new Vector2(viewRectangle.X + viewRectangle.W / 2.0f, viewRectangle.Y + viewRectangle.Z / 2.0f);
        }
        public Rectangle GetRenderPosition(Vector4 position)
        {
            return new Rectangle((int)(position.X * (float)mapWindowPosition.Width / viewRectangle.W),
                                 (int)(position.Y * (float)mapWindowPosition.Height / viewRectangle.Z),
                                 (int)(position.W * (float)mapWindowPosition.Width / viewRectangle.W),
                                 (int)(position.Z * (float)mapWindowPosition.Height / viewRectangle.Z));
        }

        public int GetTileWidth()
        {
            return (int)(mapWindowPosition.Width / viewRectangle.W);
        }

        public int GetTileHeight()
        {
            return (int)(mapWindowPosition.Height / viewRectangle.Z);
        }

        public bool Visible(Vector4 pos)
        {
            if (((int)pos.X <= mapWindowPosition.X + mapWindowPosition.Width) && (pos.X >= -1.0f) && (pos.Y >= -1.0f) && ((int)pos.Y <= mapWindowPosition.Y + mapWindowPosition.Height))
                return true;
            return false;
        }
        public Vector2 GetMapPosition(Point screenPosition)
        {
            if (screenPosition.X > mapWindowPosition.Width)
                return new Vector2(-1.0f, -1.0f);
            if (screenPosition.Y < mapWindowPosition.Y)
                return new Vector2(-1.0f, -1.0f);

            Vector2 v = new Vector2();
            v.X=viewRectangle.X+viewRectangle.W*((float)screenPosition.X)/((float)mapWindowPosition.Width);
            v.Y=viewRectangle.Y+viewRectangle.Z*((float)(screenPosition.Y-mapWindowPosition.Y))/((float)mapWindowPosition.Height);
            return v;
        }
    }
}
