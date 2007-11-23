using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using System.Windows.Forms;
using Yad.Log.Common;

namespace Yad.Board.Common {
	/// <summary>
	/// base class for all units. must implement ai.
	/// </summary>
	public abstract class Unit : BoardObject {
		//common fields for all units - except sandworm
		protected short _damageDestroy;
		protected String _name;
		protected short _fireRange;
		protected AmmoType _ammoType;
		protected short _reloadTime;
		private short _speed;
		protected short _maxHealth;
		protected short _viewRange;
		protected short _rotationSpeed;
		protected Direction _direction;

		//used for moving
		protected bool _canCrossMountain = false, _canCrossBuildings = false, _canCrossRock = true, _canCrossTrooper = false, _canCrossTank = false;

		protected short _remainingTurnsInMove = 0;
		protected Position _lastPosition; //used while moving to remember last pos
		protected Queue<Position> _currentPath;
		//BoardObject.Position - current position, while moving unit is always at this coordinates

		protected short _typeID;

        protected BoardObject attackedObject;
        protected bool attackingBuilding; // -- need to proper cast.
        public enum UnitState {
            moving,
            chacing,
            stopped,
            attacking,
            destroyed
        }

        protected UnitState state = UnitState.stopped;
		//map-related
		protected Map _map;
		bool _alreadyOnMap = false;

		//TODO : RS implement some base AI?
		public virtual void Destroy() {
			InfoLog.WriteInfo("Unit:Destroy() Not implemented", EPrefix.SimulationInfo);
            state = UnitState.destroyed;
		}


        private static Position[] rangeSpiral;
        private static Dictionary<int, int> lenghts = new Dictionary<int,int>();

        private static void GenerateSpiral(int range) {


            List<Position> spiral = new List<Position>();
            spiral.Add(new Position(0, 0));
            lenghts[0] = 1;
            for (int i = 1; i <= range; ++i) {
                // for each radius
                double delta = 1.0 / range;
                for (double alfa = 0; alfa < 2 * Math.PI; alfa += delta) {
                    // alfa
                    int x = (int)(i * Math.Cos(alfa));
                    int y = (int)(i * Math.Sin(alfa));
                    Position p = new Position(x, y);
                    if (spiral.Contains(p) == false) spiral.Add(p);
                }
                lenghts[i] = spiral.Count;
            }
            rangeSpiral = spiral.ToArray();
        }

        protected static Position[] RangeSpiral(int range,out int max) {

            if (lenghts.ContainsKey(range)) {
                max = lenghts[range];
                return rangeSpiral;
            } else {
                GenerateSpiral(range);
                max = lenghts[range];
                return rangeSpiral;
            }
        }

        public virtual void DoAI() {
			InfoLog.WriteInfo("Unit:DoAI()", EPrefix.SimulationInfo);
            
            switch (state) {
                case UnitState.moving:
                    BoardObject nearest;
                    //if (FindNearestTargetInFireRange(out nearest)) {
                    //    InfoLog.WriteInfo("Unit:AI: move -> stop ", EPrefix.SimulationInfo);
                    //    state = UnitState.stopped;
                    //    StopMoving();
                    //} else 
                        if (Move() == false) {
                        InfoLog.WriteInfo("Unit:AI: move -> stop ", EPrefix.SimulationInfo);
                        state = UnitState.stopped;
                    } else {
                        InfoLog.WriteInfo("Unit:AI: move -> move ", EPrefix.SimulationInfo);
                    }
                    //TODO RS: modify to find way each time? - chasing another unit
                    break;
                case UnitState.chacing:
                    BoardObject nearest1;
                    if (FindNearestTargetInFireRange(out nearest1)) {
                        InfoLog.WriteInfo("Unit:AI: chacing -> stop ", EPrefix.SimulationInfo);
                        state = UnitState.stopped;
                        StopMoving();
                    } else 
                    if (Move() == false) {
                        InfoLog.WriteInfo("Unit:AI: chacing -> stop ", EPrefix.SimulationInfo);
                        state = UnitState.stopped;
                    } 
                    break;
                case UnitState.stopped:
                    if (FindNearestTargetInFireRange(out attackedObject)) {
                        state = UnitState.attacking;
                        InfoLog.WriteInfo("Unit:AI: stop -> attack ", EPrefix.SimulationInfo);
                        break;
                    }
                    BoardObject ob;
                    if (FindNearestTargetInViewRange(out ob)) {
                        InfoLog.WriteInfo("Unit:AI: stop -> chace ", EPrefix.SimulationInfo);
                        state = UnitState.chacing;
                        MoveTo(ob.Position);
                        state = UnitState.chacing;
                        break;
                    }
                    break;
                case UnitState.attacking:
                    if (CheckIfStillExistTarget(attackedObject) == false) {
                        // unit destroyed, find another one.
                        FindNearestTargetInFireRange(out attackedObject);
                    }
                    if (attackedObject == null) {
                        //unit/ building destroyed - stop
                        InfoLog.WriteInfo("Unit:AI: attack -> stop ", EPrefix.SimulationInfo);
                        state = UnitState.stopped;
                        break;
                    }
                    if (CheckRangeToShoot(attackedObject)) {
                        InfoLog.WriteInfo("Unit:AI: attack -> attack ", EPrefix.SimulationInfo);
                        Attack(attackedObject);
                        //attack, reload etc
                    } else {
                        // out of range - chase
                        InfoLog.WriteInfo("Unit:AI: attack -> chace ", EPrefix.SimulationInfo);
                        state = UnitState.chacing;
                        MoveTo(attackedObject.Position);
                        //override state
                        state = UnitState.chacing;
                    }
                    break;
            }


		}

        private bool FindNearestTargetInViewRange(out BoardObject ob) {

            int count;
            Position[] viewSpiral = RangeSpiral(this.ViewRange, out count);
            Map m = this._map;
            Position p = this.Position;
            BoardObject target;
            Position spiralPos;
            LinkedList<Unit> units;
            LinkedList<Building> buildings;
            for (int i = 0; i < count; ++i) {

                spiralPos = viewSpiral[i];
                if (p.X + spiralPos.X >= 0 
                    && p.X + spiralPos.X < m.Width
                    && p.Y + spiralPos.Y >= 0
                    && p.Y + spiralPos.Y < m.Height) {

                    units = m.Units[p.X + spiralPos.X, p.Y + spiralPos.Y];
                    foreach (Unit unit in units) {
                        //TODO erase true;)
                        if (unit.Equals(this)) continue;
                        if (true || unit.ObjectID.PlayerID != this.ObjectID.PlayerID) {
                            ob = unit;
                            InfoLog.WriteInfo("Unit:AI: found unit in view in range < " + this.ViewRange, EPrefix.SimulationInfo);
                            return true;
                        }
                    }

                    buildings = m.Buildings[p.X + spiralPos.X, p.Y + spiralPos.Y];
                    foreach (Building building in buildings) {
                        //TODO erase true;)
                        if (true || building.ObjectID.PlayerID != this.ObjectID.PlayerID) {
                            attackingBuilding = true;
                            ob = building;
                            InfoLog.WriteInfo("Unit:AI: found building in view in range < " + this.ViewRange, EPrefix.SimulationInfo);
                            return true;
                        }
                    }
                    
                }
            }


            ob = null;
            return false;
        }

        private bool FindNearestTargetInFireRange(out BoardObject nearest) {
            int count;
            Position[] viewSpiral = RangeSpiral(this.FireRange, out count);
            Map m = this._map;
            Position p = this.Position;
            BoardObject target;
            Position spiralPos;
            LinkedList<Unit> units;
            LinkedList<Building> buildings;
            for (int i = 0; i < count; ++i) {

                spiralPos = viewSpiral[i];
                if (p.X + spiralPos.X >= 0
                    && p.X + spiralPos.X < m.Width
                    && p.Y + spiralPos.Y >= 0
                    && p.Y + spiralPos.Y < m.Height) {

                    units = m.Units[p.X + spiralPos.X, p.Y + spiralPos.Y];
                    foreach (Unit unit in units) {
                        if (unit.Equals(this)) continue;
                        if (true || unit.ObjectID.PlayerID != this.ObjectID.PlayerID) {
                            // target

                            //TODO RS: bresenham to check if there is a way to shoot.
                            // else - continue -> move
                            attackingBuilding = false;
                            InfoLog.WriteInfo("Unit:AI: found unit in fire range < " + this.FireRange, EPrefix.SimulationInfo);
                            nearest = unit;
                            return true;
                        }
                    }

                    buildings = m.Buildings[p.X + spiralPos.X, p.Y + spiralPos.Y];
                    foreach (Building building in buildings) {
                        // erase true;)
                        if (true || building.ObjectID.PlayerID != this.ObjectID.PlayerID) {
                            attackingBuilding = true;
                            nearest = building;
                            InfoLog.WriteInfo("Unit:AI: found building in view in range < " + this.ViewRange, EPrefix.SimulationInfo);
                            return true;
                        }
                    }
                }
            }


            nearest = null;
            return false;
        }

        /// <summary>
        /// checks what type of object to attack; manage reload, destroying units, turret rotation
        /// </summary>
        /// <param name="ob"></param>
        private void Attack(BoardObject ob) {
            
        }
        /// <summary>
        /// checks if object is in shooting range
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        private bool CheckRangeToShoot(BoardObject ob) {
            int r = (int)(Math.Sqrt(Math.Pow(ob.Position.X-this.Position.X,2) + Math.Pow(ob.Position.Y-this.Position.Y,2)));
            int range = this.FireRange;
            InfoLog.WriteInfo("Unit:AI: in range:" + r + " ?< " + range, EPrefix.SimulationInfo);
            return r< range;
        }

        /// <summary>
        /// checks if object exists on map.
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        private bool CheckIfStillExistTarget(BoardObject ob) {
            if (attackingBuilding) {
                Building b = (Building)ob;
                return b.State == Building.BuildingState.destroyed;

            } else {
                Unit u = (Unit)ob;
                return u.state != UnitState.destroyed;
            }
        }

		public virtual bool Move() {
			if (!this.Moving) {
				return false;
			}

			if (_remainingTurnsInMove == 0) {
				//unit starts to move
				//so we set a new position
				this._remainingTurnsInMove = this._speed;

				Position newPos = _currentPath.Dequeue();

				//TODO: check newPos;
				/*
				if ( badPos ) {
					this.currentPath = Unit.FindPath(this.Position, newPos, this.map, canCrossMountain, canCrossBuildings, canCrossRock, canCrossTrooper, canCrossTank);
					if (currentPath.Count == 0
					newPos = currentPath.Dequeue();
				}
				*/

				this._map.Units[this.Position.X, this.Position.Y].Remove(this);

				this._lastPosition = Position;
				this.Position = newPos;

				this._map.Units[this.Position.X, this.Position.Y].AddFirst(this);

				//set new direction
				short dx = (short)(newPos.X - _lastPosition.X);
				short dy = (short)(newPos.Y - _lastPosition.Y);
				this._direction = Direction.None;
				if (dx > 0) {
					_direction |= Direction.East;
				} else if (dx < 0) {
					_direction |= Direction.West;
				}
				if (dy > 0) {
					_direction |= Direction.North;
				} else if (dy < 0) {
					_direction |= Direction.South;
				}

				this.ClearFogOfWar();
			}

			//move unit
			this._remainingTurnsInMove--;

			return true;
		}

		public Unit(ObjectID id, short typeID, BoardObjectClass boc, Position pos, Map map)
			: base(id, boc, pos) {
			this._typeID = typeID;
			this._map = map;
			this._lastPosition = pos;
			this._direction = Direction.North;
			this._currentPath = new Queue<Position>();
		}

		public AmmoType AmmoType {
			get { return _ammoType; }
		}

		public int FireRange {
			get { return _fireRange; }
		}

		public int ReloadTime {
			get { return _reloadTime; }
		}

		public int Health {
			get { return _maxHealth; }
		}

		public int ViewRange {
			get { return _viewRange; }
		}

		public int RotationSpeed {
			get { return _rotationSpeed; }
		}

		public String Name {
			get { return _name; }
		}

		public int DamageDestroy {
			get { return _damageDestroy; }
		}

		public Position DestinationPoint {
			get {
				if (this._currentPath.Count == 0) {
					return this.Position;
				}

				return this._currentPath.ToArray()[_currentPath.Count - 1];
			}
			//set { destinationPoint = value; }
		}

		public short TypeID {
			get { return this._typeID; }
		}

		public void MoveTo(Position destination) {
			//we can override old path
			this._currentPath = FindPath(this.Position, destination, this._map,
										this._canCrossMountain, this._canCrossBuildings,
										this._canCrossRock, this._canCrossTrooper, this._canCrossRock);
            state = UnitState.moving;
			//this._remainingTurnsInMove = this.speed;
		}

		/// <summary>
		/// Indicates whether the unit is moving:
		/// - still have a move to finish
		/// or
		/// - have destination queued
		/// </summary>
		public bool Moving {
			get { return (this._remainingTurnsInMove != 0) || (this._currentPath.Count != 0); }
		}

		public static Queue<Position> FindPath(Position source, Position dest, Map map,
												bool canCrossMountain, bool canCrossBuildings,
												bool canCrossRock, bool canCrossTrooper, bool canCrossTank) {
			Queue<Position> path = new Queue<Position>();
			
			//TODO Go-Go-Gadget!

			//remove

			int diffX = dest.X - source.X;
			int diffY = dest.Y - source.Y;
			float x = source.X, y = source.Y;
			int m;

			if (Math.Abs(diffX) > Math.Abs(diffY)) {
				m = (diffX > 0)?1:-1;
				float dy = (float)diffY / (float)Math.Abs(diffX);
				for (int i = 0; i < Math.Abs(diffX); i++) {
					x += m;
					y += dy;
					path.Enqueue(new Position((short)x, (short)y));
				}
			} else {
				m = (diffY > 0) ? 1 : -1;
				float dx = diffX / (float)Math.Abs(diffY);
				for (int i = 0; i < Math.Abs(diffY); i++) {
					x += dx;
					y += m;
					path.Enqueue(new Position((short)x, (short)y));
				}
			}

			//path.Enqueue(dest);
			//end remove

			return path;
		}

		public void StopMoving() {
			this._currentPath.Clear();
		}

		public short Speed {
			get { return this._speed; }
			set {
				if (value == 0) {
					throw new ArgumentOutOfRangeException("Speed must be greater than 0");
				}
				this._speed = value;
			}
		}

		public Position LastPosition {
			get { return this._lastPosition; }
		}

		public int RemainingTurnsInMove {
			get { return this._remainingTurnsInMove; }
		}

		public Direction Direction {
			get { return _direction; }
			set { _direction = value; }
		}

		public bool PlaceOnMap() {
			if (!_alreadyOnMap) {
				this._map.Units[this.Position.X, this.Position.Y].AddFirst(this);
				ClearFogOfWar();
				
				_alreadyOnMap = true;
				return true;
			}
			return false;
		}

		public void ClearFogOfWar() {
			for (int x = -_viewRange + Position.X; x <= _viewRange + Position.X; x++) {
				for (int y = -_viewRange + Position.Y; y <= _viewRange + Position.Y; y++) {
					if (x < 0 || y < 0 || x > _map.Width - 1 || y > _map.Height - 1) {
						continue;
					}
					_map.FogOfWar[x, y] = false;
				}
			}				
		}
        /// <summary>
        /// Sets object to attack (building or unit) by this unit.
        /// </summary>
        /// <param name="objectID"></param>
        public void OrderAttack(BoardObject boardObject,bool isBuilding) {
            attackedObject = boardObject;
            state = UnitState.attacking;
            this.attackingBuilding = isBuilding;
        }
    }
}
