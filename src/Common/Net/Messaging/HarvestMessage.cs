using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;

namespace Yad.Net.Messaging.Common
{
    public class HarvestMessage : GameMessage
    {
        private LinkedList<Position> path;
        private int idUnit;

        public HarvestMessage()
            : base(MessageType.Harvest)
        { }

        public LinkedList<Position> Path
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
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
