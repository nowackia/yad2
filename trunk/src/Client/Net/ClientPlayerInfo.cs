using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Client
{
    static class ClientPlayerInfo
    {
        private static short playerId = -1;
        private static string login = string.Empty;
        private static string gameName = string.Empty;

        public static short PlayerId
        {
            get
            { return playerId; }
            set
            { playerId = value; }
        }

        public static string Login
        {
            get
            { return login; }
            set
            { login = value; }
        }

        public static string ChatPrefix
        {
            get
            { return "[" + Login + "] : ";  }
        }

        public static string GameName
        {
            get
            { return gameName; }
            set
            { gameName = value; }
        }
    }
}
