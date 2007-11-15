using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;
using Yad.Config;

namespace Yad.Net.Messaging.Common
{
	/// <summary>
	/// Split into MessageCreate(Tank|Trooper|etc)
	/// </summary>
    public class CreateUnitMessage : GameMessage
    {
        int unitID;
		private short unitType;
		private BoardObjectClass unitKind;

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

		//deserialize as short
		public BoardObjectClass UnitKind {
			get { return this.unitKind; }
			set { this.unitKind = value; }
		}

		public override void Deserialize(System.IO.BinaryReader reader) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
