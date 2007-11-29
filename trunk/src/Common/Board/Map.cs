using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace Yad.Board.Common {
	public class Map {
		TileType[,] _tiles;
		int[,] _spice;

		short _width, _height;

		//TODO: da ka¿dego pola zrobiæ oddzieln¹ listê
		LinkedList<Building>[,] _buildings;
		LinkedList<Unit>[,] _units;

		public short Width {
			get { return _width; }
		}

		public short Height {
			get { return _height; }
		}

		public LinkedList<Building>[,] Buildings {
			get {
				return _buildings;
			}
		}

		public LinkedList<Unit>[,] Units {
			get {
				return _units;
			}
		}

		public TileType[,] Tiles {
			get { return _tiles; }
		}

		public int[,] Spice {
			get { return _spice; }
		}

		public bool CheckConststency() {
			for (int i = 1; i < _width - 1; i++)
				for (int j = 1; j < _height - 1; j++)
					if ((_tiles[i, j] == TileType.Mountain && (!CheckField(i - 1, j) || !CheckField(i + 1, j) || !CheckField(i, j - 1) || !CheckField(i, j + 1))))
						return false;
			return true;
		}

		private bool CheckField(int x, int y) {
			if (_tiles[x, y] == TileType.Mountain || _tiles[x, y] == TileType.Rock)
				return true;
			else
				return false;
		}

		public void LoadMap(String name) {

			FileStream fs = File.Open(name, FileMode.Open);
			BinaryFormatter bf = new BinaryFormatter();
			List<Point> lp = (List<Point>)bf.Deserialize(fs);
			MapData md = (MapData)bf.Deserialize(fs);

			_width = (short)md.Width;
			_height = (short)md.Height;
			_tiles = new TileType[_width, _height];
			_spice = new int[_width, _height];
			//fogOfWar = new bool[width, height];
			//slabs = new bool[width, height];
			_buildings = new LinkedList<Building>[_width, _height];
			_units = new LinkedList<Unit>[_width, _height];

			for (int y = 0; y < _height; ++y) {
				for (int x = 0; x < _width; ++x) {
					TileData td = md[x][_height - 1 - y];
					_tiles[x, y] = td.Type;
					_spice[x, y] = td.SpiceNo;

					//fogOfWar[x, y] = true;
					//slabs[x, y] = false;
					_buildings[x, y] = new LinkedList<Building>();
					_units[x, y] = new LinkedList<Unit>();
				}
			}
		}

		public bool CheckSpace(Position pos, short width, short height) {
			for (int x = pos.X; x < pos.X + width; x++) {
				for (int y = pos.Y; y < pos.Y + height; y++) {
					if (_buildings[x, y].Count != 0 || _units[x, y].Count != 0) {
						return false;
					}
				}
			}
			return true;
		}
	}
}