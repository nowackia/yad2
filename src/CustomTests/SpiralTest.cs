using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;
using Yad.Board.Common;

namespace CustomTests {
    class SpiralTest: AbstractTest {
        public override void Test() {
            int limit;
            Position[] p;
            System.Console.Out.WriteLine("#0");
            p = BoardObject.RangeSpiral(0, out limit);
            if (limit != 1) {
                throw new TestException(limit + " - zly limit");
            }
            System.Console.Out.WriteLine("#1");
            p = BoardObject.RangeSpiral(1, out limit);
            
            foreach (Position pos in p) {
                System.Console.Out.WriteLine(pos.X + " " + pos.Y);
            }
            if (limit != 9) {
                throw new TestException(limit + " - zly limit");
            }
            System.Console.Out.WriteLine("#2");
            p = BoardObject.RangeSpiral(2, out limit);
            foreach (Position pos in p) {
                //System.Console.Out.WriteLine(pos.X + " " + pos.Y);
            }
            if (limit != 21) {
                throw new TestException(limit + " - zly limit");
            }
            int c;
            p = BoardObject.RangeSpiral(30, out c);

            System.Console.Out.WriteLine("############## " + c);
            if (c != 2895) {
                System.Console.Out.WriteLine("Dzwon do radzia");
            }


        }
    }
}
