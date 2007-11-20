using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;

namespace Yad.Board.Common {
	public class Building : BoardObject {
		private int _health;
		private Position _size;
		private short _typeId;

		Map _map;
		bool _alreadyOnMap = false;

		public Building(short playerID, int unitID, short typeID, Map map, Position pos, Position size)
			: base(playerID, unitID, BoardObjectClass.Building, pos) {
			this._size = size;
			this._typeId = typeID;
			this._map = map;
		}

		public short TypeID {
			get { return this._typeId; }
		}

		public int Health {
			get {
				return _health;
			}
		}

		public Position Size {
			get { return this._size; }
		}

		public bool PlaceOnMap() {
			if (!_alreadyOnMap) {
				for (int x = 0; x < this.Size.X; x++) {
					for (int y = 0; y < this.Size.Y; y++) {
						this._map.Buildings[x + Position.X, y + Position.Y].AddLast(this);
					}
				}
				return true;
			}
			return false;
		}
	}
}
