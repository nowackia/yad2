using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common {
    public class MessageFactory {
        public static Message Create(MessageType msgType) {
            switch (msgType) {

                #region Client login messages
                case MessageType.Login:
                    return new LoginMessage();

                case MessageType.Register:
                    return new RegisterMessage();

                case MessageType.Remind:
                    return new TextMessage(MessageType.Remind);
                #endregion

                #region Client menu messages
                case MessageType.ChatEntry:
                    return new Message(MessageType.ChatEntry);

                case MessageType.ChatExit:
                    return new Message(MessageType.ChatExit);

                case MessageType.ChatText:
                    return new TextMessage(MessageType.ChatText);

                case MessageType.ChooseGameEntry:
                    return new Message(MessageType.ChooseGameEntry);

                case MessageType.JoinGameEntry:
                    return new TextMessage(MessageType.JoinGameEntry);

                case MessageType.JoinGameExit:
                    return new Message(MessageType.JoinGameExit);

                case MessageType.Logout:
                    return new Message(MessageType.Logout);

                case MessageType.GameCreate:
                    return new Message(MessageType.GameCreate);
                #endregion

                case MessageType.ChatUsers:
                    return new ChatUsersMessage();

                case MessageType.DeleteChatUser:
                    return new NumericMessage(MessageType.DeleteChatUser);

                case MessageType.LoginSuccessful:
                    return new Message(MessageType.LoginSuccessful);

                case MessageType.Move:
                    return new MoveMessage();

                #region Unknown messages
                case MessageType.Unknown:
                default:
                    return null;
                #endregion
            }
        }
    }
}
