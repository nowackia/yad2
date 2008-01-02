using System;
using System.Collections.Generic;
using System.Text;
using Yad.AI.General;
using Yad.Board;
namespace AStarTest
{
    public class TestInput : AStarInput<Position>
    {
        public delegate bool MoveCheckDelegate(short x, short y, int[,] tiles);

        #region Protected members

        /// <summary>
        /// The YAD map reference
        /// </summary>
        protected int[,] tiles;
        int width;
        int height;

        private event MoveCheckDelegate isMoveable;

        #endregion

        public event MoveCheckDelegate IsMoveable
        {
            add
            {
                isMoveable += value;
            }
            remove
            {
                isMoveable -= value;
            }
        }

        #region Constructors 

        /// <summary>
        /// The basic constructor
        /// </summary>
        /// <param name="map">The reference to YAD map</param>
        public TestInput(int[,] tiles) {

            base.MaxDepth = tiles.GetLength(0) * tiles.GetLength(1);
            width = tiles.GetLength(0);
            height = tiles.GetLength(1);
            this.tiles = tiles;
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
            bool minx = x > 0, maxx = x < width - 1;
            bool miny = y > 0, maxy = y < height - 1;

            short minus_x = ((short)(x - 1));
            short plus_x = ((short)(x + 1));
            short plus_y = ((short)(y + 1));
            short minus_y = ((short)(y - 1));
            if (minx && isMoveable(minus_x, y, tiles))
                lp.Add(new Position(minus_x, y));
            if (maxx && isMoveable(plus_x, y, tiles))
                lp.Add(new Position(plus_x, y));
            if (miny && isMoveable(x, minus_y, tiles))
                lp.Add(new Position(x, minus_y));
            if (maxy && isMoveable(x, plus_y, tiles))
                lp.Add(new Position(x, plus_y));
            if (minx && miny && isMoveable(minus_x, minus_y, tiles))
                lp.Add(new Position(minus_x, minus_y));
            if (minx && maxy && isMoveable(minus_x, plus_y, tiles))
                lp.Add(new Position(minus_x, plus_y));
            if (maxx && miny && isMoveable(plus_x, minus_y, tiles))
                lp.Add(new Position(plus_x, minus_y));
            if (maxx && maxy && isMoveable(plus_x, plus_y, tiles))
                lp.Add(new Position(plus_x, plus_y));
            return lp;
        }

        /// <summary>
        /// Heuristic function
        /// </summary>
        /// <param name="node">Node which heuristic is to be counted</param>
        /// <returns>Heuristic value of the given node</returns>
        public override int H(Position node) {
            return Math.Abs(node.X - Goal.X) +
                Math.Abs(node.Y - Goal.Y);

        }

        /// <summary>
        /// Node
        /// </summary>
        /// <returns>Propable number of nodes to consider</returns>
        public override int EvalNodesNumber() {
            return width * height;
        }

        /// <summary>
        /// Gets weight of the node
        /// </summary>
        /// <returns>Returns the weight of the node</returns>
        public override int GetWeight(Position pos) {
            return 1;
        }

        #endregion
    }
}
