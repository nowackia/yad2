using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Client {
	public interface IConnection {
		void SendMessage(Message message);
        void InitConnection(string host, int port);
        void CloseConnection();
	}
}
