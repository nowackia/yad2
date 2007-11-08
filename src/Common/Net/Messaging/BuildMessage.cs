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
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
