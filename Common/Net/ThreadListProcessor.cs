using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Yad.Net.Common {
    public abstract class ThreadListProcessor<T> : ListProcessor<T> {
        private Thread _thread;

        public ThreadListProcessor() {
            _thread = new Thread(new ThreadStart(Process));
        }

        public Thread Thread {
            get { return _thread; }
            set { _thread = value; }
        }

        public void Start() {
            _thread.Start();
        }

        public void Join() {
            _thread.Join();
        }

        public void Stop() {
            base.EndThread();
        }

    }
}
