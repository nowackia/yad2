using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    /// <summary>
    /// message from chat(?)
    /// </summary>
    public class TextMessage : Message
    {
        private string text = string.Empty;

        public TextMessage()
            : base(MessageType.Text)
        { }

        public TextMessage(MessageType msgType)
            : base(msgType)
        { }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            base.WriteString(text, writer);
        }

        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            text = base.ReadString(reader);
        }
    }
}
