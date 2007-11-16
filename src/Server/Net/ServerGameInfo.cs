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

        public short PlayerNo {
            get { return (short)_players.Count; }
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

        public override void RemovePlayer(IPlayerID playerID) {
            base.RemovePlayer(playerID);
            lock (((ICollection)_players).SyncRoot) {
                CancelGameStart();
                if (RepairTeams()) {
                    PlayersMessage msg = CreatePlayerMessage(MessageOperation.Modify, _players.GetEnumerator().Current.Value);
                    BroadcastExcl(msg, -1);
                }

            }
        }

        public override void AddPlayer(IPlayerID player) {
            base.AddPlayer(player);
            CancelGameStart();
        }

        public void ModifyPlayer(short id, PlayerInfo pi) {
            PlayersMessage pmsg;
            lock (((ICollection)_players).SyncRoot) {
                CancelGameStart();
                ServerPlayerInfo spi = _players[id] as ServerPlayerInfo;
                spi.House = pi.House;
                if (IsTeamModificationValid(pi.TeamID, id)) {
                    spi.TeamID = pi.TeamID;
                }
                pmsg = CreatePlayerMessage(MessageOperation.Modify, spi);
            }
            BroadcastExcl(pmsg, -1);
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

        public bool StartClick(short id) {
            lock (((ICollection)_players).SyncRoot) {
                ((ServerPlayerInfo)_players[id]).StartedClicked = true;
                if(IsGameStart()) {
                    BroadcastExcl(Utils.CreateResultMessage(ResponseType.StartGame, ResultType.Successful),-1);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Private Methods

        private PlayersMessage CreatePlayerMessage(MessageOperation operation, IPlayerID playerID) {
            
            List<PlayerInfo> list = new List<PlayerInfo>();
            PlayersMessage msg = (PlayersMessage)MessageFactory.Create(MessageType.Players);
            msg.Operation = (byte)operation;
            switch (operation) {
                case MessageOperation.List:
                    foreach (IPlayerID pid in _players.Values) {
                        if (pid.GetID() != playerID.GetID()) {
                            ServerPlayerInfo spi = pid as ServerPlayerInfo;
                            list.Add(spi.GePlayerInfo());
                        }
                    }
                    break;
                case MessageOperation.Add:
                    list.Add(((ServerPlayerInfo)playerID).GePlayerInfo());
                    break;
                case MessageOperation.Remove:
                    list.Add(((ServerPlayerInfo)playerID).GePlayerInfo());
                    break;
                case MessageOperation.Modify:
                    list.Add(((ServerPlayerInfo)playerID).GePlayerInfo());
                    break;
            }
            msg.PlayerList = list;
            return msg;
        }

        public bool IsInside(short id) {
            lock (((ICollection)_players).SyncRoot) {
                return _players.ContainsKey(id);
            }
        }

        private bool IsGameStart() {
            foreach (IPlayerID spi in _players.Values)
                lock (spi) {
                    if (!((ServerPlayerInfo)spi).StartedClicked)
                        return false;
                }
            return true;
        }

        private void CancelGameStart() {
            foreach (IPlayerID spi in _players.Values)
                if (((ServerPlayerInfo)spi).StartedClicked) {
                    lock (spi) {
                        ((ServerPlayerInfo)spi).StartedClicked = false;
                        SendMessage(Utils.CreateResultMessage(ResponseType.StartGame, ResultType.Unsuccesful), ((ServerPlayerInfo)spi).Id);
                    }  
                }
        }

        private bool RepairTeams() {
            if (_players.Count < 2)
                return false;
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
            return change;
                
        }
        private short GetTeamForPlayer() {
            for (short i = 0; i < PlayerInfo.MaxTeamNo; ++i)
                if (IsTeamIDValid(i))
                    return i;
            return -1; //This should never happen!
        }

        private bool IsTeamModificationValid(short teamid, short playerid) {
            foreach (ServerPlayerInfo spi in _players.Values) {
                    if (spi.Id != playerid)
                        if (spi.TeamID != teamid)
                            return true;
            }
            return false;
        }
        private bool IsTeamIDValid(int id) {
            foreach (ServerPlayerInfo spi in _players.Values)
                if (spi.TeamID != id)
                    return true;
            return false;
        }

        #endregion

        #region Protected methods

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
            ServerPlayerInfo svp = new ServerPlayerInfo(p);
            short teamId = -1;
            lock (((ICollection)_players).SyncRoot) {
                teamId = GetTeamForPlayer();
            }
            svp.TeamID = teamId;
            return svp;
        }

        #endregion
    }
}
