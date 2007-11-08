using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using System.Collections;

namespace Yad.Net.Server {
    class GameManager {
        
        List<Player> _players;
        Dictionary<string, GameInfo> _games;

        public void AddPlayer(Player player) {
            lock (((ICollection)_players).SyncRoot) {
                _players.Add(player);
            }
        }



    }
}
