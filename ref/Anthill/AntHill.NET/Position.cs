using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AntHill.NET
{
    public struct Position
    {
        private int x;
        private int y;
        /*public Position()
        {
            x = 0;
            y = 0;
        }*/
        public Position(Position pos)
        {
            x = pos.x;
            y = pos.y;
        }
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public override string ToString()
        {
            return ""+x+","+y;
        }
        public override bool Equals(object obj)
        {
            return (obj is Position)?(this == (Position)obj) :false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        static public bool operator == (Position pos1, Position pos2)
        {
            return ((pos1.x == pos2.x) && (pos1.y == pos2.y));
             
        }
        static public bool operator !=(Position pos1, Position pos2)
        {
            return ((pos1.x != pos2.x) || (pos1.y != pos2.y));
        }
	
    }
}
