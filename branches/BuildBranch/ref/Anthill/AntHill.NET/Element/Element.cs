using System;
using System.Collections.Generic;
using AntHill.NET;

namespace AntHill.NET
{
    public abstract class Element
    {
        protected Position position;

        public Element(Position pos)
        {            
            position = new Position(pos);
        }

        public Position Position
        {
            get { return position; }
            set { position = new Position(value); }
        }

        public void Move(KeyValuePair<int, int> pos)
        {//TODO zle ;)
            this.Position = new Position(pos.Key, pos.Value);
        }
        public virtual int GetTexture()
        {
            return AHGraphics.GetElementTexture(this);
        }

        public abstract bool Maintain(ISimulationWorld isw);
    }
}
