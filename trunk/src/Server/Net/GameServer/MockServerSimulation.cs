using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.GameServer.Server {
    public class MockServerSimulation : IServerSimulation {
        
        private Dictionary<short, GamePlayer> _gamePlayers;


        public MockServerSimulation() {
            _gamePlayers = new Dictionary<short, GamePlayer>();
        }

        public void AddPlayer(short id) {
            _gamePlayers.Add(id, new GamePlayer());
        }

        #region IServerSimulation Members

        public uint GetPlayerTurn(short id) {
            return _gamePlayers[id].TurnNo;
        }

        public uint GetMinTurn() {
            uint min = 0;
            foreach (GamePlayer gp in _gamePlayers.Values) {
                min = Math.Min(min, gp.TurnNo);
            }
            return min;
        }

        public void IncPlayerTurn(short id) {
            _gamePlayers[id].TurnNo++;
        }

        #endregion
    }
}
