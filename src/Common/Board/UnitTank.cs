using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;

namespace Yad.Board.Common
{
    public class UnitTank : Unit
    {
        private Animation turretAnimation;
        private int turretRotationSpeed;

		public UnitTank(short playerID, int unitID, short typeID, Position pos)
			: base(playerID, unitID, typeID, BoardObjectClass.UnitTank, pos) {
		}

        public int TurretRotationSpeed
        {
            get { return turretRotationSpeed; }
        }

        
        public Animation TurretAnimation
        {
            get { return turretAnimation; }
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
