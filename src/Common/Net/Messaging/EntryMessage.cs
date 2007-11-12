using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common {
    public class EntryMessage : Message {
        byte _serverRoom;

        public EntryMessage() : base(MessageType.Entry) { }
        public byte ServerRoom {
            get { return _serverRoom; }
            set { _serverRoom = value; }
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(_serverRoom);
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            _serverRoom = reader.ReadByte();
        }
    }
}
