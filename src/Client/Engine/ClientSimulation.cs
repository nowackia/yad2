using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;
using Yad.Board.Common;
using Yad.Config.Common;
using Yad.Board;

namespace Yad.Engine.Client {
	public class ClientSimulation : Yad.Engine.Common.Simulation {
		public static GameSettings gameSettings;

		public ClientSimulation()
			: base(false) {
			//this.onTurnBegin
			//this.onTurnEnd
		}

		protected override void OnMessageBuild(BuildMessage bm) {
			InfoLog.WriteInfo("MessageBuild", EPrefix.SimulationInfo);
			Building b = new Building(bm.PlayerId, bm.BuildingID, bm.BuildingType, bm.Position);
			if (players[bm.PlayerId] == null)
				throw new Exception("Message from unknown player");
			players[bm.PlayerId].buildings.Add(bm.BuildingID, b);
			Map.Buildings[b.Position.X, b.Position.Y].AddLast(b);
			//Map.
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
		}
	}
}
