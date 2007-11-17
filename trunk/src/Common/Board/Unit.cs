using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;

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

		//used for moving
		protected bool canCrossMountain = false, canCrossBuildings = false, canCrossRock = true, canCrossTrooper = false, canCrossTank = false;

		protected short remainingTurnsToMove = 0;
		protected Position lastPosition; //used while moving to remember last pos
		protected Queue<Position> currentPath;
		//BoardObject.Position - current position, while moving unit is always at this coordinates

		protected short typeID;

		protected Map map;

		//TODO : RS implement some base AI?
		//KŒ: yes ;P
		public abstract void Destroy();

		public virtual void Move() {
			if (!this.Moving) {
				return;
			}

			if (remainingTurnsToMove == this.speed) {
				//unit starts to move
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
				
				this.Position = newPos;

				this.map.Units[this.Position.X, this.Position.Y].AddFirst(this);				
			}

			//move unit
			this.remainingTurnsToMove--;


			return;
		}

		public abstract void DoAI();

		public Unit(short playerID, int unitID, short typeID, BoardObjectClass boc, Position pos, Map map)
			: base(playerID, unitID, boc, pos) {
			this.typeID = typeID;
			this.map = map;
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
			
			this.remainingTurnsToMove = this.speed;
		}

		/// <summary>
		/// Indicates whether the unit is moving:
		/// - still have a move to finish
		/// or
		/// - have destination queued
		/// </summary>
		public bool Moving {
			get { return (this.remainingTurnsToMove != 0) || (this.currentPath.Count != 0); }
		}

		public static Queue<Position> FindPath(Position source, Position dest, Map map,
												bool canCrossMountain, bool canCrossBuildings,
												bool canCrossRock, bool canCrossTrooper, bool canCrossTank) {
			Queue<Position> path = new Queue<Position>();
			
			
			//TODO Go-Go-Gadget!

			//remove
			path.Enqueue(dest);
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
	}
}
