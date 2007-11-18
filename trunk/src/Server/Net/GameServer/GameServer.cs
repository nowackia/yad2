using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Server;
using System.Collections;
using Yad.Net.Common;
using Yad.Log.Common;
using Yad.Net.Messaging.Common;
using Yad.Properties;
using Yad.Database.Server;
using System.Threading;
using Yad.Properties.Server;

namespace Yad.Net.GameServer.Server {

    public delegate void GameEndDelegate(object sender, GameEndEventArgs geea);

    class GameServer : BaseServer {

        #region Private members

        private ServerGameInfo _serverGameInfo;
        private event GameEndDelegate _onGameEnd;
        private object _gameEndEventLock = new object();
        private IServerSimulation _simulation;
        private Semaphore _gameEndSemaphore = new Semaphore(0, 1);
        private Thread _serverThread; 

        #endregion

        #region Properties 

        public IServerSimulation Simulation {
            get { return _simulation; }
            set { _simulation = value; }
        }

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
            _serverGameInfo = sgInfo;
            _simulation = new MockServerSimulation();
            
            lock (((ICollection)sgInfo.Players).SyncRoot) {
                foreach (IPlayerID pid in sgInfo.Players.Values) {
                    ServerPlayerInfo spi = pid as ServerPlayerInfo;
                    lock (((ICollection)_playerCollection).SyncRoot) {
                        _playerCollection.Add(new KeyValuePair<short, Player>(spi.Id, spi.Player));
                        _simulation.AddPlayer(spi.Id, (PlayerData)spi.Player.PlayerData.Clone());
                    }
                    
                }
            }

            MessageHandler = new GameMessageHandler(this);
            MessageHandler.SetSender(_msgSender);

            
            InfoLog.WriteInfo("Game server for game: " + sgInfo.Name + "created successfully", EPrefix.ServerInformation);
        }

        public void EnterMsgHandlerChange(Yad.Net.Server.Server server) {
            MessageHandlerChange(server, true);
        }
        
        public void LeaveMsgHandlerChange(Yad.Net.Server.Server server) {
            MessageHandlerChange(server, false);
        }

        public void EndGame() {
            PlayerData[] arrayPd = Simulation.GetPlayerData();
            for (int i = 0; i < arrayPd.Length; ++i)
                UpdatePlayerStats(arrayPd[i].Id);
            lock (_gameEndEventLock) {
                if (_onGameEnd != null)
                _onGameEnd(this, new GameEndEventArgs());
            }


        }

        #endregion

        #region Public Methods

        public void Start() {
            StartMessageProcessing();
            BroadcastInitMessage();
            InfoLog.WriteInfo("Game server for game: " + _serverGameInfo.Name + "started successfully", EPrefix.ServerInformation);
        }

        public void StartGameServer(){
            _serverThread = new Thread(new ThreadStart(GameServerProcess));
            _serverThread.Start();
        }

        public void StopGameServer() {
            _gameEndSemaphore.Release(1);
        }
        public void JoinGameServer() {
            _serverThread.Join();
        }
        private void GameServerProcess() {
            Start();
            _gameEndSemaphore.WaitOne();
            Stop();
            EndGame();

        }

        public void Stop() {
            StopMessageProcessing();
            InfoLog.WriteInfo("Game server for game: " + _serverGameInfo.Name + "stopped successfully", EPrefix.ServerInformation);
        }

        public void OnConnectionLost(object sender, ConnectionLostEventArgs clea) {
            Player player = sender as Player;
            //todo: opcjonalnie wywalic
            UpdatePlayerStats(player.Id);
            GameNumericMessage numMsg = (GameNumericMessage)MessageFactory.Create(MessageType.PlayerDisconnected);
            numMsg.Number = player.Id;
            numMsg.IdTurn = _simulation.GetPlayerTurn(player.Id) + _simulation.Delta;
            lock (((ICollection)_playerCollection).SyncRoot) {
                _playerCollection.Remove(player.Id);
            }
            _simulation.AddMessage(numMsg);
            _msgSender.BroadcastMessage(numMsg);
        }

        #endregion

        #region Private Methods

        private void MessageHandlerChange(Yad.Net.Server.Server server, bool enter) {
            lock (((ICollection)_playerCollection).SyncRoot) {
                foreach (Player player in _playerCollection.Values) {
                    Player p = player as Player;
                    if (enter) {
                        p.OnReceiveMessage -= new ReceiveMessageDelegate(server.MessageHandler.OnReceivePlayerMessage);
                        p.OnReceiveMessage += new ReceiveMessageDelegate(this.MessageHandler.OnReceivePlayerMessage);
                    }
                    else {
                        p.OnReceiveMessage -= new ReceiveMessageDelegate(server.MessageHandler.OnReceivePlayerMessage);
                        p.OnReceiveMessage += new ReceiveMessageDelegate(this.MessageHandler.OnReceivePlayerMessage);
                    }
                    p.OnConnectionLost += new ConnectionLostDelegate(this.OnConnectionLost);
                }
            }
        }
        private void UpdatePlayerStats(short id) {
            GamePlayer gp = Simulation.GetGamePlayer(id);
            if (null == gp)
                return;

            if (Settings.Default.DBAvail) {
                int winno = gp.HasWon && gp.HasEnded ? gp.WinNo + 1: gp.WinNo ;
                int lossno = (gp.HasEnded && !gp.HasWon) || !gp.HasEnded ? gp.LossNo + 1 : gp.LossNo;
                if (YadDB.UpdatePlayerStats(gp.Login, winno, lossno)) {
                    gp.WinNo = winno;
                    gp.LossNo = lossno;
                }
            }

            lock (((ICollection)_playerCollection).SyncRoot)
                if (_playerCollection.ContainsKey(id))
                        _playerCollection[id].SetData((PlayerData)gp.Clone());
        }
        private Message CreateInitMessage(){
            GameInitMessage giMsg = (GameInitMessage)MessageFactory.Create(MessageType.GameInit);
            int count = -1;
            PositionData[] arrPd = null;
            lock (((ICollection)_playerCollection).SyncRoot){
                count = _playerCollection.Count;
                short[] ids = new short[count];
                int index = 0;
                foreach (Player p in _playerCollection.Values)
                    ids[index++] = p.Id;
                arrPd = new PositionData[count];
                for (int i = 0; i < count; ++i) {
                    arrPd[i] = new PositionData(ids[i]);
                }
            }

            SetStartPositions(arrPd);
            Array.Sort(arrPd);
            giMsg.PositionData = arrPd;
            return giMsg;
        }

        private void SetStartPositions(PositionData[] PosData) {
            for (int i = 0; i < PosData.Length; ++i) {
                PosData[i].X = i;
                PosData[i].Y = i;
            }     
        }

        private void BroadcastInitMessage() {
            _msgSender.BroadcastMessage(CreateInitMessage());
        }

        #endregion
    }
}
