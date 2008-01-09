using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board.Common;

namespace Yad.Board
{
    [Serializable]
    public class TileData
    {
        private const TileType DefaultType = TileType.Sand;
        public const int ThickSpiceTreshold = 10;

        TileType _type = DefaultType;

        public TileType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        int _spiceNo;

        public int SpiceNo
        {
            get { return _spiceNo; }
            set { _spiceNo = value;
            if (_spiceNo < 0)
                _spiceNo = 0;
            }
        }

        public bool IsSpice
        {
            get
            {
                return _spiceNo > 0;
            }
        }

        public bool IsSpiceThin
        {
            get
            {
                return IsSpice && _spiceNo < ThickSpiceTreshold;
            }
        }

        public bool IsSpiceThick
        {
            get
            {
                return IsSpice && _spiceNo >= ThickSpiceTreshold;
            }
        }


    }
}
