using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Messaging.Common
{
    public class PlayerInfoMessage : Message
    {
        private PlayerData playerData;

        public PlayerInfoMessage(MessageType msgType)
            : base(msgType)
        { }

        public PlayerData PlayerData
        {
            get { return playerData; }
            set { playerData = value; }
        }
    }
}
