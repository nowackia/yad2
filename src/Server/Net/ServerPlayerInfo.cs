using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Server {
    class ServerPlayerInfo : PlayerInfo {
        Player _player;

        public ServerPlayerInfo(Player player) {
            _player = player;
        }

        public short Id {
            get {
                return _player.Id;
            }
        }
    }
}
