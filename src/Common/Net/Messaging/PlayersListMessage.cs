using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using Yad.Engine.Common;

namespace Yad.Net.Messaging.Common
{
    public class PlayersListMessage : Message
    {
        List<PlayerInfo> players = null;

        public PlayersListMessage(MessageType msgType)
            : base(msgType)
        { }

        public List<PlayerInfo> Players
        {
            get { return players; }
            set { players = value; }
        }

        public override void Serialize(System.IO.BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(players.Count);
            foreach (PlayerInfo pi in players)
            {
                writer.Write((byte)pi.House);
                writer.Write(pi.TeamID);
            }
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {
            base.Deserialize(reader);
            players = new List<PlayerInfo>();

            int number = reader.ReadInt32();
            for (int i = 0; i < number; ++i)
            {
                PlayerInfo pi = new PlayerInfo();
                pi.House = (HouseType)reader.ReadByte();
                pi.TeamID = reader.ReadInt32();
                players.Add(pi);

            }
        }
    }
}
