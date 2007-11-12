using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using System.Collections;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Server {
    public class ServerGameInfo : GameRoom {

        #region Private Members

        private bool _isPrivate;
        private GameInfo _gameInfo;

        #endregion

        #region Properties

        /*internal Dictionary<short, ServerPlayerInfo> Players {
            get { return _players; }
            set { _players = value; }
        }*/

        public string Name {
            get { return _gameInfo.Name; }
            set { _gameInfo.Name = value; }
        }

        public short MapId {
            get { return _gameInfo.MapId; }
            set { _gameInfo.MapId = value; }
        }

        public short MaxPlayerNumber {
            get { return _gameInfo.MaxPlayerNumber; }
            set { _gameInfo.MaxPlayerNumber = value; }
        }
        public bool IsPrivate {
            get { return _isPrivate; }
            set { _isPrivate = value; }
        }

        #endregion 

        #region Constructors

        public ServerGameInfo(GameInfo gi, IMessageSender sender) : base(sender) {
            _gameInfo = new GameInfo();
            _gameInfo.Name = gi.Name;
            _gameInfo.MapId = gi.MapId;
            _gameInfo.MaxPlayerNumber = gi.MaxPlayerNumber;
            
        }

        #endregion 

        #region Public Methods


        private PlayersMessage CreatePlayerMessage(MessageOperation operation, IPlayerID playerID) {
            
            List<GamePlayerInfo> list = new List<GamePlayerInfo>();
            PlayersMessage msg = (PlayersMessage)MessageFactory.Create(MessageType.Players);
            msg.Operation = (byte)operation;
            switch (operation) {
                case MessageOperation.List:
                    foreach (IPlayerID pid in _players.Values) {
                        if (pid.GetID() != playerID.GetID()) {
                            ServerPlayerInfo spi = pid as ServerPlayerInfo;
                            list.Add(spi.GetGamePlayerInfo());
                            msg.PlayerList = list;
                        }
                    }
                    break;
                case MessageOperation.Add:
                    list.Add((GamePlayerInfo)playerID);
                    break;
                case MessageOperation.Remove:
                    list.Add((GamePlayerInfo)playerID);
                    break;
                case MessageOperation.Modify:
                    list.Add((GamePlayerInfo)playerID);
                    break;
            }
            return msg;
        }


        public void RemovePlayer(short id) {
            lock (((ICollection)_players).SyncRoot) {
                _players.Remove(id);
            }
        }

        public bool IsInside(short id) {
            lock (((ICollection)_players).SyncRoot) {
                return _players.ContainsKey(id);
            }
        }

        private void RepairTeams() {
            if (_players.Count < 2)
                return;
            int id = ((ServerPlayerInfo)_players.Values.GetEnumerator().Current).TeamID;
            bool change = true;
            foreach (ServerPlayerInfo spi in _players.Values) {
                if (spi.TeamID != id) {
                    change = false;
                    break;
                }
            }
            if (change)
                ((ServerPlayerInfo)_players.Values.GetEnumerator().Current).TeamID = GetTeamForPlayer();
                
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



        protected override Message CreateAddMessage(IPlayerID playerID) {
            return CreatePlayerMessage(MessageOperation.Add, playerID);
        }

        protected override Message CreateListMessage(IPlayerID playerID) {
            return CreatePlayerMessage(MessageOperation.List, playerID);
        }

        protected override Message CreateRemoveMessage(IPlayerID playerID) {
            return CreatePlayerMessage(MessageOperation.Remove, playerID);
        }

        protected override IPlayerID TransformInitialy(IPlayerID player) {
            Player p = player as Player;
            ServerPlayerInfo svp = new ServerPlayerInfo(p.Id, p.Login);
            short teamId = -1;
            lock (((ICollection)_players).SyncRoot) {
                teamId = GetTeamForPlayer();
            }
            svp.TeamID = teamId;
            return svp;
        }
    }
}
