using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Engine.Messaging
{
    public abstract class MessageHandler
    {
        Queue<Message.Message> msgQueue;
        public abstract void ProcessMessage();
    }
}
