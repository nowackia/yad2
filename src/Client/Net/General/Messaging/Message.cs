using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Yad.Net.General.Messaging
{
    public abstract class Message
    {
        private MessageType type;
        private int userId;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public MessageType Type
        {
            get { return type; }
        }

        public abstract void Deserialize(StreamReader reader);
        public abstract void Serialize(StreamWriter writer);

    }
}
