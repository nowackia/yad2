using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Dune_2_Remade
{
    public class Building:BaseObject
    {
        public int energyConsumption;
        public Point size;
        public List<string> canProduceBuildings=new List<string>();
        public List<string> canProduceUnits = new List<string>();
        public Building(Houses.House race, int userID, Vector3 position, int maxHealth, float viewRange, ExtendedTexture extendedTexture, int buildSpeed, int energyConsumption, Point size,string name)
            : base(race, userID, position, maxHealth, viewRange, extendedTexture, buildSpeed,name)
        {            
            this.size = size;
            this.energyConsumption = energyConsumption;
            selected = false;
        }

        public override void Render(Camera camera, GameTime gameTime, Houses.House house, Map map)
        {
            Render(camera, gameTime, house, map, Color.White);
        }
        public void Render(Camera camera, GameTime gameTime, Houses.House house, Map map, Color c)
        {
            Vector4 pos = new Vector4();
            pos.X = position.X - camera.GetPosition().X;
            pos.Y = position.Y - camera.GetPosition().Y;
            pos.W = (float)size.X;
            pos.Z = (float)size.Y;
            int a, b;
            if (camera.Visible(pos))
            {
                for (a = 0; a < size.X; a++)
                {
                    for (b = 0; b < size.Y; b++)
                    {
                        if (!map.fog[(int)position.X + a + map.size.X * ((int)position.Y + b)])
                            break;
                    }
                    if(b<size.Y)
                        break;
                }
                if (a < size.X)
                {
                    if(IsBeingBuilt)
                        camera.spriteBatch.Draw(GlobalData.Structure_Building[size.X][ size.Y].Texture, camera.GetRenderPosition(pos), GlobalData.Structure_Building[size.X][ size.Y].GetSourceRectangle(new Vector2(1, 0), gameTime), c);
                    else
                        camera.spriteBatch.Draw(extendedTexture.Texture, camera.GetRenderPosition(pos), extendedTexture.GetSourceRectangle(new Vector2(1, 0), gameTime), c);
                    if (selected)
                        camera.spriteBatch.Draw(selectTexture.Texture, camera.GetRenderPosition(pos), selectTexture.GetSourceRectangle(new Vector2(1, 0), gameTime), new Color(0, 255, 0, 128));
                }
            }
        }
        public static Building CreateNewBuilding(string name, int userID, Point position, Houses.House house)
        {
            BuildingData bd = GlobalData.gameSettings.GetBuildingByName(name);            
            Building b = new Building(house, userID, new Vector3((int)position.X, (int)position.Y, 0), bd.health, bd.viewRange, Textures.GetTextureReference(bd.textureName), bd.buildingSpeed, bd.energyConsumption, bd.size, name);
            b.pictureName = bd.pictureName;
            b.canProduceBuildings = bd.canProduceBuildings;
            b.canProduceUnits = bd.canProduceUnits;
            return b;
        }
    }
}
