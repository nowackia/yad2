using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Board.Common {
	public class Building : BoardObject {
		public Building(short playerID, int unitID, Position pos) : base(playerID, unitID, pos) {
		}
	}
}
