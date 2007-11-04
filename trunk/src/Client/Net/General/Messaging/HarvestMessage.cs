using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging
{
    public class HarvestMessage : GameMessage
    {
        private LinkedList<Object> path;

        public LinkedList<Object> Path
        {
            get { return path; }
            set { path = value; }
        }

        private int idUnit;

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
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
