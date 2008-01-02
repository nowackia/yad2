using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    public class NumericMessage : Message
    {
        private int number;

        public NumericMessage()
            : base(MessageType.Numeric)
        { }

        public NumericMessage(MessageType msgType)
            : base(msgType)
        { }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {
            base.Deserialize(reader);
            number = reader.ReadInt32();
        }

        public override void Serialize(System.IO.BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(number);
        }
    }
}
