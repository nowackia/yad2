using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using System.Windows.Forms;
using Yad.Log.Common;
using Yad.Engine.Common;
using System.Drawing;
using Yad.AI;
using Yad.AI.General;
using Yad.Algorithms;

namespace Yad.Board.Common {
	/// <summary>
	/// base class for all units. must implement ai.
	/// </summary>
	public abstract class Unit : BoardObject, IPositionChecker {
		//common fields for all units - except sandworm
        private MapInput _mapInput;
		protected short _damageDestroy;
		protected String _name;
		protected short _fireRange;
        protected short _firePower;
		protected AmmoType _ammoType;
		protected short _reloadTime;
        protected short _ammoSpeed;
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

        private short MaxTmpDestinationRange = 10;
        private Position _goal;
        private Position _tmpGoal;

        private bool orderedAttack = false;

        public enum UnitState {
            moving,
            chasing,
            stopped,
            attacking,
            orderedAttack,
            harvesting,
            destroyed
        }

        private short ammoDamageRange = 0;
        private short damageDestroyRange = 0;

        protected UnitState state = UnitState.stopped;
		//map-related
		protected Map _map;
        protected Simulation _simulation;
		bool _alreadyOnMap = false;
		
        public virtual void Destroy() {
            state = UnitState.destroyed;
			InfoLog.WriteInfo("Unit:Destroy() Not implemented", EPrefix.SimulationInfo);

			if (_damageDestroy == 0) {
				return;
			}

            Position p = this.Position;
            int count;
            Position[] viewSpiral = RangeSpiral(this.DamageDestroyRange, out count);
            for (int i = 0; i < count; ++i) {

                Position spiralPos = viewSpiral[i];
                if (p.X + spiralPos.X >= 0
                    && p.X + spiralPos.X < _map.Width
                    && p.Y + spiralPos.Y >= 0
                    && p.Y + spiralPos.Y < _map.Height) {

                    ICollection<Unit> units = _map.Units[p.X + spiralPos.X, p.Y + spiralPos.Y];
                    Unit [] unitsArr = new Unit[units.Count];
                    units.CopyTo(unitsArr, 0);
                    foreach (Unit unit in unitsArr) {
                        _simulation.handleAttackUnit(unit, this,this._damageDestroy);
                    }

                    ICollection<Building> buildings = _map.Buildings[p.X + spiralPos.X, p.Y + spiralPos.Y];
                    Building[] buildingsArr = new Building[buildings.Count];
                    buildings.CopyTo(buildingsArr, 0);
                    foreach (Building building in buildingsArr) {
                        _simulation.handleAttackBuilding(building, this, this._damageDestroy);
                    }
                }
            }
		}


        protected string DoAIPrefix{
            get {
            return "ID: " + this.ObjectID.ToString() + " Pos: " + this.Position.ToString() + "Type: " + this.TypeID;
            }
        }
        public virtual void DoAI() {
			InfoLog.WriteInfo(DoAIPrefix + " unit:DoAI()", EPrefix.AI);
            if (_remainingTurnsToReload > 0) --_remainingTurnsToReload;
            if (_remainingTurnsInMove > 0 && Moving && state == UnitState.stopped) {
                Move();
                return;
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
                            InfoLog.WriteInfo(DoAIPrefix + "Unit:AI: move -> stop ", EPrefix.AI);
                        state = UnitState.stopped;
                        StopMoving();
                    } else {
                        InfoLog.WriteInfo(DoAIPrefix +  "Unit:AI: move -> move ", EPrefix.AI);
                    }
                    //TODO RS: modify to find way each time? - chasing another unit
                    break;
                /*case UnitState.chasing:
                    BoardObject nearest1;
                    if (FindNearestTargetInFireRange(out nearest1)) {
                        InfoLog.WriteInfo(DoAIPrefix +  "Unit:AI: chasing -> stop ", EPrefix.AI);
                        state = UnitState.stopped;
                        StopMoving();
                    } else 
                    if (Move() == false) {
                        InfoLog.WriteInfo(DoAIPrefix+ "Unit:AI: chasing -> stop ", EPrefix.AI);
                        state = UnitState.stopped;
                        StopMoving();
                    } 
                    break;*/
                case UnitState.orderedAttack:
                    if (Move() == false) {
                        InfoLog.WriteInfo(DoAIPrefix + "Unit:AI: chasing -> stop ", EPrefix.AI);
                        if (CheckIfStillExistTarget(attackedObject) == false) {
                            state = UnitState.stopped;
                            StopMoving();
                            orderedAttack = false;
                            break;
                        } else {

                            if (CheckRangeToShoot(attackedObject)) {
                                StopMoving();
                                orderedAttack = false;
                                state = UnitState.stopped;
                            } else {
                                // try to continue moveattack
                                if (MoveTo(attackedObject.Position) == false) {
                                    state = UnitState.stopped;
                                    StopMoving();
                                    orderedAttack = false;
                                }
                                state = UnitState.orderedAttack;
                            }
                        }

                    } else {
                        if (CheckRangeToShoot(attackedObject)) {
                            StopMoving();
                            orderedAttack = false;
                            state = UnitState.stopped;
                        }
                    }
                    break;
                case UnitState.stopped:

                    if (orderedAttack && CheckIfStillExistTarget(attackedObject)) {
                        state = UnitState.attacking;
                        InfoLog.WriteInfo(DoAIPrefix + "Unit:AI: stop -> attack ", EPrefix.AI);
                        break;
                    }
                    if (FindNearestTargetInFireRange(out attackedObject)) {
                        state = UnitState.attacking;
                        InfoLog.WriteInfo(DoAIPrefix + "Unit:AI: stop -> attack ", EPrefix.AI);
                        break;
                    }
                    /*BoardObject ob;
                    if (FindNearestTargetInViewRange(out ob)) {
                        InfoLog.WriteInfo(DoAIPrefix + "Unit:AI: stop -> chace ", EPrefix.AI);
                        state = UnitState.chasing;
                        InfoLog.WriteInfo(DoAIPrefix + "Starting chasing:" + ob.Position.ToString(), EPrefix.AI);
                        if (MoveTo(ob.Position))
                            state = UnitState.chasing;
                        else
                        {
                            InfoLog.WriteInfo(DoAIPrefix + "Chasing failed!", EPrefix.AI);
                            state = UnitState.stopped;
                        }
                        break;
                    }*/
                    break;
                case UnitState.attacking:
                    if (CheckIfStillExistTarget(attackedObject) == false) {
                        // unit destroyed, find another one.
                        FindNearestTargetInFireRange(out attackedObject);
                        orderedAttack = false;
                    }
                    if (attackedObject == null) {
                        //unit/ building destroyed - stop
                        InfoLog.WriteInfo(DoAIPrefix + "Unit:AI: attack -> stop ", EPrefix.AI);
                        state = UnitState.stopped;
                        StopMoving();
                        orderedAttack = false;
                        break;
                    }
                    if (CheckRangeToShoot(attackedObject)) {
                        InfoLog.WriteInfo(DoAIPrefix + "Unit:AI: attack -> attack ", EPrefix.AI);
                        TryAttack(attackedObject);
                        //attack, reload etc
                    } else {
                        // out of range - stop
                        if(orderedAttack == false){
                        InfoLog.WriteInfo(DoAIPrefix + "Unit:AI: attack -> stop ", EPrefix.AI);
                        state = UnitState.stopped;

                        }else{
                            InfoLog.WriteInfo(DoAIPrefix + "Unit:AI: attack -> orderedAttack ", EPrefix.AI);
                        state = UnitState.orderedAttack;
                        MoveTo(attackedObject.Position);
                        //override state
                        state = UnitState.orderedAttack; 
                        }
                    }
                    break;
            }
            InfoLog.WriteInfo(DoAIPrefix + " unit:DoAI() end",EPrefix.AI);
		}

        protected bool FindNearestTargetInViewRange(out BoardObject ob) {

            int count;
            Position[] viewSpiral = RangeSpiral(this.ViewRange, out count);
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
                        //TODO erase true;)
                        if (unit.Equals(this)) continue;
                        if (unit.ObjectID.PlayerID != this.ObjectID.PlayerID &&
                            _simulation.Players[unit.ObjectID.PlayerID].TeamID != _simulation.Players[this.ObjectID.PlayerID].TeamID) {
                            ob = unit;
                            InfoLog.WriteInfo("Unit:AI: found unit in view in range < " + this.ViewRange, EPrefix.SimulationInfo);
                            return true;
                        }
                    }

                    buildings = m.Buildings[p.X + spiralPos.X, p.Y + spiralPos.Y];
                    foreach (Building building in buildings) {
                        //TODO erase true;)
                        if (building.ObjectID.PlayerID != this.ObjectID.PlayerID &&
                            _simulation.Players[building.ObjectID.PlayerID].TeamID != _simulation.Players[this.ObjectID.PlayerID].TeamID) {
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

        protected virtual bool IsMoveable(short x, short y, Map map)
        {
            if (map.Units[x, y].Count != 0)
                return false;
            if (map.Buildings[x, y].Count != 0) {
                foreach (Building b in map.Buildings[x, y]) {
                    if (!b.IsPositionRideable(x, y))
                        return false;
                }
                return true;
            }
            return true;
        }

      

        protected bool FindNearestTargetInFireRange(out BoardObject nearest) {
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
                        if (unit.ObjectID.PlayerID != this.ObjectID.PlayerID &&
                            _simulation.Players[unit.ObjectID.PlayerID].TeamID != _simulation.Players[this.ObjectID.PlayerID].TeamID) {
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
                        if (building.ObjectID.PlayerID != this.ObjectID.PlayerID &&
                            _simulation.Players[building.ObjectID.PlayerID].TeamID != _simulation.Players[this.ObjectID.PlayerID].TeamID) {
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
        /// attacks region - manage ammo type.
        /// </summary>
        /// <param name="ob"></param>
        protected void AttackRegion(BoardObject ob) {
            Position s = ob.Position;

            Ammo a = new Ammo(new ObjectID(this.ObjectID.PlayerID,_simulation.Players[this.ObjectID.PlayerID].GenerateObjectID()),
                this.Position, ob.Position, this.AmmoType, this._ammoSpeed,
                this._firePower, this.ammoDamageRange, _simulation);
            InfoLog.WriteInfo(a.ObjectID.ToString() + " for ammunition ", EPrefix.GObj);
            this._simulation.AddAmmo(a);
            this._simulation.OnShoot(a);
            /*

            List<BoardObject> objectsInRange = GetObjectsInRange(s);

            foreach (BoardObject boardObject in objectsInRange) {
                if (boardObject.BoardObjectClass == BoardObjectClass.Building) {
                    _simulation.handleAttackBuilding((Building)boardObject, this);
                } else {
                    _simulation.handleAttackUnit((Unit)boardObject, this);
                }
            }
             */
            this._remainingTurnsToReload = this._reloadTime;
        }

        private List<BoardObject> GetObjectsInRange(Position p) {
            List<BoardObject> objects = new List<BoardObject>();
            List<Position> positions = new List<Position>();
            switch (AmmoType) {
                case AmmoType.Bullet:
                    // object in same position as target
                    positions.Add(p);
                    break;
                    
                case AmmoType.Rocket:
                    // objects in radius from target
                    int max;
                    Position[] tab = Unit.RangeSpiral(this.damageDestroyRange, out max);
                    for (int i = 0; i < max;++i ) {
                        positions.Add(tab[i]);
                    }
                    break;
                case AmmoType.Sonic:
                    // objects from attacker to target
                    Position tmp = this.Position;
                    Queue<Position> path = Unit.Bresenham(ref tmp,ref p);
                    positions.AddRange(path);
                    break;
            }
            foreach (Position position in positions) {
                foreach (BoardObject building in _map.Buildings[position.X, position.Y]) {
                    objects.Add(building);
                }
                foreach (BoardObject unit in _map.Units[position.X, position.Y]) {
                    objects.Add(unit);
                }
            }

            return objects;
        }

        /// <summary>
        /// checks what type of object to attack; manage reload, destroying units, turret rotation
        /// </summary>
        /// <param name="ob"></param>
        protected virtual void TryAttack(BoardObject ob) {

            // Direction.East - no delta
            if (RotateBody(ob,Direction.East) == true) return;
            if (_remainingTurnsToReload == 0) {
                AttackRegion(ob);
            }
        }



        /// <summary>
        /// checks if object is in shooting range
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        protected bool CheckRangeToShoot(BoardObject ob) {
            int r = (int)Math.Floor(Math.Pow(ob.Position.X-this.Position.X,2) + Math.Pow(ob.Position.Y-this.Position.Y,2));
            int range = this.FireRange * this.FireRange;
            //InfoLog.WriteInfo("Unit:AI: in range:" + r + " ?< " + range, EPrefix.SimulationInfo);
            return r <= range;
        }

        /// <summary>
        /// checks if object exists on map.
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        protected bool CheckIfStillExistTarget(BoardObject ob) {
            if (attackingBuilding) {
                Building b = (Building)ob;
                return b.State != Building.BuildingState.destroyed;

            } else {
                Unit u = (Unit)ob;
                return u.state != UnitState.destroyed;
            }
        }
        protected string InfoPrefix()
        {
            return this._name + " Unit: " + this.ObjectID.ToString() + " :";
        }
        public override string ToString()
        {
            return this.ObjectID.ToString() + " name: " + this.Name + " type: " + this.TypeID;
        }
		public virtual bool Move() {
			if (!this.Moving) {
                if (this._lastPosition.X != Position.X && this._lastPosition.Y != Position.Y)
                    this._map.Units[this._lastPosition.X, this._lastPosition.Y].Remove(this);
                this._lastPosition = Position;
                _remainingTurnsInMove = 0;
				return false;
			}

			if (_remainingTurnsInMove == 0) {
				//unit starts to move
				//so we set a new position
				
                //goal reached
                if (_currentPath.Count == 0) {
                    
                    if (this.Position.Equals(_goal))
                    {
                        this._lastPosition = Position;
                        InfoLog.WriteInfo(InfoPrefix() + "Reached goal: " + _goal.ToString(), EPrefix.AStar);
                        return false;
                    }
                    else if (this.Position.Equals(_tmpGoal))
                    {
                        this._lastPosition = Position;
                        InfoLog.WriteInfo(InfoPrefix() + "Reached substitute goal: " + _tmpGoal.ToString(), EPrefix.AStar);
                        if (!MoveTo(_goal))
                            return false;
                    }
                }
				Position newPos = _currentPath.Dequeue();
                if (!IsMoveable(newPos.X, newPos.Y, this._map)) {
                    InfoLog.WriteInfo(InfoPrefix() + "Position: " + newPos.ToString() + "is not moveable", EPrefix.AStar);
                    if (MoveTo(_goal))
                        newPos = _currentPath.Dequeue();
                    else
                        return false;
                }

                InfoLog.WriteInfo(InfoPrefix() + "Moving to position: " + newPos, EPrefix.AStar);
                this._remainingTurnsInMove = this._speed;
                
				//TODO: check newPos;
				/*
				if ( badPos ) {
					this.currentPath = Unit.FindPath(this.Position, newPos, this.map, canCrossMountain, canCrossBuildings, canCrossRock, canCrossTrooper, canCrossTank);
					if (currentPath.Count == 0
					newPos = currentPath.Dequeue();
				}
				*/
                this._map.Units[this._lastPosition.X, this._lastPosition.Y].Remove(this);
				this._map.Units[this.Position.X, this.Position.Y].Remove(this);
                this._map.Units[this.Position.X, this.Position.Y].Remove(this);

				this._lastPosition = Position;
				this.Position = newPos;
                this._map.Units[this.Position.X, this.Position.Y].Remove(this);
				this._map.Units[this.Position.X, this.Position.Y].AddFirst(this);
				if(_simulation.Players.ContainsKey(this.ObjectID.PlayerID))
					_simulation.ClearFogOfWar(this);


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
			}

            if (_map.Units[this.Position.X, this.Position.Y].Count > 1)
            {
                InfoLog.WriteInfo("!%!%!%!%!%!Two units on the same position !%!%!%!%!%!", EPrefix.AStar);
                foreach (Unit u in _map.Units[this.Position.X, this.Position.Y])
                {
                    InfoLog.WriteInfo("Wrong unit: " + u.ToString(), EPrefix.AStar);
                }
            }
			//move unit
			this._remainingTurnsInMove--;

			return true;
		}

		public Unit(ObjectID id, short typeID, String ammo, BoardObjectClass boc, Position pos, Map map, Simulation sim, short ammoDamageRange, short damageDestroyRange, short damageDestroy, short ammoSpeed)
			: base(id, boc, pos) {

			this._typeID = typeID;
			this._map = map;
            this._simulation = sim;
			this._lastPosition = pos;
			this._direction = Direction.North;
			this._currentPath = new Queue<Position>();
            this._damageDestroy = damageDestroy;
            this.damageDestroyRange = damageDestroyRange;
            this.ammoDamageRange = ammoDamageRange;
            this._mapInput = new MapInput(sim.Map);
            this._mapInput.IsMoveable += new MapInput.MoveCheckDelegate(this.IsMoveable);
            this._ammoSpeed = ammoSpeed;
            if(ammo=="Bullet"){
                this._ammoType = AmmoType.Bullet;
            }
            else if(ammo=="Rocket"){
                this._ammoType = AmmoType.Rocket;
            } else if (ammo=="Sonic") {
                this._ammoType = AmmoType.Sonic;
            } else {
                this._ammoType = AmmoType.None;
            }
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

        protected virtual bool RotateBody(BoardObject ob, Direction turretDelta) {
            if (RotateBodyInternal(ob, turretDelta) == false) return false;

            for (int i = 1; i < this.RotationSpeed; ++i) {
                if (RotateBodyInternal(ob, turretDelta) == false)
                    return false;
            }
            return true;
        }

        protected int ConvertToNumber(Direction dir) {
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

        private bool RotateBodyInternal(BoardObject ob, Direction weaponDelta) {
            int alfaTarget = GetAlfa(ob.Position.X - this.Position.X, ob.Position.Y - this.Position.Y);
            int unitRotation = ConvertToNumber(Direction);
            int weaponRotationDelta = ConvertToNumber(weaponDelta);

            int weaponRotation = weaponRotationDelta + unitRotation;


            weaponRotation %= 360;
            int delta = weaponRotation - alfaTarget;
            delta += 360;
            delta %= 360;
            InfoLog.WriteInfo("## weapon rot " + weaponRotationDelta + " unit rot: " + unitRotation + "# target: " + alfaTarget + "### " + delta, EPrefix.SimulationInfo);
            int turn = 0;
            if (delta < 360 - 23 && delta >= 180) {
                // rotate clockwise
                turn = 45;
                unitRotation += turn;
                unitRotation += 360;
                unitRotation %= 360;
                Direction = ConvertToDirection(unitRotation);
                return true;
            } else if (delta > 23 && delta < 180) {
                // rotate counterclockwise
                turn = -45;
                unitRotation += turn;
                unitRotation += 360;
                unitRotation %= 360;
                Direction = ConvertToDirection(unitRotation);

                return true;
            } else {
                return false;
            }
        }

		public virtual bool MoveTo(Position destination) {
            InfoLog.WriteInfo(InfoPrefix() + "Counting path...", EPrefix.AStar);
            _tmpGoal = Position.Invalid;
            //czy cel jest zajety
            if (!IsMoveable(destination.X, destination.Y, _map)) {
                InfoLog.WriteInfo(InfoPrefix() + "Destination is occupied", EPrefix.AStar);
                //cel zajety - poszukiwanie celu zastepczego
                _tmpGoal = UtilsAlgorithm.SurroundSearch(destination, this.MaxTmpDestinationRange, this);
                //czy znaleziono cel zastepczy
                if (_tmpGoal.Equals(destination)) {
                    //nie znaleziono
                    InfoLog.WriteInfo(InfoPrefix() + "Subsittute destination not found", EPrefix.AStar);
                    _tmpGoal = Position.Invalid;
                    _goal = Position.Invalid;
                    this._currentPath.Clear();
                    return false;
                }
                //znaleziono cel zastpeczy szukana jest sciezka do celu zastepszego
                this._currentPath = FindPath(this.Position, _tmpGoal, this._map,
                            this._canCrossMountain, this._canCrossBuildings,
                            this._canCrossRock, this._canCrossTrooper, this._canCrossRock);
                //czy znaleziono sciezke
                if (this._currentPath.Count == 0) {
                    InfoLog.WriteInfo(InfoPrefix() + "Path to subsittute destination not found", EPrefix.AStar);
                    //nie znaleziono sciezki
                    _tmpGoal = Position.Invalid;
                    _goal = Position.Invalid;
                    return false;
                }
                //znaleziono sciezke - w _tmpGoal cel posredni w _goal bezposredni
                InfoLog.WriteInfo(InfoPrefix() + "Counted path to substitute goal: (" + _tmpGoal.X + " , " + _tmpGoal.Y + ")", EPrefix.AStar);
                _goal = destination;
                state = UnitState.moving;
                return true;
            }
            //cel nie jest zajety, szukana jest bezposrednia sciezka do celu
            this._currentPath = FindPath(this.Position, destination, this._map,
                            this._canCrossMountain, this._canCrossBuildings,
                            this._canCrossRock, this._canCrossTrooper, this._canCrossRock);
            if (this._currentPath.Count == 0) {
                //nie znaleziono sciezki
                _tmpGoal = Position.Invalid;
                _goal = Position.Invalid;
                InfoLog.WriteInfo(InfoPrefix() + "Path not found", EPrefix.AStar);
                return false;
            }
            state = UnitState.moving;
            _goal = destination;
            InfoLog.WriteInfo(InfoPrefix() + "Counted path to goal: (" + _goal.X + " , " + _goal.Y + ")", EPrefix.AStar);
            return true;


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

		public Queue<Position> FindPath(Position source, Position dest, Map map,
												bool canCrossMountain, bool canCrossBuildings,
												bool canCrossRock, bool canCrossTrooper, bool canCrossTank) {
            //Queue<Position> path = Bresenham(ref source, ref dest);

			//path.Enqueue(dest);
			//end remove

			//return path;
                                                    this._mapInput.Start = source;
                                                    this._mapInput.Goal = dest;
                                                    Queue<Position> path = AStar.Search<Position>(this._mapInput);
                                                    if (path == null)
                                                        path = new Queue<Position>();
                                                    if (path.Count != 0)
                                                        path.Dequeue();
                                                    return path;
		}

        

		public void StopMoving() {
            InfoLog.WriteInfo("Stopped moving: " + this.ObjectID.ToString(),EPrefix.AStar);
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
				//ClearFogOfWar();
				
				_alreadyOnMap = true;
				return true;
			}
			return false;
		}

        /// <summary>
        /// Sets object to attack (building or unit) by this unit.
        /// </summary>
        /// <param name="objectID"></param>
        public void OrderAttack(BoardObject boardObject,bool isBuilding) {
            attackedObject = boardObject;
            orderedAttack = true;
            if (CheckRangeToShoot(boardObject) == false) {
                MoveTo(boardObject.Position);
                state = UnitState.orderedAttack;
            } else {
                // will shoot the nearest.
                state = UnitState.stopped;
                StopMoving();
            }
            this.attackingBuilding = isBuilding;

            InfoLog.WriteInfo("Unit:AI: attacking!!!! ", EPrefix.SimulationInfo);
        }

		public abstract float getSize();
		public abstract float getMaxHealth();

		public UnitState State {
			get { return state; }
            set { state = value; }
		}

		public short AmmoDamageRange {
			get { return ammoDamageRange; }
			set { ammoDamageRange = value; }
		}

		public short DamageDestroyRange {
			get { return damageDestroyRange; }
			set { damageDestroyRange = value; }
		}

        #region IPositionChecker Members

        public bool CheckPosition(short x, short y) {
            if (x >= 0 && y >= 0 && x < _map.Width && y < _map.Height)
                return IsMoveable(x, y, _map);
            return false;
               
        }

        #endregion
    }
}
