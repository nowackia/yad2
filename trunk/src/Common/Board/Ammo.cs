using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board.Common;
using Yad.Engine.Common;

namespace Yad.Board {
    public class Ammo : BoardObject {

        private AmmoType ammoType;
        private short damage;
        private short damageRange;
        private Simulation sim;
        private Direction direction;
        // how many moves in one turn
        private short speed;
        Queue<Position> path;
        private Position from;
        private Position to;
        private Position lastPosition;
        public Position LastPosition {
            get { return lastPosition; }
        }

        public Position To {
            get { return to; }
        }

        public Position From {
            get { return from; }
        }





        public Ammo(ObjectID obId,  Position from, Position to,AmmoType ammoType, short speed, short damage, short damageRange, Simulation sim) : base(obId, Yad.Config.BoardObjectClass.Ammo,from) {
            this.ammoType = ammoType;
            this.damage = damage;
            this.damageRange = damageRange;
            this.sim = sim;
            this.speed = speed;
            this.Position = this.lastPosition = from;
            path = BoardObject.Bresenham(ref from, ref to);
            // jest na odwrot
            int dir = GetAlfa(to.X - from.X, to.Y - from.Y);
            direction = ConvertToDirection(dir);
            this.from = from;
            this.to = to;
            
        }

        /// <summary>
        /// ai for ammo - auto aiming bullet?:P
        /// </summary>
        public void DoAI() {
            if (this.path.Count == 0) {
                DestinationReached();
                this.sim.RemoveAmmo(this);
                return;
            }
            this.lastPosition = Position;
            for (int i = 0; i < speed; ++i) {
                Position next = this.path.Dequeue();
                this.Position = next;
                if (this.path.Count == 0) {
                    DestinationReached();
                    this.sim.RemoveAmmo(this);
                    return;
                }

                LocationMove();
            }
        }

        /// <summary>
        /// handles move to location - give damage on each location
        /// </summary>
        private void LocationMove() {
            switch (ammoType) {
                case AmmoType.None:
                    break;
                case AmmoType.Bullet:
                    break;
                case AmmoType.Rocket:
                    break;
                case AmmoType.Sonic:
                    AttackRegion(this.Position);
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// for racket - blow
        /// </summary>
        private void DestinationReached() {
            this.sim.OnAmmoBlow(this);
            switch (ammoType) {
                case AmmoType.None:
                    break;
                case AmmoType.Bullet:

                    AttackRegion(this.Position);
                    break;
                case AmmoType.Rocket:
                    int max;
                    Position[] tab = Unit.RangeSpiral(this.damageRange, out max);
                    for (int i = 0; i < max; ++i) {
                        Position p = new Position(tab[i].X + Position.X,tab[i].Y + Position.Y);
                        if(p.X >= 0 && p.X < sim.Map.Width && p.Y >=0 && p.Y < sim.Map.Height)
                            AttackRegion(p);
                    }
                    break;
                case AmmoType.Sonic:
                    AttackRegion(this.Position);
                    break;
                default:
                    break;
            }
        }

        private void AttackRegion(Position reg) {
            LinkedList<Building> buildings = new LinkedList<Building>(sim.Map.Buildings[reg.X, reg.Y]);
            foreach (Building b in buildings) {
                this.sim.handleAttackBuilding(b, this);
            }
            LinkedList<Unit> units = new LinkedList<Unit>(sim.Map.Units[reg.X, reg.Y]);
            foreach (Unit u in units) {
                this.sim.handleAttackUnit(u, this);
            }
        }



        public short Damage {
            get { return damage; }
        }

        public short DamageRange {
            get { return damageRange; }
        }

        public Direction Direction {
            get { return direction; }
        }

        public AmmoType Type {
            get { return ammoType; }
            set { ammoType = value; }
        }

        
       



    }
}
