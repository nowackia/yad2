using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;

namespace Yad.Board.Common {
	public class UnitMCV : Unit {
		public UnitMCV(short playerID, int unitID, UnitMCVData ud, Position pos, Map map)
			: base(playerID, unitID, ud.TypeID, BoardObjectClass.UnitMCV, pos, map) {
			this.Speed = ud.Speed;
			//fill other properties
			this.viewRange = ud.ViewRange;
			this.damageDestroy = ud.DamageDestroy;
			//ud.BuildSpeed
			this.health = ud.Health;
			this.rotationSpeed = ud.RotationSpeed;
		}

		public override void Destroy() {
			base.Destroy();
		}

		public override void Move() {
			base.Move();
		}

		public override void DoAI() {
			base.DoAI();
		}
	}
}
