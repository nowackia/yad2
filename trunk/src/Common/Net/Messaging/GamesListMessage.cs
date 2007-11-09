using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using Yad.Net.Common;

namespace Yad.Net.Messaging.Common {
    public class GamesListMessage : Message {
        
        List<GameInfo> _games = null;

        public GamesListMessage()
            : base(MessageType.GamesList) {

        }

        public List<GameInfo> Games {
            get { return _games; }
            set { _games = value; }
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(_games.Count);
            foreach (GameInfo gi in _games) {
                writer.Write(gi.MapId);
                this.WriteString(gi.Name, writer);
                writer.Write(gi.MaxPlayerNumber);
            }
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            _games = new List<GameInfo>();
            int number = reader.ReadInt32();
            for (int i = 0; i < number; ++i) {
                GameInfo gi = new GameInfo();
                gi.MapId = reader.ReadInt16();
                gi.Name = this.ReadString(reader);
                gi.MaxPlayerNumber = reader.ReadInt16();
                _games.Add(gi);
                
            }
        }
    }
}
