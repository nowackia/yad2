using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Messaging.Common
{
    class PlayerInfoMessage : Message
    {
        private PlayerData playerData;

        public PlayerData PlayerData
        {
            get { return playerData; }
            set { playerData = value; }
        }
    }
}
