using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Messaging
{
    public class TurnAskMessage : Message
    {
        public TurnAskMessage()
            : base(MessageType.TurnAsk)
        { }
    }
}
