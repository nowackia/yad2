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
    [Serializable]
    public struct ID
    {
        public int userID, objectID;
        public ID(int userID, int objectID)
        {
            this.userID = userID;
            this.objectID = objectID;
        }        
    }        
    public abstract class BaseObject
    {
        public int damage;
        public string pictureName;
        public bool selected = false;
        public Houses.House race;
        public string name;
        public ID objectID;
        public Vector3 position;
        public int maxHealth;
        public float currentHealth;
        public float viewRange;
        public ExtendedTexture selectTexture;
        public ExtendedTexture extendedTexture;
        private bool isBeingBuilt = true;
        public int buildSpeed;

        public bool IsBeingBuilt
        {
            get
            {
                return isBeingBuilt;
            }
            set
            {
                if (isBeingBuilt == true && value == false && objectID.userID == GlobalData.me.userID)
                {
                    DuneGame.RacesSB.PlayCue(GlobalData.me.abb + "const");
                    if (this.name == "Radar")
                        DuneGame.MiscSB.PlayCue("radar");
                }
                isBeingBuilt = value;
            }
        }
        public BaseObject(Houses.House race, int userID, Vector3 position, int maxHealth, float viewRange, ExtendedTexture extendedTexture,int buildSpeed,string name)
        {
            this.name = name;
            this.race = race;
            this.objectID.userID = userID;
            this.objectID.objectID = BaseObject.GetNewID();
            this.position = position;         
            this.maxHealth = maxHealth;
            this.currentHealth = 1;
            this.viewRange = viewRange;
            this.extendedTexture = new ExtendedTexture(extendedTexture);
            this.isBeingBuilt = true;
            this.buildSpeed = buildSpeed;
            this.selectTexture = new ExtendedTexture(GlobalData.objectSelected);
        }
        abstract public void Render(Camera camera, GameTime gameTime, Houses.House house, Map map);        
        static private int ID = 0;
        public static int GetNewID()
        {
            ID++;
            return ID;
        }
    }
}
