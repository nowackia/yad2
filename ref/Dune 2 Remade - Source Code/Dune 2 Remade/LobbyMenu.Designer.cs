namespace Dune_2_Remade
{
    partial class LobbyMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LobbyMenu));
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnJoin = new System.Windows.Forms.Button();
            this.lstPlayers = new System.Windows.Forms.ListBox();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.txtChatInput = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.pnlJoin = new System.Windows.Forms.Panel();
            this.pnlHost = new System.Windows.Forms.Panel();
            this.btnAtreides = new System.Windows.Forms.Button();
            this.btnHarkonnen = new System.Windows.Forms.Button();
            this.btnOrdos = new System.Windows.Forms.Button();
            this.pnlJoin.SuspendLayout();
            this.pnlHost.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(4, 4);
            this.txtIP.Margin = new System.Windows.Forms.Padding(4);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(133, 27);
            this.txtIP.TabIndex = 0;
            this.txtIP.Text = "127.0.0.1";
            // 
            // btnJoin
            // 
            this.btnJoin.BackColor = System.Drawing.Color.Transparent;
            this.btnJoin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnJoin.BackgroundImage")));
            this.btnJoin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnJoin.FlatAppearance.BorderSize = 0;
            this.btnJoin.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnJoin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnJoin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnJoin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoin.Location = new System.Drawing.Point(145, 4);
            this.btnJoin.Margin = new System.Windows.Forms.Padding(4);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(112, 32);
            this.btnJoin.TabIndex = 1;
            this.btnJoin.Text = "Join";
            this.btnJoin.UseVisualStyleBackColor = false;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // lstPlayers
            // 
            this.lstPlayers.FormattingEnabled = true;
            this.lstPlayers.ItemHeight = 19;
            this.lstPlayers.Location = new System.Drawing.Point(418, 13);
            this.lstPlayers.Margin = new System.Windows.Forms.Padding(4);
            this.lstPlayers.Name = "lstPlayers";
            this.lstPlayers.Size = new System.Drawing.Size(209, 213);
            this.lstPlayers.TabIndex = 2;
            // 
            // txtChat
            // 
            this.txtChat.BackColor = System.Drawing.Color.White;
            this.txtChat.Location = new System.Drawing.Point(13, 13);
            this.txtChat.Margin = new System.Windows.Forms.Padding(4);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.ReadOnly = true;
            this.txtChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChat.Size = new System.Drawing.Size(397, 290);
            this.txtChat.TabIndex = 3;
            // 
            // txtChatInput
            // 
            this.txtChatInput.AcceptsReturn = true;
            this.txtChatInput.Location = new System.Drawing.Point(13, 308);
            this.txtChatInput.Margin = new System.Windows.Forms.Padding(4);
            this.txtChatInput.Name = "txtChatInput";
            this.txtChatInput.Size = new System.Drawing.Size(397, 27);
            this.txtChatInput.TabIndex = 4;
            this.txtChatInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtChatInput_KeyPress);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Dune_2_Remade.Properties.Resources.UI_MessageBox;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(418, 234);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(209, 32);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.BackColor = System.Drawing.Color.Transparent;
            this.btnDisconnect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDisconnect.BackgroundImage")));
            this.btnDisconnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.FlatAppearance.BorderSize = 0;
            this.btnDisconnect.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnDisconnect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDisconnect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisconnect.Location = new System.Drawing.Point(145, 37);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(112, 32);
            this.btnDisconnect.TabIndex = 6;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = false;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackgroundImage = global::Dune_2_Remade.Properties.Resources.UI_MessageBox;
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point(0, 0);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(209, 29);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start Game";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // pnlJoin
            // 
            this.pnlJoin.BackColor = System.Drawing.Color.Transparent;
            this.pnlJoin.Controls.Add(this.txtIP);
            this.pnlJoin.Controls.Add(this.btnJoin);
            this.pnlJoin.Controls.Add(this.btnDisconnect);
            this.pnlJoin.Location = new System.Drawing.Point(362, 391);
            this.pnlJoin.Margin = new System.Windows.Forms.Padding(4);
            this.pnlJoin.Name = "pnlJoin";
            this.pnlJoin.Size = new System.Drawing.Size(265, 76);
            this.pnlJoin.TabIndex = 8;
            // 
            // pnlHost
            // 
            this.pnlHost.BackColor = System.Drawing.Color.Transparent;
            this.pnlHost.Controls.Add(this.btnStart);
            this.pnlHost.Location = new System.Drawing.Point(418, 274);
            this.pnlHost.Margin = new System.Windows.Forms.Padding(4);
            this.pnlHost.Name = "pnlHost";
            this.pnlHost.Size = new System.Drawing.Size(209, 29);
            this.pnlHost.TabIndex = 9;
            // 
            // btnAtreides
            // 
            this.btnAtreides.BackColor = System.Drawing.Color.Transparent;
            this.btnAtreides.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAtreides.BackgroundImage")));
            this.btnAtreides.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAtreides.FlatAppearance.BorderSize = 0;
            this.btnAtreides.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAtreides.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAtreides.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAtreides.Location = new System.Drawing.Point(13, 344);
            this.btnAtreides.Name = "btnAtreides";
            this.btnAtreides.Size = new System.Drawing.Size(111, 123);
            this.btnAtreides.TabIndex = 11;
            this.btnAtreides.Tag = "Atreides";
            this.btnAtreides.UseVisualStyleBackColor = false;
            this.btnAtreides.Click += new System.EventHandler(this.OnButtonHouseClicked);
            // 
            // btnHarkonnen
            // 
            this.btnHarkonnen.BackColor = System.Drawing.Color.Transparent;
            this.btnHarkonnen.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHarkonnen.BackgroundImage")));
            this.btnHarkonnen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnHarkonnen.FlatAppearance.BorderSize = 0;
            this.btnHarkonnen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHarkonnen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHarkonnen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHarkonnen.Location = new System.Drawing.Point(130, 344);
            this.btnHarkonnen.Name = "btnHarkonnen";
            this.btnHarkonnen.Size = new System.Drawing.Size(111, 123);
            this.btnHarkonnen.TabIndex = 12;
            this.btnHarkonnen.Tag = "Harkonnen";
            this.btnHarkonnen.UseVisualStyleBackColor = false;
            this.btnHarkonnen.Click += new System.EventHandler(this.OnButtonHouseClicked);
            // 
            // btnOrdos
            // 
            this.btnOrdos.BackColor = System.Drawing.Color.Transparent;
            this.btnOrdos.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOrdos.BackgroundImage")));
            this.btnOrdos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOrdos.FlatAppearance.BorderSize = 0;
            this.btnOrdos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOrdos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOrdos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrdos.Location = new System.Drawing.Point(244, 344);
            this.btnOrdos.Name = "btnOrdos";
            this.btnOrdos.Size = new System.Drawing.Size(111, 123);
            this.btnOrdos.TabIndex = 13;
            this.btnOrdos.Tag = "Ordos";
            this.btnOrdos.UseVisualStyleBackColor = false;
            this.btnOrdos.Click += new System.EventHandler(this.OnButtonHouseClicked);
            // 
            // LobbyMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.ControlBox = false;
            this.Controls.Add(this.btnOrdos);
            this.Controls.Add(this.btnHarkonnen);
            this.Controls.Add(this.btnAtreides);
            this.Controls.Add(this.pnlHost);
            this.Controls.Add(this.pnlJoin);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtChatInput);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.lstPlayers);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Sylfaen", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LobbyMenu";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Lobby";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LobbyMenu_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LobbyMenu_FormClosing);
            this.pnlJoin.ResumeLayout(false);
            this.pnlJoin.PerformLayout();
            this.pnlHost.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.ListBox lstPlayers;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.TextBox txtChatInput;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel pnlJoin;
        private System.Windows.Forms.Panel pnlHost;
        private System.Windows.Forms.Button btnAtreides;
        private System.Windows.Forms.Button btnHarkonnen;
        private System.Windows.Forms.Button btnOrdos;
    }
}