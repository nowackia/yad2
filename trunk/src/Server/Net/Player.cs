
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
    
    public delegate void ReceiveMessageDelegate(object sender, ReceiveMessageEventArgs eventArgs);
    public delegate void ConnectionLostDelegate(object sender, ConnectionLostEventArgs eventArgs);

    public class Player : IPlayerID {

        #region Private members

        private BinaryReader _readStream;
        private BinaryWriter _writeStream;
        private Thread _rcvThread;
        private event ReceiveMessageDelegate _onReceiveMessage;
        private event ConnectionLostDelegate _onConnectionLost;
        private short _id;
        private MenuState _state;
        private PlayerData _data;
        private string _gameName;
        private bool _isDisconnected = false;

        private readonly object _rcvMsgLock = new object();
        private readonly object _clMsgLock = new object();
        private readonly object _syncRoot = new object();

        public object SyncRoot {
            get { return _syncRoot; }
        } 


        #endregion Private members

        #region Properties

        public string GameName {
            get { return _gameName; }
            set { _gameName = value; }
        }

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
            set { _data.WinNo = value; }
        }

        public int LossNo {
            get { return _data.LossNo; }
            set { _data.LossNo = value; }
        }

        public event ConnectionLostDelegate OnConnectionLost {
            add {
                lock (_clMsgLock) {
                    _onConnectionLost += value;
                }
            }
            remove {
                lock (_clMsgLock) {
                    _onConnectionLost -= value;
                }
            }
        }

        public event ReceiveMessageDelegate OnReceiveMessage {
            add {
                lock (_rcvMsgLock) {
                    _onReceiveMessage += value;
                }
            }
            remove 
            {
                lock (_rcvMsgLock) {
                    _onReceiveMessage -= value;
                }
            }
        }

        #endregion Properties

        public Player(short id, TcpClient client) {
            _id = id;
            _readStream = new BinaryReader(client.GetStream());
            _writeStream = new BinaryWriter(client.GetStream());
            _isDisconnected = false;
            _rcvThread = new Thread(new ThreadStart(ReceiveMessages));
            
        }

        

        public void Start() {
            _rcvThread.Start();
        }

        public void SetData(PlayerData pd) {
            if (pd != null) {
                _data = pd;
                pd.Id = _id;
            }
        }

        public void Stop() {
            _writeStream.Close();
            _rcvThread.Join();
        }

        public void SendMessage(Message msg) {
            try {
                if (!_isDisconnected) {
                    msg.Serialize(_writeStream);
                }
            }
            catch (Exception) {
                ExecuteOnConnectionLost();
            }
        }

        private void ExecuteOnConnectionLost() {
            if (!_isDisconnected) {
                _isDisconnected = true;
                lock (_clMsgLock) {
                    if (_onConnectionLost != null)
                        _onConnectionLost(this, new ConnectionLostEventArgs());
                }
            }
        }

        private void ExecuteOnReceiveMessage(Message msg) {
            lock (_rcvMsgLock) {
                if (_onReceiveMessage != null)
                _onReceiveMessage(this, new ReceiveMessageEventArgs(msg));
            }
        }
        public void ReceiveMessages() {
            byte type;
            while (true) {
                try {
                    type = _readStream.ReadByte();
                }
                catch (Exception) {
                    ExecuteOnConnectionLost();
                    return;
                    
                }
                InfoLog.WriteInfo("Player sent message");
                Message msg = MessageFactory.Create((MessageType)type);
                InfoLog.WriteInfo("Received message with type " + type, EPrefix.MessageReceivedInfo);
                if (null == msg) {
                    InfoLog.WriteInfo("Received unknown message", EPrefix.MessageReceivedInfo);
                    continue;
                }
                try {
                    msg.Deserialize(_readStream);
                }
                catch (Exception) {
                    ExecuteOnConnectionLost();
                    return;
                }

                msg.PlayerId = Id;
                ExecuteOnReceiveMessage(msg);
            }
        }



        #region IPlayerID Members

        public short GetID() {
            return this.Id;
        }



        #endregion
    }
}
