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

        public CreateUnitMessage()
            : base(MessageType.CreateUnit)
        { }

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
            base.Deserialize(reader);
            unitID = reader.ReadInt32();
            unitType = reader.ReadInt16();
            unitKind = (BoardObjectClass)reader.ReadInt16();
            

        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(unitID);
            writer.Write(unitType);
            writer.Write((short)unitKind);
        }
    }
}
