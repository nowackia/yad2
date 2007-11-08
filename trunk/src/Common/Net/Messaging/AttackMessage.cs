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

		public AttackMessage()
            : base(MessageType.Attack)
        { }

		public override void Deserialize(System.IO.BinaryReader reader) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
