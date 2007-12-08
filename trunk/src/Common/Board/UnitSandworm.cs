using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config.Common;
using Yad.Engine.Common;
using Yad.Log.Common;

namespace Yad.Board.Common {
	public class UnitSandworm : Unit {
		UnitSandwormData _sandwormData;
		Position nearest;
		public UnitSandworm(ObjectID id, UnitSandwormData ud, Position pos, Map map, Simulation sim, int speed)
			: base(id, ud.TypeID, null, Yad.Config.BoardObjectClass.UnitSandworm, pos, map, sim, 0, 0, 0) {
			_sandwormData = ud;
			this.Speed = ud.Speed;
			this.MaxHealth = this.Health = ud.__Health;
			//this.FirePower = ud.
			//this._viewRange = ud.ViewRange;
			//this._fireRange = ud.FireRange;
			//this._reloadTime = ud.ReloadTime;
		}

		public UnitSandwormData SandwormData {
			get { return _sandwormData; }
		}

		public override float getSize() {
			return _sandwormData.Size;
		}

		public override float getMaxHealth() {
			return _sandwormData.Health;
		}

		protected override bool IsMoveable(short x, short y, Map map) {
			if (_map.Tiles[x, y] != TileType.Sand)
				return false;
			return true;
		}

		public override void DoAI() {
			InfoLog.WriteInfo("Unit:DoAI()", EPrefix.SimulationInfo);
			if (_remainingTurnsToReload > 0) --_remainingTurnsToReload;
			if (_remainingTurnsInMove > 0 && Moving && state == UnitState.stopped) {
				Move();
			}
			switch (state) {
				case UnitState.moving:
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
					if (FindNearestTargetOnSandInViewRange(out nearest1)) {
						InfoLog.WriteInfo("Unit:AI: chasing -> stop ", EPrefix.SimulationInfo);
						//state = UnitState.chasing;
						if (!(nearest.X==nearest1.Position.X && nearest.Y == nearest1.Position.Y)) {
							StopMoving();
							state = UnitState.stopped;
							StopMoving();
						}
							
					}
						if (Move() == false) {
							InfoLog.WriteInfo("Unit:AI: chasing -> stop ", EPrefix.SimulationInfo);
							state = UnitState.stopped;
							StopMoving();
						}
					break;
				case UnitState.stopped:
					BoardObject ob;
					if (FindNearestTargetOnSandInViewRange(out ob)) {
						InfoLog.WriteInfo("Unit:AI: stop -> chace ", EPrefix.SimulationInfo);
						if (BelowTarged(ob)) {
							state = UnitState.attacking;
							InfoLog.WriteInfo("Unit:AI: stop -> attack ", EPrefix.SimulationInfo);
							attackedObject = ob;
							break;
						}
						state = UnitState.chasing;
						nearest = ob.Position;
						MoveTo(ob.Position);
						state = UnitState.chasing;
					}
					break;
				case UnitState.attacking:
					if (CheckIfStillExistTarget(attackedObject) == false) {
						// unit destroyed, find another one.
						FindNearestTargetOnSandInViewRange(out attackedObject);
					}
					if (attackedObject == null) {
						//unit/ building destroyed - stop
						InfoLog.WriteInfo("Unit:AI: attack -> stop ", EPrefix.SimulationInfo);
						state = UnitState.stopped;
						StopMoving();
						break;
					}
					if (BelowTarged(attackedObject)) {
						InfoLog.WriteInfo("Unit:AI: attack -> attack ", EPrefix.SimulationInfo);
						if (_remainingTurnsToReload == 0) {
							_simulation.handleAttackUnit((Unit)attackedObject, this, this.SandwormData.__FirePower);
						}
						//attack, reload etc
					} else {
						// out of range - chase
						InfoLog.WriteInfo("Unit:AI: attack -> chase ", EPrefix.SimulationInfo);
						state = UnitState.chasing;
						MoveTo(attackedObject.Position);
						//override state
						state = UnitState.chasing;
					}
					break;
			}
			//base.DoAI();
		}

		private bool BelowTarged(BoardObject attackedObject) {
			if (this.Position.X == attackedObject.Position.X && this.Position.Y == attackedObject.Position.Y)
				return true;
			else
				return false;
		}
		protected bool FindNearestTargetOnSandInViewRange(out BoardObject ob) {

			int count;
			Position[] viewSpiral = RangeSpiral(this.SandwormData.__ViewRange, out count);
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
					&& p.Y + spiralPos.Y < m.Height && _map.Tiles[p.X + spiralPos.X, p.Y + spiralPos.Y] == TileType.Sand) {

					units = m.Units[p.X + spiralPos.X, p.Y + spiralPos.Y];
					foreach (Unit unit in units) {
						//TODO erase true;)
						if (unit.Equals(this)) continue;
							ob = unit;
							InfoLog.WriteInfo("Unit:AI: found unit in view in range < " + this.ViewRange, EPrefix.SimulationInfo);
							return true;
					}
				}
			}
			ob = null;
			return false;
		}


	}
}
