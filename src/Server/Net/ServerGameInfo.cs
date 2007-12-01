using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using System.Collections;
using Yad.Net.Messaging.Common;
using System.Drawing;

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

        public string MapName {
            get { return _gameInfo.MapName; }
            set { _gameInfo.MapName = value; }
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
            _gameInfo.MapName = gi.MapName;
            _gameInfo.MaxPlayerNumber = gi.MaxPlayerNumber;
            _gameInfo.GameType = gi.GameType;
            if (gi.GameType == GameType.Private)
                _isPrivate = true;
            else
                _isPrivate = false;
            //_gameInfo.Description = gi.Description;
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
            PlayersMessage pim = CreatePlayerMessage(MessageOperation.Modify, (ServerPlayerInfo)_players[player.GetID()]);
            SendMessage(pim, player.GetID());
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
                if (IsColorModificationValid(pi.Color, id)) {
                    spi.Color = pi.Color;
                }
                pmsg = CreatePlayerMessage(MessageOperation.Modify, spi);
            }
            BroadcastExcl(pmsg, -1);
        }

        private bool IsColorModificationValid(Color color, short id) {
            if (_players.Count == 1)
                return true;
            foreach (ServerPlayerInfo spi in _players.Values) {
                if (spi.Id != id)
                    if (spi.Color == color)
                        return false;
            }
            return true;
        }


        public bool IsAddPosible() {
            lock (((ICollection)_players).SyncRoot) {
                return _players.Count < MaxPlayerNumber;
            }
        }

        public GameInfo GetGameInfo() {
            GameInfo gi = new GameInfo();
            gi.Name = this.Name;
            gi.MapName = this.MapName;
            gi.MaxPlayerNumber = this.MaxPlayerNumber;
            gi.GameType = this._gameInfo.GameType;
            //gi.Description = this._gameInfo.Description;
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
            if (_players.Count != this._gameInfo.MaxPlayerNumber)
                return false;
            foreach (IPlayerID spi in _players.Values)
                    if (!((ServerPlayerInfo)spi).StartedClicked)
                        return false;
            return true;
        }

        private void CancelGameStart() {
            foreach (IPlayerID spi in _players.Values)
                if (((ServerPlayerInfo)spi).StartedClicked) {
                    ((ServerPlayerInfo)spi).StartedClicked = false;
                    SendMessage(Utils.CreateResultMessage(ResponseType.StartGame, ResultType.Unsuccesful), ((ServerPlayerInfo)spi).Id);
                }
        }

        private bool RepairTeams() {
            if (_players.Count < 2)
                return false;
            ServerPlayerInfo firstSpi = null;
            foreach (IPlayerID pid in _players.Values)
            {
                firstSpi = (ServerPlayerInfo)pid;
                break;
            }
            int id = firstSpi.TeamID;
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
            for (short i = 0; i < this.GetGameInfo().MaxPlayerNumber; ++i)
                if (IsTeamIDValid(i+1))
                    return (short)(i+1);
            return -1; //This should never happen!
        }

        private Color GetColorForPlayer() {
            for (short i = 0; i < this.GetGameInfo().MaxPlayerNumber; ++i) {
                if (IsColorValid(ServerPlayerInfo.StartColors[i]))
                    return ServerPlayerInfo.StartColors[i];
            }
            return Color.Pink; //This should never happen!
        }

        private bool IsColorValid(Color color) {
            foreach (ServerPlayerInfo spi in _players.Values)
                if (spi.Color == color)
                    return false;
            return true;
        }

        private bool IsTeamModificationValid(short teamid, short playerid) {
            if (_players.Count == 1)
                return true;
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
            short teamId = 1;
            Color baseColor = ServerPlayerInfo.StartColors[0];
            if (_players.Count > 0) {
                lock (((ICollection)_players).SyncRoot) {
                    teamId = GetTeamForPlayer();
                    baseColor = GetColorForPlayer();
                }
            }
            svp.TeamID = teamId;
            svp.Color = baseColor;
            return svp;
        }

        #endregion
    }
}
