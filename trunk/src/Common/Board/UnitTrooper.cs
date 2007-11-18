using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;

namespace Yad.Board.Common {
	public class UnitTrooper : Unit {
		public UnitTrooper(short playerID, int unitID, UnitTrooperData ud, Position pos, Map map)
			: base(playerID, unitID, ud.TypeID, BoardObjectClass.UnitTrooper, pos, map) {
			this.Speed = ud.Speed;
			//fill other properties
		}

		public override void Destroy() {
			throw new Exception("The method or operation is not implemented.");
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Move() {
			base.Move();
		}

		public override void DoAI() {
			throw new Exception("The method or operation is not implemented.");
		}
	}
}