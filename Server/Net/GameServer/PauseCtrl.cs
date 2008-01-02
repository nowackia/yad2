using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.GameServer.Server {
    class PauseCtrl {
        int _pauseTurnNo = Int32.MaxValue;
        List<short> _pausedPlayers = new List<short>();
        bool _isPaused;
        short _pausedId;

        public int PauseTurn {
            get { return _pauseTurnNo; }
        }
        
        public bool IsPaused {
            get { return _isPaused; }
        }

        public short PausedId {
            get { return _pausedId; }
        }

        public void SetPause(int turn, short id) {
            _pauseTurnNo = Math.Min(turn, _pauseTurnNo);
            _isPaused = true;
            _pausedId = id;
        }

        public void AddPausedPlayer(short player) {
            _pausedPlayers.Add(player);
        }

        public List<short> ResumeGame() {
            _isPaused = false;
            _pauseTurnNo = Int32.MaxValue;
            List<short> playersToResume = new List<short>();
            foreach(short id in _pausedPlayers)
                playersToResume.Add(id);
            _pausedPlayers.Clear();
            return playersToResume;
        }
    }
}
