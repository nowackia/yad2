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

        public Thread Thread {
            get { return _thread; }
            set { _thread = value; }
        }

        public MenuMessageHandler()
            : base() {
            _thread = new Thread(new ThreadStart(Process));
        }

        public void Start() {
            _thread.Start();
        }

        public void Join() {
            _thread.Join();
        }

        public override void ProcessMessage(Message msg) {
            switch (msg.Type) {
                case MessageType.Login:
                    ProcessLogin((LoginMessage)msg);
                    break;
            }

        }

        public void ProcessLogin(LoginMessage msg) {

        }

    }
}
