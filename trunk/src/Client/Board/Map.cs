using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Board {
	class Map {
		TileType[,] tiles;
		bool[,] fogOfWar;

		//TODO: da ka¿dego pola zrobiæ oddzieln¹ listê
		LinkedList<Building> buildings;
		LinkedList<Unit> units;
		public LinkedList<Building> Buildings {
			get { return buildings; }
		}

		public LinkedList<Unit> Units {
			get { return units; }
		}
		public Map() {
			buildings = new LinkedList<Building>();
			units = new LinkedList<Unit>();
		}

	}
}
