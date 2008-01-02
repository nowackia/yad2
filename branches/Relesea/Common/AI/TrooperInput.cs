using System;
using System.Collections.Generic;
using System.Text;
using Yad.AI.General;
using Yad.Board;
using Yad.Board.Common;

namespace Yad.AI {
    /// <summary>
    /// Class that represents AStarInput for a trooper unit
    /// </summary>
    /*public class TrooperInput : MapInput {

        #region Constructors 

        /// <summary>
        /// The basic constructor
        /// </summary>
        /// <param name="start">The start position</param>
        /// <param name="goal">The goal position</param>
        /// <param name="depth">The search depth</param>
        /// <param name="map">The Yad map</param>
        public TrooperInput(Position start, Position goal, int depth, Map map)
            : base(map) {
            base.Start = start;
            base.Goal = goal;
            base.MaxDepth = depth;
        }

        #endregion

        #region Protected methods 

        /// <summary>
        /// Function checks whether move to position (x,y) is possible
        /// </summary>
        /// <param name="x">x-coordinate of field to move</param>
        /// <param name="y">y-coordinate of field to move</param>
        /// <returns>true, if the it is possible to move onto field (x,y)</returns>
        protected override bool IsMoveable(short x, short y) {
            if (_map.Units[x, y].Count == 0 && _map.Buildings[x, y].Count == 0)
                return true;
            return false;
        }

        #endregion
    }*/
}
