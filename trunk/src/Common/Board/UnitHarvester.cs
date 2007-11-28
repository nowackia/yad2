using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;
using Yad.Engine.Common;

namespace Yad.Board.Common {
	public class UnitHarvester : Unit {



        int spiceCounter;

        // local states

        private enum HarvestingState { harvesting, returningToBase, unloading }

        HarvestingState harvestingState = HarvestingState.harvesting;

        int refineryFindingCounter = 10;

		UnitHarvesterData _harvesterData;	

		public UnitHarvester(ObjectID id, UnitHarvesterData ud, Position pos, Map map, Simulation sim,int speed)
			: base(id, ud.TypeID,null, BoardObjectClass.UnitHarvester, pos, map,sim,0,ud.__DamageDestroyRange,ud.__DamageDestroy) {
			_harvesterData = ud;
			this.Speed = ud.Speed;
			this._viewRange = ud.ViewRange;
            this.state = UnitState.stopped;
            this.MaxHealth = this.Health = ud.__Health;
            
		}



		public override void Destroy() {
			base.Destroy();// give damage destroy
		}


		public override void DoAI() {
            if (_remainingTurnsInMove > 0 && Moving && state == UnitState.stopped) {
                Move();
            }
            Position loc;
            if (harvestingState == HarvestingState.harvesting) {
                if (this.spiceCounter == this.HarvesterData.__Capacity) {
                    // harvester is full
                    this.harvestingState = HarvestingState.returningToBase;
                    this.state = UnitState.stopped;
                    return;
                }
                #region seeking for spice
                switch (state) {
                    case UnitState.harvesting:                        
                        int spiceOnLocaltion = _map.Spice[Position.X, Position.Y];
                        if (spiceOnLocaltion == 0) {
                            // seek for spice
                         
                            if (FindNearestSpice(this.Position, out loc)) {
                                // found spice
                                state = UnitState.moving;
                                MoveTo(loc);
                            } else {
                                // no spice visible - stopping but still seeking for spice
                                state = UnitState.stopped;
                            }
                        } else {
                            // collect spise
                            ++this.spiceCounter;
                            _map.Spice[Position.X, Position.Y]--;
                        }
                        break;
                    case UnitState.stopped:
                        if (_map.Spice[Position.X, Position.Y] > 0) {
                            state = UnitState.harvesting;
                            break;
                        }
                        if (FindNearestSpice(this.Position, out loc)) {
                            // found spice
                            state = UnitState.moving;
                            MoveTo(loc);
                        }
                        break;

                    case UnitState.moving:
                        if (Move() == false) {
                            // stopped moving. 
                            state = UnitState.stopped;
                        }
                        break;
                }
                #endregion

            } else if (harvestingState == HarvestingState.returningToBase) {
                #region returning to base
                switch (state) {
                    case UnitState.moving:
                        // returning to base
                        if (Move() == false) {
                            // destination reached
                            this.state = UnitState.stopped;                            
                        }

                        break;

                    case UnitState.stopped:
                        // refinery reached or refinery destroyed
                        if (RefineryReached()) {
                            harvestingState = HarvestingState.unloading;
                        } else {
                            bool canFind;
                            if (this.refineryFindingCounter == 9) {
                                canFind = true;
                            } else {
                                if (this.refineryFindingCounter == 0)
                                    this.refineryFindingCounter = 10;
                                canFind = false;
                            }

                            this.refineryFindingCounter--;
                            if (canFind == false) return;
                            if (FindNearestRefinery(this.Position, out loc)) {
                                // found refinery
                                state = UnitState.moving;
                                MoveTo(loc);
                            } else {
                                // no refinery found
                                state = UnitState.stopped;
                            }
                        }

                        break;
                }

                #endregion



            } else {
                // unloading
                this.spiceCounter = 0;
                this.harvestingState = HarvestingState.harvesting;
                this.state = UnitState.stopped;
            }



		}

        private bool FindNearestRefinery(Position position, out Position loc) {
            // harvester knows whole map?
            Position[] veryBigSpiral;
            int count;

            veryBigSpiral = RangeSpiral(_map.Width > _map.Height ? _map.Width : _map.Height, out count);
            Position spiralPos;
            Position p = this.Position;
            for (int i = 0; i < count; ++i) {
                spiralPos = veryBigSpiral[i];

                if (p.X + spiralPos.X >= 0
                        && p.X + spiralPos.X < _map.Width
                        && p.Y + spiralPos.Y >= 0
                        && p.Y + spiralPos.Y < _map.Height) {
                    // check if spiral exits
                    ICollection<Building> buildings = _map.Buildings[p.X + spiralPos.X, p.Y + spiralPos.Y];
                    Building refinery = null;
                    foreach (Building building in buildings) {
                        if (building.BuildingData.IsRefinery == true) {
                            refinery = building;
                            break;
                        }
                    }
                    if (refinery != null) {
                        Position rideableField;
                        if (GetRideableField(refinery, out rideableField)) {
                            loc = rideableField;
                            return true;
                        }
                    }

                }
            }
            loc = p;
            return false;
        }

        private bool GetRideableField(Building refinery, out Position rideableField) {
            if (refinery.BuildingData.RideableFields.Count == 0) {
                rideableField = refinery.Position;
                return false;
            }
            short field = refinery.BuildingData.RideableFields[0];
            int rowY = (field+1) / refinery.Height;
            int rowX = (field + 1) % refinery.Width;
            rideableField = new Position(refinery.Position.X + rowX, refinery.Position.Y + rowY);
            return true;
        }

        private bool RefineryReached() {
            ICollection<Building> buildings = _map.Buildings[this.Position.X, this.Position.Y];
            
            foreach (Building b in buildings) {
                if (b.BuildingData.IsRefinery) {
                    //rideable field.
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// searches map for spice - 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="loc"></param>
        /// <returns></returns>
        private bool FindNearestSpice(Position position, out Position loc) {
            // harvester knows whole map?
            Position[] veryBigSpiral;
            int count;

            veryBigSpiral = RangeSpiral(_map.Width > _map.Height ? _map.Width : _map.Height, out count);
            Position spiralPos;
            Position p = this.Position;
            for(int i =0;i< count ;++i){
                spiralPos = veryBigSpiral[i];

                if (p.X + spiralPos.X >= 0
                        && p.X + spiralPos.X < _map.Width
                        && p.Y + spiralPos.Y >= 0
                        && p.Y + spiralPos.Y < _map.Height) {
                    // check if spiral exits

                    if (_map.Spice[p.X+ spiralPos.X, p.Y + spiralPos.Y] > 0) {
                        // even 1 spice 
                        loc = new Position(p.X+ spiralPos.X, p.Y + spiralPos.Y);
                        return true;
                    }

                }
            }
            loc = p;
            return false;
        }

		public UnitHarvesterData HarvesterData {
			get { return _harvesterData; }
		}

		public override float getSize() {
			return _harvesterData.Size;
		}

		public override float getMaxHealth() {
			return _harvesterData.Health;
		}
	}
}
