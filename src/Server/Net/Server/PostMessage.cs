using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.General.Messaging;

namespace Server.Net.Server
{
    class PostMessage
    {
        public const int BroadCastMessage = -1;
        private int _priority;
        private int _recipient;
        private Message _message;

        public int Priority {
            get { return _priority; }
            set { _priority = value; }
        }

        public int Recipient {
          get { return _recipient; }
          set { _recipient = value; }
        }
        
        public Message Message {
          get { return _message; }
          set { _message = value; }
        }
    }
}
