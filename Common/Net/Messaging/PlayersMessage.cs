using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Yad.Net.Common;
using Yad.Engine.Common;

namespace Yad.Net.Messaging.Common {
    public class PlayersMessage : Message {

        byte _operation;
        List<PlayerInfo> _playerList;

        public byte Operation {
            get { return _operation; }
            set { _operation = value; }
        }
        
        public List<PlayerInfo> PlayerList {
            get { return _playerList; }
            set { _playerList = value; }
        }

        public PlayersMessage() : base(MessageType.Players) {
            _playerList = new List<PlayerInfo>();
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(_operation);
            writer.Write(_playerList.Count);
            foreach (PlayerInfo pi in _playerList) {
                writer.Write(pi.Id);
                WriteString(pi.Name, writer);
                writer.Write(pi.TeamID);
                writer.Write(pi.House);
                writer.Write(pi.Color.R);
                writer.Write(pi.Color.G);
                writer.Write(pi.Color.B);
            }
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            _operation = reader.ReadByte();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; ++i) {
                PlayerInfo pi = new PlayerInfo();
                pi.Id = reader.ReadInt16();
                pi.Name = ReadString(reader);
                pi.TeamID = reader.ReadInt16();
                pi.House = reader.ReadInt16();
                pi.Color = Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                _playerList.Add(pi);
            }
           
        }


    }
}
