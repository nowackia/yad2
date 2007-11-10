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

        public void AddPlayer(Player player) {
            lock (((ICollection)_players).SyncRoot) {
                _players.Add(player.Id, player);
            }
            SendGameListMessage(player.Id);
        }

        public void SendGameListMessage(int recipient) {
            List<GameInfo> list = new List<GameInfo>();
            lock (((ICollection)_players).SyncRoot) {
                foreach (ServerGameInfo sgi in _games.Values)
                    if (!sgi.IsPrivate)
                        list.Add(sgi.GetGameInfo());
            }

            GamesListMessage msg = MessageFactory.Create(MessageType.GamesList) as GamesListMessage;
            msg.Games = list;
            _sender.MessagePost(msg, recipient);
        }

        public CreateGameResult CreateGame(GameInfo gi) {
            if (GameNumber == MaxGameNumber)
                return CreateGameResult.MaxServerGameError;
            CreateGameResult result;
            lock (((ICollection)_games).SyncRoot) {
                result = IsValid(gi);
                if (result != CreateGameResult.CreateSuccessful)
                    return result;
                ServerGameInfo sgi = new ServerGameInfo(gi);
                _games.Add(gi.Name, sgi);
            }
            return result;
            

        }

        private short GameNumber {
            get {
                return (short)_games.Count;
            }
        }

        public CreateGameResult IsValid(GameInfo gi) {
            if (!IsNameValid(gi.Name))
                return CreateGameResult.NameExistsError;
            if (!IsPlayerNoValid(gi.MaxPlayerNumber))
                return CreateGameResult.InvalidPlayerNoError;
            if (!IsMapValid(gi.MapId))
                return CreateGameResult.InvalidMapIdNoError;
            return CreateGameResult.CreateSuccessful;
            
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
