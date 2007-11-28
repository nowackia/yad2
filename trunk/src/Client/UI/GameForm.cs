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

        private BuildManager _buildManager;
		#endregion

		#region Constructor
		public GameForm() {
			try {
                
				InfoLog.WriteInfo("MainForm constructor starts", EPrefix.Menu);

				InitializeComponent();

                this.gameFormClose = false;
				this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

                Connection.Instance.ConnectionLost += new ConnectionLostEventHandler(ConnectionInstance_ConnectionLost);

				_gameLogic = new GameLogic();
				_gameLogic.Simulation.BuildingCompleted += new ClientSimulation.BuildingHandler(Simulation_OnBuildingCompleted);
				_gameLogic.Simulation.UnitCompleted += new ClientSimulation.UnitHandler(Simulation_OnUnitCompleted);
				_gameLogic.Simulation.onTurnEnd += new SimulationHandler(Simulation_onTurnEnd);
				_gameLogic.Simulation.OnCreditsUpdate += new ClientSimulation.OnCreditsHandler(UpdateCredits);

				leftStripe.onBuildingChosen += new BuildingChosenHandler(leftStripe_onBuildingChosen);
				rightStripe.onBuildingChosen += new BuildingChosenHandler(rightStripe_onBuildingChosen);
				rightStripe.onUnitChosen += new UnitChosenHandler(rightStripe_onUnitChosen);

				InfoLog.WriteInfo("MainForm constructor: initializing OpenGL", EPrefix.GameGraphics);

				//initializes GameGraphics
				this.openGLView.InitializeContexts();

				//First: set appropriate properties
				InfoLog.WriteInfo("MainForm constructor: initializing GameLogic", EPrefix.GameGraphics);
				GameGraphics.InitGL(_gameLogic, this);
				GameGraphics.SetViewSize(openGLView.Width, openGLView.Height);
				InfoLog.WriteInfo("MainForm constructor: initializing Textures", EPrefix.GameGraphics);
				GameGraphics.InitTextures(_gameLogic.Simulation);

				InfoLog.WriteInfo("MainForm constructor: initializing OpenGL finished", EPrefix.GameGraphics);

				GameGraphics.GameGraphicsChanged += new EventHandler(gg_GameGraphicsChanged);

				this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
                _buildManager = new BuildManager(this._gameLogic, this.leftStripe, this.rightStripe);
				_gameLogic.Simulation.onTurnEnd += new SimulationHandler(_buildManager.ProcessTurn);
                _gameLogic.Simulation.BuildingDestroyed += new ClientSimulation.BuildingHandler(Simulation_BuildingDestroyed);
                _gameLogic.GameEnd += new GameLogic.GameEndHandler(Simulation_GameEnd);
                _buildManager.CreateUnit += new CreateUnitHandler(this.PlaceUnit);
                GameMessageHandler.Instance.Resume();

			} catch (Exception e) {
				Console.Out.WriteLine(e);
				MessageBox.Show(e.ToString());
			}
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | 0x200;
                return myCp;
            }
        } 
        #endregion

		#region Simulation events handling
		void Simulation_onTurnEnd() {
			this.openGLView.Invalidate();
		}

		void Simulation_OnUnitCompleted(Unit u) {
			//TODO
			//this.rightStripe.RemovePercentCounter(unitType);
		}
		void Simulation_OnBuildingCompleted(Building b) {
			//this.rightStripe.RemovePercentCounter(buildingType);
			//TODO: add building type, update tech-tree
			if (b.ObjectID.PlayerID == _gameLogic.CurrentPlayer.Id) {
				AddBuildingToStripe(b.ObjectID, b.TypeID);
			}
		}

        void ConnectionInstance_ConnectionLost(object sender, EventArgs e)
        {
            GameFormClose = true;
            if (this.InvokeRequired) this.Invoke(new MethodInvoker(this.Close));
            else this.Close();
        }

        void Simulation_GameEnd(int winTeamId)
        {
            InfoLog.WriteInfo("Game End Event", EPrefix.ClientInformation);

            /* Sending message to server */
            bool isWinner = false;

            if (winTeamId == _gameLogic.CurrentPlayer.TeamID)
                isWinner = true;

            GameEndMessage gameEndMessage = (GameEndMessage)Utils.CreateMessageWithSenderId(MessageType.EndGame);
            gameEndMessage.HasWon = isWinner;

            /* Managing the UI */
            MainMenuForm mainMenuForm = FormPool.GetForm(Views.MainMenuForm) as MainMenuForm;
            if (mainMenuForm != null)
                mainMenuForm.MenuMessageHandler.Suspend();

            Connection.Instance.MessageHandler = mainMenuForm.MenuMessageHandler;
            Connection.Instance.SendMessage(gameEndMessage);

            MessageBoxEx.Show(this, "Game result: " + isWinner, "Game End", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.GameFormClose = true;
            if (this.InvokeRequired) this.Invoke(new MethodInvoker(this.Close));
            else this.Close();

            OnMenuOptionChange(MenuOption.GameFormToChat);

            if (mainMenuForm != null)
                mainMenuForm.MenuMessageHandler.Resume();
        }

        void Simulation_BuildingDestroyed(Building b)
        {
            if (b.ObjectID.PlayerID == _gameLogic.CurrentPlayer.Id)
                _buildManager.RemoveBuilding(b.ObjectID, b.TypeID);
        }
		#endregion

		#region Form events
        public bool GameFormClose
        {
            get
            { return gameFormClose; }
            set
            { gameFormClose = value; }
        }

		void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = !this.gameFormClose;
		}
		#endregion

		#region GameGraphics-related

		void gg_GameGraphicsChanged(object sender, EventArgs e) {
			this.openGLView.Invalidate();
		}

		private void openGLView_Paint(object sender, PaintEventArgs e) {
			GameGraphics.Draw();
		}

		private void openGLView_Resize(object sender, EventArgs e) {
			GameGraphics.SetViewSize(openGLView.Width, openGLView.Height);
		}

		#endregion

		#region UI control
		private void openGLView_KeyDown(object sender, KeyEventArgs e) {
			InfoLog.WriteInfo(e.KeyCode.ToString());
			if (e.KeyCode == Keys.Z) {
				Settings.Default.UseSafeRendering = !Settings.Default.UseSafeRendering;
				this.openGLView.Invalidate();
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
			}
		}

		void MainForm_MouseWheel(object sender, MouseEventArgs e) {
			GameGraphics.Zoom(e.Delta / 120);
		}

		private void openGLView_MouseDown(object sender, MouseEventArgs e) {
			InfoLog.WriteInfo("MouseDown");

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

		void optionsButton_Click(object sender, System.EventArgs e) {
			//TODO: zaimplementowaæ menu
		}

		private void HandleRightButtonDown(MouseEventArgs e) {
			_isCreatingBuilding = _isCreatingUnit = false;

			if (_gameLogic.CanGiveOrders()) {
				BoardObject bo = _gameLogic.SimpleSelectAttack(GameGraphics.TranslateMousePosition(e.Location));
				if (bo != null) {
					_gameLogic.AttackOrder(bo);
				} else {
					_gameLogic.MoveOrder(GameGraphics.TranslateMousePosition(e.Location));
				}

			}
		}

		private void HandleMiddleButtonDown(MouseEventArgs e) {
			_mousePos = e.Location;
			_scrolling = true;
			Cursor.Hide();
		}

		private void HandleLeftButtonDown(MouseEventArgs e) {
			Position pos = GameGraphics.TranslateMousePosition(e.Location);

			if (this._isCreatingBuilding) {
				_gameLogic.CreateBuilding(pos, _objectToCreateId);
				this._isCreatingBuilding = false;
				return;
			}

			//this._selecting = true;
			_selectionStart = _selectionEnd = GameGraphics.TranslateMousePosition(e.Location);
		}

		private void openGLView_MouseUp(object sender, MouseEventArgs e) {
			InfoLog.WriteInfo("MouseUp");

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
			_selecting = false;
			this._selectionEnd = GameGraphics.TranslateMousePosition(e.Location);
			_gameLogic.Select(_selectionStart, _selectionEnd);
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
				Cursor.Position = openGLView.PointToScreen(_mousePos);
			}

			if (e.Button == MouseButtons.Left) {
				if (!_selecting) {
					_selectionStart = GameGraphics.TranslateMousePosition(e.Location);
					_selecting = true;
				} else {
					_selectionEnd = GameGraphics.TranslateMousePosition(e.Location);
				}
			}
		}
		#endregion

		#region Stripes-related
		void rightStripe_onUnitChosen(int id, string name) {
			InfoLog.WriteInfo("rightStripe_onUnitChosen " + id, EPrefix.GameGraphics);
            _buildManager.RightBuildingClick(id);
			//PlaceUnit((short)id, name);
		}

		void rightStripe_onBuildingChosen(int id) {
			InfoLog.WriteInfo("rightStripe_onBuildChosen " + id, EPrefix.GameGraphics);
            if (_buildManager.RightBuildingClick(id))
		        PlaceBuilding((short)id);
		}

		private void UpdateCredits(int cost) {
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

		void leftStripe_onBuildingChosen(int id) {
			InfoLog.WriteInfo("leftStripe_onBuildChosen " + id, EPrefix.GameGraphics);
            _buildManager.SwitchCurrentBuilding(id);
			// show building on rightStripe
			//ShowPossibilitiesForBuilding(id);
		}

		private void PlaceUnit(int objectID, short id) {
			//create or let user choose where to place unit?
			//this.isCreatingUnit = true;
			_gameLogic.createUnit(id, objectID);
			_objectToCreateId = id;
		}

		private void PlaceBuilding(short id) {
			this._isCreatingBuilding = true;
			_objectToCreateId = id;
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

		#region public methods
		public bool Selecting {
			get { return _selecting; }
		}

		public Position SelectionStart {
			get { return _selectionStart; }
		}

		public Position SelectionEnd {
			get { return _selectionEnd; }
		}

		#endregion
	}
}
