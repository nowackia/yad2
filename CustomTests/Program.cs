using System;
using System.Collections.Generic;
using System.Text;

namespace CustomTests {
    class Program {
        static void Main(string[] args) {
            List<AbstractTest> tests = new List<AbstractTest>();
            tests.Add(new SpiralTest());




            int i = 1;
            foreach (AbstractTest test in tests) {
                System.Console.Out.WriteLine("Test nr. " + i);
                try {
                    test.Test();
                } catch (TestException te) {
                    System.Console.Out.WriteLine(te.Message);
                }
            }
        }
    }
}
