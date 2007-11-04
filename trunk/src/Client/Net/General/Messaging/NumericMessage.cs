using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging
{
    public class NumericMessage: Message
    {
        private int number;

        public int Number
        {
            get { return number; }
            set { number = value; }
        }


    }
}
