using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Messaging.Common {
    public class GamesMessage : Message {

        private byte _operation;
        private List<GameInfo> _listGameInfo;

        public byte Operation {
            get { return _operation; }
            set { _operation = value; }
        }
        

        public List<GameInfo> ListGameInfo {
            get { return _listGameInfo; }
            set { _listGameInfo = value; }
        }

        public GamesMessage() 
            : base(MessageType.GamesMessage) {
            _operation = 0;
            _listGameInfo = new List<GameInfo>();
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(_operation);
            writer.Write(_listGameInfo.Count);
            foreach (GameInfo gi in _listGameInfo) {
                WriteString(gi.Name, writer);
                writer.Write(gi.MapId);
                writer.Write(gi.MaxPlayerNumber);
            }
                
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            _operation = reader.ReadByte();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; ++i) {
                GameInfo gi = new GameInfo();
                gi.Name = ReadString(reader);
                gi.MapId = reader.ReadInt16();
                gi.MaxPlayerNumber = reader.ReadInt16();
                _listGameInfo.Add(gi);
            }
        }



    }
}
