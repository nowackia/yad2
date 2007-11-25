using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Client {
	public static class ClientPlayerInfo {
		private static GameInfo gameInfo = null;
		private static PlayerInfo playerInfo = new PlayerInfo();
		private static Players enemies = new Players();

		public static short SenderId {
			get { return playerInfo.Id; }
			set { playerInfo.Id = value; }
		}

		public static PlayerInfo Player {
			get { return playerInfo; }
            set { playerInfo = value; }
		}

		public static string ChatPrefix {
			get { return "[" + playerInfo.Name + "] : "; }
		}

		public static GameInfo GameInfo {
			get { return gameInfo; }
			set {
				gameInfo = value;
			}
		}

		public static Players Enemies {
			get { return enemies; }
		}

		public static List<PlayerInfo> GetAllPlayers() {
			List<PlayerInfo> result = new List<PlayerInfo>();
			result.Add(playerInfo);
			foreach (PlayerInfo pi in enemies.GetPlayerInfos()) {
				result.Add(pi);
			}
			return result;
		}
	}
}
