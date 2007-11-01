using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Board.Renderers
{
    static class RendererPool
    {
        static private AmmoRenderer ammoRenderer = new AmmoRenderer();
        static public AmmoRenderer AmmoRenderer
        {
            get { return ammoRenderer; }
        }
        static private BuildingRenderer buildingRenderer = new BuildingRenderer();
        static public BuildingRenderer BuildingRenderer
        {
            get { return buildingRenderer; }
        }
        static private MapRenderer mapRenderer = new MapRenderer();
        static public MapRenderer MapRenderer
        {
            get { return mapRenderer; }
        }
        static private SandwormRenderer sandwormRenderer = new SandwormRenderer();
        static public SandwormRenderer SandwormRenderer
        {
            get { return sandwormRenderer; }
        }

        static private MCVRenderer mCVRenderer = new MCVRenderer();
        static public MCVRenderer MCVRenderer
        {
            get { return mCVRenderer; }
        }
        static private TankRenderer tankRenderer;
        static public TankRenderer TankRenderer
        {
            get { return tankRenderer; }
        }
        static private TrooperRenderer trooperRenderer;
        static public TrooperRenderer TrooperRenderer
        {
            get { return trooperRenderer; }
        }

        static private MiniMapRenderer miniMapRenderer;
        static public MiniMapRenderer MiniMapRenderer
        {
            get { return miniMapRenderer; }
        }



    }
}
