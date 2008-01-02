using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.GameServer {
    public struct PlayerTurn {
        int _turnNo;

        public int TurnNo {
            get { return _turnNo; }
            set { _turnNo = value; }
        }

        short _playerID;

        public short PlayerID {
            get { return _playerID; }
            set { _playerID = value; }
        }

        public PlayerTurn(short id) {
            _playerID = id;
            _turnNo = 1;
        }

    }

    public class PlayerTurnComparer : IComparer<PlayerTurn> {
        #region IComparer<PlayerTurn> Members

        public int Compare(PlayerTurn x, PlayerTurn y) {
            if (x.TurnNo < y.TurnNo)
                return 1;
            if (x.TurnNo == y.TurnNo)
                return 0;
            else
                return -1;

        }

        #endregion
    }
}
