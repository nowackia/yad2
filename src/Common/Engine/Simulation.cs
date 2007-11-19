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

namespace Yad.Engine.Common {

	public delegate void SimulationHandler();

	public abstract class Simulation {

		#region static members

		/// <summary>
		/// Buffers messages to currentTurn + delta
		/// </summary>
		static int delta = Yad.Properties.Common.Settings.Default.Delta;
		/// <summary>
		/// Turn length in miliseconds
		/// </summary>
		const int turnLength = 200;
		/// <summary>
		/// Transmission delay in miliseconds
		/// </summary>
		const int transmissionDelay = 30;

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

		protected Map map;

		protected GameSettingsWrapper gameSettingsWrapper;

		protected Player currentPlayer;

		//animations

		#endregion

		#region constructor
		public Simulation(GameSettingsWrapper settingsWrapper, Map map, Player currPlayer, bool useFastTurnProcessing) {
			this.gameSettingsWrapper = settingsWrapper;
			this.map = map;
			this.currentPlayer = currPlayer;
			this.fastTurnProcessing = useFastTurnProcessing;
			this.turnProcessor = new Thread(new ThreadStart(ProcessTurns));
			this.turnProcessor.IsBackground = true;

			for (int i = 0; i < 3 * delta; i++) {
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
				//InfoLog.WriteInfo("Next turn", EPrefix.SimulationInfo);

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
					} else {
						throw new NotImplementedException("This message type is not supported! Refer to Simulation.cs");
					}
				}

				//process all units & building & animations

				//TODO: Does dictionary and foreach/enumerator imply proper order??

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
					while (unitsToProcess.Count != 0) {
						Unit u = unitsToProcess[0];
						unitsToProcess.RemoveAt(0);
						handleUnit(u);
					}
				}
				//this.fastTurnProcessing = true;
				//2 * transmissionDelay - to server & back
				int remainingTime = Simulation.turnLength - (Environment.TickCount - turnStart) - 2 * transmissionDelay;
				if (!this.fastTurnProcessing) { //in server - just do turn, don't wait

					if (!this.SpeedUp) { // client
						if (remainingTime > 0) {
							Thread.Sleep(remainingTime);
						}
					} else {
						if (--speedUpLength == 0) {
							SpeedUp = false;
						}
					}

				}

				//InfoLog.WriteInfo((Environment.TickCount - turnStart).ToString(), EPrefix.SimulationInfo);

				if (this.onTurnEnd != null) {
					this.onTurnEnd();
				}
			}
		}

		/// <summary>
		/// Handles unit - animation, movement, etc. If you need to destroy another unit or building
		/// from within this function then use destroyX() functions.
		/// </summary>
		/// <param name="u"></param>
		private void handleUnit(Unit u) {
			u.Move();

			/*
			if (!u.Moving && (Randomizer.Next(3)!=0)) {

				Position pos = u.Position;


				//TODO: remove all Randomizer's in the future
				pos.X += (short)(Randomizer.NextShort(3) - 1);
				pos.Y += (short)(Randomizer.NextShort(3) - 1);

				UsefulFunctions.CorrectPosition(ref pos, map.Width, map.Height);

				u.MoveTo(pos);
			}
			 * */
		}

		/// <summary>
		/// Handles building. If you need to destroy another unit or building
		/// from within this function then use destroyX() functions.
		/// </summary>
		/// <param name="b"></param>
		//any ideas how?
		private void handleBuilding(Building b) {

		}

		/// <summary>
		/// this should only be used from within handleX() functions
		/// </summary>
		/// <param name="u"></param>
		private void destroyUnit(Unit u) {
			this.unitsToProcess.Remove(u);
			this.UnitsProcessed.Remove(u);
			this.players[u.PlayerID].RemoveUnit(u);
		}

		/// <summary>
		/// this should only be used from within handleX() functions
		/// </summary>
		/// <param name="u"></param>
		private void destroyBuilding(Building b) {
			this.buildingsToProcess.Remove(b);
			this.buildingsProcessed.Remove(b);
			this.players[b.PlayerID].RemoveBuilding(b);
		}

		#endregion

		#region protected methods
		protected abstract void OnMessageBuild(BuildMessage bm);
		protected abstract void onMessageMove(MoveMessage gm);
		protected abstract void onMessageAttack(AttackMessage am);
		protected abstract void onMessageDestroy(DestroyMessage dm);
		protected abstract void onMessageHarvest(HarvestMessage hm);
		protected abstract void onMessageCreate(CreateUnitMessage cum);

		protected abstract void onInvalidMove(Unit unit);
		#endregion

		#region public methods
		public void AddPlayer(Player p) {
			players.Add(p.ID, p);
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

		public GameSettingsWrapper GameSettingsWrapper {
			get { return this.gameSettingsWrapper; }
		}

		public Map Map {
			get { return this.map; }
		}

		public ICollection<Player> GetPlayers() {
			return players.Values;
		}
		#endregion
	}
}
