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
    public enum AmmunitionType {Bullet, Rocket};
    public class Unit:BaseObject
    {
        public BaseObject target = null;
        public List<Point> way = new List<Point>();
        public float speed;
        private bool moving = false;
        public Vector2 direction;
        public ExtendedTexture turretTexture;
        public Vector2 turretDirection;
        public float turretRotationSpeed;
        public bool isFlying;
        public float fireRange;
        public int reloadTime; //miliseconds
        public int reloadTimePassed = 0;
        public AmmunitionType ammunitionType;

        public bool Moving
        {
            get
            {
                return moving;
            }
            set
            {
                if (this.objectID.userID == GlobalData.me.userID && moving == false && value == true)
                    DuneGame.MiscSB.PlayCue("movingOut");
                moving = value;
                extendedTexture.ResetTime();
            }        
        }        
        public Unit(Houses.House race, int userID, Vector3 position, int maxHealth, float viewRange, ExtendedTexture extendedTexture, int buildSpeed, float speed, string name, Vector2 direction): base(race, userID, position, maxHealth, viewRange, extendedTexture, buildSpeed,name)
        {
            this.speed = Houses.CalculateSpeed(race, speed);
            this.direction = direction;
            moving = false;
            selected = false;
        }
        public Unit(Houses.House race, int userID, Vector3 position, int maxHealth, float viewRange, ExtendedTexture extendedTexture, int buildSpeed, float speed, string name)
            : base(race, userID, position, Houses.CalculateHealth(race,maxHealth), viewRange, extendedTexture, buildSpeed,name)
        {
            this.speed = Houses.CalculateSpeed(race, speed);
            this.direction = new Vector2(1.0f,0.0f);
            moving = false;
            selected = false;
        }
        public override void Render(Camera camera, GameTime gameTime, Houses.House house, Map map)
        {
            Vector4 pos = new Vector4();
            pos.X = position.X - camera.GetPosition().X;
            pos.Y = position.Y - camera.GetPosition().Y;
            pos.W = 1.0f;
            pos.Z = 1.0f;
            if (camera.Visible(pos))
            {
                if (!IsBeingBuilt)
                {
                    if (!map.fog[(int)position.X + map.size.X * (int)position.Y])
                    {
                        if (moving)
                        {
                            camera.spriteBatch.Draw(extendedTexture.Texture, camera.GetRenderPosition(pos), extendedTexture.GetSourceRectangle(direction, gameTime), Houses.GetColor(house));
                            if (turretTexture != null)
                                camera.spriteBatch.Draw(turretTexture.Texture, camera.GetRenderPosition(pos), extendedTexture.GetSourceRectangle(turretDirection, gameTime), Houses.GetColor(house));
                        }
                        else
                        {
                            camera.spriteBatch.Draw(extendedTexture.Texture, camera.GetRenderPosition(pos), extendedTexture.GetSourceRectangle(direction, 0.0f), Houses.GetColor(house));
                            if (turretTexture != null)
                                camera.spriteBatch.Draw(turretTexture.Texture, camera.GetRenderPosition(pos), extendedTexture.GetSourceRectangle(turretDirection, 0.0f), Houses.GetColor(house));
                        }
                        if (selected)
                            camera.spriteBatch.Draw(selectTexture.Texture, camera.GetRenderPosition(pos), selectTexture.GetSourceRectangle(direction, gameTime), new Color(0, 255, 0, 128));
                    }
                }
            }
        }       
        public static Unit CreateNewUnit(string name, int userID, Point position, Houses.House house)
        {            
            UnitData ud = GlobalData.gameSettings.GetUnitByName(name);            
            Unit u = new Unit(house, userID, new Vector3((int)position.X, (int)position.Y, 0), ud.health, ud.viewRange, Textures.GetTextureReference(ud.textureName), ud.buildingSpeed, ud.speed,name);
            if(ud.turretTextureName!=null)
                if (ud.turretTextureName == "")
                    u.turretTexture = null;
            else
                u.turretTexture = new ExtendedTexture(Textures.GetTextureReference(ud.turretTextureName));
            u.turretDirection = u.direction;
            u.turretRotationSpeed = ud.turretRotationSpeed;
            u.isFlying=ud.isFlying;
            u.fireRange=ud.fireRange;
            u.reloadTime=ud.reloadTime;
            u.pictureName = ud.pictureName;
            u.IsBeingBuilt = true;
            u.target = null;
            u.damage = ud.damage;
            u.reloadTimePassed = -1;
            switch(ud.ammunitionType)
            {
                case 0:
                    u.ammunitionType=AmmunitionType.Bullet;
                    break;
                case 1:
                    u.ammunitionType=AmmunitionType.Rocket;
                    break;
                default:
                    u.ammunitionType=AmmunitionType.Bullet;
                    break;
            }            
            return u;
        }
    }
}