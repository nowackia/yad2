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
	
		#region events
		public delegate void BuildingCompletedHandler (short buildingType);
		public delegate void UnitCompletedHandler (short unitType);

		public event BuildingCompletedHandler OnBuildingCompleted;
		public event UnitCompletedHandler OnUnitCompleted;
		#endregion

		IConnection connectionToServer;

		public ClientSimulation(GameSettingsWrapper settings, Map map, Player currPlayer, IConnection conn)
			: base(settings, map, currPlayer, false) {
			this.connectionToServer = conn;
			//this.onTurnBegin
			this.onTurnEnd += new Yad.Engine.Common.SimulationHandler(ClientSimulation_onTurnEnd);
		}

		void ClientSimulation_onTurnEnd() {
			//InfoLog.WriteInfo("Asking for turn", EPrefix.SimulationInfo);
			connectionToServer.SendMessage(new TurnAskMessage());
		}

		protected override void OnMessageBuild(BuildMessage bm) {
			InfoLog.WriteInfo("MessageBuild", EPrefix.SimulationInfo);
			BuildingData bd = base.GameSettingsWrapper.buildingsMap[bm.BuildingType];
			Building b = new Building(bm.IdPlayer, bm.BuildingID, bm.BuildingType, this.map, bm.Position, new Position(bd.Size));
			if (players[bm.IdPlayer] == null)
				throw new Exception("Message from unknown player");
            if (bm.IdPlayer.Equals(currentPlayer.ID)) {
				if (this.OnBuildingCompleted != null) {
					this.OnBuildingCompleted(bm.BuildingType);
				}
                //StripesManager.RemovePercentageCounter(bm.BuildingType,true);
            }
			players[bm.IdPlayer].AddBuilding(b);
		}

		protected override void onMessageMove(MoveMessage gm)
		{
			InfoLog.WriteInfo("MessageMove: PlayerID:" + gm.IdPlayer + " UnitID:" + gm.IdUnit, EPrefix.SimulationInfo);

			Player p = this.players[gm.IdPlayer];
			Unit u = p.GetUnit(gm.IdUnit);
			u.MoveTo(gm.Path);			
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
				u = new UnitTank(cum.IdPlayer, cum.UnitID, gameSettingsWrapper.tanksMap[cum.UnitType], cum.Position, this.map);
			} else if (boc == BoardObjectClass.UnitTrooper) {
				u = new UnitTrooper(cum.IdPlayer, cum.UnitID, gameSettingsWrapper.troopersMap[cum.UnitType], cum.Position, this.map);
			}
			if (players[cum.IdPlayer] == null)
				throw new Exception("Message from unknown player");
			players[cum.IdPlayer].AddUnit(u);
			this.map.Units[u.Position.X, u.Position.Y].AddLast(u);
		}

		protected override void onInvalidMove(Yad.Board.Common.Unit unit) {
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
