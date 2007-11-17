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
