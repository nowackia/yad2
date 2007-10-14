using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Dune_2_Remade
{
    [Serializable]
    public class GameSettings
    {
        public List<UnitData> UnitsData = new List<UnitData>();
        public List<BuildingData> BuildingsData = new List<BuildingData>();

        public static GameSettings LoadSettings(string str)
        {
            GameSettings gs = new GameSettings();
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
            XmlTextReader reader = new XmlTextReader("Dune.xml");
            return (GameSettings)serializer.Deserialize(reader);
        }

        public void SaveSettings(string str)
        {
            XmlTextWriter writer = new XmlTextWriter(str, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
            serializer.Serialize(writer, this);
        }

        public UnitData GetUnitByName(string str)
        {
            for (int i=0; i<UnitsData.Count; i++)
                if (UnitsData[i].name == str) return UnitsData[i];
            return null;
        }

        public BuildingData GetBuildingByName(string str)
        {
            for (int i=0; i<BuildingsData.Count; i++)
                if (BuildingsData[i].name == str) return BuildingsData[i];
            return null;
        }
    }

    public class ObjectData
    {
        public int cost;
        public string pictureName;
        public string name;
        public string textureName;
        public int health;
        public int buildingSpeed;
        public float viewRange;
        
    }

    [Serializable]
    public class BuildingData : ObjectData
    {
        public Point size;
        public int[] rideableFields;
        public int energyConsumption;
        public List<string> canProduceBuildings;
        public List<string> canProduceUnits;
    }

    [Serializable]
    public class UnitData : ObjectData
    {
        public float speed;
        public bool isFlying;
        public float turretRotationSpeed;
        public float fireRange;
        public int reloadTime; //miliseconds
        public int ammunitionType; //Nad typem jeszcze pomyœlê
        public string turretTextureName;
        //Amunicja: Pociski mo¿emy traktowaqæ jako Units lub stworzyæ im now¹ klasê Narazie zak³adam, ¿e s¹ Unit-sami wiêc wpiszê tu odpowiednie dane
        public int damage;
        public float radius;
        public float accuracy;
        public bool antiAircraft;
    }
}
