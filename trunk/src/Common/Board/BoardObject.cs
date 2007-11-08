using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Board.Common {
	/// <summary>
	/// base object for all objects placed on map: units, buildings.
	/// base object have animation -> bullet, rocket ..
	/// </summary>
	public class BoardObject {

		/// <summary>
		/// These two values identify board object. PlayerID and objectID is assigned by each Player.
		/// </summary>
		short playerID;
		int objectID;
		Position position;

		/// <summary>
		/// This will be null for Server.
		/// </summary>
		Animation animation;
	}
}
