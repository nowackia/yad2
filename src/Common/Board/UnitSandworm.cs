using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config.Common;

namespace Yad.Board.Common {
	public class UnitSandworm : Unit {
		public UnitSandworm(short playerID, int unitID, UnitSandwormData ud, Position pos, Map map, int speed)
			: base(playerID, unitID, ud.TypeID, Yad.Config.BoardObjectClass.UnitSandworm, pos, map) {
			this.Speed = ud.Speed;
			//fill other properties
		}

		public override void Destroy() {
			throw new Exception("The method or operation is not implemented.");
		}

		public override void Move() {
			throw new Exception("The method or operation is not implemented.");
		}

		public override void DoAI() {
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
