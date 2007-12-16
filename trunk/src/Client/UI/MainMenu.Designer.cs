using System.Windows.Forms;

namespace Yad.UI.Client
{
    partial class MainMenuForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenuForm));
            this.tabControl = new Yad.UI.MenuTabControl();
            this.mainMenu = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.newGameMainMenu = new System.Windows.Forms.Button();
            this.optionsMainMenu = new System.Windows.Forms.Button();
            this.exitMainMenu = new System.Windows.Forms.Button();
            this.creditsMainMenu = new System.Windows.Forms.Button();
            this.loginMenu = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxServer = new System.Windows.Forms.GroupBox();
            this.serverLoginMenu = new System.Windows.Forms.ComboBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.groupBoxLogin = new System.Windows.Forms.GroupBox();
            this.loginLabel = new System.Windows.Forms.Label();
            this.loginTBLoginMenu = new System.Windows.Forms.TextBox();
            this.passwordLoginMenu = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.registerLoginMenu = new System.Windows.Forms.Button();
            this.loginBTLoginMenu = new System.Windows.Forms.Button();
            this.remindPasswordLoginMenu = new System.Windows.Forms.Button();
            this.cancelLoginMenu = new System.Windows.Forms.Button();
            this.registerMenu = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.loginLBRegisterMenu = new System.Windows.Forms.Label();
            this.emailLRegisterMenu = new System.Windows.Forms.Label();
            this.loginTBRegisterMenu = new System.Windows.Forms.TextBox();
            this.repeatPasswordLRegisterMenu = new System.Windows.Forms.Label();
            this.passwordTBRegisterMenu = new System.Windows.Forms.TextBox();
            this.emailTBRegisterMenu = new System.Windows.Forms.TextBox();
            this.repeatPasswordTBRegisterMenu = new System.Windows.Forms.TextBox();
            this.passwordLRegisterMenu = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.registerRegisterMenu = new System.Windows.Forms.Button();
            this.backRegisterMenu = new System.Windows.Forms.Button();
            this.chatMenu = new System.Windows.Forms.TabPage();
            this.sendChatMenu = new System.Windows.Forms.Button();
            this.userListChatMenu = new System.Windows.Forms.ListBox();
            this.chatListChatMenu = new System.Windows.Forms.ListBox();
            this.chatInputTBChatMenu = new System.Windows.Forms.TextBox();
            this.backChatMenu = new System.Windows.Forms.Button();
            this.gameChatMenu = new System.Windows.Forms.Button();
            this.playerInfoMenu = new System.Windows.Forms.TabPage();
            this.playerInfoLInfoMenu = new System.Windows.Forms.Label();
            this.backInfoMenu = new System.Windows.Forms.Button();
            this.chooseGameMenu = new System.Windows.Forms.TabPage();
            this.textBoxTBGameName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxTBGameDescription = new System.Windows.Forms.TextBox();
            this.backChooseGameMenu = new System.Windows.Forms.Button();
            this.listOfGames = new System.Windows.Forms.ListBox();
            this.createChooseGameMenu = new System.Windows.Forms.Button();
            this.joinChooseGameMenu = new System.Windows.Forms.Button();
            this.createGameMenu = new System.Windows.Forms.TabPage();
            this.maxPlayerNumberNUPCreateGameMenu = new System.Windows.Forms.NumericUpDown();
            this.maxPlayerNumberLCreateGameMenu = new System.Windows.Forms.Label();
            this.gameNameLCreateGameMenu = new System.Windows.Forms.Label();
            this.gameNameTBCreateGameMenu = new System.Windows.Forms.TextBox();
            this.privateCreateGameMenu = new System.Windows.Forms.RadioButton();
            this.publicCreateGameMenu = new System.Windows.Forms.RadioButton();
            this.mapsLCreateGameMenu = new System.Windows.Forms.Label();
            this.listBoxLBCreateGame = new System.Windows.Forms.ListBox();
            this.cancelCreateGameMenu = new System.Windows.Forms.Button();
            this.createCreateGameMenu = new System.Windows.Forms.Button();
            this.waitingForPlayersMenu = new System.Windows.Forms.TabPage();
            this.currentDataGBWaitingForPlayersMenu = new System.Windows.Forms.GroupBox();
            this.pictureBoxHouse = new System.Windows.Forms.PictureBox();
            this.infoColorWaitingForPlayersMenu = new System.Windows.Forms.Button();
            this.infoColorLWaitingForPlayersMenu = new System.Windows.Forms.Label();
            this.infoLWaitingForPlayersMenu = new System.Windows.Forms.Label();
            this.changeGBWaitingForPlayersMenu = new System.Windows.Forms.GroupBox();
            this.colorLWaitingForPlayersMenu = new System.Windows.Forms.Label();
            this.changeWaitingForPlayersMenu = new System.Windows.Forms.Button();
            this.colorWaitingForPlayersMenu = new System.Windows.Forms.Button();
            this.houseLWaitingForPlayersMenu = new System.Windows.Forms.Label();
            this.houseCBWaitingForPlayersMenu = new System.Windows.Forms.ComboBox();
            this.teamCBWaitingForPlayersMenu = new System.Windows.Forms.ComboBox();
            this.teamLWaitingForPlayersMenu = new System.Windows.Forms.Label();
            this.dataGridViewPlayers = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlayersName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.House = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Team = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionLWaitingForPlayersMenu = new System.Windows.Forms.Label();
            this.descriptionWaitingForPlayersMenu = new System.Windows.Forms.TextBox();
            this.playersLWaitingForPlayersMenu = new System.Windows.Forms.Label();
            this.startWaitingForPlayersMenu = new System.Windows.Forms.Button();
            this.cancelWaitingForPlayersMenu = new System.Windows.Forms.Button();
            this.optionsMenu = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.musicVolumeLOptionsMenu = new System.Windows.Forms.Label();
            this.soundVolumeLOptionsMenu = new System.Windows.Forms.Label();
            this.musicVolumeNMOptionsMenu = new System.Windows.Forms.NumericUpDown();
            this.muteSoundOptionsMenu = new System.Windows.Forms.CheckBox();
            this.soundVolumeNMOptionsMenu = new System.Windows.Forms.NumericUpDown();
            this.muteMusicOptionsMenu = new System.Windows.Forms.CheckBox();
            this.okOptionsMenu = new System.Windows.Forms.Button();
            this.cancelOptionsMenu = new System.Windows.Forms.Button();
            this.gameMenu = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pauseGameMenu = new System.Windows.Forms.Button();
            this.exitGameMenu = new System.Windows.Forms.Button();
            this.optionsGameMenu = new System.Windows.Forms.Button();
            this.okGameMenu = new System.Windows.Forms.Button();
            this.pauseMenu = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.continuePauseMenu = new System.Windows.Forms.Button();
            this.exitPauseMenu = new System.Windows.Forms.Button();
            this.optionsPauseMenu = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabControl.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.loginMenu.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBoxServer.SuspendLayout();
            this.groupBoxLogin.SuspendLayout();
            this.panel1.SuspendLayout();
            this.registerMenu.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.chatMenu.SuspendLayout();
            this.playerInfoMenu.SuspendLayout();
            this.chooseGameMenu.SuspendLayout();
            this.createGameMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxPlayerNumberNUPCreateGameMenu)).BeginInit();
            this.waitingForPlayersMenu.SuspendLayout();
            this.currentDataGBWaitingForPlayersMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHouse)).BeginInit();
            this.changeGBWaitingForPlayersMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlayers)).BeginInit();
            this.optionsMenu.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.musicVolumeNMOptionsMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.soundVolumeNMOptionsMenu)).BeginInit();
            this.gameMenu.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pauseMenu.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl.Controls.Add(this.mainMenu);
            this.tabControl.Controls.Add(this.loginMenu);
            this.tabControl.Controls.Add(this.registerMenu);
            this.tabControl.Controls.Add(this.chatMenu);
            this.tabControl.Controls.Add(this.playerInfoMenu);
            this.tabControl.Controls.Add(this.chooseGameMenu);
            this.tabControl.Controls.Add(this.createGameMenu);
            this.tabControl.Controls.Add(this.waitingForPlayersMenu);
            this.tabControl.Controls.Add(this.optionsMenu);
            this.tabControl.Controls.Add(this.gameMenu);
            this.tabControl.Controls.Add(this.pauseMenu);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(535, 382);
            this.tabControl.TabIndex = 25;
            // 
            // mainMenu
            // 
            this.mainMenu.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.mainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.mainMenu.Controls.Add(this.tableLayoutPanel1);
            this.mainMenu.Location = new System.Drawing.Point(4, 4);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(3);
            this.mainMenu.Size = new System.Drawing.Size(527, 338);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "MainMenu";
            this.mainMenu.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.42802F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.29559F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.27639F));
            this.tableLayoutPanel1.Controls.Add(this.newGameMainMenu, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.optionsMainMenu, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.exitMainMenu, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.creditsMainMenu, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(521, 332);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // newGameMainMenu
            // 
            this.newGameMainMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.newGameMainMenu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("newGameMainMenu.BackgroundImage")));
            this.newGameMainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.newGameMainMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newGameMainMenu.Font = new System.Drawing.Font("Bell MT", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newGameMainMenu.ForeColor = System.Drawing.Color.Maroon;
            this.newGameMainMenu.Location = new System.Drawing.Point(197, 83);
            this.newGameMainMenu.Name = "newGameMainMenu";
            this.newGameMainMenu.Size = new System.Drawing.Size(131, 23);
            this.newGameMainMenu.TabIndex = 1;
            this.newGameMainMenu.Text = "New Game";
            this.newGameMainMenu.UseVisualStyleBackColor = true;
            this.newGameMainMenu.Click += new System.EventHandler(this.NewGameMainMenu_Click);
            // 
            // optionsMainMenu
            // 
            this.optionsMainMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsMainMenu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("optionsMainMenu.BackgroundImage")));
            this.optionsMainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.optionsMainMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optionsMainMenu.Font = new System.Drawing.Font("Bell MT", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsMainMenu.ForeColor = System.Drawing.Color.Maroon;
            this.optionsMainMenu.Location = new System.Drawing.Point(197, 112);
            this.optionsMainMenu.Name = "optionsMainMenu";
            this.optionsMainMenu.Size = new System.Drawing.Size(131, 23);
            this.optionsMainMenu.TabIndex = 2;
            this.optionsMainMenu.Text = "Options";
            this.optionsMainMenu.UseVisualStyleBackColor = true;
            this.optionsMainMenu.Click += new System.EventHandler(this.OptionsMainMenu_Click);
            // 
            // exitMainMenu
            // 
            this.exitMainMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.exitMainMenu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("exitMainMenu.BackgroundImage")));
            this.exitMainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.exitMainMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitMainMenu.Font = new System.Drawing.Font("Bell MT", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitMainMenu.ForeColor = System.Drawing.Color.Maroon;
            this.exitMainMenu.Location = new System.Drawing.Point(197, 170);
            this.exitMainMenu.Name = "exitMainMenu";
            this.exitMainMenu.Size = new System.Drawing.Size(131, 23);
            this.exitMainMenu.TabIndex = 4;
            this.exitMainMenu.Text = "Exit";
            this.exitMainMenu.UseVisualStyleBackColor = true;
            this.exitMainMenu.Click += new System.EventHandler(this.exitMainMenu_Click);
            // 
            // creditsMainMenu
            // 
            this.creditsMainMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.creditsMainMenu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("creditsMainMenu.BackgroundImage")));
            this.creditsMainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.creditsMainMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.creditsMainMenu.Font = new System.Drawing.Font("Bell MT", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creditsMainMenu.ForeColor = System.Drawing.Color.Maroon;
            this.creditsMainMenu.Location = new System.Drawing.Point(197, 141);
            this.creditsMainMenu.Name = "creditsMainMenu";
            this.creditsMainMenu.Size = new System.Drawing.Size(131, 23);
            this.creditsMainMenu.TabIndex = 3;
            this.creditsMainMenu.Text = "Credits";
            this.creditsMainMenu.UseVisualStyleBackColor = true;
            this.creditsMainMenu.Click += new System.EventHandler(this.creditsMainMenu_Click);
            // 
            // loginMenu
            // 
            this.loginMenu.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.loginMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.loginMenu.Controls.Add(this.tableLayoutPanel4);
            this.loginMenu.Location = new System.Drawing.Point(4, 4);
            this.loginMenu.Name = "loginMenu";
            this.loginMenu.Padding = new System.Windows.Forms.Padding(3);
            this.loginMenu.Size = new System.Drawing.Size(527, 338);
            this.loginMenu.TabIndex = 1;
            this.loginMenu.Text = "LoginMenu";
            this.loginMenu.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.tableLayoutPanel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel4.Controls.Add(this.groupBoxServer, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.groupBoxLogin, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.panel1, 1, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(521, 332);
            this.tableLayoutPanel4.TabIndex = 10;
            // 
            // groupBoxServer
            // 
            this.groupBoxServer.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxServer.Controls.Add(this.serverLoginMenu);
            this.groupBoxServer.Controls.Add(this.serverLabel);
            this.groupBoxServer.Location = new System.Drawing.Point(81, 3);
            this.groupBoxServer.Name = "groupBoxServer";
            this.groupBoxServer.Size = new System.Drawing.Size(358, 74);
            this.groupBoxServer.TabIndex = 0;
            this.groupBoxServer.TabStop = false;
            // 
            // serverLoginMenu
            // 
            this.serverLoginMenu.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.serverLoginMenu.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.serverLoginMenu.FormattingEnabled = true;
            this.serverLoginMenu.Location = new System.Drawing.Point(93, 29);
            this.serverLoginMenu.MaxDropDownItems = 5;
            this.serverLoginMenu.MaxLength = 20;
            this.serverLoginMenu.Name = "serverLoginMenu";
            this.serverLoginMenu.Size = new System.Drawing.Size(142, 21);
            this.serverLoginMenu.TabIndex = 1;
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.ForeColor = System.Drawing.Color.White;
            this.serverLabel.Location = new System.Drawing.Point(6, 32);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(36, 13);
            this.serverLabel.TabIndex = 10;
            this.serverLabel.Text = "server";
            // 
            // groupBoxLogin
            // 
            this.groupBoxLogin.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxLogin.Controls.Add(this.loginLabel);
            this.groupBoxLogin.Controls.Add(this.loginTBLoginMenu);
            this.groupBoxLogin.Controls.Add(this.passwordLoginMenu);
            this.groupBoxLogin.Controls.Add(this.passwordLabel);
            this.groupBoxLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxLogin.Location = new System.Drawing.Point(81, 83);
            this.groupBoxLogin.Name = "groupBoxLogin";
            this.groupBoxLogin.Size = new System.Drawing.Size(358, 110);
            this.groupBoxLogin.TabIndex = 1;
            this.groupBoxLogin.TabStop = false;
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.ForeColor = System.Drawing.Color.White;
            this.loginLabel.Location = new System.Drawing.Point(6, 36);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(29, 13);
            this.loginLabel.TabIndex = 11;
            this.loginLabel.Text = "login";
            // 
            // loginTBLoginMenu
            // 
            this.loginTBLoginMenu.Location = new System.Drawing.Point(93, 33);
            this.loginTBLoginMenu.MaxLength = 25;
            this.loginTBLoginMenu.Name = "loginTBLoginMenu";
            this.loginTBLoginMenu.Size = new System.Drawing.Size(142, 20);
            this.loginTBLoginMenu.TabIndex = 2;
            // 
            // passwordLoginMenu
            // 
            this.passwordLoginMenu.Location = new System.Drawing.Point(93, 59);
            this.passwordLoginMenu.MaxLength = 25;
            this.passwordLoginMenu.Name = "passwordLoginMenu";
            this.passwordLoginMenu.Size = new System.Drawing.Size(142, 20);
            this.passwordLoginMenu.TabIndex = 3;
            this.passwordLoginMenu.Text = "testpassword";
            this.passwordLoginMenu.UseSystemPasswordChar = true;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.ForeColor = System.Drawing.Color.White;
            this.passwordLabel.Location = new System.Drawing.Point(6, 62);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(52, 13);
            this.passwordLabel.TabIndex = 12;
            this.passwordLabel.Text = "password";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.registerLoginMenu);
            this.panel1.Controls.Add(this.loginBTLoginMenu);
            this.panel1.Controls.Add(this.remindPasswordLoginMenu);
            this.panel1.Controls.Add(this.cancelLoginMenu);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(81, 199);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 110);
            this.panel1.TabIndex = 2;
            // 
            // registerLoginMenu
            // 
            this.registerLoginMenu.BackColor = System.Drawing.SystemColors.Control;
            this.registerLoginMenu.Location = new System.Drawing.Point(125, 3);
            this.registerLoginMenu.Name = "registerLoginMenu";
            this.registerLoginMenu.Size = new System.Drawing.Size(110, 23);
            this.registerLoginMenu.TabIndex = 5;
            this.registerLoginMenu.Text = "Register";
            this.registerLoginMenu.UseVisualStyleBackColor = false;
            this.registerLoginMenu.Click += new System.EventHandler(this.registerLoginMenu_Click);
            // 
            // loginBTLoginMenu
            // 
            this.loginBTLoginMenu.BackColor = System.Drawing.SystemColors.Control;
            this.loginBTLoginMenu.Location = new System.Drawing.Point(9, 3);
            this.loginBTLoginMenu.Name = "loginBTLoginMenu";
            this.loginBTLoginMenu.Size = new System.Drawing.Size(110, 23);
            this.loginBTLoginMenu.TabIndex = 4;
            this.loginBTLoginMenu.Text = "Login";
            this.loginBTLoginMenu.UseVisualStyleBackColor = false;
            this.loginBTLoginMenu.Click += new System.EventHandler(this.loginBTLoginMenu_Click);
            // 
            // remindPasswordLoginMenu
            // 
            this.remindPasswordLoginMenu.BackColor = System.Drawing.SystemColors.Control;
            this.remindPasswordLoginMenu.Location = new System.Drawing.Point(9, 32);
            this.remindPasswordLoginMenu.Name = "remindPasswordLoginMenu";
            this.remindPasswordLoginMenu.Size = new System.Drawing.Size(110, 23);
            this.remindPasswordLoginMenu.TabIndex = 6;
            this.remindPasswordLoginMenu.Text = "Remind Password";
            this.remindPasswordLoginMenu.UseVisualStyleBackColor = false;
            this.remindPasswordLoginMenu.Click += new System.EventHandler(this.remindPasswordLoginMenu_Click);
            // 
            // cancelLoginMenu
            // 
            this.cancelLoginMenu.BackColor = System.Drawing.SystemColors.Control;
            this.cancelLoginMenu.Location = new System.Drawing.Point(125, 32);
            this.cancelLoginMenu.Name = "cancelLoginMenu";
            this.cancelLoginMenu.Size = new System.Drawing.Size(110, 23);
            this.cancelLoginMenu.TabIndex = 7;
            this.cancelLoginMenu.Text = "Cancel";
            this.cancelLoginMenu.UseVisualStyleBackColor = false;
            this.cancelLoginMenu.Click += new System.EventHandler(this.cancelLoginMenu_Click);
            // 
            // registerMenu
            // 
            this.registerMenu.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.registerMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.registerMenu.Controls.Add(this.tableLayoutPanel5);
            this.registerMenu.Location = new System.Drawing.Point(4, 4);
            this.registerMenu.Name = "registerMenu";
            this.registerMenu.Padding = new System.Windows.Forms.Padding(3);
            this.registerMenu.Size = new System.Drawing.Size(527, 338);
            this.registerMenu.TabIndex = 2;
            this.registerMenu.Text = "RegisterMenu";
            this.registerMenu.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.tableLayoutPanel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.Controls.Add(this.groupBox2, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.panel2, 1, 2);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 4;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(521, 332);
            this.tableLayoutPanel5.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.loginLBRegisterMenu);
            this.groupBox2.Controls.Add(this.emailLRegisterMenu);
            this.groupBox2.Controls.Add(this.loginTBRegisterMenu);
            this.groupBox2.Controls.Add(this.repeatPasswordLRegisterMenu);
            this.groupBox2.Controls.Add(this.passwordTBRegisterMenu);
            this.groupBox2.Controls.Add(this.emailTBRegisterMenu);
            this.groupBox2.Controls.Add(this.repeatPasswordTBRegisterMenu);
            this.groupBox2.Controls.Add(this.passwordLRegisterMenu);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(107, 83);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(306, 120);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // loginLBRegisterMenu
            // 
            this.loginLBRegisterMenu.AutoSize = true;
            this.loginLBRegisterMenu.ForeColor = System.Drawing.Color.White;
            this.loginLBRegisterMenu.Location = new System.Drawing.Point(6, 13);
            this.loginLBRegisterMenu.Name = "loginLBRegisterMenu";
            this.loginLBRegisterMenu.Size = new System.Drawing.Size(33, 13);
            this.loginLBRegisterMenu.TabIndex = 10;
            this.loginLBRegisterMenu.Text = "Login";
            // 
            // emailLRegisterMenu
            // 
            this.emailLRegisterMenu.AutoSize = true;
            this.emailLRegisterMenu.ForeColor = System.Drawing.Color.White;
            this.emailLRegisterMenu.Location = new System.Drawing.Point(7, 94);
            this.emailLRegisterMenu.Name = "emailLRegisterMenu";
            this.emailLRegisterMenu.Size = new System.Drawing.Size(32, 13);
            this.emailLRegisterMenu.TabIndex = 13;
            this.emailLRegisterMenu.Text = "Email";
            // 
            // loginTBRegisterMenu
            // 
            this.loginTBRegisterMenu.Location = new System.Drawing.Point(103, 10);
            this.loginTBRegisterMenu.MaxLength = 25;
            this.loginTBRegisterMenu.Name = "loginTBRegisterMenu";
            this.loginTBRegisterMenu.Size = new System.Drawing.Size(166, 20);
            this.loginTBRegisterMenu.TabIndex = 1;
            // 
            // repeatPasswordLRegisterMenu
            // 
            this.repeatPasswordLRegisterMenu.AutoSize = true;
            this.repeatPasswordLRegisterMenu.ForeColor = System.Drawing.Color.White;
            this.repeatPasswordLRegisterMenu.Location = new System.Drawing.Point(6, 64);
            this.repeatPasswordLRegisterMenu.Name = "repeatPasswordLRegisterMenu";
            this.repeatPasswordLRegisterMenu.Size = new System.Drawing.Size(91, 13);
            this.repeatPasswordLRegisterMenu.TabIndex = 12;
            this.repeatPasswordLRegisterMenu.Text = "Repeat Password";
            // 
            // passwordTBRegisterMenu
            // 
            this.passwordTBRegisterMenu.Location = new System.Drawing.Point(103, 36);
            this.passwordTBRegisterMenu.MaxLength = 25;
            this.passwordTBRegisterMenu.Name = "passwordTBRegisterMenu";
            this.passwordTBRegisterMenu.Size = new System.Drawing.Size(166, 20);
            this.passwordTBRegisterMenu.TabIndex = 2;
            this.passwordTBRegisterMenu.UseSystemPasswordChar = true;
            // 
            // emailTBRegisterMenu
            // 
            this.emailTBRegisterMenu.Location = new System.Drawing.Point(103, 87);
            this.emailTBRegisterMenu.MaxLength = 100;
            this.emailTBRegisterMenu.Name = "emailTBRegisterMenu";
            this.emailTBRegisterMenu.Size = new System.Drawing.Size(166, 20);
            this.emailTBRegisterMenu.TabIndex = 4;
            // 
            // repeatPasswordTBRegisterMenu
            // 
            this.repeatPasswordTBRegisterMenu.Location = new System.Drawing.Point(103, 61);
            this.repeatPasswordTBRegisterMenu.MaxLength = 25;
            this.repeatPasswordTBRegisterMenu.Name = "repeatPasswordTBRegisterMenu";
            this.repeatPasswordTBRegisterMenu.Size = new System.Drawing.Size(166, 20);
            this.repeatPasswordTBRegisterMenu.TabIndex = 3;
            this.repeatPasswordTBRegisterMenu.UseSystemPasswordChar = true;
            // 
            // passwordLRegisterMenu
            // 
            this.passwordLRegisterMenu.AutoSize = true;
            this.passwordLRegisterMenu.ForeColor = System.Drawing.Color.White;
            this.passwordLRegisterMenu.Location = new System.Drawing.Point(6, 39);
            this.passwordLRegisterMenu.Name = "passwordLRegisterMenu";
            this.passwordLRegisterMenu.Size = new System.Drawing.Size(53, 13);
            this.passwordLRegisterMenu.TabIndex = 11;
            this.passwordLRegisterMenu.Text = "Password";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.registerRegisterMenu);
            this.panel2.Controls.Add(this.backRegisterMenu);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(107, 209);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(306, 100);
            this.panel2.TabIndex = 1;
            // 
            // registerRegisterMenu
            // 
            this.registerRegisterMenu.BackColor = System.Drawing.SystemColors.Control;
            this.registerRegisterMenu.Location = new System.Drawing.Point(22, 3);
            this.registerRegisterMenu.Name = "registerRegisterMenu";
            this.registerRegisterMenu.Size = new System.Drawing.Size(75, 23);
            this.registerRegisterMenu.TabIndex = 5;
            this.registerRegisterMenu.Text = "Register";
            this.registerRegisterMenu.UseVisualStyleBackColor = false;
            this.registerRegisterMenu.Click += new System.EventHandler(this.registerRegisterMenu_Click);
            // 
            // backRegisterMenu
            // 
            this.backRegisterMenu.BackColor = System.Drawing.SystemColors.Control;
            this.backRegisterMenu.Location = new System.Drawing.Point(103, 3);
            this.backRegisterMenu.Name = "backRegisterMenu";
            this.backRegisterMenu.Size = new System.Drawing.Size(75, 23);
            this.backRegisterMenu.TabIndex = 6;
            this.backRegisterMenu.Text = "Back";
            this.backRegisterMenu.UseVisualStyleBackColor = false;
            this.backRegisterMenu.Click += new System.EventHandler(this.backRegisterMenu_Click);
            // 
            // chatMenu
            // 
            this.chatMenu.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.chatMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chatMenu.Controls.Add(this.sendChatMenu);
            this.chatMenu.Controls.Add(this.userListChatMenu);
            this.chatMenu.Controls.Add(this.chatListChatMenu);
            this.chatMenu.Controls.Add(this.chatInputTBChatMenu);
            this.chatMenu.Controls.Add(this.backChatMenu);
            this.chatMenu.Controls.Add(this.gameChatMenu);
            this.chatMenu.Location = new System.Drawing.Point(4, 4);
            this.chatMenu.Name = "chatMenu";
            this.chatMenu.Padding = new System.Windows.Forms.Padding(3);
            this.chatMenu.Size = new System.Drawing.Size(527, 338);
            this.chatMenu.TabIndex = 3;
            this.chatMenu.Text = "ChatMenu";
            this.chatMenu.UseVisualStyleBackColor = true;
            // 
            // sendChatMenu
            // 
            this.sendChatMenu.Location = new System.Drawing.Point(388, 252);
            this.sendChatMenu.Name = "sendChatMenu";
            this.sendChatMenu.Size = new System.Drawing.Size(116, 23);
            this.sendChatMenu.TabIndex = 2;
            this.sendChatMenu.Text = "Send";
            this.sendChatMenu.UseVisualStyleBackColor = true;
            this.sendChatMenu.Click += new System.EventHandler(this.sendChatMenu_Click);
            // 
            // userListChatMenu
            // 
            this.userListChatMenu.FormattingEnabled = true;
            this.userListChatMenu.Location = new System.Drawing.Point(388, 57);
            this.userListChatMenu.Name = "userListChatMenu";
            this.userListChatMenu.Size = new System.Drawing.Size(116, 186);
            this.userListChatMenu.TabIndex = 5;
            this.userListChatMenu.DoubleClick += new System.EventHandler(this.userListChatMenu_DoubleClick);
            // 
            // chatListChatMenu
            // 
            this.chatListChatMenu.FormattingEnabled = true;
            this.chatListChatMenu.Location = new System.Drawing.Point(35, 57);
            this.chatListChatMenu.Name = "chatListChatMenu";
            this.chatListChatMenu.Size = new System.Drawing.Size(347, 186);
            this.chatListChatMenu.TabIndex = 4;
            // 
            // chatInputTBChatMenu
            // 
            this.chatInputTBChatMenu.Location = new System.Drawing.Point(35, 255);
            this.chatInputTBChatMenu.MaxLength = 200;
            this.chatInputTBChatMenu.Name = "chatInputTBChatMenu";
            this.chatInputTBChatMenu.Size = new System.Drawing.Size(347, 20);
            this.chatInputTBChatMenu.TabIndex = 1;
            this.chatInputTBChatMenu.KeyUp += new System.Windows.Forms.KeyEventHandler(this.chatInputTBChatMenu_KeyUp);
            // 
            // backChatMenu
            // 
            this.backChatMenu.Location = new System.Drawing.Point(388, 281);
            this.backChatMenu.Name = "backChatMenu";
            this.backChatMenu.Size = new System.Drawing.Size(116, 23);
            this.backChatMenu.TabIndex = 3;
            this.backChatMenu.Text = "Back";
            this.backChatMenu.UseVisualStyleBackColor = true;
            this.backChatMenu.Click += new System.EventHandler(this.backChatMenu_Click);
            // 
            // gameChatMenu
            // 
            this.gameChatMenu.Location = new System.Drawing.Point(35, 17);
            this.gameChatMenu.Name = "gameChatMenu";
            this.gameChatMenu.Size = new System.Drawing.Size(75, 23);
            this.gameChatMenu.TabIndex = 0;
            this.gameChatMenu.Text = "Game";
            this.gameChatMenu.UseVisualStyleBackColor = true;
            this.gameChatMenu.Click += new System.EventHandler(this.gameChatMenu_Click);
            // 
            // playerInfoMenu
            // 
            this.playerInfoMenu.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.playerInfoMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.playerInfoMenu.Controls.Add(this.playerInfoLInfoMenu);
            this.playerInfoMenu.Controls.Add(this.backInfoMenu);
            this.playerInfoMenu.Location = new System.Drawing.Point(4, 4);
            this.playerInfoMenu.Name = "playerInfoMenu";
            this.playerInfoMenu.Padding = new System.Windows.Forms.Padding(3);
            this.playerInfoMenu.Size = new System.Drawing.Size(527, 338);
            this.playerInfoMenu.TabIndex = 4;
            this.playerInfoMenu.Text = "PlayerInfoMenu";
            this.playerInfoMenu.UseVisualStyleBackColor = true;
            // 
            // playerInfoLInfoMenu
            // 
            this.playerInfoLInfoMenu.AutoSize = true;
            this.playerInfoLInfoMenu.ForeColor = System.Drawing.Color.White;
            this.playerInfoLInfoMenu.Location = new System.Drawing.Point(164, 86);
            this.playerInfoLInfoMenu.Name = "playerInfoLInfoMenu";
            this.playerInfoLInfoMenu.Size = new System.Drawing.Size(57, 13);
            this.playerInfoLInfoMenu.TabIndex = 1;
            this.playerInfoLInfoMenu.Text = "Player Info";
            // 
            // backInfoMenu
            // 
            this.backInfoMenu.Location = new System.Drawing.Point(192, 176);
            this.backInfoMenu.Name = "backInfoMenu";
            this.backInfoMenu.Size = new System.Drawing.Size(75, 23);
            this.backInfoMenu.TabIndex = 0;
            this.backInfoMenu.Text = "Back";
            this.backInfoMenu.UseVisualStyleBackColor = true;
            this.backInfoMenu.Click += new System.EventHandler(this.backInfoMenu_Click);
            // 
            // chooseGameMenu
            // 
            this.chooseGameMenu.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.chooseGameMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chooseGameMenu.Controls.Add(this.textBoxTBGameName);
            this.chooseGameMenu.Controls.Add(this.label1);
            this.chooseGameMenu.Controls.Add(this.textBoxTBGameDescription);
            this.chooseGameMenu.Controls.Add(this.backChooseGameMenu);
            this.chooseGameMenu.Controls.Add(this.listOfGames);
            this.chooseGameMenu.Controls.Add(this.createChooseGameMenu);
            this.chooseGameMenu.Controls.Add(this.joinChooseGameMenu);
            this.chooseGameMenu.Location = new System.Drawing.Point(4, 4);
            this.chooseGameMenu.Name = "chooseGameMenu";
            this.chooseGameMenu.Padding = new System.Windows.Forms.Padding(3);
            this.chooseGameMenu.Size = new System.Drawing.Size(527, 338);
            this.chooseGameMenu.TabIndex = 5;
            this.chooseGameMenu.Text = "ChooseGameMenu";
            this.chooseGameMenu.UseVisualStyleBackColor = true;
            // 
            // textBoxTBGameName
            // 
            this.textBoxTBGameName.Location = new System.Drawing.Point(42, 259);
            this.textBoxTBGameName.Name = "textBoxTBGameName";
            this.textBoxTBGameName.Size = new System.Drawing.Size(155, 20);
            this.textBoxTBGameName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(281, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Description";
            // 
            // textBoxTBGameDescription
            // 
            this.textBoxTBGameDescription.Location = new System.Drawing.Point(284, 57);
            this.textBoxTBGameDescription.Multiline = true;
            this.textBoxTBGameDescription.Name = "textBoxTBGameDescription";
            this.textBoxTBGameDescription.ReadOnly = true;
            this.textBoxTBGameDescription.Size = new System.Drawing.Size(199, 225);
            this.textBoxTBGameDescription.TabIndex = 5;
            // 
            // backChooseGameMenu
            // 
            this.backChooseGameMenu.Location = new System.Drawing.Point(408, 288);
            this.backChooseGameMenu.Name = "backChooseGameMenu";
            this.backChooseGameMenu.Size = new System.Drawing.Size(75, 23);
            this.backChooseGameMenu.TabIndex = 3;
            this.backChooseGameMenu.Text = "Back";
            this.backChooseGameMenu.UseVisualStyleBackColor = true;
            this.backChooseGameMenu.Click += new System.EventHandler(this.backChooseGameMenu_Click);
            // 
            // listOfGames
            // 
            this.listOfGames.FormattingEnabled = true;
            this.listOfGames.Location = new System.Drawing.Point(41, 57);
            this.listOfGames.Name = "listOfGames";
            this.listOfGames.Size = new System.Drawing.Size(237, 199);
            this.listOfGames.TabIndex = 4;
            this.listOfGames.SelectedIndexChanged += new System.EventHandler(this.listOfGames_SelectedIndexChanged);
            // 
            // createChooseGameMenu
            // 
            this.createChooseGameMenu.Location = new System.Drawing.Point(327, 288);
            this.createChooseGameMenu.Name = "createChooseGameMenu";
            this.createChooseGameMenu.Size = new System.Drawing.Size(75, 23);
            this.createChooseGameMenu.TabIndex = 2;
            this.createChooseGameMenu.Text = "Create";
            this.createChooseGameMenu.UseVisualStyleBackColor = true;
            this.createChooseGameMenu.Click += new System.EventHandler(this.createChooseGameMenu_Click);
            // 
            // joinChooseGameMenu
            // 
            this.joinChooseGameMenu.Location = new System.Drawing.Point(203, 259);
            this.joinChooseGameMenu.Name = "joinChooseGameMenu";
            this.joinChooseGameMenu.Size = new System.Drawing.Size(75, 23);
            this.joinChooseGameMenu.TabIndex = 0;
            this.joinChooseGameMenu.Text = "Join";
            this.joinChooseGameMenu.UseVisualStyleBackColor = true;
            this.joinChooseGameMenu.Click += new System.EventHandler(this.joinChooseGameMenu_Click);
            // 
            // createGameMenu
            // 
            this.createGameMenu.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.createGameMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.createGameMenu.Controls.Add(this.maxPlayerNumberNUPCreateGameMenu);
            this.createGameMenu.Controls.Add(this.maxPlayerNumberLCreateGameMenu);
            this.createGameMenu.Controls.Add(this.gameNameLCreateGameMenu);
            this.createGameMenu.Controls.Add(this.gameNameTBCreateGameMenu);
            this.createGameMenu.Controls.Add(this.privateCreateGameMenu);
            this.createGameMenu.Controls.Add(this.publicCreateGameMenu);
            this.createGameMenu.Controls.Add(this.mapsLCreateGameMenu);
            this.createGameMenu.Controls.Add(this.listBoxLBCreateGame);
            this.createGameMenu.Controls.Add(this.cancelCreateGameMenu);
            this.createGameMenu.Controls.Add(this.createCreateGameMenu);
            this.createGameMenu.Location = new System.Drawing.Point(4, 4);
            this.createGameMenu.Name = "createGameMenu";
            this.createGameMenu.Padding = new System.Windows.Forms.Padding(3);
            this.createGameMenu.Size = new System.Drawing.Size(527, 338);
            this.createGameMenu.TabIndex = 6;
            this.createGameMenu.Text = "CreateGameMenu";
            this.createGameMenu.UseVisualStyleBackColor = true;
            // 
            // maxPlayerNumberNUPCreateGameMenu
            // 
            this.maxPlayerNumberNUPCreateGameMenu.Location = new System.Drawing.Point(272, 80);
            this.maxPlayerNumberNUPCreateGameMenu.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.maxPlayerNumberNUPCreateGameMenu.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxPlayerNumberNUPCreateGameMenu.Name = "maxPlayerNumberNUPCreateGameMenu";
            this.maxPlayerNumberNUPCreateGameMenu.Size = new System.Drawing.Size(36, 20);
            this.maxPlayerNumberNUPCreateGameMenu.TabIndex = 1;
            this.maxPlayerNumberNUPCreateGameMenu.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // maxPlayerNumberLCreateGameMenu
            // 
            this.maxPlayerNumberLCreateGameMenu.AutoSize = true;
            this.maxPlayerNumberLCreateGameMenu.ForeColor = System.Drawing.Color.White;
            this.maxPlayerNumberLCreateGameMenu.Location = new System.Drawing.Point(269, 64);
            this.maxPlayerNumberLCreateGameMenu.Name = "maxPlayerNumberLCreateGameMenu";
            this.maxPlayerNumberLCreateGameMenu.Size = new System.Drawing.Size(99, 13);
            this.maxPlayerNumberLCreateGameMenu.TabIndex = 11;
            this.maxPlayerNumberLCreateGameMenu.Text = "Max Player Number";
            // 
            // gameNameLCreateGameMenu
            // 
            this.gameNameLCreateGameMenu.AutoSize = true;
            this.gameNameLCreateGameMenu.ForeColor = System.Drawing.Color.White;
            this.gameNameLCreateGameMenu.Location = new System.Drawing.Point(269, 113);
            this.gameNameLCreateGameMenu.Name = "gameNameLCreateGameMenu";
            this.gameNameLCreateGameMenu.Size = new System.Drawing.Size(66, 13);
            this.gameNameLCreateGameMenu.TabIndex = 12;
            this.gameNameLCreateGameMenu.Text = "Game Name";
            // 
            // gameNameTBCreateGameMenu
            // 
            this.gameNameTBCreateGameMenu.Location = new System.Drawing.Point(272, 129);
            this.gameNameTBCreateGameMenu.Name = "gameNameTBCreateGameMenu";
            this.gameNameTBCreateGameMenu.Size = new System.Drawing.Size(179, 20);
            this.gameNameTBCreateGameMenu.TabIndex = 2;
            this.gameNameTBCreateGameMenu.Text = "test_game";
            // 
            // privateCreateGameMenu
            // 
            this.privateCreateGameMenu.AutoSize = true;
            this.privateCreateGameMenu.ForeColor = System.Drawing.Color.White;
            this.privateCreateGameMenu.Location = new System.Drawing.Point(272, 189);
            this.privateCreateGameMenu.Name = "privateCreateGameMenu";
            this.privateCreateGameMenu.Size = new System.Drawing.Size(58, 17);
            this.privateCreateGameMenu.TabIndex = 4;
            this.privateCreateGameMenu.Text = "Private";
            this.privateCreateGameMenu.UseVisualStyleBackColor = true;
            // 
            // publicCreateGameMenu
            // 
            this.publicCreateGameMenu.AutoSize = true;
            this.publicCreateGameMenu.Checked = true;
            this.publicCreateGameMenu.ForeColor = System.Drawing.Color.White;
            this.publicCreateGameMenu.Location = new System.Drawing.Point(272, 166);
            this.publicCreateGameMenu.Name = "publicCreateGameMenu";
            this.publicCreateGameMenu.Size = new System.Drawing.Size(54, 17);
            this.publicCreateGameMenu.TabIndex = 3;
            this.publicCreateGameMenu.TabStop = true;
            this.publicCreateGameMenu.Text = "Public";
            this.publicCreateGameMenu.UseVisualStyleBackColor = true;
            // 
            // mapsLCreateGameMenu
            // 
            this.mapsLCreateGameMenu.AutoSize = true;
            this.mapsLCreateGameMenu.ForeColor = System.Drawing.Color.White;
            this.mapsLCreateGameMenu.Location = new System.Drawing.Point(43, 48);
            this.mapsLCreateGameMenu.Name = "mapsLCreateGameMenu";
            this.mapsLCreateGameMenu.Size = new System.Drawing.Size(67, 13);
            this.mapsLCreateGameMenu.TabIndex = 10;
            this.mapsLCreateGameMenu.Text = "Choose Map";
            // 
            // listBoxLBCreateGame
            // 
            this.listBoxLBCreateGame.FormattingEnabled = true;
            this.listBoxLBCreateGame.Location = new System.Drawing.Point(46, 64);
            this.listBoxLBCreateGame.Name = "listBoxLBCreateGame";
            this.listBoxLBCreateGame.Size = new System.Drawing.Size(217, 199);
            this.listBoxLBCreateGame.TabIndex = 2;
            // 
            // cancelCreateGameMenu
            // 
            this.cancelCreateGameMenu.Location = new System.Drawing.Point(272, 269);
            this.cancelCreateGameMenu.Name = "cancelCreateGameMenu";
            this.cancelCreateGameMenu.Size = new System.Drawing.Size(75, 23);
            this.cancelCreateGameMenu.TabIndex = 6;
            this.cancelCreateGameMenu.Text = "Cancel";
            this.cancelCreateGameMenu.UseVisualStyleBackColor = true;
            this.cancelCreateGameMenu.Click += new System.EventHandler(this.cancelCreateGameMenu_Click);
            // 
            // createCreateGameMenu
            // 
            this.createCreateGameMenu.Location = new System.Drawing.Point(188, 269);
            this.createCreateGameMenu.Name = "createCreateGameMenu";
            this.createCreateGameMenu.Size = new System.Drawing.Size(75, 23);
            this.createCreateGameMenu.TabIndex = 5;
            this.createCreateGameMenu.Text = "Create";
            this.createCreateGameMenu.UseVisualStyleBackColor = true;
            this.createCreateGameMenu.Click += new System.EventHandler(this.createCreateGameMenu_Click);
            // 
            // waitingForPlayersMenu
            // 
            this.waitingForPlayersMenu.BackgroundImage = global::Yad.Properties.Resources.Menu;
            this.waitingForPlayersMenu.Controls.Add(this.currentDataGBWaitingForPlayersMenu);
            this.waitingForPlayersMenu.Controls.Add(this.changeGBWaitingForPlayersMenu);
            this.waitingForPlayersMenu.Controls.Add(this.dataGridViewPlayers);
            this.waitingForPlayersMenu.Controls.Add(this.descriptionLWaitingForPlayersMenu);
            this.waitingForPlayersMenu.Controls.Add(this.descriptionWaitingForPlayersMenu);
            this.waitingForPlayersMenu.Controls.Add(this.playersLWaitingForPlayersMenu);
            this.waitingForPlayersMenu.Controls.Add(this.startWaitingForPlayersMenu);
            this.waitingForPlayersMenu.Controls.Add(this.cancelWaitingForPlayersMenu);
            this.waitingForPlayersMenu.Location = new System.Drawing.Point(4, 4);
            this.waitingForPlayersMenu.Name = "waitingForPlayersMenu";
            this.waitingForPlayersMenu.Padding = new System.Windows.Forms.Padding(3);
            this.waitingForPlayersMenu.Size = new System.Drawing.Size(527, 338);
            this.waitingForPlayersMenu.TabIndex = 7;
            this.waitingForPlayersMenu.Text = "WaitingForPlayersMenu";
            this.waitingForPlayersMenu.UseVisualStyleBackColor = true;
            // 
            // currentDataGBWaitingForPlayersMenu
            // 
            this.currentDataGBWaitingForPlayersMenu.BackColor = System.Drawing.Color.Transparent;
            this.currentDataGBWaitingForPlayersMenu.Controls.Add(this.pictureBoxHouse);
            this.currentDataGBWaitingForPlayersMenu.Controls.Add(this.infoColorWaitingForPlayersMenu);
            this.currentDataGBWaitingForPlayersMenu.Controls.Add(this.infoColorLWaitingForPlayersMenu);
            this.currentDataGBWaitingForPlayersMenu.Controls.Add(this.infoLWaitingForPlayersMenu);
            this.currentDataGBWaitingForPlayersMenu.Location = new System.Drawing.Point(299, 8);
            this.currentDataGBWaitingForPlayersMenu.Name = "currentDataGBWaitingForPlayersMenu";
            this.currentDataGBWaitingForPlayersMenu.Size = new System.Drawing.Size(209, 279);
            this.currentDataGBWaitingForPlayersMenu.TabIndex = 17;
            this.currentDataGBWaitingForPlayersMenu.TabStop = false;
            this.currentDataGBWaitingForPlayersMenu.Text = "Current Data";
            // 
            // pictureBoxHouse
            // 
            this.pictureBoxHouse.Location = new System.Drawing.Point(63, 142);
            this.pictureBoxHouse.Name = "pictureBoxHouse";
            this.pictureBoxHouse.Size = new System.Drawing.Size(110, 120);
            this.pictureBoxHouse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxHouse.TabIndex = 20;
            this.pictureBoxHouse.TabStop = false;
            // 
            // infoColorWaitingForPlayersMenu
            // 
            this.infoColorWaitingForPlayersMenu.Enabled = false;
            this.infoColorWaitingForPlayersMenu.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.infoColorWaitingForPlayersMenu.Location = new System.Drawing.Point(63, 83);
            this.infoColorWaitingForPlayersMenu.Name = "infoColorWaitingForPlayersMenu";
            this.infoColorWaitingForPlayersMenu.Size = new System.Drawing.Size(99, 21);
            this.infoColorWaitingForPlayersMenu.TabIndex = 19;
            this.infoColorWaitingForPlayersMenu.UseVisualStyleBackColor = false;
            // 
            // infoColorLWaitingForPlayersMenu
            // 
            this.infoColorLWaitingForPlayersMenu.AutoSize = true;
            this.infoColorLWaitingForPlayersMenu.Location = new System.Drawing.Point(6, 91);
            this.infoColorLWaitingForPlayersMenu.Name = "infoColorLWaitingForPlayersMenu";
            this.infoColorLWaitingForPlayersMenu.Size = new System.Drawing.Size(31, 13);
            this.infoColorLWaitingForPlayersMenu.TabIndex = 18;
            this.infoColorLWaitingForPlayersMenu.Text = "Color";
            // 
            // infoLWaitingForPlayersMenu
            // 
            this.infoLWaitingForPlayersMenu.AutoSize = true;
            this.infoLWaitingForPlayersMenu.Location = new System.Drawing.Point(6, 23);
            this.infoLWaitingForPlayersMenu.Name = "infoLWaitingForPlayersMenu";
            this.infoLWaitingForPlayersMenu.Size = new System.Drawing.Size(25, 13);
            this.infoLWaitingForPlayersMenu.TabIndex = 0;
            this.infoLWaitingForPlayersMenu.Text = "Info";
            // 
            // changeGBWaitingForPlayersMenu
            // 
            this.changeGBWaitingForPlayersMenu.BackColor = System.Drawing.Color.Transparent;
            this.changeGBWaitingForPlayersMenu.Controls.Add(this.colorLWaitingForPlayersMenu);
            this.changeGBWaitingForPlayersMenu.Controls.Add(this.changeWaitingForPlayersMenu);
            this.changeGBWaitingForPlayersMenu.Controls.Add(this.colorWaitingForPlayersMenu);
            this.changeGBWaitingForPlayersMenu.Controls.Add(this.houseLWaitingForPlayersMenu);
            this.changeGBWaitingForPlayersMenu.Controls.Add(this.houseCBWaitingForPlayersMenu);
            this.changeGBWaitingForPlayersMenu.Controls.Add(this.teamCBWaitingForPlayersMenu);
            this.changeGBWaitingForPlayersMenu.Controls.Add(this.teamLWaitingForPlayersMenu);
            this.changeGBWaitingForPlayersMenu.Location = new System.Drawing.Point(15, 8);
            this.changeGBWaitingForPlayersMenu.Name = "changeGBWaitingForPlayersMenu";
            this.changeGBWaitingForPlayersMenu.Size = new System.Drawing.Size(278, 104);
            this.changeGBWaitingForPlayersMenu.TabIndex = 16;
            this.changeGBWaitingForPlayersMenu.TabStop = false;
            this.changeGBWaitingForPlayersMenu.Text = "Change";
            // 
            // colorLWaitingForPlayersMenu
            // 
            this.colorLWaitingForPlayersMenu.AutoSize = true;
            this.colorLWaitingForPlayersMenu.Location = new System.Drawing.Point(6, 79);
            this.colorLWaitingForPlayersMenu.Name = "colorLWaitingForPlayersMenu";
            this.colorLWaitingForPlayersMenu.Size = new System.Drawing.Size(31, 13);
            this.colorLWaitingForPlayersMenu.TabIndex = 17;
            this.colorLWaitingForPlayersMenu.Text = "Color";
            // 
            // changeWaitingForPlayersMenu
            // 
            this.changeWaitingForPlayersMenu.BackColor = System.Drawing.SystemColors.Control;
            this.changeWaitingForPlayersMenu.Location = new System.Drawing.Point(193, 45);
            this.changeWaitingForPlayersMenu.Name = "changeWaitingForPlayersMenu";
            this.changeWaitingForPlayersMenu.Size = new System.Drawing.Size(75, 23);
            this.changeWaitingForPlayersMenu.TabIndex = 5;
            this.changeWaitingForPlayersMenu.Text = "Change";
            this.changeWaitingForPlayersMenu.UseVisualStyleBackColor = false;
            this.changeWaitingForPlayersMenu.Click += new System.EventHandler(this.changeWaitingForPlayersMenu_Click);
            // 
            // colorWaitingForPlayersMenu
            // 
            this.colorWaitingForPlayersMenu.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colorWaitingForPlayersMenu.Location = new System.Drawing.Point(79, 75);
            this.colorWaitingForPlayersMenu.Name = "colorWaitingForPlayersMenu";
            this.colorWaitingForPlayersMenu.Size = new System.Drawing.Size(99, 21);
            this.colorWaitingForPlayersMenu.TabIndex = 17;
            this.colorWaitingForPlayersMenu.UseVisualStyleBackColor = true;
            this.colorWaitingForPlayersMenu.Click += new System.EventHandler(this.colorWaitingForPlayersMenu_Click);
            // 
            // houseLWaitingForPlayersMenu
            // 
            this.houseLWaitingForPlayersMenu.AutoSize = true;
            this.houseLWaitingForPlayersMenu.Location = new System.Drawing.Point(6, 23);
            this.houseLWaitingForPlayersMenu.Name = "houseLWaitingForPlayersMenu";
            this.houseLWaitingForPlayersMenu.Size = new System.Drawing.Size(38, 13);
            this.houseLWaitingForPlayersMenu.TabIndex = 14;
            this.houseLWaitingForPlayersMenu.Text = "House";
            // 
            // houseCBWaitingForPlayersMenu
            // 
            this.houseCBWaitingForPlayersMenu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.houseCBWaitingForPlayersMenu.FormattingEnabled = true;
            this.houseCBWaitingForPlayersMenu.Location = new System.Drawing.Point(79, 20);
            this.houseCBWaitingForPlayersMenu.MaxDropDownItems = 10;
            this.houseCBWaitingForPlayersMenu.Name = "houseCBWaitingForPlayersMenu";
            this.houseCBWaitingForPlayersMenu.Size = new System.Drawing.Size(99, 21);
            this.houseCBWaitingForPlayersMenu.TabIndex = 3;
            this.houseCBWaitingForPlayersMenu.SelectedIndexChanged += new System.EventHandler(this.CBWaitingForPlayersMenu_SelectedIndexChanged);
            // 
            // teamCBWaitingForPlayersMenu
            // 
            this.teamCBWaitingForPlayersMenu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.teamCBWaitingForPlayersMenu.FormattingEnabled = true;
            this.teamCBWaitingForPlayersMenu.Location = new System.Drawing.Point(79, 47);
            this.teamCBWaitingForPlayersMenu.Name = "teamCBWaitingForPlayersMenu";
            this.teamCBWaitingForPlayersMenu.Size = new System.Drawing.Size(99, 21);
            this.teamCBWaitingForPlayersMenu.TabIndex = 4;
            this.teamCBWaitingForPlayersMenu.SelectedIndexChanged += new System.EventHandler(this.CBWaitingForPlayersMenu_SelectedIndexChanged);
            // 
            // teamLWaitingForPlayersMenu
            // 
            this.teamLWaitingForPlayersMenu.AutoSize = true;
            this.teamLWaitingForPlayersMenu.Location = new System.Drawing.Point(6, 50);
            this.teamLWaitingForPlayersMenu.Name = "teamLWaitingForPlayersMenu";
            this.teamLWaitingForPlayersMenu.Size = new System.Drawing.Size(34, 13);
            this.teamLWaitingForPlayersMenu.TabIndex = 15;
            this.teamLWaitingForPlayersMenu.Text = "Team";
            // 
            // dataGridViewPlayers
            // 
            this.dataGridViewPlayers.AllowUserToAddRows = false;
            this.dataGridViewPlayers.AllowUserToDeleteRows = false;
            this.dataGridViewPlayers.AllowUserToResizeRows = false;
            this.dataGridViewPlayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPlayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Color,
            this.PlayersName,
            this.House,
            this.Team});
            this.dataGridViewPlayers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewPlayers.Location = new System.Drawing.Point(15, 131);
            this.dataGridViewPlayers.MultiSelect = false;
            this.dataGridViewPlayers.Name = "dataGridViewPlayers";
            this.dataGridViewPlayers.ReadOnly = true;
            this.dataGridViewPlayers.RowHeadersVisible = false;
            this.dataGridViewPlayers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewPlayers.Size = new System.Drawing.Size(278, 98);
            this.dataGridViewPlayers.TabIndex = 10;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 25;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ID.Visible = false;
            this.ID.Width = 25;
            // 
            // Color
            // 
            this.Color.HeaderText = "Color";
            this.Color.MinimumWidth = 35;
            this.Color.Name = "Color";
            this.Color.ReadOnly = true;
            this.Color.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Color.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Color.Width = 35;
            // 
            // PlayersName
            // 
            this.PlayersName.HeaderText = "Name";
            this.PlayersName.MinimumWidth = 100;
            this.PlayersName.Name = "PlayersName";
            this.PlayersName.ReadOnly = true;
            this.PlayersName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PlayersName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // House
            // 
            this.House.HeaderText = "House";
            this.House.MinimumWidth = 75;
            this.House.Name = "House";
            this.House.ReadOnly = true;
            this.House.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.House.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.House.Width = 75;
            // 
            // Team
            // 
            this.Team.HeaderText = "Team";
            this.Team.MinimumWidth = 65;
            this.Team.Name = "Team";
            this.Team.ReadOnly = true;
            this.Team.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Team.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Team.Width = 65;
            // 
            // descriptionLWaitingForPlayersMenu
            // 
            this.descriptionLWaitingForPlayersMenu.AutoSize = true;
            this.descriptionLWaitingForPlayersMenu.BackColor = System.Drawing.Color.Transparent;
            this.descriptionLWaitingForPlayersMenu.Location = new System.Drawing.Point(12, 232);
            this.descriptionLWaitingForPlayersMenu.Name = "descriptionLWaitingForPlayersMenu";
            this.descriptionLWaitingForPlayersMenu.Size = new System.Drawing.Size(60, 13);
            this.descriptionLWaitingForPlayersMenu.TabIndex = 13;
            this.descriptionLWaitingForPlayersMenu.Text = "Description";
            // 
            // descriptionWaitingForPlayersMenu
            // 
            this.descriptionWaitingForPlayersMenu.Location = new System.Drawing.Point(15, 248);
            this.descriptionWaitingForPlayersMenu.Multiline = true;
            this.descriptionWaitingForPlayersMenu.Name = "descriptionWaitingForPlayersMenu";
            this.descriptionWaitingForPlayersMenu.ReadOnly = true;
            this.descriptionWaitingForPlayersMenu.Size = new System.Drawing.Size(278, 84);
            this.descriptionWaitingForPlayersMenu.TabIndex = 11;
            // 
            // playersLWaitingForPlayersMenu
            // 
            this.playersLWaitingForPlayersMenu.AutoSize = true;
            this.playersLWaitingForPlayersMenu.BackColor = System.Drawing.Color.Transparent;
            this.playersLWaitingForPlayersMenu.Location = new System.Drawing.Point(12, 115);
            this.playersLWaitingForPlayersMenu.Name = "playersLWaitingForPlayersMenu";
            this.playersLWaitingForPlayersMenu.Size = new System.Drawing.Size(41, 13);
            this.playersLWaitingForPlayersMenu.TabIndex = 12;
            this.playersLWaitingForPlayersMenu.Text = "Players";
            // 
            // startWaitingForPlayersMenu
            // 
            this.startWaitingForPlayersMenu.Location = new System.Drawing.Point(380, 293);
            this.startWaitingForPlayersMenu.Name = "startWaitingForPlayersMenu";
            this.startWaitingForPlayersMenu.Size = new System.Drawing.Size(61, 23);
            this.startWaitingForPlayersMenu.TabIndex = 1;
            this.startWaitingForPlayersMenu.Text = "Start Game";
            this.startWaitingForPlayersMenu.UseVisualStyleBackColor = true;
            this.startWaitingForPlayersMenu.Click += new System.EventHandler(this.startWaitingForPlayersMenu_Click);
            // 
            // cancelWaitingForPlayersMenu
            // 
            this.cancelWaitingForPlayersMenu.Location = new System.Drawing.Point(447, 293);
            this.cancelWaitingForPlayersMenu.Name = "cancelWaitingForPlayersMenu";
            this.cancelWaitingForPlayersMenu.Size = new System.Drawing.Size(61, 23);
            this.cancelWaitingForPlayersMenu.TabIndex = 2;
            this.cancelWaitingForPlayersMenu.Text = "Cancel";
            this.cancelWaitingForPlayersMenu.UseVisualStyleBackColor = true;
            this.cancelWaitingForPlayersMenu.Click += new System.EventHandler(this.cancelWaitingForPlayersMenu_Click);
            // 
            // optionsMenu
            // 
            this.optionsMenu.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.optionsMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.optionsMenu.Controls.Add(this.groupBox3);
            this.optionsMenu.Controls.Add(this.okOptionsMenu);
            this.optionsMenu.Controls.Add(this.cancelOptionsMenu);
            this.optionsMenu.Location = new System.Drawing.Point(4, 4);
            this.optionsMenu.Name = "optionsMenu";
            this.optionsMenu.Padding = new System.Windows.Forms.Padding(3);
            this.optionsMenu.Size = new System.Drawing.Size(527, 338);
            this.optionsMenu.TabIndex = 8;
            this.optionsMenu.Text = "OptionsMenu";
            this.optionsMenu.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.musicVolumeLOptionsMenu);
            this.groupBox3.Controls.Add(this.soundVolumeLOptionsMenu);
            this.groupBox3.Controls.Add(this.musicVolumeNMOptionsMenu);
            this.groupBox3.Controls.Add(this.muteSoundOptionsMenu);
            this.groupBox3.Controls.Add(this.soundVolumeNMOptionsMenu);
            this.groupBox3.Controls.Add(this.muteMusicOptionsMenu);
            this.groupBox3.Location = new System.Drawing.Point(153, 125);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(273, 83);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            // 
            // musicVolumeLOptionsMenu
            // 
            this.musicVolumeLOptionsMenu.AutoSize = true;
            this.musicVolumeLOptionsMenu.ForeColor = System.Drawing.Color.White;
            this.musicVolumeLOptionsMenu.Location = new System.Drawing.Point(6, 16);
            this.musicVolumeLOptionsMenu.Name = "musicVolumeLOptionsMenu";
            this.musicVolumeLOptionsMenu.Size = new System.Drawing.Size(73, 13);
            this.musicVolumeLOptionsMenu.TabIndex = 4;
            this.musicVolumeLOptionsMenu.Text = "Music Volume";
            // 
            // soundVolumeLOptionsMenu
            // 
            this.soundVolumeLOptionsMenu.AutoSize = true;
            this.soundVolumeLOptionsMenu.ForeColor = System.Drawing.Color.White;
            this.soundVolumeLOptionsMenu.Location = new System.Drawing.Point(6, 40);
            this.soundVolumeLOptionsMenu.Name = "soundVolumeLOptionsMenu";
            this.soundVolumeLOptionsMenu.Size = new System.Drawing.Size(76, 13);
            this.soundVolumeLOptionsMenu.TabIndex = 5;
            this.soundVolumeLOptionsMenu.Text = "Sound Volume";
            // 
            // musicVolumeNMOptionsMenu
            // 
            this.musicVolumeNMOptionsMenu.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.musicVolumeNMOptionsMenu.Location = new System.Drawing.Point(98, 15);
            this.musicVolumeNMOptionsMenu.Name = "musicVolumeNMOptionsMenu";
            this.musicVolumeNMOptionsMenu.Size = new System.Drawing.Size(50, 20);
            this.musicVolumeNMOptionsMenu.TabIndex = 2;
            // 
            // muteSoundOptionsMenu
            // 
            this.muteSoundOptionsMenu.AutoSize = true;
            this.muteSoundOptionsMenu.ForeColor = System.Drawing.Color.White;
            this.muteSoundOptionsMenu.Location = new System.Drawing.Point(180, 41);
            this.muteSoundOptionsMenu.Name = "muteSoundOptionsMenu";
            this.muteSoundOptionsMenu.Size = new System.Drawing.Size(84, 17);
            this.muteSoundOptionsMenu.TabIndex = 1;
            this.muteSoundOptionsMenu.Text = "Mute Sound";
            this.muteSoundOptionsMenu.UseVisualStyleBackColor = true;
            this.muteSoundOptionsMenu.CheckedChanged += new System.EventHandler(this.muteSoundOptionsMenu_CheckedChanged);
            // 
            // soundVolumeNMOptionsMenu
            // 
            this.soundVolumeNMOptionsMenu.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.soundVolumeNMOptionsMenu.Location = new System.Drawing.Point(98, 40);
            this.soundVolumeNMOptionsMenu.Name = "soundVolumeNMOptionsMenu";
            this.soundVolumeNMOptionsMenu.Size = new System.Drawing.Size(50, 20);
            this.soundVolumeNMOptionsMenu.TabIndex = 3;
            // 
            // muteMusicOptionsMenu
            // 
            this.muteMusicOptionsMenu.AutoSize = true;
            this.muteMusicOptionsMenu.ForeColor = System.Drawing.Color.White;
            this.muteMusicOptionsMenu.Location = new System.Drawing.Point(180, 15);
            this.muteMusicOptionsMenu.Name = "muteMusicOptionsMenu";
            this.muteMusicOptionsMenu.Size = new System.Drawing.Size(81, 17);
            this.muteMusicOptionsMenu.TabIndex = 0;
            this.muteMusicOptionsMenu.Text = "Mute Music";
            this.muteMusicOptionsMenu.UseVisualStyleBackColor = true;
            this.muteMusicOptionsMenu.CheckedChanged += new System.EventHandler(this.muteMusicOptionsMenu_CheckedChanged);
            // 
            // okOptionsMenu
            // 
            this.okOptionsMenu.Location = new System.Drawing.Point(270, 214);
            this.okOptionsMenu.Name = "okOptionsMenu";
            this.okOptionsMenu.Size = new System.Drawing.Size(75, 23);
            this.okOptionsMenu.TabIndex = 7;
            this.okOptionsMenu.Text = "OK";
            this.okOptionsMenu.UseVisualStyleBackColor = true;
            this.okOptionsMenu.Click += new System.EventHandler(this.okOptionsMenu_Click);
            // 
            // cancelOptionsMenu
            // 
            this.cancelOptionsMenu.Location = new System.Drawing.Point(351, 214);
            this.cancelOptionsMenu.Name = "cancelOptionsMenu";
            this.cancelOptionsMenu.Size = new System.Drawing.Size(75, 23);
            this.cancelOptionsMenu.TabIndex = 6;
            this.cancelOptionsMenu.Text = "Cancel";
            this.cancelOptionsMenu.UseVisualStyleBackColor = true;
            this.cancelOptionsMenu.Click += new System.EventHandler(this.cancelOptionsMenu_Click);
            // 
            // gameMenu
            // 
            this.gameMenu.Controls.Add(this.tableLayoutPanel3);
            this.gameMenu.Location = new System.Drawing.Point(4, 4);
            this.gameMenu.Name = "gameMenu";
            this.gameMenu.Padding = new System.Windows.Forms.Padding(3);
            this.gameMenu.Size = new System.Drawing.Size(527, 338);
            this.gameMenu.TabIndex = 9;
            this.gameMenu.Text = "GameMenu";
            this.gameMenu.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.tableLayoutPanel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Controls.Add(this.pauseGameMenu, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.exitGameMenu, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.optionsGameMenu, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.okGameMenu, 1, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(521, 332);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // pauseGameMenu
            // 
            this.pauseGameMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pauseGameMenu.Location = new System.Drawing.Point(107, 83);
            this.pauseGameMenu.Name = "pauseGameMenu";
            this.pauseGameMenu.Size = new System.Drawing.Size(306, 23);
            this.pauseGameMenu.TabIndex = 0;
            this.pauseGameMenu.Text = "Pause";
            this.pauseGameMenu.UseVisualStyleBackColor = true;
            this.pauseGameMenu.Click += new System.EventHandler(this.pauseGameMenu_Click);
            // 
            // exitGameMenu
            // 
            this.exitGameMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.exitGameMenu.Location = new System.Drawing.Point(107, 170);
            this.exitGameMenu.Name = "exitGameMenu";
            this.exitGameMenu.Size = new System.Drawing.Size(306, 23);
            this.exitGameMenu.TabIndex = 3;
            this.exitGameMenu.Text = "Exit";
            this.exitGameMenu.UseVisualStyleBackColor = true;
            this.exitGameMenu.Click += new System.EventHandler(this.exitGameMenu_Click);
            // 
            // optionsGameMenu
            // 
            this.optionsGameMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsGameMenu.Location = new System.Drawing.Point(107, 112);
            this.optionsGameMenu.Name = "optionsGameMenu";
            this.optionsGameMenu.Size = new System.Drawing.Size(306, 23);
            this.optionsGameMenu.TabIndex = 1;
            this.optionsGameMenu.Text = "Options";
            this.optionsGameMenu.UseVisualStyleBackColor = true;
            this.optionsGameMenu.Click += new System.EventHandler(this.optionsGameMenu_Click);
            // 
            // okGameMenu
            // 
            this.okGameMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.okGameMenu.Location = new System.Drawing.Point(107, 141);
            this.okGameMenu.Name = "okGameMenu";
            this.okGameMenu.Size = new System.Drawing.Size(306, 23);
            this.okGameMenu.TabIndex = 2;
            this.okGameMenu.Text = "OK";
            this.okGameMenu.UseVisualStyleBackColor = true;
            this.okGameMenu.Click += new System.EventHandler(this.okGameMenu_Click);
            // 
            // pauseMenu
            // 
            this.pauseMenu.Controls.Add(this.tableLayoutPanel2);
            this.pauseMenu.Location = new System.Drawing.Point(4, 4);
            this.pauseMenu.Name = "pauseMenu";
            this.pauseMenu.Padding = new System.Windows.Forms.Padding(3);
            this.pauseMenu.Size = new System.Drawing.Size(527, 338);
            this.pauseMenu.TabIndex = 10;
            this.pauseMenu.Text = "PauseMenu";
            this.pauseMenu.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackgroundImage = global::Yad.Properties.Resources.Menu_Planet;
            this.tableLayoutPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Controls.Add(this.continuePauseMenu, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.exitPauseMenu, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.optionsPauseMenu, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(521, 332);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // continuePauseMenu
            // 
            this.continuePauseMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.continuePauseMenu.Location = new System.Drawing.Point(107, 83);
            this.continuePauseMenu.Name = "continuePauseMenu";
            this.continuePauseMenu.Size = new System.Drawing.Size(306, 23);
            this.continuePauseMenu.TabIndex = 0;
            this.continuePauseMenu.Text = "Continue";
            this.continuePauseMenu.UseVisualStyleBackColor = true;
            this.continuePauseMenu.Click += new System.EventHandler(this.continuePauseMenu_Click);
            // 
            // exitPauseMenu
            // 
            this.exitPauseMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.exitPauseMenu.Location = new System.Drawing.Point(107, 141);
            this.exitPauseMenu.Name = "exitPauseMenu";
            this.exitPauseMenu.Size = new System.Drawing.Size(306, 23);
            this.exitPauseMenu.TabIndex = 2;
            this.exitPauseMenu.Text = "Exit";
            this.exitPauseMenu.UseVisualStyleBackColor = true;
            this.exitPauseMenu.Click += new System.EventHandler(this.exitPauseMenu_Click);
            // 
            // optionsPauseMenu
            // 
            this.optionsPauseMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsPauseMenu.Location = new System.Drawing.Point(107, 112);
            this.optionsPauseMenu.Name = "optionsPauseMenu";
            this.optionsPauseMenu.Size = new System.Drawing.Size(306, 23);
            this.optionsPauseMenu.TabIndex = 1;
            this.optionsPauseMenu.Text = "Options";
            this.optionsPauseMenu.UseVisualStyleBackColor = true;
            this.optionsPauseMenu.Click += new System.EventHandler(this.optionsPauseMenu_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkRate = 0;
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 381);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainMenuForm";
            this.Text = "MainMenu";
            this.tabControl.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.loginMenu.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.groupBoxServer.ResumeLayout(false);
            this.groupBoxServer.PerformLayout();
            this.groupBoxLogin.ResumeLayout(false);
            this.groupBoxLogin.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.registerMenu.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.chatMenu.ResumeLayout(false);
            this.chatMenu.PerformLayout();
            this.playerInfoMenu.ResumeLayout(false);
            this.playerInfoMenu.PerformLayout();
            this.chooseGameMenu.ResumeLayout(false);
            this.chooseGameMenu.PerformLayout();
            this.createGameMenu.ResumeLayout(false);
            this.createGameMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxPlayerNumberNUPCreateGameMenu)).EndInit();
            this.waitingForPlayersMenu.ResumeLayout(false);
            this.waitingForPlayersMenu.PerformLayout();
            this.currentDataGBWaitingForPlayersMenu.ResumeLayout(false);
            this.currentDataGBWaitingForPlayersMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHouse)).EndInit();
            this.changeGBWaitingForPlayersMenu.ResumeLayout(false);
            this.changeGBWaitingForPlayersMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlayers)).EndInit();
            this.optionsMenu.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.musicVolumeNMOptionsMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.soundVolumeNMOptionsMenu)).EndInit();
            this.gameMenu.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pauseMenu.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MenuTabControl tabControl;
        private System.Windows.Forms.TabPage mainMenu;
        private System.Windows.Forms.TabPage loginMenu;
        private System.Windows.Forms.TabPage registerMenu;
        private System.Windows.Forms.TabPage chatMenu;
        private System.Windows.Forms.TabPage playerInfoMenu;
        private System.Windows.Forms.TabPage chooseGameMenu;
        private System.Windows.Forms.TabPage createGameMenu;
        private System.Windows.Forms.TabPage waitingForPlayersMenu;
        private System.Windows.Forms.TabPage optionsMenu;
        private System.Windows.Forms.TabPage gameMenu;
        private System.Windows.Forms.TabPage pauseMenu;
        private System.Windows.Forms.Button newGameMainMenu;
        private System.Windows.Forms.Button exitMainMenu;
        private System.Windows.Forms.Button creditsMainMenu;
        private System.Windows.Forms.Button optionsMainMenu;
        private System.Windows.Forms.Button loginBTLoginMenu;
        private System.Windows.Forms.TextBox passwordLoginMenu;
        private System.Windows.Forms.TextBox loginTBLoginMenu;
        private System.Windows.Forms.Button cancelLoginMenu;
        private System.Windows.Forms.Button registerLoginMenu;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.Button remindPasswordLoginMenu;
        private System.Windows.Forms.Button backRegisterMenu;
        private System.Windows.Forms.Button registerRegisterMenu;
        private System.Windows.Forms.Label emailLRegisterMenu;
        private System.Windows.Forms.Label repeatPasswordLRegisterMenu;
        private System.Windows.Forms.Label passwordLRegisterMenu;
        private System.Windows.Forms.Label loginLBRegisterMenu;
        private System.Windows.Forms.TextBox loginTBRegisterMenu;
        private System.Windows.Forms.TextBox passwordTBRegisterMenu;
        private System.Windows.Forms.TextBox repeatPasswordTBRegisterMenu;
        private System.Windows.Forms.TextBox emailTBRegisterMenu;
        private System.Windows.Forms.Button exitPauseMenu;
        private System.Windows.Forms.Button optionsPauseMenu;
        private System.Windows.Forms.Button continuePauseMenu;
        private System.Windows.Forms.Button exitGameMenu;
        private System.Windows.Forms.Button okGameMenu;
        private System.Windows.Forms.Button optionsGameMenu;
        private System.Windows.Forms.Button pauseGameMenu;
        private System.Windows.Forms.CheckBox muteMusicOptionsMenu;
        private System.Windows.Forms.CheckBox muteSoundOptionsMenu;
        private System.Windows.Forms.Label soundVolumeLOptionsMenu;
        private System.Windows.Forms.Label musicVolumeLOptionsMenu;
        private System.Windows.Forms.NumericUpDown soundVolumeNMOptionsMenu;
        private System.Windows.Forms.NumericUpDown musicVolumeNMOptionsMenu;
        private System.Windows.Forms.Button okOptionsMenu;
        private System.Windows.Forms.Button cancelOptionsMenu;
        private System.Windows.Forms.Button gameChatMenu;
        private System.Windows.Forms.Button backChatMenu;
        private System.Windows.Forms.ListBox chatListChatMenu;
        private System.Windows.Forms.TextBox chatInputTBChatMenu;
        private System.Windows.Forms.ListBox userListChatMenu;
        private System.Windows.Forms.Button backInfoMenu;
        private System.Windows.Forms.Label playerInfoLInfoMenu;
        private System.Windows.Forms.ListBox listOfGames;
        private System.Windows.Forms.Button createChooseGameMenu;
        private System.Windows.Forms.Button joinChooseGameMenu;
        private System.Windows.Forms.Button backChooseGameMenu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxTBGameDescription;
        private System.Windows.Forms.Button cancelCreateGameMenu;
        private System.Windows.Forms.Button createCreateGameMenu;
        private System.Windows.Forms.Label mapsLCreateGameMenu;
        private System.Windows.Forms.ListBox listBoxLBCreateGame;
        private System.Windows.Forms.RadioButton privateCreateGameMenu;
        private System.Windows.Forms.RadioButton publicCreateGameMenu;
        private System.Windows.Forms.Label gameNameLCreateGameMenu;
        private System.Windows.Forms.TextBox gameNameTBCreateGameMenu;
        private System.Windows.Forms.Button startWaitingForPlayersMenu;
        private System.Windows.Forms.Button cancelWaitingForPlayersMenu;
        private System.Windows.Forms.Label descriptionLWaitingForPlayersMenu;
        private System.Windows.Forms.TextBox descriptionWaitingForPlayersMenu;
        private System.Windows.Forms.Label playersLWaitingForPlayersMenu;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private GroupBox groupBoxLogin;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel5;
        private GroupBox groupBox2;
        private Panel panel2;
        private GroupBox groupBox3;
        private GroupBox groupBoxServer;
        private Button sendChatMenu;
        private TextBox textBoxTBGameName;
        private DataGridView dataGridViewPlayers;
        private NumericUpDown maxPlayerNumberNUPCreateGameMenu;
        private Label maxPlayerNumberLCreateGameMenu;
        private ComboBox teamCBWaitingForPlayersMenu;
        private ComboBox houseCBWaitingForPlayersMenu;
        private Label teamLWaitingForPlayersMenu;
        private Label houseLWaitingForPlayersMenu;
        private Button changeWaitingForPlayersMenu;
        private ErrorProvider errorProvider;
        private GroupBox changeGBWaitingForPlayersMenu;
        private Label colorLWaitingForPlayersMenu;
        private Button colorWaitingForPlayersMenu;
        private GroupBox currentDataGBWaitingForPlayersMenu;
        private Label infoLWaitingForPlayersMenu;
        private Button infoColorWaitingForPlayersMenu;
        private Label infoColorLWaitingForPlayersMenu;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Color;
        private DataGridViewTextBoxColumn PlayersName;
        private DataGridViewTextBoxColumn House;
        private DataGridViewTextBoxColumn Team;
        private ComboBox serverLoginMenu;
        private PictureBox pictureBoxHouse;
    }
}