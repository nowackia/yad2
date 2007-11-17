using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;

namespace Yad.Board.Common {
	public class UnitTank : Unit {
		private Animation turretAnimation;
		private int turretRotationSpeed;

		public UnitTank(short playerID, int unitID, UnitTankData ud, Position pos, Map map)
			: base(playerID, unitID, ud.TypeID, BoardObjectClass.UnitTank, pos, map) {
			this.Speed = ud.Speed;
			//fill other properties
		}

		public int TurretRotationSpeed {
			get { return turretRotationSpeed; }
		}


		public Animation TurretAnimation {
			get { return turretAnimation; }
		}

		public override void Destroy() {
			throw new Exception("The method or operation is not implemented.");
		}

		public override void Move() {
			base.Move();
		}

		public override void DoAI() {
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
