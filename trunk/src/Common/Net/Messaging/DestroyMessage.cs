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
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
