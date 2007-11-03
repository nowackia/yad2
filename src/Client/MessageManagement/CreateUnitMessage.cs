using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Message
{
    public class CreateUnitMessage : GameMessage
    {
        private int idUnit;
        private Object place;

        public Object Place
        {
            get { return place; }
            set { place = value; }
        }

        public int IdUnit
        {
            get { return idUnit; }
            set { idUnit = value; }
        }
        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
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
