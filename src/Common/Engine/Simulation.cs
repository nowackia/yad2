using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using System.Threading;
using System.Runtime.CompilerServices;
using Yad.Log.Common;

namespace Yad.Engine.Common {

	public delegate void SimulationHandler();

	public abstract class Simulation {
		/*
		/// <summary>
		/// Used for pausing 
		/// </summary>
		Semaphore pauseSem = new Semaphore(1, 1);
		bool pause = false;
		*/
		public event SimulationHandler onTurnEnd;
		public event SimulationHandler onTurnBegin;

		/// <summary>
		/// this messages are processed by ProcessTurns
		/// </summary>
		List<GameMessage> currentMessages = new List<GameMessage>();

		/// <summary>
		/// This is blocking ProcessTurns from processing another turn. Released by DoTurn().
		/// </summary>
		Semaphore allowTurn = new Semaphore(1, 1);

		/// <summary>
		/// When server sends MessageTurn it can state that the client needs to speed up a little bit ;p
		/// </summary>
		bool speedUp = false;
		Object speedUpLocker = new Object();

		public static int delta = 3;
		public static int turnLength = 200; //ms
		private static int transmissionDelay = 5; //ms

		public int Delta {
			get { return delta; }
		}

		/// <summary>
		/// Current turnNumber
		/// </summary>
		int currentTurn = 0;
		int turnsToSpeed = delta;

		Thread turnProcessor = null;

		/* slowest player: turn x
		 * fastest player: turn x+delta-1
		 * newest message x+delta-1 + delta
		 * max turns cached: x+delta-1 + delta - x + 1 = 2 * delta
		 */
		List<GameMessage>[] turns = new List<GameMessage>[2 * delta];

		public Simulation() {
			this.turnProcessor = new Thread(new ThreadStart(ProcessTurns));
			this.turnProcessor.IsBackground = true;

			for (int i = 0; i < 2 * delta; i++) {
				this.turns[i] = new List<GameMessage>();
			}

		}

		public void AddGameMessage(GameMessage gameMessage) {
			InfoLog.WriteInfo("Waiting to add message", EPrefix.SimulationInfo);
			lock (turns.SyncRoot) {
				InfoLog.WriteInfo("Adding message: " + gameMessage.Type.ToString(), EPrefix.SimulationInfo);
				this.turns[gameMessage.IdTurn - (this.CurrentTurn + 1)].Add(gameMessage);
			}
		}

		/// <summary>
		/// This function should be called ONLY after ProcessTurns completes one turn
		/// and sends a message to the server asking for a next turn
		/// </summary>
		public void DoTurn() {
			InfoLog.WriteInfo("queue new turn", EPrefix.SimulationInfo);
			lock (turns.SyncRoot) {
				currentMessages = ShiftTurns();
				currentTurn++;
				allowTurn.Release();
			}
		}

		private List<GameMessage> ShiftTurns() {
			List<GameMessage> res = turns[0];
			int i;
			for (i = 0; i < turns.Length - 1; i++) {
				turns[i] = turns[i + 1];
			}
			turns[i] = new List<GameMessage>();
			return res;
		}

		private void ProcessTurns() {
			List<GameMessage> messages;
			List<GameMessage>.Enumerator messagesEnum;
			while (true) {
				allowTurn.WaitOne(); //wait for MessageTurn
				int turnStart = Environment.TickCount;

				messages = currentMessages;
				messagesEnum = messages.GetEnumerator();
				while (messagesEnum.MoveNext()) {
					GameMessage gm = messagesEnum.Current;
					if (gm is CreateUnitMessage) {
						this.onMessageCreate((CreateUnitMessage)gm);
					} else if (gm is BuildMessage) {
						this.OnMessageBuild((BuildMessage)gm);
					} else if (gm is MoveMessage) {
						this.onMessageMove((MoveMessage)gm);
					} else if (gm is AttackMessage) {
						this.onMessageAttack((AttackMessage)gm);
					} else if (gm is DestroyMessage) {
						this.onMessageDestroy((DestroyMessage)gm);
					} else if (gm is HarvestMessage) {
						this.onMessageHarvest((HarvestMessage)gm);
					} else {
						throw new NotImplementedException("This message type is not supported! Refer to Simulation.cs");
					}
				}

				if (!this.SpeedUp) {
					int remainingTime = Simulation.turnLength - (Environment.TickCount - turnStart) - transmissionDelay;
					if (remainingTime > 0)
						Thread.Sleep(remainingTime);
				}

				this.turnsToSpeed--;
				if (turnsToSpeed == 0) {
					SpeedUp = false;
				}

				if (this.onTurnEnd != null) {
					this.onTurnEnd();
				}

				//TODO:
				//Send server a message asking for a new turn
			}
		}

		public int CurrentTurn {
			get {
				lock (turns.SyncRoot) {
					return currentTurn;
				}
			}
		}

		public bool SpeedUp {
			get {
				lock (speedUpLocker) {
					return this.speedUp;
				}
			}
			set {
				lock (speedUpLocker) {
					speedUp = value;
				}
			}
		}

		public void StartSimulation() {
			turnProcessor.Start();
		}

		public void AbortSimulation() {
			this.turnProcessor.Abort();
		}

		protected abstract void OnMessageBuild(BuildMessage bm);
		protected abstract void onMessageMove(MoveMessage gm);
		protected abstract void onMessageAttack(AttackMessage am);
		protected abstract void onMessageDestroy(DestroyMessage dm);
		protected abstract void onMessageHarvest(HarvestMessage hm);
		protected abstract void onMessageCreate(CreateUnitMessage cum);
	}
}
