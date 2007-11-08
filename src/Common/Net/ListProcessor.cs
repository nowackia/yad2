using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;


namespace Yad.Net.Common {
    public abstract class ListProcessor<T> {
        private ConsumerSync _sync;
        private List<T> _queue;

        public ListProcessor() {
            _sync = new ConsumerSync();
            _queue = new List<T>();
        }
        public void AddItem(T item) {
            lock (((ICollection)_queue).SyncRoot) {
                _queue.Add(item);
            }
            _sync.AddObject();
        }

        public void BeginThread() {
            _sync.ExitThreadEvent.Reset();
        }

        public void EndThread() {
            _sync.ExitThreadEvent.Set();
        }

        public void Process() {
            T item;
            while (WaitHandle.WaitAny(_sync.EventArray) != _sync.WaitIndex) {
                lock (((ICollection)_queue).SyncRoot) {
                    item = _queue[0];
                    _queue.RemoveAt(0);
                }
                ProcessItem(item);

            }
        }

        public abstract void ProcessItem(T item);
    }
}
