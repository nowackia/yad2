using System;

namespace AntHill.NET
{
    public abstract class Ant : Creature
    {
        private int _turnsWithoutFood;
        private int _turnsToBecomeHungry;

        public Ant(Position pos) : base(pos)
        {
            health = AntHillConfig.antMaxLife;
            _turnsToBecomeHungry = AntHillConfig.antTurnNumberToBecomeHungry;
            _turnsWithoutFood = AntHillConfig.antMaxLifeWithoutFood;
        }

        public int TurnsWithoutFood
        {
            get { return _turnsWithoutFood; }
            set { _turnsWithoutFood = value; }
        }
        
        public int TurnsToBecomeHungry
        {
            get { return _turnsToBecomeHungry; }
            set { _turnsToBecomeHungry = value; }
        }

        public virtual bool IsAlive()
        {
            if (_turnsToBecomeHungry >= 0)
            {
                _turnsToBecomeHungry--;
                return true;
            }

            if (_turnsWithoutFood >= 0)
            {
                _turnsWithoutFood--;
                return true;
            }
            
            return false;
        }

        public virtual void Eat()
        {
            TurnsToBecomeHungry = AntHillConfig.antTurnNumberToBecomeHungry;
            _turnsWithoutFood = AntHillConfig.antMaxLifeWithoutFood;
        }
    }
}
