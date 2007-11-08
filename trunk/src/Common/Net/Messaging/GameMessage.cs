using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    public abstract class GameMessage : Message
    {
        private int idTurn;

        public GameMessage(MessageType msgType)
            : base(msgType)
        { }

        public int IdTurn
        {
            get { return idTurn; }
            set { idTurn = value; }
        }
        //public abstract void Process();
        //public abstract void Execute();
    }
}
