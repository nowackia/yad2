using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;

namespace Yad.Net.Messaging.Common
{
    public class MoveMessage : GameMessage
    {
        private int idUnit;
        private Position position;

        public MoveMessage()
            : base(MessageType.Move)
        { }

        public Position Path
        {
            get { return position; }
            set { position = value; }
        }

        public int IdUnit
        {
            get { return idUnit; }
            set { idUnit = value; }
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            idUnit = reader.ReadInt32();
            position.Deserialize(reader);

            
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(idUnit);
            position.Serialize(writer);
        }
    }
}
