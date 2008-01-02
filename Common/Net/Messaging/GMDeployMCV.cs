using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using Yad.Board;

namespace Yad.Net.Messaging {
	public class GMDeployMCV : GameMessage {

        private ObjectID mcvID;

        public ObjectID McvID {
            get { return mcvID; }
            set { mcvID = value; }
        }

		public GMDeployMCV()
            : base(MessageType.DeployMCV)
        { }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
			mcvID.Deserialize(reader);
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            mcvID.Serialize(writer);
        }

        public override string ToString() {
            return base.ToString() + " GMDeployMCV: mcvID: " + mcvID.ToString();  
        }

	}
}
