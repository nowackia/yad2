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
	public class BoardObject {

		protected static GameSettings gameSettings = new GameSettings();

		/// <summary>
		/// These two values identify board object. PlayerID and objectID is assigned by each Player.
		/// </summary>
		short playerID;
		int objectID;
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
		public BoardObject(short pID, int oID, BoardObjectClass ot, Position pos) {
			this.playerID = pID;
			this.objectID = oID;
			this.position = pos;
			this.boardObjectClass = ot;
		}

		public BoardObjectClass BoardObjectClass {
			get {
				return this.boardObjectClass;
			}
		}

		public short PlayerID {
			get { return this.playerID; }
		}

		public int ObjectID {
			get { return this.objectID; }
		}

		public Position Position {
			get { return position; }
			set { position = value; }
		}
	}
}
