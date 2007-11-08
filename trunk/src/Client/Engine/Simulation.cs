using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.General.Messaging;
using Client.Board;

namespace Client.Engine
{
    public abstract class Simulation
    {
        public static int delta = 3;
        // turn counter incremented by message
        int turnNumber;
        Queue<Queue<Message>> messages;

        public abstract void Update();
    }
}
