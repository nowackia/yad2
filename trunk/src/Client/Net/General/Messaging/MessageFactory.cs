using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging {
    class MessageFactory {
        public static Message Create(MessageType msgType) {
            switch (msgType) {

                case MessageType.Move:
                    return new MoveMessage();

                /*case MessageType.Numeric:
                    TextMessage textmsg = new TextMessage();
                    textmsg.Type = MessageType.ChatText;
                    return textmsg;*/

                case MessageType.ChatText:
                    TextMessage textmsg = new TextMessage();
                    textmsg.Type = MessageType.ChatText;
                    return textmsg;

                case MessageType.DeleteChatUser:
                    NumericMessage nummsg = new NumericMessage();
                    nummsg.Type = MessageType.DeleteChatUser;
                    return nummsg;
            }
            return null;
        }
    }
}
