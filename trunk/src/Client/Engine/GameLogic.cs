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
using Yad.Log.Common;
using Yad.Config;
using Yad.Config.XMLLoader.Common;
using Yad.Properties;
using System.IO;
using Yad.Utilities.Common;
using Yad.Properties.Common;
using Yad.Net.Common;

namespace Yad.Engine.Client {
	public class GameLogic {
		ClientSimulation sim;
		Player currPlayer;
		IConnection conn;
		short race;

		public GameLogic() {

			GameSettingsWrapper gameSettingsWrapper = XMLLoader.get(Settings.Default.ConfigFile, Settings.Default.ConfigFileXSD);
			Map map = new Map();
			map.LoadMap(Path.Combine(Settings.Default.Maps, "test.map"));
			currPlayer = new Player(0);

			conn = Connection.Instance;
			GameMessageHandler.Instance.GameMessageReceive += new GameMessageEventHandler(Instance_GameMessageReceive);
			GameMessageHandler.Instance.DoTurnPermission += new DoTurnEventHandler(Instance_DoTurnPermission);
			GameMessageHandler.Instance.GameInitialization += new GameInitEventHandler(Instance_GameInitialization);
			sim = new ClientSimulation(gameSettingsWrapper, map, currPlayer, conn);
			Connection.Instance.ResumeReceiving();
		}

		void Instance_GameInitialization(object sender, GameInitEventArgs e) {
			PositionData[] aPd = e.gameInitInfo;

			foreach (PositionData pd in aPd) {
				Player p = new Player(pd.PlayerId);
				sim.AddPlayer(p);
				//FIXME!
				UnitTank u = new UnitTank(p.ID, p.GenerateObjectID(), sim.GameSettingsWrapper.GameSettings.UnitTanksData[0], new Position((short)pd.X, (short)pd.Y), this.sim.Map);
				p.AddUnit(u);
				this.sim.Map.Units[u.Position.X, u.Position.Y].AddLast(u);
				//FIXME END
			}
			/*
			sim.StartSimulation();
			sim.DoTurn();
			 */
		}

		void Instance_DoTurnPermission(object sender, EventArgs e) {
			InfoLog.WriteInfo("Turn permitted", EPrefix.SimulationInfo);
			sim.DoTurn();
		}

		void Instance_GameMessageReceive(object sender, GameMessageEventArgs e) {
			this.sim.AddGameMessage(e.gameMessage);
		}

		[Obsolete("Remove when connection to server is ready. This is just for GameForm constructor")]
		public void StartSimulation() {
			this.sim.StartSimulation();
		}

		public short Race { get { return race; } }

		public delegate void AddBuildingDelegate(short id, short key);
		public event AddBuildingDelegate AddBuildingEvent;

		public delegate void AddUnitDelegate(string name, short key);
		public event AddUnitDelegate AddUnitEvent;
		private Dictionary<short, short> buldingCounter = new Dictionary<short, short>();
        public bool hasBuilding(short id) {
            short c;
            if (buldingCounter.TryGetValue(id, out c)) {
                return c > 0;
            }
            return false;
        }
		/// <summary>;
		/// if gamer wants to locate builing on the map
		/// </summary>
		private bool isLocatingBuilding = false;

		private short buildingToBuild;

		private short unitToCreate;

		private bool isCreatingUnit = false;

		/// <summary>
		/// Waiting for building a building
		/// </summary>
		private bool isWaitingForBuildingBuilt = false;

		/// <summary>
		/// Waiting for umit creation
		/// </summary>
		private bool isWaitingForUnitCreation = false;

		private List<Unit> selectedUnits = new List<Unit>();

		/// <summary>
		/// Defined groups: Ctrl+1 - Ctrl+4
		/// </summary>
		private List<Unit>[] definedGroups = new List<Unit>[4];

		private Building selectedBuilding = null;

		private bool buildingPositionOK(Position pos, short buildingTypeId) {
			BuildingData bd = sim.GameSettingsWrapper.buildingsMap[buildingTypeId];

			Map map = sim.Map;

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
		public void MouseLeftClick(GameForm gf, MouseEventArgs e) {
			Position pos = GameGraphics.TranslateMousePosition(e.Location);
			if (isLocatingBuilding) {

				//AAAAAAAAAA!  I KILL YOU!                                                                vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
				//if (buildingPositionOK(pos, GameForm.sim.GameSettingsWrapper.GameSettings.BuildingsData.BuildingDataCollection[0].TypeID)) {

				//OMG! I'M SO SCARED

                //Silence! I kill you!

				if (buildingPositionOK(pos, buildingToBuild)) {
					//TODO: to jeszcze poprawić w miarę potrzeby
					BuildMessage bm = (BuildMessage)Yad.Net.Client.Utils.CreateMessageWithPlayerId(MessageType.Build);
					bm.BuildingID = currPlayer.GenerateObjectID();
					bm.PlayerId = currPlayer.ID;
					bm.BuildingType = buildingToBuild;
					bm.Type = MessageType.Build;

					AddBuildingCounter(bm.BuildingType, race);
					AddUnitCreation(gf, bm.BuildingType);
					bm.Position = pos;
					bm.IdTurn = sim.CurrentTurn + sim.Delta;
					conn.SendMessage(bm);
					isLocatingBuilding = false;
					foreach (TechnologyDependence techRef in sim.GameSettingsWrapper.racesMap[race].TechnologyDependences) {
						short ids = sim.GameSettingsWrapper.namesToIds[techRef.BuildingName];
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
		private void AddUnitCreation(GameForm gf, short p) {
			short o;
			if (buldingCounter[p] == 1) {
				BuildingData b = sim.GameSettingsWrapper.buildingsMap[p];
				foreach (string s in b.UnitsCanProduce) {
					if (sim.GameSettingsWrapper.namesToIds.TryGetValue(s, out o))
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
		public short getRaceIdx(short raceId) {
			for (short i = 0; i < sim.GameSettingsWrapper.GameSettings.RacesData.Count; i++)
				if (sim.GameSettingsWrapper.GameSettings.RacesData[i].TypeID == raceId)
					return i;
			return -1;
		}

		public bool IsWaitingForBuildingBuild {
			get {
				return isWaitingForBuildingBuilt;
			}
		}

		public bool IsWaitingForUnitCreation {
			get {
				return isWaitingForUnitCreation;
			}
		}

		private void AddBuildingCounter(short id, short key) {
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
		private bool CheckReqBuildingsToAddNewBuilding(GameForm gf, BuildingsNames coll) {

			foreach (String buildingName in coll) {
				short id;
				short count;
				if (sim.GameSettingsWrapper.namesToIds.TryGetValue(buildingName, out id)) {
					if (buldingCounter.TryGetValue(id, out count)) {
						if (count == 0)
							return false;
					} else return false;
				} else return false;
			}
			return true;
		}

		public void LocateBuilding(short p) {
			isLocatingBuilding = true;
			buildingToBuild = p;
		}

		internal void CreateUnit(short p) {
			isCreatingUnit = true;
			unitToCreate = p;
		}

		/// <summary>
		/// Initializating stripes and delegates
		/// </summary>
		/// <param name="name"></param>
		/// <param name="key"></param>
		internal void InitStripes(string name, short key) {
			race = key;
            short id = sim.GameSettingsWrapper.namesToIds[name];
            AddBuildingCounter(id, key);
			AddBuildingEvent(id, key);


		}

		public bool Select(Position pos) {

			InfoLog.WriteInfo("Selecting position: " + pos.ToString(), EPrefix.GameLogic);
			selectedUnits.Clear();
			selectedBuilding = null;

			LinkedList<Unit> unitsOnPos = sim.Map.Units[pos.X, pos.Y];
			selectedUnits.AddRange(unitsOnPos);
			if (selectedUnits.Count != 0) {
				return true;
			}

			LinkedList<Building> buildingOnPos = sim.Map.Buildings[pos.X, pos.Y];
			if (buildingOnPos.Count != 0) {
				selectedBuilding = buildingOnPos.First.Value;
				return true;
			}
			return false;
		}

		public bool Select(Position a, Position b) {
			selectedUnits.Clear();
			selectedBuilding = null;

			Utilities.Common.UsefulFunctions.CorrectPosition(ref a, sim.Map.Width, sim.Map.Height);
			Utilities.Common.UsefulFunctions.CorrectPosition(ref b, sim.Map.Width, sim.Map.Height);

			int xMin = Math.Min(a.X, b.X);
			int xMax = Math.Max(a.X, b.X);
			int yMin = Math.Min(a.Y, b.Y);
			int yMax = Math.Max(a.Y, b.Y);

			LinkedList<Unit> unitsOnPos;
			for (int x = xMin; x <= xMax; x++) {
				for (int y = yMin; y <= yMax; y++) {
					unitsOnPos = sim.Map.Units[x, y];
					selectedUnits.AddRange(unitsOnPos);
				}
			}
			if (selectedUnits.Count != 0) {
				return true;
			}

			LinkedList<Building> buildingOnPos;
			for (int x = xMin; x <= xMax; x++) {
				for (int y = yMin; y <= yMax; y++) {
					buildingOnPos = sim.Map.Buildings[x, y];
					if (buildingOnPos.Count != 0) {
						selectedBuilding = buildingOnPos.First.Value;
						return true;
					}
				}
			}
			return false;
		}

		public List<Unit> SelectedUnits {
			get { return selectedUnits; }
		}

		public Building SelectedBuilding {
			get { return selectedBuilding; }
		}

		public ClientSimulation Simulation {
			get { return this.sim; }
		}

		public GameSettingsWrapper GameSettingsWrapper {
			get { return this.sim.GameSettingsWrapper; }
		}

		internal void IssuedOrder(Position newPos) {

			if (selectedUnits.Count != 0) {
				foreach (Unit u in selectedUnits) {
					//if no enemy at newPos

					MoveMessage mm = (MoveMessage)MessageFactory.Create(MessageType.Move);
					mm.IdUnit = u.ObjectID;
					mm.Path = newPos;
					mm.PlayerId = currPlayer.ID;
					Connection.Instance.SendMessage(mm);
					//if enemy - attack

				}
				return;
			}
		}
	}

	public class DummyConnection : IConnection {
		int currentTurn = 0;
		public Simulation sim;

		public void SendMessage(Yad.Net.Messaging.Common.Message message) {
			if (message.Type == MessageType.TurnAsk) {
				currentTurn++;
				sim.DoTurn();
			} else if (message is GameMessage) {
				GameMessage gm = message as GameMessage;
				gm.IdTurn = currentTurn + sim.Delta;
				sim.AddGameMessage(gm);
			}
		}

		public void CloseConnection() {

		}

		public void InitConnection(string host, int port) {

		}
	}
}
