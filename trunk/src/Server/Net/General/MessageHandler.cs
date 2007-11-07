using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.General.Messaging;
using System.Threading;
using Client.Log;
using Yad.Net.Server;

namespace Yad.Net.General {
    abstract class MessageHandler : ThreadListProcessor<Message> {
        private IMessageSender _sender;

        public MessageHandler() {

        }

        public void SetSender(IMessageSender sender) {
            _sender = sender;
        }
      

        protected void SendMessage(Message message, int recipient) {
            _sender.PostMessage(message, recipient);
        }

        public void OnReceivePlayerMessage(object sender, RecieveMessageEventArgs args) {
            this.AddItem(args.Message);
        }
    }
}
