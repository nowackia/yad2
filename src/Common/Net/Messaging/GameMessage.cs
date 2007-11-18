using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    public abstract class GameMessage : Message
    {
        private int idTurn;

        public GameMessage(MessageType msgType)
            : base(msgType)
        { }

        public int IdTurn
        {
            get { return idTurn; }
            set { idTurn = value; }
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(idTurn);
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            idTurn = reader.ReadInt32();
        }
    }
}
