using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.General;
using Yad.Net.General.Messaging;
using Client.Log;
using System.Threading;
using Client.Net.General.Messaging;

namespace Server.ServerManagement {
    class MenuMessageHandler : MessageProcessor {

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

        public override void ProcessMessage(Message msg) {
            InfoLog.WriteInfo(msg.Type.ToString(), EPrefix.MessageReceivedInfo);
            switch (msg.Type) {
                case MessageType.Login:
                    ProcessLogin((LoginMessage)msg);
                    break;
            }

        }

        public void ProcessLogin(LoginMessage msg) {
            Player player = null; 
            lock (_server.PlayersUnlogged) {
                if (_server.PlayersUnlogged.ContainsKey(msg.UserId)) {
                    player = _server.PlayersUnlogged[player.Id];
                }
            }
            if (null == player)
                return;

            
            if (Login(msg.Login, msg.Password)) {
                MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.Login);
                if (state == MenuState.Invalid)
                    return;
                player.State = state;
                player.SetData(LoadPlayerData(msg.Login));
            }

            lock (_server.PlayersUnlogged) {
                _server.PlayersUnlogged.Remove(player.Id);
            }
            lock (_server.PlayersLogged) {
                _server.PlayersLogged.Add(player.Id, player);
            }
            _server.Chat.AddPlayer(player);
        }

        public bool Login(string login, string password) {
            //TODO - implementacja
            return true;    
        }

        public PlayerData LoadPlayerData(string login) {
            return null;
        }



    }
}
