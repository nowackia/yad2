using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Server;
using Yad.Net.Common;

namespace Yad.Net.GameServer {
    public class GamePlayer : PlayerData {
        private int _turnNo = 0;
        bool _isWaiting = false;
        bool _hasEnded = false;
        bool _hasWon = false;

        public bool HasWon {
            get { return _hasWon; }
            set { _hasWon = value; }
        }

        public bool HasEnded {
            get { return _hasEnded; }
            set { _hasEnded = value; }
        }

        public bool IsWaiting {
            get { return _isWaiting; }
            set { _isWaiting = value; }
        }

        public int TurnNo {
            get { return _turnNo; }
            set { _turnNo = value; }
        }

        public GamePlayer(PlayerData pd) {
            this.Id = pd.Id;
            this.Login = pd.Login;
            this.LossNo = pd.LossNo;
            this.WinNo = pd.WinNo;
        }


    }
}
