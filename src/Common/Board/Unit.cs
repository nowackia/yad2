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

		//map-related
		protected Map _map;
		bool _alreadyOnMap = false;

		//TODO : RS implement some base AI?
		public virtual void Destroy() {
			InfoLog.WriteInfo("Unit:Destroy() Not implemented", EPrefix.SimulationInfo);
		}
		public virtual void DoAI() {
			InfoLog.WriteInfo("Unit:DoAI() Not implemented", EPrefix.SimulationInfo);
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
	}
}
