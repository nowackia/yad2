using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Board.Common
{
    /// <summary>
    /// base class for all units. must implement ai.
    /// </summary>
    public abstract class Unit : BoardObject
    {
        //common fields for all units - except sandworm
        private int damageDestroy;
        private String name;
        private int fireRange;
        private AmmoType ammoType;
        private int speed;
        private int reloadTime;
        private int health;
        private int viewRange;
        private int rotationSpeed;

		private bool isMoving;
		private short remainingTurnsToMove;
		Position destinationPoint;

        //TODO : RS implement some base AI?
        public abstract void Destroy();
        public abstract void Move();
        public abstract void DoAI();

		public Unit(short playerID, int unitID, Position pos) : base(playerID, unitID, pos) {
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
    }
}
