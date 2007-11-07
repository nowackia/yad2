using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging
{
    public class MoveMessage : GameMessage
    {
        private int idUnit;
        //TODO RS: table or list? what type of structure? Drawing.Point?
        private LinkedListNode<Object> path;

        public MoveMessage()
            : base(MessageType.Move)
        { }

        public LinkedListNode<Object> Path
        {
            get { return path; }
            set { path = value; }
        }

        public int IdUnit
        {
            get { return idUnit; }
            set { idUnit = value; }
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
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
        }
    }
}
