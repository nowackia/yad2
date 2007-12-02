using System;
using System.Collections.Generic;
using System.Text;
using AntHill.NET;

namespace astar
{
    static class Astar
    {
        class AstarNode : IComparable, AntHill.NET.Heap.IEquality<AstarNode>
        {
            
            private AstarNode parent;

            public AstarNode Parent
            {
                get { return parent; }
                set { parent = value; }
            }

            private int deep;

            public int Deep
            {
                get { return deep; }
                set { deep = value; }
            }


            private double cost;
            public double Cost
            {
                get { return cost; }
                set { cost = value; }
            }

            private double goalEstimate;

            public double GoalEstimate
            {
                get { return goalEstimate; }
                set { goalEstimate = value; }
            }

            public double TotalCost
            {
                get { return Cost + GoalEstimate; }

            }

            public int CompareTo(Object other)
            {
                if (!(other is AstarNode)) return -1;
                return -this.TotalCost.CompareTo(((AstarNode)other).TotalCost);
            }


            public AstarNode(KeyValuePair<int, int> loc, double weightCost, double parentCost,int lastDeep)
            {
                this.loc = loc;
                this.deep = lastDeep + 1;
                cost = weightCost + parentCost;
                if (weightCost >= int.MaxValue) isObstacle = true;
            }

            private bool isObstacle;

            public bool IsObstacle
            {
                get { return isObstacle; }
                set { isObstacle = value; }
            }

            private KeyValuePair<int, int> loc;

            public KeyValuePair<int, int> Loc
            {
                get { return loc; }
                set { loc = value; }
            }

            public void Calc(KeyValuePair<int, int> goal)
            {
                GoalEstimate = Math.Sqrt(Math.Pow(this.loc.Key - goal.Key, 2) + Math.Pow(this.loc.Value - goal.Value, 2));
            }


            #region IEquality<AstarNode> Members

            public bool EqualTo(AstarNode to)
            {
                return this.loc.Key ==to.loc.Key && this.loc.Value==to.loc.Value;
            }

            #endregion
        }
        public const int max_depth = 10;
        static AntHill.NET.Heap.Heap<AstarNode> openHeap = new AntHill.NET.Heap.Heap<AstarNode>();
        static AntHill.NET.Heap.Heap<AstarNode> closedHeap = new AntHill.NET.Heap.Heap<AstarNode>();
        static System.Collections.Generic.Dictionary<KeyValuePair<int, int>, AstarNode> dict = new Dictionary<KeyValuePair<int, int>, AstarNode>();
        static private int width;
        static private int height;

        static public int Height
        {
            get { return height; }
            set { height = value; }
        }

        static public int Width
        {
            get { return width; }
            set { width = value; }
        }

        static public void Init(int width, int height)
        {
            Width = width;
            Height = height;
        }

        static List<KeyValuePair<int, int>> CreatePath(AstarNode last)
        {
            List<KeyValuePair<int, int>> path = new List<KeyValuePair<int, int>>();
            do
            {
                path.Add(last.Loc);
                last = (AstarNode)last.Parent;
            } while (last != null);

            path.Reverse();

            return path;
        }

        static public List<KeyValuePair<int, int>> Search(KeyValuePair<int, int> start, KeyValuePair<int, int> goal, IAstar ia)
        {
            openHeap = new AntHill.NET.Heap.Heap<AstarNode>();
            closedHeap = new AntHill.NET.Heap.Heap<AstarNode>();
            AstarNode StartNode = new AstarNode(start, ia.GetWeight(start.Key, start.Value),0,0);
            StartNode.Parent = null;
            StartNode.Calc(goal);
            dict=new Dictionary<KeyValuePair<int,int>,AstarNode>();
            int idx;
            openHeap.Insert(StartNode);
            dict.Add(StartNode.Loc, StartNode);
            bool inClosed;
            bool flag;
            List<AstarNode> near;
            List<AstarNode>.Enumerator e;
            AstarNode other;
            while (openHeap.Count != 0)
            {
                AstarNode node = openHeap.DeleteMax();
                dict.Remove(node.Loc);
                if (node.Loc.Equals(goal) || node.Deep ==  max_depth)
                {
                    return CreatePath(node);
                }
                near = NearNodes(node, ia);
                e = near.GetEnumerator();
                while(e.MoveNext())
                {
                    other = e.Current;
                    if (other.IsObstacle) { closedHeap.Insert(other); /*Closed.Add(other)*/; continue; }
                    inClosed = false;
                    other.Calc(goal);
                    flag = false;
                    if (dict.ContainsKey(other.Loc))
                    {
                        if (dict[other.Loc].TotalCost < other.TotalCost) continue;
                        flag = true;
                    }
                    else if ((idx= closedHeap.Contains(other))!=-1)
                    {
                        inClosed = true;
                        if (closedHeap[idx].TotalCost < other.TotalCost) continue;
                    }
                    other.Parent = node;
                    if (inClosed) closedHeap.Remove(other);
                    else
                    {
                        if (flag)
                        {
                            openHeap.Remove(dict[other.Loc]);
                            dict.Remove(other.Loc);
                        }
                        openHeap.Insert(other);
                        dict.Add(other.Loc, other);
                    }

                }//foreach
                closedHeap.Insert(node);
            }
            List<KeyValuePair<int, int>> l=new List<KeyValuePair<int,int>>();
            return l;
        }

        static List<AstarNode> NearNodes(AstarNode center, IAstar ia)
        {
            List<AstarNode> list = new List<AstarNode>();
            KeyValuePair<int, int> loc;
            if (Inside(loc = new KeyValuePair<int, int>(center.Loc.Key, center.Loc.Value - 1)))
            {
                list.Add(new AstarNode(loc,ia.GetWeight(center.Loc.Key, center.Loc.Value - 1),center.Cost,center.Deep));
            }
            if (Inside(loc = new KeyValuePair<int, int>(center.Loc.Key - 1, center.Loc.Value)))
            {
                list.Add(new AstarNode(loc, ia.GetWeight(center.Loc.Key - 1, center.Loc.Value), center.Cost, center.Deep));
            }
            if (Inside(loc = new KeyValuePair<int, int>(center.Loc.Key + 1, center.Loc.Value)))
            {
                list.Add(new AstarNode(loc, ia.GetWeight(center.Loc.Key + 1, center.Loc.Value), center.Cost, center.Deep));
            }
            if (Inside(loc = new KeyValuePair<int, int>(center.Loc.Key, center.Loc.Value + 1)))
            {
                list.Add(new AstarNode(loc, ia.GetWeight(center.Loc.Key, center.Loc.Value + 1), center.Cost, center.Deep));
            }
            return list;
        }

        static bool Inside(KeyValuePair<int, int> loc)
        {
            return loc.Key >= 0 && loc.Key < width && loc.Value >= 0 && loc.Value < height;
        }
    }
}
