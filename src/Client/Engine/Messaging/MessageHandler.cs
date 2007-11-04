using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.General.Messaging;

namespace Client.Engine.Messaging
{
    public abstract class MessageHandler
    {
        Queue<Message> msgQueue;
        public abstract void ProcessMessage();
    }
}
