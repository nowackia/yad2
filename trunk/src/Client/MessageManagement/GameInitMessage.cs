using System;
using System.Collections.Generic;
using System.Text;

namespace Client.MessageManagement
{
    public class GameInitMessage : ControlMessage
    {
        // first row - id of player
        // second row - x - pos
        // third row - y - pos
        // columns  - 1 column  =  1 player
        private int[,] playerStartPoints;
        public int[,] PlayerStartPoints
        {
            get { return playerStartPoints; }
            set { playerStartPoints = value; }
        }
    }
}
