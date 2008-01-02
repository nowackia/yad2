using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Client;
using Yad.Log.Common;

namespace CommunicationTest {
    public class TurnAskTimeCounter {
        List<DateTime> _sendTimeList = new List<DateTime>();
        object listLock = new object();
        public void OnMessageSend(object sender, MessageEventArgs e) {
            if (e.message.Type == Yad.Net.Messaging.Common.MessageType.TurnAsk) {
                lock (listLock) {
                    _sendTimeList.Add(DateTime.Now);
                }
            }
        }
        public void OnMessageRecieve(object sender, MessageEventArgs e) {
            if (e.message.Type == Yad.Net.Messaging.Common.MessageType.DoTurn) {
                DateTime old;
                lock (listLock) {
                    old = _sendTimeList[0];
                    _sendTimeList.RemoveAt(0);
                }
                TimeSpan ts = DateTime.Now - old;
                InfoLog.WriteInfo("TurnAsk do DoTurn: " + ts.Milliseconds + " ms");
            }
        }
    }
}
