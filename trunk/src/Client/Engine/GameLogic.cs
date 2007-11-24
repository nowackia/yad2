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
using System.Drawing;

namespace Yad.Engine.Client {
	/// <summary>
	/// This is our GameLogic. There are many like it, but this one is OURS.
	/// </summary>
	public class GameLogic {
		#region events
		public delegate void NewUnitDelegate(string name, short key);
		public delegate void BadLocationHandler();

		public event NewUnitDelegate OnNewUnit;
		public event BadLocationHandler OnBadLocation;
		#endregion

		#region private members
		ClientSimulation _sim;
		Player _currPlayer;

		private Dictionary<short, short> _buildingCounter = new Dictionary<short, short>();
		private List<Unit> _selectedUnits = new List<Unit>();

		/// <summary>
		/// Defined groups: Ctrl+1 - Ctrl+4
		/// </summary>
		private List<Unit>[] _definedGroups = new List<Unit>[4];
		private Building _selectedBuilding = null;
		#endregion

		#region constructor
		public GameLogic() {
			GameSettingsWrapper wrapper = GlobalSettings.Wrapper;
			Map map = new Map();
			map.LoadMap(Path.Combine(Settings.Default.Maps, ClientPlayerInfo.GameInfo.MapName));
			
			PlayerInfo currPI = ClientPlayerInfo.Player;
			//TODO: usunąć gdy Adam/Piotrek dodadzą ustawianie rasy do ClientPlayerInfo
			//currPI.House = wrapper.Races[0].TypeID;
			//
			_currPlayer = new Player(currPI.Id, currPI.Name, currPI.House, currPI.Color);

			GameMessageHandler.Instance.GameMessageReceive += new GameMessageEventHandler(Instance_GameMessageReceive);
			GameMessageHandler.Instance.DoTurnPermission += new DoTurnEventHandler(Instance_DoTurnPermission);
			GameMessageHandler.Instance.GameInitialization += new GameInitEventHandler(Instance_GameInitialization);
			_sim = new ClientSimulation(map, _currPlayer);
			
			//Add all players
			foreach (PlayerInfo pi in ClientPlayerInfo.GetAllPlayers()) {
				Player p = new Player(pi.Id, pi.Name, pi.House, pi.Color);
				//TODO: usunąć gdy Adam/Piotrek dodadzą ustawianie rasy do ClientPlayerInfo
				//p.House = wrapper.Races[0].TypeID;
				//
				_sim.AddPlayer(p);
			}

			_sim.BuildingCompleted += new ClientSimulation.BuildingHandler(_sim_OnBuildingCompleted);


			//GameMessageHandler.Instance.Resume();
		}

		void _sim_OnBuildingCompleted(Building b) {
			IncreaseBuildingCounter(b.TypeID);
		}

		#endregion

		#region message handling
		void Instance_GameInitialization(object sender, GameInitEventArgs e) {
			PositionData[] aPd = e.gameInitInfo;
			
			foreach (PositionData pd in aPd) {
				//TODO: get info
				Player p = _sim.Players[pd.PlayerId];
				ObjectID mcvID = new ObjectID(p.Id, p.GenerateObjectID());
                UnitMCV mcv = new UnitMCV(mcvID, GlobalSettings.Wrapper.MCVs[0], new Position(pd.X, pd.Y), _sim.Map, this._sim);
				p.AddUnit(mcv);

				// vjust for fun ;p
				ObjectID tankID = new ObjectID(p.Id, p.GenerateObjectID());
                UnitTank u = new UnitTank(tankID, GlobalSettings.Wrapper.Tanks[0], new Position((short)((pd.X + 1) % _sim.Map.Width), pd.Y), this._sim.Map, this._sim);
				p.AddUnit(u);
				// ^

				//this.sim.Map.Units[u.Position.X, u.Position.Y].AddLast(u);
			}

			this._sim.StartSimulation();
			/*
			sim.DoTurn();
			 */
		}

		void Instance_DoTurnPermission(object sender, EventArgs e) {
			//InfoLog.WriteInfo("Turn permitted", EPrefix.SimulationInfo);
			_sim.DoTurn();
		}

		void Instance_GameMessageReceive(object sender, GameMessageEventArgs e) {
			this._sim.AddGameMessage(e.gameMessage);
		}
		#endregion

		#region properties
		public List<Unit> SelectedUnits {
			get { return _selectedUnits; }
		}

		public Building SelectedBuilding {
			get { return _selectedBuilding; }
		}

		public ClientSimulation Simulation {
			get { return this._sim; }
		}

		public Player CurrentPlayer {
			get { return this._currPlayer; }
		}
		#endregion

		#region user orders

        /// <summary>
        /// checks if 
        /// </summary>
        /// <returns></returns>
        public bool CanGiveOrders() {
            foreach (Unit unit in SelectedUnits) {
                if (unit.ObjectID.PlayerID.Equals(CurrentPlayer.Id)) return true;
            }
            //if (SelectedBuilding.ObjectID.PlayerID.Equals(CurrentPlayer.Id)) return true;
            
            return false;
        }

		public bool Select(Position pos) {

			InfoLog.WriteInfo("Selecting position: " + pos.ToString(), EPrefix.GameLogic);
			_selectedUnits.Clear();
			_selectedBuilding = null;

			LinkedList<Unit> unitsOnPos = _sim.Map.Units[pos.X, pos.Y];
			if (unitsOnPos.Count != 0) {
				_selectedUnits.Add(unitsOnPos.First.Value);
				return true;
			}

			LinkedList<Building> buildingOnPos = _sim.Map.Buildings[pos.X, pos.Y];
			if (buildingOnPos.Count != 0) {
				_selectedBuilding = buildingOnPos.First.Value;
				return true;
			}
			return false;
		}

        public BoardObject SimpleSelectAttack(Position pos) {
            
            
            LinkedList<Unit> unitsOnPos = _sim.Map.Units[pos.X, pos.Y];
            if (unitsOnPos.Count != 0) {
                return unitsOnPos.First.Value;
            }

            LinkedList<Building> buildingOnPos = _sim.Map.Buildings[pos.X, pos.Y];
            if (buildingOnPos.Count != 0) {
                return buildingOnPos.First.Value;
            }
            return null;
        }

		public bool Select(Position a, Position b) {
			_selectedUnits.Clear();
			_selectedBuilding = null;

			Utilities.Common.UsefulFunctions.CorrectPosition(ref a, _sim.Map.Width, _sim.Map.Height);
			Utilities.Common.UsefulFunctions.CorrectPosition(ref b, _sim.Map.Width, _sim.Map.Height);

			int xMin = Math.Min(a.X, b.X);
			int xMax = Math.Max(a.X, b.X);
			int yMin = Math.Min(a.Y, b.Y);
			int yMax = Math.Max(a.Y, b.Y);

			LinkedList<Unit> unitsOnPos;
			for (int x = xMin; x <= xMax; x++) {
				for (int y = yMin; y <= yMax; y++) {
					unitsOnPos = _sim.Map.Units[x, y];
					_selectedUnits.AddRange(unitsOnPos);
				}
			}
			if (_selectedUnits.Count != 0) {
				return true;
			}

			LinkedList<Building> buildingOnPos;
			for (int x = xMin; x <= xMax; x++) {
				for (int y = yMin; y <= yMax; y++) {
					buildingOnPos = _sim.Map.Buildings[x, y];
					if (buildingOnPos.Count != 0) {
						_selectedBuilding = buildingOnPos.First.Value;
						return true;
					}
				}
			}
			return false;
		}

        internal void AttackOrder(BoardObject attacked) {
            if (_selectedUnits.Count == 0 && _selectedBuilding == null) {
                return;
            }
            if (_selectedBuilding != null) {
                return;
                //TODO RS: user can order building to attack?
                AttackMessage am = (AttackMessage)MessageFactory.Create(MessageType.Attack);
                am.Attacker = _selectedBuilding.ObjectID;
                am.Attacked = attacked.ObjectID;
                am.IdPlayer = _selectedBuilding.ObjectID.PlayerID;
                Connection.Instance.SendMessage(am);
            }

            foreach (Unit u in _selectedUnits) {
                if (u.ObjectID.PlayerID != CurrentPlayer.Id) continue;
                AttackMessage am = (AttackMessage)MessageFactory.Create(MessageType.Attack);
                am.Attacker = u.ObjectID;
                am.Attacked = attacked.ObjectID;
                am.IdPlayer = u.ObjectID.PlayerID;
                Connection.Instance.SendMessage(am);
            }
        }

		internal void MoveOrder(Position newPos) {
			if (_selectedUnits.Count == 0) {
				return;
			}
			foreach (Unit u in _selectedUnits) {
				MoveMessage mm = (MoveMessage)MessageFactory.Create(MessageType.Move);
				mm.IdUnit = u.ObjectID.ObjectId;
				mm.Destination = newPos;
				mm.IdPlayer = u.ObjectID.PlayerID;
				Connection.Instance.SendMessage(mm);
			}
		}

		public void CreateBuilding(Position pos, short buildingId) {
			if (!checkBuildingPosition(pos, buildingId)) {
				if (OnBadLocation != null) {
					OnBadLocation();
				}
				return;
			}

			BuildMessage bm = (BuildMessage)Yad.Net.Client.Utils.CreateMessageWithSenderId(MessageType.Build);
			bm.BuildingID = _currPlayer.GenerateObjectID();
			bm.IdPlayer = _currPlayer.Id;
			bm.BuildingType = buildingId;
			bm.Type = MessageType.Build;
			bm.Position = pos;
			Connection.Instance.SendMessage(bm);

			// building is not built yet
			//IncreaseBuildingCounter(buildingId);

			//TODO: need help - what is this for?
			/*
			AddUnitCreation(bm.BuildingType);
			
			foreach (TechnologyDependence techRef in sim.GameSettingsWrapper.racesMap[currPlayer.Race].TechnologyDependences) {
				short ids = sim.GameSettingsWrapper.namesToIds[techRef.BuildingName];
				if (gf.IsStripContainingBuilding(ids) == true) continue;
				if (CheckReqBuildingsToAddNewBuilding(techRef.RequiredBuildings)) {
					// adds new building to strip
					OnNewBuilding(ids, currPlayer.Race);
				}
			}
			 */
		}
		#endregion


		private bool checkBuildingPosition(Position pos, short buildingTypeId) {
			BuildingData bd = GlobalSettings.Wrapper.buildingsMap[buildingTypeId];

			Map map = _sim.Map;

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

		private void IncreaseBuildingCounter(short buildingId) {
			if (_buildingCounter.ContainsKey(buildingId)) {
				_buildingCounter[buildingId]++;
			} else {
				_buildingCounter[buildingId] = 1;
			}
		}

		private void DecreaseBuildingCounter(short buildingId) {
			if (!_buildingCounter.ContainsKey(buildingId)) {
				return;
			}

			_buildingCounter[buildingId]--;
		}

		#region public methods
		public bool hasBuilding(short id) {
			return _buildingCounter.ContainsKey(id);
		}
		#endregion

		/*
		/// <summary>
		/// Adds certain unit to the unic creation stripe as a possibility of creation a new unit
		/// </summary>
		/// <param name="p"></param>
		private void AddUnitCreation(short p) {
			short o;
			if (buldingCounter[p] == 1) {
				BuildingData b = sim.GameSettingsWrapper.buildingsMap[p];
				foreach (string s in b.UnitsCanProduce) {
					if (sim.GameSettingsWrapper.namesToIds.TryGetValue(s, out o)) {
						OnNewUnit(s, currPlayer.Race);
					}
				}
			}
		}

		/// <summary>
		/// Checks requied building needed to build requested one
		/// </summary>
		/// <param name="coll"></param>
		/// <returns></returns>
		private bool CheckReqBuildingsToAddNewBuilding(BuildingsNames coll) {
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
		 */

		internal void DeployMCV() {
			if (_selectedUnits.Count == 0) {
				return;
			}

			Unit u = _selectedUnits[0];
			if (u.BoardObjectClass != BoardObjectClass.UnitMCV) {
				return;
			}

			/*
			destroy mcv, so that the construction yard can be built
			i think we HAVE TO send ANOTHER MESSAGE to server, ie:
			MessageDeployMCV, because the MCV has to be destroyed
			 synchronously
			 */

			// v remove (workaround), send MessageDeployMCV instead
			short constructionYardId = GlobalSettings.Wrapper.namesToIds["ConstructionYard"];
			Position newPos = new Position(u.Position.X + 1, u.Position.Y);
			this.CreateBuilding(newPos, constructionYardId);
			// ^
		}
	}
}
