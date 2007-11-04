using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using Yad.Net.General.Messaging;
using System.Threading;
using Client.Log;
using Yad.Net.General;


namespace Server.ServerManagement {
    
    delegate void ReceiveMessageDelegate(Message msg);
    delegate void ConnectionLostDelegate(Player player);

    class Player {

        #region Private members

        private BinaryReader _readStream;
        private BinaryWriter _writeStream;
        private Thread _rcvThread;
        private ReceiveMessageDelegate _onReceiveMessage;
        private ConnectionLostDelegate _onConnectionLost;
        private int _id;
        private MenuState _state;

        #endregion Private members

        public int Id {
            get { return _id; }
            set { _id = value; }
        }

        public MenuState State {
            get { return _state; }
            set { _state = value; }
        }

        internal ConnectionLostDelegate OnConnectionLost {
            get { return _onConnectionLost; }
            set { _onConnectionLost = value; }
        }

        internal ReceiveMessageDelegate OnReceiveMessage {
            get { return _onReceiveMessage; }
            set { _onReceiveMessage = value; }
        }

        

        public Player(int id, TcpClient client) {
            _id = id;
            _readStream = new BinaryReader(client.GetStream());
            _writeStream = new BinaryWriter(client.GetStream());
            _rcvThread = new Thread(new ThreadStart(ReceiveMessages));
        }

        public void Start() {
            _rcvThread.Start();
        }

        public void SendMessage(Message msg) {
            try {
                msg.Serialize(_writeStream);
            }
            catch (Exception) {
                lock (_onConnectionLost) {
                    if (_onConnectionLost != null)
                        _onConnectionLost(this);
                }
            }
        }

        public void ReceiveMessages() {
            byte type;
            while (true) {
                try {
                    type = _readStream.ReadByte();
                }
                catch (Exception) {
                    lock (_onConnectionLost) {
                        if (_onConnectionLost != null)
                            _onConnectionLost(this);
                    }
                    return;
                    
                }
                InfoLog.WriteInfo("Player sent message");
                Message msg = MessageFactory.Create((MessageType)type);
                if (null == msg) {
                    InfoLog.WriteInfo("Received unknown message", EPrefix.MessageReceivedInfo);
                    continue;
                }
                msg.Deserialize(_readStream);
                lock (_onReceiveMessage) {
                    _onReceiveMessage(msg);
                }
            }
        }

        
    }
}
