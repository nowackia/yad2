using System;
using System.Collections.Generic;
using astar;
using AntHill.NET.Utilities;

namespace AntHill.NET
{
    public enum Dir { S = 2, W = 3, E = 1, N = 0 }
    public enum CreatureType { queen, warrior, spider, worker }
    public enum AddsType { rain=0, food=1 }

    public abstract class Creature : Element
    {
        public List<KeyValuePair<int, int>> path = new List<KeyValuePair<int,int>>();
        protected List<KeyValuePair<int, int>> currentTrail = new List<KeyValuePair<int, int>>();
        protected int health;
        protected int randomMovementCount = 0;
        protected Dir direction;
        protected Position randomDestination = new Position(-1, 0);

        public Creature(Position pos)
            : base(pos)
        {
            int i = Randomizer.Next(Enum.GetValues(typeof(Dir)).Length);
            direction = (Dir)i;
            path = new List<KeyValuePair<int, int>>();
        }
        
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        
        protected void MoveRandomly(ISimulationWorld isw)
        {            
            randomMovementCount++;

            if (randomMovementCount < currentTrail.Count)
            {   
                if ((this.Position.X == currentTrail[randomMovementCount].Key) && (this.Position.Y == currentTrail[randomMovementCount].Value))
                    randomMovementCount++;
            }

            if ((randomMovementCount >= currentTrail.Count) || (randomDestination.X < 0))
            {
                randomMovementCount = 0;
                randomDestination = new Position(isw.GetMap().GetRandomIndoorOrOutdoorTile().Position);
                currentTrail = Astar.Search(new KeyValuePair<int, int>(this.Position.X, this.Position.Y),
                                            new KeyValuePair<int, int>(randomDestination.X, randomDestination.Y),
                                            new AstarOtherObject());
            }            
            if (currentTrail == null)
            {
                randomDestination.X = -1;
                return;
            }
            if (currentTrail.Count <= 1)
            {
                randomDestination.X = -1;
                return;
            }
            if (!MoveOrRotate(currentTrail[randomMovementCount]))
                randomMovementCount--;
            if (randomMovementCount >= 10)
                randomDestination.X = -1;
        }

        public Spider GetNearestSpider(LIList<Spider> spiders)
        {            
            int min = Int32.MaxValue;
            int tmp;
            LIList<Spider>.Enumerator e = spiders.GetEnumerator();
            Spider bestSpider = null;
            while(e.MoveNext())
            {
                if ((tmp = DistanceMeasurer.Taxi(this.Position, e.Current.Position)) < min)
                {
                    bestSpider = e.Current;
                    min = tmp;
                }
            }
            return bestSpider;
        }

        public void MoveRotateOrAttack(Creature aggressor, Creature prey, ISimulationWorld isw)
        {
            int distance = DistanceMeasurer.Taxi(aggressor.Position, prey.Position);
            if (distance == 0)
            {
                isw.Attack(aggressor, prey);
            }
            if (distance == 1)
            {
                if (aggressor.Position.X == prey.Position.X)
                {
                    if (aggressor.Position.Y == prey.Position.Y + 1) //ant 1 tile above
                    {
                        if (aggressor.Direction == Dir.N)
                        {
                            isw.Attack(aggressor, prey);
                            return;
                        }
                        else
                        {
                            aggressor.Direction = Dir.N;
                        }
                    }
                    else
                    {
                        if (aggressor.Direction == Dir.S)
                        {
                            isw.Attack(aggressor, prey);
                            return;
                        }
                        else
                        {
                            aggressor.Direction = Dir.S;
                        }
                    }
                }
                else
                {
                    if (aggressor.Position.X == prey.Position.X + 1) //ant 1 tile left
                    {
                        if (aggressor.Direction == Dir.W)
                        {
                            isw.Attack(aggressor, prey);
                            return;
                        }
                        else
                        {
                            aggressor.Direction = Dir.W;
                        }
                    }
                    else
                    {
                        if (aggressor.Direction == Dir.E)
                        {
                            isw.Attack(aggressor, prey);
                            return;
                        }
                        else
                        {
                            aggressor.Direction = Dir.E;
                        }
                    }

                }
                return;
            }
            if (distance > 1)
            {
                List<KeyValuePair<int, int>> trail = Astar.Search(new KeyValuePair<int, int>(aggressor.Position.X, aggressor.Position.Y), new KeyValuePair<int, int>(prey.Position.X, prey.Position.Y), new AstarOtherObject());
                if (trail == null)
                    return;
                if (trail.Count <= 1)
                    return;
                MoveOrRotate(trail[1]);                
            }
        }

        public Dir Direction
        {
            get { return direction; }
            set { direction = value; }
        }        
        
        protected bool IsMoveOrRotate(KeyValuePair<int, int> pos)
        {
            if (Position.X == pos.Key)
            {
                if (Position.Y == pos.Value + 1) //ant 1 tile above
                {
                    if (Direction == Dir.N)
                    {
                        return true;
                    }
                    else
                    {
                        Direction = Dir.N;
                    }
                }
                else
                {
                    if (Direction == Dir.S)
                    {
                        return true;
                    }
                    else
                    {
                        Direction = Dir.S;
                    }
                }
            }
            else
            {
                if (Position.X == pos.Key + 1) //ant 1 tile left
                {
                    if (Direction == Dir.W)
                    {
                        return true;
                    }
                    else
                    {
                        Direction = Dir.W;
                    }
                }
                else
                {
                    if (Direction == Dir.E)
                    {
                        return true;
                    }
                    else
                    {
                        Direction = Dir.E;
                    }
                }
            }
            return false;
        }

        protected bool MoveOrRotate(KeyValuePair<int, int> pos)
        {
            if (Position.X == pos.Key)
            {
                if (Position.Y == pos.Value + 1) //ant 1 tile above
                {
                    if (Direction == Dir.N)
                    {
                        Move(pos);
                        return true;
                    }
                    else
                    {
                        Direction = Dir.N;
                    }
                }
                else
                {
                    if (Direction == Dir.S)
                    {
                        Move(pos);
                        return true;
                    }
                    else
                    {
                        Direction = Dir.S;
                    }
                }
            }
            else
            {
                if (Position.X == pos.Key + 1) //ant 1 tile left
                {
                    if (Direction == Dir.W)
                    {
                        Move(pos);
                        return true;
                    }
                    else
                    {
                        Direction = Dir.W;
                    }
                }
                else
                {
                    if (Direction == Dir.E)
                    {
                        Move(pos);
                        return true;
                    }
                    else
                    {
                        Direction = Dir.E;
                    }
                }
            }
            return false;
        }
    }
}
