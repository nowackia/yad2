using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net
{
    class Connection
    {
        private static Connection instance;

        static Connection()
        {
            instance = new Connection();
        }

        public static Connection Instance
        {
            get
            { return instance; }
        }
    }
}
