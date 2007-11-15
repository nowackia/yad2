using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board.Common;
using Yad.Engine.GameGraphics.Client;

namespace Yad.Engine.GameGraphics.Renderes.Client

{
    class MapRenderer : IRenderer
    {

        #region IRenderer Members

        public void Render(BoardObject boardObject)
        {
            //TODO:
			//renderowanie mapy

			//renderowanie jednostek z symulacji


            /* 1) renderowanie tile */

			/*
            BuildingRenderer buildingRenderer = RendererPool.BuildingRenderer;
            LinkedList<Building>.Enumerator buildingEnumerator = map.Buildings.GetEnumerator();
            while (buildingEnumerator.MoveNext())
            {
                buildingRenderer.Render(buildingEnumerator.Current);
            }
            TrooperRenderer trooperRenderer = RendererPool.TrooperRenderer;
            LinkedList<Unit>.Enumerator unitEnumerator = map.Units.GetEnumerator();
            while (unitEnumerator.MoveNext())
            {
                trooperRenderer.Render(unitEnumerator.Current);
            }
			 * */
            /* 4) renderowanie mgly */

            throw new Exception("The method or operation is not implemented.");
			
        }

        #endregion
    }
}
