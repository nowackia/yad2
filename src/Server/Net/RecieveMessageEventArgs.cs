using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Server {

    public class RecieveMessageEventArgs : EventArgs {

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
