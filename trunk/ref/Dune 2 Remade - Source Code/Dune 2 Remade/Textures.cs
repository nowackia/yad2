using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.Configuration;

namespace Dune_2_Remade
{
    public class ExtendedTexture
    {
        Texture2D texture;        
        double elapsedAnimationTime=0;
        int directionCount;
        int animationCount;
        double animationTime;

        public ExtendedTexture(ExtendedTexture t)
        {
            texture = t.texture;
            elapsedAnimationTime = t.elapsedAnimationTime;
            directionCount = t.directionCount;
            animationCount = t.animationCount;
            animationTime = t.animationTime;
        }
        public ExtendedTexture(Texture2D texture, int directionCount, int animationCount, float animationTime)
        {
            this.texture = texture;
            this.directionCount = directionCount;
            this.animationCount = animationCount;
            this.animationTime = animationTime;            
        }
        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
        }        
        public void ResetTime()
        {
            elapsedAnimationTime = 0;
        }        
        public Rectangle GetSourceRectangle(Vector2 direction, GameTime gameTime)
        {
            elapsedAnimationTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedAnimationTime > animationTime)
                elapsedAnimationTime -= animationTime;
            return GetSourceRectangle(direction, (float)(elapsedAnimationTime / animationTime));
        }
        public Rectangle GetSourceRectangle(Vector2 direction, float animationRate)
        {
            float sourceX = 0, sourceWidth = texture.Width;
            float sourceY = 0, sourceHeight = texture.Height;
            direction.Y = -direction.Y;
            if (animationCount > 1)
            {
                if (animationRate > 1)
                    animationRate = 1;
                if (animationRate < 0)
                    animationRate = 0;
                animationRate *= texture.Height;
                sourceHeight /= (float)animationCount;
                while (sourceY < animationRate - sourceHeight)
                    sourceY += sourceHeight;
            }
            if (directionCount == 4)
            {
                sourceWidth /= directionCount;
                direction.Normalize();
                if (Math.Abs(direction.X) > Math.Abs(direction.Y))//lewo,prawo                
                {
                    if (direction.X < 0)//lewo
                        sourceX = 2 * sourceWidth;
                }
                else//góra, dó³                
                {
                    if (direction.Y < 0)//dó³
                        sourceX = 3 * sourceWidth;
                    else
                        sourceX = sourceWidth;
                }
            }
            if (directionCount == 8)
            {
                sourceWidth /= directionCount;
                direction.Normalize();

                if (Math.Abs(direction.X) <= 0.01)
                {
                    if (direction.Y > 0)
                        sourceX += 2 * sourceWidth;
                    else
                        sourceX += 6 * sourceWidth;
                }
                else
                {
                    double angle;
                    if (direction.X > 0) //prawa czêœæ
                        angle = Math.Asin(direction.Y);
                    else
                        angle = Math.PI - Math.Asin(direction.Y);
                    angle += Math.PI / 8;
                    if (angle < 0)
                        angle += 2 * Math.PI;
                    for (double atmp = Math.PI / 4; atmp < angle; atmp += Math.PI / 4)
                        sourceX += sourceWidth;
                }

            }
            if (directionCount == 16)
            {
                sourceWidth /= directionCount;
                direction.Normalize();

                if (Math.Abs(direction.X) <= 0.01)
                {
                    if (direction.Y > 0)
                        sourceX += 4 * sourceWidth;
                    else
                        sourceX += 12 * sourceWidth;
                }
                else
                {
                    double angle;
                    if (direction.X > 0) //prawa czêœæ
                        angle = Math.Asin(direction.Y);
                    else
                        angle = Math.PI - Math.Asin(direction.Y);
                    angle += Math.PI / 16;
                    if (angle < 0)
                        angle += 2 * Math.PI;
                    for (double atmp = Math.PI / 8; atmp < angle; atmp += Math.PI / 8)
                        sourceX += sourceWidth;
                }

            }
            return new Rectangle((int)sourceX, (int)sourceY, (int)sourceWidth, (int)sourceHeight);
        }
    }
    static class Textures
    {        
        public static Dictionary<string, ExtendedTexture> textures = new Dictionary<string,ExtendedTexture>();
        public static Dictionary<string, Texture2D> pictures = new Dictionary<string, Texture2D>();
        public static void Add(string str, ExtendedTexture texture)
        {
            Textures.textures.Add(str, texture);            
        }
        public static void Add(string str, Texture2D picture)
        {
            Textures.pictures.Add(str,picture);
        }
        public static ExtendedTexture GetTextureReference(string name)
        {           
            return Textures.textures[name];
        }
        public static Texture2D GetPictureReference(string name)
        {
            return Textures.pictures[name];
        }
        
    }
}
