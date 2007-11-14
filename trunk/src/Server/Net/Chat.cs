using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using System.Collections;
using Yad.Net.Common;

namespace Yad.Net.Server {
    class Chat : GameRoom {

        #region Private Members

        /// <summary>
        /// Format wysylanej wiadomosci
        /// </summary>
        private string MessageFormat = "[{0}] : {1}";

        #endregion

        #region Constructors

        public Chat(IMessageSender sender) : base(sender) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Wyslanie wiadomosci tekstowej. W message powinna miec id playera, ktory ja
        /// wyslal
        /// </summary>
        /// <param name="msg"></param>
        public void AddTextMessage(TextMessage msg) {
            string message = string.Format(MessageFormat, 
                ((ChatUser)_players[msg.PlayerId]).Name, msg.Text);
            short id = msg.PlayerId;
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

        #endregion

        #region Overriden Methods

        protected override Message CreateAddMessage(IPlayerID playerID) {
            ChatUsersMessage msg = CreateChatUserMessage();
            msg.ChatUsers.Add((ChatUser)playerID);
            msg.Option = (byte)MessageOperation.Add;
            return msg;
        }

        protected override Message CreateListMessage(IPlayerID playerID) {
            ChatUsersMessage msg = CreateChatUserMessage();
            msg.Option = (byte)MessageOperation.List;
            lock (((ICollection)_players).SyncRoot) {
                foreach (IPlayerID pid in _players.Values) {
                    if (pid.GetID() != playerID.GetID())
                        msg.ChatUsers.Add((ChatUser)pid);
                }
            }
            return msg;
        }

        protected override Message CreateRemoveMessage(IPlayerID playerID) {
            ChatUsersMessage msg = CreateChatUserMessage();
            msg.Option = (byte)MessageOperation.Remove;
            msg.ChatUsers.Add((ChatUser)playerID);
            return msg;
        }

        protected override IPlayerID TransformInitialy(IPlayerID player) {
                Player p = (Player)player;
                return (IPlayerID)PlayerToChatUser(p);
        }

        #endregion
    }
}
