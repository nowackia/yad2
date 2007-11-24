using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;
using Yad.Engine.Common;

namespace Yad.Board.Common {
	public class UnitTank : Unit {
		private Animation turretAnimation;
		UnitTankData _tankData;

		public UnitTank(ObjectID id, UnitTankData ud, Position pos, Map map, Simulation sim)
			: base(id, ud.TypeID, BoardObjectClass.UnitTank, pos, map, sim) {
			_tankData = ud;
			this.Speed = ud.Speed;
			this._viewRange = ud.ViewRange;
            this._fireRange = ud.FireRange;
            this._reloadTime = ud.ReloadTime;
            this._firePower = ud.FirePower;
            this.MaxHealth = this.Health = ud.__Health;
		}

		public Animation TurretAnimation {
			get { return turretAnimation; }
		}

		public override void Destroy() {
			base.Destroy();
		}

		public override bool Move() {
			return base.Move();
		}

		public override void DoAI() {
			base.DoAI();
		}

		public UnitTankData TankData {
			get { return _tankData; }
		}
	}
}
