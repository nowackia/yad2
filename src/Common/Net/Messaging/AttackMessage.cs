using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    public class AttackMessage : GameMessage
    {
        private int id;

        public AttackMessage()
            : base(MessageType.Attack)
        { }

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
