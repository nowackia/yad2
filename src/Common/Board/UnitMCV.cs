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
			: base(id, ud.TypeID,null, BoardObjectClass.UnitMCV, pos, map,sim,0,ud.__DamageDestroyRange,ud.__DamageDestroy,0) {
			_mcvData = ud;
			this.Speed = ud.Speed;
			this._viewRange = ud.ViewRange;
            this.MaxHealth = this.Health = ud.__Health;

		}

		public override void DoAI() {
            if (_remainingTurnsInMove > 0 && Moving && state == UnitState.stopped) {
                Move();
                return;
            }
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

        protected override bool IsMoveable(short x, short y, Map map)
        {
            if (base.IsMoveable(x, y, map))
            {
                if (_map.Tiles[x, y] == TileType.Mountain)
                    return false;
                return true;
            }
            return false;
        }


	}
}
