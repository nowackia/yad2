using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Yad.Net.Messaging.Common;
using Yad.Net.Client;
using Yad.Board;
using Yad.UI.Client;
using Yad.Config.Common;
using Yad.Engine.Common;
using Yad.Board.Common;

namespace Yad.Engine.Client {
	static class GameLogic {
		private static short race;

		public static short Race { get { return race; } }
		public delegate void AddBuildingDelegate(short id, short key);
		public static event AddBuildingDelegate AddBuildingEvent;

		public delegate void AddUnitDelegate(string name, short key);
		public static event AddUnitDelegate AddUnitEvent;
		private static Dictionary<short, short> buldingCounter = new Dictionary<short, short>();
		/// <summary>;
		/// if gamer wants to locate builing on the map
		/// </summary>
		private static bool isLocatingBuilding = false;

		private static short buildingToBuild;

		private static short unitToCreate;

		private static bool isCreatingUnit = false;

		/// <summary>
		/// Waiting for building a building
		/// </summary>
		private static bool isWaitingForBuildingBuilt = false;

		/// <summary>
		/// Waiting for umit creation
		/// </summary>
		private static bool isWaitingForUnitCreation = false;

		private static bool buildingPositionOK(Position pos, short buildingTypeId) {
			BuildingData bd = GameForm.sim.GameSettingsWrapper.buildingsMap[buildingTypeId];

			Map map = GameForm.sim.Map;

			if ((pos.X + bd.Size.X - 1 >= map.Width)
				|| (pos.Y + bd.Size.Y >= map.Height)) {
				return false;
			}

			for (int x = pos.X; x < pos.X + bd.Size.X; x++) {
				for (int y = pos.Y; y < pos.Y + bd.Size.Y; y++) {
					if (map.Tiles[x, y] != TileType.Rock)
						return false;
					if (map.Units[x, y].Count > 0)
						return false;
					if (map.Buildings[x, y].Count > 0)
						return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Reaction on left mouse button on the map
		/// </summary>
		/// <param name="e"></param>
		public static void MouseLeftClick(GameForm gf, MouseEventArgs e) {
			Position pos = GameGraphics.TranslateMousePosition(e.Location);
			if (isLocatingBuilding) {

				//AAAAAAAAAA!  I KILL YOU!                                                                vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
				//if (buildingPositionOK(pos, GameForm.sim.GameSettingsWrapper.GameSettings.BuildingsData.BuildingDataCollection[0].TypeID)) {

				//OMG! I'M SO SCARED

				if (buildingPositionOK(pos, buildingToBuild)) {
					//TODO: to jeszcze poprawić w miarę potrzeby
					BuildMessage bm = (BuildMessage)Yad.Net.Client.Utils.CreateMessageWithPlayerId(MessageType.Build);
					bm.BuildingID = GameForm.currPlayer.GenerateObjectID();
					bm.PlayerId = GameForm.currPlayer.ID;
					bm.BuildingType = buildingToBuild;
					bm.Type = MessageType.Build;

					AddBuildingCounter(bm.BuildingType, race);
					AddUnitCreation(gf, bm.BuildingType);
					bm.Position = pos;
					bm.IdTurn = GameForm.sim.CurrentTurn + GameForm.sim.Delta;
					GameForm.conn.SendMessage(bm);
					isLocatingBuilding = false;
					foreach (TechnologyDependence techRef in GameForm.sim.GameSettingsWrapper.racesMap[race].TechnologyDependences) {
						short ids = GameForm.sim.GameSettingsWrapper.namesToIds[techRef.BuildingName];
						if (gf.IsStripContainingBuilding(ids) == true) continue;
						if (CheckReqBuildingsToAddNewBuilding(gf, techRef.RequiredBuildings)) {
							// adds new building to strip
							AddBuildingEvent(ids, race);
							

						}
					}
					//MessageBox.Show(pos.ToString());
				}
			}
		}

		/// <summary>
		/// Adds certain unit to the unic creation stripe as a possibility of creation a new unit
		/// </summary>
		/// <param name="p"></param>
		private static void AddUnitCreation(GameForm gf, short p) {
			short o;
			if (buldingCounter[p] == 1) {
				BuildingData b = GameForm.sim.GameSettingsWrapper.buildingsMap[p];
				foreach (string s in b.UnitsCanProduce) {
					if (GameForm.sim.GameSettingsWrapper.namesToIds.TryGetValue(s, out o))
					{
						AddUnitEvent(s, race);
					}
				}
			}
		}


		/// <summary>
		/// Retrieves race index in race collection by the race id
		/// </summary>
		/// <param name="raceId"></param>
		/// <returns></returns>
		[Obsolete("We've got Dictionaries in GameSettingsWrapper...")]
		public static short getRaceIdx(short raceId) {
			for (short i = 0; i < GameForm.sim.GameSettingsWrapper.GameSettings.RacesData.Count; i++)
				if (GameForm.sim.GameSettingsWrapper.GameSettings.RacesData[i].TypeID == raceId)
					return i;
			return -1;
		}

		public static bool IsWaitingForBuildingBuild {
			get {
				return isWaitingForBuildingBuilt;
			}
		}

		public static bool IsWaitingForUnitCreation {
			get {
				return isWaitingForUnitCreation;
			}
		}

		private static void AddBuildingCounter(short id, short key) {
			if (buldingCounter.ContainsKey(id))
				buldingCounter[id]++;
			else
				buldingCounter[id] = 1;


		}

		/// <summary>
		/// Checks requied building needed to build requested one
		/// </summary>
		/// <param name="gf"></param>
		/// <param name="coll"></param>
		/// <returns></returns>
		private static bool CheckReqBuildingsToAddNewBuilding(GameForm gf, BuildingsNames coll) {

			foreach (String buildingName in coll) {
				short id;
				short count;
				if (GameForm.sim.GameSettingsWrapper.namesToIds.TryGetValue(buildingName, out id)) {
					if (buldingCounter.TryGetValue(id, out count)) {
						if (count == 0)
							return false;
					} else return false;
				} else return false;
			}
			return true;
		}

		public static void LocateBuilding(short p) {
			isLocatingBuilding = true;
			buildingToBuild = p;
		}

		internal static void CreateUnit(short p) {
			isCreatingUnit = true;
			unitToCreate = p;
		}

		/// <summary>
		/// Initializating stripes and delegates
		/// </summary>
		/// <param name="name"></param>
		/// <param name="key"></param>
		internal static void InitStripes(string name, short key) {
			race = key;
			AddBuildingEvent(GameForm.sim.GameSettingsWrapper.namesToIds[name], key);


		}
	}
}
