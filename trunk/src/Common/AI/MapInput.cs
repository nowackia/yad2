using System;
using System.Collections.Generic;
using System.Text;
using Yad.AI.General;
using Yad.Board;
using Yad.Board.Common;

namespace Yad.AI {
    /// <summary>
    /// Class that represents AStarInput related to YAD map
    /// </summary>
    public abstract class MapInput : AStarInput<Position> {

        #region Protected members

        /// <summary>
        /// The YAD map reference
        /// </summary>
        protected Map _map;

        #endregion

        #region Constructors 

        /// <summary>
        /// The basic constructor
        /// </summary>
        /// <param name="map">The reference to YAD map</param>
        public MapInput(Map map) {
            this._map = map;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns list of neighbouring nodes to the given one
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Neighbours of the given node</returns>
        public override List<Position> GetNeighbours(Position node) {
            short x = node.X;
            short y = node.Y;

            List<Position> lp = new List<Position>();
            bool minx = x > 0, maxx = x < _map.Width - 1;
            bool miny = y > 0, maxy = x < _map.Height - 1;

            short minus_x = ((short)(x - 1));
            short plus_x = ((short)(x + 1));
            short plus_y = ((short)(y + 1));
            short minus_y = ((short)(y - 1));
            if (minx && IsMoveable(minus_x, y))
                lp.Add(new Position(minus_x, y));
            if (maxx && IsMoveable(plus_x, y))
                lp.Add(new Position(plus_x, y));
            if (miny && IsMoveable(x, minus_y))
                lp.Add(new Position(x, minus_y));
            if (maxy && IsMoveable(x, plus_y))
                lp.Add(new Position(x, plus_y));
            if (minx && miny && IsMoveable(minus_x, minus_y))
                lp.Add(new Position(minus_x, minus_y));
            if (minx && maxy && IsMoveable(minus_x, plus_y))
                lp.Add(new Position(minus_x, plus_y));
            if (maxx && miny && IsMoveable(plus_x, minus_y))
                lp.Add(new Position(plus_x, minus_y));
            if (maxx && maxy && IsMoveable(plus_x, plus_y))
                lp.Add(new Position(plus_x, plus_y));
            return lp;
        }

        /// <summary>
        /// Heuristic function
        /// </summary>
        /// <param name="node">Node which heuristic is to be counted</param>
        /// <returns>Heuristic value of the given node</returns>
        public override int H(Position node) {
            return (int)Math.Floor(Math.Sqrt((node.X - Goal.X) * (node.X - Goal.X) +
                (node.Y - Goal.Y) * (node.Y - Goal.Y)));
        }

        /// <summary>
        /// Node
        /// </summary>
        /// <returns>Propable number of nodes to consider</returns>
        public override int EvalNodesNumber() {
            return _map.Width * _map.Height;
        }

        /// <summary>
        /// Gets weight of the node
        /// </summary>
        /// <returns>Returns the weight of the node</returns>
        public override int GetWeight(Position pos) {
            return 1;
        }

        #endregion

        #region Protected Methods 

        /// <summary>
        /// Function checks whether move to position (x,y) is possible
        /// </summary>
        /// <param name="x">x-coordinate of field to move</param>
        /// <param name="y">y-coordinate of field to move</param>
        /// <returns>true, if the it is possible to move onto field (x,y)</returns>
        protected abstract bool IsMoveable(short x, short y);

        #endregion
    }
}
