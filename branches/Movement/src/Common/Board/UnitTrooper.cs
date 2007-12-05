using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;
using Yad.Engine.Common;

namespace Yad.Board.Common {
	public class UnitTrooper : Unit {
		UnitTrooperData _trooperData;

		public UnitTrooper(ObjectID id, UnitTrooperData ud, Position pos, Map map, Simulation sim)
			: base(id, ud.TypeID,ud.__AmmoType, BoardObjectClass.UnitTrooper, pos, map,sim,ud.__AmmoDamageRange,ud.__DamageDestroyRange,ud.__DamageDestroy) {
			_trooperData = ud;
			this.Speed = ud.Speed;
			this._viewRange = ud.ViewRange;
            this._fireRange = ud.FireRange;
            this._reloadTime = ud.ReloadTime;
            this._firePower = ud.FirePower;
            this.MaxHealth = this.Health = ud.__Health;
		}

		public UnitTrooperData TrooperData {
			get { return _trooperData; }
		}

		public override float getSize() {
			return _trooperData.Size;
		}

		public override float getMaxHealth() {
			return _trooperData.Health;
		}
	}
}
