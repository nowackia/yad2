using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;

namespace Yad.Board.Common {
	public class UnitHarvester : Unit {

		private int capacity;

		public UnitHarvester(short playerID, int unitID, UnitHarvesterData ud, Position pos, Map map, int speed)
			: base(playerID, unitID, ud.TypeID, BoardObjectClass.UnitHarvester, pos, map) {
			this.Speed = ud.Speed;
			//fill other properties
			this.viewRange = ud.ViewRange;
			this.damageDestroy = ud.DamageDestroy;
			//ud.BuildSpeed
			this.health = ud.Health;
			this.rotationSpeed = ud.RotationSpeed;

		}

		public int Capacity {
			get { return capacity; }
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
