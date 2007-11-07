using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Server;
using Yad.Net.General.Messaging;
using System.Collections;
using Yad.Net.General;

namespace Server.Net.General.Server {
    class Chat {
        List<Player> _players;
        IMessageSender _sender;
        public Chat(IMessageSender sender) {
            _players = new List<Player>();
            _sender = sender;
        }
        public void AddPlayer(Player player) {
            lock (((ICollection)_players).SyncRoot) {
                _players.Add(player);
            }
            Message m = MessageFactory.Create(MessageType.ChatUsers);
            
            _sender.PostMessage(m, player.Id);
            BroadcastExcl(new NewChatUserMessage(player.Login, player.Id), player.Id);
            
        }

        public void Remove(Player player) {
            _players.Remove(player);
            Message m = MessageFactory.Create(MessageType.DeleteChatUser);
            ((NumericMessage)m).Number = player.Id;
            foreach (Player p in _players) {
                p.SendMessage(m);
            }
        }

        private void BroadcastExcl(Message msg, int id)
        {
            lock (((ICollection)_players).SyncRoot)
                foreach (Player p in _players)
                    if (p.Id != id)
                        _sender.PostMessage(msg, id);
        }

        public void PostMessage(TextMessage msg) {
            foreach (Player p in _players) {
                if (p.Id != msg.UserId) {
                    p.SendMessage(msg);
                }
                 
            }
        }


    }
}
