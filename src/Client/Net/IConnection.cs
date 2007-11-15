using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Client.Net {
	public interface IConnection {
		void SendMessage(Message message);
        void InitConnection(string host, int port);
        void CloseConnection();
	}
}
