using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;

namespace Yad.Board.Common
{
    /// <summary>
    /// base class for all units. must implement ai.
    /// </summary>
    public abstract class Unit : BoardObject
    {
        //common fields for all units - except sandworm
        protected int damageDestroy;
		protected String name;
		protected int fireRange;
		protected AmmoType ammoType;
		protected int speed;
		protected int reloadTime;
		protected int health;
		protected int viewRange;
		protected int rotationSpeed;

		protected bool isMoving;
		protected short remainingTurnsToMove;
		Position destinationPoint;

		protected short typeID;

        //TODO : RS implement some base AI?
        public abstract void Destroy();
        public abstract void Move();
        public abstract void DoAI();

		public Unit(short playerID, int unitID, short typeID, BoardObjectClass boc, Position pos) : base(playerID, unitID, boc, pos) {
			this.typeID = typeID;
		}

        public AmmoType AmmoType
        {
            get { return ammoType; }
        }

        public int FireRange
        {
            get { return fireRange; }
        }

        public int Speed
        {
            get { return speed; }
        }

        public int ReloadTime
        {
            get { return reloadTime; }
        }

        public int Health
        {
            get { return health; }
        }

        public int ViewRange
        {
            get { return viewRange; }
        }

        public int RotationSpeed
        {
            get { return rotationSpeed; }
        }

        public String Name
        {
            get { return name; }
        }

        public int DamageDestroy
        {
            get { return damageDestroy; }
        }

		public bool Moving {
			get { return isMoving; }
		}

		public Position DestinationPoint {
			get { return destinationPoint; }
			set { destinationPoint = value; }
		}

		public short TypeID {
			get { return this.typeID; }
		}
    }
}
