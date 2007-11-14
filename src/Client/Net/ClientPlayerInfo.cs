using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Net
{
    static class ClientPlayerInfo
    {
        private static string login = string.Empty;
        private static string gameName = string.Empty;

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
