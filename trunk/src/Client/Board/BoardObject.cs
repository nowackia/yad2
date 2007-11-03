using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Board {
	/// <summary>
	/// base object for all objects placed on map: units, buildings.
	/// base object have animation -> bullet, rocket ..
	/// </summary>
	public class BoardObject {
		/// <summary>
		/// This will be null for Server.
		/// </summary>
		Animation animation;
	}
}
