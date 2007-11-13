using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using System.Threading;
using System.Runtime.CompilerServices;
using Yad.Log.Common;
using System.Collections;

namespace Yad.Engine.Common {

	public delegate void SimulationHandler();

	public abstract class Simulation {

		#region static members

		/// <summary>
		/// Buffers messages to currentTurn + delta
		/// </summary>
		static int delta = 3;
		/// <summary>
		/// Turn length in miliseconds
		/// </summary>
		static int turnLength = 200;
		/// <summary>
		/// Transmission delay in miliseconds
		/// </summary>
		static int transmissionDelay = 5;

		#endregion

		#region events
		public event SimulationHandler onTurnEnd;
		public event SimulationHandler onTurnBegin;
		#endregion

		#region private members

		/// <summary>
		/// this messages are processed by ProcessTurns
		/// </summary>
		List<GameMessage> currentMessages = new List<GameMessage>();

		/// <summary>
		/// This is blocking ProcessTurns from processing another turn. Released by DoTurn().
		/// </summary>
		Semaphore nextTurn = new Semaphore(1, 1);

		/// <summary>
		/// When server sends MessageTurn it can state that the client needs to speed up a little bit
		/// and ignore standard turn length
		/// </summary>
		bool speedUp = false;

		/// <summary>
		/// SpeedUp length in turns
		/// </summary>
		int speedUpLength = delta;

		int currentTurn = 0;

		/// <summary>
		/// This thread will process messages from current turn
		/// </summary>
		Thread turnProcessor = null;

		/* slowest player: turn x
		 * fastest player: turn x+delta-1
		 * newest message x+delta-1 + delta
		 * max turns cached: x+delta-1 + delta - x + 1 = 2 * delta
		 */
		/// <summary>
		/// This table holds turns' messages
		/// </summary>
		List<GameMessage>[] turns = new List<GameMessage>[2 * delta];

		bool fastTurnProcessing = true;

		#endregion

		#region protected members

		/// <summary>
		/// (short id, Player player)
		/// </summary>
		protected Dictionary<short, Player> players = new Dictionary<short, Player>();
		

		//animations

		#endregion
		#region constructor
		public Simulation(bool useFastTurnProcessing) {
			this.fastTurnProcessing = useFastTurnProcessing;
			this.turnProcessor = new Thread(new ThreadStart(ProcessTurns));
			this.turnProcessor.IsBackground = true;

			for (int i = 0; i < 2 * delta; i++) {
				this.turns[i] = new List<GameMessage>();
			}

		}
		#endregion

		#region private methods
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
				nextTurn.WaitOne(); //wait for MessageTurn
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

				if (!this.fastTurnProcessing) {
					if (!this.SpeedUp) {
						int remainingTime = Simulation.turnLength - (Environment.TickCount - turnStart) - transmissionDelay;
						//InfoLog.WriteInfo(remainingTime.ToString(), EPrefix.SimulationInfo);
						if (remainingTime > 0)
							Thread.Sleep(remainingTime);
					} else {
						speedUpLength--;
						if (speedUpLength == 0) {
							SpeedUp = false;
						}
					}
				}

				if (this.onTurnEnd != null) {
					this.onTurnEnd();
				}				
			}
		}
		#endregion

		#region protected methods
		protected abstract void OnMessageBuild(BuildMessage bm);
		protected abstract void onMessageMove(MoveMessage gm);
		protected abstract void onMessageAttack(AttackMessage am);
		protected abstract void onMessageDestroy(DestroyMessage dm);
		protected abstract void onMessageHarvest(HarvestMessage hm);
		protected abstract void onMessageCreate(CreateUnitMessage cum);
		#endregion

		#region public methods
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
				nextTurn.Release();
			}
		}

		public void StartSimulation() {
			turnProcessor.Start();
		}

		public void AbortSimulation() {
			this.turnProcessor.Abort();
		}


		public int CurrentTurn {
			get {
				lock (turns.SyncRoot) {
					return currentTurn;
				}
			}
		}

		public int Delta {
			get { return delta; }
		}

		public bool SpeedUp {
			get {
				return this.speedUp;
			}
			set {
				speedUp = value;
				if (speedUp) {
					this.speedUpLength = delta;
				}
			}
		}
		#endregion
	}
}