using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using Client.MessageManagement;
using System.Threading;
using Client.MessageManagment;

namespace Server.ServerManagement {
    delegate void ReceiveMessageDelegate(Message msg);
    class Player {
        StreamReader _readStream;
        StreamWriter _writeStream;
        Thread _rcvThread;
        ReceiveMessageDelegate _onReceiveMessage;

        internal ReceiveMessageDelegate OnReceiveMessage {
            get { return _onReceiveMessage; }
            set { _onReceiveMessage = value; }
        }

        public Player(TcpClient client) {
            _readStream = new StreamReader(client.GetStream());
            _writeStream = new StreamWriter(client.GetStream());
            _rcvThread = new Thread(new ThreadStart(RecieveMessages));
        }

        public void RecieveMessages() {
            MessageType msgType;
            while ((msgType = _readStream.Read()) != -1) {
                Message msg = MessageFactory.Create(msgType);
                msg.Deserialize(_readStream);
                lock (_onReceiveMessage) {
                    _onReceiveMessage(msg);
                }
            }
        }

        
    }
}
