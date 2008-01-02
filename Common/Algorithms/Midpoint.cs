using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;

namespace Yad.Algorithms
{
    public class Midpoint
    {
        public static List<Position> MidpointCircle(int R)
        {
            int deltaE = 3;
            int deltaSE = 5 - 2 * R;
            int d = 1 - R;
            int x = 0;
            int y = R;
            List<Position> result = new List<Position>();
            result.Add(new Position(x, y));
            
            while (y > x)
            {
                if (d < 0)   //Select E
                {
                    d += deltaE;
                    deltaE += 2;
                    deltaSE += 2;
                }
                else       //Select SE
                {
                    d += deltaSE;
                    deltaE += 2;
                    deltaSE += 4;
                    y--;
                }
                x++;
                result.Add(new Position(x,y));
            }
           
            List<Position> toAdd = new List<Position>();
            for(int i = 0; i < result.Count; ++i){
                Position pos = result[i];
                if (pos.X == 0) {
                    toAdd.Add(new Position(pos.X, -pos.Y));
                    toAdd.Add(new Position(pos.Y, pos.X));
                    toAdd.Add(new Position(-pos.Y, pos.X));
                }

                else if (pos.X == pos.Y) {
                    toAdd.Add(new Position(-pos.X, pos.Y));
                    toAdd.Add(new Position(-pos.X, -pos.Y));
                    toAdd.Add(new Position(pos.X, -pos.Y));
                }
                else if (pos.X > pos.Y) {
                    result.Remove(pos);
                    i--;
                    continue;
                }
                else {
                    toAdd.Add(new Position(-pos.X, pos.Y));
                    toAdd.Add(new Position(-pos.X, -pos.Y));
                    toAdd.Add(new Position(pos.X, -pos.Y));
                    toAdd.Add(new Position(pos.Y, pos.X));
                    toAdd.Add(new Position(-pos.Y, pos.X));
                    toAdd.Add(new Position(pos.Y, -pos.X));
                    toAdd.Add(new Position(-pos.Y, -pos.X));
                } 
            }
            result.AddRange(toAdd);
            return result;
        }
    }
}
