using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using System.Collections;

namespace Yad.Net.Server {
    class BaseServer : IPlayerProvider {

        protected MessageHandler _msgHandler;
        protected ServerMessageSender _msgSender;
        protected IDictionary<int, Player> _playerCollection;

        public BaseServer() {
            _msgSender = new ServerMessageSender();
            _msgSender.AddProvider(this);
        }

        protected void StartMessageProcessing() {
            _msgHandler.Start();
            _msgSender.Start();
        }

        protected void StopMessageProcessing() {
            _msgHandler.Stop();
            _msgHandler.Join();
            _msgSender.Stop();
            _msgSender.Join();
        }

        public void AddPlayer(int key, Player p) {
            lock((((ICollection)_playerCollection).SyncRoot)){
                _playerCollection.Add(new KeyValuePair<int,Player>(key, p));   
            }
        }

        #region IPlayerProvider Members

        public virtual Player GetPlayer(int id) {
            if (_playerCollection != null)
                lock (((ICollection)(_playerCollection)).SyncRoot)
                    if (_playerCollection.ContainsKey(id))
                        return _playerCollection[id];
            return null;
        }

        public virtual IEnumerator<KeyValuePair<int,Player>> GetPlayers() {
            if (_playerCollection != null)
                lock (((ICollection)(_playerCollection)).SyncRoot)
                    return _playerCollection.GetEnumerator();
            return null;
        }

        #endregion
    }
}
