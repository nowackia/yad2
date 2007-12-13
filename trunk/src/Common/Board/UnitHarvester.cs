using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;
using Yad.Engine.Common;
using Yad.Log.Common;

namespace Yad.Board.Common {
    public delegate void SpiceUnloadDelegate(short playerID, int credits);

	public class UnitHarvester : Unit {


        private event SpiceUnloadDelegate spiceUnload;
        int spiceCounter;

        // local states

        private enum HarvestingState { harvesting, returningToBase, unloading }

        HarvestingState harvestingState = HarvestingState.harvesting;

        int refineryFindingCounter = 10;
        int spiceFindingCounter = 100;
		UnitHarvesterData _harvesterData;

        bool knowsLastKnownPosition = false;
        Position lastKnownSpicePosition;

        public event SpiceUnloadDelegate SpiceUnload {
            add {
                spiceUnload += value;
            }
            remove {
                spiceUnload -= value;
            }
        }
		public UnitHarvester(ObjectID id, UnitHarvesterData ud, Position pos, Map map, Simulation sim,int speed)
			: base(id, ud.TypeID,null, BoardObjectClass.UnitHarvester, pos, map,sim,0,ud.__DamageDestroyRange,ud.__DamageDestroy,0) {
			_harvesterData = ud;
			this.Speed = ud.Speed;
			this._viewRange = ud.ViewRange;
            this.state = UnitState.stopped;
            this.MaxHealth = this.Health = ud.__Health;
            
		}



		public override void Destroy() {
			base.Destroy();// give damage destroy
		}


        private void MoveTo(Position loc, HarvestingState hState) {
            base.MoveTo(loc);

        }

        public override bool MoveTo(Position destination) {
            bool result = base.MoveTo(destination);
            this.harvestingState = HarvestingState.harvesting;
            return result;
        }
        

		public override void DoAI() {
            InfoLog.WriteInfo(DoAIPrefix + " harvester DoAI", EPrefix.AI);
            if (_remainingTurnsInMove > 0 && Moving && state == UnitState.stopped) {
                Move();
                InfoLog.WriteInfo(DoAIPrefix + " harvester moved", EPrefix.AI);
                return;
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
                        this.spiceFindingCounter = 100;
                        int spiceOnLocaltion = _map.Spice[Position.X, Position.Y];
                        if (spiceOnLocaltion == 0) {
                            // seek for spice
                         
                            if (FindNearestSpice(this.Position, out loc)) {
                                // found spice
                                state = UnitState.moving;
                                knowsLastKnownPosition = true;
                                lastKnownSpicePosition = loc;
                                MoveTo(loc, HarvestingState.harvesting);
                                InfoLog.WriteInfo(DoAIPrefix + "harvester searching for spice", EPrefix.AI);
                            } else {
                                // no spice visible - stopping but still seeking for spice
                                state = UnitState.stopped;
                               
                            }
                        } else if( spiceOnLocaltion > 0) {
                            // collect spise
                            
                            this.spiceCounter+=10;
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
                            knowsLastKnownPosition = true;
                            lastKnownSpicePosition = loc;
                            MoveTo(loc, HarvestingState.harvesting);
                            InfoLog.WriteInfo(DoAIPrefix + "harvester searching for spice", EPrefix.AI);
                            break;
                        }
                        if (this.Position.X == lastKnownSpicePosition.X &&
                                   this.Position.Y == lastKnownSpicePosition.Y) {
                            knowsLastKnownPosition = false;


                        }
                        if (knowsLastKnownPosition) {
                            state = UnitState.moving;
                            MoveTo(lastKnownSpicePosition, HarvestingState.harvesting);
                            state = UnitState.moving;
                            break;
                        }
                        if (this.spiceFindingCounter-- == 0) {
                            // przeliczyl sie.
                            this.spiceFindingCounter = 100; // magic number! - przeniesc do ustawien
                            this.state = UnitState.stopped;
                            this.harvestingState = HarvestingState.returningToBase;
                            break;
                        }
                        break;

                    case UnitState.moving:
                        this.spiceFindingCounter = 100;
                        if (Move() == false) {
                            // stopped moving. 
                            state = UnitState.stopped;
                        }
                        break;
                }
                #endregion

            } else if (harvestingState == HarvestingState.returningToBase) {
                #region returning to base

                if (FindNearestSpice(this.Position, out loc)) {
                    knowsLastKnownPosition = true;
                    lastKnownSpicePosition = loc;
                }

                switch (state) {
                    case UnitState.moving:
                        // returning to base
                        InfoLog.WriteInfo(DoAIPrefix + "harvester returning to base", EPrefix.AI);
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
                            if (this.refineryFindingCounter >= 9) {
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
                                MoveTo(loc, HarvestingState.returningToBase);
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
                InfoLog.WriteInfo(DoAIPrefix + "harvester unloading", EPrefix.AI);
                _simulation.Players[this.ObjectID.PlayerID].Credits += spiceCounter;
                if (spiceUnload != null)
                    spiceUnload(this.ObjectID.PlayerID, spiceCounter);
                this.spiceCounter = 0;
                this.harvestingState = HarvestingState.harvesting;
                this.state = UnitState.stopped;
            }
            InfoLog.WriteInfo(DoAIPrefix + "harvester end DoAI",EPrefix.AI);



		}

        private bool FindNearestRefinery(Position position, out Position loc) {
            // harvester knows whole map?
            Position[] veryBigSpiral;
            int count;

            //veryBigSpiral = RangeSpiral(_map.Width > _map.Height ? _map.Width : _map.Height, out count);
            veryBigSpiral = RangeSpiral(this.HarvesterData.ViewRange, out count);
            Position spiralPos;
            Position p = this.Position;

            ICollection<Building> buildings = this._simulation.Players[this.ObjectID.PlayerID].GetAllBuildings();


            Building min = null;
            double minRange = double.MaxValue;
            foreach (Building b in buildings) {
                if (b.BuildingData.IsRefinery == true) {
                    double tmp = Math.Sqrt(Math.Pow(b.Position.X - this.Position.X, 2) + Math.Pow(b.Position.Y - this.Position.Y,2));
                    if (tmp < minRange) {
                        tmp = minRange;
                        min = b;
                    }
                }

            }

            if (min != null) {
                Position rideableField;
                if (GetRideableField(min, out rideableField)) {
                    loc = rideableField;
                    return true;
                }

            }

            //for (int i = 0; i < count; ++i) {
            //    spiralPos = veryBigSpiral[i];

            //    if (p.X + spiralPos.X >= 0
            //            && p.X + spiralPos.X < _map.Width
            //            && p.Y + spiralPos.Y >= 0
            //            && p.Y + spiralPos.Y < _map.Height) {
            //        // check if spiral exits
            //        ICollection<Building> buildings = _map.Buildings[p.X + spiralPos.X, p.Y + spiralPos.Y];
            //        Building refinery = null;
            //        foreach (Building building in buildings) {
            //            if (building.BuildingData.IsRefinery == true && building.ObjectID.PlayerID == this.ObjectID.PlayerID) {
            //                refinery = building;
            //                break;
            //            }
            //        }
            //        if (refinery != null) {
            //            Position rideableField;
            //            if (GetRideableField(refinery, out rideableField)) {
            //                loc = rideableField;
            //                return true;
            //            }
            //        }

            //    }
            //}
            loc = p;
            return false;
        }

        

        private bool GetRideableField(Building refinery, out Position rideableField) {
            if (refinery.BuildingData.RideableFields.Count == 0) {
                rideableField = refinery.Position;
                return false;
            }
            short field = refinery.BuildingData.RideableFields[0];
            int rowY = (field) / refinery.Width;
            int rowX = (field) % refinery.Width;
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

            //veryBigSpiral = RangeSpiral(_map.Width > _map.Height ? _map.Width : _map.Height, out count);
            veryBigSpiral = RangeSpiral(this.HarvesterData.ViewRange, out count);
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

        protected override bool IsMoveable(short x, short y, Map map)
        {
            if (base.IsMoveable(x, y, map))
            {
                if (_map.Tiles[x, y] == TileType.Mountain)
                    return false;
                return true;
            }
            return false;
        }
	}
}
