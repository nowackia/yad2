using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Server {

    public class ReceiveMessageEventArgs : EventArgs {

        private Message _msg;

        public Message Message {
            get { return _msg; }
            set { _msg = value; }
        }
        public ReceiveMessageEventArgs(Message msg) {
            _msg = msg;
        }
    }
}
