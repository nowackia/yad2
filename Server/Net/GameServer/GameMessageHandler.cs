using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;
using Yad.Properties;
using Yad.Net.Server;
using Yad.Net.Messaging;
using Yad.Net.Utils;

namespace Yad.Net.GameServer.Server {

    /// <summary>
    /// Class that handles game messages
    /// </summary>
    class GameMessageHandler : MessageHandler {

        #region Private members

        private GameServer _gameServer;

        #endregion

        public GameMessageHandler(GameServer gameServer)
            : base() {
            _gameServer = gameServer;
        }
        /// <summary>
        /// Main processing method
        /// </summary>
        /// <param name="item">Message to process</param>
        public override void ProcessItem(Message item) {
            Player p = _gameServer.GetPlayer(item.SenderId);
            if (null == p){
                InfoLog.WriteError("Processing message from not connected player!", EPrefix.GameMessageProccesing);
                return;
            }
            else
                InfoLog.WriteInfo(string.Format(Resources.GameProcessStringFormat, item.Type.ToString(),
                    _gameServer.Name, p.Login), EPrefix.GameMessageProccesing);
            switch (item.Type) {
                case MessageType.TurnAsk:
                    ProcessTurnAsk((MessageTurnAsk)item);
                    break;
                case MessageType.Pause:
                    ProcessPause(item.SenderId);
                    break;
                case MessageType.Resume:
                    ProcessResume(item.SenderId);
                    break;
                case MessageType.Move:
                case MessageType.Destroy:
                case MessageType.CreateUnit:
                case MessageType.Build:
                case MessageType.Harvest:
                case MessageType.Attack:
                case MessageType.DeployMCV:
                case MessageType.BuildUnitMessage:
                    ProcessGameMessage((GameMessage)item);
                    break;
                case MessageType.EndGame:
                    ProcessGameEnd((GameEndMessage)item);
                    break;
                default:
                    InfoLog.WriteInfo("Unknown message to process: " + item.Type, EPrefix.GameMessageProccesing);
                    break;
            }
        }

        private void ProcessResume(short p) {
            InfoLog.WriteInfo("Resuming game...");
            List<short> paused = _gameServer.PauseCtrl.ResumeGame();
            if (paused != null) {
                InfoLog.WriteInfo("Paused count: " + paused.Count);
                foreach (short playerNo in paused) {
                    int playerTurn = _gameServer.Simulation.GetPlayerTurn(playerNo);
                    ProcessTurnChange(playerTurn, playerNo, PauseAction.Resume);
                }
            }
        }

        private void ProcessPause(short id) {
            InfoLog.WriteInfo("Setting pause...");
            int turnid = _gameServer.Simulation.GetPlayerTurn(id) + _gameServer.Simulation.Delta + 1;
            _gameServer.PauseCtrl.SetPause(turnid, id);
            InfoLog.WriteInfo("for turn: " + turnid);
        }

        private void ProcessGameEnd(GameEndMessage item) {
            _gameServer.Simulation.SetEndGame(item.SenderId, item.HasWon);
            if (_gameServer.Simulation.HasGameEnded()) {
                InfoLog.Write("Game " + _gameServer.Name + " has finished.");
                _gameServer.EndServerGame();
                //_gameServer.StopGameServer();
            }
        }

       public override void OnReceivePlayerMessage(object sender, ReceiveMessageEventArgs args)
        {
            /*if (args.Message.Type == MessageType.TurnAsk) {
                ///Player p = sender as Player;
                //ProcessFast(p);
            }
            else*/
                base.OnReceivePlayerMessage(sender, args);
        }

        private void ProcessGameMessage(GameMessage gameMessage) {
            if (gameMessage.Type == MessageType.BuildUnitMessage) {
                InfoLog.WriteInfo("Server received buildUnitMessage: " + gameMessage.ToString(), EPrefix.Test);
            }
            if (gameMessage.Type == MessageType.Build) {
                InfoLog.WriteInfo("Server received buildMessage: " + gameMessage.ToString(), EPrefix.Test);
            }
            InfoLog.WriteInfo("Processing message: " + gameMessage.Type + 
                " from player: " + _gameServer.GetPlayer(gameMessage.SenderId).Login, 
                EPrefix.GameMessageProccesing);
			gameMessage.IdTurn = this._gameServer.Simulation.GetPlayerTurn(gameMessage.SenderId) + _gameServer.Simulation.Delta + 1;
            _gameServer.Simulation.AddMessage(gameMessage);
            this.SendMessage(gameMessage, -1);
        }
        private void ProcessFast(Player p) {
            if (_gameServer.Simulation.GetPlayerTurn(p.Id) < _gameServer.Simulation.GetMinTurn() + _gameServer.Simulation.Delta - 1){
                IncreaseTurn(p);
            }
            else {
                WaitPlayer(p.Id);
            }
        }

        private void IncreaseTurn(Player p) {
            int minTurnBefore = _gameServer.Simulation.GetMinTurn();
            _gameServer.Simulation.IncPlayerTurn(p.Id);
            int minTurn = _gameServer.Simulation.GetMinTurn();
            // if this is slowest player then tell him to speed up
            DoTurnMessage dtm = (DoTurnMessage)MessageFactory.Create(MessageType.DoTurn);
            if (minTurn != minTurnBefore) {
                dtm.SpeedUp = true;
            }
            //
            p.SendMessage(dtm);
            if (minTurn != minTurnBefore) {
                ResumeWaitingPlayers();
            }
        }

        private void ResumeWaitingPlayers()
        {
            short[] stoppedWaiting = _gameServer.Simulation.StopWaiting();
            for (int i = 0; i < stoppedWaiting.Length; ++i)
            {
                _gameServer.Simulation.IncPlayerTurn(stoppedWaiting[i]);
                SendMessage(MessageFactory.Create(MessageType.DoTurn), stoppedWaiting[i]);
            }
        }
        
        private void ProcessTurnAsk(MessageTurnAsk turnAskMessage) {
            InfoLog.WriteInfo("Processing Turn Ask message from player: " +
                 _gameServer.GetPlayer(turnAskMessage.SenderId).Login,
               EPrefix.GameMessageProccesing);
            int playerTurn = _gameServer.Simulation.GetPlayerTurn(turnAskMessage.SenderId);
            short senderId = turnAskMessage.SenderId;
            
            if (!TurnAskPause(playerTurn, senderId))
                ProcessTurnChange(playerTurn, senderId, PauseAction.None);
        }

        private bool TurnAskPause(int playerTurn, short senderId) {
            if (_gameServer.PauseCtrl.IsPaused) {
                if (_gameServer.PauseCtrl.PauseTurn == playerTurn) {
                    DoTurnMessage dtMsg = (DoTurnMessage)MessageFactory.Create(MessageType.DoTurn);
                    dtMsg.Pause = (byte)PauseAction.Pause;
                    InfoLog.WriteInfo("Adding player to pause queue!");
                    _gameServer.PauseCtrl.AddPausedPlayer(senderId);
                    SendMessage(dtMsg, senderId);
                    return true;
                }
            }
            return false;
        }

        private void ProcessTurnChange(int playerTurn, short senderId, PauseAction pauseAction) {
            if (playerTurn < _gameServer.Simulation.GetMinTurn() + _gameServer.Simulation.Delta - 1)
                IncreaseTurn(senderId, pauseAction);
            else
                WaitPlayer(senderId);
        }

        private void IncreaseTurn(short id, PauseAction pauseAction) {
            InfoLog.WriteInfo("Increasing turn for player: " + id, EPrefix.GameMessageProccesing);
            int minTurnBefore = _gameServer.Simulation.GetMinTurn();
            _gameServer.Simulation.IncPlayerTurn(id);
            int minTurn = _gameServer.Simulation.GetMinTurn();
			// if this is slowest player then tell him to speed up
			DoTurnMessage dtm = (DoTurnMessage)MessageFactory.Create(MessageType.DoTurn);
            dtm.Pause = (byte)pauseAction;
			if (minTurn != minTurnBefore) {
				dtm.SpeedUp = true;
			}
			//
            SendMessage(dtm, id);
            if (minTurn != minTurnBefore) {
                InfoLog.WriteInfo("Waking waiting players", EPrefix.GameMessageProccesing);
                short[] stoppedWaiting = _gameServer.Simulation.StopWaiting();
                InfoLog.WriteInfo("Players to wake: " + stoppedWaiting.ToString(), EPrefix.GameMessageProccesing);
                for (int i = 0; i < stoppedWaiting.Length; ++i){
                    _gameServer.Simulation.IncPlayerTurn(stoppedWaiting[i]);
                    SendMessage(MessageFactory.Create(MessageType.DoTurn), stoppedWaiting[i]);
                }
            }
        }

        private void WaitPlayer(short id) {
            InfoLog.WriteInfo("Adding player: " + id + "to waiting queue", EPrefix.GameMessageProccesing);
            _gameServer.Simulation.SetWaiting(id);
        }
    }
}
