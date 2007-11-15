using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;

namespace Yad.Board.Common {
	public class UnitHarvester : Unit {

		private int capacity;

		public UnitHarvester(short playerID, int unitID, short typeID, Position pos)
			: base(playerID, unitID, typeID, BoardObjectClass.UnitHarvester, pos) {
		}

		public int Capacity {
			get { return capacity; }
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
