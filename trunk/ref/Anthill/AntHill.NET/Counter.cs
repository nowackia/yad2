using System;

namespace AntHill.NET
{
    class Counter
    {
        int rounds = 0;
        int frames = 0;
        int roundTick;
        int frameTick;
        int fps=0;
        int rps=0;

        public Counter()
        {
            roundTick = frameTick = Environment.TickCount-1;
        }

        public void FrameTick()
        {
            if (Environment.TickCount - frameTick > 1000)
            {
                fps = frames;
                frames = 0;
                frameTick = Environment.TickCount;
            }
            ++frames;
        }

        public void RoundTick()
        {
            if (Environment.TickCount - roundTick > 1000)
            {
                rps = rounds;
                rounds = 0;
                roundTick = Environment.TickCount ;
            }
            ++rounds;
        }

        public double FPS { get { return fps; } }
        public double RPS { get { return rps; } }
    }
}
