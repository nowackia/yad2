using System;
using System.Collections.Generic;
using System.Text;
using Client.MessageManagement;

namespace Client.Engine.Messaging
{
    public abstract class MessageHandler
    {
        Queue<Message> msgQueue;
        public abstract void ProcessMessage();
    }
}
