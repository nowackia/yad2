using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Client {
    public interface IConnection
    {
        void InitConnection(string host, int port);
        void CloseConnection();
        void SendMessage(Message message);
    }
}
