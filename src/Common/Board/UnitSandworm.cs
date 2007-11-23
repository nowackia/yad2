using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config.Common;

namespace Yad.Board.Common {
	public class UnitSandworm : Unit {
		UnitSandwormData _sandwormData;

		public UnitSandworm(ObjectID id, UnitSandwormData ud, Position pos, Map map, int speed)
			: base(id, ud.TypeID, Yad.Config.BoardObjectClass.UnitSandworm, pos, map) {
			_sandwormData = ud;
			this.Speed = ud.Speed;
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
