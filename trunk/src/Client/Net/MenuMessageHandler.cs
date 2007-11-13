using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;

namespace Client.Net
{
    public delegate void RequestReplyEventHandler(object sender, RequestReplyEventArgs e);
    public delegate void ChatEventHandler(object sender, ChatEventArgs e);
    public delegate void GamesEventHandler(object sender, GameEventArgs e);
    public delegate void PlayersEventHandler(object sender, PlayerEventArgs e);

    public class RequestReplyEventArgs : EventArgs
    {
        public bool successful;
        public string reason;

        public RequestReplyEventArgs(bool successful)
            : this(successful, string.Empty)
        { }

        public RequestReplyEventArgs(bool successful, string reason)
        {
            this.successful = successful;
            this.reason = reason;
        }
    }

    public class ChatEventArgs : EventArgs
    {
        public ChatUser[] chatUsers;
        public string text;


        public ChatEventArgs(string text)
        {
            chatUsers = null;
            this.text = text;
        }

        public ChatEventArgs(ChatUser chatUser)
            : this(chatUser, string.Empty)
        { }

        public ChatEventArgs(ChatUser[] chatUsers)
            : this(chatUsers, string.Empty)
        { }

        public ChatEventArgs(ChatUser chatUser, string text)
            : this(new ChatUser[] { chatUser }, text)
        { }

        public ChatEventArgs(ChatUser[] chatUsers, string text)
        {
            this.chatUsers = chatUsers;
            this.text = text;
        }
    }

    public class GameEventArgs : EventArgs
    {
        public GameInfo[] games;
        public string text;

        public GameEventArgs(GameInfo game, string text)
            : this(new GameInfo[] { game }, text)
        { }

        public GameEventArgs(GameInfo[] games, string text)
        {
            this.games = games;
            this.text = text;
        }
    }

    public class PlayerEventArgs : EventArgs
    {
        public PlayerInfo[] players;
        public string text;
        public bool update;

        public PlayerEventArgs(PlayerInfo player, string text)
            : this(player, text, false)
        { }

        public PlayerEventArgs(PlayerInfo[] players, string text)
            : this(players, text, false)
        { }

        public PlayerEventArgs(PlayerInfo player, string text, bool update)
            : this(new PlayerInfo[] { player }, text, update)
        { }

        public PlayerEventArgs(PlayerInfo[] players, string text, bool update)
        {
            this.players = players;
            this.text = text;
            this.update = update;
        }
    }

    public class MenuMessageHandler : IMessageHandler
    {
        public event RequestReplyEventHandler LoginRequestReply;
        public event RequestReplyEventHandler RegisterRequestReply;
        public event RequestReplyEventHandler RemindRequestReply;

        public event RequestReplyEventHandler PlayerInfoRequestReply;
        public event RequestReplyEventHandler JoinGameRequestReply;
        public event RequestReplyEventHandler StartGameRequestReply;

        public event ChatEventHandler NewChatUsers;
        public event ChatEventHandler DeleteChatUsers;
        public event ChatEventHandler ResetChatUsers;
        public event ChatEventHandler ChatTextReceive;

        public event GamesEventHandler NewGamesInfo;
        public event GamesEventHandler DeleteGamesInfo;
        public event GamesEventHandler ResetGamesInfo;

        public event PlayersEventHandler NewPlayers;
        public event PlayersEventHandler DeletePlayers;
        public event PlayersEventHandler UpdatePlayers;
        public event PlayersEventHandler ResetPlayers;

        public void ProcessMessage(Message message)
        {
            switch (message.Type)
            {
                case MessageType.Result:
                    {
                        ResultMessage result = message as ResultMessage;
                        switch ((ResponseType)result.ResponseType)
                        {
                            case ResponseType.Login:
                                if (LoginRequestReply != null)
                                    LoginRequestReply(this, new RequestReplyEventArgs(!Convert.ToBoolean(result.Result), ((ResultType)(result.Result)).ToString()));
                                break;

                            case ResponseType.Register:
                                if (RegisterRequestReply != null)
                                    RegisterRequestReply(this, new RequestReplyEventArgs(!Convert.ToBoolean(result.Result), ((ResultType)(result.Result)).ToString()));
                                break;

                            case ResponseType.Remind:
                                if (RemindRequestReply != null)
                                    RemindRequestReply(this, new RequestReplyEventArgs(!Convert.ToBoolean(result.Result), ((ResultType)(result.Result)).ToString()));
                                break;
                        }
                    }
                    break;

                case MessageType.ChatUsers:
                    {
                        ChatUsersMessage chatMessage = message as ChatUsersMessage;
                        switch((MessageOperation)chatMessage.Option)
                        {
                            case MessageOperation.Add:
                                NewChatUsers(this, new ChatEventArgs(chatMessage.ChatUsers.ToArray()));
                                break;

                            case MessageOperation.Remove:
                                DeleteChatUsers(this, new ChatEventArgs(chatMessage.ChatUsers.ToArray()));
                                break;

                            case MessageOperation.List:
                                ResetChatUsers(this, new ChatEventArgs(chatMessage.ChatUsers.ToArray()));
                                break;
                        }
                    }
                    break;

                case MessageType.ChatText:
                    if (ChatTextReceive != null)
                    {
                        TextMessage textMessage = message as TextMessage;
                        ChatTextReceive(this, new ChatEventArgs(textMessage.Text));
                    }
                    break;

                case MessageType.PlayerInfoSuccessful:
                    if (PlayerInfoRequestReply != null)
                    {
                        PlayerData playerData = ((PlayerInfoMessage)message).PlayerData;
                        string info = "Login: " + playerData.Login + Environment.NewLine + "Wins: " + playerData.WinNo + Environment.NewLine + "Losses: " + playerData.LossNo;
                        PlayerInfoRequestReply(this, new RequestReplyEventArgs(true, info));
                    }
                    break;

                case MessageType.PlayerInfoUnsuccessful:
                    if (PlayerInfoRequestReply != null)
                        PlayerInfoRequestReply(this, new RequestReplyEventArgs(true, "Error getting player data"));
                    break;

                case MessageType.JoinGameSuccessful:
                    if (JoinGameRequestReply != null)
                        JoinGameRequestReply(this, new RequestReplyEventArgs(true, "Join game successful"));
                    break;

                case MessageType.JoinGameUnsuccessful:
                    if (JoinGameRequestReply != null)
                        JoinGameRequestReply(this, new RequestReplyEventArgs(false, ((TextMessage)message).Text));
                    break;

                case MessageType.StartGameSuccessful:
                    if (StartGameRequestReply != null)
                        StartGameRequestReply(this, new RequestReplyEventArgs(true, "Start game successful"));
                    break;

                case MessageType.StartGameUnsuccessful:
                    if (StartGameRequestReply != null)
                        StartGameRequestReply(this, new RequestReplyEventArgs(false, ((TextMessage)message).Text));
                    break;

                case MessageType.NewGame:
                    if(NewGamesInfo != null)
                    {
                        GamesMessage gamesList = message as GamesMessage;
                        NewGamesInfo(this, new GameEventArgs(gamesList.ListGameInfo.ToArray(), string.Empty));
                    }
                    break;

                case MessageType.DeleteGame:
                    if(DeleteGamesInfo != null)
                    {
                        GamesMessage gamesList = message as GamesMessage;
                        DeleteGamesInfo(this, new GameEventArgs(gamesList.ListGameInfo.ToArray(), string.Empty));
                    }
                    break;

                /*case MessageType.GamesList:
                    if(ResetGamesInfo != null)
                    {
                        GamesListMessage gamesList = message as GamesListMessage;
                        ResetGamesInfo(this, new GameEventArgs(gamesList.Games.ToArray(), string.Empty));
                    }
                    break;

                case MessageType.PlayersList:
                    if (ResetPlayers != null)
                    {
                        PlayersListMessage playersList = message as PlayersListMessage;
                        ResetPlayers(this, new PlayerEventArgs(playersList.Players.ToArray(), string.Empty));
                    }
                    break;

                case MessageType.NewPlayer:
                    if (NewPlayers != null)
                    {
                        PlayersListMessage playersList = message as PlayersListMessage;
                        NewPlayers(this, new PlayerEventArgs(playersList.Players.ToArray(), string.Empty));
                    }
                    break;

                case MessageType.DeletePlayer:
                    if (DeletePlayers != null)
                    {
                        PlayersListMessage playersList = message as PlayersListMessage;
                        DeletePlayers(this, new PlayerEventArgs(playersList.Players.ToArray(), string.Empty));
                    }
                    break;

                case MessageType.UpdatePlayer:
                    if (UpdatePlayers != null)
                    {
                        PlayersListMessage playersList = message as PlayersListMessage;
                        UpdatePlayers(this, new PlayerEventArgs(playersList.Players.ToArray(), string.Empty, true));
                    }
                    break;*/

                default:
                    break;
            }
        }
    }
}
