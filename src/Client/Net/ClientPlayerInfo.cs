using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Net
{
    static class ClientPlayerInfo
    {
        private static string login = string.Empty;

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
    }
}
