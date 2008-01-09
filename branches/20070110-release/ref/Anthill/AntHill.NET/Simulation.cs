using System;
using System.Collections.Generic;
using astar;
using AntHill.NET.Utilities;

namespace AntHill.NET
{
    public class Simulation : ISimulationUser, ISimulationWorld
    {
        #region Static Members
        public static Simulation singletonInstance = null;

        public static bool Init(Map map)
        {
            try
            {
                if (singletonInstance == null)
                    singletonInstance = new Simulation(map);
            }
            catch (Exception exc) //Simulation(map) can throw an exception
            {
                throw exc;
            }

            Astar.Init(AntHillConfig.mapColCount, AntHillConfig.mapRowCount);

            return true;
        }

        public static void DeInit()
        {
            singletonInstance = null;
        }

        public static Simulation simulation
        {
            get { return singletonInstance; }
        }
        #endregion

        private Map _map;
        public LIList<Egg> eggs;
        public LIList<Message> messages;
        public LIList<Food> food;
        public LIList<Spider> spiders;
        public LIList<Ant> ants;
        public Rain rain;
        public Queen queen;

        private int _turnCounter = 0;

        private void Initialize()
        {
            _map = null;
            eggs = new LIList<Egg>();
            eggs.Clear();
            messages = new LIList<Message>();
            messages.Clear();
            food = new LIList<Food>();
            food.Clear();
            spiders = new LIList<Spider>();
            spiders.Clear();
            ants = new LIList<Ant>();
            ants.Clear();
            rain = null;
            queen = new Queen(new Position(AntHillConfig.queenXPosition, AntHillConfig.queenYPosition));
        }

	    public Simulation(Map map)
        {
            Initialize();

            _map = map;

            for (int i = AntHillConfig.workerStartCount; i > 0; i--)
                this.CreateWorker(Map.GetRandomTile(TileType.Indoor).Position);

            for (int i = AntHillConfig.warriorStartCount; i > 0; i--)
                this.CreateWarrior(Map.GetRandomTile(TileType.Indoor).Position);
        }

        public Map Map
        {
            get { return _map; }
        }

        Position GetRandomPosition()
        {
            return new Position(Randomizer.Next(Map.Width), Randomizer.Next(Map.Height));
        }

        Position GetRandomPositionForRain()
        {
            return new Position(Randomizer.Next(Map.Width - AntHillConfig.rainWidth),
                                Randomizer.Next(Map.Height - AntHillConfig.rainWidth));
        }

	    #region ISimulation Members

        /// <summary>
        /// This is the most important function - activity diagram
        /// </summary>
        bool ISimulationUser.DoTurn()
        {
            if (queen == null) return false;
            _turnCounter++;
            if (Randomizer.NextDouble() <= AntHillConfig.spiderProbability)
                    this.CreateSpider(Map.GetRandomTile(TileType.Outdoor).Position);

            if (Randomizer.NextDouble() <= AntHillConfig.foodProbability)
                this.CreateFood(Map.GetRandomTile(TileType.Outdoor).Position, GetRandomFoodQuantity());

            if ((rain == null) && (Randomizer.NextDouble() <= AntHillConfig.rainProbability))
                this.CreateRain(GetRandomPositionForRain());

            if (rain != null)
                rain.Maintain(this);

            LinkedListNode<Message> msg = messages.First;
            LinkedListNode<Message> msgT;
            while (msg != null)
            {
                msg.Value.Maintain(this);
                if (msg.Value.Empty)
                {
                    msgT = msg;
                    msg = msg.Next;
                    messages.Remove(msgT);
                }
                else
                    msg = msg.Next;
            }

            LinkedListNode<Ant> ant = ants.First;
            LinkedListNode<Ant> antTemp;            
            while(ant != null)
            {
                if (!ant.Value.Maintain(this))
                {
                    antTemp = ant;
                    ant = ant.Next;
                    ants.Remove(antTemp);
                }
                else
                    ant = ant.Next;
            }

            LinkedListNode<Spider> spider = spiders.First;
            LinkedListNode<Spider> spiderTemp;
            while (spider != null)
            {
                if (!spider.Value.Maintain(this))
                {
                    spiderTemp = spider;
                    spider = spider.Next;
                    spiders.Remove(spiderTemp);
                }
                else
                    spider = spider.Next;
            }

            LinkedListNode<Egg> egg = eggs.First;
            LinkedListNode<Egg> eggTemp;
            while (egg != null)
            {
                if (!egg.Value.Maintain(this))
                {
                    eggTemp = egg;
                    egg = egg.Next;
                    eggs.Remove(eggTemp);
                }
                else
                    egg = egg.Next;
            }
            
            if (queen != null && !queen.Maintain(this))
            {
                queen = null;
                return false;
            }
            return true;
        }

        void ISimulationUser.Reset()
        {
            Initialize();
            _turnCounter = 0;
        }

        void ISimulationUser.Start() { }
        void ISimulationUser.Stop() { }

        #endregion

        #region ISimulationWorld Members

        private bool CreateRain(Position point)
        {
            rain = new Rain(point);
            return true;
        }

        public bool CreateFood(Position point, int quantity)
        {
            food.AddLast(new Food(point, quantity));
            return true;
        }

        public bool CreateSpider(Position point)
        {
            spiders.AddLast(new Spider(point));
            return true;
        }


        public bool CreateWarrior(Position pos)
        {
            ants.AddLast(new Warrior(pos));
            return true;
        }

        public bool CreateWorker(Position pos)
        {
            ants.AddLast(new Worker(pos));
            return true;
        }

        public bool CreateSpider()
        {
            spiders.AddLast(new Spider(Map.GetRandomTile(TileType.Outdoor).Position));
            return true;
        }

        //returns true if cD is killed
        public bool Attack(Creature cAttacking, Creature cDefending)
        {
            if (cAttacking is Spider)
            {
                if (cDefending is Queen) queen = null;
                else if (cDefending is Ant)
                    this.DeleteAnt((Ant)cDefending);

                return true;
            }
            else if (cAttacking is Warrior && cDefending is Spider)
            {
                Spider s = (Spider)cDefending;
                s.Health -= AntHillConfig.antStrength;
                if (s.Health <= 0)
                    this.DeleteSpider(s);
                return true;
            }
            return false;
        }

        public LIList<Ant> GetVisibleAnts(Element c)
        {
            LIList<Ant> res_ants = new LIList<Ant>();
            LinkedListNode<Ant> antNode = ants.First;
            
            if (c is Spider || c is Ant) //same radius
            {
                int radius2 = AntHillConfig.antSightRadius * AntHillConfig.antSightRadius;
                while (antNode != null)
                {
                    int dx = antNode.Value.Position.X - c.Position.X;
                    int dy = antNode.Value.Position.Y - c.Position.Y;
                    if (dx * dx + dy * dy <= radius2)
                        if (_map.CheckVisibility(c.Position, antNode.Value.Position))
                            res_ants.AddLast(antNode.Value);
                    antNode = antNode.Next;
                }
            }
            else if (c is Rain)
            {
                while (antNode != null)
                {
                    if (_map.GetTile(antNode.Value.Position).TileType == TileType.Outdoor &&
                        ((Rain)c).IsRainOver(antNode.Value.Position))
                        res_ants.AddLast(antNode.Value);
                    antNode = antNode.Next;
                }
            }                
            return res_ants;
        }

        public LIList<Food> GetVisibleFood(Element c)
        {
            LIList<Food> res_food = new LIList<Food>();
            LinkedListNode<Food> foodNode = food.First;

            if (c is Spider || c is Ant) //same radius
            {
                int radius2 = AntHillConfig.antSightRadius * AntHillConfig.antSightRadius;
                while (foodNode != null)
                {
                    int dx = foodNode.Value.Position.X - c.Position.X;
                    int dy = foodNode.Value.Position.Y - c.Position.Y;
                    if (dx * dx + dy * dy <= radius2)
                        if (_map.CheckVisibility(c.Position, foodNode.Value.Position))
                            res_food.AddLast(foodNode.Value);
                    foodNode = foodNode.Next;
                }
            }
            else if (c is Rain)
            {
                while (foodNode != null)
                {
                    if (_map.GetTile(foodNode.Value.Position).TileType == TileType.Outdoor &&
                        ((Rain)c).IsRainOver(foodNode.Value.Position))
                        res_food.AddLast(foodNode.Value);
                    foodNode = foodNode.Next;
                }
            }
            return res_food;
        }

        public LIList<Spider> GetVisibleSpiders(Element c)
        {
            LIList<Spider> res_spiders = new LIList<Spider>();
            LinkedListNode<Spider> spiderNode = spiders.First;

            if (c is Spider || c is Ant) //same radius
            {
                int radius2 = AntHillConfig.antSightRadius * AntHillConfig.antSightRadius;
                while (spiderNode != null)
                {
                    int dx = spiderNode.Value.Position.X - c.Position.X;
                    int dy = spiderNode.Value.Position.Y - c.Position.Y;
                    if (dx * dx + dy * dy <= radius2)
                        if (_map.CheckVisibility(c.Position, spiderNode.Value.Position))
                            res_spiders.AddLast(spiderNode.Value);
                    spiderNode = spiderNode.Next;
                }
            }
            else if (c is Rain)
            {
                while (spiderNode != null)
                {
                    if (_map.GetTile(spiderNode.Value.Position).TileType == TileType.Outdoor &&
                        ((Rain)c).IsRainOver(spiderNode.Value.Position))
                        res_spiders.AddLast(spiderNode.Value);
                    spiderNode = spiderNode.Next;
                }
            }
            return res_spiders;
        }

        public LIList<Message> GetVisibleMessages(Element c)
        {
            LIList<Message> res_messages = new LIList<Message>();
            LinkedListNode<Message> messageNode;
            if (c is Ant)
            {
                messageNode = Map.GetTile(c.Position).messages.First;
                while (messageNode != null)
                {
                    res_messages.AddLast(messageNode.Value);
                    messageNode = messageNode.Next;
                }
            }
            else if (c is Rain)
            {
                for (int i = 0; i < AntHillConfig.rainWidth; i++)
                {
                    for (int j = 0; j < AntHillConfig.rainWidth; j++)
                    {
                        messageNode = Map.GetTile(c.Position.X + i, c.Position.Y + j).messages.First;
                        while (messageNode != null)
                        {
                            if (!res_messages.Contains(messageNode.Value))
                                res_messages.AddLast(messageNode.Value);
                            messageNode = messageNode.Next;
                        }
                    }
                }
            }
            return res_messages;
        }

        public bool FeedQueen(Worker w)
        {
            if (w.Position != queen.Position)
                return false;
            queen.FoodQuantity += w.FoodQuantity;
            w.FoodQuantity = 0;
            return true;
        }

        public bool CreateAnt(Position position)
        {
            if (Randomizer.NextDouble() < AntHillConfig.eggHatchWarriorProbability)
                ants.AddLast(new Warrior(position));
            else
                ants.AddLast(new Worker(position));
            return true;
        }

        public bool DeleteEgg(Egg egg)
        {
            eggs.Remove(egg);
            return true;
        }

        public bool DeleteRain()
        {
            rain = null;
            return true;
        }

        public bool CreateEgg(Position pos)
        {
            eggs.AddLast(new Egg(pos));
            return true;
        }

        public bool DeleteFood(Food food)
        {
            this.food.Remove(food);
            return true;
        }

        public bool DeleteAnt(Ant ant)
        {
            this.food.AddLast(new Food(ant.Position, AntHillConfig.antFoodQuantityAfterDeath));
            ants.Remove(ant);
            return true;
        }

        public bool DeleteSpider(Spider spider)
        {
            this.food.AddLast(new Food(spider.Position, AntHillConfig.spiderFoodQuantityAfterDeath));
            spiders.Remove(spider);
            return true;
        }

        public Map GetMap()
        {
            return _map;
        }

        public bool CreateMessage(Position position, MessageType mt, Position tagetPosition)
        {
            Message ms = new Message(position, mt,tagetPosition);
            this.messages.AddLast(ms);
            ms.Spread(this, position, AntHillConfig.messageLifeTime);

            return true;
        }

        private int GetRandomFoodQuantity()
        {
            return Randomizer.Next(AntHillConfig.foodRandomMaxQuantity) + 1;
        }
        #endregion

        #region ISimulationUser Members
        public int GetAntsCount()
        {
            return (ants!=null)?ants.Count:0;
        }

        public int GetSignalsCount()
        {
            return (messages!=null)?messages.Count:0;
        }

        public int GetTurnsCount()
        {
            return _turnCounter;
        }

        public int GetSpidersCount()
        {
            return (spiders!=null)?spiders.Count:0;
        }
        #endregion
    }
}
