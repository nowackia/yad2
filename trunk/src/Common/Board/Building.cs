using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Board.Common {
	public class Building : BoardObject {
		private int health;
		private short typeId;
		private Position size;

		public Building(short playerID, int unitID, short typeId, Position pos, Position size)
			: base(playerID, unitID, pos) {
			this.typeId = typeId;
			this.size = size;
		}

		public short TypeId {
			get {
				return this.typeId;
			}
		}

		public int Health {
			get {
				return Health;
			}
		}

		public Position Size {
			get { return this.size; }
		}
	}
}
