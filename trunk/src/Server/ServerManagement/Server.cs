using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Client.Log;
using System.Collections;
using System.Windows.Forms;
using Server.Net.General.Server;

namespace Server.ServerManagement {
    class Server {

        private int _portNumber;
        private TcpListener _listener;

        private Dictionary<int, Player> _playersUnlogged;
        private Dictionary<int, Player> _playersLogged;
        private Chat _chat;

        internal Chat Chat {
            get { return _chat; }
            set { _chat = value; }
        }

        internal Dictionary<int, Player> PlayersLogged {
            get { return _playersLogged; }
            set { _playersLogged = value; }
        }

        internal Dictionary<int, Player> PlayersUnlogged {
            get { return _playersUnlogged; }
            set { _playersUnlogged = value; }
        }

        private MenuMessageHandler _menuMsgHandler;
        private bool _serverEnd = false;
        private int playerID = 1;

        public Server(int PortNumber) {

            _portNumber = PortNumber;
            _playersUnlogged = new Dictionary<int, Player>();
            _playersLogged = new Dictionary<int, Player>();
            _listener = new TcpListener(_portNumber);
            _listener.Start();
            InfoLog.WriteInfo("Server listnening started successfully", EPrefix.ServerInformation);
            _menuMsgHandler = new MenuMessageHandler(this);
            _menuMsgHandler.Start();
            InfoLog.WriteInfo("Server menu message handling started successfully", EPrefix.ServerInformation);

        }

        public void Start() {
            while (!_serverEnd) {
                    AcceptConnections();
            }
            _menuMsgHandler.EndThread();
            
        }

        public void Stop() {
            _listener.Stop();
        }


        public void RemovePlayer(Player player) {
            InfoLog.WriteInfo("Player " + player.Id + " has disconnected", EPrefix.ServerInformation);
            if (player.State == Yad.Net.General.MenuState.Unlogged)
                _playersUnlogged.Remove(player.Id);
            else {
                _playersLogged.Remove(player.Id);
            }
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
            InfoLog.WriteInfo("Server accepted new client");
            Player player = new Player(playerID + 1, client);
            player.OnReceiveMessage += new ReceiveMessageDelegate(_menuMsgHandler.AddMessage);
            player.OnConnectionLost += new ConnectionLostDelegate(RemovePlayer);
            player.Start();
            lock(((ICollection)_playersUnlogged).SyncRoot)
                _playersUnlogged.Add(++playerID, player);

        }

        
        


    }
}
