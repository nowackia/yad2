using System;
using System.Collections.Generic;
using AntHill.NET.Utilities;

namespace AntHill.NET
{
    public class Map
    {
        #region Private Memebers
        private int _width,
                    _height;
        private Tile[,] _tiles;
        private Tile[] _indoorTiles,
                       _wallTiles,
                       _outdoorTiles;
        private int _indoorTilesCount,
                    _wallTilesCount,
                    _outdoorTilesCount;
        private MessageCount[,] _messagesCount;
        #endregion
        #region Constructor
        public Map(int w, int h, Tile[,] tiles)
        {
            this._width = w;
            this._height = h;

            this._tiles = new Tile[w, h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    this._tiles[x, y] = new Tile(tiles[x, y].TileType, tiles[x, y].Position);

            _messagesCount = new MessageCount[_width, _height];

            LIList<Tile> indoorTilesTemp = new LIList<Tile>(),
                         wallTilesTemp = new LIList<Tile>(),
                         outdoorTilesTemp = new LIList<Tile>();
            Tile t;
            for (int y = 0; y < this._height; y++)
            {
                for (int x = 0; x < this._width; x++)
                {
                    switch ((t = tiles[x, y]).TileType)
                    {
                        case TileType.Outdoor:
                            outdoorTilesTemp.AddLast(t);
                            break;
                        case TileType.Indoor:
                            indoorTilesTemp.AddLast(t);
                            break;
                        case TileType.Wall:
                            wallTilesTemp.AddLast(t);
                            break;
                    }
                }
            }

            if (outdoorTilesTemp.Count == 0)
                throw new Exception(Properties.Resources.noOutdoorTilesError);
            if (indoorTilesTemp.Count == 0)
                throw new Exception(Properties.Resources.noIndoorTilesError);

            _indoorTiles = new Tile[indoorTilesTemp.Count + wallTilesTemp.Count];
            _wallTiles = new Tile[indoorTilesTemp.Count + wallTilesTemp.Count];
            _outdoorTiles = new Tile[outdoorTilesTemp.Count];
            _indoorTilesCount = indoorTilesTemp.Count;
            _wallTilesCount = wallTilesTemp.Count;
            _outdoorTilesCount = outdoorTilesTemp.Count;

            RewriteTiles(_indoorTiles, indoorTilesTemp);
            RewriteTiles(_outdoorTiles, outdoorTilesTemp);
            RewriteTiles(_wallTiles, wallTilesTemp);
        }
        #endregion
        #region Properties
        public int GetIndoorCount { get { return _indoorTilesCount; } }
        public int GetOutdoorCount { get { return _outdoorTilesCount; } }
        public int GetWallCount { get { return _wallTilesCount; } }
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
        public MessageCount[,] MsgCount { get { return _messagesCount; } }
        #endregion

        private void RewriteTiles(Tile[] destTiles, LIList<Tile> srcTiles)
        {
            LinkedListNode<Tile> node = srcTiles.First;
            int i = 0;
            while (node != null)
            {
                destTiles[i++] = node.Value;
                node = node.Next;
            }
        }

        public bool IsInside(int x, int y)
        {
            return (x >= 0) && (x < _width) && (y >= 0) && (y < _height);
        }
        public bool IsInside(Position pos)
        {
            return (pos.X >= 0) && (pos.X < _width) && (pos.Y >= 0) && (pos.Y < _height);
        }

        public Tile GetTile(int x, int y) { return _tiles[x, y]; }
        public Tile GetTile(Position pos) { return _tiles[pos.X, pos.Y]; }

        public Tile GetRandomTile(TileType tt)
        {
            switch (tt)
            {
                case TileType.Wall:
                    if (_wallTilesCount == 0) return null;
                    return _wallTiles[Randomizer.Next(_wallTilesCount)];
                case TileType.Outdoor:
                    if (_outdoorTilesCount == 0) return null;
                    return _outdoorTiles[Randomizer.Next(_outdoorTilesCount)];
                case TileType.Indoor:
                    if (_indoorTilesCount == 0) return null;
                    return _indoorTiles[Randomizer.Next(_indoorTilesCount)];
                default:
                    return null;
            }
        }

        public Tile GetRandomIndoorOrOutdoorTile()
        {
            int c = Randomizer.Next(_outdoorTilesCount + _indoorTilesCount);
            if (c < _outdoorTilesCount)
                return _outdoorTiles[c];
            return _indoorTiles[c - _outdoorTilesCount];
        }

        public void DestroyWall(Tile t)
        {
            if (t.TileType != TileType.Wall) return;
            int i=0;
            while (i < _wallTilesCount)
            {
                if (_wallTiles[i].Position == t.Position)
                    break;
                i++;
            }
            if (i == _wallTilesCount) return; /* it might happen if this "t" is a fake! */

            t.TileType = TileType.Indoor;
            _indoorTiles[_indoorTilesCount++] = t;
            _wallTiles[i] = _wallTiles[--_wallTilesCount];
        }

        #region MessageCount-Specific
        public void AddMessage(MessageType mt, Position pos)
        {
            _messagesCount[pos.X, pos.Y].IncreaseCount(mt);
        }
        public void AddMessage(MessageType mt, int x, int y)
        {
            _messagesCount[x, y].IncreaseCount(mt);
        }

        public void RemoveMessage(MessageType mt, Position pos)
        {
            _messagesCount[pos.X, pos.Y].LowerCount(mt);
        }
        #endregion
        #region MessageCount
        public struct MessageCount
        {
            int queenInDanger,
                queenIsHungry,
                spiderLocation,
                foodLocation;

            public int GetCount(MessageType mt)
            {
                switch (mt)
                {
                    case MessageType.QueenIsHungry:
                        return queenIsHungry;
                    case MessageType.QueenInDanger:
                        return queenInDanger;
                    case MessageType.FoodLocalization:
                        return foodLocation;
                    case MessageType.SpiderLocalization:
                        return spiderLocation;
                }
                return -1;
            }

            public void SetCount(MessageType mt, int count)
            {
                switch (mt)
                {
                    case MessageType.QueenIsHungry:
                        queenIsHungry = count;
                        break;
                    case MessageType.QueenInDanger:
                        queenInDanger = count;
                        break;
                    case MessageType.FoodLocalization:
                        foodLocation = count;
                        break;
                    case MessageType.SpiderLocalization:
                        spiderLocation = count;
                        break;
                }
            }

            public void IncreaseCount(MessageType mt)
            {
                switch (mt)
                {
                    case MessageType.QueenIsHungry:
                        queenIsHungry++;
                        break;
                    case MessageType.QueenInDanger:
                        queenInDanger++;
                        break;
                    case MessageType.FoodLocalization:
                        foodLocation++;
                        break;
                    case MessageType.SpiderLocalization:
                        spiderLocation++;
                        break;
                }
            }

            public void LowerCount(MessageType mt)
            {
                switch (mt)
                {
                    case MessageType.QueenIsHungry:
                        --queenIsHungry;
                        break;
                    case MessageType.QueenInDanger:
                        --queenInDanger;
                        break;
                    case MessageType.FoodLocalization:
                        --foodLocation;
                        break;
                    case MessageType.SpiderLocalization:
                        --spiderLocation;
                        break;
                }
            }
        }
        #endregion

        /// <summary>
        /// Basically, it's BresenhamsAlgorithm with map visibility check.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public bool CheckVisibility(Position src, Position dest)
        {            
            int x1 = src.X, y1 = src.Y, x2 = dest.X, y2 = dest.Y;
            int delta_x = Math.Abs(x2 - x1) << 1;
            int delta_y = Math.Abs(y2 - y1) << 1;

            // if x1 == x2 or y1 == y2, then it does not matter what we set here
            int ix = x2 > x1 ? 1 : -1;
            int iy = y2 > y1 ? 1 : -1;

            if (delta_x >= delta_y)
            {
                int error = delta_y - (delta_x >> 1); // error may go below zero

                while (x1 != x2)
                {
                    if (error >= 0)
                    {
                        if ((error > 0) || (ix > 0))
                        {
                            y1 += iy;
                            error -= delta_x;
                        }
                    }

                    x1 += ix;
                    error += delta_y;

                    if (this.GetTile(x1, y1).TileType == TileType.Wall)
                        return false;
                }
            }
            else
            {
                int error = delta_x - (delta_y >> 1);

                while (y1 != y2)
                {
                    if (error >= 0)
                    {
                        if ((error > 0) || (iy > 0))
                        {
                            x1 += ix;
                            error -= delta_y;
                        }
                    }

                    y1 += iy;
                    error += delta_x;

                    if (this.GetTile(x1, y1).TileType == TileType.Wall)
                        return false;
                }
            }
            return true; //There's no wall in line-of-sight
        }
    }
}
