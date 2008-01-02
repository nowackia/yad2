using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board.Common;
using Yad.Board;

namespace Yad.Net.Messaging.Common
{
    public class AttackMessage : GameMessage
    {
		private ObjectID _attackingObject, _attackedObject;

        public AttackMessage()
            : base(MessageType.Attack)
        { }

		public ObjectID Attacker {
			get { return this._attackingObject; }
            set { _attackingObject = value; }
		}

		public ObjectID Attacked {
			get { return this._attackedObject; }
            set { _attackedObject = value; }
		}

		public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
			_attackingObject.Deserialize(reader);
			_attackedObject.Deserialize(reader);
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
			_attackingObject.Serialize(writer);
			_attackedObject.Serialize(writer);
        }

        public override string ToString() {
            return base.ToString() + "AttackMessage: _attackingObject: " + 
                _attackingObject + " _attackedObject: " + _attackedObject;
        }
    }
}
