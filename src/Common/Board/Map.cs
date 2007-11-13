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
		static LinkedList<Building>[,] buildings;
		static LinkedList<Unit>[,] units;

		public static short Width {
			get { return width; }
		}

		public static short Height {
			get { return height; }
		}

		public static LinkedList<Building>[,] Buildings
		{
			get
			{
				return buildings;
			}
		}

		public static LinkedList<Unit>[,] Units
		{
			get
			{
				return units;
			}
		}

		public static TileType[,] Tiles {
			get { return tiles; }
		}

		public static bool CheckConststency()
		{
			for (int i = 1; i < width - 1; i++)
				for (int j = 1; j < height - 1; j++)
					if ((tiles[i, j] == TileType.Mountain && (!CheckField(i - 1, j) || !CheckField(i + 1, j) || !CheckField(i, j - 1) || !CheckField(i, j + 1))))
						return false;
			return true;
		}

		private static bool CheckField(int x, int y)
		{
			if (tiles[x, y] == TileType.Mountain || tiles[x, y] == TileType.Rock)
				return true;
			else
				return false;
		}

		public static void LoadMap(String name)
		{
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

			fogOfWar = new bool[width, height];
			buildings = new LinkedList<Building>[width, height];
			units = new LinkedList<Unit>[width, height];
			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
				{
					fogOfWar[i, j] = true;
					buildings[i, j] = new LinkedList<Building>();
					units[i, j] = new LinkedList<Unit>();
				}

		}
	}
}