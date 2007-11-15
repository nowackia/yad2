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

namespace Yad.Engine.Client {
	public class ClientSimulation : Yad.Engine.Common.Simulation {

		IConnection connectionToServer;

		public ClientSimulation(GameSettingsWrapper settings, Map map, IConnection conn)
			: base(settings, map, false) {
			this.connectionToServer = conn;
			//this.onTurnBegin
			this.onTurnEnd += new Yad.Engine.Common.SimulationHandler(ClientSimulation_onTurnEnd);
		}

		void ClientSimulation_onTurnEnd() {
			connectionToServer.SendMessage(new MessageTurnAsk());
		}

		protected override void OnMessageBuild(BuildMessage bm) {
			InfoLog.WriteInfo("MessageBuild", EPrefix.SimulationInfo);
			BuildingData bd = base.GameSettingsWrapper.buildingsMap[bm.BuildingType];
			Building b = new Building(bm.PlayerId, bm.BuildingID, bm.BuildingType, bm.Position, new Position(bd.Size));
			if (players[bm.PlayerId] == null)
				throw new Exception("Message from unknown player");
			players[bm.PlayerId].AddBuilding(b);
			this.map.Buildings[b.Position.X, b.Position.Y].AddLast(b);
		}

		protected override void onMessageMove(MoveMessage gm) {
			InfoLog.WriteInfo("MessageMove", EPrefix.SimulationInfo);
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
				u = new UnitTank(cum.PlayerId, cum.UnitID, cum.UnitType, cum.Position);
			} else if (boc == BoardObjectClass.UnitTrooper) {
				u = new UnitTrooper(cum.PlayerId, cum.UnitID, cum.UnitType, cum.Position);
			}
			if (players[cum.PlayerId] == null)
				throw new Exception("Message from unknown player");
			players[cum.PlayerId].AddUnit(u);
			this.map.Units[u.Position.X, u.Position.Y].AddLast(u);
		}

		protected override void onInvalidMove(Yad.Board.Common.Unit unit) {
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
