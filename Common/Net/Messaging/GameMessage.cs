using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common {
	public abstract class GameMessage : Message {
		private int idTurn;
		private short idPlayer;

		public short IdPlayer {
			get { return idPlayer; }
			set { idPlayer = value; }
		}

		public GameMessage(MessageType msgType)
			: base(msgType) { }

		public int IdTurn {
			get { return idTurn; }
			set { idTurn = value; }
		}

		public override void Serialize(System.IO.BinaryWriter writer) {
			base.Serialize(writer);
			writer.Write(idTurn);
			writer.Write(idPlayer);
		}

		public override void Deserialize(System.IO.BinaryReader reader) {
			base.Deserialize(reader);
			idTurn = reader.ReadInt32();
			idPlayer = reader.ReadInt16();
		}

        public override string ToString() {
            return base.ToString() + "idPlayer: " + idPlayer + "idTurn: " + idTurn;
        }
	}
}
