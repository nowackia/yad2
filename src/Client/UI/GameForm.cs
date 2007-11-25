using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tao.Platform.Windows;
using System.IO;
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

namespace Yad.UI.Client {
	public partial class GameForm : UIManageable {

		#region private members
		bool _scrolling = false;
		bool _selecting = false;
		bool _wasScrolled = false;
		Point _mousePos;
		Position _selectionStart;
		Position _selectionEnd;
		GameLogic _gameLogic;

		/// <summary>
		/// True after player clicks strip
		/// </summary>
		private bool _isCreatingBuilding = false, _isCreatingUnit = false;
		/// <summary>
		/// Can be both - unit or building
		/// </summary>
		private short _objectToCreateId;

		#endregion

		#region constructor
		public GameForm() {
			try {
				InfoLog.WriteInfo("MainForm constructor starts", EPrefix.Menu);

				InitializeComponent();

				this.FormClosed += new FormClosedEventHandler(MainForm_FormClosed);
				this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

				_gameLogic = new GameLogic();
				_gameLogic.Simulation.BuildingCompleted += new ClientSimulation.BuildingHandler(Simulation_OnBuildingCompleted);
				_gameLogic.Simulation.UnitCompleted += new ClientSimulation.UnitHandler(Simulation_OnUnitCompleted);
				_gameLogic.Simulation.onTurnEnd += new SimulationHandler(Simulation_onTurnEnd);
			_gameLogic.Simulation.OnCreditsUpdate += new ClientSimulation.OnCreditsHandler(UpdateCredits);


				leftStripe.onBuildingChosen += new BuildingChosenHandler(leftStripe_onBuildingChosen);
				//leftStripe.onUnitChosen //there should be no units there...
				rightStripe.onBuildingChosen += new BuildingChosenHandler(rightStripe_onBuildingChosen);
				rightStripe.onUnitChosen += new UnitChosenHandler(rightStripe_onUnitChosen);

				InfoLog.WriteInfo("MainForm constructor: initializing OpenGL", EPrefix.GameGraphics);

				//initializes GameGraphics
				this.openGLView.InitializeContexts();

				//First: set appropriate properties
				InfoLog.WriteInfo("MainForm constructor: initializing GameLogic", EPrefix.GameGraphics);
				GameGraphics.InitGL(_gameLogic);
				GameGraphics.SetViewSize(openGLView.Width, openGLView.Height);
				InfoLog.WriteInfo("MainForm constructor: initializing Textures", EPrefix.GameGraphics);
				GameGraphics.InitTextures(_gameLogic.Simulation);

				InfoLog.WriteInfo("MainForm constructor: initializing OpenGL finished", EPrefix.GameGraphics);

				GameGraphics.GameGraphicsChanged += new EventHandler(gg_GameGraphicsChanged);

				this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);

				GameMessageHandler.Instance.Resume();
			} catch (Exception e) {
				Console.Out.WriteLine(e);
				MessageBox.Show(e.ToString());
			}
		}
		#endregion

		#region simulation events handling
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
			AddBuildingToStripe(b.TypeID);
		}
		#endregion

		#region form events
		void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			OnMenuOptionChange(MenuOption.Options);
			e.Cancel = true;
		}

		void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
			OnMenuOptionChange(MenuOption.Options);
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
			//InfoLog.WriteInfo("Resizing...", EPrefix.UIManager);

			GameGraphics.SetViewSize(openGLView.Width, openGLView.Height);
		}

		#endregion

		#region ui control
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

			this._selecting = true;
			this._selectionStart = GameGraphics.TranslateMousePosition(e.Location);
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
		}
		#endregion

		#region stripes-related
		void rightStripe_onUnitChosen(short id) {
			InfoLog.WriteInfo("rightStripe_onUnitChosen " + id, EPrefix.GameGraphics);
			PlaceUnit(id);
		}

		void rightStripe_onBuildingChosen(short id) {
			InfoLog.WriteInfo("rightStripe_onBuildChosen " + id, EPrefix.GameGraphics);
				PlaceBuilding(id);
		}

		private void UpdateCredits(short id) {
			if (GlobalSettings.Wrapper.buildingsMap.ContainsKey(id)) {
				creditsPictureBox.Value -= GlobalSettings.Wrapper.buildingsMap[id].Cost;
			} else if (GlobalSettings.Wrapper.harvestersMap.ContainsKey(id)) {
				creditsPictureBox.Value -= GlobalSettings.Wrapper.harvestersMap[id].Cost;
			} else if (GlobalSettings.Wrapper.mcvsMap.ContainsKey(id)) {
				creditsPictureBox.Value -= GlobalSettings.Wrapper.mcvsMap[id].Cost;
			} else if (GlobalSettings.Wrapper.tanksMap.ContainsKey(id)) {
				creditsPictureBox.Value -= GlobalSettings.Wrapper.tanksMap[id].Cost;
			} else if (GlobalSettings.Wrapper.troopersMap.ContainsKey(id)) {
				creditsPictureBox.Value -= GlobalSettings.Wrapper.troopersMap[id].Cost;
			}

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

		void leftStripe_onBuildingChosen(short id) {
			InfoLog.WriteInfo("leftStripe_onBuildChosen " + id, EPrefix.GameGraphics);
			// show building on rightStripe
			ShowPossibilitiesForBuilding(id);
		}

		private void PlaceUnit(short id) {
			//create or let user choose where to place unit?
			//this.isCreatingUnit = true;
			//gameLogic.createUnit(short unitID, Building!!)
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

		public void AddBuildingToStripe(short id) {
			String name = GlobalSettings.Wrapper.buildingsMap[id].Name;
			leftStripe.Add(id, name, name, true); //TODO add picture name to xsd.
		}

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
		}

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
	}
}
