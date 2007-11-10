using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using System.Collections;

namespace Yad.Net.Server {
    class ServerGameInfo : GameInfo {

        #region Private Members

        Dictionary<short, ServerPlayerInfo> _players;
        private bool _isPrivate;
      

        #endregion

        #region Properties

        internal Dictionary<short, ServerPlayerInfo> Players {
            get { return _players; }
            set { _players = value; }
        }

        public bool IsPrivate {
            get { return _isPrivate; }
            set { _isPrivate = value; }
        }

        #endregion 

        #region Constructors

        public ServerGameInfo(GameInfo gi) {
            _players = new Dictionary<short, ServerPlayerInfo>();
            this.Name = gi.Name;
            this.MapId = gi.MapId;
            this.MaxPlayerNumber = gi.MaxPlayerNumber;
            
        }

        #endregion 

        #region Public Methods

        public void AddPlayer(Player player) {
            ServerPlayerInfo svp = new ServerPlayerInfo(player);
            lock (((ICollection)_players).SyncRoot) {
                short teamId = GetTeamForPlayer();
                svp.TeamID = teamId;
                _players.Add(player.Id, svp);
            }
        }


        public void RemovePlayer(short id) {
            lock (((ICollection)_players).SyncRoot) {
                _players.Remove(id);
            }
        }

        private void RepairTeams() {
            if (_players.Count < 2)
                return;
            int id = _players.Values.GetEnumerator().Current.TeamID;
            bool change = true;
            foreach (ServerPlayerInfo spi in _players.Values) {
                if (spi.TeamID != id) {
                    change = false;
                    break;
                }
            }
            if (change)
                _players.Values.GetEnumerator().Current.TeamID = GetTeamForPlayer();
                
        }
        private short GetTeamForPlayer() {
            for (short i = 0; i < PlayerInfo.MaxTeamNo; ++i)
                if (IsTeamIDValid(i))
                    return i;
            return -1; //This should never happen!
        }
        

        private bool IsTeamIDValid(int id) {
            foreach (ServerPlayerInfo spi in _players.Values)
                if (spi.TeamID != id)
                    return true;
            return false;
        }

        public bool IsAddPosible() {
            lock (((ICollection)_players).SyncRoot) {
                return _players.Count < MaxPlayerNumber;
            }
        }

        public GameInfo GetGameInfo() {
            GameInfo gi = new GameInfo();
            gi.Name = this.Name;
            gi.MapId = this.MapId;
            gi.MaxPlayerNumber = this.MaxPlayerNumber;
            return gi;
        }


        #endregion


    }
}
