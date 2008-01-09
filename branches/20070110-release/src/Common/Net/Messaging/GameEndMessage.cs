using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common {
    public class GameEndMessage : Message {
        bool _hasWon;

        public bool HasWon {
            get { return _hasWon; }
            set { _hasWon = value; }
        }
   
        public GameEndMessage()
            : base(MessageType.EndGame) {
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            _hasWon = reader.ReadBoolean();
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(_hasWon);
        }
    }
}
