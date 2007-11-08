using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Yad.Config.Common;

namespace Yad.Config.XMLLoader.Common {
	public static class XMLLoader {
		/*
        public static string xmlFile = "Config/dune_example.xml";
		public static string schema = "Config/dune.xsd";
        public static string validateNamespace = "http://www.example.org/dune";
		*/
		/*
        public static GameSettings getGameSettings()
        {
            if (GS == null)
            {
                try
                {
                    System.IO.FileStream sr = new FileStream(xmlFile, FileMode.Open);
                    XmlSerializer xmlSer = new XmlSerializer(typeof(GameSettings));
                    System.Xml.XmlReader xr = new XmlTextReader(sr);
                    XmlValidatingReader xvr = new XmlValidatingReader(xr);
					xvr.Schemas.Add("http://www.example.org/dune", schema);
                    GS = (GameSettings)xmlSer.Deserialize(xvr);
                    xvr.Close();
                    xr.Close();
                    sr.Close();
                }
                catch (Exception e)
                {
                    throw new XMLLoaderException(e);
                }
                
            }
            return GS;
        }
		 * */

		public static GameSettings get(String configFilePath, String configFileXSDPath) {
			try {
				FileStream sr = new FileStream(configFilePath, FileMode.Open);
				XmlSerializer xmlSer = new XmlSerializer(typeof(GameSettings));
				XmlReader xr = new XmlTextReader(sr);
				XmlValidatingReader xvr = new XmlValidatingReader(xr);
				xvr.Schemas.Add(Declarations.SchemaVersion, configFileXSDPath);
				GameSettings gameSettings = (GameSettings)xmlSer.Deserialize(xvr);
				GameSettingsInitializer(gameSettings);
				xvr.Close();
				xr.Close();
				sr.Close();

				return gameSettings;
			} catch (Exception e) {
				throw new XMLLoaderException(e);
			}
		}

		private static void GameSettingsInitializer(GameSettings gameSettings) {
			int id = 1;

			foreach (AmmoData ad in gameSettings.AmmosData.AmmoDataCollection) {
				ad.ID = GetID();
			}

			foreach (BuildingData bd in gameSettings.BuildingsData.BuildingDataCollection) {
				bd.ID = GetID();
			}

			foreach (RaceData rd in gameSettings.RacesData.RaceDataCollection) {
				rd.ID = GetID();
			}

			foreach (UnitHarvesterData uh in gameSettings.UnitHarvestersData.UnitHarvesterDataCollection) {
				uh.ID = GetID();
			}

			foreach (UnitMCVData uh in gameSettings.UnitMCVsData.UnitMCVDataCollection) {
				uh.ID = GetID();
			}

			foreach (UnitSandwormData uh in gameSettings.UnitSandwormsData.UnitSandwormDataCollection) {
				uh.ID = GetID();
			}

			foreach (UnitTankData uh in gameSettings.UnitTanksData.UnitTankDataCollection) {
				uh.ID = GetID();
			}

			foreach (UnitTrooperData uh in gameSettings.UnitTroopersData.UnitTrooperDataCollection) {
				uh.ID = GetID();
			}
		}

		static int generatedID;

		static void InitializeIDGenerator() {
			generatedID = 1;
		}

		static int GetID() {
			return generatedID++;
		}
	}
}
