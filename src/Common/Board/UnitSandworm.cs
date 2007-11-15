using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Board.Common {
	public class UnitSandworm : Unit {
		public UnitSandworm(short playerID, int unitID, short typeID, Position pos) : base(playerID, unitID, typeID, Yad.Config.BoardObjectClass.UnitSandworm, pos) { }
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
