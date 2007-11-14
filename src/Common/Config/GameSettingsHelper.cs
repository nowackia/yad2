using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config.Common;
using System.Collections;

namespace Yad.Config {
	public class GameSettingsWrapper {
		short generatedID = 1;
		short GetTypeID() {
			return generatedID++;
		}

		private GameSettings gameSettings;

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

			foreach (AmmoData o in gameSettings.AmmosData.AmmoDataCollection) {
				o.TypeID = GetTypeID();
				ammosMap.Add(o.TypeID, o);
			}

			foreach (BuildingData o in gameSettings.BuildingsData.BuildingDataCollection) {
				o.TypeID = GetTypeID();
				buildingsMap.Add(o.TypeID, o);
			}

			foreach (RaceData o in gameSettings.RacesData.RaceDataCollection) {
				o.TypeID = GetTypeID();
				racesMap.Add(o.TypeID, o);
			}

			foreach (UnitHarvesterData o in gameSettings.UnitHarvestersData.UnitHarvesterDataCollection) {
				o.TypeID = GetTypeID();
				harvestersMap.Add(o.TypeID, o);
			}

			foreach (UnitMCVData o in gameSettings.UnitMCVsData.UnitMCVDataCollection) {
				o.TypeID = GetTypeID();
				mcvsMap.Add(o.TypeID, o);
			}

			foreach (UnitSandwormData o in gameSettings.UnitSandwormsData.UnitSandwormDataCollection) {
				o.TypeID = GetTypeID();
				sandwormsMap.Add(o.TypeID, o);
			}

			foreach (UnitTankData o in gameSettings.UnitTanksData.UnitTankDataCollection) {
				o.TypeID = GetTypeID();
				tanksMap.Add(o.TypeID, o);
			}

			foreach (UnitTrooperData o in gameSettings.UnitTroopersData.UnitTrooperDataCollection) {
				o.TypeID = GetTypeID();
				troopersMap.Add(o.TypeID, o);
			}
		}
	}
}
