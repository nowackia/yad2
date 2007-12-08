using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config.Common;
using Yad.Engine.Common;

namespace Yad.Board.Common {
	public class UnitSandworm : Unit {
		UnitSandwormData _sandwormData;

		public UnitSandworm(ObjectID id, UnitSandwormData ud, Position pos, Map map, Simulation sim, int speed)
			: base(id, ud.TypeID,null, Yad.Config.BoardObjectClass.UnitSandworm, pos, map,sim,0,0,0) {
			_sandwormData = ud;
			this.Speed = ud.Speed;
            this.MaxHealth = this.Health = ud.__Health;
            //this.FirePower = ud.
            //this._viewRange = ud.ViewRange;
            //this._fireRange = ud.FireRange;
            //this._reloadTime = ud.ReloadTime;
		}

		public UnitSandwormData SandwormData {
			get { return _sandwormData; }
		}

		public override float getSize() {
			return _sandwormData.Size;
		}

		public override float getMaxHealth() {
			return _sandwormData.Health;
		}

        protected override bool IsMoveable(short x, short y, Map map)
        {
            if (base.IsMoveable(x, y, map))
            {
                if (_map.Tiles[x, y] != TileType.Sand)
                    return false;
                return true;
            }
            return false;
        }
	}
}
