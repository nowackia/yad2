using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;

namespace Yad.Net.Messaging.Common
{
    public class CreateUnitMessage : GameMessage
    {
        int unitID;
		private short unitType;

		/// <summary>
		/// Same value as assigned by XMLLoader.
		/// </summary>
		public short UnitType {
			get { return unitType; }
			set { unitType = value; }
		}

		private Position position;

		public Position Position {
			get { return position; }
			set { position = value; }
		}

		
		public CreateUnitMessage()
			: base(MessageType.CreateUnit) { }

		

        public int UnitID
        {
            get { return unitID; }
            set { unitID = value; }
        }

		public override void Deserialize(System.IO.BinaryReader reader) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
