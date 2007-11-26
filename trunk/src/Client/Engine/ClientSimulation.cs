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

namespace Yad.Engine.Client {
	/// <summary>
	/// Use:
	/// destroyUnit, destroyBuilding
	/// </summary>
	public class ClientSimulation : Yad.Engine.Common.Simulation {
		MessageTurnAsk tam = new MessageTurnAsk();
		#region events
		public delegate void BuildingHandler (Building b);
		public delegate void UnitHandler (Unit u);
		public delegate void OnLowPowerHandler(Player p);
		public delegate void OnNoPowerHandler(Player p);
		public delegate void OnCreditsHandler(int cost);

		public event OnCreditsHandler OnCreditsUpdate;
		public event OnLowPowerHandler OnLowPowerResources;
		public event OnNoPowerHandler OnNoPowerResources; 
		public event BuildingHandler BuildingCompleted;
		public event UnitHandler UnitCompleted;
		public event BuildingHandler BuildingDestroyed;
		public event UnitHandler UnitDestroyed;
		public event BuildingHandler BuildingStarted;
		public event UnitHandler UnitStarted;
		#endregion

		public ClientSimulation(Map map)
			: base(map, false) {

			PlayerInfo currPI = ClientPlayerInfo.Player;

			//Add all players
			foreach (PlayerInfo pi in ClientPlayerInfo.GetAllPlayers()) {
				Player p = new Player(pi.Id, pi.Name, pi.House, pi.Color);
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
			InfoLog.WriteInfo("MessageBuild", EPrefix.SimulationInfo);

			BuildingData bd = GlobalSettings.Wrapper.buildingsMap[bm.BuildingType];
			ObjectID id = new ObjectID(bm.IdPlayer, bm.BuildingID);
			Building b = new Building(id, bd, this.map, bm.Position, this);

			//TO JEST B£¥D! jeœli nas nie staæ to nam siê nie wybuduje, ale wszystkim innym siê wybuduje!
			//if (b.ObjectID.PlayerID.Equals(currentPlayer.Id) && GlobalSettings.Wrapper.buildingsMap[b.TypeID].Cost > credits)
			//	return;

			Player p = players[b.ObjectID.PlayerID];
			p.AddBuilding(b);
			p.Credits -= b.BuildingData.Cost;

            OnBuildingCompleted(b);
			UpdatePowerManagement(b);
			OnCreditsUpdate(b.BuildingData.Cost);
		}

		protected override void onMessageMove(MoveMessage gm)
		{
			InfoLog.WriteInfo("MessageMove: PlayerID:" + gm.IdPlayer + " UnitID:" + gm.IdUnit, EPrefix.SimulationInfo);

			Player p = this.players[gm.IdPlayer];
			Unit u = p.GetUnit(ObjectID.From(gm.IdUnit,gm.IdPlayer));
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
            Player p = players[am.Attacked.PlayerID];
            BoardObject b;
            bool isBuilding = false;
            
            b = p.GetBuilding(am.Attacked);
            if(b!=null){
                isBuilding = true;
            }else
            {
                b = p.GetUnit(am.Attacked);
                if(b==null)
                {
                    InfoLog.WriteInfo("MessageMove: PlayerID:" + am.Attacked.PlayerID + " attacked object does not exist", EPrefix.SimulationInfo);
                   return;
                }
            }

            attacker.OrderAttack(b,isBuilding);


		}

		protected override void onMessageDestroy(DestroyMessage dm) {
			InfoLog.WriteInfo("MessageDestroy", EPrefix.SimulationInfo);
		}

		protected override void onMessageHarvest(HarvestMessage hm) {
			InfoLog.WriteInfo("MessageHarvest", EPrefix.SimulationInfo);
		}

		protected override void onMessageCreate(CreateUnitMessage cum) {
			InfoLog.WriteInfo("MessageCreate", EPrefix.SimulationInfo);

			BoardObjectClass boc = cum.UnitKind;
			Unit u = null;
			ObjectID id = new ObjectID(cum.IdPlayer, cum.UnitID);
			if (boc == BoardObjectClass.UnitTank) {
				u = new UnitTank(id, GlobalSettings.Wrapper.tanksMap[cum.UnitType], cum.Position, this.map,this);
			} else if (boc == BoardObjectClass.UnitTrooper) {
                u = new UnitTrooper(id, GlobalSettings.Wrapper.troopersMap[cum.UnitType], cum.Position, this.map, this);
			}
			players[cum.IdPlayer].AddUnit(u);

            OnUnitCompleted(u);
		}

        

		protected override void onMessageDeployMCV(Yad.Net.Messaging.GMDeployMCV dmcv) {
			InfoLog.WriteInfo("onMessageDeployMCV: not implemented");
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
            attacked.Health -= attacker.FirePower;
            if (attacked.Health <= 0) {
                InfoLog.WriteInfo("destroying unit ", EPrefix.SimulationInfo);
                destroyUnit(attacked);
				OnUnitDestroyed(attacked);
            }
        }
        /// <summary>
        /// handles attack - counts damage, destroy building
        /// </summary>
        /// <param name="attacked"></param>
        /// <param name="attacker"></param>
        public override void handleAttackBuilding(Building attacked, Unit attacker) {
            attacked.Health -= attacker.FirePower;
            if (attacked.Health <= 0) {
                InfoLog.WriteInfo("destroying building ", EPrefix.SimulationInfo);
                destroyBuilding(attacked);
				OnBuildingDestroyed(attacked);
            }
        }

        public override void handleAttackBuilding(Building attacked, Building attacker) {
            attacked.Health -= attacker.BuildingData.FirePower;
            if (attacked.Health <= 0) {
                InfoLog.WriteInfo("destroying building ", EPrefix.SimulationInfo);
                destroyBuilding(attacked);
				OnBuildingDestroyed(attacked);
            }
        }

        public override void handleAttackUnit(Unit attacked, Building attacker) {
            attacked.Health -= attacker.BuildingData.FirePower;
            if (attacked.Health <= 0) {
                InfoLog.WriteInfo("destroying building ", EPrefix.SimulationInfo);
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

        private void OnBuildingCompleted(Building b) {
            if (BuildingCompleted != null) {
                //TODO RS: run in different thread
                this.BuildingCompleted(b);
            }
        }

		void ClientSimulation_UnitStarted(Unit u) {
			throw new Exception("The method or operation is not implemented.");
		}


		internal void UpdatePowerManagement(Building b) {
			Player p = players[b.ObjectID.PlayerID];
			p.Power -= b.BuildingData.EnergyConsumption;
			if (p.Power < Settings.Default.PowerLowBorder) {
				OnLowPowerResources(p);
			} else if (p.Power < 0) {
				OnNoPowerResources(p);
			}
		}

		void ClientSimulation_OnLowPowerResources(Player p) {
			//TODO: Stub metody do obsluzenia sytuacji kiedy jest malo energii
		}

		void ClientSimulation_OnNoPowerResources(Player p) {
			//TODO: Stub metody do obsluzenia sytuacji kiedy zapotrzebowanie na energie przekroczylo jej produkcje
		}

		internal void CreateUnit(short id, Position pos) {
			throw new Exception("The method or operation is not implemented.");
		}


		void ClientSimulation_UnitCompleted(Unit u) {
			//if (UnitCompleted != null) {
				//TODO RS: run in different thread
				//this.UnitCompleted(u);
			//}
		}

		public Player getPlayer(short id) {
			return this.players[id];
		}
	}
}
