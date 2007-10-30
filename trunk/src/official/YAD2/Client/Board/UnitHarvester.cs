using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Board
{
    public class UnitHarvester : Unit
    {
        private int capacity;
        public int Capacity
        {
            get { return capacity; }
        }


        public override void Destroy()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Move()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DoAI()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
