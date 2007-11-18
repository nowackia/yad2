namespace KaleeTests {
	partial class TestForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbWin = new System.Windows.Forms.RadioButton();
			this.rbLose = new System.Windows.Forms.RadioButton();
			this.rbPeace = new System.Windows.Forms.RadioButton();
			this.rbFight = new System.Windows.Forms.RadioButton();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnRandom = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbWin);
			this.groupBox1.Controls.Add(this.rbLose);
			this.groupBox1.Controls.Add(this.rbPeace);
			this.groupBox1.Controls.Add(this.rbFight);
			this.groupBox1.Controls.Add(this.btnNext);
			this.groupBox1.Controls.Add(this.btnRandom);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(135, 118);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Audio";
			// 
			// rbWin
			// 
			this.rbWin.AutoSize = true;
			this.rbWin.Location = new System.Drawing.Point(70, 88);
			this.rbWin.Name = "rbWin";
			this.rbWin.Size = new System.Drawing.Size(44, 17);
			this.rbWin.TabIndex = 5;
			this.rbWin.Text = "Win";
			this.rbWin.UseVisualStyleBackColor = true;
			// 
			// rbLose
			// 
			this.rbLose.AutoSize = true;
			this.rbLose.Location = new System.Drawing.Point(70, 65);
			this.rbLose.Name = "rbLose";
			this.rbLose.Size = new System.Drawing.Size(48, 17);
			this.rbLose.TabIndex = 4;
			this.rbLose.Text = "Lose";
			this.rbLose.UseVisualStyleBackColor = true;
			// 
			// rbPeace
			// 
			this.rbPeace.AutoSize = true;
			this.rbPeace.Location = new System.Drawing.Point(70, 42);
			this.rbPeace.Name = "rbPeace";
			this.rbPeace.Size = new System.Drawing.Size(56, 17);
			this.rbPeace.TabIndex = 3;
			this.rbPeace.Text = "Peace";
			this.rbPeace.UseVisualStyleBackColor = true;
			// 
			// rbFight
			// 
			this.rbFight.AutoSize = true;
			this.rbFight.Checked = true;
			this.rbFight.Location = new System.Drawing.Point(70, 19);
			this.rbFight.Name = "rbFight";
			this.rbFight.Size = new System.Drawing.Size(48, 17);
			this.rbFight.TabIndex = 2;
			this.rbFight.TabStop = true;
			this.rbFight.Text = "Fight";
			this.rbFight.UseVisualStyleBackColor = true;
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(6, 50);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(58, 22);
			this.btnNext.TabIndex = 1;
			this.btnNext.Text = "Next";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnRandom
			// 
			this.btnRandom.Location = new System.Drawing.Point(6, 19);
			this.btnRandom.Name = "btnRandom";
			this.btnRandom.Size = new System.Drawing.Size(58, 25);
			this.btnRandom.TabIndex = 0;
			this.btnRandom.Text = "Random";
			this.btnRandom.UseVisualStyleBackColor = true;
			this.btnRandom.Click += new System.EventHandler(this.btnRandom_Click);
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(618, 417);
			this.Controls.Add(this.groupBox1);
			this.Name = "TestForm";
			this.Text = "Test Form";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbWin;
		private System.Windows.Forms.RadioButton rbLose;
		private System.Windows.Forms.RadioButton rbPeace;
		private System.Windows.Forms.RadioButton rbFight;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnRandom;
	}
}

