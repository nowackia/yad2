using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Client.Log;
using Client.Engine.GameGraphics;

namespace Client.UI {
	public partial class MainForm : Form {
		bool scrolling = false,
			rotating = false;
		Point mousePos;

		public MainForm() {
			InfoLog.WriteInfo("MainForm constructor starts", EPrefix.Menu);

			InitializeComponent();

			InfoLog.WriteInfo("MainForm constructor: initializing OpenGL", EPrefix.GameGraphics);

			//initializes GameGraphics
			this.openGLView.InitializeContexts();

			GameGraphics gg = GameGraphics.GetInstance();
			//First: set appropriate properties
			gg.SetMapSize(16, 16); //TODO: change
			gg.SetViewSize(openGLView.Width, openGLView.Height);
			gg.InitGL();
			gg.InitTextures();

			InfoLog.WriteInfo("MainForm constructor: initializing OpenGL finished", EPrefix.GameGraphics);
			
			gg.GameGraphicsChanged += new EventHandler(gg_GameGraphicsChanged);

			this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
		}

		void gg_GameGraphicsChanged(object sender, EventArgs e) {
			this.openGLView.Invalidate();
		}

		void MainForm_MouseWheel(object sender, MouseEventArgs e) {
			GameGraphics gg = GameGraphics.GetInstance();
			gg.Zoom(e.Delta / 120);
		}

		private void openGLView_KeyDown(object sender, KeyEventArgs e) {
			GameGraphics gg = GameGraphics.GetInstance();

			if (e.KeyCode == Keys.W) {
				gg.RotateX(-2);
			} else if (e.KeyCode == Keys.S) {
				gg.RotateX(2);
			} else if (e.KeyCode == Keys.A) {
				gg.RotateY(2);
			} else if (e.KeyCode == Keys.D) {
				gg.RotateY(-2);
			}
		}

		private void openGLView_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				mousePos = e.Location;
				scrolling = true;
			} else if (e.Button == MouseButtons.Middle) {
				mousePos = e.Location;
				rotating = true;
			}
		}

		private void openGLView_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				scrolling = false;
			} else if (e.Button == MouseButtons.Middle) {
				rotating = false;
			}
		}

		private void openGLView_MouseMove(object sender, MouseEventArgs e) {
			GameGraphics gg = GameGraphics.GetInstance();

			if (scrolling) {
				int dx = e.X - mousePos.X;
				int dy = e.Y - mousePos.Y;

				InfoLog.WriteInfo("Translating: " + dx + " " + dy , EPrefix.GameGraphics);

				gg.TranslateX(dx * 0.2f);
				gg.TranslateY(dy * 0.2f);

				mousePos = e.Location;
			} else if (rotating) {
				int dx = e.X - mousePos.X;
				int dy = e.Y - mousePos.Y;

				InfoLog.WriteInfo("Rotating: " + dx + " " + dy, EPrefix.GameGraphics);

				gg.RotateX(dy * 0.1f);
				gg.RotateY(dx * 0.1f);

				mousePos = e.Location;
			}
		}

		private void openGLView_Paint(object sender, PaintEventArgs e) {
			GameGraphics gg = GameGraphics.GetInstance();
			gg.Draw();
		}

		private void openGLView_Resize(object sender, EventArgs e) {
			Console.Out.WriteLine("Resizing...");

			GameGraphics gg = GameGraphics.GetInstance();
			gg.SetViewSize(openGLView.Width, openGLView.Height);
		}
	}
}