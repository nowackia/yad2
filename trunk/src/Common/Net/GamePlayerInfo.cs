using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Common {
    public class GamePlayerInfo : PlayerInfo {
        private short id;
        private string name;

        public short Id {
            get { return id; }
            set { id = value; }
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }
    }
}
