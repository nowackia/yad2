using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;
using Yad.Log.Common;
using Yad.Engine.Common;
using Yad.Engine;

namespace Yad.Board.Common {
	public class Building : BoardObject {
        #region Building 
        
        BuildStatus _bStatus;

        public BuildStatus BuildStatus {
            get { return _bStatus; }
            set { _bStatus = value; }
        }

        #endregion

        private BuildingData _buildingData;
		private int _currentHealth;

		//private bool attackingBuilding;
		private BoardObject attacked;
		private short roundToReload;
        private short _ammoSpeed = 3;//TODO move to xml settings

		Direction _direction;

		private AmmoType ammoType;

		public AmmoType AmmoType {
			get { return ammoType; }
		}


		public enum BuildingState {
			constructing,
			normal,
			creating,
			destroyed
		}

		private BuildingState state;

		public BuildingState State {
			get { return state; }
			set { state = value; }
		}


		Map _map;
		protected Simulation _simulation;
		bool _alreadyOnMap = false;

		public Building(ObjectID id, BuildingData bd, Map map, Position pos, Simulation sim)
			: base(id, BoardObjectClass.Building, pos) {
			this._direction = Direction.North;
			this._buildingData = bd;
			this._map = map;
			this.state = BuildingState.constructing;
			this.Health = bd.__Health;
			this.roundToReload = 0;
			this._simulation = sim;
			String ammo = bd.AmmoType;
			if (ammo == "Bullet") {
				this.ammoType = AmmoType.Bullet;
			} else if (ammo == "Rocket") {
				this.ammoType = AmmoType.Rocket;
			} else if (ammo == "Sonic") {
				this.ammoType = AmmoType.Sonic;
			} else {
				this.ammoType = AmmoType.None;
			}
		}

		public void Destroy() {
			state = BuildingState.destroyed;
		}

		public short TypeID {
			get { return this._buildingData.TypeID; }
		}

		public int Health {
			get {
				return _currentHealth;
			}
			set {
				_currentHealth = value;
			}
		}

		public BuildingData BuildingData {
			get { return this._buildingData; }
		}

		public short Width {
			get { return this._buildingData.Size.X; }
		}

		public short Height {
			get { return this._buildingData.Size.Y; }
		}

		public bool PlaceOnMap() {
			if (!_alreadyOnMap) {
				for (int x = 0; x < this.Width; x++) {
					for (int y = 0; y < this.Height; y++) {
						this._map.Buildings[x + Position.X, y + Position.Y].AddLast(this);
					}
				}
				_alreadyOnMap = true;
				return true;
			}
			return false;
		}

		/*
		public void ClearFogOfWar() {
			int viewRange = _buildingData.ViewRange;

			for (int x = -viewRange + Position.X; x <= viewRange + Position.X; x++) {
				for (int y = -viewRange + Position.Y; y <= viewRange + Position.Y; y++) {
					if (x < 0 || y < 0 || x > _map.Width - 1 || y > _map.Height - 1) {
						continue;
					}
					_map.FogOfWar[x, y] = false;
				}
			}
		}
		*/


		/// <summary>
		/// manage building
		/// there are buildings which can shoot - some ai
		/// </summary>
		public void DoAI() {
            if (_bStatus != null) {
                if (_bStatus.DoTurn()) {
                    this.state = BuildingState.normal;
                }
            }
            if (this.State == BuildingState.normal)
            {
                if (this.BuildingData.FirePower == 0) return;
                if (roundToReload > 0)
                {
                    // reload time - cant shoot.
                    roundToReload--;
                    return;
                }
                if (FindNearestTargetInFireRange(out attacked))
                {
                    /*
                     * actions for looking for target, shooting
                     * 
                     */
                    TryAttack(attacked);
                    roundToReload = BuildingData.__ReloadTime;

                }
            }
		}

		/// <summary>
		/// attacks region - manage ammo type.
		/// </summary>
		/// <param name="ob"></param>
		protected void AttackRegion(BoardObject ob) {
            Position s = ob.Position;

            Ammo a = new Ammo(new ObjectID(this.ObjectID.PlayerID, _simulation.Players[this.ObjectID.PlayerID].GenerateObjectID()),
                this.Position, ob.Position, this.AmmoType, this._ammoSpeed,
                this.BuildingData.FirePower, this.BuildingData.AmmoDamageRange, _simulation);
            InfoLog.WriteInfo(a.ObjectID.ToString() + " for ammunition ", EPrefix.GObj);
            this._simulation.AddAmmo(a);
			/*Position s = ob.Position;
			List<BoardObject> objectsInRange = GetObjectsInRange(s);

			foreach (BoardObject boardObject in objectsInRange) {
				if (boardObject.BoardObjectClass == BoardObjectClass.Building) {
					_simulation.handleAttackBuilding((Building)boardObject, this);
				} else {
					_simulation.handleAttackUnit((Unit)boardObject, this);
				}
			}*/
		}

		private List<BoardObject> GetObjectsInRange(Position p) {
			List<BoardObject> objects = new List<BoardObject>();
			List<Position> positions = new List<Position>();
			switch (ammoType) {
				case AmmoType.Bullet:
					// object in same position as target
					positions.Add(p);
					break;

				case AmmoType.Rocket:
					// objects in radius from target
					int max;
					Position[] tab = Unit.RangeSpiral(this.BuildingData.AmmoDamageRange, out max);
					for (int i = 0; i < max; ++i) {
						Position spiralPos = tab[i];
						if (p.X + spiralPos.X >= 0
					&& p.X + spiralPos.X < _map.Width
					&& p.Y + spiralPos.Y >= 0
					&& p.Y + spiralPos.Y < _map.Height) {
							positions.Add(new Position(p.X + spiralPos.X, p.Y + spiralPos.Y));
						}
					}
					break;
				case AmmoType.Sonic:
					// objects from attacker to target
					Position tmp = this.Position;
					Queue<Position> path = Unit.Bresenham(ref tmp, ref p);
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
		protected void TryAttack(BoardObject ob) {
			// rotate turret
			if (RotateIfNeeded(ob) == true) return;
			if (this.roundToReload == 0) {
				AttackRegion(ob);
			}
		}

		protected bool RotateIfNeeded(BoardObject ob) {
			// check first rotation
			if (this.BuildingData.IsTurret && RotateIfNeededInternal(ob) == false) return false;
			//for (int i = 1; i < RotationSpeed; ++i) {
			//    if (RotateIfNeededInternal(ob) == false) break;
			//}
			// rotated more than once.
			return true;
		}



		private bool FindNearestTargetInFireRange(out BoardObject nearest) {
			int count;
			Position[] viewSpiral = RangeSpiral(this.BuildingData.FireRange, out count);
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
							//attackingBuilding = false;
							InfoLog.WriteInfo("Building:AI: found unit in fire range < " + this.BuildingData.FireRange, EPrefix.SimulationInfo);
							nearest = unit;
							return true;
						}
					}

					buildings = m.Buildings[p.X + spiralPos.X, p.Y + spiralPos.Y];
					foreach (Building building in buildings) {
						// erase true;)
						if (building.ObjectID.PlayerID != this.ObjectID.PlayerID) {
							//attackingBuilding = true;
							nearest = building;
							InfoLog.WriteInfo("Building:AI: found building in fire range < " + this.BuildingData.FireRange, EPrefix.SimulationInfo);
							return true;
						}
					}
				}
			}


			nearest = null;
			return false;
		}

		

		/// <summary>
		/// rotate if target is out of range
		/// </summary>
		/// <param name="ob">target</param>
		/// <returns>if rotation was needed</returns>
		protected bool RotateIfNeededInternal(BoardObject ob) {


			int alfaTarget = GetAlfa(ob.Position.X - this.Position.X, ob.Position.Y - this.Position.Y);
			int turretRotationDelta = ConvertToNumber(Direction);


			int delta = turretRotationDelta - alfaTarget;
			delta += 360;
			delta %= 360;
			int turn = 0;
			if (delta < 360 - 23 && delta >= 180) {
				// rotate clockwise
				turn = 45;
				turretRotationDelta += turn;
				turretRotationDelta += 360;
				turretRotationDelta %= 360;
				_direction = ConvertToDirection(turretRotationDelta);
				return true;
			} else if (delta > 23 && delta < 180) {
				// rotate counterclockwise
				turn = -45;
				turretRotationDelta += turn;
				turretRotationDelta += 360;
				turretRotationDelta %= 360;
				_direction = ConvertToDirection(turretRotationDelta);

				return true;
			} else {
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

		


		public float getMaxHealth() {
			return _buildingData.Health;
		}

		public Direction Direction {
			get { return _direction; }
		}

        public bool IsPositionRideable(short x, short y) {
            for (int i = 0; i < BuildingData.RideableFields.Count; ++i) {
                int fieldNo = BuildingData.RideableFields[i];
                short fx = ((short)(fieldNo % Width));
                short fy = ((short)(fieldNo / Width));
                if ((Position.X + fx) == x && (Position.Y + fy) == y)
                    return true;
            }
            return false;
        }
		public static bool CheckBuildPosition(BuildingData bd, Position pos, Map map, short playerID) {
			//czy puste miejsce
			if (!map.CheckSpace(pos, bd.Size.X, bd.Size.Y)) {
				return false;
			}

			for (int x = pos.X; x < pos.X + bd.Size.X; x++) {
				for (int y = pos.Y; y < pos.Y + bd.Size.Y; y++) {
					if (map.Tiles[x, y] != TileType.Rock)
						return false;
				}
			}
			//czy styka z innym budynkiem

			if (bd.Name.Equals("ConstructionYard")) {
				return true;
			}

			//lewy bok
			int left = pos.X - 1;
			if (left >= 0) {
				for (int y = pos.Y - 1; y < pos.Y + bd.Size.Y + 1; y++) {
					if (y < 0) //moze sie zwiekszyc
						continue;
					if (y >= map.Height) //mniejsze juz nie bedzie...
						break;

					foreach (Building b in map.Buildings[left, y]) {
						if (b.ObjectID.PlayerID == playerID) {
							return true;
						}
					}
				}
			}

			//prawy bok
			int right = pos.X + bd.Size.X;
			if (right < map.Width) {
				for (int y = pos.Y - 1; y < pos.Y + bd.Size.Y + 1; y++) {
					if (y < 0) //moze sie zwiekszyc
						continue;
					if (y >= map.Height) //mniejsze juz nie bedzie...
						break;

					foreach (Building b in map.Buildings[right, y]) {
						if (b.ObjectID.PlayerID == playerID) {
							return true;
						}
					}
				}
			}

			//dolna krawedz
			int bottom = pos.Y - 1;
			if (bottom >= 0) {
				for (int x = pos.X - 1; x < pos.X + bd.Size.X + 1; x++) {
					if (x < 0) //moze sie zwiekszyc
						continue;
					if (x >= map.Width) //mniejsze juz nie bedzie...
						break;

					foreach (Building b in map.Buildings[x, bottom]) {
						if (b.ObjectID.PlayerID == playerID) {
							return true;
						}
					}
				}
			}

			//gorna
			int top = pos.Y + bd.Size.Y;
			if (top < map.Height) {
				for (int x = pos.X - 1; x < pos.X + bd.Size.X + 1; x++) {
					if (x < 0) //moze sie zwiekszyc
						continue;
					if (x >= map.Width) //mniejsze juz nie bedzie...
						break;

					foreach (Building b in map.Buildings[x, top]) {
						if (b.ObjectID.PlayerID == playerID) {
							return true;
						}
					}
				}
			}

			return false;
		}
	}
}
