using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace Client.Board {
	static class Map {
		static TileType[,] tiles;
		static bool[,] fogOfWar;
		static int width, height;

		//TODO: da ka¿dego pola zrobiæ oddzieln¹ listê
		//static LinkedList<Building> buildings;
		//static LinkedList<Unit> units;

		public static int Width {
			get { return width; }
		}

		public static int Height {
			get { return height; }
		}

		public static TileType[,] Tiles {
			get { return tiles; }
		}

		public static void LoadMap(String name) {
			string c;
			List<int[]> tempList = new List<int[]>();
			int[] tempRow = null;

			StreamReader sr = new StreamReader(name);
			while (!sr.EndOfStream) {
				c = sr.ReadLine();
				tempRow = new int[c.Length];
				for (int i = 0; i < c.Length; i++) {
					tempRow[i] = c[i] - '0';
				}
				tempList.Add(tempRow);
			}

			width = tempList[0].Length;
			height = tempList.Count;

			tiles = new TileType[width, height];
			for (int y = height - 1; y >= 0; y--) {
				for (int x = 0; x < width; x++) {
					tiles[x, y] = (TileType)tempList[y][x];
				}
			}
		}
	}
}