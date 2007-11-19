using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Yad.Net.Common;
using Yad.Engine.Common;

namespace Yad.Net.GameServer.Server {
    public class MockServerSimulation : IServerSimulation {
        
        private Dictionary<short, GamePlayer> _gamePlayers;
        static int _Delta = Yad.Properties.Common.Settings.Default.Delta;

        public int Delta {
            get { return _Delta; }
        }
        public MockServerSimulation()  {
            _gamePlayers = new Dictionary<short, GamePlayer>();
        }

        public void AddPlayer(short id, PlayerData pd) {
            _gamePlayers.Add(id, new GamePlayer(pd));
        }

        #region IServerSimulation Members

        public int GetPlayerTurn(short id) {
            lock (((ICollection)_gamePlayers).SyncRoot)
                return _gamePlayers[id].TurnNo;
        }

        public int GetMinTurn() {
            int min = int.MaxValue;
            lock (((ICollection)_gamePlayers).SyncRoot) {
                foreach (GamePlayer gp in _gamePlayers.Values) {
                    min = Math.Min(min, gp.TurnNo);
                }
            }
            return min;
        }

        public void IncPlayerTurn(short id) {
            lock (((ICollection)_gamePlayers).SyncRoot) {
                if (_gamePlayers.ContainsKey(id))
                    _gamePlayers[id].TurnNo++;
            }
        }

        public bool IsPlayerWaiting(short id) {
            lock (((ICollection)_gamePlayers).SyncRoot) {
                if (_gamePlayers.ContainsKey(id))
                    return _gamePlayers[id].IsWaiting;
                return false;
            }
        }

        public void SetWaiting(short id) {
            lock (((ICollection)_gamePlayers).SyncRoot) {
                if (_gamePlayers.ContainsKey(id))
                    _gamePlayers[id].IsWaiting = true;
            }
        }

        public short[] StopWaiting() {
            List<short> waitingList = new List<short>();
            lock (((ICollection)_gamePlayers).SyncRoot) {
                foreach (KeyValuePair<short, GamePlayer> kgp in _gamePlayers)
                    if (kgp.Value.IsWaiting) {
                        waitingList.Add(kgp.Key);
                        kgp.Value.IsWaiting = false;
                    }
            }
            return waitingList.ToArray();
        }

        public void AddMessage(Yad.Net.Messaging.Common.Message msg) {

        }

        public void SetEndGame(short id, bool hasWon) {
            lock (((ICollection)_gamePlayers).SyncRoot)
                if (_gamePlayers.ContainsKey(id)) {
                    _gamePlayers[id].HasEnded = true;
                    _gamePlayers[id].HasWon = hasWon;
                }
        }

        public bool HasGameEnded() {
            bool result =  true;
            lock (((ICollection)_gamePlayers).SyncRoot)
                foreach(GamePlayer gp in _gamePlayers.Values)
                    if (!gp.HasEnded) {
                        result = false;
                        break;
                    }
            return result;
        }

        public GamePlayer GetGamePlayer(short id) {
            lock (((ICollection)_gamePlayers).SyncRoot)
                if (_gamePlayers.ContainsKey(id))
                    return _gamePlayers[id];
            return null;
        }

        #endregion
        /*
        protected override void OnMessageBuild(Yad.Net.Messaging.Common.BuildMessage bm) {
            //throw new Exception("The method or operation is not implemented.");
        }

        protected override void onMessageMove(Yad.Net.Messaging.Common.MoveMessage gm) {
            //throw new Exception("The method or operation is not implemented.");
        }

        protected override void onMessageAttack(Yad.Net.Messaging.Common.AttackMessage am) {
            //throw new Exception("The method or operation is not implemented.");
        }

        protected override void onMessageDestroy(Yad.Net.Messaging.Common.DestroyMessage dm) {
            //throw new Exception("The method or operation is not implemented.");
        }

        protected override void onMessageHarvest(Yad.Net.Messaging.Common.HarvestMessage hm) {
            //throw new Exception("The method or operation is not implemented.");
        }

        protected override void onMessageCreate(Yad.Net.Messaging.Common.CreateUnitMessage cum) {
            //throw new Exception("The method or operation is not implemented.");
        }

        protected override void onInvalidMove(Yad.Board.Common.Unit unit) {
            //throw new Exception("The method or operation is not implemented.");
        }*/

        #region IServerSimulation Members


        public PlayerData[] GetPlayerData() {
            PlayerData[] arrPd = null;
            lock(((ICollection)_gamePlayers).SyncRoot)
            {
                arrPd = new PlayerData[_gamePlayers.Count];
                int index = 0;
                foreach(GamePlayer gp in _gamePlayers.Values)
                    arrPd[index++] = (PlayerData)gp.Clone();
            }
            return arrPd;
        }

        #endregion
    }
}
