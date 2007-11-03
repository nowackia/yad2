using System;
using System.Collections.Generic;
using System.Text;
using Client.MessageManagement;

namespace Yad.Net.General {
    interface IMessageHandler {
        void ProcessMessage(Message msg);
    }
}
