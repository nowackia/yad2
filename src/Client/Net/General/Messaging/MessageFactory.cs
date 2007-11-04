using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging {
    class MessageFactory {
        public static Message Create(MessageType msgType) {
            switch (msgType) {
                case MessageType.Move:
                    return new MoveMessage();
                    break;
            }
            return null;
        }
    }
}
