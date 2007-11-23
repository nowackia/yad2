using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config;
using Yad.Config.Common;

namespace Yad.Board.Common {
	public class Building : BoardObject {
		private BuildingData _buildingData;
		private int _currentHealth;

		Map _map;
		bool _alreadyOnMap = false;

		public Building(ObjectID id, BuildingData bd, Map map, Position pos)
			: base(id, BoardObjectClass.Building, pos) {
			this._buildingData = bd;
			this._map = map;
		}

		public short TypeID {
			get { return this._buildingData.TypeID; }
		}

		public int Health {
			get {
				return _currentHealth;
			}
		}

		public BuildingData BuildingData {
			get { return this._buildingData; }
		}

		public short Width {
			get { return this._buildingData.Size.X; }
		}

		public short Height {
			get { return this._buildingData.Size.Y; }
		}

		public bool PlaceOnMap() {
			if (!_alreadyOnMap) {
				for (int x = 0; x < this.Width; x++) {
					for (int y = 0; y < this.Height; y++) {
						this._map.Buildings[x + Position.X, y + Position.Y].AddLast(this);
					}
				}
				return true;
			}
			return false;
		}

		public void ClearFogOfWar() {
			int viewRange = _buildingData.ViewRange;

			for (int x = -viewRange + Position.X; x <= viewRange + Position.X; x++) {
				for (int y = -viewRange + Position.Y; y <= viewRange + Position.Y; y++) {
					if (x < 0 || y < 0 || x > _map.Width - 1 || y > _map.Height - 1) {
						continue;
					}
					_map.FogOfWar[x, y] = false;
				}
			}
		}
	}
}
