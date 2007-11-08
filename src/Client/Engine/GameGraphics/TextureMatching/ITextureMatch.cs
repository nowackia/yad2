using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board.Common;

namespace Client.Engine.GameGraphics.TextureMatching
{
	interface ITextureMatch
	{
		public static int match(TileType upper, TileType lower, TileType left, TileType right);
	}
}
