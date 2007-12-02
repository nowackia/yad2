using System;
using System.Collections.Generic;
using astar;
using AntHill.NET.Utilities;

namespace AntHill.NET
{
    class Warrior: Citizen
    {
        private Spider lastSpider = null;
        private Food lastFood = null;

        public Warrior(Position pos) : base(pos) { }

        private bool MaintainSignals(MessageType mT)
        {
            Message m = _messages[(int)mT];
            if (m != null)
            {
                if (DistanceMeasurer.Taxi(this.Position, m.TargetPosition) >= 0)
                {
                    List<KeyValuePair<int, int>> trail = Astar.Search(new KeyValuePair<int, int>(this.Position.X, this.Position.Y), new KeyValuePair<int, int>(m.TargetPosition.X, m.TargetPosition.Y), new AstarOtherObject());
                    if (trail == null)
                        return true;
                    if (trail.Count <= 1)
                        return true;
                    MoveOrRotate(trail[1]);
                    return true;
                }
            }
            return false;
        }

        
        public override bool Maintain(ISimulationWorld isw)
        {//TODO malo:)
            if (!base.IsAlive())
                return false;

            SpreadSignal(isw);
            LIList<Message>.Enumerator msg = isw.GetVisibleMessages(this).GetEnumerator();
            while(msg.MoveNext())
                this.AddToSet(msg.Current, msg.Current.GetPointWithIntensity(this.Position).Intensity);

            LIList<Spider> spiders;
            if ((spiders = isw.GetVisibleSpiders(this)).Count != 0)
            {
                Spider spider = GetNearestSpider(spiders);
                if (spider != lastSpider)
                {
                    if (!FindEqualSignal(MessageType.SpiderLocalization, spider.Position))
                    {
                        isw.CreateMessage(this.Position, MessageType.SpiderLocalization, spider.Position);
                        lastSpider = spider;
                    }
                }
                MoveRotateOrAttack(this, spider, isw);
                randomDestination.X = -1;
                return true;
            }
            if (MaintainSignals(MessageType.QueenInDanger))
            {
                randomDestination.X = -1;
                return true;
            }
            if (MaintainSignals(MessageType.SpiderLocalization))
            {
                randomDestination.X = -1;
                return true;
            }

            // teraz wcinamy

            LIList<Food> foods = isw.GetVisibleFood(this);
            if (foods.Count != 0)
            {
                Food food = GetNearestFood(foods);
                int distance = DistanceMeasurer.Taxi(this.Position, food.Position);
                if (food != lastFood)
                {
                    if (!FindEqualSignal(MessageType.FoodLocalization, food.Position))
                    {
                        isw.CreateMessage(this.Position, MessageType.FoodLocalization, food.Position);
                        lastFood = food;
                    }
                }
                if (this.TurnsToBecomeHungry <= 0)
                {
                    if (distance == 0)
                    {
                        food.Maintain(isw);
                        this.Eat();

                        randomDestination.X = -1;
                        return true;
                    }
                    List<KeyValuePair<int, int>> trail = Astar.Search(new KeyValuePair<int, int>(this.Position.X, this.Position.Y), new KeyValuePair<int, int>(food.Position.X, food.Position.Y), new AstarOtherObject());
                    if (trail == null)
                    {
                        randomDestination.X = -1;
                        return true;
                    }
                    if (trail.Count <= 1)
                    {
                        randomDestination.X = -1;
                        return true;
                    }
                    MoveOrRotate(trail[1]);
                    randomDestination.X = -1;
                    return true;
                }
            }
            else
            {
                if (MaintainSignals(MessageType.FoodLocalization))
                {
                    randomDestination.X = -1;
                    return true;
                }
            }

            MoveRandomly(isw);
            return true;
        }
    }
}
