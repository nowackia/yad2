using System;
using System.Collections.Generic;
using System.Text;

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
}
