using System;
using System.Collections.Generic;
using System.Text;

namespace AntHill.NET
{
    class AstarOtherObject : IAstar
    {
        public AstarOtherObject() { }

        int IAstar.GetWeight(int x, int y)
        {
            bool rain = false;
            if (Simulation.simulation.rain != null)
            {
                rain = Simulation.simulation.rain.IsRainOver(x, y);
            }
            switch (Simulation.simulation.GetMap().GetTile(x, y).TileType)
            {
                case TileType.Wall:
                    return int.MaxValue;
                case TileType.Outdoor:
                    return rain ? int.MaxValue : 1;//int.MaxValue;
                case TileType.Indoor:
                    return 1;
                default:
                    break;
            }
            return 0;
        }
    }
}
