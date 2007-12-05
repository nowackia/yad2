using System;

namespace AntHill.NET.Utilities
{
    static class Randomizer
    {
        static Random rnd = new Random();

        static public int Next(int max)
        {
            return rnd.Next(max);
        }

        static public double NextDouble()
        {
            return rnd.NextDouble();
        }
    }
}
