using System;
using System.Collections.Generic;
using AntHill.NET.Utilities;

namespace AntHill.NET
{
    public class Rain : Element
    {
        private int timeToLive;

        public Rain(Position pos)
            : base(pos)
        {
            timeToLive = Randomizer.Next(AntHillConfig.rainMaxDuration + 1);
        }

        public int TimeToLive
        {
            get { return timeToLive; }
            set { timeToLive = value; }
        }        

        public bool IsRainOver(int x, int y)
        {
            if ((x - Position.X) < AntHillConfig.rainWidth &&
                (x - Position.X) >= 0 &&
                (y - Position.Y) < AntHillConfig.rainWidth &&
                (y - Position.Y) >= 0)
                return true;
            return false;
        }
        public bool IsRainOver(Position pos)
        {
            if ((pos.X - Position.X) < AntHillConfig.rainWidth &&
                (pos.X - Position.X) >= 0 &&
                (pos.Y - Position.Y) < AntHillConfig.rainWidth &&
                (pos.Y - Position.Y) >= 0)
                return true;
            return false;
        }

        public override bool Maintain(ISimulationWorld isw)
        {
            if (--timeToLive < 0)
            {
                isw.DeleteRain();
                return false;
            }

            LIList<Food> lFood = isw.GetVisibleFood(this);
            LIList<Ant> lAnt = isw.GetVisibleAnts(this);
            LIList<Spider> lSpider = isw.GetVisibleSpiders(this);
            LIList<Message> lMessage = isw.GetVisibleMessages(this);

            while(lFood.Count > 0)
            {
                isw.DeleteFood(lFood.First.Value);
                lFood.RemoveFirst();
            }

            while (lAnt.Count > 0)
            {
                isw.DeleteAnt(lAnt.First.Value);
                lAnt.RemoveFirst();
            }
            while (lSpider.Count > 0)
            {
                isw.DeleteSpider(lSpider.First.Value);
                lSpider.RemoveFirst();
            }

            Map map = isw.GetMap();

            if (lMessage!=null)
            {
                LinkedListNode<Message> enumMsg = lMessage.First;
                LinkedListNode<PointWithIntensity> enumPwI, enumPwItemp;
                while (enumMsg != null)
                {
                    enumPwI = enumMsg.Value.Points.First;
                    while (enumPwI != null)
                    {
                        if (IsRainOver(enumPwI.Value.Position))
                        {
                            map.RemoveMessage(enumMsg.Value.GetMessageType, enumPwI.Value.Position);
                            enumPwItemp = enumPwI;
                            enumPwI = enumPwI.Next;
                            enumMsg.Value.Points.Remove(enumPwItemp);
                        }
                        else
                            enumPwI = enumPwI.Next;
                    }
                    enumMsg = enumMsg.Next;
                } 
            }            

            // Rain is always on the map
            for (int i = 0; i < AntHillConfig.rainWidth; i++) // && i+this.Position.X < map.Width; i++)
            {
                for (int j = 0; j < AntHillConfig.rainWidth; j++) // && j+this.Position.Y < map.Height; j++)
                {
                    map.GetTile(this.Position.X + i, this.Position.Y + j).messages.Clear();
                }
            }
            return true;
        }
    }
}
