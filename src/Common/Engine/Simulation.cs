using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using System.Threading;
using System.Runtime.CompilerServices;
using Yad.Log.Common;
using System.Collections;
using Yad.Board.Common;
using Yad.Config.Common;
using Yad.Board;
using Yad.Config;
using System.Windows.Forms;
using Yad.Utilities.Common;
using Yad.UI.Common;
using Yad.Net.Messaging;
using Yad.Net.Common;

namespace Yad.Engine.Common {

	public delegate void SimulationHandler();

	/// <summary>
	/// Try not to modify this class...
	/// </summary>
	public abstract class Simulation {

		#region static members

		/// <summary>
		/// Buffers messages to currentTurn + delta
		/// </summary>
		static int delta = Yad.Properties.Common.Settings.Default.Delta;
		/// <summary>
		/// Turn length in miliseconds
		/// </summary>
		static int turnLength = Yad.Properties.Common.Settings.Default.TurnLength;

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

		//these lists are used by ProcessTurn and are created for 1 player at a time
		List<Building> buildingsProcessed = new List<Building>(), buildingsToProcess;
		List<Unit> UnitsProcessed = new List<Unit>(), unitsToProcess;

		/// <summary>
		/// This is blocking ProcessTurns from processing another turn. Released by DoTurn().
		/// </summary>
		Semaphore nextTurn = new Semaphore(1, 1);

		/// <summary>
		/// SpeedUp length in turns
		/// When server sends MessageTurn it can state that the client needs to speed up a little bit
		/// and ignore standard turn length
		/// </summary>
		int speedUpLength = 0;

		int currentTurn = 0;

		/// <summary>
		/// This thread will process messages from current turn
		/// </summary>
		Thread turnProcessor = null;

		/* slowest player: turn x
		 * fastest player: turn x+delta-1
		 * newest message x+delta-1 + delta
		 * max turns cached: x+delta-1 + delta - x + 1 = 2 * delta
		 * but it doesn't work so we have to add 1 :D :D :D
		 */
		int bufferLength = 2 * delta + 1;
		/// <summary>
		/// This table holds turns' messages
		/// </summary>
		List<GameMessage>[] turns;

		bool fastTurnProcessing = true;

		#endregion

		#region protected members

		/// <summary>
		/// (short id, Player player)
		/// </summary>
		protected Dictionary<short, Player> players;


		protected Dictionary<int, Unit> sandworms;

		protected Map _map;

		//animations

		#endregion

		#region constructor
		public Simulation(Map map, bool useFastTurnProcessing) {
			this._map = map;

			this.players = new Dictionary<short, Player>();

			this.fastTurnProcessing = useFastTurnProcessing;
			turns = new List<GameMessage>[bufferLength];
			this.turnProcessor = new Thread(new ThreadStart(ProcessTurns));
			this.turnProcessor.IsBackground = true;

			for (int i = 0; i < bufferLength; i++) {
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

		int turnAsk;
		private void ProcessTurns() {
			List<GameMessage> messages;
			List<GameMessage>.Enumerator messagesEnum;
			while (true) {
				InfoLog.WriteInfo("Waiting for new turn", EPrefix.SimulationInfo);
				nextTurn.WaitOne(); //wait for MessageTurn
				InfoLog.WriteInfo("Next turn", EPrefix.SimulationInfo);

				turnAsk = Environment.TickCount;
				if (onTurnBegin != null) {
					onTurnBegin();
				}

				int turnStart = Environment.TickCount;

				messages = currentMessages;
				messagesEnum = messages.GetEnumerator();
				while (messagesEnum.MoveNext()) {
					GameMessage gm = messagesEnum.Current;
					if (gm.Type == MessageType.CreateUnit) {
						this.onMessageCreate((CreateUnitMessage)gm);
					} else if (gm.Type == MessageType.Build) {
						this.OnMessageBuild((BuildMessage)gm);
					} else if (gm.Type == MessageType.Move) {
						this.onMessageMove((MoveMessage)gm);
					} else if (gm.Type == MessageType.Attack) {
						this.onMessageAttack((AttackMessage)gm);
					} else if (gm.Type == MessageType.Destroy) {
						this.onMessageDestroy((DestroyMessage)gm);
					} else if (gm.Type == MessageType.Harvest) {
						this.onMessageHarvest((HarvestMessage)gm);
					} else if (gm.Type == MessageType.DeployMCV) {
						this.onMessageDeployMCV((GMDeployMCV)gm);
                    } else if (gm.Type == MessageType.BuildUnitMessage){
                        this.onMessageBuildUnit((BuildUnitMessage)gm);
					} else {
						throw new NotImplementedException("This message type is not supported! Refer to Simulation.cs");
					}
				}

				//process all units & building & animations

				Dictionary<short, Player>.Enumerator playersEnumerator = players.GetEnumerator();


				while (playersEnumerator.MoveNext()) {
					Player p = playersEnumerator.Current.Value;

					//get copy of buildings' list
					buildingsToProcess = p.GetAllBuildings();
					//while there are unprocessed buildings
					while (buildingsToProcess.Count != 0) {
						Building b = buildingsToProcess[0];
						buildingsToProcess.RemoveAt(0);
						handleBuilding(b);
					}

					unitsToProcess = p.GetAllUnits();
					unitsToProcess.AddRange(new List<Unit>(sandworms.Values));
					while (unitsToProcess.Count != 0) {
						Unit u = unitsToProcess[0];
						unitsToProcess.RemoveAt(0);
						handleUnit(u);
					}
				}
				//this.fastTurnProcessing = true;
				int remainingTime = Simulation.turnLength - (Environment.TickCount - turnStart);
				if (!this.fastTurnProcessing) { //in server - just do turn, don't wait

					if (SpeedUp) {
						--speedUpLength;
						remainingTime /= 3;
					}
					if (remainingTime > 0) {
						Thread.Sleep(remainingTime);
					}
				}

				//InfoLog.WriteInfo((Environment.TickCount - turnStart).ToString(), EPrefix.SimulationInfo);

				InfoLog.WriteInfo("OnTurnEnd begin", EPrefix.SimulationInfo);
				if (this.onTurnEnd != null) {
					this.onTurnEnd();
				}
				InfoLog.WriteInfo("OnTurnEnd end", EPrefix.SimulationInfo);
			}
		}

		/// <summary>
		/// Handles unit - animation, movement, etc. If you need to destroy another unit or building
		/// from within this function then use destroyX() functions.
		/// </summary>
		/// <param name="u"></param>
		protected abstract void handleUnit(Unit u);

		/// <summary>
		/// Handles building. If you need to destroy another unit or building
		/// from within this function then use destroyX() functions.
		/// </summary>
		/// <param name="b"></param>
		//any ideas how?
		protected abstract void handleBuilding(Building b);

        /// <summary>
        /// this should only be used from within handleX() functions
        /// </summary>
        /// <param name="u"></param>
        protected void destroyUnit(Unit u) {
            u.Destroy();
            this.unitsToProcess.Remove(u);
            this.UnitsProcessed.Remove(u);
            this.players[u.ObjectID.PlayerID].RemoveUnit(u);
            this._map.Units[u.Position.X, u.Position.Y].Remove(u);

        }

        /// <summary>
        /// this should only be used from within handleX() functions
        /// </summary>
        /// <param name="u"></param>
        protected void destroyBuilding(Building b) {
            b.Destroy();
            this.buildingsToProcess.Remove(b);
            this.buildingsProcessed.Remove(b);
            this.players[b.ObjectID.PlayerID].RemoveBuilding(b);
            for (int i = b.Position.X; i < b.BuildingData.Size.X + b.Position.X; ++i) {
                for (int j = b.Position.Y; j < b.Position.Y + b.BuildingData.Size.Y; ++j) {
                    this._map.Buildings[i, j].Remove(b);
                }
            }

        }

        /// <summary>
        /// handles attack - counts damage, destroy unit
        /// </summary>
        /// <param name="attacked"></param>
        /// <param name="attacker"></param>
        public abstract void handleAttackUnit(Unit attacked, Unit attacker);
        public abstract void handleAttackUnit(Unit attacked, Unit attacker, short count);
        /// <summary>
        /// handles attack - counts damage, destroy building
        /// </summary>
        /// <param name="attacked"></param>
        /// <param name="attacker"></param>
        public abstract void handleAttackBuilding(Building attacked, Unit attacker);
        public abstract void handleAttackBuilding(Building attacked, Unit attacker, short count);

        public abstract void handleAttackBuilding(Building attacked, Building attacker);
        public abstract void handleAttackBuilding(Building attacked, Building attacker, short count);

        public abstract void handleAttackUnit(Unit attacked, Building attacker);
        public abstract void handleAttackUnit(Unit attacked, Building attacker, short count);
		#endregion

		#region protected methods
		protected abstract void OnMessageBuild(BuildMessage bm);
		protected abstract void onMessageMove(MoveMessage gm);
		protected abstract void onMessageAttack(AttackMessage am);
		protected abstract void onMessageDestroy(DestroyMessage dm);
		protected abstract void onMessageHarvest(HarvestMessage hm);
		protected abstract void onMessageCreate(CreateUnitMessage cum);
		protected abstract void onMessageDeployMCV(GMDeployMCV dmcv);
        protected abstract void onMessageBuildUnit(BuildUnitMessage msg);
		public abstract void ClearFogOfWar(Building b);
		public abstract void ClearFogOfWar(Unit u);

		protected abstract void onInvalidMove(Unit unit);
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
			//InfoLog.WriteInfo("queue new turn", EPrefix.SimulationInfo);
			lock (turns.SyncRoot) {
				currentMessages = ShiftTurns();
				currentTurn++;
				try {
					nextTurn.Release();
				} catch (SemaphoreFullException) {
					currentTurn--;
					MessageBoxEx.Show("DoTurn called to early! Previous turn not yet completed! This can lead to certain problems.");
				}
			}
			int ms = (Environment.TickCount - turnAsk);
			InfoLog.WriteInfo("Ask-Permit length: " +  ms.ToString(), EPrefix.SimulationInfo);
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
			get { return this.speedUpLength > 0; }
			set {
				if (value) {
					this.speedUpLength = delta;
				} else {
					this.speedUpLength = 0;
				}
			}
		}

		public Map Map {
			get { return this._map; }
		}

		public ICollection<Player> GetPlayers() {
			return players.Values;
		}

		public Dictionary<short, Player> Players {
			get { return this.players; }
		}

       
		#endregion
	}
}
