using System;
using System.Collections.Generic;
using System.Text;

namespace CustomTests {
    class TestException: Exception {
        public TestException(String s): base(s) {
            
        }
    }
     abstract class  AbstractTest {
        public abstract void Test();
    }
}
