using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;

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
			base.DoAI();
		}

		public UnitMCVData MCVData {
			get { return _mcvData; }
		}
	}
}
