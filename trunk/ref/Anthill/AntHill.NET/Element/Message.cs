using System;
using System.Collections.Generic;

namespace AntHill.NET
{
    public enum MessageType
    {
        QueenIsHungry,
        QueenInDanger,
        FoodLocalization,
        SpiderLocalization
    }

    public class Message : Element
    {
        private MessageType type;
        private Position targetPosition;
        private LIList<PointWithIntensity> points;

        public Position TargetPosition { get { return targetPosition; } }
        public LIList<PointWithIntensity> Points { get { return points; } }
        public MessageType GetMessageType { get { return type; } }
        public bool Empty { get { return points.Count == 0; } }

        public void AddPointWithIntensity(Tile t, int intensity, Map map)
        {
            points.AddLast(new PointWithIntensity(t, intensity));
            map.AddMessage(this.GetMessageType, t.Position);
        }

        public Message(Position pos, MessageType mt, Position targetPosition) : base(pos)
        {
            type = mt;
            points = new LIList<PointWithIntensity>();
            this.targetPosition = new Position(targetPosition);
        }

        public PointWithIntensity GetPointWithIntensity(Position p)
        {
            LIList<PointWithIntensity>.Enumerator e = points.GetEnumerator();
            while(e.MoveNext())            
                if (e.Current.Tile.Position == p) return e.Current;            
            return null;
        }

        /*
        private int GetPointIndex(int x, int y)
        {           
            LIList<PointWithIntensity>.Enumerator e = points.GetEnumerator();
            int i = 0;
            while (e.MoveNext())
            {
                Tile t = e.Current.Tile;
                if (t.Position.X == x && t.Position.Y == y)
                    return i;
                i++;
            }            
            return -1;
        }
         */
        
        public override bool Maintain(ISimulationWorld isw)
        {
            LinkedListNode<PointWithIntensity> msg = points.First;
            LinkedListNode<PointWithIntensity> msgT;
            while (msg != null)
            {
                if (--msg.Value.Intensity <= 0)
                {
                    isw.GetMap().RemoveMessage(this.GetMessageType, msg.Value.Tile.Position);
                    msg.Value.Tile.messages.Remove(this);

                    msgT = msg;
                    msg = msg.Next;
                    points.Remove(msgT);
                }
                else
                {
                    msg = msg.Next;
                }
            }
            return true;
        }

        public void Spread(ISimulationWorld isw, Position point, int intensity)
        {
            int radius = AntHillConfig.messageRadius, radius2 = radius * radius;
            int i2, j2;
            Map map = isw.GetMap();
            Tile t;
            Position p;
            for (int i = -radius; i <= radius; i++)
            {
                i2 = i * i;
                for (int j = -radius; j <= radius; j++)
                {
                    j2 = j * j;
                    if (i2 + j2 <= radius2)
                    {
                        p = new Position(i + point.X, j + point.Y);
                        if (map.IsInside(p))
                        {// czy wogole w srodku
                            t = map.GetTile(p);
                            if (t.TileType == TileType.Wall)
                                continue;
                            if (!t.messages.Contains(this))
                            {// nie ma w danym tile - wrzucamy
                                t.messages.AddLast(this);
                                this.points.AddLast(new PointWithIntensity(t, intensity));
                                //update map
                                map.AddMessage(this.GetMessageType, p);
                            }
                        }
                    }
                }
            }
        }
    }
}
