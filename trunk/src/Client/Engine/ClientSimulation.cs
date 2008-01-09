using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;
using Yad.Board.Common;
using Yad.Config.Common;
using Yad.Board;
using Yad.Config;
using Yad.Net.Messaging;
using Yad.Net.Client;
using Yad.UI.Client;
using System.Windows.Forms;
using Yad.Engine.Common;
using Yad.Properties.Client;
using Yad.Net.Common;
//194.29.178.229
namespace Yad.Engine.Client {
	/// <summary>
	/// Use:
	/// destroyUnit, destroyBuilding
	/// </summary>
	public class ClientSimulation : Yad.Engine.Common.Simulation {
		MessageTurnAsk tam = new MessageTurnAsk();
		#region events
		public delegate void BuildingCreationHandler(Building b, int creatorID);
        public delegate void BuildingHandler(Building b);
		public delegate void UnitHandler(Unit u);
        public delegate void AmmoHandler(Ammo u);
		public delegate void OnLowPowerHandler(Player p);
		public delegate void OnNoPowerHandler(Player p);
		public delegate void OnCreditsHandler(short playerNo, int cost);
		public delegate void InvalidBuildHandler(int objectCreatorId);
        
        public delegate void UpdateStripItemHandler(int playerID, int objectID, short typeID, int percent); 

		public event OnCreditsHandler OnCreditsUpdate;
		public event OnLowPowerHandler OnLowPowerResources;
		public event OnNoPowerHandler OnNoPowerResources;
		public event BuildingCreationHandler BuildingCompleted;
		public event UnitHandler UnitCompleted;
		public event BuildingHandler BuildingDestroyed;
		public event UnitHandler UnitDestroyed;
		public event UnitHandler MCVDeployed;
		public event UnitHandler UnitStarted;
		public event InvalidBuildHandler InvalidBuild;
        public event UpdateStripItemHandler UpdateStripItem;
        public event AmmoHandler ammoShoot;
        public event AmmoHandler ammoBlow;
		#endregion

		public ClientSimulation(Map map)
			: base(map /*, false */) {

			//PlayerInfo currPI = ClientPlayerInfo.Player;
			sandworms = new Dictionary<int, Unit>();

			//Add all players
          
			foreach (PlayerInfo pi in ClientPlayerInfo.GetAllPlayers()) {
				Player p = new Player(pi.Id, pi.Name, pi.House, pi.Color, map);
				p.TeamID = pi.TeamID;
				p.Credits = Settings.Default.CreditsAtStart;
				p.Power = Settings.Default.PowerAtStart;
				players.Add(p.Id, p);               
			}

			this.onTurnBegin += new SimulationHandler(ClientSimulation_onTurnBegin);
			this.onTurnEnd += new SimulationHandler(ClientSimulation_onTurnEnd);
			this.OnLowPowerResources += new OnLowPowerHandler(ClientSimulation_OnLowPowerResources);
			this.OnNoPowerResources += new OnNoPowerHandler(ClientSimulation_OnNoPowerResources);
			this.UnitStarted += new UnitHandler(ClientSimulation_UnitStarted);
			this.UnitCompleted += new UnitHandler(ClientSimulation_UnitCompleted);
		}

		void ClientSimulation_onTurnBegin() {
			int bufferedTurns = this.getBufferedTurns();
			//if (bufferedTurns == 0) { //u¿ywaæ tego jeœli nie dzia³a poni¿sze
			if (bufferedTurns <= 2) {
				Connection.Instance.SendMessage(tam);
			}
		}

		public Dictionary<int, Unit> Sandworms {
			get { return sandworms; }
			set { sandworms = value; }
		}

		void ClientSimulation_onTurnEnd() {
		}

		protected override void OnMessageBuild(BuildMessage bm) {
			//InfoLog.WriteInfo("MessageBuild", EPrefix.SimulationInfo);
            InfoLog.WriteInfo("OnMessageBuild", EPrefix.Test);
            InfoLog.WriteInfo("BuildingType: " + bm.BuildingType + " CreatorID: " + bm.CreatorID, EPrefix.Test);
			BuildingData bd = GlobalSettings.Wrapper.buildingsMap[bm.BuildingType];
			if (!Building.CheckBuildPosition(bd, bm.Position, _map, bm.IdPlayer)) {
				if (InvalidBuild != null) {
                    InfoLog.WriteInfo("Buidling: " + bm.BuildingType + " was not built because of location", EPrefix.BMan);
					InvalidBuild(bm.CreatorID);
                    /*int cost = GlobalSettings.Wrapper.buildingsMap[bm.BuildingType].Cost;
                    p.Credits += cost;
                    OnCreditsUpdate(cost);*/
				}
				return;
			}
            int playerCredits = players[bm.IdPlayer].Credits;
            int buildingCost = GlobalSettings.Wrapper.buildingsMap[bm.BuildingType].Cost;
            if (playerCredits < buildingCost)
            {
                InfoLog.WriteInfo("Buidling: " + bm.BuildingType + " was not built because of credits", EPrefix.BMan);
                if (InvalidBuild != null)
                    InvalidBuild(bm.CreatorID);
                return;
            }
            players[bm.IdPlayer].Credits -= buildingCost;
            if (OnCreditsUpdate != null)
                OnCreditsUpdate(bm.IdPlayer, buildingCost);
            Building b = AddBuilding(bm.IdPlayer, bm.CreatorID, bm.BuildingType, bm.Position);
            b.State = Building.BuildingState.constructing;
            b.BuildStatus = new BuildStatus(bm.CreatorID, bm.BuildingType, b.BuildingData.BuildSpeed, BuildType.Building);
            UpdatePowerManagement(b);
            InfoLog.WriteInfo("OnMessageBuildEnd", EPrefix.BMan);
            //OnBuildingCompleted(b, bm.CreatorID);
            //194.29.178.207
		}

		protected override void onMessageMove(MoveMessage gm) {
			InfoLog.WriteInfo("MessageMove: PlayerID:" + gm.IdPlayer + " UnitID:" + gm.IdUnit, EPrefix.Move);

			Player p = this.players[gm.IdPlayer];
			Unit u = p.GetUnit(ObjectID.From(gm.IdUnit, gm.IdPlayer));
			if (u == null) {
				InfoLog.WriteInfo("MessageMove: PlayerID:" + gm.IdPlayer + " unit has been already destroyed", EPrefix.Move);
				return;
			}
            u.OrderedAttack = false;
			u.MoveTo(gm.Destination);
		}

		protected override void onMessageAttack(AttackMessage am) {
			InfoLog.WriteInfo("MessageAttack", EPrefix.SimulationInfo);
			// set attacked object in attacker unit
			Unit attacker = players[am.Attacker.PlayerID].GetUnit(am.Attacker);
			if (attacker == null) return;

			Player p = players[am.Attacked.PlayerID];
			BoardObject b;
			bool isBuilding = false;

			b = p.GetBuilding(am.Attacked);
			if (b != null) {
				isBuilding = true;
			} else {
				b = p.GetUnit(am.Attacked);
				if (b == null) {
					InfoLog.WriteInfo("MessageMove: PlayerID:" + am.Attacked.PlayerID + " attacked object does not exist", EPrefix.SimulationInfo);
					return;
				}
			}
            attacker.OrderedAttack = true;
			attacker.OrderAttack(b, isBuilding);
		}

		protected override void onMessageDestroy(DestroyMessage dm) {
			InfoLog.WriteInfo("MessageDestroy", EPrefix.SimulationInfo);
		}

		protected override void onMessageHarvest(HarvestMessage hm) {
			InfoLog.WriteInfo("MessageHarvest", EPrefix.SimulationInfo);
		}

        private void CreateUnit(short playerId, short type, Position pos) {
            InfoLog.WriteInfo("CreateUnit", EPrefix.Test);
			Player p = players[playerId];
			Unit u = null;
			ObjectID id = new ObjectID(playerId, p.GenerateObjectID());
            InfoLog.WriteInfo(id.ToString() + " dla jednostki: " + type, EPrefix.Test);
            if (GlobalSettings.Wrapper.harvestersMap.ContainsKey(type)){
				u = new UnitHarvester(id, GlobalSettings.Wrapper.harvestersMap[type], pos, this._map, this, GlobalSettings.Wrapper.harvestersMap[type].__Speed);
                ((UnitHarvester)u).SpiceUnload += new SpiceUnloadDelegate(SpiceUnload);
            }
			else if (GlobalSettings.Wrapper.mcvsMap.ContainsKey(type)){
				u = new UnitMCV(id, GlobalSettings.Wrapper.mcvsMap[type], pos, this._map, this);
            }
			else if (GlobalSettings.Wrapper.tanksMap.ContainsKey(type)){
				u = new UnitTank(id, GlobalSettings.Wrapper.tanksMap[type], pos, this._map, this);
            }
			else if (GlobalSettings.Wrapper.troopersMap.ContainsKey(type)){
				u = new UnitTrooper(id, GlobalSettings.Wrapper.troopersMap[type], pos, this._map, this);
            }

			p.AddUnit(u);
			ClearFogOfWar(u);
            OnUnitCompleted(u);
        }
        private void SpiceUnload(short idplayer, int credits){
            if (OnCreditsUpdate != null)
                OnCreditsUpdate(idplayer,-credits);
        }
		protected override void onMessageCreate(CreateUnitMessage cum) {
			InfoLog.WriteInfo("MessageCreate", EPrefix.Test);
			int cost=0;
			Player p = players[cum.IdPlayer];
			BoardObjectClass boc = cum.UnitKind;
			Unit u = null;
			ObjectID id = new ObjectID(cum.IdPlayer, p.GenerateObjectID());
            InfoLog.WriteInfo(id.ToString() + " unit created: " + cum.Type, EPrefix.GObj);
			if (boc == BoardObjectClass.UnitTank) {
				if (players[cum.IdPlayer].Credits < (cost=GlobalSettings.Wrapper.tanksMap[cum.UnitType].Cost))
					return;
				u = new UnitTank(id, GlobalSettings.Wrapper.tanksMap[cum.UnitType], cum.Position, this._map, this);
				players[cum.IdPlayer].Credits -= GlobalSettings.Wrapper.tanksMap[cum.UnitType].Cost;
			} else if (boc == BoardObjectClass.UnitTrooper) {
				if (players[cum.IdPlayer].Credits < (cost=GlobalSettings.Wrapper.troopersMap[cum.UnitType].Cost))
					return;
				u = new UnitTrooper(id, GlobalSettings.Wrapper.troopersMap[cum.UnitType], cum.Position, this._map, this);
				players[cum.IdPlayer].Credits -= GlobalSettings.Wrapper.troopersMap[cum.UnitType].Cost;
			} else if (boc == BoardObjectClass.UnitHarvester) {
				if (players[cum.IdPlayer].Credits < (cost=GlobalSettings.Wrapper.harvestersMap[cum.UnitType].Cost))
					return;
				u = new UnitHarvester(id, GlobalSettings.Wrapper.harvestersMap[cum.UnitType], cum.Position, this._map, this, GlobalSettings.Wrapper.harvestersMap[cum.UnitType].__Speed);
                ((UnitHarvester)u).SpiceUnload += new SpiceUnloadDelegate(SpiceUnload);
				players[cum.IdPlayer].Credits -= GlobalSettings.Wrapper.harvestersMap[cum.UnitType].Cost;
			} else if (boc == BoardObjectClass.UnitMCV) {
				if (players[cum.IdPlayer].Credits < (cost=GlobalSettings.Wrapper.mcvsMap[cum.UnitType].Cost))
					return;
				u = new UnitMCV(id, GlobalSettings.Wrapper.mcvsMap[cum.UnitType], cum.Position, this._map, this);
				players[cum.IdPlayer].Credits -= GlobalSettings.Wrapper.mcvsMap[cum.UnitType].Cost;
			} else if (boc == BoardObjectClass.UnitSandworm) {
				u = new UnitSandworm(id, GlobalSettings.Wrapper.sandwormsMap[cum.UnitType], cum.Position, this._map, this, GlobalSettings.Wrapper.sandwormsMap[cum.UnitType].__Speed);
				Sandworms[id.ObjectId] = (UnitSandworm)u;
				return;
			//} else if (boc == BoardObjectClass.UnitTank) {
			//    u = new UnitTank(id, GlobalSettings.Wrapper.tanksMap[cum.UnitType], cum.Position, this._map, this);
			//} else if (boc == BoardObjectClass.UnitTrooper) {
			//    u = new UnitTrooper(id, GlobalSettings.Wrapper.troopersMap[cum.UnitType], cum.Position, this._map, this);

			}
			OnCreditsUpdate(cum.IdPlayer, cost);
			p.AddUnit(u);
			ClearFogOfWar(u);

			OnUnitCompleted(u);
		}


        private Building AddBuilding(short playerID, int creatorID, short buildingType, Position pos) {
            Player p = players[playerID];
            int objid = p.GenerateObjectID();
            ObjectID id = new ObjectID(playerID, objid);
            InfoLog.WriteInfo(id.ToString() + " adding building: " + buildingType, EPrefix.BMan);
            InfoLog.WriteInfo("Adding building id: (" + playerID + "," + id + ")", EPrefix.BMan);
            BuildingData bd = GlobalSettings.Wrapper.buildingsMap[buildingType];
            Building b = new Building(id, bd, this._map, pos, this);
            p.AddBuilding(b);
            ClearFogOfWar(b);
            return b;
        }

		protected override void onMessageDeployMCV(Yad.Net.Messaging.GMDeployMCV dmcv) {
            Player p = players[dmcv.IdPlayer];
            UnitMCV mcv = (UnitMCV)p.GetUnit(ObjectID.From(dmcv.McvID.ObjectId, dmcv.McvID.PlayerID));
            if (mcv == null) {
                InfoLog.WriteInfo("No mcv to deploy found.", EPrefix.SimulationInfo);
                return;
            }
            string bName = mcv.MCVData.BuildingCanProduce;
            short btype = GlobalSettings.Wrapper.namesToIds[bName];
            this._map.Units[mcv.Position.X, mcv.Position.Y].Remove(mcv);
            BuildingData bd = GlobalSettings.Wrapper.buildingsMap[btype];
            if (!Building.CheckBuildPosition(bd, mcv.Position, _map, dmcv.McvID.PlayerID)) {
                this._map.Units[mcv.Position.X, mcv.Position.Y].AddLast(mcv);
                if (InvalidBuild != null) {
                    InvalidBuild(-1);
                }
                return;
            }
            this._map.Units[mcv.Position.X, mcv.Position.Y].AddLast(mcv);
            Building b = AddBuilding(dmcv.McvID.PlayerID, -1, btype,  mcv.Position);
			b.State = Building.BuildingState.normal;
            UpdatePowerManagement(b);
            OnBuildingCompleted(b, -1);
            destroyUnit(mcv);
            //OnUnitDestroyed(mcv);
			if (MCVDeployed != null) {
				MCVDeployed(mcv);
			}
		}

		protected override void onInvalidMove(Yad.Board.Common.Unit unit) {
			throw new Exception("The method or operation is not implemented.");
		}
		/// <summary>
		/// in this method unit should choose whether to move or attack - deterministic AI
		/// </summary>
		/// <param name="u"></param>
		protected override void handleUnit(Unit u) {

			u.DoAI();

		}

		protected override void handleBuilding(Building b) {
            Building.BuildingState bstate = b.State;
			b.DoAI();
            if (b.BuildStatus != null) {
                switch (bstate) {
                    case Building.BuildingState.constructing:
                        if (b.State == Building.BuildingState.normal) {
                            if (UpdateStripItem != null)
                                UpdateStripItem(b.ObjectID.PlayerID, b.BuildStatus.ObjectId, b.BuildStatus.Typeid, -1);
                            OnBuildingCompleted(b, b.BuildStatus.ObjectId);
                            b.BuildStatus = null;

                        }
                        else {
                            if (UpdateStripItem != null)
                                UpdateStripItem(b.ObjectID.PlayerID,b.BuildStatus.ObjectId, b.BuildStatus.Typeid, b.BuildStatus.Percent);
                        }
                        break;
                    case Building.BuildingState.creating:
                        if (b.State == Building.BuildingState.normal) {
                            if (UpdateStripItem != null)
                                UpdateStripItem(b.ObjectID.PlayerID, b.BuildStatus.ObjectId, b.BuildStatus.Typeid, -1);
                            Position pos = FindFreeLocation(b.Position, this.Map);
                            CreateUnit(b.ObjectID.PlayerID, b.BuildStatus.Typeid, pos);
                            b.BuildStatus = null;
                            
                            
                        }
                        else {
                            if (UpdateStripItem != null)
                                UpdateStripItem(b.ObjectID.PlayerID, b.BuildStatus.ObjectId, b.BuildStatus.Typeid, b.BuildStatus.Percent);
                        }
                        break;
                }
            }
		}

        /// <summary>
        /// Finds location on which can be placed new unit
        /// </summary>
        /// <param name="p"></param>
        private Position FindFreeLocation(Position p, Map map) {
            short radius = 3;
            int dotsCounter;
            int dotsInSquare;
            Position loop = new Position();
            int nearestBorder = Math.Max(Math.Max(p.X, p.Y), Math.Max(map.Width - p.X, map.Height - p.Y));
            for (; radius < nearestBorder; radius += 2) {
                loop.X = (short)(p.X - radius / 2); loop.Y = (short)(p.Y + radius / 2);  /////albo -

                dotsInSquare = radius * radius - (radius - 2) * (radius - 2);
                for (dotsCounter = 0; dotsCounter < dotsInSquare; ) {
                    if (loop.X >= 0 && loop.X < map.Width & loop.Y >= 0 && loop.Y < map.Height && checkFreeLocation(loop, map))
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
			if (map.Tiles[loop.X, loop.Y] != TileType.Mountain && (map.Buildings[loop.X, loop.Y].Count == 0 || map.Buildings[loop.X, loop.Y].First.Value.TypeID == GlobalSettings.Wrapper.namesToIds["Slab1"] || map.Buildings[loop.X, loop.Y].First.Value.TypeID == GlobalSettings.Wrapper.namesToIds["Slab4"]) && map.Units[loop.X, loop.Y].Count == 0)
                return true;
            else
                return false;
        }



		/// <summary>
		/// handles attack - counts damage, destroy unit
		/// </summary>
		/// <param name="attacked"></param>
		/// <param name="attacker"></param>
		public override void handleAttackUnit(Unit attacked, Unit attacker) {
			handleAttackUnit(attacked, attacker, attacker.FirePower);
		}
		/// <summary>
		/// handles attack - counts damage, destroy building
		/// </summary>
		/// <param name="attacked"></param>
		/// <param name="attacker"></param>
		public override void handleAttackBuilding(Building attacked, Unit attacker) {
			handleAttackBuilding(attacked, attacker, attacker.FirePower);
		}

		public override void handleAttackBuilding(Building attacked, Building attacker) {
			handleAttackBuilding(attacked, attacker, attacker.BuildingData.FirePower);
		}

		public override void handleAttackUnit(Unit attacked, Building attacker) {
			handleAttackUnit(attacked, attacker, attacker.BuildingData.FirePower);
		}

		public override void handleAttackUnit(Unit attacked, Unit attacker, short count) {
			if (attacked.State == Unit.UnitState.destroyed) {
				// handles stack overflow;P
				return;
			}
			attacked.Health -= count;
			if (attacked.Health <= 0) {
				InfoLog.WriteInfo("destroying building ", EPrefix.SimulationInfo);
				destroyUnit(attacked);
				OnUnitDestroyed(attacked);
			}
		}

		public override void handleAttackBuilding(Building attacked, Unit attacker, short count) {
			if (attacked.State == Building.BuildingState.destroyed) {
				// handles stack overflow;P
				return;
			}
			attacked.Health -= count;
			if (attacked.Health <= 0) {
				InfoLog.WriteInfo("destroying building ", EPrefix.SimulationInfo);
				destroyBuilding(attacked);
				OnBuildingDestroyed(attacked);
			}
		}

		public override void handleAttackBuilding(Building attacked, Building attacker, short count) {
			if (attacked.State == Building.BuildingState.destroyed) {
				// handles stack overflow;P
				return;
			}
			attacked.Health -= count;
			if (attacked.Health <= 0) {
				InfoLog.WriteInfo("destroying building ", EPrefix.SimulationInfo);
				destroyBuilding(attacked);
				OnBuildingDestroyed(attacked);
			}
		}

        public override void handleAttackBuilding(Building attacked, Ammo attacker) {
            if (attacked.State == Building.BuildingState.destroyed) {
                // handles stack overflow;P
                return;
            }
            attacked.Health -= attacker.Damage;
            if (attacked.Health <= 0) {
                InfoLog.WriteInfo("destroying building ", EPrefix.SimulationInfo);
                destroyBuilding(attacked);
                OnBuildingDestroyed(attacked);
            }
        }

        public override void handleAttackUnit(Unit attacked, Ammo attacker) {
            if (attacked.State == Unit.UnitState.destroyed) {
                // handles stack overflow;P
                return;
            }
            attacked.Health -= attacker.Damage;
            if (attacked.Health <= 0) {
                InfoLog.WriteInfo("Destroying unit ", EPrefix.SimulationInfo);
                destroyUnit(attacked);
                OnUnitDestroyed(attacked);
            }

        }

		public override void handleAttackUnit(Unit attacked, Building attacker, short count) {
			if (attacked.State == Unit.UnitState.destroyed) {
				// handles stack overflow;P
				return;
			}
			attacked.Health -= count;
			if (attacked.Health <= 0) {
				InfoLog.WriteInfo("Destroying unit ", EPrefix.SimulationInfo);
				destroyUnit(attacked);
				OnUnitDestroyed(attacked);
			}
		}

		private void OnUnitCompleted(Unit u) {
			if (UnitCompleted != null) {
				//TODO RS: run in different thread
				this.UnitCompleted(u);
			}
		}

		private void OnBuildingDestroyed(Building b) {
			if (BuildingDestroyed != null) {
				//TODO RS: run in different thread
				BuildingDestroyed(b);
			}
		}

		private void OnUnitDestroyed(Unit u) {
			if (UnitDestroyed != null) {
				//TODO RS: run in different thread
				UnitDestroyed(u);
			}
		}

		private void OnBuildingCompleted(Building b, int creator_id) {
			if (BuildingCompleted != null) {
				//TODO RS: run in different thread
				this.BuildingCompleted(b, creator_id);
			}
		}

		void ClientSimulation_UnitStarted(Unit u) {
			throw new Exception("The method or operation is not implemented.");
		}


		internal void UpdatePowerManagement(Building b) {
			Player p = players[b.ObjectID.PlayerID];
			p.Power -= b.BuildingData.EnergyConsumption;
			/* lol 
			if (p.Power < Settings.Default.PowerLowBorder) {
				OnLowPowerResources(p);
			} else if (p.Power < 0) {
				OnNoPowerResources(p);
			}
			 */
			if (p.Power < 0) {
				OnNoPowerResources(p);
			} else if (p.Power < Settings.Default.PowerLowBorder) {
				OnLowPowerResources(p);
			}
		}

		void ClientSimulation_OnLowPowerResources(Player p) {
			//TODO: Stub metody do obsluzenia sytuacji kiedy jest malo energii
		}

		void ClientSimulation_OnNoPowerResources(Player p) {
			//TODO: Stub metody do obsluzenia sytuacji kiedy zapotrzebowanie na energie przekroczylo jej produkcje
		}

		void ClientSimulation_UnitCompleted(Unit u) {
			//if (UnitCompleted != null) {
			//TODO RS: run in different thread
			//this.UnitCompleted(u);
			//}
		}

		public Player GetPlayer(short id) {
			return this.players[id];
		}

		public override void ClearFogOfWar(Unit o) {
			int max;
			Position[] tiles = BoardObject.RangeSpiral(o.ViewRange, out max);
			bool[,]fow = players[o.ObjectID.PlayerID].FogOfWar;
			for (int i = 0; i < max; i++) {
				Position p = tiles[i];
				int x = p.X + o.Position.X;
				int y = p.Y + o.Position.Y;

				if (x < 0 || y < 0 || x > _map.Width - 1 || y > _map.Height - 1) {
					continue;
				}
				fow[x, y] = false;
			}
		}

		public override void ClearFogOfWar(Building b) {
			int range = b.BuildingData.ViewRange;
			bool[,]fow = players[b.ObjectID.PlayerID].FogOfWar;
			for (int x = -range; x < range + b.Width; x++) {
				for (int y = -range; y < range + b.Height; y++) {
					int rx = x + b.Position.X;
					int ry = y + b.Position.Y;
					if (rx < 0 || ry < 0 || rx > _map.Width - 1 || ry > _map.Height - 1) {
						continue;
					}
					fow[rx, ry] = false;
				}
			}
		}

        protected override void onMessageBuildUnit(BuildUnitMessage msg) {
            InfoLog.WriteInfo("OnMessageBuildUnit: " + msg.ToString());
			if (GlobalSettings.Wrapper.sandwormsMap.ContainsKey(msg.UnitType)) {
				//sandworm
			} else {
				//ordynary unit creation
				ObjectID id = new ObjectID(msg.IdPlayer, msg.CreatorID);
				Building b = players[msg.IdPlayer].GetBuilding(id);
				if (null == b) {
					InfoLog.WriteInfo("Invalid onMessageBuildUnit", EPrefix.BMan);
                    if (InvalidBuild != null)
                        InvalidBuild(msg.CreatorID);
					return;
				}
				int cost = GlobalSettings.GetUnitCost(msg.UnitType);
                if (players[msg.IdPlayer].Credits < cost) {
                    InfoLog.WriteInfo("Invalid cost", EPrefix.BMan);
                    if (InvalidBuild != null)
                        InvalidBuild(msg.CreatorID);
                    return;
                }
				players[msg.IdPlayer].Credits -= cost;
				OnCreditsUpdate(msg.IdPlayer, cost);
				b.BuildStatus = new BuildStatus(msg.CreatorID, msg.UnitType, (short)GetUnitBuildTime(msg.UnitType), BuildType.Unit);
				b.State = Building.BuildingState.creating;
			}
        }

        
        private int GetUnitBuildTime(short type) {
            if (GlobalSettings.Wrapper.tanksMap.ContainsKey(type))
                return GlobalSettings.Wrapper.tanksMap[type].BuildSpeed;
            if (GlobalSettings.Wrapper.troopersMap.ContainsKey(type))
                return GlobalSettings.Wrapper.troopersMap[type].BuildSpeed;
            if (GlobalSettings.Wrapper.mcvsMap.ContainsKey(type))
                return GlobalSettings.Wrapper.mcvsMap[type].BuildSpeed;
            if (GlobalSettings.Wrapper.harvestersMap.ContainsKey(type))
                return GlobalSettings.Wrapper.harvestersMap[type].BuildSpeed;
            return 0;

        }

        public override void OnShoot(Ammo a) {
            if (ammoShoot != null) {
                ammoShoot(a);
            }
        }

        public override void OnAmmoBlow(Ammo a) {
            if (ammoBlow != null) {
                ammoBlow(a);
            }
        }

		protected override void onMessagePlayerDisconnected(GameNumericMessage gameNumericMessage) {
			players[(short)gameNumericMessage.Number].IsDisconnected = true;
		}
	}
}
