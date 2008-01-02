using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Server {
    class Utils {
        public static ResultMessage CreateResultMessage(ResponseType response, ResultType result) {
            ResultMessage msg = MessageFactory.Create(MessageType.Result) as ResultMessage;
            msg.ResponseType = (byte)response;
            msg.Result = (byte)result;
            return msg;
        }
    }
}
