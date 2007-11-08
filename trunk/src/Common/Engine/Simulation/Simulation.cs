using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using System.Threading;

namespace Yad.Engine.Simulation.Common
{
    public abstract class Simulation
    {
        public static int delta = 3;

        int turnNumber = 0;

		/* slowest player: turn x
		 * fastest player: turn x+delta-1
		 * newest message x+delta-1 + delta
		 * max turns cached: x+delta-1 + delta - x + 1 = 2 * delta
		 */
		List<GameMessage>[] turns = new List<GameMessage>[2 * delta];

		protected IOnGameMessage onMessageBuild;
		protected IOnGameMessage onMessageMove;
		protected IOnGameMessage onMessageAttack;
		protected IOnGameMessage onMessageDestroy;
		protected IOnGameMessage onMessageHarvest;
		protected IOnGameMessage onMessageCreate;

		private List<GameMessage> GetCurrentTurn() {
			return turns[0];
		}

		public void AddGameMessage(GameMessage gameMessage) {
			
		}

		public void DoTurn() {
			turnNumber++;
			List<GameMessage> currentTurn = this.GetCurrentTurn();
			ShiftTurns();


			//simulate in new thread using currentTurn
		}

		private void ShiftTurns() {
			int i;
			for (i = 0; i < 2 * delta - 1; i++) {
				turns[i] = turns[i + 1];
			}
			turns[i] = null;
		}

		public int Turn {
			get { return turnNumber; }
		}
    }
}
