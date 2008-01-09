using System;
using System.Collections.Generic;
using System.Text;
using System.Xml; 
using System.Collections;
using System.Xml.Schema;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;

namespace AntHill.NET
{
    class XmlReaderWriter
    {
        List<string> Errors = new List<string>();      

        bool ValidateMe()
        {
            XmlSchemaSet sc = new XmlSchemaSet();

            // Add the schema to the collection.
            sc.Add(null, "C:\\b.xsd");

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create("c:\\b.xml", settings);

            // Parse the file. 
            while (reader.Read()) ;

            if (Errors.Count == 0)
                return true;
            else
                return false;
        }
        
         // Display any validation errors.
        private  void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            Errors.Add(e.Message);
        }

        public void ReadMe(string filename)
        {
            int rowCount = 0;

            XmlTextReader textReader = null;

            try
            {
                textReader = new XmlTextReader(filename);
                textReader.Read();
                CultureInfo ci = CultureInfo.InvariantCulture;
                while (textReader.Read())
                {
                    if (textReader.HasAttributes)
                        switch (textReader.Name)
                        {
                            case "WorldMap":
                                {
                                    AntHillConfig.mapRowCount = int.Parse(textReader.GetAttribute("rowCount"));
                                    AntHillConfig.mapColCount = int.Parse(textReader.GetAttribute("colCount"));
                                    AntHillConfig.tiles = new Tile[AntHillConfig.mapColCount, AntHillConfig.mapRowCount];
                                } break;
                            case "Map":
                                {
                                    //If this is happens before 'WorldMap' then we're f***ed up
                                    //because AntHillConfig.tiles array won't be initialized.
                                    //I suggest we use more reliable xml parsing.
                                    if (rowCount >= AntHillConfig.mapRowCount)
                                    {
                                        rowCount++;
                                        break;
                                    }
                                    string s = textReader.GetAttribute("row");

                                    for (int i = 0; i < s.Length; ++i)
                                    {
                                        switch (s[i])
                                        {
                                            case 'o':
                                                AntHillConfig.tiles[i, rowCount] = new Tile(TileType.Indoor, new Position(i, rowCount));
                                                break;
                                            case 'x':
                                                AntHillConfig.tiles[i, rowCount] = new Tile(TileType.Wall, new Position(i, rowCount));
                                                break;
                                            case 's':
                                                AntHillConfig.tiles[i, rowCount] = new Tile(TileType.Outdoor, new Position(i, rowCount));
                                                break;
                                        }
                                    }
                                    rowCount++;
                                } break;
                            case "Ant":
                                {
                                    AntHillConfig.antMaxLife = int.Parse(textReader.GetAttribute("maxLife"));
                                    AntHillConfig.antMaxLifeWithoutFood = int.Parse(textReader.GetAttribute("maxLifeWithoutFood"));
                                    AntHillConfig.antTurnNumberToBecomeHungry = int.Parse(textReader.GetAttribute("turnNumberToBecomeHungry"));
                                    AntHillConfig.antStrength = int.Parse(textReader.GetAttribute("strength"));
                                    AntHillConfig.antForgettingTime = int.Parse(textReader.GetAttribute("forgettingTime"));
                                    AntHillConfig.antSightRadius = int.Parse(textReader.GetAttribute("sightRadius"));
                                } break;
                            case "Warrior":
                                {
                                    AntHillConfig.warriorStartCount = int.Parse(textReader.GetAttribute("startCount"));
                                } break;
                            case "Worker":
                                {
                                    AntHillConfig.workerStartCount = int.Parse(textReader.GetAttribute("startCount"));
                                } break;
                            case "Queen":
                                {
                                    AntHillConfig.queenLayEggProbability = float.Parse(textReader.GetAttribute("layEggProbability"), ci.NumberFormat);
                                    AntHillConfig.queenXPosition = int.Parse(textReader.GetAttribute("xPosition"));
                                    AntHillConfig.queenYPosition = int.Parse(textReader.GetAttribute("yPosition"));
                                } break;
                            case "Egg":
                                {
                                    AntHillConfig.eggHatchWarriorProbability = float.Parse(textReader.GetAttribute("hatchWarriorProbability"), ci.NumberFormat);
                                    AntHillConfig.eggHatchTime = int.Parse(textReader.GetAttribute("hatchTime"));
                                } break;
                            case "Spider":
                                {
                                    AntHillConfig.spiderMaxHealth = int.Parse(textReader.GetAttribute("maxHealth"));
                                    AntHillConfig.spiderProbability = float.Parse(textReader.GetAttribute("probability"), ci.NumberFormat);
                                    AntHillConfig.spiderFoodQuantityAfterDeath = int.Parse(textReader.GetAttribute("foodQuantityAfterDeath"));
                                } break;
                            case "Rain":
                                {
                                    AntHillConfig.rainWidth = int.Parse(textReader.GetAttribute("cloudWidth"));
                                    AntHillConfig.rainProbability = float.Parse(textReader.GetAttribute("probability"), ci.NumberFormat);
                                    AntHillConfig.rainMaxDuration = int.Parse(textReader.GetAttribute("maxDuration"));
                                } break;
                            case "Food":
                                {
                                    AntHillConfig.foodProbability = float.Parse(textReader.GetAttribute("probability"), ci.NumberFormat);
                                } break;
                            case "Signal":
                                {
                                    AntHillConfig.messageLifeTime = int.Parse(textReader.GetAttribute("lifeTime"));
                                    AntHillConfig.messageRadius = int.Parse(textReader.GetAttribute("radius"));
                                } break;
                        }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (textReader != null)
                    textReader.Close();
            }            
            if (rowCount < AntHillConfig.mapRowCount)
                throw new Exception("Not enough rows");
            //MessageBox.Show(rowCount.ToString());
        }
    }
}
