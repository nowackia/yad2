using System;
using System.Collections.Generic;
using System.Text;
using Yad.DataStructures;

namespace Yad.AI.General {
    public class AStar {

        #region Private Methods 

        private static LinkedList<V> GetPath<V>(AStarNode<V> lastNode) {
            LinkedList<V> list = new LinkedList<V>();
            do {
                list.AddFirst(lastNode.Value);
                lastNode = lastNode.Parent;
            } while (lastNode != null);
            return list;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Method creates the shortest path between start and goal defined in input
        /// </summary>
        /// <typeparam name="V">Class that represents position</typeparam>
        /// <param name="input">Algorithm input data <see cref="Yad.AI.General.AStarInput"/></param>
        /// <returns>Path as linked list the first element is the start position
        /// the last one is the goal position. If there is no path between start
        /// and goal then null is returned. If the maximum search treshold is reached
        /// tle last position is not the goal</returns>
        public static LinkedList<V> Search<V>(AStarInput<V> input) {

            int max = input.EvalNodesNumber();
            PriorityQueue<AStarNode<V>> open = new PriorityQueue<AStarNode<V>>(max);
            List<AStarNode<V>> closed = new List<AStarNode<V>>(max);
            Dictionary<V, AStarNode<V>> openitems = new Dictionary<V, AStarNode<V>>(max);
            Dictionary<V, AStarNode<V>> allitems = new Dictionary<V, AStarNode<V>>(max);

            AStarNode<V> s = new AStarNode<V>(input.Start, 0, null);
            s.Hvalue = input.H(s.Value);
            open.Insert(s);
            openitems.Add(s.Value, s);
            allitems.Add(s.Value, s);

            while (open.Size() > 0) {
                AStarNode<V> n = open.Remove();
                openitems.Remove(n.Value);
                closed.Add(n);

                if (n.Value.Equals(input.Goal) || input.MaxDepth == n.Depth)
                    return GetPath(n);
                List<V> neighs = input.GetNeighbours(n.Value);
                foreach (V pos in neighs) {
                    AStarNode<V> newNode = new AStarNode<V>(pos, input.GetWeight(pos), n);
                    if (allitems.ContainsKey(newNode.Value)) {
                        AStarNode<V> oldNode = allitems[newNode.Value];
                        if (oldNode.Weight > newNode.Weight) {
                            newNode.Hvalue = oldNode.Hvalue;
                            if (openitems.ContainsKey(newNode.Value)) {
                                open.Change(oldNode, newNode);
                                openitems.Add(newNode.Value, newNode);
                                allitems.Add(newNode.Value, newNode);
                                continue;
                            }
                            else {
                                closed.Remove(oldNode);
                                openitems.Add(newNode.Value, newNode);
                                allitems.Add(newNode.Value, newNode);
                                open.Insert(newNode);
                            }
                        }
                        continue;

                    }
                    newNode.Hvalue = input.H(newNode.Value);
                    openitems.Add(newNode.Value, newNode);
                    allitems.Add(newNode.Value, newNode);
                    open.Insert(newNode);
                }
            }
            return null;
        }

        #endregion
    }
}
