using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Yad.Log.Common;
using Yad.Net.Client;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Client
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

        public GameEventArgs(GameInfo game)
            : this(new GameInfo[] { game })
        { }

        public GameEventArgs(GameInfo[] games)
        {
            this.games = games;
        }
    }

    public class PlayerEventArgs : EventArgs
    {
        public PlayerInfo[] players;

        public PlayerEventArgs(PlayerInfo player)
            : this(new PlayerInfo[] { player })
        { }

        public PlayerEventArgs(PlayerInfo[] players)
        {
            this.players = players;
        }
    }

    public class MenuMessageHandler : IMessageHandler, ISuspender
    {
        public event RequestReplyEventHandler LoginRequestReply;
        public event RequestReplyEventHandler RegisterRequestReply;
        public event RequestReplyEventHandler RemindRequestReply;
        public event RequestReplyEventHandler PlayerInfoRequestReply;
        public event RequestReplyEventHandler CreateGameRequestReply;
        public event RequestReplyEventHandler JoinGameRequestReply;
        public event RequestReplyEventHandler GameParamsRequestReply;
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

        private Semaphore handlerSuspender = new Semaphore(1, 1);

        public void Suspend()
        {
            handlerSuspender.WaitOne();
        }

        public void Resume()
        {
            handlerSuspender.Release();
        }

        public void ProcessMessage(Message message)
        {
            handlerSuspender.WaitOne();
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

                            case ResponseType.CreateGame:
                                if (CreateGameRequestReply != null)
                                    CreateGameRequestReply(this, new RequestReplyEventArgs(!Convert.ToBoolean(result.Result), ((ResultType)(result.Result)).ToString()));
                                break;

                            case ResponseType.JoinGame:
                                if (JoinGameRequestReply != null)
                                    JoinGameRequestReply(this, new RequestReplyEventArgs(!Convert.ToBoolean(result.Result), ((ResultType)(result.Result)).ToString()));
                                break;

                            case ResponseType.StartGame:
                                if (StartGameRequestReply != null)
                                    StartGameRequestReply(this, new RequestReplyEventArgs(!Convert.ToBoolean(result.Result), ((ResultType)(result.Result)).ToString()));
                                break;
                        }
                    }
                    break;

                case MessageType.IdInformation:
                    {
                        NumericMessage numericMessage = message as NumericMessage;
                        ClientPlayerInfo.SenderId = (short)numericMessage.Number;
                    }
                    break;

                case MessageType.ChatUsers:
                    {
                        ChatUsersMessage chatMessage = message as ChatUsersMessage;
                        switch((MessageOperation)chatMessage.Option)
                        {
                            case MessageOperation.Add:
                                if(NewChatUsers != null)
                                    NewChatUsers(this, new ChatEventArgs(chatMessage.ChatUsers.ToArray()));
                                break;

                            case MessageOperation.Remove:
                                if(DeleteChatUsers != null)
                                    DeleteChatUsers(this, new ChatEventArgs(chatMessage.ChatUsers.ToArray()));
                                break;

                            case MessageOperation.List:
                                if(ResetChatUsers != null)
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

                case MessageType.PlayerInfoResponse:
                    if(PlayerInfoRequestReply != null)
                    {
                        PlayerInfoMessage playerInfoMessage = message as PlayerInfoMessage;
                        PlayerInfoRequestReply(this, new RequestReplyEventArgs(true, playerInfoMessage.PlayerData.ToString()));
                    }
                    break;

                case MessageType.Games:
                    {
                        GamesMessage gamesMessage = message as GamesMessage;
                        switch ((MessageOperation)gamesMessage.Operation)
                        {
                            case MessageOperation.Add:
                                if (NewGamesInfo != null)
                                    NewGamesInfo(this, new GameEventArgs(gamesMessage.ListGameInfo.ToArray()));
                                break;

                            case MessageOperation.Remove:
                                if (DeleteGamesInfo != null)
                                    DeleteGamesInfo(this, new GameEventArgs(gamesMessage.ListGameInfo.ToArray()));
                                break;

                            case MessageOperation.List:
                                if (ResetGamesInfo != null)
                                    ResetGamesInfo(this, new GameEventArgs(gamesMessage.ListGameInfo.ToArray()));
                                break;
                        }
                    }
                    break;

                case MessageType.GameParams:
                    {
                        GameInfoMessage gameInfoMessage = message as GameInfoMessage;
                        ClientPlayerInfo.GameInfo = gameInfoMessage.GameInfo;
                        if (GameParamsRequestReply != null)
                            GameParamsRequestReply(this, new RequestReplyEventArgs(true, gameInfoMessage.GameInfo.Description));
                    }
                    break;

                case MessageType.Players:
                    {
                        PlayersMessage playersMessage = message as PlayersMessage;
                        switch ((MessageOperation)playersMessage.Operation)
                        {
                            case MessageOperation.Add:
                                ClientPlayerInfo.Enemies.Add(playersMessage.PlayerList.ToArray());
                                if (NewPlayers != null)
                                    NewPlayers(this, new PlayerEventArgs(playersMessage.PlayerList.ToArray()));
                                break;

                            case MessageOperation.Remove:
                                ClientPlayerInfo.Enemies.Remove(playersMessage.PlayerList.ToArray());
                                if (DeletePlayers != null)
                                    DeletePlayers(this, new PlayerEventArgs(playersMessage.PlayerList.ToArray()));
                                break;

                            case MessageOperation.Modify:
                                {
                                    PlayerInfo[] players = playersMessage.PlayerList.ToArray();
                                    for (int i = 0; i < players.Length; i++)
                                    {
                                        if (ClientPlayerInfo.Player.Id == players[i].Id)
                                            ClientPlayerInfo.Player = players[i];
                                        break;
                                    }
                                    ClientPlayerInfo.Enemies.Modify(players);
                                    if (UpdatePlayers != null)
                                        UpdatePlayers(this, new PlayerEventArgs(players));
                                }
                                break;

                            case MessageOperation.List:
                                ClientPlayerInfo.Enemies.Clear();
                                ClientPlayerInfo.Enemies.Add(playersMessage.PlayerList.ToArray());
                                if (ResetPlayers != null)
                                    ResetPlayers(this, new PlayerEventArgs(playersMessage.PlayerList.ToArray()));
                                break;
                        }
                    }
                    break;

                default:
                    InfoLog.WriteInfo("MenuMessageHandler received unknown message type: " + message.Type, EPrefix.ClientInformation);
                    break;
            }
            handlerSuspender.Release();
        }
    }
}
