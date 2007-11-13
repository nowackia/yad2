using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Board.Common {
	public class Building : BoardObject {
		private int health;
		private short typeId;


		public Building(short playerID, int unitID, short typeId,  Position pos) : base(playerID, unitID, pos) {
			this.typeId = typeId;
		}

		public short TypeId
		{
			get
			{
				return this.typeId;
			}
		}

		public int Health
		{
			get
			{
				return Health;
			}
		}
	}
}
