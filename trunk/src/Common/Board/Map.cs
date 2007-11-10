using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Yad.Board.Common {
	public static class Map {
		static TileType[,] tiles;
		static bool[,] fogOfWar;
		static short width, height;

		//TODO: da ka¿dego pola zrobiæ oddzieln¹ listê
		//static LinkedList<Building> buildings;
		//static LinkedList<Unit> units;

		public static short Width {
			get { return width; }
		}

		public static short Height {
			get { return height; }
		}

		public static TileType[,] Tiles {
			get { return tiles; }
		}

		public static void LoadMap(String name) {
			string c;
			int len = -1;
			List<byte[]> tempList = new List<byte[]>();
			byte[] tempRow = null;

			StreamReader sr = new StreamReader(name);
			while (!sr.EndOfStream) {
				c = sr.ReadLine();
				if (len == -1)
					len = c.Length;
				else if (c.Length != len)
					throw new Exception("Wrong map format");
				tempRow = new byte[c.Length];
				for (int i = 0; i < c.Length; i++) {
					tempRow[i] = (byte)(c[i] - '0');
				}
				tempList.Add(tempRow);
			}

			width = (short)tempList[0].Length;
			height = (short)tempList.Count;

			tiles = new TileType[width, height];
			for (int y = height - 1; y >= 0; y--) {
				for (int x = 0; x < width; x++) {
					tiles[x, y] = (TileType)tempList[y][x];
				}
			}
		}
	}
}