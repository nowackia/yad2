using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Board.Common
{
    [Serializable]
    public class MapData
    {
        TileData[][] _data;

        public MapData(int width, int height)
        {
            _data = new TileData[width][];

            for (int i = 0; i < width; ++i)
            {
                _data[i] = new TileData[height];
                for (int j = 0; j < height; ++j)
                    _data[i][j] = new TileData();
            }
        }

        public TileData[] this[int index]
        {
            get {
                return _data[index];
            }
        }
    }
}
