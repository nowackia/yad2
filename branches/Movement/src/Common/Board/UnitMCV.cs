using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;
using Yad.Log.Common;
using Yad.Engine.Common;

namespace Yad.Board.Common {
	public class UnitMCV : Unit {
		UnitMCVData _mcvData;

		public UnitMCV(ObjectID id, UnitMCVData ud, Position pos, Map map, Simulation sim )
			: base(id, ud.TypeID,null, BoardObjectClass.UnitMCV, pos, map,sim,0,ud.__DamageDestroyRange,ud.__DamageDestroy) {
			_mcvData = ud;
			this.Speed = ud.Speed;
			this._viewRange = ud.ViewRange;
            this.MaxHealth = this.Health = ud.__Health;

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

		public override float getSize() {
			return _mcvData.Size;
		}

		public override float getMaxHealth() {
			return _mcvData.Health;
		}
	}
}
