using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Client
{
    public class GameInitInfo
    {
        private short playerId;

        private int xPosition;
        private int yPosition;

        public GameInitInfo(short playerId, int x, int y)
        {
            this.playerId = playerId;
            this.xPosition = x;
            this.yPosition = y;
        }

        public GameInitInfo(int playerId, int x, int y)
            : this((short)playerId, x, y)
        { }

        public short PlayerId
        {
            get
            { return playerId; }
        }

        public int X
        {
            get
            { return xPosition; }
            set
            { xPosition = value; }
        }

        public int Y
        {
            get
            { return yPosition; }
            set
            { yPosition = value; }
        }
    }
}
