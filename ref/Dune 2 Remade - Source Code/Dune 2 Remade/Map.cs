using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dune_2_Remade
{
    public struct Terrain 
    {
        public int LogicalValue;
        public ExtendedTexture texture;
    }
    public class Map
    {
        public Point size;
        public int radarNumber = 0;
        public Terrain[] terrain;
        public bool[] fog;
        public Map()
        {
            size.X = 0;
            size.Y = 0;
            terrain = null;
            fog = null;
        }
        private double Lenght(Point p1, int x, int y)
        {
            return Math.Sqrt((x-p1.X)*(x-p1.X)+(y-p1.Y)*(y-p1.Y));
        }
        public void removeFog(Point p, float radius)
        {
            int r = (int)radius;
            for (int x = p.X - r; x < p.X + r; x++)
            {
                if(x<0)
                    continue;
                if(x>=size.X)
                    return;
                for (int y = p.Y - r; y < p.Y + r; y++)
                {
                    if (y < 0)
                        continue;
                    if (y >= size.Y)
                        break;
                    if(Lenght(p,x,y)<=radius)
                        fog[x+y*size.X]=false;
                }
            }
        }
        public Map(String filename)
        {
            Load(filename);
        }
        public void Load(string filename)
        {
            StreamReader input;
            try
            {
                input = new StreamReader(filename);                
            }
            catch (Exception)
            {
                return;
            }
            Load(input);
        }
        public List<Point> FindWay(Point start, Point destination, Unit u)
        {
            if (u.name == "Trooper")
                return FindWay(start, destination, true);
            return FindWay(start, destination, false);
        }
        public List<Point> FindWay(Point start, Point destination, bool mayMoveOnMountain)
        {
            List<Point> list = new List<Point>();
            list.Add(start);
            //list.Add(new Point(10, 10));
            list.Add(destination);
            return list;
        }
        public void Load(StreamReader input)
        {
            String s = input.ReadToEnd();
            int i;
            for (i = 0; i < s.Length; i++)
            {
                if (s[i] == '\n')
                {
                    size.X = i-1;
                    break;
                }
            }
            size.Y = (s.Length+2) / (i+1);
            terrain = new Terrain[size.X * size.Y];
            fog = new bool[size.X * size.Y];
            for (int w = 0; w < fog.Length; w++)
                fog[w] = true;
            ExtendedTexture[] textureTemplates = new ExtendedTexture[4];
            textureTemplates[0] = Textures.GetTextureReference("Terrain_Dunes");
            textureTemplates[1] = Textures.GetTextureReference("Terrain_Mountain");
            textureTemplates[2] = Textures.GetTextureReference("Terrain_Rock");
            textureTemplates[3] = Textures.GetTextureReference("Terrain_Spice");
            int j=0;
            for (i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case '0':
                        terrain[j].LogicalValue = 0;
                        terrain[j].texture = textureTemplates[terrain[j].LogicalValue];
                        break;
                    case '1':
                        terrain[j].LogicalValue = 1;
                        terrain[j].texture = textureTemplates[terrain[j].LogicalValue];
                        break;
                    case '2':
                        terrain[j].LogicalValue = 2;
                        terrain[j].texture = textureTemplates[terrain[j].LogicalValue];
                        break;
                    case '3':
                        terrain[j].LogicalValue = 3;
                        terrain[j].texture = textureTemplates[terrain[j].LogicalValue];
                        break;
                    default:
                        j--;
                        break;
                }
                j++;
                if (j >= terrain.Length)
                    break;
            }
        }
        public void Render(Camera camera)
        {
            if (terrain == null)
                return;
            int dX = (int)camera.GetPosition().X;
            int dY = (int)camera.GetPosition().Y;
            float X = (float)dX - camera.GetPosition().X;
            float Y = (float)dY - camera.GetPosition().Y;
            for (int y = (int)camera.GetPosition().Y; y < (int)(camera.GetPosition().Y + camera.GetPosition().Z+1.0f); y++)
            {
                X = (float)dX - camera.GetPosition().X;
                for (int x = (int)camera.GetPosition().X; x < (int)(camera.GetPosition().X + camera.GetPosition().W+1.0f); x++)
                {
                    if (x + size.X * y < terrain.Length)
                        if(!fog[x + size.X * y])
                            camera.spriteBatch.Draw(terrain[x + size.X * y].texture.Texture, camera.GetRenderPosition(new Vector4(X, Y, 1.2f, 1.2f)), terrain[x + size.X * y].texture.GetSourceRectangle(new Vector2(1, 0), 0.0f), Color.White);                        
                        else
                            camera.spriteBatch.Draw(terrain[x + size.X * y].texture.Texture, camera.GetRenderPosition(new Vector4(X, Y, 1.2f, 1.2f)), terrain[x + size.X * y].texture.GetSourceRectangle(new Vector2(1, 0), 0.0f), Color.Black);                        
                    X+=1.0f;    
                }
                Y+=1.0f;
            }
            /*
            int h = camera.GetTileHeight(), w = camera.GetTileWidth();
            Rectangle rc = camera.GetRenderPosition(new Vector4(X, Y, 1.02f, 1.02f));
            int xpos = rc.X, ypos = rc.Y;
            for (int y = (int)camera.GetPosition().Y; y < (int)(camera.GetPosition().Y + camera.GetPosition().Z + 1.0f); y++)
            {
                //X = (float)dX - camera.GetPosition().X;
                for (int x = (int)camera.GetPosition().X; x < (int)(camera.GetPosition().X + camera.GetPosition().W + 1.0f); x++)
                {
                    if (x + size.X * y < terrain.Length)
                        camera.spriteBatch.Draw(terrain[x + size.X * y].texture.Texture, new Rectangle(xpos,ypos, w, h), terrain[x + size.X * y].texture.GetSourceRectangle(new Vector2(1, 0), 0.0f), Color.White);
                    X += 1.0f;
                    xpos += w;
                }
                //Y += 1.0f;
                xpos = rc.X;
                ypos += h;
            }*/
            
        }        
        public void RenderMiniMap(SpriteBatch spriteBatch, Rectangle destination, List<Unit> AllMyUnits, List<Building> AllMyBuildings, List<Unit> AllEnemyUnits, List<Building> AllEnemyBuildings, Camera camera)
        {
            int z=0;
            if (size.X > size.Y)
            {
                z = size.X - size.Y;
                destination.Height -= z;
                z >>= 1;
                destination.Y += z;                
            }
            if (size.Y > size.X)
            {
                z = size.Y - size.X;
                destination.Width -= z;
                z >>= 1;
                destination.X += z;
            }
            Rectangle destinationRect=new Rectangle();
            int x, y;
            for(x=0;x<size.X;x++)
            {
                for(y=0; y<size.Y;y++)
                {                    
                    destinationRect.X = GetStep(destination.Width, size.X, x);
                    destinationRect.Y = GetStep(destination.Height, size.Y, y);
                    destinationRect.Width = GetStep(destination.Width, size.X, x + 1);
                    destinationRect.Height = GetStep(destination.Height, size.Y, y + 1);
                    destinationRect.Width -= destinationRect.X;
                    destinationRect.Height -= destinationRect.Y;
                    destinationRect.X += destination.X;
                    destinationRect.Y += destination.Y;
                    if(!fog[x+y*size.X])
                        spriteBatch.Draw(terrain[y * size.X + x].texture.Texture, destinationRect, terrain[y * size.X + x].texture.GetSourceRectangle(new Vector2(1, 0), 0), Color.White);
                    else
                        spriteBatch.Draw(terrain[y * size.X + x].texture.Texture, destinationRect, terrain[y * size.X + x].texture.GetSourceRectangle(new Vector2(1, 0), 0), Color.Black);
                }
            }               
            Texture2D texture=Textures.GetPictureReference("MiniMap_Object");
            foreach (Unit u in AllMyUnits)
	        {
                x = (int)u.position.X;
                y = (int)u.position.Y;
                destinationRect.X = GetStep(destination.Width, size.X, x);
                destinationRect.Y = GetStep(destination.Height, size.Y, y);
                destinationRect.Width = GetStep(destination.Width, size.X, x + 1);
                destinationRect.Height = GetStep(destination.Height, size.Y, y + 1);
                destinationRect.Width -= destinationRect.X;
                destinationRect.Height -= destinationRect.Y;
                destinationRect.X += destination.X;
                destinationRect.Y += destination.Y;
                spriteBatch.Draw(texture, destinationRect, Color.LightSeaGreen);                    
	        }
            foreach (Unit u in AllEnemyUnits)
            {
                x = (int)u.position.X;
                y = (int)u.position.Y;
                if (!fog[x + y * size.X])
                {
                    destinationRect.X = GetStep(destination.Width, size.X, x);
                    destinationRect.Y = GetStep(destination.Height, size.Y, y);
                    destinationRect.Width = GetStep(destination.Width, size.X, x + 1);
                    destinationRect.Height = GetStep(destination.Height, size.Y, y + 1);
                    destinationRect.Width -= destinationRect.X;
                    destinationRect.Height -= destinationRect.Y;
                    destinationRect.X += destination.X;
                    destinationRect.Y += destination.Y;
                    spriteBatch.Draw(texture, destinationRect, Color.Red);
                }
            }
            foreach (Building u in AllMyBuildings)
            {
                x = (int)u.position.X;
                y = (int)u.position.Y;
                destinationRect.X = GetStep(destination.Width, size.X, x);
                destinationRect.Y = GetStep(destination.Height, size.Y, y);
                destinationRect.Width = GetStep(destination.Width, size.X, x + u.size.X);
                destinationRect.Height = GetStep(destination.Height, size.Y, y + u.size.Y);
                destinationRect.Width -= destinationRect.X;
                destinationRect.Height -= destinationRect.Y;
                destinationRect.X += destination.X;
                destinationRect.Y += destination.Y;
                spriteBatch.Draw(texture, destinationRect, Color.LightGreen);
            }
            foreach (Building u in AllEnemyBuildings)
            {
                x = (int)u.position.X;
                y = (int)u.position.Y;
                int a, b;
                for (a = 0; a < u.size.X; a++)
                {
                    for (b = 0; b < u.size.Y; b++)
                    {
                        if((int)u.position.X + a + size.X * ((int)u.position.Y + b)<fog.Length)
                            if (!fog[(int)u.position.X + a + size.X * ((int)u.position.Y + b)])
                                break;
                    }
                    if (b < u.size.Y)
                        break;
                }
                if (a < u.size.X)
                {
                    destinationRect.X = GetStep(destination.Width, size.X, x);
                    destinationRect.Y = GetStep(destination.Height, size.Y, y);
                    destinationRect.Width = GetStep(destination.Width, size.X, x + u.size.X);
                    destinationRect.Height = GetStep(destination.Height, size.Y, y + u.size.Y);
                    destinationRect.Width -= destinationRect.X;
                    destinationRect.Height -= destinationRect.Y;
                    destinationRect.X += destination.X;
                    destinationRect.Y += destination.Y;
                    spriteBatch.Draw(texture, destinationRect, Color.LightPink);
                }
            }
            destinationRect.X=GetStep(destination.Width,size.X,(int)camera.GetPosition().X);
            destinationRect.Y=GetStep(destination.Height,size.Y,(int)camera.GetPosition().Y);
            destinationRect.Width=GetStep(destination.Width,size.X,(int)(camera.GetPosition().W));
            destinationRect.Height=1;
            destinationRect.X += destination.X;
            destinationRect.Y += destination.Y;
            spriteBatch.Draw(texture,destinationRect,Color.Gold);

            destinationRect.Y = GetStep(destination.Height, size.Y, (int)(camera.GetPosition().Z+camera.GetPosition().Y));
            destinationRect.Y += destination.Y;
            spriteBatch.Draw(texture, destinationRect, Color.Gold);

            destinationRect.Y = GetStep(destination.Height, size.Y, (int)(camera.GetPosition().Y));
            destinationRect.Y += destination.Y;
            destinationRect.Width = 1;
            destinationRect.Height = GetStep(destination.Height, size.Y, (int)(camera.GetPosition().Z));
            spriteBatch.Draw(texture, destinationRect, Color.Gold);

            destinationRect.X = GetStep(destination.Width, size.X, (int)(camera.GetPosition().X+camera.GetPosition().W));
            destinationRect.X += destination.X;
            spriteBatch.Draw(texture, destinationRect, Color.Gold);

        }
        public Vector2 GetMapPositionFromMinimap(Point point)
        {
            Rectangle destination=UserInterface.GetMiniMapLocation();
            destination.X = 0;
            destination.Y = 0;
            float x,y;
            int z = 0;
            if (size.X > size.Y)
            {
                z = size.X - size.Y;
                destination.Height -= z;
                z >>= 1;
                destination.Y += z;
            }
            if (size.Y > size.X)
            {
                z = size.Y - size.X;
                destination.Width -= z;
                z >>= 1;
                destination.X += z;
            }
            if (point.X < destination.X)
                x = 0;
            else if (point.X >= destination.Right)
                x = size.X - 1;
            else
            {
                point.X -= destination.X;
                x = (float)(point.X * size.X) / (float)destination.Width;
            }
            if (point.Y < destination.Y)
                y = 0;
            else if (point.Y > destination.Bottom)
                y = size.Y - 1;
            else
            {
                point.Y -= destination.Y;
                y = (float)(point.Y * size.Y) / (float)destination.Height;
            }
            return new Vector2(x, y);
        }
        private int GetStep(int w, int steps, int n)
        {
            return (n * w) / steps;
        }
    }
}
