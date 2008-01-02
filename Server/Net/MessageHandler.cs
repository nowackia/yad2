using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using System.Threading;
using Yad.Log;
using Yad.Net.Common;

namespace Yad.Net.Server {
    public abstract class MessageHandler : ThreadListProcessor<Message> {
        private IMessageSender _sender;

        public MessageHandler() {

        }

        public void SetSender(IMessageSender sender) {
            _sender = sender;
        }
      

        protected void SendMessage(Message message, short recipient) {
            _sender.MessagePost(message, recipient);
        }

        public virtual void OnReceivePlayerMessage(object sender, ReceiveMessageEventArgs args) {
            this.AddItem(args.Message);
        }
    }
}
