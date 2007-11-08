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
            }

        }

        public void ProcessLogin(LoginMessage msg) {
            Player player = _server.GetPlayerUnlogged(msg.PlayerId);
            if (null == player || player.State == MenuState.Invalid)
                return;

            int unloggedkey = player.Id;
            if (Login(msg.Login, msg.Password)) {
                MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.Login);
                if (state == MenuState.Invalid) {
                    SendMessage(MessageFactory.Create(MessageType.LoginUnsuccessful), player.Id);
                    return;
                }
                player.State = state;
                player.SetData(LoadPlayerData(msg.Login));
            }

            //_server.Chat.AddPlayer(player);
            SendMessage((MessageFactory.Create(MessageType.LoginSuccessful)), player.Id);
        }

        public bool Login(string login, string password) {
            return true;    
        }

        public PlayerData LoadPlayerData(string login) {
            PlayerData pd = new PlayerData();
            pd.Id = 1;
            pd.Login = login;
            pd.LossNo = 1;
            pd.WinNo = 1;
            return pd;
        }

        public void OnReceivePlayerMessage(object sender, RecieveMessageEventArgs args) {
            this.AddItem(args.Message);
        }



    }
}
