using System;
using System.Collections.Generic;
using System.Text;
using Client.MessageManagement;

namespace Client.Net {
    interface IMessageHandler {
        void ProcessMessage(Message msg);
    }
}
