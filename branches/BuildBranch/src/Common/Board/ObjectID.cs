using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Yad.Board {

	public struct ObjectID {
		int _objectID;
		short _playerID;

		public ObjectID(short playerID, int objectID) {
			this._objectID = objectID;
			this._playerID = playerID;
		}

		public int ObjectId {
			get { return _objectID; }
		}

		public short PlayerID {
			get { return _playerID; }
		}

		public static ObjectID From(int objectID, short playerID) {
			return new ObjectID(playerID, objectID);
		}

		public override bool Equals(object obj) {
			if ((obj is ObjectID) == false) return false;
			ObjectID ob = (ObjectID)obj;
			return this._playerID == ob._playerID && this._objectID == ob._objectID;
		}

		public override int GetHashCode() {
			return _objectID + 1000 * this.PlayerID;

			//Maybe this? (max 8 players) So that we don't limit objectID to 1000
			//return objectID * 10 + this.playerID;
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(this._playerID);
			writer.Write(this._objectID);
		}

		public void Deserialize(BinaryReader reader) {
			_playerID = reader.ReadInt16();
			_objectID = reader.ReadInt32();
		}

        public static ObjectID CreateNull(){
            return new ObjectID(-1,-1);
            
        }
	}
}
