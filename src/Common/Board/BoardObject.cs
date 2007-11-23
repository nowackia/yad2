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
	}
}
