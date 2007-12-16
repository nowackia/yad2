using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Tao.Platform.Windows;
using Yad.Log;
using Yad.Log.Common;
using Yad.Config.XMLLoader.Common;
using Yad.Config.Common;
using Yad.Board.Common;
using Yad.Engine.GameGraphics.Client;
using Yad.Engine.Client;
using Yad.Net.Messaging.Common;
using Yad.Engine.Common;
using Yad.Config;
using Yad.Board;
using Yad.Net.Client;
using Yad.Utilities.Common;
using Yad.Properties;
using Yad.Properties.Client;
using Yad.Engine;
using Yad.UI.Common;
using Yad.Net.Messaging;
using Tao.OpenGl;

namespace Yad.UI.Client {
	public partial class GameForm : UIManageable {

		#region Private members
		bool _scrolling = false;
		bool _selecting = false;
		bool _wasScrolled = false;
		Point _mousePos;
		Position _selectionStart;
		Position _selectionEnd;
		GameLogic _gameLogic;
		bool gameFormClose;
		/// <summary>
		/// Dictionary of bulding id -> List of items that can be build in that building
		/// </summary>
		Dictionary<short, List<short>> dependDic = new Dictionary<short, List<short>>();
		/// <summary>
		/// True after player clicks strip
		/// </summary>
		private bool _isCreatingBuilding = false, _isCreatingUnit = false;
		/// <summary>
		/// Can be both - unit or building
		/// </summary>
		private short _objectToCreateId;

		/// <summary>
		/// Id of building creator
		/// </summary>
		private int _objectCreatorId;

		private BuildManager _buildManager;
		#endregion

		#region Constructor
		public GameForm() {
			try {

				InfoLog.WriteInfo("MainForm constructor starts", EPrefix.Menu);

				InitializeComponent();

				/* Play peaceful music */
				AudioEngine.Instance.Music.PlayNext(MusicType.Peace);

				this.gameFormClose = false;
				this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

				Connection.Instance.ConnectionLost += new ConnectionLostEventHandler(ConnectionInstance_ConnectionLost);

				_gameLogic = new GameLogic();
				_gameLogic.Simulation.BuildingCompleted += new ClientSimulation.BuildingCreationHandler(Simulation_OnBuildingCompleted);
				_gameLogic.Simulation.UnitCompleted += new ClientSimulation.UnitHandler(Simulation_OnUnitCompleted);
				_gameLogic.Simulation.onTurnEnd += new SimulationHandler(Simulation_onTurnEnd);
				_gameLogic.Simulation.OnCreditsUpdate += new ClientSimulation.OnCreditsHandler(UpdateCredits);
				
				//_gameLogic.Simulation.onTurnEnd += new SimulationHandler(_buildManager.ProcessTurn);
				_gameLogic.Simulation.BuildingDestroyed += new ClientSimulation.BuildingHandler(Simulation_BuildingDestroyed);
				_gameLogic.Simulation.UpdateStripItem += new ClientSimulation.UpdateStripItemHandler(this.UpdateStrip);
				_gameLogic.GameEnd += new GameLogic.GameEndHandler(Simulation_GameEnd);
				_gameLogic.PauseResume += new GameLogic.PauseResumeHandler(this.onPauseResume);

				leftStripe.onBuildingChosen += new BuildingChosenHandler(leftStripe_onBuildingChosen);
				rightStripe.onBuildingChosen += new BuildingChosenHandler(rightStripe_onBuildingChosen);
				rightStripe.onUnitChosen += new UnitChosenHandler(rightStripe_onUnitChosen);
				_buildManager = new BuildManager(this._gameLogic, this.leftStripe, this.rightStripe);
				//_buildManager.CreateUnit += new CreateUnitHandler(this.PlaceUnit);
				_gameLogic.Simulation.InvalidLocation += new ClientSimulation.InvalidLocationHandler(_buildManager.OnBadLocation);
				_gameLogic.OnBadLocation += new GameLogic.BadLocationHandler(_buildManager.OnBadLocation);


				InfoLog.WriteInfo("MainForm constructor: initializing OpenGL", EPrefix.GameGraphics);

				//initializes GameGraphics
				this.miniMap.InitializeContexts();
				this.mapView.InitializeContexts();
				mapView.MakeCurrent();

				//First: set appropriate properties
				InfoLog.WriteInfo("MainForm constructor: initializing GameLogic", EPrefix.GameGraphics);
				GameGraphics.InitGL(_gameLogic, this, mapView, miniMap);
				GameGraphics.SetViewSize(mapView.Width, mapView.Height);
				InfoLog.WriteInfo("MainForm constructor: initializing Textures", EPrefix.GameGraphics);
				GameGraphics.InitTextures(_gameLogic.Simulation);

				InfoLog.WriteInfo("MainForm constructor: initializing OpenGL finished", EPrefix.GameGraphics);

				GameGraphics.GameGraphicsChanged += new EventHandler(gg_GameGraphicsChanged);

				this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
				
				GameMessageHandler.Instance.GameInitialization += new GameInitEventHandler(Instance_GameInitialization);

				GameMessageHandler.Instance.Resume();

			} catch (Exception e) {
				Console.Out.WriteLine(e);
				MessageBox.Show(e.ToString());
			}
		}

		void Instance_GameInitialization(object sender, GameInitEventArgs e) {
			//Jeœli nie bêdzie jednostki to siê samo nie wycentruje
			List<Unit> units = _gameLogic.CurrentPlayer.GetAllUnits();
			if (units.Count == 0) return;
			GameGraphics.centerOn(units[0].Position);
		}


		#endregion

		#region Simulation events handling
		void Simulation_onTurnEnd() {
			this.mapView.Invalidate();
		}

		void Simulation_OnUnitCompleted(Unit u) {
			//TODO
			//this.rightStripe.RemovePercentCounter(unitType);
		}

		void Simulation_OnBuildingCompleted(Building b, int creatorID) {
			//this.rightStripe.RemovePercentCounter(buildingType);
			//TODO: add building type, update tech-tree
			if (b.ObjectID.PlayerID == _gameLogic.CurrentPlayer.Id) {
				if (creatorID != -1)
					BuildEndStripViewUpdate(creatorID);
				AddBuildingToStripe(b.ObjectID, b.TypeID);
			}
		}

		void ConnectionInstance_ConnectionLost(object sender, EventArgs e) {
			AudioEngine.Instance.Music.Stop();
			GameFormClose = true;
			if (this.InvokeRequired) this.Invoke(new MethodInvoker(this.Close));
			else this.Close();
		}

		void Simulation_GameEnd(int winTeamId) {
			InfoLog.WriteInfo("Game End Event", EPrefix.ClientInformation);

			GameGraphics.GameGraphicsChanged -= new EventHandler(gg_GameGraphicsChanged);
			mapView.Paint -= new PaintEventHandler(openGLView_Paint);

			GameGraphics.DeinitGL();

			_gameLogic.Simulation.AbortSimulation();

			_gameLogic.Simulation.BuildingCompleted -= new ClientSimulation.BuildingCreationHandler(Simulation_OnBuildingCompleted);
			_gameLogic.Simulation.UnitCompleted -= new ClientSimulation.UnitHandler(Simulation_OnUnitCompleted);
			_gameLogic.Simulation.onTurnEnd -= new SimulationHandler(Simulation_onTurnEnd);
			_gameLogic.Simulation.OnCreditsUpdate -= new ClientSimulation.OnCreditsHandler(UpdateCredits);
			_gameLogic.Simulation.InvalidLocation -= new ClientSimulation.InvalidLocationHandler(_buildManager.OnBadLocation);
			//_gameLogic.Simulation.onTurnEnd -= new SimulationHandler(_buildManager.ProcessTurn);
			_gameLogic.Simulation.BuildingDestroyed -= new ClientSimulation.BuildingHandler(Simulation_BuildingDestroyed);
			_gameLogic.Simulation.UpdateStripItem -= new ClientSimulation.UpdateStripItemHandler(this.UpdateStrip);
			_gameLogic.OnBadLocation -= new GameLogic.BadLocationHandler(_buildManager.OnBadLocation);
			_gameLogic.GameEnd -= new GameLogic.GameEndHandler(Simulation_GameEnd);
			_gameLogic.PauseResume -= new GameLogic.PauseResumeHandler(this.onPauseResume);
			
			GameMessageHandler.Instance.GameInitialization -= new GameInitEventHandler(Instance_GameInitialization);

			bool isWinner = false;
			if (winTeamId == _gameLogic.CurrentPlayer.TeamID)
				isWinner = true;

			/* Playing proper music */
			if (isWinner) {
				AudioEngine.Instance.Music.Play(MusicType.Win);
				AudioEngine.Instance.Sound.PlayHouse(_gameLogic.CurrentPlayer.House, HouseSoundType.Win);
			} else {
				AudioEngine.Instance.Music.Play(MusicType.Lose);
				AudioEngine.Instance.Sound.PlayHouse(_gameLogic.CurrentPlayer.House, HouseSoundType.Lose);
			}

			/*
			 * KŒ: tu jest burdel, GameForm nie jest od przepinania handlerów, ani od zarz¹dzania okienkami w aplikacji
			 * nie mo¿na wyœwietliæ GameForm jako okna modalnego, albo zrobiæ w nim event?
			 */

			/* Sending message to server */
			GameEndMessage gameEndMessage = (GameEndMessage)Utils.CreateMessageWithSenderId(MessageType.EndGame);
			gameEndMessage.HasWon = isWinner;

			/* Managing the UI */
			MainMenuForm mainMenuForm = FormPool.GetForm(Views.MainMenuForm) as MainMenuForm;
			if (mainMenuForm != null)
				mainMenuForm.MenuMessageHandler.Suspend();

			Connection.Instance.MessageHandler = mainMenuForm.MenuMessageHandler;
			Connection.Instance.SendMessage(gameEndMessage);

			MessageBoxEx.Show(this, "Game result: " + isWinner, "Game End", MessageBoxButtons.OK, MessageBoxIcon.Information);
			/* Stop playing music */
			AudioEngine.Instance.Music.Stop();

			this.GameFormClose = true;
			if (this.InvokeRequired) this.Invoke(new MethodInvoker(this.Close));
			else this.Close();

			OnMenuOptionChange(MenuOption.GameFormToChat);

			if (mainMenuForm != null)
				mainMenuForm.MenuMessageHandler.Resume();

			//_gameLogic.Simulation.AbortSimulation();
		}

		void Simulation_BuildingDestroyed(Building b) {
			if (b.ObjectID.PlayerID == _gameLogic.CurrentPlayer.Id)
				_buildManager.RemoveBuilding(b.ObjectID, b.TypeID);
		}
		#endregion

		#region Form events
		public bool GameFormClose {
			get { return gameFormClose; }
			set { gameFormClose = value; }
		}

		void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			e.Cancel = !this.gameFormClose;

			if (!this.gameFormClose) {
				return;
			}

			//KŒ: nie wiem czy niezbêdne, ale tak na wszelki wypadek...
			Connection.Instance.ConnectionLost -= new ConnectionLostEventHandler(ConnectionInstance_ConnectionLost);
			GameMessageHandler.Instance.GameInitialization -= new GameInitEventHandler(Instance_GameInitialization);
		}
		#endregion

		#region GameGraphics-related

		void gg_GameGraphicsChanged(object sender, EventArgs e) {
			this.mapView.Invalidate();
			this.miniMap.Invalidate();
		}

		private void openGLView_Paint(object sender, PaintEventArgs e) {
			GameGraphics.Draw();
		}

		private void openGLView_Resize(object sender, EventArgs e) {
			GameGraphics.SetViewSize(mapView.Width, mapView.Height);
		}


		#endregion

		#region UI control
		private void openGLView_KeyDown(object sender, KeyEventArgs e) {
			InfoLog.WriteInfo(e.KeyCode.ToString());
			if (e.KeyCode == Keys.Z) {
				Settings.Default.UseSafeRendering = !Settings.Default.UseSafeRendering;
				this.mapView.Invalidate();
			} else if (e.KeyCode == Keys.Q) {
				GameGraphics.Zoom(-1);
			} else if (e.KeyCode == Keys.E) {
				GameGraphics.Zoom(1);
			} else if (e.KeyCode == Keys.A) {
				GameGraphics.OffsetX(-Settings.Default.ScrollingSpeed);
			} else if (e.KeyCode == Keys.D) {
				GameGraphics.OffsetX(Settings.Default.ScrollingSpeed);
			} else if (e.KeyCode == Keys.W) {
				GameGraphics.OffsetY(Settings.Default.ScrollingSpeed);
			} else if (e.KeyCode == Keys.S) {
				GameGraphics.OffsetY(-Settings.Default.ScrollingSpeed);
			} else if (e.KeyCode == Keys.X) {
				_gameLogic.DeployMCV();
			} else if (e.KeyCode == Keys.U) {
				AudioEngine.Instance.Sound.PlayMisc(MiscSoundType.ComeToPappa);
			}
		}

		void MainForm_MouseWheel(object sender, MouseEventArgs e) {
			GameGraphics.Zoom(e.Delta / 120);
		}

		private void openGLView_MouseDown(object sender, MouseEventArgs e) {
			InfoLog.WriteInfo("MouseDown", EPrefix.UIManager);

			switch (e.Button) {
				case MouseButtons.Left:
					HandleLeftButtonDown(e);
					break;
				case MouseButtons.Middle:
					HandleMiddleButtonDown(e);
					break;
				case MouseButtons.Right:
					HandleRightButtonDown(e);
					break;
				case MouseButtons.XButton1:
				case MouseButtons.XButton2:
				case MouseButtons.None:
				default:
					break;
			}
		}

		private void HandleRightButtonDown(MouseEventArgs e) {
			Position pos = GameGraphics.TranslateMousePosition(e.Location);
			if (!_isCreatingBuilding && !_isCreatingUnit) {
				BoardObject bo = boardObjectAt(pos);
				if (bo != null) {
					_gameLogic.AttackOrder(bo);
				} else {
					_gameLogic.MoveOrder(pos);
				}
				return;
			}

			if (_isCreatingBuilding) {
				_buildManager.OnBadLocation(_objectCreatorId);
			}

			_isCreatingUnit = false;
			_isCreatingBuilding = false;
		}

		public BoardObject boardObjectAt(Position pos) {
			LinkedList<Unit> unitsOnPos = _gameLogic.Simulation.Map.Units[pos.X, pos.Y];
			if (unitsOnPos.Count != 0) {
				return unitsOnPos.First.Value;
			}

			LinkedList<Building> buildingOnPos = _gameLogic.Simulation.Map.Buildings[pos.X, pos.Y];
			if (buildingOnPos.Count != 0) {
				return buildingOnPos.First.Value;
			}
			return null;
		}

		private void HandleMiddleButtonDown(MouseEventArgs e) {
			_mousePos = e.Location;
			_scrolling = true;
			Cursor.Hide();
		}

		static bool yes = true;
		private void HandleLeftButtonDown(MouseEventArgs e) {

			Position pos = GameGraphics.TranslateMousePosition(e.Location);
			if (this._isCreatingBuilding) {
				_gameLogic.CreateBuilding(pos, _objectToCreateId, _objectCreatorId);
				this._isCreatingBuilding = false;
				return;
			}

			//this._selecting = true;
			_selectionStart = _selectionEnd = GameGraphics.TranslateMousePosition(e.Location);
		}

		

		private void openGLView_MouseUp(object sender, MouseEventArgs e) {
			InfoLog.WriteInfo("MouseUp", EPrefix.UIManager);

			switch (e.Button) {
				case MouseButtons.Left:
					HandleLeftButtonUp(e);
					break;
				case MouseButtons.Middle:
					HandleMiddleButtonUp();
					break;
				case MouseButtons.Right:
					break;
				case MouseButtons.XButton1:
				case MouseButtons.XButton2:
				case MouseButtons.None:
				default:
					break;
			}
		}

		private void HandleMiddleButtonUp() {
			_scrolling = false;
			Cursor.Show();
		}

		private void HandleLeftButtonUp(MouseEventArgs e) {
			if (_selecting) {
				_selecting = false;
				this._selectionEnd = GameGraphics.TranslateMousePosition(e.Location);
				_gameLogic.Select(_selectionStart, _selectionEnd);
			}
		}

		private void openGLView_MouseMove(object sender, MouseEventArgs e) {
			if (_wasScrolled) {
				_wasScrolled = false;
				return;
			}

			if (_scrolling) {
				int dx = e.X - _mousePos.X;
				int dy = e.Y - _mousePos.Y;

				GameGraphics.OffsetX(-dx * 0.05f);
				GameGraphics.OffsetY(dy * 0.05f); //opengl uses different coordinate system

				_wasScrolled = true;
				Cursor.Position = mapView.PointToScreen(_mousePos);
			}

			if (e.Button == MouseButtons.Left) {
				if (!_selecting) {
					_selectionStart = GameGraphics.TranslateMousePosition(e.Location);
					_selecting = true;
				} else {
					mapView.Invalidate();
					_selectionEnd = GameGraphics.TranslateMousePosition(e.Location);
				}
			}
		}

		private void pictureButtonOptions_Click(object sender, EventArgs e) {
			OnMenuOptionChange(MenuOption.Options);
		}

		private void onPauseResume(bool isPause) {
			if (isPause)
				OnMenuOptionChange(MenuOption.Pause);
			else
				OnMenuOptionChange(MenuOption.Continue);
		}
		#endregion

		#region Stripes-related
		void UpdateStrip(int playerID, int id, short typeID, int percent) {
			if (playerID == _gameLogic.CurrentPlayer.Id)
				_buildManager.UpdateStrip(id, typeID, percent);
		}
		void rightStripe_onUnitChosen(int id, string name) {
			InfoLog.WriteInfo("rightStripe_onUnitChosen " + id, EPrefix.GameGraphics);
			int playerCredits = _gameLogic.CurrentPlayer.Credits;
			int cost = GlobalSettings.GetUnitCost((short)id);
			if (playerCredits >= cost)
				_buildManager.RightBuildingClick(id, true);
            else
                /* Sound */
                AudioEngine.Instance.Sound.PlayMisc(MiscSoundType.Cannot);
		}



		void rightStripe_onBuildingChosen(int id) {
			InfoLog.WriteInfo("rightStripe_onBuildChosen " + id, EPrefix.GameGraphics);
			int playerCredits = _gameLogic.CurrentPlayer.Credits;
			int cost = GlobalSettings.Wrapper.buildingsMap[(short)id].Cost;
			if (playerCredits >= cost) {
				int creator = _buildManager.RightBuildingClick(id, false);
				if (creator != -1)
					PlaceBuilding((short)id, creator);
			}
            else
                /* Sound */
                AudioEngine.Instance.Sound.PlayMisc(MiscSoundType.Cannot);
		}

		private void UpdateCredits(short idPlayer, int cost) {
			if (idPlayer == _gameLogic.CurrentPlayer.Id) {
				creditsPictureBox.Value -= cost;

				//strip's update
				foreach (BuildingData b in GlobalSettings.Wrapper.Buildings)
					rightStripe.Enabled(b.TypeID, (b.Cost < creditsPictureBox.Value));
				foreach (UnitHarvesterData b in GlobalSettings.Wrapper.Harvesters)
					rightStripe.Enabled(b.TypeID, (b.Cost < creditsPictureBox.Value));
				foreach (UnitMCVData b in GlobalSettings.Wrapper.MCVs)
					rightStripe.Enabled(b.TypeID, (b.Cost < creditsPictureBox.Value));
				foreach (UnitTankData b in GlobalSettings.Wrapper.Tanks)
					rightStripe.Enabled(b.TypeID, (b.Cost < creditsPictureBox.Value));
				foreach (UnitTrooperData b in GlobalSettings.Wrapper.Troopers)
					rightStripe.Enabled(b.TypeID, (b.Cost < creditsPictureBox.Value));
			}
		}

		void leftStripe_onBuildingChosen(int id) {
			InfoLog.WriteInfo("leftStripe_onBuildChosen " + id, EPrefix.GameGraphics);
			_buildManager.SwitchCurrentBuilding(id);
			// show building on rightStripe
			//ShowPossibilitiesForBuilding(id);
		}

		/*
		private void PlaceUnit(int objectID, short id) {
			//create or let user choose where to place unit?
			//this.isCreatingUnit = true;
			_gameLogic.createUnit(id, objectID);
			//_objectToCreateId = id;
		}
		 */

		private void PlaceBuilding(short id, int creatorID) {
			_objectToCreateId = id;
			_objectCreatorId = creatorID;
			this._isCreatingBuilding = true;
		}
		private void PlaceBuilding(short id) {
			_objectToCreateId = id;
			this._isCreatingBuilding = true;
		}

		/*
		internal void addUnitCreationPossibility(string name, short key) {

			short id = gameLogic.GameSettingsWrapper.namesToIds[name];
			//stripesManager.BuildingClickedOnMap(id); //remove -- this method will be used when smb. clicks on a building -> units on menu
			rightStripe.Add(id, name, Path.Combine(Settings.Default.Pictures, name + ".png"), false);//TODO add picture name to xsd.
		}
		*/
		/* RS
		public void AddBuildingToStripe(short id) {
			String name = GlobalSettings.Wrapper.buildingsMap[id].Name;
			leftStripe.Add(id, name, name, true); //TODO add picture name to xsd.
		}*/

		public void BuildEndStripViewUpdate(int creatorID) {
			_buildManager.ReadyReset(creatorID);
		}
		public void AddBuildingToStripe(ObjectID objectid, short typeId) {
			_buildManager.AddBuilding(objectid, typeId);
			//string name = GlobalSettings.Wrapper.buildingsMap[id].Name;
			//leftStripe.Add(id, name, name, true);
		}

		Dictionary<short, OwnerDrawPictureButton> stripLeftItems = new Dictionary<short, OwnerDrawPictureButton>();
		/*public void CreateBuildingButtonsOnStripe() {
			foreach (short id in GlobalSettings.Wrapper.buildingsMap.Keys){
				List<short> dep = new List<short>();
				string name = GlobalSettings.Wrapper.buildingsMap[id].Name;
				//leftStripe.Add(id, name, name, true);
				BuildingData bdata = GlobalSettings.Wrapper.buildingsMap[id];
				foreach (String bname in bdata.BuildingsCanProduce) {
					short idb = GlobalSettings.Wrapper.namesToIds[bname];
					ObjectID obid = new ObjectID();
					obid.ObjectId = -1;
					rightStripe.Add(idb, obid, bname, bname, true);
					dep.Add(idb);
                    
				}
				foreach (String uname in bdata.UnitsCanProduce) {
					short idu = GlobalSettings.Wrapper.namesToIds[uname];
					ObjectID obid = new ObjectID();
					obid.ObjectId = -1;
					rightStripe.Add(idu, obid, uname, uname, false);
					dep.Add(idu);
				}
				dependDic.Add(id, dep);
			}
			//leftStripe.HideAll();
			rightStripe.HideAll();
         
		}*/
		/*public void ShowPossibilitiesForBuilding(short idB) {
			rightStripe.HideAll();
			List<short> itemsToShow = new List<short>();

			foreach (short id in dependDic[idB]) {
				if (GlobalSettings.Wrapper.buildingsMap.ContainsKey(id)) {
					string name = GlobalSettings.Wrapper.buildingsMap[id].Name;
					if (CheckDependencies(name))
						itemsToShow.Add(id);
				}
				else
					itemsToShow.Add(id);
                    
			}
			rightStripe.ShowRange(itemsToShow.ToArray());
		}*/
		/*
		public void ShowPossibilitiesForBuilding(short idB) {
			rightStripe.RemoveAll(); // flush the stripe

			//simulation.GameSettingsWrapper.
			if (leftStripe.Ids.Contains(idB)) {
				BuildingData data = GlobalSettings.Wrapper.buildingsMap[idB];
				foreach (String name in data.BuildingsCanProduce) {
					short id = GlobalSettings.Wrapper.namesToIds[name];
					InfoLog.WriteInfo(name, EPrefix.ClientInformation);
					if (rightStripe.Ids.Contains(id)) continue;
					//TODO: use dictionary<short id, Bitmap picture>, initialize in GameSettingsWrapper contructor
					if (CheckDependencies(name)) {
						rightStripe.Add(id, name, name, true);
					}
				}
				foreach (String name in data.UnitsCanProduce) {
					short id = GlobalSettings.Wrapper.namesToIds[name];
					if (rightStripe.Ids.Contains(id)) continue;

					//TODO: use dictionary<short id, Bitmap picture>, initialize in GameSettingsWrapper contructor
					rightStripe.Add(id, name, name, false);
				}
			}
		}*/

		private bool CheckDependencies(string name) {
			TechnologyDependences deps = GlobalSettings.Wrapper.racesMap[_gameLogic.CurrentPlayer.House].TechnologyDependences;
			foreach (TechnologyDependence dep in deps.TechnologyDependenceCollection) {
				if (dep.BuildingName.Equals(name)) {
					foreach (string n in dep.RequiredBuildings) {
						short id = GlobalSettings.Wrapper.namesToIds[n];
						if (_gameLogic.hasBuilding(id) == false) return false;
					}
				}
			}
			return true;
		}

		internal bool IsStripContainingBuilding(short ids) {
			return leftStripe.Ids.Contains(ids);
		}
		#endregion

		#region Public methods
		public bool IsSelecting {
			get { return _selecting; }
		}

		public Position SelectionStart {
			get { return _selectionStart; }
		}

		public Position SelectionEnd {
			get { return _selectionEnd; }
		}

		public bool IsCreatingBuilding {
			get { return _isCreatingBuilding; }
		}

		public short CreatingBuildingId {
			get { return _objectToCreateId; }
		}

		public Point getMousePositionInMapView() {
			return mapView.PointToClient(Cursor.Position);
		}

		#endregion

		private void miniMap_MouseDown(object sender, MouseEventArgs e) {
			InfoLog.WriteInfo("miniMap MouseDown", EPrefix.UIManager);

			switch (e.Button) {
				case MouseButtons.Left:
					HandleMiniMapLeftButtonDown(e);
					break;
				case MouseButtons.Right:
					HandleMiniMapRightButtonDown(e);
					break;
				case MouseButtons.XButton1:
				case MouseButtons.XButton2:
				case MouseButtons.None:
				default:
					break;
			}
		}

		private void HandleMiniMapRightButtonDown(MouseEventArgs e) {
			
		}

		private void HandleMiniMapLeftButtonDown(MouseEventArgs e) {
			GameGraphics.centerOn(GameGraphics.TranslateMinimapMousePosition(e.Location));
		}

		private void miniMap_MouseMove(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				Position p = GameGraphics.TranslateMinimapMousePosition(e.Location);
				GameGraphics.centerOn(p);
				Console.Out.WriteLine(e.Location + " -> " + p);
			}
		}
	}
}
