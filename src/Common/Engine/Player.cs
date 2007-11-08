using System;
using System.Collections.Generic;
using System.Text;
using Yad.Properties.Common;

namespace Yad.Engine.Common {
	class Player {
		int objectID = 0;

		/// <summary>
		/// Player id assigned by server.
		/// </summary>
		int playerID = -1;

		/// <summary>
		/// Player name assigned by player.
		/// </summary>
		string playerName;

		public int ID {
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
	}
}
