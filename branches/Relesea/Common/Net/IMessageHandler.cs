using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Common {
    public interface IMessageHandler {
        void ProcessMessage(Message msg);
    }
}
