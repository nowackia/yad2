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

namespace Yad.Engine.Client {
	public class ClientSimulation : Yad.Engine.Common.Simulation {
		TurnAskMessage tam = new TurnAskMessage();
	
		#region events
		public delegate void BuildingCompletedHandler (Building b);
		public delegate void UnitCompletedHandler (Unit u);

		public event BuildingCompletedHandler OnBuildingCompleted;
		public event UnitCompletedHandler OnUnitCompleted;
		#endregion

		public ClientSimulation(Map map, Player currPlayer)
			: base(map, currPlayer, false) {
			this.onTurnBegin += new SimulationHandler(ClientSimulation_onTurnBegin);
			this.onTurnEnd += new Yad.Engine.Common.SimulationHandler(ClientSimulation_onTurnEnd);
		}

		void ClientSimulation_onTurnBegin() {
			//This optimisation roxxxz! :D
			Connection.Instance.SendMessage(tam);
		}

		void ClientSimulation_onTurnEnd() {
			//InfoLog.WriteInfo("Asking for turn", EPrefix.SimulationInfo);
			//connectionToServer.SendMessage(tam);
		}

		protected override void OnMessageBuild(BuildMessage bm) {
			InfoLog.WriteInfo("MessageBuild", EPrefix.SimulationInfo);

			BuildingData bd = GlobalSettings.Wrapper.buildingsMap[bm.BuildingType];
			Building b = new Building(bm.IdPlayer, bm.BuildingID, bm.BuildingType, this.map, bm.Position, new Position(bd.Size));

			players[b.ObjectID.PlayerID].AddBuilding(b);

			if (b.ObjectID.PlayerID.Equals(currentPlayer.Id)) {
				if (OnBuildingCompleted != null) {
					this.OnBuildingCompleted(b);
				}
			}
		}

		protected override void onMessageMove(MoveMessage gm)
		{
			InfoLog.WriteInfo("MessageMove: PlayerID:" + gm.IdPlayer + " UnitID:" + gm.IdUnit, EPrefix.SimulationInfo);

			Player p = this.players[gm.IdPlayer];
			Unit u = p.GetUnit(ObjectID.From(gm.IdUnit,gm.IdPlayer));
			u.MoveTo(gm.Destination);			
		}

		protected override void onMessageAttack(AttackMessage am) {
			InfoLog.WriteInfo("MessageAttack", EPrefix.SimulationInfo);
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

			if (boc == BoardObjectClass.UnitTank) {
				u = new UnitTank(cum.IdPlayer, cum.UnitID, GlobalSettings.Wrapper.tanksMap[cum.UnitType], cum.Position, this.map);
			} else if (boc == BoardObjectClass.UnitTrooper) {
				u = new UnitTrooper(cum.IdPlayer, cum.UnitID, GlobalSettings.Wrapper.troopersMap[cum.UnitType], cum.Position, this.map);
			}
			players[cum.IdPlayer].AddUnit(u);

			if (u.ObjectID.PlayerID == currentPlayer.Id) {
				if (OnUnitCompleted != null) {
					this.OnUnitCompleted(u);
				}
			}
		}

		protected override void onInvalidMove(Yad.Board.Common.Unit unit) {
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
