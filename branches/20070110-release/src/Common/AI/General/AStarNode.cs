using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.AI.General {
    public class AStarNode<V> : IComparer<AStarNode<V>>, IComparable<AStarNode<V>> {

        /// <summary>
        /// Depth of the A* search
        /// </summary>
        private int depth;

        /// <summary>
        /// H value of the node
        /// </summary>
        private int hvalue;

        /// <summary>
        /// Weight of the node
        /// </summary>
        private int weight;

        /// <summary>
        /// Node parent
        /// </summary>
        private AStarNode<V> parent;

        /// <summary>
        /// Node value (position)
        /// </summary>
        private V value;

        public int Weight {
            get { return weight; }
            set { weight = value; }
        }
     
        public V Value {
            get { return this.value; }
            set { this.value = value; }
        }

        public AStarNode<V> Parent {
            get { return parent; }
        }

        public int Hvalue {
            get { return hvalue; }
            set { hvalue = value; }
        }

        public int Depth {
            get { return depth; }
            set { depth = value; }
        }

        public int TotalValue {
            get { return weight + hvalue; }
        }

        /// <summary>
        /// Creates a new instance of A* Node
        /// </summary>
        /// <param name="value">Value (position) of the Node</param>
        /// <param name="weight">Weight of the node</param>
        /// <param name="parent">Parent node</param>
        public AStarNode(V value, int weight, AStarNode<V> parent) {
            this.value = value;
            this.weight = weight;
            this.depth = 1;
            if (null != parent) {
                this.depth += parent.depth;
                this.weight += parent.weight;
                this.parent = parent;
            }
            else this.depth = 0;
        }

        public override int GetHashCode() {
            return value.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj is AStarNode<V>) {
                AStarNode<V> node = (AStarNode<V>)obj;
                return this.value.Equals(node.value);
            }
            return false;
        }

        #region IComparer<AStarNode<V>> Members

        public int Compare(AStarNode<V> x, AStarNode<V> y) {
            return x.TotalValue - y.TotalValue;
        }

        #endregion

        #region IComparable<AStarNode<V>> Members

        public int CompareTo(AStarNode<V> other) {
            return this.TotalValue - other.TotalValue;
        }

        #endregion
    }
}

