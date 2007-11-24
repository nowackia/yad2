using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;
using Yad.Log.Common;
using Yad.Engine.Common;

namespace Yad.Board.Common {
	public class Building : BoardObject {
		private BuildingData _buildingData;
		private int _currentHealth;

        private bool attackingBuilding;
        private BoardObject attacked;
        private short roundToReload;

        public enum BuildingState{
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
			this._buildingData = bd;
			this._map = map;
            this.state = BuildingState.constructing;
            this.Health = bd.__Health;
            this.roundToReload = 0;
            this._simulation = sim;
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
				return true;
			}
			return false;
		}

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

        /// <summary>
        /// manage building
        /// there are buildings which can shoot - some ai
        /// </summary>
        public void DoAI() {
            /*
             * common actions:
             * building units,
             * 
             * 
             */

            if (this.BuildingData.FirePower == 0) return;
            if (roundToReload > 0) {
                // reload time - cant shoot.
                roundToReload--;
                return;
            }
            if (FindNearestTargetInFireRange(out attacked)) {
                /*
                 * actions for looking for target, shooting
                 * 
                 */
                roundToReload = BuildingData.__ReloadTime;
                if (attackingBuilding) {
                    _simulation.handleAttackBuilding((Building)attacked, this);
                } else {
                    _simulation.handleAttackUnit((Unit)attacked, this);
                }
            }
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
                            attackingBuilding = false;
                            InfoLog.WriteInfo("Building:AI: found unit in fire range < " + this.BuildingData.FireRange, EPrefix.SimulationInfo);
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
                            InfoLog.WriteInfo("Building:AI: found building in fire range < " + this.BuildingData.FireRange, EPrefix.SimulationInfo);
                            return true;
                        }
                    }
                }
            }


            nearest = null;
            return false;
        }
    }
}
