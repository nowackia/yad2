using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using System.Collections;
using Yad.Net.Common;

namespace Yad.Net.Server {
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
            if (m != null)
            {
                _sender.MessagePost(m, player.Id);
                BroadcastExcl(new NewChatUserMessage(player.Login, player.Id), player.Id);
            }
            SendMessage(CreateChatUserMessage(), player.Id);
            
        }

        public ChatUsersMessage CreateChatUserMessage(){
            ChatUsersMessage msg = MessageFactory.Create(MessageType.ChatUsers) as ChatUsersMessage;
            return msg;
        }

        public ChatUser PlayerToChatUser(Player p)
        {
            return null;
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
                        _sender.MessagePost(msg, id);
        }

        public void PostMessage(TextMessage msg) {
            foreach (Player p in _players) {
                if (p.Id != msg.PlayerId) {
                    p.SendMessage(msg);
                }
            }
        }

        public void SendMessage(Message msg, short id) {
            _sender.MessagePost(msg, id);
        }


    }
}
