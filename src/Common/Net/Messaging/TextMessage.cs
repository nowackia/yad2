using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    /// <summary>
    /// message from chat(?)
    /// </summary>
    public class TextMessage : Message
    {
        private String text;

        public TextMessage()
            : base(MessageType.Text)
        { }

        public TextMessage(MessageType msgType)
            : base(msgType)
        { }

        public String Text
        {
            get { return text; }
            set { text = value; }
        }
    }
}
