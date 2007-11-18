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

		bool scrolling = false;
		bool wasScrolled = false;
		Point mousePos;

		public static ClientSimulation sim;
		public static Player currPlayer;
		public static IConnection conn;
        public static StripesManager stripesManager;

        #region Properties
        public static ClientSimulation ClientSimulation
        {
            get
            { return sim; }
            set
            { sim = value; }
        }

        public static Player Player
        {
            get
            { return currPlayer; }
            set
            { currPlayer = value; }
        }

        public static IConnection Connection
        {
            get
            { return conn; }
            set
            { conn = value; }
        }

        public static StripesManager StripesManager
        {
            get
            { return stripesManager; }
            set
            { stripesManager = value; }
        }
        #endregion



		public GameForm() {
			InfoLog.WriteInfo("MainForm constructor starts", EPrefix.Menu);

			InitializeComponent();

			//TODO: use real connection
			conn = new DummyConnection();

			GameSettingsWrapper gameSettingsWrapper = XMLLoader.get(Settings.Default.ConfigFile, Settings.Default.ConfigFileXSD);
			Map map = new Map();
			map.LoadMap(Path.Combine(Settings.Default.Maps, "test.map"));
			sim = new ClientSimulation(gameSettingsWrapper, map, conn);
            
            short key=0;
            //TODO wtf? which race i am?
            foreach (short k in gameSettingsWrapper.racesMap.Keys)
	        {
                key = k;
                break;
	        }
            stripesManager = new StripesManager(sim, key, rightStripe, this.leftStripe);
			GameLogic.AddBuildingEvent += new GameLogic.AddBuildingDelegate(AddBuilding);

			

            //^to remove

			//to remove
			currPlayer = new Player(0);
			sim.AddPlayer(currPlayer);
			CreateUnitMessage cum = new CreateUnitMessage();
			cum.IdTurn = sim.CurrentTurn + sim.Delta;
			cum.PlayerId = currPlayer.ID;
			cum.UnitID = currPlayer.GenerateObjectID();
			cum.UnitType = sim.GameSettingsWrapper.GameSettings.UnitTanksData.UnitTankDataCollection[0].TypeID;
			cum.UnitKind = BoardObjectClass.UnitTank;
			cum.Position = new Yad.Board.Position(Randomizer.NextShort(sim.Map.Width), Randomizer.NextShort(sim.Map.Height));
			sim.AddGameMessage(cum);

			CreateUnitMessage cum1 = new CreateUnitMessage();
			cum1.IdTurn = sim.CurrentTurn + sim.Delta + sim.Delta - 1;
			cum1.PlayerId = currPlayer.ID;
			cum1.UnitID = currPlayer.GenerateObjectID();
			cum1.UnitType = sim.GameSettingsWrapper.GameSettings.UnitTroopersData.UnitTrooperDataCollection[0].TypeID;
			cum1.UnitKind = BoardObjectClass.UnitTrooper;
			cum1.Position = new Yad.Board.Position(Randomizer.NextShort(sim.Map.Width), Randomizer.NextShort(sim.Map.Height));
			sim.AddGameMessage(cum1);
			//to remove end
			GameLogic.InitStripes("ConstructionYard", key);
			this.FormClosed += new FormClosedEventHandler(MainForm_FormClosed);
			this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

			InfoLog.WriteInfo("MainForm constructor: initializing OpenGL", EPrefix.GameGraphics);

			//initializes GameGraphics
			this.openGLView.InitializeContexts();

			//First: set appropriate properties
			GameGraphics.InitGL(sim);
			GameGraphics.SetViewSize(openGLView.Width, openGLView.Height);
			GameGraphics.InitTextures(sim);

			InfoLog.WriteInfo("MainForm constructor: initializing OpenGL finished", EPrefix.GameGraphics);

			GameGraphics.GameGraphicsChanged += new EventHandler(gg_GameGraphicsChanged);

			this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);

			//TODO: start on game message
			sim.StartSimulation();
		}

		void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			OnMenuOptionChange(MenuOption.Options);
			e.Cancel = true;
		}

		void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
			OnMenuOptionChange(MenuOption.Options);
		}

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
			} else if (e.KeyCode == Keys.F5) {
				sim.DoTurn(); //todo: erase later
			}
		}

		public bool IsStripContainingBuilding(short ids)
		{
			return StripesManager.ContainsId(ids);
		}

		private void openGLView_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				mousePos = e.Location;
				scrolling = true;
			} else if (e.Button == MouseButtons.Left) {
				GameLogic.MouseLeftClick(this, e);
				//TODO END
			}
		}

		private void openGLView_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				scrolling = false;
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

		private class DummyConnection : IConnection {

			#region IConnection Members
			int currentTurn = 0;
			public void SendMessage(Yad.Net.Messaging.Common.Message message) {
				if (message.Type == MessageType.TurnAsk) {
					currentTurn++;
					GameForm.sim.DoTurn();
				} else if (message is GameMessage) {
					GameMessage gm = message as GameMessage;
					gm.IdTurn = currentTurn + GameForm.sim.Delta;
					GameForm.sim.AddGameMessage(gm);
				}
			}

			public void CloseConnection() {
				
			}

			public void InitConnection(string host, int port) {
				
			}

			#endregion
		}

		internal void addUnitCreationPossibility(string s)
		{
			
		}

		public void AddBuilding(short id, short key)
		{
			String name = GameForm.sim.GameSettingsWrapper.buildingsMap[id].Name;
			//stripesManager = new StripesManager(sim, key, rightStripe,this.leftStripe);
			stripesManager.AddBuilding(id);
			stripesManager.BuildingClickedOnMap(id); //remove -- this method will be used when smb. clicks on a building -> units on menu
			leftStripe.Add(id, name, Path.Combine(Settings.Default.Pictures, name + ".png"));//TODO add picture name to xsd.
		}
	}
}
