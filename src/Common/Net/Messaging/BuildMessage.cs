using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    public class BuildMessage : GameMessage
    {
        private int idBuilding;

        //TODO RS: struct or object?
        private Object place;

        public BuildMessage()
            : base(MessageType.Build)
        { }

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
		/*
        public override void Process()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Execute()
        {
            throw new Exception("The method or operation is not implemented.");
        }
		*/
        public override void Deserialize(System.IO.BinaryReader reader) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
