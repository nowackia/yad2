using Yad.Net.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Yad.Log;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;

namespace Yad.Net.Server {
    
    delegate void ReceiveMessageDelegate(object sender, RecieveMessageEventArgs eventArgs);
    delegate void ConnectionLostDelegate(object sender, ConnectionLostEventArgs eventArgs);

    class Player {

        #region Private members

        private BinaryReader _readStream;
        private BinaryWriter _writeStream;
        private Thread _rcvThread;
        private event ReceiveMessageDelegate _onReceiveMessage;
        private event ConnectionLostDelegate _onConnectionLost;
        private short _id;
        private MenuState _state;
        private PlayerData _data;

        #endregion Private members

        public short Id {
            get { return _id; }
            set { _id = value; }
        }

        public MenuState State {
            get { return _state; }
            set { _state = value; }
        }

        public string Login {
            get { return _data.Login; }
        }

        public int WinNo {
            get { return _data.WinNo; }
        }

        public int LossNo {
            get { return _data.LossNo; }
        }

        internal ConnectionLostDelegate OnConnectionLost {
            get { return _onConnectionLost; }
            set { _onConnectionLost = value; }
        }

        internal ReceiveMessageDelegate OnReceiveMessage {
            get { return _onReceiveMessage; }
            set { _onReceiveMessage = value; }
        }


        public void SetData(PlayerData pd) {
            if (pd != null)
            {
                _data = pd;
                _id = pd.Id;
            }
        }
        public Player(short id, TcpClient client) {
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
                ExecuteOnConnectionLost();
            }
        }

        private void ExecuteOnConnectionLost() {
            lock (_onConnectionLost) {
                if (_onConnectionLost != null)
                    _onConnectionLost(this, new ConnectionLostEventArgs());
            }
        }

        private void ExecuteOnReceiveMessage(Message msg) {
            lock (_onReceiveMessage) {
                if (_onReceiveMessage != null)
                _onReceiveMessage(this, new RecieveMessageEventArgs(msg));
            }
        }
        public void ReceiveMessages() {
            byte type;
            while (true) {
                try {
                    type = _readStream.ReadByte();
                }
                catch (Exception ex) {
                    ExecuteOnConnectionLost();
                    InfoLog.WriteException(ex);
                    return;
                    
                }
                InfoLog.WriteInfo("Player sent message");
                Message msg = MessageFactory.Create((MessageType)type);
                InfoLog.WriteInfo("Received message with type " + type, EPrefix.MessageReceivedInfo);
                if (null == msg) {
                    InfoLog.WriteInfo("Received unknown message", EPrefix.MessageReceivedInfo);
                    continue;
                }
                msg.Deserialize(_readStream);
                msg.PlayerId = Id;
                ExecuteOnReceiveMessage(msg);
            }
        }

        
    }
}
