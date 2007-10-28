using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Message
{
    public class DestroyMessage : GameMessage
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public override void Process()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Execute()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
