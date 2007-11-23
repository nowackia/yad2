using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace Yad.Board.Common {
	public class Map {
		TileType[,] tiles;
		bool[,] fogOfWar;
		bool[,] slabs; // they don't work as normal buildings

		short width, height;

		//TODO: da ka¿dego pola zrobiæ oddzieln¹ listê
		LinkedList<Building>[,] buildings;
		LinkedList<Unit>[,] units;

		public short Width {
			get { return width; }
		}

		public short Height {
			get { return height; }
		}

        private void DrawTile(int x, int y)
        {

        }

		public LinkedList<Building>[,] Buildings {
			get {
				return buildings;
			}
		}

		public bool[,] FogOfWar
		{
			get
			{
				return fogOfWar;
			}
		}


		public LinkedList<Unit>[,] Units {
			get {
				return units;
			}
		}

		public TileType[,] Tiles {
			get { return tiles; }
		}

		public bool CheckConststency() {
			for (int i = 1; i < width - 1; i++)
				for (int j = 1; j < height - 1; j++)
					if ((tiles[i, j] == TileType.Mountain && (!CheckField(i - 1, j) || !CheckField(i + 1, j) || !CheckField(i, j - 1) || !CheckField(i, j + 1))))
						return false;
			return true;
		}

		private bool CheckField(int x, int y) {
			if (tiles[x, y] == TileType.Mountain || tiles[x, y] == TileType.Rock)
				return true;
			else
				return false;
		}

		public void LoadMap(String name) {
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
				tempList.Insert(0, tempRow);
			}

			width = (short)tempList[0].Length;
			height = (short)tempList.Count;
            /*
            FileStream fs = File.Open(name, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            List<Point> lp = (List<Point>)bf.Deserialize(fs);
            MapData md = (MapData)bf.Deserialize(fs);

            width = (short)md.Width;
            height = (short)md.Height;
			tiles = new TileType[width, height];
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
                    tiles[x, y] = md[x][y].Type;
				}
			}*/

            tiles = new TileType[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, y] = (TileType)tempList[y][x];
                }
            }

			fogOfWar = new bool[width, height];
			slabs = new bool[width, height];

			buildings = new LinkedList<Building>[width, height];
			units = new LinkedList<Unit>[width, height];

			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					fogOfWar[i, j] = true;
					slabs[i, j] = false;
					buildings[i, j] = new LinkedList<Building>();
					units[i, j] = new LinkedList<Unit>();
				}
			}
		}
	}
}