 using Yad.Net.Common;
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

       
        public override void ProcessItem(Message msg) {
            InfoLog.WriteInfo(msg.Type.ToString(), EPrefix.MessageReceivedInfo);
            switch (msg.Type) {
                case MessageType.Login:
                    ProcessLogin((LoginMessage)msg);
                    break;
                case MessageType.ChatEntry:
                    ProcessChatEntry(msg);
                    break;
            }

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

            player.State = state;
            _server.Chat.AddPlayer(player);

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
                    SendMessage(MessageFactory.Create(MessageType.LoginUnsuccessful), player.Id);
                    return;
                }

                LoggingTransfer(player);
                player.State = state;
                player.SetData(LoadPlayerData(msg.Login));
            }
            _server.AddPlayer(player.Id, player);
            SendMessage((MessageFactory.Create(MessageType.LoginSuccessful)), player.Id);
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
