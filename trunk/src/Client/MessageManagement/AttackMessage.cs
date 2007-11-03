using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Message
{
    public class AttackMessage:GameMessage
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private Object place;

        public Object Place
        {
            get { return place; }
            set { place = value; }
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
