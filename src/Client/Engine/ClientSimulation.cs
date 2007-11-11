using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Engine.Client
{
    class ClientSimulation : Yad.Engine.Common.Simulation
    {
		protected override void OnMessageBuild(BuildMessage bm) {
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void onMessageMove(MoveMessage gm) {
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void onMessageAttack(AttackMessage am) {
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void onMessageDestroy(DestroyMessage dm) {
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void onMessageHarvest(HarvestMessage hm) {
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void onMessageCreate(CreateUnitMessage cum) {
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
