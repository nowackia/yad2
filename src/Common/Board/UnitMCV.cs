using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;

namespace Yad.Board.Common
{
    public class UnitMCV : Unit
    {
		public UnitMCV(short playerID, int unitID, short typeID, Position pos)
			: base(playerID, unitID, typeID, BoardObjectClass.UnitMCV, pos) {
		}

        public override void Destroy()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Move()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DoAI()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
