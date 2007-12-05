using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config.Common;
using Yad.Config;

namespace Yad.Board.Common {
	/// <summary>
	/// base object for all objects placed on map: units, buildings.
	/// base object have animation -> bullet, rocket ..
	/// </summary>
	/// 

	public class BoardObject {
		ObjectID objectID;
		Position position;

		BoardObjectClass boardObjectClass;

		/// <summary>
		/// This will be null for Server.
		/// </summary>
		Animation animation;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="pID">playerID</param>
		/// <param name="oID">objectID</param>
		/// <param name="pos">position</param>
		public BoardObject(ObjectID objectID, BoardObjectClass ot, Position pos) {
			this.objectID = objectID;
			this.position = pos;
			this.boardObjectClass = ot;
		}

		public BoardObjectClass BoardObjectClass {
			get {
				return this.boardObjectClass;
			}
		}

		public ObjectID ObjectID {
			get { return this.objectID; }
		}

		public Position Position {
			get { return position; }
			set { position = value; }
		}

		private static Position[] rangeSpiral;
		private static Dictionary<int, int> lenghts = new Dictionary<int, int>();

		protected static Queue<Position> Bresenham(ref Position source, ref Position dest) {
			Queue<Position> path = new Queue<Position>();

			//TODO Go-Go-Gadget!

			//remove

			int diffX = dest.X - source.X;
			int diffY = dest.Y - source.Y;
			float x = source.X, y = source.Y;
			int m;

			if (Math.Abs(diffX) > Math.Abs(diffY)) {
				m = (diffX > 0) ? 1 : -1;
				float dy = (float)diffY / (float)Math.Abs(diffX);
				for (int i = 0; i < Math.Abs(diffX); i++) {
					x += m;
					y += dy;
					path.Enqueue(new Position((short)x, (short)y));
				}
			} else {
				m = (diffY > 0) ? 1 : -1;
				float dx = diffX / (float)Math.Abs(diffY);
				for (int i = 0; i < Math.Abs(diffY); i++) {
					x += dx;
					y += m;
					path.Enqueue(new Position((short)x, (short)y));
				}
			}
			return path;
		}

		private static void GenerateSpiral(int range) {


			List<Position> spiral = new List<Position>();
			spiral.Add(new Position(0, 0));
			lenghts[0] = 1;
			for (int i = 1; i <= range; ++i) {
				// for each radius
				double delta = 1.0 / range;
				for (double alfa = 0; alfa < 2 * Math.PI; alfa += delta) {
					// alfa
					int x = (int)(i * Math.Cos(alfa));
					int y = (int)(i * Math.Sin(alfa));
					Position p = new Position(x, y);
					if (spiral.Contains(p) == false) spiral.Add(p);
				}
				lenghts[i] = spiral.Count;
			}
			rangeSpiral = spiral.ToArray();
		}

		public static Position[] RangeSpiral(int range, out int max) {
			if (!lenghts.ContainsKey(range)) {
				GenerateSpiral(range);
			}
			max = lenghts[range];
			return rangeSpiral;
		}

	}
}
