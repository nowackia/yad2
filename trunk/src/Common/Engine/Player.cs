using System;
using System.Collections.Generic;
using System.Text;
using Yad.Properties.Common;
using Yad.Board.Common;
using System.Collections;

namespace Yad.Engine.Common {
	public class Player {
		/// <summary>
		/// Player id assigned by server.
		/// </summary>
		short playerID = -1;

		/// <summary>
		/// Player name assigned by player.
		/// </summary>
		string playerName;

		int objectID = 0;

		//used for fast access to an object
		private Dictionary<int, Building> buildingsDict = new Dictionary<int, Building>();
		private Dictionary<int, Unit> unitsDict = new Dictionary<int, Unit>();

		//used for slow acces to an object :D
		//but pretty useful for a turn processing
		private List<Building> buildings = new List<Building>();
		private List<Unit> units = new List<Unit>();

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
			unitsDict.Add(u.ObjectID, u);
			units.Add(u);
		}

		public void RemoveUnit(Unit u) {
			unitsDict.Remove(u.ObjectID);
			units.Remove(u);
		}

		public void AddBuilding(Building b) {
			buildingsDict.Add(b.ObjectID, b);
			buildings.Add(b);
		}

		public void RemoveBuilding(Building b) {
			buildingsDict.Remove(b.ObjectID);
			buildings.Remove(b);
		}

		/// <summary>
		/// return copy of units list
		/// </summary>
		/// <returns></returns>
		public List<Unit> GetAllUnits() {
			return new List<Unit>(this.units);
		}

		/// <summary>
		/// return copy of buildings list
		/// </summary>
		/// <returns></returns>
		public List<Building> GetAllBuildings() {
			return new List<Building>(this.buildings);
		}
	}
}
