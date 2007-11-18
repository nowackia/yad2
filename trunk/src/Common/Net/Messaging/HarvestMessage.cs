using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;

namespace Yad.Net.Messaging.Common
{
    public class HarvestMessage : GameMessage
    {
        private Position position;
        private int idUnit;

        public HarvestMessage()
            : base(MessageType.Harvest)
        { }

        public Position Position
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
