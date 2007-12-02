using System;

namespace AntHill.NET
{
    public class Egg : Element
    {
        private int timeToHatch;

        public Egg(Position pos) : base(pos)
        {
            timeToHatch = AntHillConfig.eggHatchTime;
        }
        
        public override bool Maintain(ISimulationWorld isw)
        {
            if (--timeToHatch < 0)
            {
                isw.CreateAnt(this.Position);
                return false;
            }
            return true;
        }
    }
}
