using System;
using System.Collections.Generic;
using System.Text;
using Yad.Log.Common;
using System.Collections;

namespace Yad.Net.Utils
{
    public class TurnAskProcessCounter
    {
        List<DateTime> _receivedTime;

        public TurnAskProcessCounter()
        {
            _receivedTime = new List<DateTime>();
        }
        public void Set()
        {
            lock (((ICollection)_receivedTime).SyncRoot)
                _receivedTime.Add(DateTime.Now);
        }

        public void Unset(string name)
        {

            DateTime old = DateTime.Now;
            lock (((ICollection)_receivedTime).SyncRoot)
            {
                if (_receivedTime.Count > 0) {
                    old = _receivedTime[0];
                    _receivedTime.RemoveAt(0);
                }
            }
            TimeSpan ts = DateTime.Now - old;
            InfoLog.WriteInfo("Processing of TurnAsk for player: " + name + " lasted: " + ts.Milliseconds + " ms ",EPrefix.Performance);
        }

    }
}
