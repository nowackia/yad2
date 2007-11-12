using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using System.Collections;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Server {
     
    class GameManager {

        private const short MaxGameNumber = 20;
        Dictionary<short, Player> _players = null;
        Dictionary<string, ServerGameInfo> _games = null;
        IMessageSender _sender = null;

        public GameManager(IMessageSender sender) {
            _players = new Dictionary<short, Player>();
            _games = new Dictionary<string, ServerGameInfo>();
            _sender = sender;
        }

        public void ProcessPlayerEntry(Player player) {
            if (!_players.ContainsKey(player.Id)) {
                lock (((ICollection)_players).SyncRoot) {
                    _players.Add(player.Id, player);
                }
                SendGameListMessage(player.Id);
            }
            else {
                RemoveFromGameJoin(player.Id);
            }
        }

        public void RemoveFromGameJoin(short id) {
            lock (((ICollection)_games).SyncRoot) {
                foreach (ServerGameInfo sgi in _games.Values)
                    if (sgi.IsInside(id)) {
                        sgi.RemovePlayer(id);
                        break;
                    }
            }
        }

        public void SendGameListMessage(int recipient) {
            List<GameInfo> list = new List<GameInfo>();
            lock (((ICollection)_players).SyncRoot) {
                foreach (ServerGameInfo sgi in _games.Values)
                    if (!sgi.IsPrivate)
                        list.Add(sgi.GetGameInfo());
            }

            GamesMessage msg = MessageFactory.Create(MessageType.GamesMessage) as GamesMessage;
            msg.ListGameInfo = list;
            msg.Operation = (byte)MessageOperation.List;
            _sender.MessagePost(msg, recipient);
        }

        public ResultType CreateGame(GameInfo gi) {
            if (GameNumber == MaxGameNumber)
                return ResultType.MaxServerGameError;
            ResultType result;
            lock (((ICollection)_games).SyncRoot) {
                result = IsValid(gi);
                if (result != ResultType.Successful)
                    return result;
                ServerGameInfo sgi = new ServerGameInfo(gi, _sender);
                _games.Add(gi.Name, sgi);
            }
            return result;
        }


        public ResultType JoinGame(string name, Player player) {
            lock (((ICollection)_games).SyncRoot) {
                ResultType result = IsJoinPossible(name);
                if (ResultType.Successful == result) {
                    _games[name].AddPlayer(player);
                }
                return result;
            }
         
        }



        private void SendMessage(Message msg, short id) {
            _sender.MessagePost(msg, id);
        }
        private short GameNumber {
            get {
                return (short)_games.Count;
            }
        }
        private ResultType IsJoinPossible(string name) {
            //czy nazwa istnieje
            if (!_games.ContainsKey(name))
                return ResultType.GameNotExists;
           //czy gra jest pelna
            if (!_games[name].IsAddPosible())
                return ResultType.GameFull;
            //czy gra sie zaczela - jak bedzie wiadomo jak rozpoczac gre :)
            return ResultType.Successful;
           
        }
        public ResultType IsValid(GameInfo gi) {
            if (!IsNameValid(gi.Name))
                return ResultType.NameExistsError;
            if (!IsPlayerNoValid(gi.MaxPlayerNumber))
                return ResultType.InvalidPlayerNoError;
            if (!IsMapValid(gi.MapId))
                return ResultType.InvalidMapIdNoError;
            return ResultType.Successful;
            
        }

        public bool IsNameValid(string gameName) {
            if (_games.ContainsKey(gameName))
                return false;
            return true;
        }

        public bool IsPlayerNoValid(int playerNo) {
            return true;
        }

        public bool IsMapValid(int mapID) {
            return true;
        }


    }
}
