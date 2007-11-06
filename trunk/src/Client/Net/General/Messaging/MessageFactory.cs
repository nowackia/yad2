using System;
using System.Collections.Generic;
using System.Text;
using Client.Net.General.Messaging;

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
                case MessageType.Login:
                    LoginMessage loginmsg = new LoginMessage();
                    loginmsg.Type = MessageType.Login;
                    return loginmsg;
                case MessageType.LoginSuccessful:
                    Message m = new Message();
                    m.Type = MessageType.LoginSuccessful;
                    return m;
            }
            return null;
        }
    }
}
