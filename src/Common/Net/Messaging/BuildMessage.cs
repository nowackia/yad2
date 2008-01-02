using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;

namespace Yad.Net.Messaging.Common
{
    public class BuildMessage : GameMessage
    {
        private int creatorID;
        private short buildingType;
        private Position position;

        public int CreatorID{
            get { return creatorID; }
            set { creatorID = value; }
        }
		
		public short BuildingType {
			get { return buildingType; }
			set { buildingType = value; }
		}

		public Position Position {
			get { return position; }
			set { position = value; }
		}
	
        public BuildMessage()
            : base(MessageType.Build)
        { }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            creatorID = reader.ReadInt32();
            buildingType = reader.ReadInt16();
            position = new Position();
            position.Deserialize(reader);
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(CreatorID);
            writer.Write(buildingType);
            position.Serialize(writer);
        }

        public override string ToString() {
            return base.ToString() + "BuildMessage :creatorID: " + 
                creatorID + " buildType: " + buildingType + "position: " + position;
        }
    }
}
