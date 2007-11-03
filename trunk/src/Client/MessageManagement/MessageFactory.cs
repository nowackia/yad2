using System;
using System.Collections.Generic;
using System.Text;

namespace Client.MessageManagement {
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
