// Stephen Toub
// stoub@microsoft.com
//
// DrumsetDemo
// Demo program that demonstrates how midi events can be played in real-time.
//
// TO USE:
// 1. Run the application.  Click on the drums with the left or right mouse button
//    (depending on the drum, different sounds might be evoked based on the button).
//    The sounds can also be played by typing the letter that appears next to the instrument.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Toub.Sound.Midi;

namespace Toub.Demos
{
	/// <summary>Demo program that demonstrates how midi events can be played in real-time.</summary>
	public class DrumsetDemo : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DrumsetDemo()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			InitializeDrumset();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DrumsetDemo));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(328, 240);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(200, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Click the drum set to hear sounds...";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(152, 200);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(16, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "B";
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(280, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(16, 16);
			this.label3.TabIndex = 3;
			this.label3.Text = "C";
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(208, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(16, 16);
			this.label4.TabIndex = 4;
			this.label4.Text = "T";
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(88, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(8, 16);
			this.label5.TabIndex = 5;
			this.label5.Text = "L";
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(80, 136);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(16, 16);
			this.label6.TabIndex = 6;
			this.label6.Text = "S";
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.Transparent;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(40, 48);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(16, 16);
			this.label7.TabIndex = 7;
			this.label7.Text = "H";
			// 
			// DrumsetDemo
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(328, 238);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label7,
																		  this.label6,
																		  this.label5,
																		  this.label4,
																		  this.label3,
																		  this.label2,
																		  this.label1,
																		  this.pictureBox1});
			this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "DrumsetDemo";
			this.Text = "Drumset";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DrumsetDemo_KeyDown);
			this.Load += new System.EventHandler(this.DrumsetDemo_Load);
			this.Closed += new System.EventHandler(this.DrumsetDemo_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new DrumsetDemo());
		}

		/* =========================================================================
		 * =========================================================================
		 * ========================================================================= */

		private void DrumsetDemo_Load(object sender, System.EventArgs e)
		{
			MidiPlayer.OpenMidi();
		}

		private void DrumsetDemo_Closed(object sender, System.EventArgs e)
		{
			MidiPlayer.CloseMidi();
		}

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;

		private Rectangle _bassDrum;
		private Rectangle _crash;
		private Rectangle _hiTom;
		private Rectangle _midTom;
		private Rectangle _snare;
		private Rectangle _hiHat;
		private Point _lastMouse;
		private Brush _boxBrush;
		private Rectangle [] _boxes;

		private void InitializeDrumset()
		{
			_bassDrum = new Rectangle(106,115,87,87);
			_crash = new Rectangle(216,3,66,35);
			_hiTom = new Rectangle(160,60,49,24);
			_midTom = new Rectangle(98,54,46,25);
			_snare = new Rectangle(50,93,55,23);
			_hiHat = new Rectangle(6,67,56,22);
			_lastMouse = Point.Empty;
			_boxBrush = new SolidBrush(Color.FromArgb(20, 255, 0, 0));
			_boxes = new Rectangle [] { _bassDrum, _crash, _hiTom, _midTom, _snare, _hiHat };
		}

		private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Point click = new Point(e.X, e.Y);
			if (_bassDrum.Contains(click))
			{
				MidiPlayer.Play(new NoteOn(0, 
					e.Button == MouseButtons.Left ? GeneralMidiPercussion.BassDrum : GeneralMidiPercussion.BassDrum1, 
					127));
			} 
			else if (_crash.Contains(click))
			{
				MidiPlayer.Play(new NoteOn(0, 
					e.Button == MouseButtons.Left ? GeneralMidiPercussion.CrashCymbal1 : GeneralMidiPercussion.CrashCymbal2, 
					127));
			} 			
			else if (_hiTom.Contains(click))
			{
				MidiPlayer.Play(new NoteOn(0,
					e.Button == MouseButtons.Left ? GeneralMidiPercussion.HighTom : GeneralMidiPercussion.HiMidTom, 
					127));
			} 			
			else if (_midTom.Contains(click))
			{
				MidiPlayer.Play(new NoteOn(0, 
					e.Button == MouseButtons.Left ? GeneralMidiPercussion.LowTom : GeneralMidiPercussion.LowMidTom, 
					127));
			} 			
			else if (_snare.Contains(click))
			{
				MidiPlayer.Play(new NoteOn(0, GeneralMidiPercussion.AcousticSnare, 127));
			} 			
			else if (_hiHat.Contains(click))
			{
				MidiPlayer.Play(new NoteOn(0, 
					e.Button == MouseButtons.Left ? GeneralMidiPercussion.OpenHiHat : GeneralMidiPercussion.ClosedHiHat, 
					127));
			} 
		}

		private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			foreach(Rectangle rect in _boxes) if (rect.Contains(_lastMouse)) e.Graphics.FillRectangle(_boxBrush, rect);
		}

		private void pictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_lastMouse = new Point(e.X, e.Y);
			Refresh();
		}

		private void DrumsetDemo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.B: MidiPlayer.Play(new NoteOn(0, GeneralMidiPercussion.BassDrum , 127)); break;
				case Keys.C: MidiPlayer.Play(new NoteOn(0, GeneralMidiPercussion.CrashCymbal1 , 127)); break;
				case Keys.T: MidiPlayer.Play(new NoteOn(0, GeneralMidiPercussion.HighTom , 127)); break;
				case Keys.L: MidiPlayer.Play(new NoteOn(0, GeneralMidiPercussion.LowTom , 127)); break;
				case Keys.S: MidiPlayer.Play(new NoteOn(0, GeneralMidiPercussion.AcousticSnare , 127)); break;
				case Keys.H: MidiPlayer.Play(new NoteOn(0, GeneralMidiPercussion.OpenHiHat , 127)); break;
			}
		}
	}
}
