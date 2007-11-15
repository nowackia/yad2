using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Client
{
    static class ClientPlayerInfo
    {
        private static short playerId = -1;
        private static string login = string.Empty;
        private static GameInfo gameInfo = null;
        private static PlayerInfo playerInfo = new PlayerInfo();

        public static short PlayerId
        {
            get
            { return playerInfo.Id; }
            set
            { playerInfo.Id = value; }
        }

        public static string Login
        {
            get
            { return playerInfo.Name; }
            set
            { playerInfo.Name = value; }
        }

        public static string ChatPrefix
        {
            get
            { return "[" + Login + "] : ";  }
        }

        public static GameInfo GameInfo
        {
            get
            { return gameInfo; }
            set
            { gameInfo = value; }
        }

        public static PlayerInfo PlayerInfo
        {
            get
            { return playerInfo; }
            set
            { playerInfo = value; }
        }
    }
}
