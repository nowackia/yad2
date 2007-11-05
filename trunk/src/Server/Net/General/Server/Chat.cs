using System;
using System.Collections.Generic;
using System.Text;
using Server.ServerManagement;
using Yad.Net.General.Messaging;

namespace Server.Net.General.Server {
    class Chat {
        List<Player> _players;

        public void AddPlayer(Player player) {
            lock (_players) {
                _players.Add(player);
            }
            Message m = MessageFactory.Create(MessageType.ChatUsers);
            player.SendMessage(m);
        }

        public void Remove(Player player) {
            _players.Remove(player);
            Message m = MessageFactory.Create(MessageType.DeleteChatUser);
            ((NumericMessage)m).Number = player.Id;
            foreach (Player p in _players) {
                p.SendMessage(m);
            }
           
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
