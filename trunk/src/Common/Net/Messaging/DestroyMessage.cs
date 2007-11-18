using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    public class DestroyMessage : GameMessage
    {
        private int objectID;
		private short playerID;

        public DestroyMessage()
            : base(MessageType.Destroy)
        { }

		public short PlayerID {
			get { return playerID; }
			set { playerID = value; }
		}

        public int ObjectID
        {
            get { return objectID; }
            set { objectID = value; }
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            objectID = reader.ReadInt32();
            playerID = reader.ReadInt16();
            
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(objectID);
            writer.Write(playerID);
        }
    }
}
