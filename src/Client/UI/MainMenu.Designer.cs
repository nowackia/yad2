using System.Windows.Forms;

namespace Client.UI
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.mainMenu = new System.Windows.Forms.TabPage();
            this.haxxx = new System.Windows.Forms.Button();
            this.newGameMainMenu = new System.Windows.Forms.Button();
            this.exitMainMenu = new System.Windows.Forms.Button();
            this.creditsMainMenu = new System.Windows.Forms.Button();
            this.optionsMainMenu = new System.Windows.Forms.Button();
            this.loginMenu = new System.Windows.Forms.TabPage();
            this.remindPasswordLoginMenu = new System.Windows.Forms.Button();
            this.serverLoginMenu = new System.Windows.Forms.TextBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.loginLabel = new System.Windows.Forms.Label();
            this.cancelLoginMenu = new System.Windows.Forms.Button();
            this.registerLoginMenu = new System.Windows.Forms.Button();
            this.loginBTLoginMenu = new System.Windows.Forms.Button();
            this.passwordLoginMenu = new System.Windows.Forms.TextBox();
            this.loginTBLoginMenu = new System.Windows.Forms.TextBox();
            this.registerMenu = new System.Windows.Forms.TabPage();
            this.emailLRegisterMenu = new System.Windows.Forms.Label();
            this.repeatPasswordLRegisterMenu = new System.Windows.Forms.Label();
            this.passwordLRegisterMenu = new System.Windows.Forms.Label();
            this.loginLBRegisterMenu = new System.Windows.Forms.Label();
            this.loginTBRegisterMenu = new System.Windows.Forms.TextBox();
            this.passwordTBRegisterMenu = new System.Windows.Forms.TextBox();
            this.repeatPasswordTBRegisterMenu = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.backRegisterMenu = new System.Windows.Forms.Button();
            this.registerRegisterMenu = new System.Windows.Forms.Button();
            this.chatMenu = new System.Windows.Forms.TabPage();
            this.userListChatMenu = new System.Windows.Forms.ListBox();
            this.chatListChatMenu = new System.Windows.Forms.ListBox();
            this.chatInputTBChatMenu = new System.Windows.Forms.TextBox();
            this.backChatMenu = new System.Windows.Forms.Button();
            this.gameChatMenu = new System.Windows.Forms.Button();
            this.infoMenu = new System.Windows.Forms.TabPage();
            this.looseValueInfoMenu = new System.Windows.Forms.Label();
            this.wonValueInfoMenu = new System.Windows.Forms.Label();
            this.loginValueInfoMenu = new System.Windows.Forms.Label();
            this.lossLInfoMenu = new System.Windows.Forms.Label();
            this.winLInfoMenu = new System.Windows.Forms.Label();
            this.loginLInfoMenu = new System.Windows.Forms.Label();
            this.backInfoMenu = new System.Windows.Forms.Button();
            this.chooseGameMenu = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.backChooseGameMenu = new System.Windows.Forms.Button();
            this.listOfGames = new System.Windows.Forms.ListBox();
            this.createChooseGameMenu = new System.Windows.Forms.Button();
            this.joinChooseGameMenu = new System.Windows.Forms.Button();
            this.createGameMenu = new System.Windows.Forms.TabPage();
            this.gameNameLCreateGameMenu = new System.Windows.Forms.Label();
            this.gameNameTBCreateGameMenu = new System.Windows.Forms.TextBox();
            this.privateCreateGameMenu = new System.Windows.Forms.RadioButton();
            this.publicCreateGameMenu = new System.Windows.Forms.RadioButton();
            this.mapsLCreateGameMenu = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.cancelCreateGameMenu = new System.Windows.Forms.Button();
            this.createCreateGameMenu = new System.Windows.Forms.Button();
            this.waitingForPlayersMenu = new System.Windows.Forms.TabPage();
            this.descriptionLWaitingForPlayersMenu = new System.Windows.Forms.Label();
            this.descriptionWaitingForPlayersMenu = new System.Windows.Forms.TextBox();
            this.playersLWaitingForPlayersMenu = new System.Windows.Forms.Label();
            this.playerListWaitingForPlayersMenu = new System.Windows.Forms.ListBox();
            this.startWaitingForPlayersMenu = new System.Windows.Forms.Button();
            this.cancelWaitingForPlayersMenu = new System.Windows.Forms.Button();
            this.optionsMenu = new System.Windows.Forms.TabPage();
            this.okOptionsMenu = new System.Windows.Forms.Button();
            this.cancelOptionsMenu = new System.Windows.Forms.Button();
            this.soundVolumeLOptionsMenu = new System.Windows.Forms.Label();
            this.musicVolumeLOptionsMenu = new System.Windows.Forms.Label();
            this.soundVolumeNMOptionsMenu = new System.Windows.Forms.NumericUpDown();
            this.musicVolumeNMOptionsMenu = new System.Windows.Forms.NumericUpDown();
            this.muteSoundOptionsMenu = new System.Windows.Forms.CheckBox();
            this.muteMusicOptionsMenu = new System.Windows.Forms.CheckBox();
            this.gameMenu = new System.Windows.Forms.TabPage();
            this.exitGameMenu = new System.Windows.Forms.Button();
            this.okGameMenu = new System.Windows.Forms.Button();
            this.optionsGameMenu = new System.Windows.Forms.Button();
            this.pauseGameMenu = new System.Windows.Forms.Button();
            this.pauseMenu = new System.Windows.Forms.TabPage();
            this.exitPauseMenu = new System.Windows.Forms.Button();
            this.optionsPauseMenu = new System.Windows.Forms.Button();
            this.continuePauseMenu = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.loginMenu.SuspendLayout();
            this.registerMenu.SuspendLayout();
            this.chatMenu.SuspendLayout();
            this.infoMenu.SuspendLayout();
            this.chooseGameMenu.SuspendLayout();
            this.createGameMenu.SuspendLayout();
            this.waitingForPlayersMenu.SuspendLayout();
            this.optionsMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.soundVolumeNMOptionsMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicVolumeNMOptionsMenu)).BeginInit();
            this.gameMenu.SuspendLayout();
            this.pauseMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl.Controls.Add(this.mainMenu);
            this.tabControl.Controls.Add(this.loginMenu);
            this.tabControl.Controls.Add(this.registerMenu);
            this.tabControl.Controls.Add(this.chatMenu);
            this.tabControl.Controls.Add(this.infoMenu);
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
            this.tabControl.TabIndex = 0;
            // 
            // mainMenu
            // 
            this.mainMenu.Controls.Add(this.haxxx);
            this.mainMenu.Controls.Add(this.newGameMainMenu);
            this.mainMenu.Controls.Add(this.exitMainMenu);
            this.mainMenu.Controls.Add(this.creditsMainMenu);
            this.mainMenu.Controls.Add(this.optionsMainMenu);
            this.mainMenu.Location = new System.Drawing.Point(4, 4);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(3);
            this.mainMenu.Size = new System.Drawing.Size(527, 338);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "MainMenu";
            this.mainMenu.UseVisualStyleBackColor = true;
            // 
            // haxxx
            // 
            this.haxxx.Location = new System.Drawing.Point(351, 242);
            this.haxxx.Name = "haxxx";
            this.haxxx.Size = new System.Drawing.Size(98, 23);
            this.haxxx.TabIndex = 4;
            this.haxxx.Text = "Fast Game";
            this.haxxx.UseVisualStyleBackColor = true;
            this.haxxx.Click += new System.EventHandler(this.haxxx_Click);
            // 
            // newGameMainMenu
            // 
            this.newGameMainMenu.Location = new System.Drawing.Point(190, 64);
            this.newGameMainMenu.Name = "newGameMainMenu";
            this.newGameMainMenu.Size = new System.Drawing.Size(75, 23);
            this.newGameMainMenu.TabIndex = 3;
            this.newGameMainMenu.Text = "New Game";
            this.newGameMainMenu.UseVisualStyleBackColor = true;
            this.newGameMainMenu.Click += new System.EventHandler(this.NewGameMainMenu_Click);
            // 
            // exitMainMenu
            // 
            this.exitMainMenu.Location = new System.Drawing.Point(190, 153);
            this.exitMainMenu.Name = "exitMainMenu";
            this.exitMainMenu.Size = new System.Drawing.Size(75, 23);
            this.exitMainMenu.TabIndex = 2;
            this.exitMainMenu.Text = "Exit";
            this.exitMainMenu.UseVisualStyleBackColor = true;
            this.exitMainMenu.Click += new System.EventHandler(this.exitMainMenu_Click);
            // 
            // creditsMainMenu
            // 
            this.creditsMainMenu.Location = new System.Drawing.Point(190, 123);
            this.creditsMainMenu.Name = "creditsMainMenu";
            this.creditsMainMenu.Size = new System.Drawing.Size(75, 23);
            this.creditsMainMenu.TabIndex = 1;
            this.creditsMainMenu.Text = "Creditzz (Pay us)";
            this.creditsMainMenu.UseVisualStyleBackColor = true;
            // 
            // optionsMainMenu
            // 
            this.optionsMainMenu.Location = new System.Drawing.Point(190, 93);
            this.optionsMainMenu.Name = "optionsMainMenu";
            this.optionsMainMenu.Size = new System.Drawing.Size(75, 23);
            this.optionsMainMenu.TabIndex = 0;
            this.optionsMainMenu.Text = "Options";
            this.optionsMainMenu.UseVisualStyleBackColor = true;
            this.optionsMainMenu.Click += new System.EventHandler(this.OptionsMainMenu_Click);
            // 
            // loginMenu
            // 
            this.loginMenu.Controls.Add(this.remindPasswordLoginMenu);
            this.loginMenu.Controls.Add(this.serverLoginMenu);
            this.loginMenu.Controls.Add(this.serverLabel);
            this.loginMenu.Controls.Add(this.passwordLabel);
            this.loginMenu.Controls.Add(this.loginLabel);
            this.loginMenu.Controls.Add(this.cancelLoginMenu);
            this.loginMenu.Controls.Add(this.registerLoginMenu);
            this.loginMenu.Controls.Add(this.loginBTLoginMenu);
            this.loginMenu.Controls.Add(this.passwordLoginMenu);
            this.loginMenu.Controls.Add(this.loginTBLoginMenu);
            this.loginMenu.Location = new System.Drawing.Point(4, 4);
            this.loginMenu.Name = "loginMenu";
            this.loginMenu.Padding = new System.Windows.Forms.Padding(3);
            this.loginMenu.Size = new System.Drawing.Size(527, 338);
            this.loginMenu.TabIndex = 1;
            this.loginMenu.Text = "LoginMenu";
            this.loginMenu.UseVisualStyleBackColor = true;
            // 
            // remindPasswordLoginMenu
            // 
            this.remindPasswordLoginMenu.Location = new System.Drawing.Point(258, 166);
            this.remindPasswordLoginMenu.Name = "remindPasswordLoginMenu";
            this.remindPasswordLoginMenu.Size = new System.Drawing.Size(106, 23);
            this.remindPasswordLoginMenu.TabIndex = 9;
            this.remindPasswordLoginMenu.Text = "Remind Password";
            this.remindPasswordLoginMenu.UseVisualStyleBackColor = true;
            // 
            // serverLoginMenu
            // 
            this.serverLoginMenu.Location = new System.Drawing.Point(211, 44);
            this.serverLoginMenu.Name = "serverLoginMenu";
            this.serverLoginMenu.Size = new System.Drawing.Size(100, 20);
            this.serverLoginMenu.TabIndex = 8;
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(139, 47);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(36, 13);
            this.serverLabel.TabIndex = 7;
            this.serverLabel.Text = "server";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(139, 99);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(52, 13);
            this.passwordLabel.TabIndex = 6;
            this.passwordLabel.Text = "password";
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.Location = new System.Drawing.Point(139, 73);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(29, 13);
            this.loginLabel.TabIndex = 5;
            this.loginLabel.Text = "login";
            // 
            // cancelLoginMenu
            // 
            this.cancelLoginMenu.Location = new System.Drawing.Point(176, 165);
            this.cancelLoginMenu.Name = "cancelLoginMenu";
            this.cancelLoginMenu.Size = new System.Drawing.Size(75, 23);
            this.cancelLoginMenu.TabIndex = 4;
            this.cancelLoginMenu.Text = "Cancel";
            this.cancelLoginMenu.UseVisualStyleBackColor = true;
            this.cancelLoginMenu.Click += new System.EventHandler(this.cancelLoginMenu_Click);
            // 
            // registerLoginMenu
            // 
            this.registerLoginMenu.Location = new System.Drawing.Point(130, 136);
            this.registerLoginMenu.Name = "registerLoginMenu";
            this.registerLoginMenu.Size = new System.Drawing.Size(75, 23);
            this.registerLoginMenu.TabIndex = 3;
            this.registerLoginMenu.Text = "Register";
            this.registerLoginMenu.UseVisualStyleBackColor = true;
            this.registerLoginMenu.Click += new System.EventHandler(this.registerLoginMenu_Click);
            // 
            // loginBTLoginMenu
            // 
            this.loginBTLoginMenu.Location = new System.Drawing.Point(211, 136);
            this.loginBTLoginMenu.Name = "loginBTLoginMenu";
            this.loginBTLoginMenu.Size = new System.Drawing.Size(75, 23);
            this.loginBTLoginMenu.TabIndex = 2;
            this.loginBTLoginMenu.Text = "Login";
            this.loginBTLoginMenu.UseVisualStyleBackColor = true;
            this.loginBTLoginMenu.Click += new System.EventHandler(this.loginBTLoginMenu_Click);
            // 
            // passwordLoginMenu
            // 
            this.passwordLoginMenu.Location = new System.Drawing.Point(211, 96);
            this.passwordLoginMenu.Name = "passwordLoginMenu";
            this.passwordLoginMenu.Size = new System.Drawing.Size(100, 20);
            this.passwordLoginMenu.TabIndex = 1;
            this.passwordLoginMenu.UseSystemPasswordChar = true;
            // 
            // loginTBLoginMenu
            // 
            this.loginTBLoginMenu.Location = new System.Drawing.Point(211, 70);
            this.loginTBLoginMenu.Name = "loginTBLoginMenu";
            this.loginTBLoginMenu.Size = new System.Drawing.Size(100, 20);
            this.loginTBLoginMenu.TabIndex = 0;
            // 
            // registerMenu
            // 
            this.registerMenu.Controls.Add(this.emailLRegisterMenu);
            this.registerMenu.Controls.Add(this.repeatPasswordLRegisterMenu);
            this.registerMenu.Controls.Add(this.passwordLRegisterMenu);
            this.registerMenu.Controls.Add(this.loginLBRegisterMenu);
            this.registerMenu.Controls.Add(this.loginTBRegisterMenu);
            this.registerMenu.Controls.Add(this.passwordTBRegisterMenu);
            this.registerMenu.Controls.Add(this.repeatPasswordTBRegisterMenu);
            this.registerMenu.Controls.Add(this.textBox1);
            this.registerMenu.Controls.Add(this.backRegisterMenu);
            this.registerMenu.Controls.Add(this.registerRegisterMenu);
            this.registerMenu.Location = new System.Drawing.Point(4, 4);
            this.registerMenu.Name = "registerMenu";
            this.registerMenu.Padding = new System.Windows.Forms.Padding(3);
            this.registerMenu.Size = new System.Drawing.Size(527, 338);
            this.registerMenu.TabIndex = 2;
            this.registerMenu.Text = "RegisterMenu";
            this.registerMenu.UseVisualStyleBackColor = true;
            // 
            // emailLRegisterMenu
            // 
            this.emailLRegisterMenu.AutoSize = true;
            this.emailLRegisterMenu.Location = new System.Drawing.Point(108, 133);
            this.emailLRegisterMenu.Name = "emailLRegisterMenu";
            this.emailLRegisterMenu.Size = new System.Drawing.Size(32, 13);
            this.emailLRegisterMenu.TabIndex = 9;
            this.emailLRegisterMenu.Text = "Email";
            // 
            // repeatPasswordLRegisterMenu
            // 
            this.repeatPasswordLRegisterMenu.AutoSize = true;
            this.repeatPasswordLRegisterMenu.Location = new System.Drawing.Point(108, 107);
            this.repeatPasswordLRegisterMenu.Name = "repeatPasswordLRegisterMenu";
            this.repeatPasswordLRegisterMenu.Size = new System.Drawing.Size(91, 13);
            this.repeatPasswordLRegisterMenu.TabIndex = 8;
            this.repeatPasswordLRegisterMenu.Text = "Repeat Password";
            // 
            // passwordLRegisterMenu
            // 
            this.passwordLRegisterMenu.AutoSize = true;
            this.passwordLRegisterMenu.Location = new System.Drawing.Point(108, 81);
            this.passwordLRegisterMenu.Name = "passwordLRegisterMenu";
            this.passwordLRegisterMenu.Size = new System.Drawing.Size(53, 13);
            this.passwordLRegisterMenu.TabIndex = 7;
            this.passwordLRegisterMenu.Text = "Password";
            // 
            // loginLBRegisterMenu
            // 
            this.loginLBRegisterMenu.AutoSize = true;
            this.loginLBRegisterMenu.Location = new System.Drawing.Point(108, 55);
            this.loginLBRegisterMenu.Name = "loginLBRegisterMenu";
            this.loginLBRegisterMenu.Size = new System.Drawing.Size(33, 13);
            this.loginLBRegisterMenu.TabIndex = 6;
            this.loginLBRegisterMenu.Text = "Login";
            // 
            // loginTBRegisterMenu
            // 
            this.loginTBRegisterMenu.Location = new System.Drawing.Point(220, 52);
            this.loginTBRegisterMenu.Name = "loginTBRegisterMenu";
            this.loginTBRegisterMenu.Size = new System.Drawing.Size(100, 20);
            this.loginTBRegisterMenu.TabIndex = 5;
            // 
            // passwordTBRegisterMenu
            // 
            this.passwordTBRegisterMenu.Location = new System.Drawing.Point(220, 78);
            this.passwordTBRegisterMenu.Name = "passwordTBRegisterMenu";
            this.passwordTBRegisterMenu.Size = new System.Drawing.Size(100, 20);
            this.passwordTBRegisterMenu.TabIndex = 4;
            this.passwordTBRegisterMenu.UseSystemPasswordChar = true;
            // 
            // repeatPasswordTBRegisterMenu
            // 
            this.repeatPasswordTBRegisterMenu.Location = new System.Drawing.Point(220, 104);
            this.repeatPasswordTBRegisterMenu.Name = "repeatPasswordTBRegisterMenu";
            this.repeatPasswordTBRegisterMenu.Size = new System.Drawing.Size(100, 20);
            this.repeatPasswordTBRegisterMenu.TabIndex = 3;
            this.repeatPasswordTBRegisterMenu.UseSystemPasswordChar = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(220, 130);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            // 
            // backRegisterMenu
            // 
            this.backRegisterMenu.Location = new System.Drawing.Point(220, 169);
            this.backRegisterMenu.Name = "backRegisterMenu";
            this.backRegisterMenu.Size = new System.Drawing.Size(75, 23);
            this.backRegisterMenu.TabIndex = 1;
            this.backRegisterMenu.Text = "Back";
            this.backRegisterMenu.UseVisualStyleBackColor = true;
            this.backRegisterMenu.Click += new System.EventHandler(this.backRegisterMenu_Click);
            // 
            // registerRegisterMenu
            // 
            this.registerRegisterMenu.Location = new System.Drawing.Point(138, 169);
            this.registerRegisterMenu.Name = "registerRegisterMenu";
            this.registerRegisterMenu.Size = new System.Drawing.Size(75, 23);
            this.registerRegisterMenu.TabIndex = 0;
            this.registerRegisterMenu.Text = "Register";
            this.registerRegisterMenu.UseVisualStyleBackColor = true;
            this.registerRegisterMenu.Click += new System.EventHandler(this.registerRegisterMenu_Click);
            // 
            // chatMenu
            // 
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
            // userListChatMenu
            // 
            this.userListChatMenu.FormattingEnabled = true;
            this.userListChatMenu.Location = new System.Drawing.Point(360, 31);
            this.userListChatMenu.Name = "userListChatMenu";
            this.userListChatMenu.Size = new System.Drawing.Size(89, 186);
            this.userListChatMenu.TabIndex = 4;
            // 
            // chatListChatMenu
            // 
            this.chatListChatMenu.FormattingEnabled = true;
            this.chatListChatMenu.Location = new System.Drawing.Point(7, 31);
            this.chatListChatMenu.Name = "chatListChatMenu";
            this.chatListChatMenu.Size = new System.Drawing.Size(347, 186);
            this.chatListChatMenu.TabIndex = 3;
            // 
            // chatInputTBChatMenu
            // 
            this.chatInputTBChatMenu.Location = new System.Drawing.Point(7, 223);
            this.chatInputTBChatMenu.Name = "chatInputTBChatMenu";
            this.chatInputTBChatMenu.Size = new System.Drawing.Size(347, 20);
            this.chatInputTBChatMenu.TabIndex = 2;
            // 
            // backChatMenu
            // 
            this.backChatMenu.Location = new System.Drawing.Point(7, 6);
            this.backChatMenu.Name = "backChatMenu";
            this.backChatMenu.Size = new System.Drawing.Size(75, 23);
            this.backChatMenu.TabIndex = 1;
            this.backChatMenu.Text = "Back";
            this.backChatMenu.UseVisualStyleBackColor = true;
            this.backChatMenu.Click += new System.EventHandler(this.backChatMenu_Click);
            // 
            // gameChatMenu
            // 
            this.gameChatMenu.Location = new System.Drawing.Point(88, 6);
            this.gameChatMenu.Name = "gameChatMenu";
            this.gameChatMenu.Size = new System.Drawing.Size(75, 23);
            this.gameChatMenu.TabIndex = 0;
            this.gameChatMenu.Text = "Game";
            this.gameChatMenu.UseVisualStyleBackColor = true;
            this.gameChatMenu.Click += new System.EventHandler(this.gameChatMenu_Click);
            // 
            // infoMenu
            // 
            this.infoMenu.Controls.Add(this.looseValueInfoMenu);
            this.infoMenu.Controls.Add(this.wonValueInfoMenu);
            this.infoMenu.Controls.Add(this.loginValueInfoMenu);
            this.infoMenu.Controls.Add(this.lossLInfoMenu);
            this.infoMenu.Controls.Add(this.winLInfoMenu);
            this.infoMenu.Controls.Add(this.loginLInfoMenu);
            this.infoMenu.Controls.Add(this.backInfoMenu);
            this.infoMenu.Location = new System.Drawing.Point(4, 4);
            this.infoMenu.Name = "infoMenu";
            this.infoMenu.Padding = new System.Windows.Forms.Padding(3);
            this.infoMenu.Size = new System.Drawing.Size(527, 338);
            this.infoMenu.TabIndex = 4;
            this.infoMenu.Text = "InfoMenu";
            this.infoMenu.UseVisualStyleBackColor = true;
            // 
            // looseValueInfoMenu
            // 
            this.looseValueInfoMenu.AutoSize = true;
            this.looseValueInfoMenu.Location = new System.Drawing.Point(263, 141);
            this.looseValueInfoMenu.Name = "looseValueInfoMenu";
            this.looseValueInfoMenu.Size = new System.Drawing.Size(13, 13);
            this.looseValueInfoMenu.TabIndex = 6;
            this.looseValueInfoMenu.Text = "2";
            // 
            // wonValueInfoMenu
            // 
            this.wonValueInfoMenu.AutoSize = true;
            this.wonValueInfoMenu.Location = new System.Drawing.Point(263, 114);
            this.wonValueInfoMenu.Name = "wonValueInfoMenu";
            this.wonValueInfoMenu.Size = new System.Drawing.Size(25, 13);
            this.wonValueInfoMenu.TabIndex = 5;
            this.wonValueInfoMenu.Text = "666";
            // 
            // loginValueInfoMenu
            // 
            this.loginValueInfoMenu.AutoSize = true;
            this.loginValueInfoMenu.Location = new System.Drawing.Point(263, 86);
            this.loginValueInfoMenu.Name = "loginValueInfoMenu";
            this.loginValueInfoMenu.Size = new System.Drawing.Size(38, 13);
            this.loginValueInfoMenu.TabIndex = 4;
            this.loginValueInfoMenu.Text = "Golota";
            // 
            // lossLInfoMenu
            // 
            this.lossLInfoMenu.AutoSize = true;
            this.lossLInfoMenu.Location = new System.Drawing.Point(164, 141);
            this.lossLInfoMenu.Name = "lossLInfoMenu";
            this.lossLInfoMenu.Size = new System.Drawing.Size(29, 13);
            this.lossLInfoMenu.TabIndex = 3;
            this.lossLInfoMenu.Text = "Loss";
            // 
            // winLInfoMenu
            // 
            this.winLInfoMenu.AutoSize = true;
            this.winLInfoMenu.Location = new System.Drawing.Point(164, 114);
            this.winLInfoMenu.Name = "winLInfoMenu";
            this.winLInfoMenu.Size = new System.Drawing.Size(26, 13);
            this.winLInfoMenu.TabIndex = 2;
            this.winLInfoMenu.Text = "Win";
            // 
            // loginLInfoMenu
            // 
            this.loginLInfoMenu.AutoSize = true;
            this.loginLInfoMenu.Location = new System.Drawing.Point(164, 86);
            this.loginLInfoMenu.Name = "loginLInfoMenu";
            this.loginLInfoMenu.Size = new System.Drawing.Size(33, 13);
            this.loginLInfoMenu.TabIndex = 1;
            this.loginLInfoMenu.Text = "Login";
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
            this.chooseGameMenu.Controls.Add(this.label1);
            this.chooseGameMenu.Controls.Add(this.textBox2);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(249, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "description";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(249, 27);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(199, 209);
            this.textBox2.TabIndex = 4;
            // 
            // backChooseGameMenu
            // 
            this.backChooseGameMenu.Location = new System.Drawing.Point(6, 242);
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
            this.listOfGames.Location = new System.Drawing.Point(6, 11);
            this.listOfGames.Name = "listOfGames";
            this.listOfGames.Size = new System.Drawing.Size(237, 225);
            this.listOfGames.TabIndex = 2;
            this.listOfGames.SelectedIndexChanged += new System.EventHandler(this.listOfGames_SelectedIndexChanged);
            // 
            // createChooseGameMenu
            // 
            this.createChooseGameMenu.Location = new System.Drawing.Point(168, 242);
            this.createChooseGameMenu.Name = "createChooseGameMenu";
            this.createChooseGameMenu.Size = new System.Drawing.Size(75, 23);
            this.createChooseGameMenu.TabIndex = 1;
            this.createChooseGameMenu.Text = "Create";
            this.createChooseGameMenu.UseVisualStyleBackColor = true;
            this.createChooseGameMenu.Click += new System.EventHandler(this.createChooseGameMenu_Click);
            // 
            // joinChooseGameMenu
            // 
            this.joinChooseGameMenu.Location = new System.Drawing.Point(87, 242);
            this.joinChooseGameMenu.Name = "joinChooseGameMenu";
            this.joinChooseGameMenu.Size = new System.Drawing.Size(75, 23);
            this.joinChooseGameMenu.TabIndex = 0;
            this.joinChooseGameMenu.Text = "Join";
            this.joinChooseGameMenu.UseVisualStyleBackColor = true;
            this.joinChooseGameMenu.Click += new System.EventHandler(this.joinChooseGameMenu_Click);
            // 
            // createGameMenu
            // 
            this.createGameMenu.Controls.Add(this.gameNameLCreateGameMenu);
            this.createGameMenu.Controls.Add(this.gameNameTBCreateGameMenu);
            this.createGameMenu.Controls.Add(this.privateCreateGameMenu);
            this.createGameMenu.Controls.Add(this.publicCreateGameMenu);
            this.createGameMenu.Controls.Add(this.mapsLCreateGameMenu);
            this.createGameMenu.Controls.Add(this.listBox1);
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
            // gameNameLCreateGameMenu
            // 
            this.gameNameLCreateGameMenu.AutoSize = true;
            this.gameNameLCreateGameMenu.Location = new System.Drawing.Point(232, 53);
            this.gameNameLCreateGameMenu.Name = "gameNameLCreateGameMenu";
            this.gameNameLCreateGameMenu.Size = new System.Drawing.Size(66, 13);
            this.gameNameLCreateGameMenu.TabIndex = 7;
            this.gameNameLCreateGameMenu.Text = "Game Name";
            // 
            // gameNameTBCreateGameMenu
            // 
            this.gameNameTBCreateGameMenu.Location = new System.Drawing.Point(232, 69);
            this.gameNameTBCreateGameMenu.Name = "gameNameTBCreateGameMenu";
            this.gameNameTBCreateGameMenu.Size = new System.Drawing.Size(217, 20);
            this.gameNameTBCreateGameMenu.TabIndex = 6;
            // 
            // privateCreateGameMenu
            // 
            this.privateCreateGameMenu.AutoSize = true;
            this.privateCreateGameMenu.Location = new System.Drawing.Point(235, 129);
            this.privateCreateGameMenu.Name = "privateCreateGameMenu";
            this.privateCreateGameMenu.Size = new System.Drawing.Size(58, 17);
            this.privateCreateGameMenu.TabIndex = 5;
            this.privateCreateGameMenu.TabStop = true;
            this.privateCreateGameMenu.Text = "Private";
            this.privateCreateGameMenu.UseVisualStyleBackColor = true;
            // 
            // publicCreateGameMenu
            // 
            this.publicCreateGameMenu.AutoSize = true;
            this.publicCreateGameMenu.Location = new System.Drawing.Point(235, 105);
            this.publicCreateGameMenu.Name = "publicCreateGameMenu";
            this.publicCreateGameMenu.Size = new System.Drawing.Size(54, 17);
            this.publicCreateGameMenu.TabIndex = 4;
            this.publicCreateGameMenu.TabStop = true;
            this.publicCreateGameMenu.Text = "Public";
            this.publicCreateGameMenu.UseVisualStyleBackColor = true;
            // 
            // mapsLCreateGameMenu
            // 
            this.mapsLCreateGameMenu.AutoSize = true;
            this.mapsLCreateGameMenu.Location = new System.Drawing.Point(6, 7);
            this.mapsLCreateGameMenu.Name = "mapsLCreateGameMenu";
            this.mapsLCreateGameMenu.Size = new System.Drawing.Size(67, 13);
            this.mapsLCreateGameMenu.TabIndex = 3;
            this.mapsLCreateGameMenu.Text = "Choose Map";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(9, 23);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(217, 199);
            this.listBox1.TabIndex = 2;
            // 
            // cancelCreateGameMenu
            // 
            this.cancelCreateGameMenu.Location = new System.Drawing.Point(154, 239);
            this.cancelCreateGameMenu.Name = "cancelCreateGameMenu";
            this.cancelCreateGameMenu.Size = new System.Drawing.Size(75, 23);
            this.cancelCreateGameMenu.TabIndex = 1;
            this.cancelCreateGameMenu.Text = "Cancel";
            this.cancelCreateGameMenu.UseVisualStyleBackColor = true;
            this.cancelCreateGameMenu.Click += new System.EventHandler(this.cancelCreateGameMenu_Click);
            // 
            // createCreateGameMenu
            // 
            this.createCreateGameMenu.Location = new System.Drawing.Point(235, 239);
            this.createCreateGameMenu.Name = "createCreateGameMenu";
            this.createCreateGameMenu.Size = new System.Drawing.Size(75, 23);
            this.createCreateGameMenu.TabIndex = 0;
            this.createCreateGameMenu.Text = "Create";
            this.createCreateGameMenu.UseVisualStyleBackColor = true;
            this.createCreateGameMenu.Click += new System.EventHandler(this.createCreateGameMenu_Click);
            // 
            // waitingForPlayersMenu
            // 
            this.waitingForPlayersMenu.Controls.Add(this.descriptionLWaitingForPlayersMenu);
            this.waitingForPlayersMenu.Controls.Add(this.descriptionWaitingForPlayersMenu);
            this.waitingForPlayersMenu.Controls.Add(this.playersLWaitingForPlayersMenu);
            this.waitingForPlayersMenu.Controls.Add(this.playerListWaitingForPlayersMenu);
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
            // descriptionLWaitingForPlayersMenu
            // 
            this.descriptionLWaitingForPlayersMenu.AutoSize = true;
            this.descriptionLWaitingForPlayersMenu.Location = new System.Drawing.Point(238, 64);
            this.descriptionLWaitingForPlayersMenu.Name = "descriptionLWaitingForPlayersMenu";
            this.descriptionLWaitingForPlayersMenu.Size = new System.Drawing.Size(60, 13);
            this.descriptionLWaitingForPlayersMenu.TabIndex = 5;
            this.descriptionLWaitingForPlayersMenu.Text = "Description";
            // 
            // descriptionWaitingForPlayersMenu
            // 
            this.descriptionWaitingForPlayersMenu.Location = new System.Drawing.Point(238, 89);
            this.descriptionWaitingForPlayersMenu.Multiline = true;
            this.descriptionWaitingForPlayersMenu.Name = "descriptionWaitingForPlayersMenu";
            this.descriptionWaitingForPlayersMenu.Size = new System.Drawing.Size(211, 147);
            this.descriptionWaitingForPlayersMenu.TabIndex = 4;
            // 
            // playersLWaitingForPlayersMenu
            // 
            this.playersLWaitingForPlayersMenu.AutoSize = true;
            this.playersLWaitingForPlayersMenu.Location = new System.Drawing.Point(6, 64);
            this.playersLWaitingForPlayersMenu.Name = "playersLWaitingForPlayersMenu";
            this.playersLWaitingForPlayersMenu.Size = new System.Drawing.Size(41, 13);
            this.playersLWaitingForPlayersMenu.TabIndex = 3;
            this.playersLWaitingForPlayersMenu.Text = "Players";
            // 
            // playerListWaitingForPlayersMenu
            // 
            this.playerListWaitingForPlayersMenu.FormattingEnabled = true;
            this.playerListWaitingForPlayersMenu.Location = new System.Drawing.Point(6, 89);
            this.playerListWaitingForPlayersMenu.Name = "playerListWaitingForPlayersMenu";
            this.playerListWaitingForPlayersMenu.Size = new System.Drawing.Size(225, 147);
            this.playerListWaitingForPlayersMenu.TabIndex = 2;
            // 
            // startWaitingForPlayersMenu
            // 
            this.startWaitingForPlayersMenu.Location = new System.Drawing.Point(238, 242);
            this.startWaitingForPlayersMenu.Name = "startWaitingForPlayersMenu";
            this.startWaitingForPlayersMenu.Size = new System.Drawing.Size(75, 23);
            this.startWaitingForPlayersMenu.TabIndex = 1;
            this.startWaitingForPlayersMenu.Text = "Start Game";
            this.startWaitingForPlayersMenu.UseVisualStyleBackColor = true;
            this.startWaitingForPlayersMenu.Click += new System.EventHandler(this.startWaitingForPlayersMenu_Click);
            // 
            // cancelWaitingForPlayersMenu
            // 
            this.cancelWaitingForPlayersMenu.Location = new System.Drawing.Point(156, 242);
            this.cancelWaitingForPlayersMenu.Name = "cancelWaitingForPlayersMenu";
            this.cancelWaitingForPlayersMenu.Size = new System.Drawing.Size(75, 23);
            this.cancelWaitingForPlayersMenu.TabIndex = 0;
            this.cancelWaitingForPlayersMenu.Text = "Cancel";
            this.cancelWaitingForPlayersMenu.UseVisualStyleBackColor = true;
            this.cancelWaitingForPlayersMenu.Click += new System.EventHandler(this.cancelWaitingForPlayersMenu_Click);
            // 
            // optionsMenu
            // 
            this.optionsMenu.Controls.Add(this.okOptionsMenu);
            this.optionsMenu.Controls.Add(this.cancelOptionsMenu);
            this.optionsMenu.Controls.Add(this.soundVolumeLOptionsMenu);
            this.optionsMenu.Controls.Add(this.musicVolumeLOptionsMenu);
            this.optionsMenu.Controls.Add(this.soundVolumeNMOptionsMenu);
            this.optionsMenu.Controls.Add(this.musicVolumeNMOptionsMenu);
            this.optionsMenu.Controls.Add(this.muteSoundOptionsMenu);
            this.optionsMenu.Controls.Add(this.muteMusicOptionsMenu);
            this.optionsMenu.Location = new System.Drawing.Point(4, 4);
            this.optionsMenu.Name = "optionsMenu";
            this.optionsMenu.Padding = new System.Windows.Forms.Padding(3);
            this.optionsMenu.Size = new System.Drawing.Size(527, 338);
            this.optionsMenu.TabIndex = 8;
            this.optionsMenu.Text = "OptionsMenu";
            this.optionsMenu.UseVisualStyleBackColor = true;
            // 
            // okOptionsMenu
            // 
            this.okOptionsMenu.Location = new System.Drawing.Point(252, 120);
            this.okOptionsMenu.Name = "okOptionsMenu";
            this.okOptionsMenu.Size = new System.Drawing.Size(75, 23);
            this.okOptionsMenu.TabIndex = 7;
            this.okOptionsMenu.Text = "Ok";
            this.okOptionsMenu.UseVisualStyleBackColor = true;
            this.okOptionsMenu.Click += new System.EventHandler(this.okOptionsMenu_Click);
            // 
            // cancelOptionsMenu
            // 
            this.cancelOptionsMenu.Location = new System.Drawing.Point(140, 120);
            this.cancelOptionsMenu.Name = "cancelOptionsMenu";
            this.cancelOptionsMenu.Size = new System.Drawing.Size(75, 23);
            this.cancelOptionsMenu.TabIndex = 6;
            this.cancelOptionsMenu.Text = "Cancel";
            this.cancelOptionsMenu.UseVisualStyleBackColor = true;
            this.cancelOptionsMenu.Click += new System.EventHandler(this.cancelOptionsMenu_Click);
            // 
            // soundVolumeLOptionsMenu
            // 
            this.soundVolumeLOptionsMenu.AutoSize = true;
            this.soundVolumeLOptionsMenu.Location = new System.Drawing.Point(98, 71);
            this.soundVolumeLOptionsMenu.Name = "soundVolumeLOptionsMenu";
            this.soundVolumeLOptionsMenu.Size = new System.Drawing.Size(76, 13);
            this.soundVolumeLOptionsMenu.TabIndex = 5;
            this.soundVolumeLOptionsMenu.Text = "Sound Volume";
            // 
            // musicVolumeLOptionsMenu
            // 
            this.musicVolumeLOptionsMenu.AutoSize = true;
            this.musicVolumeLOptionsMenu.Location = new System.Drawing.Point(98, 46);
            this.musicVolumeLOptionsMenu.Name = "musicVolumeLOptionsMenu";
            this.musicVolumeLOptionsMenu.Size = new System.Drawing.Size(73, 13);
            this.musicVolumeLOptionsMenu.TabIndex = 4;
            this.musicVolumeLOptionsMenu.Text = "Music Volume";
            // 
            // soundVolumeNMOptionsMenu
            // 
            this.soundVolumeNMOptionsMenu.Location = new System.Drawing.Point(180, 69);
            this.soundVolumeNMOptionsMenu.Name = "soundVolumeNMOptionsMenu";
            this.soundVolumeNMOptionsMenu.Size = new System.Drawing.Size(120, 20);
            this.soundVolumeNMOptionsMenu.TabIndex = 3;
            // 
            // musicVolumeNMOptionsMenu
            // 
            this.musicVolumeNMOptionsMenu.Location = new System.Drawing.Point(180, 42);
            this.musicVolumeNMOptionsMenu.Name = "musicVolumeNMOptionsMenu";
            this.musicVolumeNMOptionsMenu.Size = new System.Drawing.Size(120, 20);
            this.musicVolumeNMOptionsMenu.TabIndex = 2;
            // 
            // muteSoundOptionsMenu
            // 
            this.muteSoundOptionsMenu.AutoSize = true;
            this.muteSoundOptionsMenu.Location = new System.Drawing.Point(322, 72);
            this.muteSoundOptionsMenu.Name = "muteSoundOptionsMenu";
            this.muteSoundOptionsMenu.Size = new System.Drawing.Size(84, 17);
            this.muteSoundOptionsMenu.TabIndex = 1;
            this.muteSoundOptionsMenu.Text = "Mute Sound";
            this.muteSoundOptionsMenu.UseVisualStyleBackColor = true;
            // 
            // muteMusicOptionsMenu
            // 
            this.muteMusicOptionsMenu.AutoSize = true;
            this.muteMusicOptionsMenu.Location = new System.Drawing.Point(322, 43);
            this.muteMusicOptionsMenu.Name = "muteMusicOptionsMenu";
            this.muteMusicOptionsMenu.Size = new System.Drawing.Size(81, 17);
            this.muteMusicOptionsMenu.TabIndex = 0;
            this.muteMusicOptionsMenu.Text = "Mute Music";
            this.muteMusicOptionsMenu.UseVisualStyleBackColor = true;
            // 
            // gameMenu
            // 
            this.gameMenu.Controls.Add(this.exitGameMenu);
            this.gameMenu.Controls.Add(this.okGameMenu);
            this.gameMenu.Controls.Add(this.optionsGameMenu);
            this.gameMenu.Controls.Add(this.pauseGameMenu);
            this.gameMenu.Location = new System.Drawing.Point(4, 4);
            this.gameMenu.Name = "gameMenu";
            this.gameMenu.Padding = new System.Windows.Forms.Padding(3);
            this.gameMenu.Size = new System.Drawing.Size(527, 338);
            this.gameMenu.TabIndex = 9;
            this.gameMenu.Text = "GameMenu";
            this.gameMenu.UseVisualStyleBackColor = true;
            // 
            // exitGameMenu
            // 
            this.exitGameMenu.Location = new System.Drawing.Point(199, 131);
            this.exitGameMenu.Name = "exitGameMenu";
            this.exitGameMenu.Size = new System.Drawing.Size(75, 23);
            this.exitGameMenu.TabIndex = 3;
            this.exitGameMenu.Text = "Exit";
            this.exitGameMenu.UseVisualStyleBackColor = true;
            this.exitGameMenu.Click += new System.EventHandler(this.exitGameMenu_Click);
            // 
            // okGameMenu
            // 
            this.okGameMenu.Location = new System.Drawing.Point(199, 101);
            this.okGameMenu.Name = "okGameMenu";
            this.okGameMenu.Size = new System.Drawing.Size(75, 23);
            this.okGameMenu.TabIndex = 2;
            this.okGameMenu.Text = "Ok";
            this.okGameMenu.UseVisualStyleBackColor = true;
            this.okGameMenu.Click += new System.EventHandler(this.okGameMenu_Click);
            // 
            // optionsGameMenu
            // 
            this.optionsGameMenu.Location = new System.Drawing.Point(199, 71);
            this.optionsGameMenu.Name = "optionsGameMenu";
            this.optionsGameMenu.Size = new System.Drawing.Size(75, 23);
            this.optionsGameMenu.TabIndex = 1;
            this.optionsGameMenu.Text = "Options";
            this.optionsGameMenu.UseVisualStyleBackColor = true;
            this.optionsGameMenu.Click += new System.EventHandler(this.optionsGameMenu_Click);
            // 
            // pauseGameMenu
            // 
            this.pauseGameMenu.Location = new System.Drawing.Point(199, 41);
            this.pauseGameMenu.Name = "pauseGameMenu";
            this.pauseGameMenu.Size = new System.Drawing.Size(75, 23);
            this.pauseGameMenu.TabIndex = 0;
            this.pauseGameMenu.Text = "Pause";
            this.pauseGameMenu.UseVisualStyleBackColor = true;
            this.pauseGameMenu.Click += new System.EventHandler(this.pauseGameMenu_Click);
            // 
            // pauseMenu
            // 
            this.pauseMenu.Controls.Add(this.exitPauseMenu);
            this.pauseMenu.Controls.Add(this.optionsPauseMenu);
            this.pauseMenu.Controls.Add(this.continuePauseMenu);
            this.pauseMenu.Location = new System.Drawing.Point(4, 4);
            this.pauseMenu.Name = "pauseMenu";
            this.pauseMenu.Padding = new System.Windows.Forms.Padding(3);
            this.pauseMenu.Size = new System.Drawing.Size(527, 338);
            this.pauseMenu.TabIndex = 10;
            this.pauseMenu.Text = "PauseMenu";
            this.pauseMenu.UseVisualStyleBackColor = true;
            // 
            // exitPauseMenu
            // 
            this.exitPauseMenu.Location = new System.Drawing.Point(197, 113);
            this.exitPauseMenu.Name = "exitPauseMenu";
            this.exitPauseMenu.Size = new System.Drawing.Size(75, 23);
            this.exitPauseMenu.TabIndex = 2;
            this.exitPauseMenu.Text = "Exit";
            this.exitPauseMenu.UseVisualStyleBackColor = true;
            this.exitPauseMenu.Click += new System.EventHandler(this.exitPauseMenu_Click);
            // 
            // optionsPauseMenu
            // 
            this.optionsPauseMenu.Location = new System.Drawing.Point(197, 84);
            this.optionsPauseMenu.Name = "optionsPauseMenu";
            this.optionsPauseMenu.Size = new System.Drawing.Size(75, 23);
            this.optionsPauseMenu.TabIndex = 1;
            this.optionsPauseMenu.Text = "Options";
            this.optionsPauseMenu.UseVisualStyleBackColor = true;
            this.optionsPauseMenu.Click += new System.EventHandler(this.optionsPauseMenu_Click);
            // 
            // continuePauseMenu
            // 
            this.continuePauseMenu.Location = new System.Drawing.Point(197, 55);
            this.continuePauseMenu.Name = "continuePauseMenu";
            this.continuePauseMenu.Size = new System.Drawing.Size(75, 23);
            this.continuePauseMenu.TabIndex = 0;
            this.continuePauseMenu.Text = "Continue";
            this.continuePauseMenu.UseVisualStyleBackColor = true;
            this.continuePauseMenu.Click += new System.EventHandler(this.continuePauseMenu_Click);
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 397);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "MainMenuForm";
            this.Text = "MainMenu";
            this.tabControl.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.loginMenu.ResumeLayout(false);
            this.loginMenu.PerformLayout();
            this.registerMenu.ResumeLayout(false);
            this.registerMenu.PerformLayout();
            this.chatMenu.ResumeLayout(false);
            this.chatMenu.PerformLayout();
            this.infoMenu.ResumeLayout(false);
            this.infoMenu.PerformLayout();
            this.chooseGameMenu.ResumeLayout(false);
            this.chooseGameMenu.PerformLayout();
            this.createGameMenu.ResumeLayout(false);
            this.createGameMenu.PerformLayout();
            this.waitingForPlayersMenu.ResumeLayout(false);
            this.waitingForPlayersMenu.PerformLayout();
            this.optionsMenu.ResumeLayout(false);
            this.optionsMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.soundVolumeNMOptionsMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicVolumeNMOptionsMenu)).EndInit();
            this.gameMenu.ResumeLayout(false);
            this.pauseMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage mainMenu;
        private System.Windows.Forms.TabPage loginMenu;
        private System.Windows.Forms.TabPage registerMenu;
        private System.Windows.Forms.TabPage chatMenu;
        private System.Windows.Forms.TabPage infoMenu;
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
        private System.Windows.Forms.TextBox serverLoginMenu;
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
        private System.Windows.Forms.TextBox textBox1;
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
        private System.Windows.Forms.Button haxxx;
        private System.Windows.Forms.Button backInfoMenu;
        private System.Windows.Forms.Label loginValueInfoMenu;
        private System.Windows.Forms.Label lossLInfoMenu;
        private System.Windows.Forms.Label winLInfoMenu;
        private System.Windows.Forms.Label loginLInfoMenu;
        private System.Windows.Forms.Label looseValueInfoMenu;
        private System.Windows.Forms.Label wonValueInfoMenu;
        private System.Windows.Forms.ListBox listOfGames;
        private System.Windows.Forms.Button createChooseGameMenu;
        private System.Windows.Forms.Button joinChooseGameMenu;
        private System.Windows.Forms.Button backChooseGameMenu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button cancelCreateGameMenu;
        private System.Windows.Forms.Button createCreateGameMenu;
        private System.Windows.Forms.Label mapsLCreateGameMenu;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.RadioButton privateCreateGameMenu;
        private System.Windows.Forms.RadioButton publicCreateGameMenu;
        private System.Windows.Forms.Label gameNameLCreateGameMenu;
        private System.Windows.Forms.TextBox gameNameTBCreateGameMenu;
        private System.Windows.Forms.Button startWaitingForPlayersMenu;
        private System.Windows.Forms.Button cancelWaitingForPlayersMenu;
        private System.Windows.Forms.Label descriptionLWaitingForPlayersMenu;
        private System.Windows.Forms.TextBox descriptionWaitingForPlayersMenu;
        private System.Windows.Forms.Label playersLWaitingForPlayersMenu;
        private System.Windows.Forms.ListBox playerListWaitingForPlayersMenu;
    }
}