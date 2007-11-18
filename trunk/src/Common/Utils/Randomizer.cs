using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;

namespace Yad.Utilities.Common {
	/// <summary>
	/// DO NOT USE IN SIMULATION!
	/// </summary>
	public static class Randomizer {
		static Random rnd = new Random();

		static public int Next(int max) {
			return rnd.Next(max);
		}

		static public double NextDouble() {
			return rnd.NextDouble();
		}

		static public short NextShort(short s) {
			return (short)rnd.Next(s);
		}
	}

	public static class UsefulFunctions {
		/// <summary>
		/// returns x <0;max)
		/// </summary>
		/// <param name="x"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static short CorrectDimension(short x, short max) {
			if (x < 0) x = 0;
			if (x >= max) x = (short)(max - 1);
			return x;
		}

		public static void CorrectPosition(ref Position pos, short maxX, short maxY) {
			if (pos.X < 0) pos.X = 0;
			if (pos.X >= maxX) pos.X = (short)(maxX - 1);
			if (pos.Y < 0) pos.Y = 0;
			if (pos.Y >= maxY) pos.Y = (short)(maxY - 1);
		}
	}
}
