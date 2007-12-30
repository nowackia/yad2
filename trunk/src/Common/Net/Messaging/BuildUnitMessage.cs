using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Messaging {
    public class BuildUnitMessage :GameMessage {
        short _type;

        public short UnitType {
            get { return _type; }
            set { _type = value; }
        }

        int _creatorID;

        public int CreatorID {
            get { return _creatorID; }
            set { _creatorID = value; }
        }

        public BuildUnitMessage()
            : base(MessageType.BuildUnitMessage) {
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            _type = reader.ReadInt16();
            _creatorID = reader.ReadInt32();
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(_type);
            writer.Write(_creatorID);
        }

        public override string ToString() {
            return base.ToString() + "UnitType: " + _type + " CreatorID: " + _creatorID;
        }


    }
}
