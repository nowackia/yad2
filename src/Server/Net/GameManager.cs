using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using System.Collections;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Server {
    class GameManager {
        
        List<Player> _players;
        Dictionary<string, GameInfo> _games;
        IMessageSender _sender;

        public void AddPlayer(Player player) {
            lock (((ICollection)_players).SyncRoot) {
                _players.Add(player);
            }
            SendGameListMessage(player.Id);
        }

        public void SendGameListMessage(int recipient) {
            List<GameInfo> list = new List<GameInfo>();
            lock (((ICollection)_players).SyncRoot) {
                foreach (GameInfo gi in _games.Values)
                    if (!gi.IsPrivate)
                        list.Add((GameInfo)gi.Clone());
            }

            GamesListMessage msg = MessageFactory.Create(MessageType.GamesList) as GamesListMessage;
            msg.Games = list;
            _sender.MessagePost(msg, recipient);
        }



    }
}
