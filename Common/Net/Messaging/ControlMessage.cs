using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    public class ControlMessage : Message
    {
        public ControlMessage()
            : base(MessageType.Control)
        { }

        public ControlMessage(MessageType msgType)
            : base(msgType)
        { }

        public void CreateResponse()
        { }
    }
}
