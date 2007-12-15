using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;
using Yad.Engine.Common;
using Yad.Log.Common;

namespace Yad.Board.Common {
	public class UnitTank : Unit {
		private Animation turretAnimation;
		UnitTankData _tankData;
        // delta direction to tank direction
        Direction turretDirectionFromTankDirection;
        Direction lastCountedDirection = Direction.None;
        Direction lastCountedTurretDirection = Direction.None;
        Direction lastTurretDirectionFromTankDirection = Direction.None;
        public Direction TurretDirection {
            get {
                // only way to check - remember last value - less operations.
                if ((this.Direction == lastCountedDirection) && 
                    (lastTurretDirectionFromTankDirection == turretDirectionFromTankDirection))
                    return lastCountedTurretDirection;

                int tankRotation = ConvertToNumber(Direction);
                int turretRotationDelta = ConvertToNumber(turretDirectionFromTankDirection);
                int turretRotation = turretRotationDelta + tankRotation;
                turretRotation %= 360;
                lastTurretDirectionFromTankDirection = turretDirectionFromTankDirection;
                lastCountedDirection = this.Direction;
                lastCountedTurretDirection = ConvertToDirection(turretRotation);
                return lastCountedTurretDirection;            
            }
        }

		public UnitTank(ObjectID id, UnitTankData ud, Position pos, Map map, Simulation sim)
			: base(id, ud.TypeID, ud.__AmmoType, BoardObjectClass.UnitTank, pos, map, sim,ud.__AmmoDamageRange,ud.__DamageDestroyRange,ud.__DamageDestroy,2) {
			_tankData = ud;
			this.Speed = ud.Speed;
			this._viewRange = ud.ViewRange;
            this._fireRange = ud.FireRange;
            this._reloadTime = ud.ReloadTime;
            this._firePower = ud.FirePower;
            this.MaxHealth = this.Health = ud.__Health;
            this._rotationSpeed = ud.RotationSpeed;
            turretDirectionFromTankDirection =  Direction.East;
		}

		public Animation TurretAnimation {
			get { return turretAnimation; }
		}

		public UnitTankData TankData {
			get { return _tankData; }
		}

		public override float getSize() {
			return _tankData.Size;
		}

		public override float getMaxHealth() {
			return _tankData.Health;
		}

        protected bool RotateIfNeeded(BoardObject ob) {
            // check first rotation
            if (RotationSpeed!=0 && RotateIfNeededInternal(ob) == false) return false;
            for (int i = 1; i < RotationSpeed; ++i) {
                if (RotateIfNeededInternal(ob) == false) break;
            }
            // rotated more than once.
            return true;
        }

        /// <summary>
        /// rotate if target is out of range
        /// </summary>
        /// <param name="ob">target</param>
        /// <returns>if rotation was needed</returns>
        protected bool RotateIfNeededInternal(BoardObject ob) {
             

            int alfaTarget = GetAlfa(ob.Position.X - this.Position.X, ob.Position.Y - this.Position.Y);
            int tankRotation = ConvertToNumber(Direction);
            int turretRotationDelta = ConvertToNumber(turretDirectionFromTankDirection);

            int turretRotation = turretRotationDelta + tankRotation;


            turretRotation %= 360;
            int delta = turretRotation - alfaTarget;
            delta += 360;
            delta %= 360;
            InfoLog.WriteInfo("## turret rot " + turretRotationDelta + " tank rot: " + tankRotation + "# target: " +alfaTarget + "### " + delta, EPrefix.SimulationInfo);
            int turn = 0;
            if (delta < 360 - 23 && delta >=180) {
                // rotate clockwise
                turn = 45;
                turretRotationDelta += turn;
                turretRotationDelta += 360;
                turretRotationDelta %= 360;
                turretDirectionFromTankDirection = ConvertToDirection(turretRotationDelta);
                return true;
            }
            else if (delta > 23 && delta < 180) {
                // rotate counterclockwise
                turn = -45;
                turretRotationDelta += turn;
                turretRotationDelta += 360;
                turretRotationDelta %= 360;
                turretDirectionFromTankDirection = ConvertToDirection(turretRotationDelta);

                return true;
            }
            else {
                return false;
            }
        }

        

        private int ConvertToNumber(Direction dir) {
            switch (dir) {
                case Direction.East:
                    return 0;
                case Direction.East | Direction.North:
                    return 45;
                case Direction.North:
                    return 90;
                case Direction.North | Direction.West:
                    return 135;
                case Direction.West:
                    return 180;
                case Direction.West | Direction.South:
                    return 225;
                case Direction.South:
                    return 270;
                case Direction.South | Direction.East:
                    return 315;
                default:
                    return 0; // never happen.

            }
        }

        

        protected override void TryAttack(BoardObject ob) {
            // rotate turret
            if (RotateIfNeeded(ob) == true) return;
            if (_remainingTurnsToReload == 0) {
                AttackRegion(ob);
            }
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
