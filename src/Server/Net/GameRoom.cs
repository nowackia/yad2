using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using System.Collections;

namespace Yad.Net.Server {
    public abstract class GameRoom {
        protected Dictionary<short, IPlayerID> _players;
        protected IMessageSender _sender;
        public Dictionary<short, IPlayerID> Players {
            get { return _players; }
            set { _players = value; }
        }

        

        public GameRoom(IMessageSender sender) {
            _sender = sender;
            _players = new Dictionary<short, IPlayerID>();
        }
        public void AddPlayer(IPlayerID player) {
            lock (((ICollection)_players).SyncRoot) {
                IPlayerID playerId = TransformInitialy(player);
                _players.Add(playerId.GetID(), playerId);
                SendAddPlayer(playerId);
                SendListPlayer(playerId);
            }
        }

        public void RemovePlayer(IPlayerID playerID) {
            lock (((ICollection)_players).SyncRoot) {
                _players.Remove(playerID.GetID());
            }
            SendRemovePlayer(playerID);
        }

        
        protected void SendRemovePlayer(IPlayerID playerID) {
            SendMessage(CreateRemoveMessage(playerID), playerID.GetID());
        }
        protected void SendListPlayer(IPlayerID playerID) {
            SendMessage(CreateListMessage(playerID), playerID.GetID());
        }


        protected void SendAddPlayer(IPlayerID playerID) {
            Message msg = CreateAddMessage(playerID);
            BroadcastExcl(msg, playerID.GetID());
        }

        protected void SendMessage(Message msg, short id) {
            _sender.MessagePost(msg, id);
        }

        protected void BroadcastExcl(Message msg, int id) {
            lock (((ICollection)_players).SyncRoot)
                foreach (IPlayerID pid in _players.Values)
                    if (pid.GetID() != id)
                        _sender.MessagePost(msg, pid.GetID());
        }

        protected abstract Message CreateAddMessage(IPlayerID playerID);
        protected abstract Message CreateListMessage(IPlayerID playerID);
        protected abstract Message CreateRemoveMessage(IPlayerID playerID);
        protected abstract IPlayerID TransformInitialy(IPlayerID player);



    }
}
