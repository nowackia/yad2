using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using System.Windows.Forms;
using Yad.Log.Common;
using Yad.Engine.Common;
using System.Drawing;

namespace Yad.Board.Common {
	/// <summary>
	/// base class for all units. must implement ai.
	/// </summary>
	public abstract class Unit : BoardObject {
		//common fields for all units - except sandworm
		protected short _damageDestroy;
		protected String _name;
		protected short _fireRange;
        protected short _firePower;
		protected AmmoType _ammoType;
		protected short _reloadTime;
		private short _speed;
		protected short _maxHealth;
        protected short _currentHealth;
		protected short _viewRange;
		protected short _rotationSpeed;
		protected Direction _direction;

		//used for moving
		protected bool _canCrossMountain = false, _canCrossBuildings = false, _canCrossRock = true, _canCrossTrooper = false, _canCrossTank = false;

		protected short _remainingTurnsInMove = 0;
        protected short _remainingTurnsToReload = 0;
		protected Position _lastPosition; //used while moving to remember last pos
		protected Queue<Position> _currentPath;
		//BoardObject.Position - current position, while moving unit is always at this coordinates

		protected short _typeID;

        protected BoardObject attackedObject;
        protected bool attackingBuilding; // -- need to proper cast.
        public enum UnitState {
            moving,
            chasing,
            stopped,
            attacking,
            orderedAttack,
            destroyed
        }

        protected UnitState state = UnitState.stopped;
		//map-related
		protected Map _map;
        protected Simulation _simulation;
		bool _alreadyOnMap = false;

		//TODO : RS implement some base AI?
		public virtual void Destroy() {
			InfoLog.WriteInfo("Unit:Destroy() Not implemented", EPrefix.SimulationInfo);
            state = UnitState.destroyed;
		}


        public virtual void DoAI() {
			InfoLog.WriteInfo("Unit:DoAI()", EPrefix.SimulationInfo);
            if (_remainingTurnsToReload > 0) --_remainingTurnsToReload;
            if (_remainingTurnsInMove > 0 && Moving && state == UnitState.stopped) {
                Move();
            }
            switch (state) {
                case UnitState.moving:
                    //BoardObject nearest;
                    //if (FindNearestTargetInFireRange(out nearest)) {
                    //    InfoLog.WriteInfo("Unit:AI: move -> stop ", EPrefix.SimulationInfo);
                    //    state = UnitState.stopped;
                    //    StopMoving();
                    //} else
                        if (Move() == false) {
                        InfoLog.WriteInfo("Unit:AI: move -> stop ", EPrefix.SimulationInfo);
                        state = UnitState.stopped;
                        StopMoving();
                    } else {
                        InfoLog.WriteInfo("Unit:AI: move -> move ", EPrefix.SimulationInfo);
                    }
                    //TODO RS: modify to find way each time? - chasing another unit
                    break;
                case UnitState.chasing:
                    BoardObject nearest1;
                    if (FindNearestTargetInFireRange(out nearest1)) {
                        InfoLog.WriteInfo("Unit:AI: chacing -> stop ", EPrefix.SimulationInfo);
                        state = UnitState.stopped;
                        StopMoving();
                    } else 
                    if (Move() == false) {
                        InfoLog.WriteInfo("Unit:AI: chacing -> stop ", EPrefix.SimulationInfo);
                        state = UnitState.stopped;
                        StopMoving();
                    } 
                    break;
                case UnitState.orderedAttack:
                    if (Move() == false) {
                        InfoLog.WriteInfo("Unit:AI: chacing -> stop ", EPrefix.SimulationInfo);
                        if (CheckIfStillExistTarget(attackedObject) == false) {
                            state = UnitState.stopped;
                            StopMoving();
                            break;
                        }
                        
                    }
                    if (CheckRangeToShoot(attackedObject) ) {
                        state = UnitState.attacking;
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
                        state = UnitState.chasing;
                        MoveTo(ob.Position);
                        state = UnitState.chasing;
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
                        StopMoving();
                        break;
                    }
                    if (CheckRangeToShoot(attackedObject)) {
                        InfoLog.WriteInfo("Unit:AI: attack -> attack ", EPrefix.SimulationInfo);
                        Attack(attackedObject);
                        //attack, reload etc
                    } else {
                        // out of range - chase
                        InfoLog.WriteInfo("Unit:AI: attack -> chace ", EPrefix.SimulationInfo);
                        state = UnitState.chasing;
                        MoveTo(attackedObject.Position);
                        //override state
                        state = UnitState.chasing;
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
                        if (unit.ObjectID.PlayerID != this.ObjectID.PlayerID) {
                            ob = unit;
                            InfoLog.WriteInfo("Unit:AI: found unit in view in range < " + this.ViewRange, EPrefix.SimulationInfo);
                            return true;
                        }
                    }

                    buildings = m.Buildings[p.X + spiralPos.X, p.Y + spiralPos.Y];
                    foreach (Building building in buildings) {
                        //TODO erase true;)
                        if ( building.ObjectID.PlayerID != this.ObjectID.PlayerID) {
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
                        if (unit.ObjectID.PlayerID != this.ObjectID.PlayerID) {
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
                        if (building.ObjectID.PlayerID != this.ObjectID.PlayerID) {
                            attackingBuilding = true;
                            nearest = building;
                            InfoLog.WriteInfo("Unit:AI: found building fire range < " + this.FireRange, EPrefix.SimulationInfo);
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
        protected virtual void Attack(BoardObject ob) {

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
                return b.State != Building.BuildingState.destroyed;

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

		public Unit(ObjectID id, short typeID, BoardObjectClass boc, Position pos, Map map, Simulation sim)
			: base(id, boc, pos) {
			this._typeID = typeID;
			this._map = map;
            this._simulation = sim;
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

        public short FirePower {
            get { return _firePower; }
        }

		public int ReloadTime {
			get { return _reloadTime; }
		}

		public short Health {
			get { return _currentHealth; }
            set { _currentHealth = value; }

		}

        public short MaxHealth {
            get { return _maxHealth; }
            set { _maxHealth = value; }

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
            Queue<Position> path = Bresenham(ref source, ref dest);

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
            MoveTo(boardObject.Position);
            state = UnitState.orderedAttack;
            this.attackingBuilding = isBuilding;
            InfoLog.WriteInfo("Unit:AI: attacking!!!! ", EPrefix.SimulationInfo);
        }

		public abstract float getSize();
		public abstract float getMaxHealth();
    }
}
