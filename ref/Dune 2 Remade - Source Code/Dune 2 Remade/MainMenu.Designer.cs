namespace Dune_2_Remade
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.btnExit = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.btnHost = new System.Windows.Forms.Button();
            this.btnJoin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExit.BackgroundImage")));
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Sylfaen", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnExit.Location = new System.Drawing.Point(245, 422);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(150, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnOptions
            // 
            this.btnOptions.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOptions.BackColor = System.Drawing.Color.Transparent;
            this.btnOptions.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOptions.BackgroundImage")));
            this.btnOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOptions.FlatAppearance.BorderSize = 0;
            this.btnOptions.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOptions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOptions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOptions.Font = new System.Drawing.Font("Sylfaen", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOptions.Location = new System.Drawing.Point(245, 386);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(150, 30);
            this.btnOptions.TabIndex = 4;
            this.btnOptions.Text = "Options";
            this.btnOptions.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnOptions.UseVisualStyleBackColor = false;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // btnHost
            // 
            this.btnHost.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnHost.BackColor = System.Drawing.Color.Transparent;
            this.btnHost.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHost.BackgroundImage")));
            this.btnHost.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHost.FlatAppearance.BorderSize = 0;
            this.btnHost.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHost.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHost.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHost.Font = new System.Drawing.Font("Sylfaen", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnHost.Location = new System.Drawing.Point(245, 350);
            this.btnHost.Name = "btnHost";
            this.btnHost.Size = new System.Drawing.Size(150, 30);
            this.btnHost.TabIndex = 5;
            this.btnHost.Tag = "Host";
            this.btnHost.Text = "Host Game";
            this.btnHost.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnHost.UseVisualStyleBackColor = false;
            this.btnHost.Click += new System.EventHandler(this.OnGameLobby);
            // 
            // btnJoin
            // 
            this.btnJoin.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnJoin.BackColor = System.Drawing.Color.Transparent;
            this.btnJoin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnJoin.BackgroundImage")));
            this.btnJoin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnJoin.FlatAppearance.BorderSize = 0;
            this.btnJoin.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnJoin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnJoin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnJoin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoin.Font = new System.Drawing.Font("Sylfaen", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnJoin.Location = new System.Drawing.Point(245, 314);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(150, 30);
            this.btnJoin.TabIndex = 6;
            this.btnJoin.Tag = "Join";
            this.btnJoin.Text = "Join Game";
            this.btnJoin.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnJoin.UseVisualStyleBackColor = false;
            this.btnJoin.Click += new System.EventHandler(this.OnGameLobby);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.btnHost);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.btnExit);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainMenu";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainMenu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.Button btnHost;
        private System.Windows.Forms.Button btnJoin;

    }
}