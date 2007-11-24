using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config.Common;
using Yad.Engine.Common;

namespace Yad.Board.Common {
	public class UnitSandworm : Unit {
		UnitSandwormData _sandwormData;

		public UnitSandworm(ObjectID id, UnitSandwormData ud, Position pos, Map map, Simulation sim, int speed)
			: base(id, ud.TypeID, Yad.Config.BoardObjectClass.UnitSandworm, pos, map,sim) {
			_sandwormData = ud;
			this.Speed = ud.Speed;
            this.MaxHealth = this.Health = ud.__Health;
            //this.FirePower = ud.
            //this._viewRange = ud.ViewRange;
            //this._fireRange = ud.FireRange;
            //this._reloadTime = ud.ReloadTime;
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

		public UnitSandwormData SandwormData {
			get { return _sandwormData; }
		}
	}
}
