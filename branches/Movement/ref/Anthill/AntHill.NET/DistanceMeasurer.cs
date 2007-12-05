using System;
using System.Collections.Generic;
using System.Text;

namespace AntHill.NET
{
    class DistanceMeasurer
    {
        public static int Taxi(Position p1, Position p2)
        {
            return (Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y));
        }
    }
}
