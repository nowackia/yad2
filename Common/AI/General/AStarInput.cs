using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board;

namespace Yad.AI.General {

    /// <summary>
    /// Class that represents AStarInput
    /// 1. Function GetNeighbours generates all neighbouring node to the given one
    /// 2. Function H is a heuristic function - it should count some kind of metrics.
    /// For YAD Euclidesian or Manhattan is recommended
    /// 3. Function EvalNodesNumber helps to allocate sufficient memory
    /// 4. Function GetWeight is used to set a weight to the field. Using weight option makes
    /// unit enter one field more often than another
    /// </summary>
    /// <typeparam name="V"></typeparam>
    public abstract class AStarInput<V> {

        #region Private members

        /// <summary>
        /// Start position
        /// </summary>
        private V start;

        /// <summary>
        /// Goal position
        /// </summary>
        private V goal;

        /// <summary>
        /// Maximum depth of the search - if its reached search stops
        /// </summary>
        private int maxDepth;

        #endregion

        #region Properites

        /// <summary>
        /// Maximum depth of the search - if its reached search stops
        /// </summary>
        public int MaxDepth {
            get { return maxDepth; }
            set { maxDepth = value; }
        }

        /// <summary>
        /// Goal position
        /// </summary>
        public V Goal {
            get { return goal; }
            set { goal = value; }
        }

        /// <summary>
        /// Start position
        /// </summary>
        public V Start {
            get { return start; }
            set { start = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns list of neighbouring nodes to the given one
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Neighbours of the given node</returns>
        public abstract List<V> GetNeighbours(V node);

        /// <summary>
        /// Heuristic function
        /// </summary>
        /// <param name="node">Node which heuristic is to be counted</param>
        /// <returns>Heuristic value of the given node</returns>
        public abstract int H(V node);

        /// <summary>
        /// Node
        /// </summary>
        /// <returns>Propable number of nodes to consider</returns>
        public abstract int EvalNodesNumber();

        /// <summary>
        /// Gets weight of the node
        /// </summary>
        /// <returns>Returns the weight of the node</returns>
        public abstract int GetWeight(V pos);

        #endregion
    }
}
