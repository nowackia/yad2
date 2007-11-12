using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Board.Common
{
    public class UnitTank : Unit
    {
        private Animation turretAnimation;
        private int turretRotationSpeed;

		public UnitTank(short playerID, int unitID, Position pos) : base(playerID, unitID, pos) {
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
