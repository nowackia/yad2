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

                case MessageType.JoinGameSuccessful:
                    return new Message(MessageType.JoinGameSuccessful);

                case MessageType.JoinGameUnsuccessful:
                    return new TextMessage(MessageType.JoinGameUnsuccessful);

                case MessageType.Logout:
                    return new Message(MessageType.Logout);
                #endregion

                case MessageType.Result:
                    return new ResultMessage();
                case MessageType.Entry:
                    return new EntryMessage();
                case MessageType.ChatUsers:
                    return new ChatUsersMessage(MessageType.ChatUsers);
                case MessageType.GamesMessage:
                    return new GamesMessage();
                case MessageType.NewChatUser:
                    return new ChatUsersMessage(MessageType.NewChatUser);

                case MessageType.DeleteChatUser:
                    return new ChatUsersMessage(MessageType.DeleteChatUser);

                case MessageType.PlayerInfo:
                    return new Message(MessageType.PlayerInfo);

                case MessageType.LoginSuccessful:
                    return new Message(MessageType.LoginSuccessful);

                case MessageType.LoginUnsuccessful:
                    return new TextMessage(MessageType.LoginUnsuccessful);

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

                case MessageType.StartGame:
                    return new TextMessage(MessageType.StartGame);

                case MessageType.StartGameSuccessful:
                    return new Message(MessageType.StartGameSuccessful);

                case MessageType.StartGameUnsuccessful:
                    return new TextMessage(MessageType.StartGameUnsuccessful);

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
