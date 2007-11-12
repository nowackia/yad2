using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common {
    public class MessageFactory {
        public static Message Create(MessageType msgType) {
            switch (msgType) {

                #region General messages
                case MessageType.Numeric:
                    return new NumericMessage();

                case MessageType.Text:
                    return new TextMessage();

                case MessageType.Result:
                    return new ResultMessage();

                case MessageType.Entry:
                    return new EntryMessage();
                #endregion

                #region Game messages
                #endregion

                #region Chat
                case MessageType.ChatEntry:
                    {
                        EntryMessage entryMessage = new EntryMessage();
                        entryMessage.ServerRoom = (byte)ServerRoom.Chat;
                        return entryMessage;
                    }

                case MessageType.ChatUsers:
                    return new ChatUsersMessage(MessageType.ChatUsers);

                case MessageType.DeleteChatUser:
                    return new ChatUsersMessage(MessageType.DeleteChatUser);

                case MessageType.NewChatUser:
                    return new ChatUsersMessage(MessageType.NewChatUser);

                case MessageType.ChatText:
                    return new TextMessage(MessageType.ChatText);
                #endregion

                #region Player Info
                case MessageType.PlayerInfo:
                    return new NumericMessage(MessageType.PlayerInfo);

                /* Depreciated */ 
                case MessageType.PlayerInfoSuccessful:
                    return new PlayerInfoMessage(MessageType.PlayerInfoSuccessful);

                /* Depreciated */ 
                case MessageType.PlayerInfoUnsuccessful:
                    return new Message(MessageType.PlayerInfoUnsuccessful);

                case MessageType.PlayerInfoResponse:
                    return new PlayerInfoMessage(MessageType.PlayerInfoResponse);
                #endregion

                #region Game Choosing
                #endregion

                #region Game Creating
                #endregion

                #region Game Joining
                #endregion

                #region Client login messages
                case MessageType.Login:
                    return new LoginMessage(MessageType.Login);

                /* Depreciated */ 
                case MessageType.LoginSuccessful:
                    return new Message(MessageType.LoginSuccessful);

                /* Depreciated */
                case MessageType.LoginUnsuccessful:
                    return new Message(MessageType.LoginUnsuccessful);
        
                case MessageType.LoginResult:
                    {
                        ResultMessage resultMessage = new ResultMessage();
                        resultMessage.ResponseType = (byte)ResponseType.Login;
                        return resultMessage;
                    }
        
                case MessageType.Register:
                    return new RegisterMessage(MessageType.Register);
        
                /* Depreciated */
                case MessageType.RegisterSuccessful:
                    return new Message(MessageType.RegisterSuccessful);

                /* Depreciated */
                case MessageType.RegisterUnsuccessful:
                    return new Message(MessageType.RegisterUnsuccessful);

                case MessageType.RegisterResult:
                    {
                        ResultMessage resultMessage = new ResultMessage();
                        resultMessage.ResponseType = (byte)ResponseType.Register;
                        return resultMessage;
                    }

                case MessageType.Remind:
                    return new RegisterMessage(MessageType.Register);

                /* Depreciated */
                case MessageType.RemindSuccessful:
                    return new RegisterMessage(MessageType.RemindSuccessful);

                /* Depreciated */
                case MessageType.RemindUnsuccessful:
                    return new RegisterMessage(MessageType.RemindUnsuccessful);

                case MessageType.RemindResult:
                    {
                        ResultMessage resultMessage = new ResultMessage();
                        resultMessage.ResponseType = (byte)ResponseType.Remind;
                        return resultMessage;
                    }
                #endregion

                #region Client menu messages
                #endregion

                #region Not changed
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

                case MessageType.Games:
                    return new GamesMessage();

                case MessageType.StartGame:
                    return new TextMessage(MessageType.StartGame);

                case MessageType.StartGameSuccessful:
                    return new Message(MessageType.StartGameSuccessful);

                case MessageType.StartGameUnsuccessful:
                    return new TextMessage(MessageType.StartGameUnsuccessful);
                #endregion

                #region Unknown messages
                case MessageType.Unknown:
                default:
                    return null;
                #endregion
            }
        }
    }
}
