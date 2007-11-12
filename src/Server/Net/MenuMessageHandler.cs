using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.Log;
using Yad.Log.Common;

namespace Yad.Net.Server
{
    class MenuMessageHandler : MessageHandler {

        private Server _server;
        public MenuMessageHandler(Server server)
            : base() {
            _server = server;
        }

        #region Message processing

        /// <summary>
        /// Main processing function
        /// </summary>
        /// <param name="msg">Message to process</param>
        public override void ProcessItem(Message msg) {
            InfoLog.WriteInfo(msg.Type.ToString(), EPrefix.MessageReceivedInfo);
            switch (msg.Type) {
                case MessageType.Login:
                    ProcessLogin((LoginMessage)msg);
                    break;
                case MessageType.Entry:
                    ProcessEntry(msg);
                    break;
                case MessageType.ChatText:
                    ProcessChatText((TextMessage)msg);
                    break;
                case MessageType.CreateGame:
                    ProcessCreateGame((GameInfoMessage)msg);
                    break;
                case MessageType.JoinGame:
                    ProcessJoinGame((TextMessage)msg);
                    break;
            }

        }


        private void ProcessJoinGame(TextMessage textMessage) {

            Player player = _server.GetPlayer(textMessage.PlayerId);
            if (player == null)
                return;
            MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.GameJoinEntry);
            if (state == MenuState.Invalid)
                SendMessage(Utils.CreateResultMessage(ResponseType.JoinGame, ResultType.Unsuccesful), player.Id);
            
            ResultType result = _server.GameManager.JoinGame(textMessage.Text, player);
            if (result == ResultType.Successful) {
                player.State = state;
            }
            SendMessage(Utils.CreateResultMessage(ResponseType.JoinGame, result), player.Id);
        }

        private void ProcessChatText(TextMessage msg) {
            _server.Chat.AddTextMessage(msg);
        }

        public void ProcessLogin(LoginMessage msg) {

            //Pobranie gracza z listy niezalogowanych
            Player player = _server.GetPlayerUnlogged(msg.PlayerId);

            //Jesli gracza nie ma, lub jego stan jest jakis dziwny - koniec obslugi
            if (null == player || player.State != MenuState.Unlogged)
                return;

            if (Login(msg.Login, msg.Password)) {
                MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.Login);
                if (state == MenuState.Invalid) {
                    SendMessage(Utils.CreateResultMessage(ResponseType.Login, ResultType.Unsuccesful), player.Id);
                    return;
                }

                LoggingTransfer(player);
                player.State = state;
                player.SetData(LoadPlayerData(msg.Login));
            }

            SendMessage(Utils.CreateResultMessage(ResponseType.Login, ResultType.Successful), player.Id);
        }

        private void ProcessEntry(Message msg) {
            EntryMessage emsg = msg as EntryMessage;
            switch ((ServerRoom)emsg.ServerRoom) {
                case ServerRoom.Chat:
                    ProcessChatEntry(emsg);
                    break;
                case ServerRoom.GameChoose:
                    ProcessGameChooseEntry(emsg);
                    break;
            }
        }

        public void ProcessGameChooseEntry(Message msg) {
            Player player = _server.GetPlayer(msg.PlayerId);

            if (null == player || player.State == MenuState.GameChoose)
                return;

            MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.GameChooseEntry);
            if (state == MenuState.Invalid) {
                InfoLog.WriteInfo("GameChoose entry unsuccesful by player: " + player.Login, EPrefix.ServerInformation);
                return;
            }
            
            //wejscie moze nastapic z czatu albo z game joina
            switch (player.State) {
                case MenuState.Chat:
                    MoveFromChatToGameRoom(player);
                    break;
                case MenuState.GameJoin:
                    RemoveFromGameJoin(player);
                    break;
            }

            player.State = state;
        }

        public void ProcessChatEntry(Message msg) {

            Player player = _server.GetPlayer(msg.PlayerId);

            if (null == player || player.State == MenuState.Chat)
                return;

            MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.ChatEntry);
            if (state == MenuState.Invalid) {
                InfoLog.WriteInfo("Chat entry unsuccesful", EPrefix.ServerInformation);
                return;
            }
            //Mogly zajsc dwie sytuacje - gracz przechodzi ze stanu zalogowany, albo z 
            //game roomu 
            switch (player.State) {
                case MenuState.GameChoose:
                    RemoveFromGameRoom(player);
                break;
            }

            player.State = state;
            AddToChat(player);
        }

        private void ProcessCreateGame(GameInfoMessage msg) {
            Player player = _server.GetPlayer(msg.PlayerId);
            MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.GameJoinEntry);
            ResultMessage resMsg  =  null;
            if (state == MenuState.GameJoin) {
                resMsg = CreateGame(msg);
                player.State = state;
            }
            SendMessage(resMsg, msg.PlayerId);
        }

        private ResultMessage CreateGame(GameInfoMessage msg) {
            ResultType resultType = _server.GameManager.CreateGame(msg.GameInfo);
            if (resultType == ResultType.Successful) {
                ResultType joinResult = _server.GameManager.JoinGame(msg.GameInfo.Name, _server.GetPlayer(msg.PlayerId));
                if (joinResult != ResultType.Successful) {
                    InfoLog.WriteError("Join not possible after create!");
                    return null;
                }
            }
            ResultMessage resultmsg = MessageFactory.Create(MessageType.Result) as ResultMessage;
            resultmsg.ResponseType = (byte)ResponseType.CreateGame;
            resultmsg.Result = (byte)resultType;
            return resultmsg;
        }

        #endregion

        public void AddToChat(Player player) {
            _server.Chat.AddPlayer(player);
        }

        public void RemoveFromGameRoom(Player player) {
            _server.GameManager.RemovePlayer(player.Id);
        }

        private void LoggingTransfer(Player player) {
            _server.RemovePlayerUnlogged(player.Id);
            _server.AddPlayer(player.Id, player);
        }

        private void RemoveFromGameJoin(Player player) {
            _server.GameManager.RemoveFromGameJoin(player.Id);
        }

        private void MoveFromChatToGameRoom(Player player) {
            _server.Chat.RemovePlayer(player);
            _server.GameManager.AddPlayer(player);
        }

        public bool Login(string login, string password) {
            return true;    
        }

        public PlayerData LoadPlayerData(string login) {
            PlayerData pd = new PlayerData();
            pd.Login = login;
            pd.LossNo = 1;
            pd.WinNo = 1;
            return pd;
        }
    }
}
