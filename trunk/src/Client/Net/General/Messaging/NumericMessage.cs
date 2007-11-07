using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging
{
    public class NumericMessage : Message
    {
        private int number;

        public NumericMessage()
            : base(MessageType.Numeric)
        { }

        public NumericMessage(MessageType msgType)
            : base(msgType)
        { }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }


    }
}
