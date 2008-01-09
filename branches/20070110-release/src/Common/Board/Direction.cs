using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Board {
	[Flags]
	public enum Direction : short {
		None = 0, North = 1, East = 2, South = 4, West = 8
	}
}
