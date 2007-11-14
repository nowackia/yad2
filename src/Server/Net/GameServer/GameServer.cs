using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Server;
using System.Collections;
using Yad.Net.Common;
using Yad.Log.Common;

namespace Yad.Net.GameServer.Server {

    public delegate void GameEndDelegate(object sender, GameEndEventArgs geea);

    public class GameServer : BaseServer {

        #region Private members

        private ServerGameInfo _serverGameInfo;
        private event GameEndDelegate _onGameEnd;

        #endregion

        #region Properties 

        public event GameEndDelegate OnGameEnd {
            add    { _onGameEnd += value; }
            remove { _onGameEnd -= value; }
        }

        #endregion

        #region Constructors

        public GameServer(ServerGameInfo sgInfo) : base() {

            _serverGameInfo = sgInfo;

            lock (((ICollection)sgInfo.Players).SyncRoot) {
                foreach (IPlayerID pid in sgInfo.Players.Values) {
                    ServerPlayerInfo spi = pid as ServerPlayerInfo;
                    lock (spi) {
                        _playerCollection.Add(new KeyValuePair<short, Player>(spi.Id, spi.Player));
                    }
                }
            }

            _msgHandler = new GameMessageHandler(this);
            _msgHandler.SetSender(_msgSender);

            
            InfoLog.WriteInfo("Game server for game: " + sgInfo.Name + "created successfully", EPrefix.ServerInformation);
        }

        #endregion

        #region Public Methods

        public void Start() {
            StartMessageProcessing();
            InfoLog.WriteInfo("Game server for game: " + _serverGameInfo.Name + "started successfully", EPrefix.ServerInformation);
        }

        public void Stop() {
            StopMessageProcessing();
        }

        #endregion
    }
}
