using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common {
    public class GameNumericMessage : GameMessage {
        int _number;

        public int Number {
            get { return _number; }
            set { _number = value; }
        }

        public GameNumericMessage(MessageType type)
            : base(type) {
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(_number);
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            _number = reader.ReadInt32();
        }

    }
}
