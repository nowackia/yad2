using System;
using System.Collections.Generic;
using astar;
using AntHill.NET.Utilities;

namespace AntHill.NET
{
    public class Spider : Creature
    {        
        public Spider(Position pos) : base(pos)
        {
            this.health = AntHillConfig.spiderMaxHealth;
        }
        
        private Ant FindNearestAnt()
        {
            if (Simulation.simulation.queen == null)
                return null;
            if (AntHillConfig.antSightRadius >= DistanceMeasurer.Taxi(Simulation.simulation.queen.Position, this.Position))                                          
                return Simulation.simulation.queen;

            LIList<Ant> ants = Simulation.simulation.GetVisibleAnts(this);
            if (ants == null)
                return null;
            if (ants.Count == 0)
                return null;
            int minDistance = DistanceMeasurer.Taxi(ants.First.Value.Position, Position);

            int distance;
            Ant bestAnt = null;
            LIList<Ant>.Enumerator ant = ants.GetEnumerator();
            while(ant.MoveNext())
            {
                if ((distance = DistanceMeasurer.Taxi(this.Position, ant.Current.Position)) < minDistance)
                {
                    bestAnt = ant.Current;                    
                    minDistance = distance;
                }                
            }           
            return bestAnt;           
        }
      
        public override bool Maintain(ISimulationWorld isw)
        {//TODO czy na pewno dobrze znajduje droge? (pobieranie astar) czy spider nie szuka krolowej?
            Ant ant = FindNearestAnt();
            if (ant == null)
            {
                MoveRandomly(isw);
                return true;
            }
            randomDestination.X = -1;
            MoveRotateOrAttack(this, ant, isw);
            return true;
        }
    }
}
