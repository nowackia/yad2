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
				double delta = 1.0 / ((2*range+1)* 4 - 4);
                delta = Math.PI * 2.0 / ((2 * range + 1) * 4 - 4);
                //System.Console.Out.WriteLine(Math.PI * 2.0 / delta);
				for (double alfa = 0; alfa < 2.0 * Math.PI; alfa += delta) {
					// alfa
                    // dla x,y >=0 +=1;
                    double x=0.5;
                    double y=0.5;
                    if (alfa <= Math.PI) {
                        y = 0.5;
                        
                    }
                    if (alfa <= Math.PI / 2 || alfa >= 3.0 * Math.PI/2.0 ) {
                        x = 0.5;
                    }
					x += ( i * Math.Cos(alfa));
					y += ( i * Math.Sin(alfa));
                    //System.Console.Out.WriteLine("test: " + alfa + ":" + x + " " + y);
					Position p = new Position((int)Math.Floor(x), (int)Math.Floor(y));
                    if (spiral.Contains(p) == false) { spiral.Add(p); } else {
                        //System.Console.Out.WriteLine("istnieje: " + alfa + ":" + p);
                    }
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

        protected int GetAlfa(double x, double y) {
            if (x == y && x == 0)
                return 0;// teoretically wont happen
            double norm = x * x + y * y;
            norm = Math.Sqrt(norm);
            x /= norm;
            y /= norm;
            double al = Math.Asin(y);
            al *= 180.0 / Math.PI;
            if (y >= 0) {
                //1 | 2 quater
                if (x >= 0) {
                    return (int)al % 360;
                } else {
                    return 180 - (int)al % 360;
                }
            } else {
                // 3 | 4 quater
                if (x >= 0) {
                    return (int)(360 + al) % 360;
                } else {
                    return (int)(270 + al) % 360;
                }
            }

        }

        protected Direction ConvertToDirection(int number) {
            switch (number) {
                case 0:
                    return Direction.East;
                case 45:
                    return Direction.East | Direction.North;
                case 90:
                    return Direction.North;
                case 135:
                    return Direction.North | Direction.West;
                case 180:
                    return Direction.West;
                case 225:
                    return Direction.West | Direction.South;
                case 270:
                    return Direction.South;
                case 315:
                    return Direction.South | Direction.East;
                default:
                    return Direction.East; // never happen.
            }

        }

	}
}
