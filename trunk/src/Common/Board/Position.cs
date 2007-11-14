using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Board {
	/// <summary>
	/// Use this to define position on map.
	/// Deserialize using two shorts only.
	/// </summary>
	public struct Position {
		private short _x;

		public short X {
			get { return _x; }
			set { _x = value; }
		}

		private short _y;

		public short Y {
			get { return _y; }
			set { _y = value; }
		}

		public Position(short x, short y) {
			this._x = x;
			this._y = y;
		}

		public override string ToString() {
			return "{" + _x + "," + _y + "}";
		}
	}
}
