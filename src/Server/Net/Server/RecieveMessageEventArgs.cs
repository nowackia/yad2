using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.General.Messaging;

namespace Yad.Net.Server {

    class RecieveMessageEventArgs : EventArgs {

        private Message _msg;

        public Message Message {
            get { return _msg; }
            set { _msg = value; }
        }
        public RecieveMessageEventArgs(Message msg) {
            _msg = msg;
        }
    }
}
