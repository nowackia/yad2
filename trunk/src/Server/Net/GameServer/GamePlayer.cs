using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Server;

namespace Yad.Net.GameServer {
    class GamePlayer {
        private uint _turnNo = 0;

        public uint TurnNo {
            get { return _turnNo; }
            set { _turnNo = value; }
        }


    }
}
