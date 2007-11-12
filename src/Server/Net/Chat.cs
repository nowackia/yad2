using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using System.Collections;
using Yad.Net.Common;

namespace Yad.Net.Server {
    class Chat {

        #region Private Members

        private Dictionary<short, ChatUser> _players;
        private IMessageSender _sender;
        private string MessageFormat = "[{0}] : {1}";

        #endregion

        #region Constructors

        public Chat(IMessageSender sender) {
            _players = new Dictionary<short, ChatUser>();
            _sender = sender;
        }

        #endregion

        #region Public Methods

        public void AddPlayer(Player player) {
            ChatUser cu = PlayerToChatUser(player);
            lock (((ICollection)_players).SyncRoot) {
                _players.Add(player.Id, cu);
            }
            SendAddPlayer(cu);
            SendListPlayer(cu);
        }

        public void RemovePlayer(Player player) {
            ChatUser cu = PlayerToChatUser(player);
            lock (((ICollection)_players).SyncRoot) {
                _players.Remove(cu.Id);
            }
            SendRemovePlayer(cu);
        }

        public void AddTextMessage(TextMessage msg) {
            string message = string.Format(MessageFormat, _players[msg.PlayerId].Name, msg.Text);
            int id = msg.PlayerId;
            msg.Text = message;
            msg.PlayerId = -1;
            BroadcastExcl(msg, id);
        }

        #endregion

        #region Private Methods

        private ChatUsersMessage CreateChatUserMessage(){
            ChatUsersMessage msg = MessageFactory.Create(MessageType.ChatUsers) as ChatUsersMessage;
            return msg;
        }

        private ChatUser PlayerToChatUser(Player p)
        {
            return new ChatUser(p.Id, p.Login);
        }

      
        private void BroadcastExcl(Message msg, int id)
        {
            lock (((ICollection)_players).SyncRoot)
                foreach (ChatUser cu in _players.Values)
                    if (cu.Id != id)
                        _sender.MessagePost(msg, cu.Id);
        }


        private void SendMessage(Message msg, short id) {
            _sender.MessagePost(msg, id);
        }

        private void SendAddPlayer(ChatUser cu) {
            ChatUsersMessage msg = CreateChatUserMessage();
            msg.ChatUsers.Add(cu);
            msg.Option = (byte)MessageOperation.Add;
            BroadcastExcl(msg, cu.Id);
        }

        private void SendListPlayer(ChatUser chuser) {
            ChatUsersMessage msg = CreateChatUserMessage();
            msg.Option = (byte)MessageOperation.List;
            lock (((ICollection)_players).SyncRoot) {
                foreach (ChatUser cu in _players.Values) {
                    msg.ChatUsers.Add(cu);
                }
            }
            SendMessage(msg, chuser.Id);
        }

        private void SendRemovePlayer(ChatUser cu) {
            ChatUsersMessage msg = CreateChatUserMessage();
            msg.Option = (byte)MessageOperation.Remove;
            msg.ChatUsers.Add(cu);
            BroadcastExcl(msg, -1);

        }

        #endregion

    }
}
