using System;
using System.Collections.Generic;
using astar;
using AntHill.NET.Utilities;

namespace AntHill.NET
{
    public class Worker : Citizen
    {
        private int foodQuantity;
        private Spider lastSpider = null;
        private Food lastFood = null;
        
        public int FoodQuantity
        {
            get { return foodQuantity; }
            set { foodQuantity = value; }
        }

        public Worker(Position pos) : base(pos) { }

        public void Dig(ISimulationWorld isw, Position pos)
        {
            isw.GetMap().DestroyWall(isw.GetMap().GetTile(pos));
        }

        public void LoadFood(ISimulationWorld isw, Food f)
        {
            isw.DeleteFood(f);
            foodQuantity += f.GetQuantity;
        }        
        
        public override bool Maintain(ISimulationWorld isw)
        {
            if (!base.IsAlive())
                return false;
            
            SpreadSignal(isw);
            LIList<Food> food;
            LIList<Spider> spiders;
            spiders = isw.GetVisibleSpiders(this);
            if (spiders.Count != 0)
            {
                Spider s = this.GetNearestSpider(spiders);
                if (s != lastSpider)
                {
                    if (!FindEqualSignal(MessageType.SpiderLocalization, s.Position))
                    {
                        isw.CreateMessage(this.Position, MessageType.SpiderLocalization, s.Position);
                        lastSpider = s;
                    }
                }
            }
            LIList<Message>.Enumerator msg = isw.GetVisibleMessages(this).GetEnumerator();
            while(msg.MoveNext())
                this.AddToSet(msg.Current, msg.Current.GetPointWithIntensity(this.Position).Intensity);

            if (this.TurnsToBecomeHungry <= 0)
                if (this.foodQuantity > 0)
                {
                    foodQuantity--;
                    Eat();
                }

            if (this.foodQuantity == 0) //search for food
            {
                path = null;
                food = isw.GetVisibleFood(this);

                if (food.Count != 0)
                {// idzie do jedzenia
                    Food nearestFood = this.GetNearestFood(food);
                    int dist = DistanceMeasurer.Taxi(this.Position, nearestFood.Position);
                    if (dist == 0)
                    {
                        this.FoodQuantity = nearestFood.GetQuantity;
                        isw.DeleteFood(nearestFood);
                    }
                    else
                    {
                        if (nearestFood != lastFood)
                        {
                            if (!FindEqualSignal(MessageType.FoodLocalization, nearestFood.Position))
                            {
                                isw.CreateMessage(this.Position, MessageType.FoodLocalization, nearestFood.Position);
                                lastFood = nearestFood;
                            }
                        }
                        // znajdujemy t¹ krótk¹ œcie¿kê - wyliczane co 'maintain'
                        List<KeyValuePair<int, int>> trail = Astar.Search(new KeyValuePair<int, int>(this.Position.X, this.Position.Y), new KeyValuePair<int, int>(nearestFood.Position.X, nearestFood.Position.Y), new AstarOtherObject());
                        if (trail.Count >= 2)
                        {
                            MoveOrRotateOrDig(isw,trail[1]);
                            randomDestination.X = -1;
                            return true;
                        }
                    }
                }
                else
                {// nie widzi
                    Message m = _messages[(int)MessageType.FoodLocalization];
                    if (m != null)
                    {
                        // ma sygnal o najwiekszej intensywnosci
                        List<KeyValuePair<int, int>> trail = Astar.Search(new KeyValuePair<int, int>(this.Position.X, this.Position.Y), new KeyValuePair<int, int>(m.Position.X, m.Position.Y), new AstarWorkerObject());
                        if (trail.Count >= 2)
                        {
                            MoveOrRotateOrDig(isw, trail[1]);
                            randomDestination.X = -1;
                            return true;
                        }
                    }
                }
            }
            else
            {
                int dist = DistanceMeasurer.Taxi(this.Position, Simulation.simulation.queen.Position);
                if (dist == 0)
                {
                    isw.FeedQueen(this);
                    path = null;
                }
                else
                {
                    if (path == null || path.Count < 2)
                    {
                        path = Astar.Search(new KeyValuePair<int, int>(this.Position.X, this.Position.Y), new KeyValuePair<int, int>(Simulation.simulation.queen.Position.X, Simulation.simulation.queen.Position.Y), new AstarWorkerObject());
                    }
                    if (path.Count >= 2)
                    {
                        if (MoveOrRotateOrDig(isw,path[1]))
                            path.RemoveAt(0);
                        randomDestination.X = -1;
                        return true;
                    }
                }
            }

            MoveRandomly(isw);

            return true;
        }
        
        protected new  void MoveRandomly(ISimulationWorld isw)
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
                                            new AstarWorkerObject());
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
            if (!MoveOrRotateOrDig(isw,currentTrail[randomMovementCount]))
                randomMovementCount--;
            if (randomMovementCount >= 10)
                randomDestination.X = -1;
        }
        protected bool MoveOrRotateOrDig(ISimulationWorld isw, KeyValuePair<int, int> where)
        {// nie chce mi sie obrotu zrobic do kopania.. 
            Tile t = isw.GetMap().GetTile(where.Key, where.Value);
            if (t.TileType == TileType.Wall)
            {// destroy wall nie ma zabawy z regionami
                if(this.IsMoveOrRotate(where))
                    isw.GetMap().DestroyWall(t);
                return false;
            }
            return MoveOrRotate(where);
        }

        #region TEST
        public Food testGetNearestFood(LIList<Food> food)
        {
             return this.GetNearestFood(food);
        }
        /*return the strongest message of given type from particular ant's visiable messages*/
        public Message testReadMessage(MessageType mt)
        {
            return _messages[(int)mt];
        }
        #endregion
    }
}
