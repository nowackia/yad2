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
		public delegate void OnLowPowerHandler(Player p);
		public delegate void OnNoPowerHandler(Player p);
		public delegate void OnCreditsHandler(int cost);
		public delegate void InvalidLocationHandler();

		public event OnCreditsHandler OnCreditsUpdate;
		public event OnLowPowerHandler OnLowPowerResources;
		public event OnNoPowerHandler OnNoPowerResources;
		public event BuildingCreationHandler BuildingCompleted;
		public event UnitHandler UnitCompleted;
		public event BuildingHandler BuildingDestroyed;
		public event UnitHandler UnitDestroyed;
		public event BuildingHandler BuildingStarted;
		public event UnitHandler UnitStarted;
		public event InvalidLocationHandler InvalidLocation;
		#endregion

		public ClientSimulation(Map map)
			: base(map, false) {

			PlayerInfo currPI = ClientPlayerInfo.Player;

			//Add all players
			foreach (PlayerInfo pi in ClientPlayerInfo.GetAllPlayers()) {
				Player p = new Player(pi.Id, pi.Name, pi.House, pi.Color, map);
				p.TeamID = pi.TeamID;
				p.Credits = Settings.Default.CreditsAtStart;
				p.Power = Settings.Default.PowerAtStart;
				players.Add(p.Id, p);
			}

			this.onTurnBegin += new SimulationHandler(ClientSimulation_onTurnBegin);
			this.onTurnEnd += new Yad.Engine.Common.SimulationHandler(ClientSimulation_onTurnEnd);
			this.OnLowPowerResources += new OnLowPowerHandler(ClientSimulation_OnLowPowerResources);
			this.OnNoPowerResources += new OnNoPowerHandler(ClientSimulation_OnNoPowerResources);
			this.UnitStarted += new UnitHandler(ClientSimulation_UnitStarted);
			this.UnitCompleted += new UnitHandler(ClientSimulation_UnitCompleted);
		}

		void ClientSimulation_onTurnBegin() {
			Connection.Instance.SendMessage(tam);
		}

		void ClientSimulation_onTurnEnd() {
		}

		protected override void OnMessageBuild(BuildMessage bm) {
			//InfoLog.WriteInfo("MessageBuild", EPrefix.SimulationInfo);

			BuildingData bd = GlobalSettings.Wrapper.buildingsMap[bm.BuildingType];
			if (!Building.CheckBuildPosition(bd, bm.Position, _map, bm.IdPlayer)) {
				if (InvalidLocation != null) {
					InvalidLocation();
                    /*int cost = GlobalSettings.Wrapper.buildingsMap[bm.BuildingType].Cost;
                    p.Credits += cost;
                    OnCreditsUpdate(cost);*/
				}
				return;
			}

            //TO JEST B£¥D! jeœli nas nie staæ to nam siê nie wybuduje, ale wszystkim innym siê wybuduje!
            int playerCredits = players[bm.IdPlayer].Credits;
            int buildingCost = GlobalSettings.Wrapper.buildingsMap[bm.BuildingType].Cost;
            if (playerCredits < buildingCost)
                return;
            players[bm.IdPlayer].Credits -= buildingCost;
            if (OnCreditsUpdate != null)
                OnCreditsUpdate(buildingCost);
            AddBuilding(bm.IdPlayer, bm.CreatorID, bm.BuildingType, bm.Position);
		}

		protected override void onMessageMove(MoveMessage gm) {
			InfoLog.WriteInfo("MessageMove: PlayerID:" + gm.IdPlayer + " UnitID:" + gm.IdUnit, EPrefix.SimulationInfo);

			Player p = this.players[gm.IdPlayer];
			Unit u = p.GetUnit(ObjectID.From(gm.IdUnit, gm.IdPlayer));
			if (u == null) {
				InfoLog.WriteInfo("MessageMove: PlayerID:" + gm.IdPlayer + " unit has been already destroyed", EPrefix.SimulationInfo);
				return;
			}
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

			attacker.OrderAttack(b, isBuilding);
		}

		protected override void onMessageDestroy(DestroyMessage dm) {
			InfoLog.WriteInfo("MessageDestroy", EPrefix.SimulationInfo);
		}

		protected override void onMessageHarvest(HarvestMessage hm) {
			InfoLog.WriteInfo("MessageHarvest", EPrefix.SimulationInfo);
		}

		protected override void onMessageCreate(CreateUnitMessage cum) {
			InfoLog.WriteInfo("MessageCreate", EPrefix.SimulationInfo);
			int cost=0;
			Player p = players[cum.IdPlayer];
			BoardObjectClass boc = cum.UnitKind;
			Unit u = null;
			ObjectID id = new ObjectID(cum.IdPlayer, p.GenerateObjectID());
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
				players[cum.IdPlayer].Credits -= GlobalSettings.Wrapper.harvestersMap[cum.UnitType].Cost;
			} else if (boc == BoardObjectClass.UnitMCV) {
				if (players[cum.IdPlayer].Credits < (cost=GlobalSettings.Wrapper.mcvsMap[cum.UnitType].Cost))
					return;
				u = new UnitMCV(id, GlobalSettings.Wrapper.mcvsMap[cum.UnitType], cum.Position, this._map, this);
				players[cum.IdPlayer].Credits -= GlobalSettings.Wrapper.mcvsMap[cum.UnitType].Cost;
			} else if (boc == BoardObjectClass.UnitSandworm) {
				u = new UnitSandworm(id, GlobalSettings.Wrapper.sandwormsMap[cum.UnitType], cum.Position, this._map, this, GlobalSettings.Wrapper.sandwormsMap[cum.UnitType].__Speed);
			//} else if (boc == BoardObjectClass.UnitTank) {
			//    u = new UnitTank(id, GlobalSettings.Wrapper.tanksMap[cum.UnitType], cum.Position, this._map, this);
			//} else if (boc == BoardObjectClass.UnitTrooper) {
			//    u = new UnitTrooper(id, GlobalSettings.Wrapper.troopersMap[cum.UnitType], cum.Position, this._map, this);

			}
			OnCreditsUpdate(cost);
			p.AddUnit(u);
			ClearFogOfWar(u);

			OnUnitCompleted(u);
		}


        private void AddBuilding(short playerID, int creatorID, short buildingType, Position pos) {
            Player p = players[playerID];
            ObjectID id = new ObjectID(playerID, p.GenerateObjectID());
            BuildingData bd = GlobalSettings.Wrapper.buildingsMap[buildingType];
            Building b = new Building(id, bd, this._map, pos, this);
            p.AddBuilding(b);
            ClearFogOfWar(b);
            UpdatePowerManagement(b);
            OnBuildingCompleted(b, creatorID);    
        }

		protected override void onMessageDeployMCV(Yad.Net.Messaging.GMDeployMCV dmcv) {
            Player p = players[dmcv.IdPlayer];
            UnitMCV mcv = (UnitMCV)p.GetUnit(ObjectID.From(dmcv.McvID.ObjectId, dmcv.McvID.PlayerID));
            string bName = mcv.MCVData.BuildingCanProduce;
            short btype = GlobalSettings.Wrapper.namesToIds[bName];
            this._map.Units[mcv.Position.X, mcv.Position.Y].Remove(mcv);
            BuildingData bd = GlobalSettings.Wrapper.buildingsMap[btype];
            if (!Building.CheckBuildPosition(bd, mcv.Position, _map, dmcv.McvID.PlayerID)) {
                this._map.Units[mcv.Position.X, mcv.Position.Y].AddLast(mcv);
                if (InvalidLocation != null) {
                    InvalidLocation();
                }
                return;
            }
            this._map.Units[mcv.Position.X, mcv.Position.Y].AddLast(mcv);
            destroyUnit(mcv);
            AddBuilding(dmcv.McvID.PlayerID, -1, btype,  mcv.Position);
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
			InfoLog.WriteInfo("handleBuilding: not implemented");
			b.DoAI();
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

		public override void handleAttackUnit(Unit attacked, Building attacker, short count) {
			if (attacked.State == Unit.UnitState.destroyed) {
				// handles stack overflow;P
				return;
			}
			attacked.Health -= count;
			if (attacked.Health <= 0) {
				InfoLog.WriteInfo("destroying unit ", EPrefix.SimulationInfo);
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
	}
}
