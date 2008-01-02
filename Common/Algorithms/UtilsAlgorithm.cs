using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;

namespace Yad.Algorithms {
    public interface IPositionChecker {
        bool CheckPosition(short x, short y);
    }
    public class UtilsAlgorithm {
        public static Position SurroundSearch(Position start, short maxRange, IPositionChecker checker) {
            for (short actualR = 1; actualR < maxRange; ++actualR) {
                for (short x = ((short)(start.X - actualR)); x <= start.X + actualR; ++x) {
                    short topY = ((short)(start.Y + actualR));
                    if (checker.CheckPosition(x, topY))
                        return new Position(x, topY);
                    if (checker.CheckPosition(x, (short)-topY))
                        return new Position(x, -topY);
                }
                for (short y = ((short)(start.Y - actualR + 1)); y < start.Y + actualR; ++y) {
                    short topX = ((short)(actualR + start.X));
                    if (checker.CheckPosition(topX, y))
                        return new Position(topX, y);
                    if (checker.CheckPosition((short)-topX, y))
                        return new Position(-topX, y);
                }
            }
            return start;
        }
    }
}
