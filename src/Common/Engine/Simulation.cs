using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using System.Threading;
using System.Runtime.CompilerServices;
using Yad.Log.Common;

namespace Yad.Engine.Common {
	public abstract class Simulation {
		/*
		/// <summary>
		/// Used for pausing 
		/// </summary>
		Semaphore pauseSem = new Semaphore(1, 1);
		bool pause = false;
		*/

		/// <summary>
		/// Increased on DoTurn();
		/// 2 * delta + 1 - maybe less?
		/// </summary>
		Semaphore turnsToDo = new Semaphore(0, 2 * delta + 1);

		/// <summary>
		/// When server sends MessageTurn it can state that the client needs to speed up a little bit ;p
		/// </summary>
		bool speedUp = false;
		Object speedUpLocker = new Object();

		public static int delta = 3;

		/// <summary>
		/// Current turnNumber
		/// </summary>
		int currentTurn = 0;

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
		}

		public void AddGameMessage(GameMessage gameMessage) {
			lock (turns.SyncRoot) {
				this.turns[gameMessage.IdTurn - this.CurrentTurn].Add(gameMessage);
			}
		}

		public void DoTurn() {
			InfoLog.WriteInfo("queue new turn", EPrefix.SimulationInfo);
			try {
				turnsToDo.Release();
			} catch (SemaphoreFullException) {
				InfoLog.WriteInfo("too many DoTurn()!", EPrefix.SimulationInfo);
			}
		}

		private void ShiftTurns() {
			int i;
			for (i = 0; i < turns.Length - 1; i++) {
				turns[i] = turns[i + 1];
			}
			turns[i] = new List<GameMessage>();
		}

		private void ProcessTurns() {
			List<GameMessage> messages;
			List<GameMessage>.Enumerator messagesEnum;
			while (true) {
				turnsToDo.WaitOne(); //wait for MessageTurn

				//get current turn
				lock (turns.SyncRoot) {
					messages = this.turns[0];
					ShiftTurns();
					currentTurn++;
				}

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
			}
		}

		public int CurrentTurn {
			get { return currentTurn; }
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

		/*
		public void PauseSimulation() {
			InfoLog.WriteInfo("Pausing...", EPrefix.SimulationInfo);
			lock (pauseSem) {
				pause = true;
			}
		}

		public void ResumeSimulation() {
			InfoLog.WriteInfo("Resuming...", EPrefix.SimulationInfo);
			lock (pauseSem) {
				if (pause == false) return;

				pause = false;
					pauseSem.Release();
			}
		}
		 */

		protected abstract void OnMessageBuild(BuildMessage bm);
		protected abstract void onMessageMove(MoveMessage gm);
		protected abstract void onMessageAttack(AttackMessage am);
		protected abstract void onMessageDestroy(DestroyMessage dm);
		protected abstract void onMessageHarvest(HarvestMessage hm);
		protected abstract void onMessageCreate(CreateUnitMessage cum);
	}
}
