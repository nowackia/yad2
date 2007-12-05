using System;
using System.Collections.Generic;
using System.Text;

namespace AntHill.NET
{
    class AstarWorkerObject:IAstar
    {
        int IAstar.GetWeight(int x, int y)
        {
            bool rain=false;
            if (Simulation.simulation.rain != null)
            {
                rain = Simulation.simulation.rain.IsRainOver(x,y);
            }
            switch (Simulation.simulation.GetMap().GetTile(x, y).TileType)
            {
                case TileType.Wall:
                    return 2;
                case TileType.Outdoor:
                    return rain?int.MaxValue:1;
                case TileType.Indoor:
                    return 1;
                default:
                    break;
            }
            return 0;
        }
    }
}
