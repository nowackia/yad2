using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Messaging
{
    public enum PauseAction : byte {
        Pause,
        Resume,
        None
    }
    public class DoTurnMessage : Message
    {
		bool _speedUp = false;
        byte _pause = (byte)PauseAction.None;
        short _turnsToGo = 1;

        

        public DoTurnMessage()
            : base(MessageType.DoTurn)
        {}

		public bool SpeedUp {
			get { return _speedUp; }
			set { _speedUp = value; }
		}

        public short TurnsToGo {
            get { return _turnsToGo; }
            set { _turnsToGo = value; }
        }

        public byte Pause {
            get { return _pause; }
            set { _pause = value; }
        }

		public override void Serialize(System.IO.BinaryWriter writer) {
			base.Serialize(writer);
			writer.Write(_speedUp);
            writer.Write(_pause);
            writer.Write(_turnsToGo);
		}

		public override void Deserialize(System.IO.BinaryReader reader) {
			base.Deserialize(reader);
			_speedUp = reader.ReadBoolean();
            _pause = reader.ReadByte();
            _turnsToGo = reader.ReadInt16();
		}

        public override string ToString() {
            return base.ToString() + "DoTurnMessage: _speedUp: " + _speedUp + " _pause: " + _pause;
        }
    }
}
