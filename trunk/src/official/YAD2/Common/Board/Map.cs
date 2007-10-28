using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Board
{
    class Map:IRenderable
    {
        TileType[,] tiles;
        LinkedList<Building> buildings;
        LinkedList<Unit> units;
        IRenderable renderer;

        public Map()
        {
            buildings = new LinkedList<Building>();
            units = new LinkedList<Unit>();
        }
        #region IRenderable Members

        public void Render()
        {
            if (renderer != null)
            {
                renderer.Render();
            }
        }

        #endregion
    }
}
