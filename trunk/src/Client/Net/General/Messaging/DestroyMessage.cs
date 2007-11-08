using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging
{
    public class DestroyMessage : GameMessage
    {
        private int id;

        public DestroyMessage()
            : base(MessageType.Destroy)
        { }

        public int Id
        {
            get { return id; }
            set { id = value; }
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