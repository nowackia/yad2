using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Message
{
    public class NumericMessage:ControlMessage
    {
        private int number;

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

    }
}
