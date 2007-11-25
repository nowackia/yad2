using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Messaging
{
    public class DoTurnMessage : Message
    {
		bool _speedUp = false;

        public DoTurnMessage()
            : base(MessageType.DoTurn)
        {}

		public bool SpeedUp {
			get { return _speedUp; }
			set { _speedUp = value; }
		}

		public override void Serialize(System.IO.BinaryWriter writer) {
			base.Serialize(writer);
			writer.Write(_speedUp);
		}

		public override void Deserialize(System.IO.BinaryReader reader) {
			base.Deserialize(reader);
			_speedUp = reader.ReadBoolean();
		}
    }
}
