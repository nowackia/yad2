using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Messaging.Common
{
    public class PlayerInfoMessage : Message
    {
        private PlayerData playerData;

        public PlayerInfoMessage()
            : base(MessageType.PlayerInfo)
        { }

        public PlayerInfoMessage(MessageType msgType)
            : base(msgType)
        { }

        public PlayerData PlayerData
        {
            get { return playerData; }
            set { playerData = value; }
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            if (playerData == null)
                writer.Write(false);
            else {
                writer.Write(true);
                writer.Write(playerData.Id);
                base.WriteString(playerData.Login, writer);
                writer.Write(playerData.LossNo);
                writer.Write(playerData.WinNo);
            }
            
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            bool isNull = !reader.ReadBoolean();
            if (isNull) {
                playerData = null;
                return;
            }
            else {
                playerData = new PlayerData();
                playerData.Id = reader.ReadInt16();
                playerData.Login = base.ReadString(reader);
                playerData.LossNo = reader.ReadInt32();
                playerData.WinNo = reader.ReadInt32();
            }
        }
    }
}
