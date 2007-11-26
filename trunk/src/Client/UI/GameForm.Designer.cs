using Yad.Properties.Client;
namespace Yad.UI.Client
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.panelUIMap = new System.Windows.Forms.Panel();
            this.openGLView = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.panelUITop_E = new System.Windows.Forms.Panel();
            this.panelUITop_W = new System.Windows.Forms.Panel();
            this.panelUIControl = new System.Windows.Forms.Panel();
            this.rightStripe = new Yad.UI.Client.BuildStripe();
            this.leftStripe = new Yad.UI.Client.BuildStripe();
            this.panelUIBall_W2 = new System.Windows.Forms.Panel();
            this.panelUIBall_E = new System.Windows.Forms.Panel();
            this.panelUIBall_W1 = new System.Windows.Forms.Panel();
            this.panelUIBall_WES = new System.Windows.Forms.Panel();
            this.panelUIBall_N = new System.Windows.Forms.Panel();
            this.panelUITop_MAIN = new System.Windows.Forms.Panel();
            this.creditsPictureBox = new Yad.Engine.Client.CreditsPictureBox();
            this.panelUIBall_ENS = new System.Windows.Forms.Panel();
            this.panelUILine_WE1 = new System.Windows.Forms.Panel();
            this.panelUILine_WE2 = new System.Windows.Forms.Panel();
            this.panelUILine_NS = new System.Windows.Forms.Panel();
            this.panelUIControl.SuspendLayout();
            this.panelUITop_MAIN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.creditsPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panelUIMap
            // 
            this.panelUIMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUIMap.BackColor = System.Drawing.Color.Black;
            this.panelUIMap.Location = new System.Drawing.Point(607, 373);
            this.panelUIMap.Name = "panelUIMap";
            this.panelUIMap.Size = new System.Drawing.Size(185, 193);
            this.panelUIMap.TabIndex = 2;
            // 
            // openGLView
            // 
            this.openGLView.AccumBits = ((byte)(0));
            this.openGLView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.openGLView.AutoCheckErrors = false;
            this.openGLView.AutoFinish = false;
            this.openGLView.AutoMakeCurrent = true;
            this.openGLView.AutoSwapBuffers = true;
            this.openGLView.BackColor = System.Drawing.Color.Black;
            this.openGLView.ColorBits = ((byte)(32));
            this.openGLView.DepthBits = ((byte)(32));
            this.openGLView.Location = new System.Drawing.Point(0, 64);
            this.openGLView.Name = "openGLView";
            this.openGLView.Size = new System.Drawing.Size(594, 502);
            this.openGLView.StencilBits = ((byte)(0));
            this.openGLView.TabIndex = 14;
            this.openGLView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openGLView_MouseDown);
            this.openGLView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openGLView_MouseMove);
            this.openGLView.Resize += new System.EventHandler(this.openGLView_Resize);
            this.openGLView.Paint += new System.Windows.Forms.PaintEventHandler(this.openGLView_Paint);
            this.openGLView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openGLView_MouseUp);
            this.openGLView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.openGLView_KeyDown);
            // 
            // panelUITop_E
            // 
            this.panelUITop_E.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUITop_E.BackgroundImage = global::Yad.Properties.Resources.UI_TopMenu_E;
            this.panelUITop_E.Location = new System.Drawing.Point(789, 0);
            this.panelUITop_E.Name = "panelUITop_E";
            this.panelUITop_E.Size = new System.Drawing.Size(4, 50);
            this.panelUITop_E.TabIndex = 13;
            // 
            // panelUITop_W
            // 
            this.panelUITop_W.BackgroundImage = global::Yad.Properties.Resources.UI_TopMenu_W;
            this.panelUITop_W.Location = new System.Drawing.Point(0, 0);
            this.panelUITop_W.Name = "panelUITop_W";
            this.panelUITop_W.Size = new System.Drawing.Size(4, 50);
            this.panelUITop_W.TabIndex = 12;
            // 
            // panelUIControl
            // 
            this.panelUIControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUIControl.BackgroundImage = global::Yad.Properties.Resources.UI_Background;
            this.panelUIControl.Controls.Add(this.rightStripe);
            this.panelUIControl.Controls.Add(this.leftStripe);
            this.panelUIControl.Location = new System.Drawing.Point(606, 64);
            this.panelUIControl.Name = "panelUIControl";
            this.panelUIControl.Size = new System.Drawing.Size(187, 295);
            this.panelUIControl.TabIndex = 1;
            // 
            // rightStripe
            // 
            this.rightStripe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rightStripe.BackgroundImage = global::Yad.Properties.Resources.UI_Background;
            this.rightStripe.Ids = ((System.Collections.Generic.List<int>)(resources.GetObject("rightStripe.Ids")));
            this.rightStripe.Location = new System.Drawing.Point(91, 6);
            this.rightStripe.Name = "rightStripe";
            this.rightStripe.Size = new System.Drawing.Size(90, 283);
            this.rightStripe.TabIndex = 1;
            // 
            // leftStripe
            // 
            this.leftStripe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.leftStripe.BackgroundImage = global::Yad.Properties.Resources.UI_Background;
            this.leftStripe.Ids = ((System.Collections.Generic.List<int>)(resources.GetObject("leftStripe.Ids")));
            this.leftStripe.Location = new System.Drawing.Point(0, 6);
            this.leftStripe.Name = "leftStripe";
            this.leftStripe.Size = new System.Drawing.Size(90, 283);
            this.leftStripe.TabIndex = 0;
            // 
            // panelUIBall_W2
            // 
            this.panelUIBall_W2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUIBall_W2.BackgroundImage = global::Yad.Properties.Resources.UI_Ball_W;
            this.panelUIBall_W2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelUIBall_W2.Location = new System.Drawing.Point(774, 359);
            this.panelUIBall_W2.Name = "panelUIBall_W2";
            this.panelUIBall_W2.Size = new System.Drawing.Size(18, 14);
            this.panelUIBall_W2.TabIndex = 8;
            // 
            // panelUIBall_E
            // 
            this.panelUIBall_E.BackgroundImage = global::Yad.Properties.Resources.UI_Ball_E;
            this.panelUIBall_E.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelUIBall_E.Location = new System.Drawing.Point(0, 50);
            this.panelUIBall_E.Name = "panelUIBall_E";
            this.panelUIBall_E.Size = new System.Drawing.Size(18, 14);
            this.panelUIBall_E.TabIndex = 6;
            // 
            // panelUIBall_W1
            // 
            this.panelUIBall_W1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUIBall_W1.BackgroundImage = global::Yad.Properties.Resources.UI_Ball_W;
            this.panelUIBall_W1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelUIBall_W1.Location = new System.Drawing.Point(774, 50);
            this.panelUIBall_W1.Name = "panelUIBall_W1";
            this.panelUIBall_W1.Size = new System.Drawing.Size(18, 14);
            this.panelUIBall_W1.TabIndex = 5;
            // 
            // panelUIBall_WES
            // 
            this.panelUIBall_WES.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUIBall_WES.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelUIBall_WES.BackgroundImage")));
            this.panelUIBall_WES.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelUIBall_WES.Location = new System.Drawing.Point(589, 50);
            this.panelUIBall_WES.Name = "panelUIBall_WES";
            this.panelUIBall_WES.Size = new System.Drawing.Size(22, 18);
            this.panelUIBall_WES.TabIndex = 4;
            // 
            // panelUIBall_N
            // 
            this.panelUIBall_N.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUIBall_N.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelUIBall_N.BackgroundImage")));
            this.panelUIBall_N.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelUIBall_N.Location = new System.Drawing.Point(593, 548);
            this.panelUIBall_N.Name = "panelUIBall_N";
            this.panelUIBall_N.Size = new System.Drawing.Size(14, 18);
            this.panelUIBall_N.TabIndex = 4;
            // 
            // panelUITop_MAIN
            // 
            this.panelUITop_MAIN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUITop_MAIN.BackgroundImage = global::Yad.Properties.Resources.UI_TopMenu_MAIN;
            this.panelUITop_MAIN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUITop_MAIN.Controls.Add(this.creditsPictureBox);
            this.panelUITop_MAIN.Location = new System.Drawing.Point(4, 0);
            this.panelUITop_MAIN.Name = "panelUITop_MAIN";
            this.panelUITop_MAIN.Size = new System.Drawing.Size(786, 50);
            this.panelUITop_MAIN.TabIndex = 0;
            // 
            // creditsPictureBox
            // 
            this.creditsPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.creditsPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.creditsPictureBox.BackgroundImage = global::Yad.Properties.Resources.UI_Credits_Background;
            this.creditsPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.creditsPictureBox.DestinationHeight = 25;
            this.creditsPictureBox.DigitsBitmap = global::Yad.Properties.Resources.CreditsDigits;
            this.creditsPictureBox.DrawingOffsetX = 2;
            this.creditsPictureBox.DrawingOffsetY = 2;
            this.creditsPictureBox.Location = new System.Drawing.Point(622, 7);
            this.creditsPictureBox.Name = "creditsPictureBox";
            this.creditsPictureBox.Size = new System.Drawing.Size(157, 35);
            this.creditsPictureBox.TabIndex = 0;
            this.creditsPictureBox.TabStop = false;
            this.creditsPictureBox.Value = global::Yad.Properties.Client.Settings.Default.CreditsAtStart;
            // 
            // panelUIBall_ENS
            // 
            this.panelUIBall_ENS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUIBall_ENS.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelUIBall_ENS.BackgroundImage")));
            this.panelUIBall_ENS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelUIBall_ENS.Location = new System.Drawing.Point(593, 355);
            this.panelUIBall_ENS.Name = "panelUIBall_ENS";
            this.panelUIBall_ENS.Size = new System.Drawing.Size(18, 22);
            this.panelUIBall_ENS.TabIndex = 3;
            // 
            // panelUILine_WE1
            // 
            this.panelUILine_WE1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUILine_WE1.BackgroundImage = global::Yad.Properties.Resources.UI_Line_WE;
            this.panelUILine_WE1.Location = new System.Drawing.Point(12, 50);
            this.panelUILine_WE1.Name = "panelUILine_WE1";
            this.panelUILine_WE1.Size = new System.Drawing.Size(768, 14);
            this.panelUILine_WE1.TabIndex = 9;
            // 
            // panelUILine_WE2
            // 
            this.panelUILine_WE2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUILine_WE2.BackgroundImage = global::Yad.Properties.Resources.UI_Line_WE;
            this.panelUILine_WE2.Location = new System.Drawing.Point(607, 359);
            this.panelUILine_WE2.Name = "panelUILine_WE2";
            this.panelUILine_WE2.Size = new System.Drawing.Size(173, 14);
            this.panelUILine_WE2.TabIndex = 10;
            // 
            // panelUILine_NS
            // 
            this.panelUILine_NS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUILine_NS.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelUILine_NS.BackgroundImage")));
            this.panelUILine_NS.Location = new System.Drawing.Point(593, 65);
            this.panelUILine_NS.Name = "panelUILine_NS";
            this.panelUILine_NS.Size = new System.Drawing.Size(14, 489);
            this.panelUILine_NS.TabIndex = 7;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.openGLView);
            this.Controls.Add(this.panelUITop_E);
            this.Controls.Add(this.panelUITop_W);
            this.Controls.Add(this.panelUIControl);
            this.Controls.Add(this.panelUIMap);
            this.Controls.Add(this.panelUIBall_W2);
            this.Controls.Add(this.panelUIBall_E);
            this.Controls.Add(this.panelUIBall_W1);
            this.Controls.Add(this.panelUIBall_WES);
            this.Controls.Add(this.panelUIBall_N);
            this.Controls.Add(this.panelUITop_MAIN);
            this.Controls.Add(this.panelUIBall_ENS);
            this.Controls.Add(this.panelUILine_WE1);
            this.Controls.Add(this.panelUILine_WE2);
            this.Controls.Add(this.panelUILine_NS);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YAD2 Client";
            this.panelUIControl.ResumeLayout(false);
            this.panelUITop_MAIN.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.creditsPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelUITop_MAIN;
        private System.Windows.Forms.Panel panelUITop_W;
        private System.Windows.Forms.Panel panelUIControl;
        private System.Windows.Forms.Panel panelUIMap;
        private System.Windows.Forms.Panel panelUIBall_ENS;
        private System.Windows.Forms.Panel panelUIBall_N;
        private System.Windows.Forms.Panel panelUIBall_WES;
        private System.Windows.Forms.Panel panelUIBall_W1;
        private System.Windows.Forms.Panel panelUIBall_E;
        private System.Windows.Forms.Panel panelUILine_NS;
        private System.Windows.Forms.Panel panelUIBall_W2;
        private System.Windows.Forms.Panel panelUILine_WE1;
        private System.Windows.Forms.Panel panelUILine_WE2;
        private System.Windows.Forms.Panel panelUITop_E;
        private Tao.Platform.Windows.SimpleOpenGlControl openGLView;
        private BuildStripe leftStripe;
        private BuildStripe rightStripe;
		private Yad.Engine.Client.CreditsPictureBox creditsPictureBox;
    }
}

