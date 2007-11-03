using System;
using System.Collections.Generic;
using System.Text;
using Client.MessageManagement;
using System.Collections;
using System.Threading;


namespace Yad.Net.General {
    abstract class MessageProcessor : IMessageHandler{
        private ConsumerSync _sync;
        private List<Message> _msgQueue;

        public MessageProcessor() {
            _sync = new ConsumerSync();
            _msgQueue = new List<Message>();
        }
        public void AddMessage(Message msg) {
            lock (((ICollection)_msgQueue).SyncRoot) {
                _msgQueue.Add(msg);
            }
            _sync.AddObject();
        }

        public void EndThread() {
            _sync.ExitThreadEvent.Set();
        }


        public void Process() {
            Message msg;
            while (WaitHandle.WaitAny(_sync.EventArray) != _sync.WaitIndex) {
                lock (((ICollection)_msgQueue).SyncRoot) {
                    msg = _msgQueue[0];
                    _msgQueue.RemoveAt(0);
                }
                ProcessMessage(msg);

            }
        }

        public void ProcessMessage(Message msg) {
        }

    }
}
