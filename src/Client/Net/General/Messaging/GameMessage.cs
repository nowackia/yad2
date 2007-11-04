using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging
{
    public abstract class GameMessage : Message
    {
        private int idTurn;

        public int IdTurn
        {
            get { return idTurn; }
            set { idTurn = value; }
        }
        //public abstract void Process();
        //public abstract void Execute();
    }
}
