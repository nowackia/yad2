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

    public class ObjectID {
        protected int objectID;
        protected short playerID;
        public int ObjectId {
            get { return objectID; }
        }
        public short PlayerID {
            get { return playerID; }
        }
        public ObjectID(int objectID, short playerID) {
            this.objectID = objectID;
            this.playerID = playerID;
        }
        public static ObjectID From(int objectID, short playerID) {
            return new ObjectID(objectID, playerID);
        }

        public override bool Equals(object obj) {
            if ((obj is ObjectID) == false) return false;
            ObjectID ob = (ObjectID)obj;
            return this.playerID == ob.playerID && this.objectID == ob.objectID;
        }

        public override int GetHashCode() {
            return objectID +  1000 * this.PlayerID;
        }

    }

	public class BoardObject {

		protected static GameSettings gameSettings = new GameSettings();


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
		public BoardObject(short pID, int oID, BoardObjectClass ot, Position pos) {
            this.objectID = new ObjectID(oID, pID);
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
