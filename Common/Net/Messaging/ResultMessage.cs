using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Messaging.Common {
    public class ResultMessage : Message {
        
        private byte _result;
        private byte _responseType;

        public ResultMessage() : base(MessageType.Result) { }

        public byte ResponseType {
            get { return _responseType; }
            set { _responseType = value; }
        }

        public byte Result {
            get { return _result; }
            set { _result = value; }
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(_responseType);
            writer.Write(_result);
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            _responseType = reader.ReadByte();
            _result = reader.ReadByte();
        }
    }
}
