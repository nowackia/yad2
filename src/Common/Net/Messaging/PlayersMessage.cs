using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using Yad.Engine.Common;

namespace Yad.Net.Messaging.Common {
    public class PlayersMessage : Message {

        byte _operation;
        List<GamePlayerInfo> _playerList;

        public byte Operation {
            get { return _operation; }
            set { _operation = value; }
        }
        
        public List<GamePlayerInfo> PlayerList {
            get { return _playerList; }
            set { _playerList = value; }
        }

        public PlayersMessage() : base(MessageType.Players) { }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(_operation);
            writer.Write(_playerList.Count);
            foreach (GamePlayerInfo gpi in _playerList) {
                writer.Write(gpi.Id);
                WriteString(gpi.Name, writer);
                writer.Write(gpi.TeamID);
                writer.Write((byte)gpi.House);
            }
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            _operation = reader.ReadByte();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; ++i) {
                GamePlayerInfo gpi = new GamePlayerInfo();
                gpi.Id = reader.ReadInt16();
                gpi.Name = ReadString(reader);
                gpi.TeamID = reader.ReadInt16();
                gpi.House = (HouseType)reader.ReadByte();
            }
           
        }


    }
}
