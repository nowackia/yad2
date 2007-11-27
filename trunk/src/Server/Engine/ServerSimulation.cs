using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;
using Yad.Config.Common;
using Yad.Board.Common;
using Yad.Config;
using Yad.Engine.Common;

namespace Yad.Engine.Server {
	/// <summary>
	/// 
	/// </summary>
	[Obsolete("Don't do anything in here! It might be useless ;P")]
	class ServerSimulation : Yad.Engine.Common.Simulation {
		public ServerSimulation(Map map)
			: base(map, true) {
			//this.onTurnBegin
			//this.onTurnEnd
		}

		protected override void OnMessageBuild(BuildMessage bm) {
			InfoLog.WriteInfo("MessageBuild", EPrefix.SimulationInfo);
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

		protected override void onMessageDeployMCV(Yad.Net.Messaging.GMDeployMCV dmcv) {
			InfoLog.WriteInfo("onMessageDeployMCV: not implemented");
		}

		protected override void onInvalidMove(Yad.Board.Common.Unit unit) {
			InfoLog.WriteInfo("onInvalidMove: not implemented");
		}


		protected override void handleUnit(Unit u) {
			//u.Move();
		}

		protected override void handleBuilding(Building b) {
			InfoLog.WriteInfo("handleBuilding: not implemented");
		}

        

        public override void handleAttackUnit(Unit attacked, Unit attacker) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void handleAttackBuilding(Building attacked, Unit attacker) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void handleAttackBuilding(Building attacked, Building attacker) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void handleAttackUnit(Unit attacked, Building attacker) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void handleAttackUnit(Unit attacked, Unit attacker, short count) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void handleAttackBuilding(Building attacked, Unit attacker, short count) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void handleAttackBuilding(Building attacked, Building attacker, short count) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void handleAttackUnit(Unit attacked, Building attacker, short count) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
