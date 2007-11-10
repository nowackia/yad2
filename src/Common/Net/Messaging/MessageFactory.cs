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
                #endregion

                case MessageType.ChatUsers:
                    return new ChatUsersMessage(MessageType.ChatUsers);

                case MessageType.NewChatUser:
                    return new ChatUsersMessage(MessageType.NewChatUser);

                case MessageType.DeleteChatUser:
                    return new ChatUsersMessage(MessageType.DeleteChatUser);

                case MessageType.PlayerInfo:
                    return new Message(MessageType.PlayerInfo);

                case MessageType.LoginSuccessful:
                    return new Message(MessageType.LoginSuccessful);

                case MessageType.PlayerInfoSuccessful:
                    return new PlayerInfoMessage(MessageType.PlayerInfoSuccessful);

                case MessageType.PlayerInfoUnsuccessful:
                    return new Message(MessageType.PlayerInfoUnsuccessful);

                case MessageType.PlayersList:
                    return new PlayersListMessage(MessageType.PlayersList);

                case MessageType.NewPlayer:
                    return new PlayersListMessage(MessageType.NewPlayer);

                case MessageType.DeletePlayer:
                    return new PlayersListMessage(MessageType.DeletePlayer);

                case MessageType.UpdatePlayer:
                    return new PlayersListMessage(MessageType.UpdatePlayer);

                case MessageType.NewGame:
                    return new GamesListMessage(MessageType.NewGame);

                case MessageType.DeleteGame:
                    return new GamesListMessage(MessageType.DeleteGame);

                case MessageType.GamesList:
                    return new GamesListMessage(MessageType.GamesList);

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
