using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging
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

        public String Text
        {
            get { return text; }
            set { text = value; }
        }


    }
}
