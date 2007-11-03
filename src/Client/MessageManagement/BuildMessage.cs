using System;
using System.Collections.Generic;
using System.Text;

namespace Client.MessageManagement
{
    public class BuildMessage : GameMessage
    {
        private int idBuilding;
        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        //TODO RS: struct or object?
        private Object place;

        public Object Place
        {
            get { return place; }
            set { place = value; }
        }

        public int IdBuilding
        {
            get { return idBuilding; }
            set { idBuilding = value; }
        }

        public override void Process()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Execute()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Deserialize(System.IO.StreamReader reader) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Serialize(System.IO.StreamWriter writer) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
