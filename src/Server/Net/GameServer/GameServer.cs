using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Server;
using System.Collections;
using Yad.Net.Common;
using Yad.Log.Common;

namespace Yad.Net.GameServer.Server {

    public delegate void GameEndDelegate(object sender, GameEndEventArgs geea);

    class GameServer : BaseServer {

        #region Private members

        private ServerGameInfo _serverGameInfo;
        private Dictionary<short, GamePlayer> _gamePlayers;
        private event GameEndDelegate _onGameEnd;
        private short _minTurn = 0;

        #endregion

        #region Properties 

        public event GameEndDelegate OnGameEnd {
            add    { _onGameEnd += value; }
            remove { _onGameEnd -= value; }
        }

        public string Name {
            get{
                return _serverGameInfo.Name;
            }
        }

        #endregion

        #region Constructors

        public GameServer(ServerGameInfo sgInfo) : base() {

            _playerCollection = new Dictionary<short, Player>();
            _gamePlayers = new Dictionary<short, GamePlayer>();
            _serverGameInfo = sgInfo;
            
            lock (((ICollection)sgInfo.Players).SyncRoot) {
                foreach (IPlayerID pid in sgInfo.Players.Values) {
                    ServerPlayerInfo spi = pid as ServerPlayerInfo;
                    lock (((ICollection)_playerCollection).SyncRoot) {
                        _playerCollection.Add(new KeyValuePair<short, Player>(spi.Id, spi.Player));
                        _gamePlayers.Add(spi.Id, new GamePlayer());
                    }
                    
                }
            }

            MessageHandler = new GameMessageHandler(this);
            MessageHandler.SetSender(_msgSender);

            
            InfoLog.WriteInfo("Game server for game: " + sgInfo.Name + "created successfully", EPrefix.ServerInformation);
        }

        public void ChangeMessageHanders(Yad.Net.Server.Server server) {
            lock (((ICollection)_playerCollection).SyncRoot) {
                foreach (Player player in _playerCollection.Values) {
                    Player p = player as Player;
                    p.OnReceiveMessage -= new ReceiveMessageDelegate(server.MessageHandler.OnReceivePlayerMessage);
                    p.OnReceiveMessage += new ReceiveMessageDelegate(this.MessageHandler.OnReceivePlayerMessage);
                    p.OnConnectionLost += new ConnectionLostDelegate(this.OnConnectionLost);
                }
            }
        }

        #endregion

        #region Public Methods

        public void Start() {
            StartMessageProcessing();
            InfoLog.WriteInfo("Game server for game: " + _serverGameInfo.Name + "started successfully", EPrefix.ServerInformation);
        }

        public void Stop() {
            StopMessageProcessing();
            InfoLog.WriteInfo("Game server for game: " + _serverGameInfo.Name + "stopped successfully", EPrefix.ServerInformation);
        }

        public void OnConnectionLost(object sender, ConnectionLostEventArgs clea) {

        }

        #endregion
    }
}
