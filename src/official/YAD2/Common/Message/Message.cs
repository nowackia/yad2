using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Message
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

    }
}
