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

using System.IO;
using Yad.Log;

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

		volatile bool _abort;

		/// <summary>
		/// this messages are processed by ProcessTurns
		/// </summary>
		List<GameMessage> _currentMessages = new List<GameMessage>();

		// these lists are used by ProcessTurn and are created for 1 player at a time
		List<Building> _buildingsProcessed = new List<Building>(), buildingsToProcess;
		List<Unit> _unitsProcessed = new List<Unit>(), unitsToProcess, sandwormToProcess;

		/// <summary>
		/// This is blocking ProcessTurns from processing another turn. Released by DoTurn().
		/// </summary>
		Semaphore _nextTurnSemaphore = new Semaphore(1, 1);

		/// <summary>
		/// SpeedUp length in turns
		/// When server sends MessageTurn it can state that the client needs to speed up a little bit
		/// and ignore standard turn length
		/// </summary>
		int _speedUpLength = 0;

		int _currentTurn = 0;

		/// <summary>
		/// This thread will process messages from current turn
		/// </summary>
		Thread _turnProcessor = null;

		/* slowest player: turn x
		 * fastest player: turn x+delta-1
		 * newest message x+delta-1 + delta
		 * max turns cached: x+delta-1 + delta - x + 1 = 2 * delta
		 * but it doesn't work so we have to add 1 :D :D :D
		 */
		int _bufferLength = 2 * delta + 1;
		/// <summary>
		/// This table holds turns' messages
		/// </summary>
		List<GameMessage>[] _turns;

		//bool _fastTurnProcessing = false;

        private  Player _simulationPlayer;

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
		public Simulation(Map map /*, bool useFastTurnProcessing */) {
			this._map = map;
            _simulationPlayer = new Player(-1, "", -1, System.Drawing.Color.Black, map);

			this.players = new Dictionary<short, Player>();
            players.Add(_simulationPlayer.Id, _simulationPlayer);

			//this._fastTurnProcessing = useFastTurnProcessing;
			_turns = new List<GameMessage>[_bufferLength];

			for (int i = 0; i < _bufferLength; i++) {
				this._turns[i] = new List<GameMessage>();
			}
		}
		#endregion

		#region private methods
		private List<GameMessage> ShiftTurns() {
			List<GameMessage> res = _turns[0];
			int i;
			for (i = 0; i < _turns.Length - 1; i++) {
				_turns[i] = _turns[i + 1];
			}
			_turns[i] = new List<GameMessage>();
			return res;
		}

		int turnAsk;

		private void ProcessTurns() {
			List<GameMessage> messages;
			while (true) {
				InfoLog.WriteInfo("Waiting for new turn", EPrefix.SimulationInfo);
				_nextTurnSemaphore.WaitOne(); //wait for MessageTurn
				if (_abort) {
					Thread.CurrentThread.IsBackground = true;
					this._turnProcessor = null;
					return;
				}
                int cur_turn = this.CurrentTurn;
                InfoLog.WriteInfo("********** TURN " + this.CurrentTurn + " BEGIN **********", EPrefix.Test);

				messages = _currentMessages;

				InfoLog.WriteInfo("Turn: " + CurrentTurn.ToString(), LogFiles.ProcessMsgLog);

				turnAsk = Environment.TickCount;
				if (onTurnBegin != null) {
					onTurnBegin();
				}

				int turnStart = Environment.TickCount;

				foreach (GameMessage gm in messages) {

					InfoLog.WriteInfo(gm.ToString(), LogFiles.ProcessMsgLog);

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
                    } else if (gm.Type == MessageType.BuildUnitMessage) {
                        this.onMessageBuildUnit((BuildUnitMessage)gm);
					} else if (gm.Type == MessageType.PlayerDisconnected) {
						this.onMessagePlayerDisconnected((GameNumericMessage)gm);
					} else {
						throw new NotImplementedException("This message type is not supported! Refer to Simulation.cs");
					}
				}

				//process all units & building & animations
				foreach (Player p in players.Values) {
                    List<Ammo> ammos = p.GetAllAmmos();
                    foreach (Ammo a in ammos) {
                        a.DoAI();
                    }
                }
                
				foreach (Player p in players.Values) {
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

                sandwormToProcess = new List<Unit>(sandworms.Values);
                while (sandwormToProcess.Count != 0) {
                    Unit u = sandwormToProcess[0];
                    sandwormToProcess.RemoveAt(0);
                    handleUnit(u);
                }


				//this.fastTurnProcessing = true;
				int remainingTime = Simulation.turnLength - (Environment.TickCount - turnStart);
				//if (!this._fastTurnProcessing) { //in server - just do turn, don't wait

					if (SpeedUp) {
						--_speedUpLength;
						remainingTime /= 3;
					}
					if (remainingTime > 0) {
						Thread.Sleep(remainingTime);
					}
				//}

				//InfoLog.WriteInfo((Environment.TickCount - turnStart).ToString(), EPrefix.SimulationInfo);

				InfoLog.WriteInfo("OnTurnEnd begin", EPrefix.SimulationInfo);
				if (this.onTurnEnd != null) {
					this.onTurnEnd();
				}
                StringBuilder sb = new StringBuilder("");
                foreach (Player p in players.Values)
                    sb.Append(" Player " + p.Id + " : " + p.Credits);
                InfoLog.WriteInfo(sb.ToString(), EPrefix.BMan);
				InfoLog.WriteInfo("OnTurnEnd end", EPrefix.SimulationInfo);
                InfoLog.WriteInfo("********* TURN " + this.CurrentTurn + " END *********", EPrefix.Test);

                recordFullSimulationState(cur_turn);
			}
           // writer.Close();
           // fs.Close();
		}

		protected abstract void onMessagePlayerDisconnected(GameNumericMessage gameNumericMessage);

        private void recordFullSimulationState(int turn)
        {
			InfoLog.Write("\r\nTurn: " + turn, LogFiles.FullSimulationLog);

            foreach(Player player in this.Players.Values)
            {
				InfoLog.Write("Player: " + player.Id.ToString() + " - " + player.Name + ". Team id: " + player.TeamID, LogFiles.FullSimulationLog);
				InfoLog.Write("Player credits: " + player.Credits.ToString(), LogFiles.FullSimulationLog);
				InfoLog.Write("Player power: " + player.Power.ToString(), LogFiles.FullSimulationLog);
				InfoLog.Write("Player colour: " + player.Color.ToString(), LogFiles.FullSimulationLog);
				InfoLog.Write("\r\n", LogFiles.FullSimulationLog);
				InfoLog.Write("Player units info: ", LogFiles.FullSimulationLog);
				InfoLog.Write("Player units count: " + player.GetAllUnits().Count, LogFiles.FullSimulationLog);
                foreach (Unit unit in player.GetAllUnits()) {
					InfoLog.Write(unit.ToString(), LogFiles.FullSimulationLog);
                }
				InfoLog.Write("\r\nPlayer buildings info: ");
                foreach (Building b in player.GetAllBuildings()){
					InfoLog.Write(b.ToString(), LogFiles.FullSimulationLog);
                }
			}
            foreach (UnitSandworm sandworm in sandworms.Values) {
                InfoLog.Write(sandworm.ToString(), LogFiles.FullSimulationLog);
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
            this._unitsProcessed.Remove(u);
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
            this._buildingsProcessed.Remove(b);
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
        public abstract void handleAttackBuilding(Building attacked, Ammo attacker);
        public abstract void handleAttackBuilding(Building attacked, Unit attacker, short count);

        public abstract void handleAttackBuilding(Building attacked, Building attacker);
        public abstract void handleAttackBuilding(Building attacked, Building attacker, short count);

        public abstract void handleAttackUnit(Unit attacked, Building attacker);
        public abstract void handleAttackUnit(Unit attacked, Ammo attacker);
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
        public abstract void OnShoot(Ammo a);
        public abstract void OnAmmoBlow(Ammo a);
		protected abstract void onInvalidMove(Unit unit);
		#endregion

		#region public methods
		public void AddGameMessage(GameMessage gameMessage) {
			InfoLog.WriteInfo("Waiting to add message", EPrefix.SimulationInfo);
			lock (_turns.SyncRoot) {
				InfoLog.WriteInfo("Adding message: " + gameMessage.Type.ToString(), EPrefix.SimulationInfo);
                int turno = gameMessage.IdTurn - (this.CurrentTurn + 1);
				if (turno < 0 || turno >= _bufferLength) {
					MessageBox.Show("Error! turno = " + turno.ToString());
				}
				if (gameMessage.Type == MessageType.BuildUnitMessage || gameMessage.Type == MessageType.Build) {
					InfoLog.WriteInfo("Adding Build \\BuildUnitMessage to _turn[" + turno + "], actual turn: " + this.CurrentTurn, EPrefix.Test);
				}
                this._turns[turno].Add(gameMessage);
			}
		}

		/// <summary>
		/// This function should be called ONLY after ProcessTurns completes one turn
		/// and sends a message to the server asking for a next turn
		/// </summary>
		public void DoTurn() {
			//InfoLog.WriteInfo("queue new turn", EPrefix.SimulationInfo);
			lock (_turns.SyncRoot) {
				_currentMessages = ShiftTurns();
				_currentTurn++;
				try {
					_nextTurnSemaphore.Release();
				} catch (SemaphoreFullException) {
					_currentTurn--;
					MessageBox.Show("DoTurn called to early! Previous turn not yet completed! This can lead to certain problems.");
				}
			}
			int ms = (Environment.TickCount - turnAsk);
			InfoLog.WriteInfo("Ask-Permit length: " +  ms.ToString(), EPrefix.SimulationInfo);
		}

		public void StartSimulation() {
			if (_turnProcessor != null) {
				throw new Exception("Simulation already started! Abort simulation first.");
			}
			this._turnProcessor = new Thread(new ThreadStart(ProcessTurns));
			//this.turnProcessor.IsBackground = true;
			_turnProcessor.Start();
		}

		public void AbortSimulation() {
			//this.turnProcessor.Abort();
			if (this._turnProcessor == null) {
				throw new Exception("Simulation not started. Start simulation first.");
			}

			this._abort = true;
			try {
				// dla pewnoœci pozwalamy w¹tkowi przejœc do nastêpnej tury,
				// gdy¿ mo¿e czekaæ na semaforze
				this._nextTurnSemaphore.Release();
			} catch (SemaphoreFullException) {
				//trudno ;p
			}
			//_turnProcessor.Join();
			//this._turnProcessor = null;
		}

		public int CurrentTurn {
			get {
				lock (_turns.SyncRoot) {
					return _currentTurn;
				}
			}
		}

		public int Delta {
			get { return delta; }
		}

		public bool SpeedUp {
			get { return this._speedUpLength > 0; }
			set {
				if (value) {
					this._speedUpLength = delta;
				} else {
					this._speedUpLength = 0;
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

        public Player SimulationPlayer {
            get {
                return _simulationPlayer;
            }
        }

       
		#endregion

        public void RemoveAmmo(Ammo ammo) {
            this.players[ammo.ObjectID.PlayerID].RemoveAmmo(ammo);
        }
        public void AddAmmo(Ammo ammo) {
            this.players[ammo.ObjectID.PlayerID].AddAmmo(ammo);
        }
    }
}
