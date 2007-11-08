using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;

namespace Yad.Net.Messaging.Common
{
    public class MoveMessage : GameMessage
    {
        private int idUnit;
        private LinkedListNode<Position> path;

        public MoveMessage()
            : base(MessageType.Move)
        { }

        public LinkedListNode<Position> Path
        {
            get { return path; }
            set { path = value; }
        }

        public int IdUnit
        {
            get { return idUnit; }
            set { idUnit = value; }
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
        }
    }
}
