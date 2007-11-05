using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Client.Log;
using Client.Engine.GameGraphics;
using Tao.Platform.Windows;

namespace Client.UI {
	public partial class MainForm : UIManageable {
		bool scrolling = false;
        bool wasScrolled = false;
		Point mousePos;

		public MainForm() {
			InfoLog.WriteInfo("MainForm constructor starts", EPrefix.Menu);

			InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(MainForm_FormClosed);
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

			InfoLog.WriteInfo("MainForm constructor: initializing OpenGL", EPrefix.GameGraphics);

			//initializes GameGraphics
			this.openGLView.InitializeContexts();

			GameGraphics gg = GameGraphics.GetInstance();
			//First: set appropriate properties
			gg.SetMapSize(32, 64); //TODO: change
			gg.SetViewSize(openGLView.Width, openGLView.Height);
			gg.InitGL();
			gg.InitTextures();

			InfoLog.WriteInfo("MainForm constructor: initializing OpenGL finished", EPrefix.GameGraphics);
			
			gg.GameGraphicsChanged += new EventHandler(gg_GameGraphicsChanged);

			this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
		}

        void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            OnOptionChoosen(MenuOption.Options);
            e.Cancel = true;
        }

        void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            OnOptionChoosen(MenuOption.Options);
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
			} else if (e.KeyCode == Keys.S) {
			} else if (e.KeyCode == Keys.A) {
			} else if (e.KeyCode == Keys.D) {
			}
		}

		private void openGLView_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				mousePos = e.Location;
				scrolling = true;
			}
		}

		private void openGLView_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				scrolling = false;
			}
		}

		private void openGLView_MouseMove(object sender, MouseEventArgs e) {
			GameGraphics gg = GameGraphics.GetInstance();

            if (wasScrolled) {
                wasScrolled = false;
                return;
            }

            if (scrolling) {
				int dx = e.X - mousePos.X;
                int dy = e.Y - mousePos.Y;

				gg.TranslateX(-dx * 0.05f);
				gg.TranslateY(dy * 0.05f); //opengl uses different coordinate system

                wasScrolled = true;
				Cursor.Position = openGLView.PointToScreen(mousePos);
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

    public class newOpengl : SimpleOpenGlControl {
        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
        }
    }
}