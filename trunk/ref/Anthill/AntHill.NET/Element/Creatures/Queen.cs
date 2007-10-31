using System;
using AntHill.NET.Utilities;

namespace AntHill.NET
{
    public class Queen : Ant
    {
        private int foodQuantity = 0;

        public Queen(Position pos) : base(pos) { }       

        public int FoodQuantity
        {
            get { return foodQuantity; }
            set { foodQuantity = value; }
        }

        public override bool Maintain(ISimulationWorld isw)
        {
            if (!IsAlive())
                return false;

            if (Randomizer.NextDouble() < AntHillConfig.queenLayEggProbability)
                isw.CreateEgg(this.Position);

            if (this.TurnsToBecomeHungry <= 0)
            {
                if (foodQuantity > 0)
                {
                    this.Eat();
                    foodQuantity--;
                    this.TurnsToBecomeHungry = AntHillConfig.antTurnNumberToBecomeHungry;
                }
                else
                {
                    isw.CreateMessage(this.Position, MessageType.QueenIsHungry, this.Position);
                }
            }
            if (isw.GetVisibleSpiders(this).Count != 0)
            {
                isw.CreateMessage(this.Position, MessageType.QueenInDanger,this.Position);
            }
            return true;
        }
    }
}
