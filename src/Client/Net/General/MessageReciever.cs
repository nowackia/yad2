using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace Yad.Net.General {
    class MessageReciever {
        private Thread _thRcv = null;

        public MessageReciever(NetworkStream NetStream) {
        }
        public void Start() {
            _thRcv = new Thread(new ThreadStart(Process));
        }
        public void Process() {

        }
    }
}
