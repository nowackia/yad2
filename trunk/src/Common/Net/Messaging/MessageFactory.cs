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
                case MessageType.GameInit:
                    return new GameInitMessage();

                case MessageType.Move:
                    return new MoveMessage();

                case MessageType.Destroy:
                    return new DestroyMessage();

                case MessageType.CreateUnit:
                    return new CreateUnitMessage();

                case MessageType.Build:
                    return new BuildMessage();

                case MessageType.Harvest:
                    return new HarvestMessage();

                case MessageType.Attack:
                    return new AttackMessage();

                case MessageType.Control:
                    return new ControlMessage();

                case MessageType.TurnAsk:
                    return new TurnAskMessage();

                case MessageType.DoTurn:
                    return new DoTurnMessage();
                case MessageType.PlayerDisconnected:
                    return new GameNumericMessage(MessageType.PlayerDisconnected);
                case MessageType.EndGame:
                    return new GameEndMessage();
                #endregion

                #region Chat
                /* For MessageFactory Only */
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
                    return new TextMessage(MessageType.PlayerInfoUnsuccessful);

                case MessageType.PlayerInfoResponse:
                    return new PlayerInfoMessage(MessageType.PlayerInfoResponse);
                #endregion

                #region Game Choosing
                /* For MessageFactory Only */
                case MessageType.ChooseGameEntry:
                    {
                        EntryMessage entryMessage = new EntryMessage();
                        entryMessage.ServerRoom = (byte)ServerRoom.GameChoose;
                        return entryMessage;
                    }

                case MessageType.Games:
                    {
                        GamesMessage gamesMessage = new GamesMessage();
                        gamesMessage.Operation = (byte)MessageOperation.List;
                        return gamesMessage;
                    }

                case MessageType.NewGame:
                    {
                        GamesMessage gamesMessage = new GamesMessage();
                        gamesMessage.Operation = (byte)MessageOperation.Add;
                        return gamesMessage;
                    }

                case MessageType.DeleteGame:
                    {
                        GamesMessage gamesMessage = new GamesMessage();
                        gamesMessage.Operation = (byte)MessageOperation.Remove;
                        return gamesMessage;
                    }
                #endregion

                #region Game Creating
                case MessageType.CreateGame:
                    return new GameInfoMessage(MessageType.CreateGame);

                /* Depreciated */
                case MessageType.CreateGameSuccessful:
                    return new Message(MessageType.CreateGameSuccessful);

                /* Depreciated */
                case MessageType.CreateGameUnsuccessful:
                    return new TextMessage(MessageType.CreateGameUnsuccessful);

                case MessageType.CreateGameResult:
                    {
                        ResultMessage resultMessage = new ResultMessage();
                        resultMessage.ResponseType = (byte)ResponseType.CreateGame;
                        return resultMessage;
                    }
                #endregion

                #region Game Joining
                case MessageType.Players:
                    {
                        PlayersMessage playersMessage = new PlayersMessage();
                        playersMessage.Operation = (byte)MessageOperation.List;
                        return playersMessage;
                    }

                case MessageType.NewPlayer:
                    {
                        PlayersMessage playersMessage = new PlayersMessage();
                        playersMessage.Operation = (byte)MessageOperation.Add;
                        return playersMessage;
                    }

                case MessageType.DeletePlayer:
                    {
                        PlayersMessage playersMessage = new PlayersMessage();
                        playersMessage.Operation = (byte)MessageOperation.Remove;
                        return playersMessage;
                    }

                case MessageType.UpdatePlayer:
                    {
                        PlayersMessage playersMessage = new PlayersMessage();
                        playersMessage.Operation = (byte)MessageOperation.Modify;
                        return playersMessage;
                    }

                /* Depreciated */
                case MessageType.StartGameSuccessful:
                    return new Message(MessageType.StartGameSuccessful);

                /* Depreciated */
                case MessageType.StartGameUnsuccessful:
                    return new TextMessage(MessageType.StartGameUnsuccessful);

                case MessageType.JoinGame:
                    return new TextMessage(MessageType.JoinGame);

                /* Depreciated */
                case MessageType.JoinGameSuccessful:
                    return new Message(MessageType.JoinGameSuccessful);

                /* Depreciated */
                case MessageType.JoinGameUnsuccessful:
                    return new TextMessage(MessageType.JoinGameUnsuccessful);

                case MessageType.JoinGameResult:
                    {
                        ResultMessage resultMessage = new ResultMessage();
                        resultMessage.ResponseType = (byte)ResponseType.JoinGame;
                        return resultMessage;
                    }

                case MessageType.GameParams:
                    return new GameInfoMessage(MessageType.GameParams);

                case MessageType.StartGame:
                    return new TextMessage(MessageType.StartGame);

                case MessageType.StartGameResult:
                    {
                        ResultMessage resultMessage = new ResultMessage();
                        resultMessage.ResponseType = (byte)ResponseType.StartGame;
                        return resultMessage;
                    }
                #endregion

                #region Client login messages
                case MessageType.IdInformation:
                    return new NumericMessage(MessageType.IdInformation);

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
                    return new TextMessage(MessageType.Register);

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
                case MessageType.Logout:
                    return new Message(MessageType.Logout);
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
