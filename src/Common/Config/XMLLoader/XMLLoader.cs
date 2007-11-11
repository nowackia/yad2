using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Yad.Config.Common;

namespace Yad.Config.XMLLoader.Common {
	public static class XMLLoader {

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

			foreach (AmmoData ad in gameSettings.AmmosData.AmmoDataCollection) {
				ad.TypeID = GetTypeID();
			}

			foreach (BuildingData bd in gameSettings.BuildingsData.BuildingDataCollection) {
				bd.TypeID = GetTypeID();
			}

			foreach (RaceData rd in gameSettings.RacesData.RaceDataCollection) {
				rd.TypeID = GetTypeID();
			}

			foreach (UnitHarvesterData uh in gameSettings.UnitHarvestersData.UnitHarvesterDataCollection) {
				uh.TypeID = GetTypeID();
			}

			foreach (UnitMCVData uh in gameSettings.UnitMCVsData.UnitMCVDataCollection) {
				uh.TypeID = GetTypeID();
			}

			foreach (UnitSandwormData uh in gameSettings.UnitSandwormsData.UnitSandwormDataCollection) {
				uh.TypeID = GetTypeID();
			}

			foreach (UnitTankData uh in gameSettings.UnitTanksData.UnitTankDataCollection) {
				uh.TypeID = GetTypeID();
			}

			foreach (UnitTrooperData uh in gameSettings.UnitTroopersData.UnitTrooperDataCollection) {
				uh.TypeID = GetTypeID();
			}
		}

		static short generatedID;

		static void InitializeIDGenerator() {
			generatedID = 1;
		}

		static short GetTypeID() {
			return generatedID++;
		}
	}
}
