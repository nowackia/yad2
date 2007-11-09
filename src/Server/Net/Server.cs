using Yad.Net.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Windows.Forms;

using Yad.Log;
using Yad.Log.Common;

namespace Yad.Net.Server {
        class Server : BaseServer {

            #region Private members 

            private int _portNumber;
            private TcpListener _listener;
            private Dictionary<int, Player> _playersUnlogged;
            private Chat _chat;
            private bool _serverEnd = false;
            private static short playerID = 0;

            #endregion 

            #region Properites

            internal Chat Chat {
                get { return _chat; }
                set { _chat = value; }
            }

            internal Dictionary<int, Player> PlayersUnlogged {
                get { return _playersUnlogged; }
                set { _playersUnlogged = value; }
            }

            #endregion

            #region Constructors

            public Server(int PortNumber)
                : base() {

                _portNumber = PortNumber;
                _listener = new TcpListener(_portNumber);
                _listener.Start();
                InfoLog.WriteInfo("Server listnening started successfully", EPrefix.ServerInformation);

                _playersUnlogged = new Dictionary<int, Player>();
                _playerCollection = new Dictionary<int, Player>();

                _msgHandler = new MenuMessageHandler(this);
                _msgHandler.SetSender(_msgSender);
                StartMessageProcessing();

                _chat = new Chat(_msgSender);
                InfoLog.WriteInfo("Server menu message handling started successfully", EPrefix.ServerInformation);

            }

            #endregion

            #region Public Methods

            public Player GetPlayerUnlogged(int key) {
                lock (((ICollection)_playersUnlogged).SyncRoot)
                    if (_playersUnlogged.ContainsKey(key))
                        return _playersUnlogged[key];
                return null;
            }

            public void RemovePlayerUnlogged(int key) {
                lock (((ICollection)_playerCollection).SyncRoot)
                    _playerCollection.Remove(key);
            }

            public void Start() {
                while (!_serverEnd) {
                    AcceptConnections();
                }
                StopPlayers();
                StopMessageProcessing();

            }

            public void StopPlayers() {
                lock (((ICollection)_playersUnlogged).SyncRoot) {
                    foreach (Player p in _playersUnlogged.Values) {
                        p.Stop();
                    }
                }
                lock (((ICollection)_playerCollection).SyncRoot) {
                    foreach (Player p in _playerCollection.Values) {
                        p.Stop();
                    }
                }
            }
            public void Stop() {
                _listener.Stop();
            }

            public override Player GetPlayer(int id) {
                Player p = base.GetPlayer(id);
                if (null == p)
                    if (_playersUnlogged != null)
                        return this.GetPlayerUnlogged(id);
                return p;
            }

            public void OnConnectionLost(object sender, ConnectionLostEventArgs args) {
                Player player = sender as Player;
                InfoLog.WriteInfo("Player " + player.Id + " has disconnected", EPrefix.ServerInformation);
                if (player.State == MenuState.Unlogged)
                    _playersUnlogged.Remove(player.Id);
                else
                    _playerCollection.Remove(player.Id);

            }

            public void AcceptConnections() {
                TcpClient client = null;
                try {
                    client = _listener.AcceptTcpClient();
                }
                catch (Exception) {
                    _serverEnd = true;
                    return;
                }
                short id = GenerateUniqueID();
                InfoLog.WriteInfo("Server accepted new client");
                Player player = new Player(id, client);
                player.OnReceiveMessage += new ReceiveMessageDelegate(_msgHandler.OnReceivePlayerMessage);
                player.OnConnectionLost += new ConnectionLostDelegate(OnConnectionLost);
                player.Start();
                lock (((ICollection)_playersUnlogged).SyncRoot)
                    _playersUnlogged.Add(id, player);

            }

            #region Static Methods

            public static short GenerateUniqueID() {
                return ++playerID;
            }

            #endregion 

            #endregion

    }
}
