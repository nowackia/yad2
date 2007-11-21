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
		protected short damageDestroy;
		protected String name;
		protected short fireRange;
		protected AmmoType ammoType;
		private short speed;
		protected short reloadTime;
		protected short health;
		protected short viewRange;
		protected short rotationSpeed;
		protected Direction direction;

		//used for moving
		protected bool canCrossMountain = false, canCrossBuildings = false, canCrossRock = true, canCrossTrooper = false, canCrossTank = false;

		protected short _remainingTurnsInMove = 0;
		protected Position lastPosition; //used while moving to remember last pos
		protected Queue<Position> currentPath;
		//BoardObject.Position - current position, while moving unit is always at this coordinates

		protected short typeID;

		//map-related
		protected Map map;
		bool _alreadyOnMap = false;

		//TODO : RS implement some base AI?
		//KŒ: yes ;P
		public virtual void Destroy() {
			InfoLog.WriteInfo("Unit:Destroy() Not implemented", EPrefix.SimulationInfo);
		}
		public virtual void DoAI() {
			InfoLog.WriteInfo("Unit:DoAI() Not implemented", EPrefix.SimulationInfo);
		}
		public virtual void Move() {
			if (!this.Moving) {
				return;
			}

			if (_remainingTurnsInMove == 0) {
				//unit starts to move
				//so we set a new position
				this._remainingTurnsInMove = this.speed;

				Position newPos = currentPath.Dequeue();

				//TODO: check newPos;
				/*
				if ( badPos ) {
					this.currentPath = Unit.FindPath(this.Position, newPos, this.map, canCrossMountain, canCrossBuildings, canCrossRock, canCrossTrooper, canCrossTank);
					if (currentPath.Count == 0
					newPos = currentPath.Dequeue();
				}
				*/

				this.map.Units[this.Position.X, this.Position.Y].Remove(this);

				this.lastPosition = Position;
				this.Position = newPos;

				this.map.Units[this.Position.X, this.Position.Y].AddFirst(this);

				//set new direction
				short dx = (short)(newPos.X - lastPosition.X);
				short dy = (short)(newPos.Y - lastPosition.Y);
				this.direction = Direction.None;
				if (dx > 0) {
					direction |= Direction.East;
				} else if (dx < 0) {
					direction |= Direction.West;
				}
				if (dy > 0) {
					direction |= Direction.North;
				} else if (dy < 0) {
					direction |= Direction.South;
				}
			}

			//move unit
			this._remainingTurnsInMove--;

			return;
		}

		public Unit(short playerID, int unitID, short typeID, BoardObjectClass boc, Position pos, Map map)
			: base(playerID, unitID, boc, pos) {
			this.typeID = typeID;
			this.map = map;
			this.lastPosition = pos;
			this.direction = Direction.North;
			this.currentPath = new Queue<Position>();
		}

		public AmmoType AmmoType {
			get { return ammoType; }
		}

		public int FireRange {
			get { return fireRange; }
		}

		public int ReloadTime {
			get { return reloadTime; }
		}

		public int Health {
			get { return health; }
		}

		public int ViewRange {
			get { return viewRange; }
		}

		public int RotationSpeed {
			get { return rotationSpeed; }
		}

		public String Name {
			get { return name; }
		}

		public int DamageDestroy {
			get { return damageDestroy; }
		}

		public Position DestinationPoint {
			get {
				if (this.currentPath.Count == 0) {
					return this.Position;
				}

				return this.currentPath.ToArray()[currentPath.Count - 1];
			}
			//set { destinationPoint = value; }
		}

		public short TypeID {
			get { return this.typeID; }
		}

		public void MoveTo(Position destination) {
			//we can override old path
			this.currentPath = FindPath(this.Position, destination, this.map,
										this.canCrossMountain, this.canCrossBuildings,
										this.canCrossRock, this.canCrossTrooper, this.canCrossRock);

			//this._remainingTurnsInMove = this.speed;
		}

		/// <summary>
		/// Indicates whether the unit is moving:
		/// - still have a move to finish
		/// or
		/// - have destination queued
		/// </summary>
		public bool Moving {
			get { return (this._remainingTurnsInMove != 0) || (this.currentPath.Count != 0); }
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
			this.currentPath.Clear();
		}

		public short Speed {
			get { return this.speed; }
			set {
				if (value == 0) {
					throw new ArgumentOutOfRangeException("Speed must be greater than 0");
				}
				this.speed = value;
			}
		}

		public Position LastPosition {
			get { return this.lastPosition; }
		}

		public int RemainingTurnsInMove {
			get { return this._remainingTurnsInMove; }
		}

		public Direction Direction {
			get { return direction; }
			set { direction = value; }
		}

		public bool PlaceOnMap() {
			if (!_alreadyOnMap) {
				this.map.Units[this.Position.X, this.Position.Y].AddFirst(this);
				_alreadyOnMap = true;
				return true;
			}
			return false;
		}
	}
}
