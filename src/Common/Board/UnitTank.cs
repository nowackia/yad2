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

        public Direction TurretDirection {
            get {
                int tankRotation = ConvertToNumber(Direction);
                int turretRotationDelta = ConvertToNumber(turretDirectionFromTankDirection);
                int turretRotation = turretRotationDelta + tankRotation;
                turretRotation %= 360;
                InfoLog.WriteInfo("####### " + turretRotation);
                return ConvertToDirection(turretRotation);            
            }
        }

		public UnitTank(ObjectID id, UnitTankData ud, Position pos, Map map, Simulation sim)
			: base(id, ud.TypeID, BoardObjectClass.UnitTank, pos, map, sim) {
			_tankData = ud;
			this.Speed = ud.Speed;
			this._viewRange = ud.ViewRange;
            this._fireRange = ud.FireRange;
            this._reloadTime = ud.ReloadTime;
            this._firePower = ud.FirePower;
            this.MaxHealth = this.Health = ud.__Health;
            this._rotationSpeed = ud.RotationSpeed;
            turretDirectionFromTankDirection =  Direction.West;
		}

		public Animation TurretAnimation {
			get { return turretAnimation; }
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

		public UnitTankData TankData {
			get { return _tankData; }
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
            
            
            int turn;
            int delta = alfaTarget - turretRotation;
            int adelta = Math.Abs(delta);
            if (adelta > 22) {
                // need to rotate - rotation = 45dg.
                //
                turn = delta > 0 ? 45 : -45;
                turretRotationDelta += turn;
                turretRotationDelta %= 360;
                turretDirectionFromTankDirection = ConvertToDirection(turretRotationDelta);

                return true;
            } else {
                return false;
            }
        }

        private int GetAlfa(double x, double y) {
            if (x == y && x == 0)
                return 0;// teoretically wont happen
            double norm = x * x + y * y;
            norm = Math.Sqrt(norm);
            x /= norm;
            y /= norm;
            double al = Math.Asin(y);
            al *= 180.0/Math.PI;
            al += 360;
            al %= 360;
            if (x >= 0) {
                return (int)al;
            } else {
                return (int)al + 180;
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

        private Direction ConvertToDirection(int number) {
            switch (number) {
                case 0:
                    return Direction.East;
                case 45:
                    return Direction.East | Direction.North;
                case 90:
                    return Direction.North;
                case 135:
                    return Direction.North | Direction.West;
                case 180:
                    return Direction.West;
                case 225:
                    return Direction.West | Direction.South;
                case 270:
                    return Direction.South;
                case 315:
                    return Direction.South | Direction.East;
                default:
                    return Direction.East; // never happen.
            }

        }

        protected override void Attack(BoardObject ob) {
            // rotate turret
            if (RotateIfNeeded(ob) == true) return;
            if (_remainingTurnsToReload == 0) {
                if (attackingBuilding) {
                    Building b = (Building)ob;
                    _simulation.handleAttackBuilding(b, this);
                    _remainingTurnsToReload = _reloadTime;
                } else {
                    Unit u = (Unit)ob;
                    _simulation.handleAttackUnit(u, this);
                    _remainingTurnsToReload = _reloadTime;
                }
            }
        }
	}
}
