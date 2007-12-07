using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

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

		public Position(int x, int y) {
			this._x = (short)x;
			this._y = (short)y;
		}

		public Position(Yad.Config.Common.Size size) {
			this._x = (short)size.X;
			this._y = (short)size.Y;
		}

		/*
		public Position(Point p) {
			this._x = (short)p.X;
			this._y = (short)p.Y;
		}
		 */

		public override string ToString() {
			return "{" + _x + "," + _y + "}";
		}

        public void Serialize(BinaryWriter writer) {
            writer.Write(this._x);
            writer.Write(this._y);
        }

        public void Deserialize(BinaryReader reader) {
            _x = reader.ReadInt16();
            _y = reader.ReadInt16();
        }

        public override bool Equals(object obj) {
            if (obj is Position) {
                Position ob = (Position)obj;
                return _x == ob._x && _y == ob._y;
            }
            return false;
        }

        public override int GetHashCode() {
            return (((int)_x)*10000 + ((int)_y));
        }
	}
}
