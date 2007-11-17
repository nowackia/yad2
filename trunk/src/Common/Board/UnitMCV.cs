using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;

namespace Yad.Board.Common
{
    public class UnitMCV : Unit
    {
		public UnitMCV(short playerID, int unitID, UnitMCVData ud, Position pos, Map map)
			: base(playerID, unitID, ud.TypeID, BoardObjectClass.UnitMCV, pos, map) {
			this.Speed = ud.Speed;
			//fill other properties
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
