using System;
using System.Collections.Generic;
using System.Drawing;

namespace AntHill.NET
{
    public enum TileType { Wall, Outdoor, Indoor };

    public class Tile
    {
        private Position position;
        public LIList<Message> messages; /* List of references to messages active on this tile. */
        private TileType tileType;

        public Tile(TileType ttype, Position pos)
        {
            position = new Position(pos);
            tileType = ttype;
            messages = new LIList<Message>();
        }

        public Position Position
        {
            get { return position; }
        }

        public TileType TileType
        {
            get { return tileType; }
            set { tileType = value; }
        }

        public int GetTexture()
        {
            return AHGraphics.GetTileTexture(this.TileType);
        }
    }
}
