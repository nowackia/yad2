using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;
using Yad.Board.Common;
using Yad.Config.Common;
using Yad.Board;
using Yad.Config;
using Client.UI;
using Client.Net;
using Yad.Net.Messaging;

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
			Building b = new Building(bm.PlayerId, bm.BuildingID, bm.BuildingType, bm.Position);
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
			Unit u = new UnitTank(cum.PlayerId, cum.UnitID, cum.Position);
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
