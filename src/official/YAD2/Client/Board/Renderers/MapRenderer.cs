using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Board.Renderers
{
    class MapRenderer : IRenderer
    {

        #region IRenderer Members

        public void Render(BoardObject boardObject)
        {
            Map map = (Map)boardObject;
            /* 1) renderowanie tile */


            /* 2) renderowanie budynkow */

            BuildingRenderer buildingRenderer = RendererPool.BuildingRenderer;
            LinkedList<Building>.Enumerator buildingEnumerator = map.Buildings.GetEnumerator();
            while (buildingEnumerator.MoveNext())
            {
                buildingRenderer.Render(buildingEnumerator.Current);
            }

            /* 3) renderowanie jednostek */
            TropperRenderer trooperRenderer = RendererPool.TrooperRenderer;
            LinkedList<Unit>.Enumerator unitEnumerator = map.Units.GetEnumerator();
            while (unitEnumerator.MoveNext())
            {
                trooperRenderer.Render(unitEnumerator.Current);
            }
            /* 4) renderowanie mgly */

            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
