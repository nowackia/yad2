using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.General.Messaging;

namespace Yad.Net.General {
    interface IMessageSender {
        void MessagePost(Message msg, int recipient);
    }
}
