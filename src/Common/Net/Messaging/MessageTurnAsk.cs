using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Messaging {
	public class MessageTurnAsk : Message {
		public MessageTurnAsk() : base(MessageType.TurnAsk) { }
	}
}
