using System;
using System.Collections.Generic;
using System.Text;
using Client.MessageManagement;
using Client.Board;

namespace Client.Engine
{
    public abstract class Simulation
    {
        public static int delta = 3;
        // turn counter incremented by message
        int turnNumber;
        Map map;
        Queue<Queue<Message>> messages;


        public abstract void Update();
    }
}
