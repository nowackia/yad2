using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    public class AttackMessage : GameMessage
    {
		private int attackingPlayerID;
		private short attackingObjectID;
		private int attackedPlayerID;
		private short attackedObjectID;

        public AttackMessage()
            : base(MessageType.Attack)
        { }

		public short AttackedObjectID {
			get { return attackedObjectID; }
			set { attackedObjectID = value; }
		}

		public int AttackedPlayerID {
			get { return attackedPlayerID; }
			set { attackedPlayerID = value; }
		}

		public short AttackingObjectID {
			get { return attackingObjectID; }
			set { attackingObjectID = value; }
		}

		public int AttackerID {
			get { return attackingPlayerID; }
			set { attackingPlayerID = value; }
		}

		public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            attackingPlayerID = reader.ReadInt32();
            attackingObjectID = reader.ReadInt16();
            attackedPlayerID = reader.ReadInt32();
            attackedObjectID = reader.ReadInt16();
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(attackingPlayerID);
            writer.Write(attackingObjectID);
            writer.Write(attackedPlayerID);
            writer.Write(attackedObjectID);
           
        }
    }
}
