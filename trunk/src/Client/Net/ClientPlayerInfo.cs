using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Client
{
    static class ClientPlayerInfo
    {
        private static GameInfo gameInfo = null;
        private static PlayerInfo playerInfo = new PlayerInfo();
        private static Players enemies = new Players();

        public static short SenderId
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
            { 
                gameInfo = value;
            }
        }

        public static Players Enemies
        {
            get { return enemies; }
        }
    }
}
