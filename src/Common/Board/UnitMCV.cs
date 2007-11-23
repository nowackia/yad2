using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;
using Yad.Log.Common;

namespace Yad.Board.Common {
	public class UnitMCV : Unit {
		UnitMCVData _mcvData;

		public UnitMCV(ObjectID id, UnitMCVData ud, Position pos, Map map)
			: base(id, ud.TypeID, BoardObjectClass.UnitMCV, pos, map) {
			_mcvData = ud;
			this.Speed = ud.Speed;
			this._viewRange = ud.ViewRange;

		}

		public override void Destroy() {
			base.Destroy();
		}

		public override bool Move() {
			return base.Move();			
		}

		public override void DoAI() {
            switch (state) {
                case UnitState.moving:
                    BoardObject nearest;
                    if (Move() == false) {
                        InfoLog.WriteInfo("MCV:AI: move -> stop ", EPrefix.SimulationInfo);
                        state = UnitState.stopped;
                    } else {
                        InfoLog.WriteInfo("MCV:AI: move -> move ", EPrefix.SimulationInfo);
                    }
                    break;
                case UnitState.stopped:
                        break;
                    }
            }
		

		public UnitMCVData MCVData {
			get { return _mcvData; }
		}
	}
}
