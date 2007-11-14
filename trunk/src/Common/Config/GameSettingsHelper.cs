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

		public Dictionary<short, AmmoData> ammos = new Dictionary<short, AmmoData>();
		public Dictionary<short, BuildingData> buildingsMap = new Dictionary<short, BuildingData>();
		public Dictionary<short, RaceData> races = new Dictionary<short, RaceData>();
		public Dictionary<short, UnitHarvesterData> harvesters = new Dictionary<short, UnitHarvesterData>();
		public Dictionary<short, UnitMCVData> mcvs = new Dictionary<short, UnitMCVData>();
		public Dictionary<short, UnitSandwormData> sandworms = new Dictionary<short, UnitSandwormData>();
		public Dictionary<short, UnitTankData> tanks = new Dictionary<short, UnitTankData>();
		public Dictionary<short, UnitTrooperData> troopers = new Dictionary<short, UnitTrooperData>();

		public List<BuildingData> buildings = new List<BuildingData>();

		public GameSettingsWrapper(GameSettings gs) {
			this.gameSettings = gs;
			GameSettingsInitializer(gameSettings);
		}

		private void GameSettingsInitializer(GameSettings gameSettings) {

			foreach (AmmoData ad in gameSettings.AmmosData.AmmoDataCollection) {
				ad.TypeID = GetTypeID();
				ammos.Add(ad.TypeID, ad);
			}

			foreach (BuildingData bd in gameSettings.BuildingsData.BuildingDataCollection) {
				bd.TypeID = GetTypeID();
				buildingsMap.Add(bd.TypeID, bd);
				buildings.Add(bd);
			}

			foreach (RaceData rd in gameSettings.RacesData.RaceDataCollection) {
				rd.TypeID = GetTypeID();
				races.Add(rd.TypeID, rd);
			}

			foreach (UnitHarvesterData uh in gameSettings.UnitHarvestersData.UnitHarvesterDataCollection) {
				uh.TypeID = GetTypeID();
				harvesters.Add(uh.TypeID, uh);
			}

			foreach (UnitMCVData uh in gameSettings.UnitMCVsData.UnitMCVDataCollection) {
				uh.TypeID = GetTypeID();
				mcvs.Add(uh.TypeID, uh);
			}

			foreach (UnitSandwormData uh in gameSettings.UnitSandwormsData.UnitSandwormDataCollection) {
				uh.TypeID = GetTypeID();
				sandworms.Add(uh.TypeID, uh);
			}

			foreach (UnitTankData uh in gameSettings.UnitTanksData.UnitTankDataCollection) {
				uh.TypeID = GetTypeID();
				tanks.Add(uh.TypeID, uh);
			}

			foreach (UnitTrooperData uh in gameSettings.UnitTroopersData.UnitTrooperDataCollection) {
				uh.TypeID = GetTypeID();
				troopers.Add(uh.TypeID, uh);
			}
		}
	}
}
