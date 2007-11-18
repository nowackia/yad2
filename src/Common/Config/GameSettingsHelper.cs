using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config.Common;
using System.Collections;
using Yad.Properties;

namespace Yad.Config {
	public class GameSettingsWrapper {
		short generatedID = 1;
		short GetTypeID() {
			return generatedID++;
		}

		private GameSettings gameSettings;
        public Dictionary<String, short> namesToIds = new Dictionary<string, short>();
		public Dictionary<short, AmmoData> ammosMap = new Dictionary<short, AmmoData>();
		public Dictionary<short, BuildingData> buildingsMap = new Dictionary<short, BuildingData>();
		public Dictionary<short, RaceData> racesMap = new Dictionary<short, RaceData>();
		public Dictionary<short, UnitHarvesterData> harvestersMap = new Dictionary<short, UnitHarvesterData>();
		public Dictionary<short, UnitMCVData> mcvsMap = new Dictionary<short, UnitMCVData>();
		public Dictionary<short, UnitSandwormData> sandwormsMap = new Dictionary<short, UnitSandwormData>();
		public Dictionary<short, UnitTankData> tanksMap = new Dictionary<short, UnitTankData>();
		public Dictionary<short, UnitTrooperData> troopersMap = new Dictionary<short, UnitTrooperData>();

		public GameSettingsWrapper(GameSettings gs) {
			this.gameSettings = gs;
			GameSettingsInitializer(gameSettings);
		}

		public GameSettings GameSettings {
			get { return this.gameSettings; }
		}

		private void GameSettingsInitializer(GameSettings gameSettings) {

			foreach (AmmoData ad in gameSettings.AmmosData.AmmoDataCollection) {
				ad.TypeID = GetTypeID();
                namesToIds.Add(ad.Name, ad.TypeID);
                ammosMap.Add(ad.TypeID, ad);
			}

			foreach (BuildingData bd in gameSettings.BuildingsData.BuildingDataCollection) {
				bd.TypeID = GetTypeID();
                namesToIds.Add(bd.Name, bd.TypeID);
				buildingsMap.Add(bd.TypeID, bd);
			}

			foreach (RaceData rd in gameSettings.RacesData.RaceDataCollection) {
				rd.TypeID = GetTypeID();
                namesToIds.Add(rd.Name, rd.TypeID);
				racesMap.Add(rd.TypeID, rd);
			}

			foreach (UnitHarvesterData uh in gameSettings.UnitHarvestersData.UnitHarvesterDataCollection) {
				uh.TypeID = GetTypeID();
                namesToIds.Add(uh.Name, uh.TypeID);
				harvestersMap.Add(uh.TypeID, uh);
			}

			foreach (UnitMCVData uh in gameSettings.UnitMCVsData.UnitMCVDataCollection) {
				uh.TypeID = GetTypeID();
                namesToIds.Add(uh.Name, uh.TypeID);
				mcvsMap.Add(uh.TypeID, uh);
			}

			foreach (UnitSandwormData uh in gameSettings.UnitSandwormsData.UnitSandwormDataCollection) {
				uh.TypeID = GetTypeID();
                namesToIds.Add(uh.Name, uh.TypeID);
				sandwormsMap.Add(uh.TypeID, uh);
			}

			foreach (UnitTankData uh in gameSettings.UnitTanksData.UnitTankDataCollection) {
				uh.TypeID = GetTypeID();
                namesToIds.Add(uh.Name, uh.TypeID);
				tanksMap.Add(uh.TypeID, uh);
			}

			foreach (UnitTrooperData uh in gameSettings.UnitTroopersData.UnitTrooperDataCollection) {
				uh.TypeID = GetTypeID();
                namesToIds.Add(uh.Name,uh.TypeID);
				troopersMap.Add(uh.TypeID, uh);
			}
		}
	}
}
