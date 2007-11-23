using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;

namespace Yad.Board.Common {
	public class UnitHarvester : Unit {


		UnitHarvesterData _harvesterData;	

		public UnitHarvester(ObjectID id, UnitHarvesterData ud, Position pos, Map map, int speed)
			: base(id, ud.TypeID, BoardObjectClass.UnitHarvester, pos, map) {
			_harvesterData = ud;
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
			base.DoAI();
		}

		public UnitHarvesterData HarvesterData {
			get { return _harvesterData; }
		}
	}
}
