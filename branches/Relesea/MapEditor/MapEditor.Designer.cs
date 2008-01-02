namespace Yad.MapEditor
{
    partial class MapEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditor));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.rockButton = new System.Windows.Forms.ToolStripButton();
            this.sandButton = new System.Windows.Forms.ToolStripButton();
            this.mountainButton = new System.Windows.Forms.ToolStripButton();
            this.btnStartPoint = new System.Windows.Forms.ToolStripButton();
            this.btnThinSpice = new System.Windows.Forms.ToolStripButton();
            this.btnThickSpice = new System.Windows.Forms.ToolStripButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(563, 24);
            this.menuStrip.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.rockButton,
            this.sandButton,
            this.mountainButton,
            this.btnStartPoint,
            this.btnThinSpice,
            this.btnThickSpice});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(563, 38);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(51, 35);
            this.toolStripLabel1.Text = "Brushes:";
            // 
            // rockButton
            // 
            this.rockButton.Image = ((System.Drawing.Image)(resources.GetObject("rockButton.Image")));
            this.rockButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rockButton.Name = "rockButton";
            this.rockButton.Size = new System.Drawing.Size(37, 35);
            this.rockButton.Text = "Rock";
            this.rockButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rockButton.Click += new System.EventHandler(this.terrainButtonClicked);
            // 
            // sandButton
            // 
            this.sandButton.Image = ((System.Drawing.Image)(resources.GetObject("sandButton.Image")));
            this.sandButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sandButton.Name = "sandButton";
            this.sandButton.Size = new System.Drawing.Size(37, 35);
            this.sandButton.Text = "Sand";
            this.sandButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.sandButton.Click += new System.EventHandler(this.terrainButtonClicked);
            // 
            // mountainButton
            // 
            this.mountainButton.Image = ((System.Drawing.Image)(resources.GetObject("mountainButton.Image")));
            this.mountainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mountainButton.Name = "mountainButton";
            this.mountainButton.Size = new System.Drawing.Size(63, 35);
            this.mountainButton.Text = "Mountain";
            this.mountainButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.mountainButton.Click += new System.EventHandler(this.terrainButtonClicked);
            // 
            // btnStartPoint
            // 
            this.btnStartPoint.CheckOnClick = true;
            this.btnStartPoint.Image = ((System.Drawing.Image)(resources.GetObject("btnStartPoint.Image")));
            this.btnStartPoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStartPoint.Name = "btnStartPoint";
            this.btnStartPoint.Size = new System.Drawing.Size(66, 35);
            this.btnStartPoint.Text = "Start point";
            this.btnStartPoint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnStartPoint.Click += new System.EventHandler(this.btnStartPoint_Click);
            // 
            // btnThinSpice
            // 
            this.btnThinSpice.Image = ((System.Drawing.Image)(resources.GetObject("btnThinSpice.Image")));
            this.btnThinSpice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnThinSpice.Name = "btnThinSpice";
            this.btnThinSpice.Size = new System.Drawing.Size(66, 35);
            this.btnThinSpice.Text = "Thin Spice";
            this.btnThinSpice.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnThinSpice.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // btnThickSpice
            // 
            this.btnThickSpice.Image = ((System.Drawing.Image)(resources.GetObject("btnThickSpice.Image")));
            this.btnThickSpice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnThickSpice.Name = "btnThickSpice";
            this.btnThickSpice.Size = new System.Drawing.Size(71, 35);
            this.btnThickSpice.Text = "Thick Spice";
            this.btnThickSpice.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnThickSpice.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(235, 224);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(563, 405);
            this.panel1.TabIndex = 3;

            // 
            // MapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 467);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MapEditor";
            this.Text = "FormMapEditor";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MapEditor_KeyPress);
            this.Load += new System.EventHandler(this.MapEditor_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton rockButton;
        private System.Windows.Forms.ToolStripButton sandButton;
        private System.Windows.Forms.ToolStripButton mountainButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton btnStartPoint;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton btnThinSpice;
        private System.Windows.Forms.ToolStripButton btnThickSpice;
    }
}

