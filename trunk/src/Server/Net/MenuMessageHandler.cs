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

        private Server _server
            ;
        public MenuMessageHandler(Server server)
            : base() {
            _server = server;
        }

       
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
            }

        }

        private void ProcessChatText(TextMessage msg) {
            _server.Chat.AddTextMessage(msg);
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
            //TODO: implementacja
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

            switch (player.State) {
                case MenuState.GameChoose:
                    RemoveFromGameRoom(player);
                break;
            }

            player.State = state;
            AddToChat(player);
            
        }

        public void AddToChat(Player player) {
            _server.Chat.AddPlayer(player);
        }

        public void RemoveFromGameRoom(Player player) {
            //TODO: implementacja
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

        private void LoggingTransfer(Player player) {
            _server.RemovePlayerUnlogged(player.Id);
            _server.AddPlayer(player.Id, player);
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
