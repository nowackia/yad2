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
using Yad.Net.Messaging;
using Yad.UI.Common;

namespace Yad.Engine.Client {
	/// <summary>
	/// This is our GameLogic. There are many like it, but this one is OURS.
	/// </summary>
	public class GameLogic {
		#region events
		public delegate void NewUnitDelegate(string name, short key);
		public delegate void BadLocationHandler(int id);
        public delegate void GameEndHandler(int winTeamId);
        public delegate void PauseResumeHandler(bool isPause);

		public event BadLocationHandler OnBadLocation;
        public event GameEndHandler GameEnd;
        public event PauseResumeHandler PauseResume;

		#endregion

		delegate bool checkLocation(Position loop, Map map);

		#region private members
		ClientSimulation _sim;

		private Dictionary<short, short> _buildingCounter = new Dictionary<short, short>();
		private List<Unit> _selectedUnits = new List<Unit>();
		private Player _currentPlayer;
		/// <summary>
		/// Defined groups: Ctrl+1 - Ctrl+4
		/// </summary>
		private List<Unit>[] _definedGroups = new List<Unit>[4];
		private Building _selectedBuilding = null;
		private Dictionary<short, Building> defaultBuildings = new Dictionary<short, Building>();
        private string OutpostName = "Radar";
		#endregion

		#region Constructor
		public GameLogic() {
			GameSettingsWrapper wrapper = GlobalSettings.Wrapper;
			Map map = new Map();
			map.LoadMap(Path.Combine(Settings.Default.Maps, ClientPlayerInfo.GameInfo.MapName));

			for (int i = 0; i < _definedGroups.Length; i++) {
				_definedGroups[i] = new List<Unit>();
			}
			GameMessageHandler.Instance.GameMessageReceive += new GameMessageEventHandler(Instance_GameMessageReceive);
			GameMessageHandler.Instance.DoTurnPermission += new DoTurnEventHandler(Instance_DoTurnPermission);
			GameMessageHandler.Instance.GameInitialization += new GameInitEventHandler(Instance_GameInitialization);
            GameMessageHandler.Instance.PlayerDisconnected += new PlayerDisconnectedHandler(Instance_PlayerDisconnected);
            _sim = new ClientSimulation(map);
			_sim.onTurnEnd += new SimulationHandler(SandwormHandler);

			//pobranie obiektu aktualnego gracza z symulacji - do obsługi w GameLogic
			_currentPlayer = _sim.GetPlayer(ClientPlayerInfo.Player.Id);

			_sim.BuildingCompleted += new ClientSimulation.BuildingCreationHandler(_sim_OnBuildingCompleted);
            _sim.BuildingDestroyed += new ClientSimulation.BuildingHandler(_sim_BuildingDestroyed);
            _sim.UnitDestroyed += new ClientSimulation.UnitHandler(_sim_UnitDestroyed);
            _sim.OnCreditsUpdate += new ClientSimulation.OnCreditsHandler(_sim_OnCreditsUpdate);
			_sim.MCVDeployed += new ClientSimulation.UnitHandler(_sim_MCVDeployed);
            _sim.ammoBlow += new ClientSimulation.AmmoHandler(_sim_ammoBlow);
            _sim.ammoShoot += new ClientSimulation.AmmoHandler(_sim_ammoShoot);
            this.GameEnd += new GameEndHandler(GameLogic_GameEnd);
			_sim.onTurnEnd += new SimulationHandler(CheckGameEndCondition);
		}

        void Instance_PlayerDisconnected(object sender, GameNumericMessage gnm) {
            //_sim.GetPlayer((short)gnm.Number).IsDisconnected = true;
            //CheckGameEndCondition();
        }        

		void GameLogic_GameEnd(int winTeamId) {
			this.GameEnd -= new GameEndHandler(GameLogic_GameEnd);

			GameMessageHandler.Instance.GameMessageReceive -= new GameMessageEventHandler(Instance_GameMessageReceive);
			GameMessageHandler.Instance.DoTurnPermission -= new DoTurnEventHandler(Instance_DoTurnPermission);
			GameMessageHandler.Instance.GameInitialization -= new GameInitEventHandler(Instance_GameInitialization);
            GameMessageHandler.Instance.PlayerDisconnected -= new PlayerDisconnectedHandler(Instance_PlayerDisconnected);
			//to pewnie jest już mniej istotne
			_sim.onTurnEnd -= new SimulationHandler(SandwormHandler);
			_sim.BuildingCompleted -= new ClientSimulation.BuildingCreationHandler(_sim_OnBuildingCompleted);
			_sim.BuildingDestroyed -= new ClientSimulation.BuildingHandler(_sim_BuildingDestroyed);
			_sim.UnitDestroyed -= new ClientSimulation.UnitHandler(_sim_UnitDestroyed);
			_sim.OnCreditsUpdate -= new ClientSimulation.OnCreditsHandler(_sim_OnCreditsUpdate);
			_sim.MCVDeployed -= new ClientSimulation.UnitHandler(_sim_MCVDeployed);
            _sim.ammoShoot -= new ClientSimulation.AmmoHandler(_sim_ammoShoot);
            _sim.ammoBlow -= new ClientSimulation.AmmoHandler(_sim_ammoBlow);
			_sim.onTurnEnd -= new SimulationHandler(CheckGameEndCondition);
        }

        void _sim_ammoShoot(Ammo u) {
            MiscSoundType type;
            switch (u.Type) {
                case AmmoType.Bullet:
                    type = MiscSoundType.Gun;
                    break;
                case AmmoType.Rocket:
                    type = MiscSoundType.Rocket;
                    break;
                case AmmoType.Sonic:
                    type = MiscSoundType.Sonic;
                    break;
                default:
                    return;
            }
            /* Sound */
            AudioEngine.Instance.Sound.PlayMisc(type);
        }

        void _sim_ammoBlow(Ammo u) {

            /* Sound */
            AudioEngine.Instance.Sound.PlayMisc(MiscSoundType.MediumExplosion);
        }

		void _sim_MCVDeployed(Unit u) {
			if (u.ObjectID.PlayerID != CurrentPlayer.Id) {
				return;
			}
            /* Sound */
            AudioEngine.Instance.Sound.PlayHouse(CurrentPlayer.House, new HouseSoundType[] { HouseSoundType.Unit, HouseSoundType.Deploy });

            _selectedUnits.Remove(u);
			removeUnitFromGroups(u);
		}

		void _sim_OnBuildingCompleted(Building b, int creatorID) {
            if (b.ObjectID.PlayerID == CurrentPlayer.Id) {
                IncreaseBuildingCounter(b.TypeID);
                foreach (string s in b.BuildingData.UnitsCanProduce.NameCollection) {
                    if (!defaultBuildings.ContainsKey(GlobalSettings.Wrapper.namesToIds[s]))
                        defaultBuildings[GlobalSettings.Wrapper.namesToIds[s]] = b;
                }
            }
            /* Sound */
            if (CurrentPlayer.IsVisible(b))
                AudioEngine.Instance.Sound.PlayMisc(MiscSoundType.PlaceStructure);
		}

        void _sim_UnitDestroyed(Unit u) {
			_selectedUnits.Remove(u);
			removeUnitFromGroups(u);

            /* Play fight music */
            if(u.BoardObjectClass != BoardObjectClass.UnitMCV)
                AudioEngine.Instance.Music.Play(MusicType.Fight);

            /* Play sound */
            if (CurrentPlayer.IsVisible(u))
                AudioEngine.Instance.Sound.PlayMisc(MiscSoundType.SmallExplosion);

            this.CheckGameEndCondition();
        }

        void _sim_BuildingDestroyed(Building b) {
			if (_selectedBuilding == b) {
				_selectedBuilding = null;
			}
            /* Play fight music */
            if (b.ObjectID.PlayerID == CurrentPlayer.Id) {
                DecreaseBuildingCounter(b.TypeID);
            }

            /* Play sound */
            if(CurrentPlayer.IsVisible(b))
                AudioEngine.Instance.Sound.PlayMisc(MiscSoundType.StructureExplosion);

            AudioEngine.Instance.Music.Play(MusicType.Fight);
            this.CheckGameEndCondition();
        }

        void _sim_OnCreditsUpdate(short playerNo, int cost)
        {
            if (playerNo == CurrentPlayer.Id)
                AudioEngine.Instance.Sound.PlayMisc(MiscSoundType.Credit);
        }
		#endregion

		#region sandworm handling

		long sandwormCounter = 0;

		private void createSandworm() {
			int i = 0;
			foreach(UnitSandwormData s in GlobalSettings.Wrapper.Sandworms)
			{
				i++;
				Position p  = getPosition(i);
                UnitSandworm u = new UnitSandworm(new ObjectID(Simulation.SimulationPlayer.Id, Simulation.SimulationPlayer.GenerateObjectID()), s, p, _sim.Map, _sim, s.Speed);
                InfoLog.WriteInfo(u.ObjectID.ToString() + " for sandworm", EPrefix.GObj);
				_sim.Sandworms.Add(u.ObjectID.ObjectId, u);
			}
			//Sandworms.Add(new UnitSandworm(new ObjectID(GlobalSettings.Wrapper.Sandworms[0].TypeID
			//_gameLogic.createUnit(GlobalSettings.Wrapper.Sandworms[0].TypeID, _gameLogic.CurrentPlayer.GenerateObjectID());
		}

		private Position getPosition(int i) {
			Position p = new Position(((_sim.CurrentTurn + i*31) % 537 + 9515) % _sim.Map.Width, ((_sim.CurrentTurn + i*17) % 743 + 9361) % _sim.Map.Width);
			p = FindLocation(p, _sim.Map, checkSandLocation);
			return p;
		}

		private void SandwormHandler() {
			if (sandwormCounter >= Yad.Properties.Client.Settings.Default.SandwormTurnsOff && _sim.Sandworms.Count == 0) {
				createSandworm();
				sandwormCounter = 0;
			} else if (sandwormCounter >= Yad.Properties.Client.Settings.Default.SandwormTurnsOn && _sim.Sandworms.Count > 0) {
				deleteSandworms();
                
				sandwormCounter = 0;
			}
			sandwormCounter++;
		}

		private void deleteSandworms() {
            //TODO move to simulation

            foreach (Unit sandworm in _sim.Sandworms.Values) {
                sandworm.State = Unit.UnitState.destroyed;
                _sim.Map.Units[sandworm.Position.X, sandworm.Position.Y].Remove(sandworm);
            }
			//_sim.Sandworms = new Dictionary<int, Unit>();
			_sim.Sandworms.Clear();
		}

		#endregion

		#region Message handling
		void Instance_GameInitialization(object sender, GameInitEventArgs e) {
			PlayerInfo pi = ClientPlayerInfo.Player;
			PositionData[] aPd = e.gameInitInfo;
			
			foreach (PositionData pd in aPd) {
				//TODO: get info

				//_currPlayer = _sim.Players[pd.PlayerId];
				Player p = _sim.Players[pd.PlayerId];
				ObjectID mcvID = new ObjectID(p.Id, p.GenerateObjectID());
                InfoLog.WriteInfo(mcvID.ToString() + " for MVC", EPrefix.GObj);
                UnitMCV mcv = new UnitMCV(mcvID, GlobalSettings.Wrapper.MCVs[0], new Position(pd.X, _sim.Map.Height - pd.Y -1), _sim.Map, this._sim);
				p.AddUnit(mcv);
				_sim.ClearFogOfWar(mcv);

				// vjust for fun ;p
				ObjectID tankID = new ObjectID(p.Id, p.GenerateObjectID());

                UnitTank u = new UnitTank(tankID, GlobalSettings.Wrapper.Tanks[0], new Position((short)((pd.X + 1) % _sim.Map.Width), _sim.Map.Height - pd.Y - 1), this._sim.Map, this._sim);
                InfoLog.WriteInfo(tankID.ToString() + " for tank: " + u.TypeID, EPrefix.GObj);
                p.AddUnit(u);
				_sim.ClearFogOfWar(u);
				// ^

				//this.sim.Map.Units[u.Position.X, u.Position.Y].AddLast(u);
			}

			this._sim.StartSimulation();

			this._sim.DoTurn(1);
		}

		void Instance_DoTurnPermission(object sender, DoTurnMessage dtm) {
			//InfoLog.WriteInfo("Turn permitted", EPrefix.SimulationInfo);
            PauseAction paction = (PauseAction)dtm.Pause;
            switch (paction) {
                case PauseAction.Resume:
                    //_isPaused = false;
                    if (dtm.SpeedUp) {
                        _sim.SpeedUp = true;
                        InfoLog.WriteInfo("Speeding Up", EPrefix.GameLogic);
                    }
                    if (PauseResume != null)
                        PauseResume(false);
                    _sim.DoTurn(1);
                    break;
                case PauseAction.None:
                    if (dtm.SpeedUp) {
                        _sim.SpeedUp = true;
                        InfoLog.WriteInfo("Speeding Up", EPrefix.GameLogic);
                    }
                    _sim.DoTurn(1);
                    break;
                case PauseAction.Pause:
                    //this._isPaused = true;
                    if (PauseResume != null)
                        PauseResume(true);
                    break;
            }
			
		}

		void Instance_GameMessageReceive(object sender, GameMessage gameMessage) {
            if (gameMessage.Type == MessageType.Build)
                InfoLog.WriteInfo("Received build message: " + ((BuildMessage)gameMessage).ToString(), EPrefix.Test);
            if (gameMessage.Type == MessageType.BuildUnitMessage)
                InfoLog.WriteInfo("Received build unit message: " + ((BuildUnitMessage)gameMessage).ToString(), EPrefix.Test);
			this._sim.AddGameMessage(gameMessage);
		}
		#endregion

		#region Properties
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
			get { return this._currentPlayer; }
		}
		#endregion

		#region User orders

        /// <summary>
        /// checks if 
        /// </summary>
        /// <returns></returns>
        public bool CanGiveOrders() {
            foreach (Unit unit in SelectedUnits) {
                if (unit.ObjectID.PlayerID.Equals(CurrentPlayer.Id) == false) return false;
            }
            //if (SelectedBuilding.ObjectID.PlayerID.Equals(CurrentPlayer.Id)) return true;
            
            return true;
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

                /* Sound */
                for (int i = 0; i < _selectedUnits.Count; i++)
                {
                    if (_selectedUnits[i].ObjectID.PlayerID == CurrentPlayer.Id)
                    {
                        AudioEngine.Instance.Sound.PlayMisc(MiscSoundType.Reporting);
                        break;
                    }
                }

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
            
            // friendy fire off
            if (attacked.ObjectID.PlayerID == CurrentPlayer.Id ||
                            _sim.Players[attacked.ObjectID.PlayerID].TeamID == CurrentPlayer.TeamID) {
                MoveOrder(attacked.Position);
                return;
            }
            if (_selectedBuilding != null) {
                return;
                /*
                AttackMessage am = (AttackMessage)MessageFactory.Create(MessageType.Attack);
                am.Attacker = _selectedBuilding.ObjectID;
                am.Attacked = attacked.ObjectID;
                am.IdPlayer = _selectedBuilding.ObjectID.PlayerID;
                Connection.Instance.SendMessage(am);*/
            }

            bool soundPlayed = false;
            List<Unit> selectedUnits = new List<Unit>(_selectedUnits);
            foreach (Unit u in selectedUnits) {
				if (u.ObjectID.PlayerID != CurrentPlayer.Id && u.State != Unit.UnitState.destroyed) {
					continue;
				}
                
                AttackMessage am = (AttackMessage)MessageFactory.Create(MessageType.Attack);
                am.Attacker = u.ObjectID;
                am.Attacked = attacked.ObjectID;
                am.IdPlayer = u.ObjectID.PlayerID;
                Connection.Instance.SendMessage(am);

                /* Sound */
                if (!soundPlayed)
                {
                    AudioEngine.Instance.Sound.PlayMisc(MiscSoundType.Affirmative);
                    soundPlayed = true;
                }
            }
        }

		internal void MoveOrder(Position newPos) {
			if (_selectedUnits.Count == 0) {
				return;
			}

            bool soundPlayed = false;

			foreach (Unit u in _selectedUnits) {
				if (u.ObjectID.PlayerID != CurrentPlayer.Id) {
					continue;
				}
				MoveMessage mm = (MoveMessage)MessageFactory.Create(MessageType.Move);
				mm.IdUnit = u.ObjectID.ObjectId;
				mm.Destination = newPos;
				mm.IdPlayer = u.ObjectID.PlayerID;
				Connection.Instance.SendMessage(mm);

                /* Sound */
                if (!soundPlayed)
                {
                    if (u.BoardObjectClass == BoardObjectClass.UnitTrooper)
                        AudioEngine.Instance.Sound.PlayMisc(MiscSoundType.InfantryOut);
                    else
                        AudioEngine.Instance.Sound.PlayMisc(MiscSoundType.Acknowledged);
                    soundPlayed = true;
                }
			}
		}

		public void CreateBuilding(Position pos, short buildingId, int creatorID) {
			BuildingData bd = GlobalSettings.Wrapper.buildingsMap[buildingId];
			if (!Building.CheckBuildPosition(bd, pos, _sim.Map, CurrentPlayer.Id)) {
				if (OnBadLocation != null) {
					OnBadLocation(creatorID);
				}
				return;
			}
			
			BuildMessage bm = (BuildMessage)Yad.Net.Client.Utils.CreateMessageWithSenderId(MessageType.Build);
			//bm.BuildingID = CurrentPlayer.GenerateObjectID();//don't set object id here! it's not reflected in other simulations
			bm.IdPlayer = CurrentPlayer.Id;
			bm.BuildingType = buildingId;
            bm.CreatorID = creatorID;
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

        #region Game end
        /// <summary>
        /// Checks if the game has ended - if any team has any objects left on map
        /// </summary>
        public void CheckGameEndCondition()
        {
            InfoLog.WriteInfo("Enter CheckGameEndCondition", EPrefix.BMan);
            if (CurrentPlayer.GameObjectsCount == 0)
                CurrentPlayer.ClearForOfWar();

            Dictionary<short, int> teamGameObjectCount = new Dictionary<short, int>();

            foreach (Player player in _sim.GetPlayers())
            {
                if(player.Equals(_sim.SimulationPlayer)) continue;
                if (teamGameObjectCount.ContainsKey(player.TeamID)) {
                        teamGameObjectCount[player.TeamID] += player.GameObjectsCount;
                }
                else {
                        teamGameObjectCount.Add(player.TeamID, player.GameObjectsCount);
                }
            }

            short anyObjectOwningTeamsCount = 0;
            short anyObjectOwningTeamId = 0;

            foreach(short teamId in teamGameObjectCount.Keys)
            {
                if (teamGameObjectCount[teamId] != 0)
                {
                    anyObjectOwningTeamsCount += 1;
                    anyObjectOwningTeamId = teamId;
                }

                /* Two or more teams still fighting */
                if (anyObjectOwningTeamsCount > 1)
                    break;
            }

            int playersCount = _sim.GetPlayers().Count-1;
            /* Only one team left */
            if (playersCount > 1 && anyObjectOwningTeamsCount == 1 && GameEnd != null) {
                InfoLog.WriteInfo("GameEnd!!!", EPrefix.BMan);
                GameEnd(anyObjectOwningTeamId);
            }

            /* No teams left */
            if (playersCount == 1 && anyObjectOwningTeamsCount == 0 && GameEnd != null) {
                InfoLog.WriteInfo("GameEnd!!!", EPrefix.BMan);
                GameEnd(anyObjectOwningTeamId);
            }
            InfoLog.WriteInfo("End CheckGameEndCondition");
        }
        #endregion

        /*
		public bool checkBuildingPosition(Position pos, short buildingTypeId) {
			BuildingData bd = GlobalSettings.Wrapper.buildingsMap[buildingTypeId];

			Map map = _sim.Map;

			if ((pos.X + bd.Size.X - 1 >= map.Width)
				|| (pos.Y + bd.Size.Y - 1 >= map.Height)) {
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
		 */

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

		public bool isOutpostOperating() {
            return HasOutpost() && CurrentPlayer.Power > Yad.Properties.Client.Settings.Default.PowerLowBorder;
		}

        private bool HasOutpost()
        {
            short type = GlobalSettings.Wrapper.namesToIds[OutpostName];
            return hasBuilding(type);
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
			/*short constructionYardId = GlobalSettings.Wrapper.namesToIds["ConstructionYard"];
			Position newPos = new Position(u.Position.X + 1, u.Position.Y);
			this.CreateBuilding(newPos, constructionYardId,-1);*/
            
			// ^
            GMDeployMCV msgMVC = (GMDeployMCV)MessageFactory.Create(MessageType.DeployMCV);
            msgMVC.IdPlayer = CurrentPlayer.Id;
            msgMVC.McvID = u.ObjectID;
            Connection.Instance.SendMessage(msgMVC); 
		}

		/*
		internal void createUnit(short id, int objectID) {
			Position p = new Position(0,0);
			bool found = false;
			List<Building> bs = Simulation.Players[CurrentPlayer.Id].GetAllBuildings();
			foreach(Building b in bs){
				if(b.ObjectID.ObjectId == objectID){
					//pierwsze wystapnienie budynku
					p = b.Position;
					found = true;
				}
			}
			if (found == false && !GlobalSettings.Wrapper.sandwormsMap.ContainsKey(id))
				return;
			Position pos = FindFreeLocation(p, _sim.Map);
			CreateUnitMessage um = (CreateUnitMessage)Yad.Net.Client.Utils.CreateMessageWithSenderId(MessageType.CreateUnit);
			//um.UnitID = CurrentPlayer.GenerateObjectID();//do not generate id here - it's not reflected in other simulations
			um.IdPlayer = CurrentPlayer.Id;
			um.UnitType = id;
			if (GlobalSettings.Wrapper.harvestersMap.ContainsKey(id))
				um.UnitKind = BoardObjectClass.UnitHarvester;
			else if (GlobalSettings.Wrapper.mcvsMap.ContainsKey(id))
				um.UnitKind = BoardObjectClass.UnitMCV;
			else if (GlobalSettings.Wrapper.tanksMap.ContainsKey(id))
				um.UnitKind = BoardObjectClass.UnitTank;
			else if (GlobalSettings.Wrapper.troopersMap.ContainsKey(id))
				um.UnitKind = BoardObjectClass.UnitTrooper;
			else if (GlobalSettings.Wrapper.sandwormsMap.ContainsKey(id)) {
				um.UnitKind = BoardObjectClass.UnitSandworm;
				int x, y;
				Random r = new Random((int)DateTime.Now.Ticks);
				do {
					x = r.Next(Simulation.Map.Width);
					y = r.Next(Simulation.Map.Height);
				} while (Simulation.Map.Tiles[x,y] != TileType.Sand);
				pos = new Position(x, y);
				//um.IdPlayer = -1;
			}
			um.Type = MessageType.CreateUnit;
			um.Position = pos;
			Connection.Instance.SendMessage(um);
		}
		 */

		private Position FindFreeLocation(Position p, Map map) {
			return FindLocation(p, map, checkFreeLocation);
		}

		/// <summary>
		/// Finds location on which can be placed new unit
		/// </summary>
		/// <param name="p"></param>
		private Position FindLocation(Position p, Map map, checkLocation c) {
			short radius = 3;
			int dotsCounter;
			int dotsInSquare;
			Position loop = new Position();
			int nearestBorder = Math.Max(Math.Max(p.X, p.Y), Math.Max(map.Width - p.X, map.Height - p.Y));
			for (; radius < nearestBorder; radius += 2 ) {
				loop.X = (short)(p.X - radius / 2); loop.Y = (short)(p.Y + radius / 2);  /////albo -

				dotsInSquare = radius * radius - (radius - 2) * (radius - 2);
				for (dotsCounter = 0; dotsCounter < dotsInSquare; ) {
					if (loop.X>=0 && loop.X<map.Width & loop.Y>=0 && loop.Y<map.Height && c(loop, map))
						return loop;
					dotsCounter++;
					if (dotsCounter < radius)
						loop.X++;
					else if (dotsCounter < 2 * radius - 1)
						loop.Y--; //albo wlasnie ++
					else if (dotsCounter < 3 * radius - 2)
						loop.X--;
					else
						loop.Y++; //albo wlasnie --

				}
			}
			return p;
		}

		private bool checkFreeLocation(Position loop, Map map) {
			if (map.Tiles[loop.X, loop.Y] != TileType.Mountain && map.Buildings[loop.X, loop.Y].Count == 0 && map.Units[loop.X, loop.Y].Count == 0)
				return true;
			else
				return false;
		}

		private bool checkSandLocation(Position loop, Map map) {
			if (map.Tiles[loop.X, loop.Y] == TileType.Sand)
				return true;
			else
				return false;
		}

		public void manageGroups(int groupNumber, bool create) {
			if (groupNumber < 0 || groupNumber >= _definedGroups.Length) {
				return;
			}

			if (create) {
				_definedGroups[groupNumber].Clear();
				_definedGroups[groupNumber].AddRange(_selectedUnits);
			} else {
				_selectedUnits.Clear();
				_selectedUnits.AddRange(_definedGroups[groupNumber]);
			}
		}

		private void removeUnitFromGroups(Unit u) {
			foreach (List<Unit> units in _definedGroups) {
				units.Remove(u);
			}
		}
	}
}
