using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config.Common;

namespace Yad.Config {
	public class GameSettingsWrapper {
		short generatedID = 1;
		short GetTypeID() {
			return generatedID++;
		}

		private GameSettings gameSettings;

		private Dictionary<short, AmmoData> ammos = new Dictionary<short, AmmoData>();
		private Dictionary<short, BuildingData> buildings = new Dictionary<short, BuildingData>();
		private Dictionary<short, RaceData> races = new Dictionary<short, RaceData>();
		private Dictionary<short, UnitHarvesterData> harvesters = new Dictionary<short, UnitHarvesterData>();
		private Dictionary<short, UnitMCVData> mcvs = new Dictionary<short, UnitMCVData>();
		private Dictionary<short, UnitSandwormData> sandworms = new Dictionary<short, UnitSandwormData>();
		private Dictionary<short, UnitTankData> tanks = new Dictionary<short, UnitTankData>();
		private Dictionary<short, UnitTrooperData> troopers = new Dictionary<short, UnitTrooperData>();

		public GameSettingsWrapper(GameSettings gs) {
			this.gameSettings = gs;
			GameSettingsInitializer(gameSettings);
		}

		public GameSettings GameSettings {
			get { return gameSettings; }
		}

		private void GameSettingsInitializer(GameSettings gameSettings) {

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
	}
}
