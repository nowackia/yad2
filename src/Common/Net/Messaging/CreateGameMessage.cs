using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using Yad.Net.Common;

namespace Yad.Net.Messaging {
    class CreateGameMessage : Message {
        GameInfo _gi;

        public CreateGameMessage(GameInfo gi) : base(MessageType.CreateGame){
            _gi = gi;
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            this.WriteString(_gi.Name, writer);
            writer.Write(_gi.MapId);
            writer.Write(_gi.MaxPlayerNumber);
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            if (null == _gi)
                _gi = new GameInfo();
            _gi.Name = this.ReadString(reader);
            _gi.MapId = reader.ReadInt16();
            _gi.MaxPlayerNumber = reader.ReadInt16();
        }
    }
}
