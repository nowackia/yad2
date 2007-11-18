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

namespace Yad.UI.Client {
	public partial class GameForm : UIManageable {

		#region private members
		bool scrolling = false;
		bool selecting = false;
		bool wasScrolled = false;
		Point mousePos;
		Position selectionStart;
		Position selectionEnd;
		GameLogic gameLogic;
		#endregion

		#region stripes
		private StripesManager stripesManager;
		#endregion

		#region constructor
		public GameForm() {
			InfoLog.WriteInfo("MainForm constructor starts", EPrefix.Menu);

			InitializeComponent();

			gameLogic = new GameLogic();
			
			//TODO wtf? which race i am?
			short key = 0;
			foreach (short k in gameLogic.GameSettingsWrapper.racesMap.Keys) {
				key = k;
				break;
			}
			stripesManager = new StripesManager(gameLogic.Simulation, key, rightStripe, leftStripe);
			gameLogic.AddBuildingEvent += new GameLogic.AddBuildingDelegate(AddBuilding);
			gameLogic.AddUnitEvent += new GameLogic.AddUnitDelegate(addUnitCreationPossibility);
			gameLogic.InitStripes("ConstructionYard", key);

			gameLogic.Simulation.OnBuildingCompleted += new ClientSimulation.BuildingCompletedHandler(Simulation_OnBuildingCompleted);
			gameLogic.Simulation.OnUnitCompleted += new ClientSimulation.UnitCompletedHandler(Simulation_OnUnitCompleted);

			this.FormClosed += new FormClosedEventHandler(MainForm_FormClosed);
			this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

			InfoLog.WriteInfo("MainForm constructor: initializing OpenGL", EPrefix.GameGraphics);

			//initializes GameGraphics
			this.openGLView.InitializeContexts();

			//First: set appropriate properties
			GameGraphics.InitGL(gameLogic);
			GameGraphics.SetViewSize(openGLView.Width, openGLView.Height);
			GameGraphics.InitTextures(gameLogic.Simulation);

			InfoLog.WriteInfo("MainForm constructor: initializing OpenGL finished", EPrefix.GameGraphics);

			GameGraphics.GameGraphicsChanged += new EventHandler(gg_GameGraphicsChanged);

			this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);

			//v remove
			this.gameLogic.StartSimulation();
			//^ remove
		}

		void Simulation_OnUnitCompleted(short unitType) {
			this.stripesManager.RemovePercentageCounter(unitType, false);
		}

		void Simulation_OnBuildingCompleted(short buildingType) {
			this.stripesManager.RemovePercentageCounter(buildingType, true);
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

		void gg_GameGraphicsChanged(object sender, EventArgs e) {
			this.openGLView.Invalidate();
		}

		void MainForm_MouseWheel(object sender, MouseEventArgs e) {
			GameGraphics.Zoom(e.Delta / 120);
		}

		private void openGLView_KeyDown(object sender, KeyEventArgs e) {
			InfoLog.WriteInfo(e.KeyCode.ToString());
			if (e.KeyCode == Keys.Z) {
				Settings.Default.UseSafeRendering = !Settings.Default.UseSafeRendering;
				this.openGLView.Invalidate();
			}
			if (e.KeyCode == Keys.Q) {
				GameGraphics.Zoom(-1);
			} else if (e.KeyCode == Keys.E) {
				GameGraphics.Zoom(1);
			} else if (e.KeyCode == Keys.A) {
				GameGraphics.OffsetX(Settings.Default.ScrollingSpeed);
			} else if (e.KeyCode == Keys.D) {
				GameGraphics.OffsetX(Settings.Default.ScrollingSpeed);
			} else if (e.KeyCode == Keys.W) {
				GameGraphics.OffsetY(Settings.Default.ScrollingSpeed);
			} else if (e.KeyCode == Keys.S) {
				GameGraphics.OffsetY(Settings.Default.ScrollingSpeed);
			}
		}

		public bool IsStripContainingBuilding(short ids) {
			return StripesManager.ContainsId(ids);
		}

		private void openGLView_MouseDown(object sender, MouseEventArgs e) {
			InfoLog.WriteInfo("MouseDown");
			if (e.Button == MouseButtons.Right) {
				mousePos = e.Location;
				scrolling = true;
			} else if (e.Button == MouseButtons.Left) {
				this.selecting = true;
				this.selectionStart = GameGraphics.TranslateMousePosition(e.Location);
			}
		}

		private void openGLView_MouseUp(object sender, MouseEventArgs e) {
			InfoLog.WriteInfo("MouseUp");
			if (e.Button == MouseButtons.Right) {
				scrolling = false;
				gameLogic.IssuedOrder(GameGraphics.TranslateMousePosition(e.Location));
			} else if (e.Button == MouseButtons.Left) {
				selecting = false;
				this.selectionEnd = GameGraphics.TranslateMousePosition(e.Location);
				gameLogic.Select(selectionStart, selectionEnd);
			}
		}

		private void openGLView_MouseMove(object sender, MouseEventArgs e) {
			if (wasScrolled) {
				wasScrolled = false;
				return;
			}
			if (scrolling) {
				int dx = e.X - mousePos.X;
				int dy = e.Y - mousePos.Y;

				GameGraphics.OffsetX(-dx * 0.05f);
				GameGraphics.OffsetY(dy * 0.05f); //opengl uses different coordinate system

				wasScrolled = true;
				Cursor.Position = openGLView.PointToScreen(mousePos);
			}
		}

		private void openGLView_Paint(object sender, PaintEventArgs e) {
			GameGraphics.Draw();
		}

		private void openGLView_Resize(object sender, EventArgs e) {
			//InfoLog.WriteInfo("Resizing...", EPrefix.UIManager);

			GameGraphics.SetViewSize(openGLView.Width, openGLView.Height);
		}

		internal void addUnitCreationPossibility(string name, short key)
		{
			
			short id = gameLogic.GameSettingsWrapper.namesToIds[name];
			//stripesManager = new StripesManager(sim, key, rightStripe,this.leftStripe);
			stripesManager.AddUnit(id);
			//stripesManager.BuildingClickedOnMap(id); //remove -- this method will be used when smb. clicks on a building -> units on menu
			rightStripe.Add(id, name, Path.Combine(Settings.Default.Pictures, name + ".png"));//TODO add picture name to xsd.
		}

		public void AddBuilding(short id, short key) {
			String name = gameLogic.GameSettingsWrapper.buildingsMap[id].Name;
			//stripesManager = new StripesManager(sim, key, rightStripe,this.leftStripe);
			stripesManager.AddBuilding(id);
			stripesManager.BuildingClickedOnMap(id); //remove -- this method will be used when smb. clicks on a building -> units on menu
			leftStripe.Add(id, name, Path.Combine(Settings.Default.Pictures, name + ".png"));//TODO add picture name to xsd.
		}

		public StripesManager StripesManager {
			get { return stripesManager; }
			set { stripesManager = value; }
		}

		private void openGLView_Click(object sender, EventArgs e) {
			InfoLog.WriteInfo("MouseClick");
		}
	}
}
