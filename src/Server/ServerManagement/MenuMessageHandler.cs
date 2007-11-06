using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.General;
using Yad.Net.General.Messaging;
using Client.Log;
using System.Threading;
using Client.Net.General.Messaging;

namespace Server.ServerManagement {
    class MenuMessageHandler : ListProcessor<Message> {

        private Thread _thread;
        private Server _server;

        public Thread Thread {
            get { return _thread; }
            set { _thread = value; }
        }

        public MenuMessageHandler(Server server)
            : base() {
            _thread = new Thread(new ThreadStart(Process));
            _server = server;
        }

        public void Start() {
            _thread.Start();
        }

        public void Join() {
            _thread.Join();
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
            Player player = _server.GetPlayerFromUnlogged(msg.UserId);
            if (null == player || player.State == MenuState.Invalid)
                return;

            int unloggedkey = player.Id;
            if (Login(msg.Login, msg.Password)) {
                MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.Login);
                if (state == MenuState.Invalid) {
                    SendMessage(player, MessageFactory.Create(MessageType.LoginUnsuccessful));
                    return;
                }
                player.State = state;
                player.SetData(LoadPlayerData(msg.Login));
            }

            _server.MoveFromUnloggedToLogged(unloggedkey, player.Id);
            _server.Chat.AddPlayer(player);
            SendMessage(player, (MessageFactory.Create(MessageType.LoginSuccessful)));
        }

        private void SendMessage(Player player, Message message) {
            player.SendMessage(message);
        }

        public bool Login(string login, string password) {
            //TODO - implementacja
            return true;    
        }

        public PlayerData LoadPlayerData(string login) {
            return null;
        }

        public void OnReceivePlayerMessage(object sender, RecieveMessageEventArgs args) {
            this.AddItem(args.Message);
        }



    }
}
