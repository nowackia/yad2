using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;

namespace Yad.Net.Messaging.Common
{
    public class BuildMessage : GameMessage
    {
		private int buildingID;

		public int BuildingID {
			get { return buildingID; }
			set { buildingID = value; }
		}

		private short buildingType;

		public short BuildingType {
			get { return buildingType; }
			set { buildingType = value; }
		}


		private Position position;

		public Position Position {
			get { return position; }
			set { position = value; }
		}
	

        public BuildMessage()
            : base(MessageType.Build)
        { }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            buildingID = reader.ReadInt32();
            buildingType = reader.ReadInt16();
            position = new Position();
            position.Deserialize(reader);
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(BuildingID);
            writer.Write(buildingType);
            position.Serialize(writer);
        }
    }
}
