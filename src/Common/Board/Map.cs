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
		int[,] spice;

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

		public LinkedList<Building>[,] Buildings {
			get {
				return buildings;
			}
		}

		public bool[,] FogOfWar {
			get {
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

		public int[,] Spice {
			get { return spice; }
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

			FileStream fs = File.Open(name, FileMode.Open);
			BinaryFormatter bf = new BinaryFormatter();
			List<Point> lp = (List<Point>)bf.Deserialize(fs);
			MapData md = (MapData)bf.Deserialize(fs);

			width = (short)md.Width;
			height = (short)md.Height;
			tiles = new TileType[width, height];
			spice = new int[width, height];
			fogOfWar = new bool[width, height];
			slabs = new bool[width, height];
			buildings = new LinkedList<Building>[width, height];
			units = new LinkedList<Unit>[width, height];

			for (int y = 0; y < height; ++y) {
				for (int x = 0; x < width; ++x) {
					TileData td= md[x][height - 1 - y];
					tiles[x, y] = td.Type;
					spice[x, y] = td.SpiceNo;

					fogOfWar[x, y] = true;
					slabs[x, y] = false;
					buildings[x, y] = new LinkedList<Building>();
					units[x, y] = new LinkedList<Unit>();
				}
			}
		}
	}
}