using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;
using Yad.Engine.Server;
using Yad.Properties;
using Yad.Net.Server;
using Yad.Net.Messaging;

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
            InfoLog.WriteInfo(string.Format(Resources.GameProcessStringFormat, item.Type.ToString(), 
                _gameServer.Name, _gameServer.GetPlayer(item.SenderId).Login),
                EPrefix.GameMessageProccesing);
            switch (item.Type) {
                case MessageType.TurnAsk:
                    ProcessTurnAsk((MessageTurnAsk)item);
                    break;
                case MessageType.Move:
                case MessageType.Destroy:
                case MessageType.CreateUnit:
                case MessageType.Build:
                case MessageType.Harvest:
                case MessageType.Attack:
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

        private void ProcessGameEnd(GameEndMessage item) {
            _gameServer.Simulation.SetEndGame(item.SenderId, item.HasWon);
            if (_gameServer.Simulation.HasGameEnded())
                _gameServer.StopGameServer();
        }

        private void ProcessGameMessage(GameMessage gameMessage) {
            InfoLog.WriteInfo("Processing message: " + gameMessage.Type + 
                " from player: " + _gameServer.GetPlayer(gameMessage.SenderId).Login, 
                EPrefix.GameMessageProccesing);
			gameMessage.IdTurn = this._gameServer.Simulation.GetPlayerTurn(gameMessage.SenderId) + _gameServer.Simulation.Delta + 1;
            _gameServer.Simulation.AddMessage(gameMessage);
            this.SendMessage(gameMessage, -1);
        }

        private void ProcessTurnAsk(MessageTurnAsk turnAskMessage) {
            InfoLog.WriteInfo("Processing Turn Ask message from player: " +
                 _gameServer.GetPlayer(turnAskMessage.SenderId).Login,
               EPrefix.GameMessageProccesing);
            if (_gameServer.Simulation.GetPlayerTurn(turnAskMessage.SenderId) < _gameServer.Simulation.GetMinTurn() + _gameServer.Simulation.Delta - 1)
                IncreaseTurn(turnAskMessage.SenderId);
            else
                WaitPlayer(turnAskMessage.SenderId);
        }

        private void IncreaseTurn(short id) {
            int minTurnBefore = _gameServer.Simulation.GetMinTurn();
            _gameServer.Simulation.IncPlayerTurn(id);
            int minTurn = _gameServer.Simulation.GetMinTurn();
            SendMessage(MessageFactory.Create(MessageType.DoTurn), id);
            if (minTurn != minTurnBefore) {
                short[] stoppedWaiting = _gameServer.Simulation.StopWaiting();
                for (int i = 0; i < stoppedWaiting.Length; ++i){
                    _gameServer.Simulation.IncPlayerTurn(stoppedWaiting[i]);
                    SendMessage(MessageFactory.Create(MessageType.DoTurn), stoppedWaiting[i]);
                }
            }
        }

        private void WaitPlayer(short id) {
            _gameServer.Simulation.SetWaiting(id);
        }
    }
}
