using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;

namespace Yad.Board.Common {
	public class Building : BoardObject {
		private int health;
		private Position size;
		private short typeId;

		public Building(short playerID, int unitID, short typeID, Position pos, Position size)
			: base(playerID, unitID, BoardObjectClass.Building, pos) {
			this.size = size;
			this.typeId = typeID;
		}

		public short TypeID {
			get { return this.typeId; }
		}

		public int Health {
			get {
				return health;
			}
		}

		public Position Size {
			get { return this.size; }
		}
	}
}
