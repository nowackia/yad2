using System;
using System.Collections.Generic;
using System.Text;
using Yad.Properties.Common;
using Yad.Board.Common;
using System.Collections;

namespace Yad.Engine.Common {
	class Player {
		/// <summary>
		/// Player id assigned by server.
		/// </summary>
		short playerID = -1;

		/// <summary>
		/// Player name assigned by player.
		/// </summary>
		string playerName;

		int objectID = 0;

		public Dictionary<int, Building> buildings = new Dictionary<int, Building>();
		public Dictionary<int, Unit> units = new Dictionary<int, Unit>();

		public Player(short id) {
			this.playerID = id;
		}

		public short ID {
			get { return playerID; }
			set { playerID = value; }
		}		

		public string Name {
			get { return this.playerName; }
			set { this.playerName = value; }
		}
		
		/// <summary>
		/// Used for generating id's for player-created objects (units/buildings)
		/// </summary>
		public int GenerateObjectID() {
			return objectID++;
		}

		public void AddUnit(Unit u) {
			units.Add(u.ObjectID, u);
		}

		public void RemoveUnit(int objectID) {
			units.Remove(objectID);
		}

		public void AddBuilding(Building b) {
			buildings.Add(b.ObjectID, b);
		}

		public void RemoveBuilding(int objectID) {
			buildings.Remove(objectID);
		}
	}
}
