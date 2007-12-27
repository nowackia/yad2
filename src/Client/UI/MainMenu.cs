using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Yad.Config;
using Yad.Config.INILoader.Common;
using Yad.Engine.Client;
using Yad.Engine.Common;
using Yad.Log.Common;
using Yad.Net.Client;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.Properties;
using Yad.Properties.Client;
using Yad.UI.Common;


namespace Yad.UI.Client
{
    public partial class MainMenuForm : UIManageable
    {
        private Dictionary<Views, TabPage> views = new Dictionary<Views, TabPage>();
        private Views lastView;
        private MenuMessageHandler menuMessageHandler;

        public MainMenuForm()
        {
            InitializeComponent();

            #region Appearance Initialization
            this.tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.MinimumSize = this.MaximumSize = this.Size = new Size(Width, 373);
            #endregion

            #region Controls Initialization
            views.Add(Views.ChatForm, chatMenu);
            views.Add(Views.ChooseGameForm, chooseGameMenu);
            views.Add(Views.CreateGameForm, createGameMenu);
            views.Add(Views.GameMenuForm, gameMenu);
            views.Add(Views.LoginForm, loginMenu);
            views.Add(Views.MainMenuForm, mainMenu);
            views.Add(Views.OptionsForm, optionsMenu);
            views.Add(Views.PauseForm, pauseMenu);
            views.Add(Views.RegistrationForm, registerMenu);
            views.Add(Views.UserInfoForm, playerInfoMenu);
            views.Add(Views.WaitingForPlayersForm, waitingForPlayersMenu);

            ResetMapFileNames(listBoxLBCreateGame);
            LoadInitializationSettings();
            #endregion

            #region MenuMessageHandler Settings
            menuMessageHandler = new MenuMessageHandler();
            menuMessageHandler.Suspend();

            menuMessageHandler.LoginRequestReply += new RequestReplyEventHandler(menuMessageHandler_LoginRequestReply);
            menuMessageHandler.RegisterRequestReply += new RequestReplyEventHandler(menuMessageHandler_RegisterRequestReply);
            menuMessageHandler.RemindRequestReply += new RequestReplyEventHandler(menuMessageHandler_RemindRequestReply);

            menuMessageHandler.ResetChatUsers += new ChatEventHandler(menuMessageHandler_ResetChatUsers);
            menuMessageHandler.NewChatUsers += new ChatEventHandler(menuMessageHandler_AddChatUsers);
            menuMessageHandler.DeleteChatUsers += new ChatEventHandler(menuMessageHandler_DeleteChatUsers);
            menuMessageHandler.ChatTextReceive += new ChatEventHandler(menuMessageHandler_ChatTextReceive);

            menuMessageHandler.PlayerInfoRequestReply += new RequestReplyEventHandler(menuMessageHandler_PlayerInfoRequestReply);

            menuMessageHandler.ResetGamesInfo += new GamesEventHandler(menuMessageHandler_ResetGamesInfo);
            menuMessageHandler.NewGamesInfo += new GamesEventHandler(menuMessageHandler_NewGamesInfo);
            menuMessageHandler.DeleteGamesInfo += new GamesEventHandler(menuMessageHandler_DeleteGamesInfo);

            menuMessageHandler.CreateGameRequestReply += new RequestReplyEventHandler(menuMessageHandler_CreateGameRequestReply);
            menuMessageHandler.JoinGameRequestReply += new RequestReplyEventHandler(menuMessageHandler_JoinGameRequestReply);

            menuMessageHandler.GameParamsRequestReply += new RequestReplyEventHandler(menuMessageHandler_GameParamsRequestReply);

            menuMessageHandler.ResetPlayers += new PlayersEventHandler(menuMessageHandler_ResetPlayers);
            menuMessageHandler.NewPlayers += new PlayersEventHandler(menuMessageHandler_NewPlayers);
            menuMessageHandler.DeletePlayers += new PlayersEventHandler(menuMessageHandler_DeletePlayers);
            menuMessageHandler.UpdatePlayers += new PlayersEventHandler(menuMessageHandler_UpdatePlayers);

            menuMessageHandler.StartGameRequestReply += new RequestReplyEventHandler(menuMessageHandler_StartGameRequestReply);

            Connection.Instance.MessageHandler = menuMessageHandler;
            menuMessageHandler.Resume();

            Connection.Instance.ConnectionLost += new ConnectionLostEventHandler(ConnectionInstance_ConnectionLost);
            #endregion
        }

        #region Controls managment
        public MenuMessageHandler MenuMessageHandler
        {
            get { return menuMessageHandler; }
            set { menuMessageHandler = value; }
        }

        public Views LastView
        {
            get { return lastView; }
            set { lastView = value; }
        }

        public void SwitchToTab(Views view)
        {
            TabPage page = views[view];
            if (page == null)
                throw new NotImplementedException("View " + view + " not exist");
            
            if(this.InvokeRequired)
                this.Invoke(new SelectTabEventHandler(tabControl.SelectTab), new object[] { page.Name });
            else
                tabControl.SelectTab(page.Name);
        }

        public void LoadInitializationSettings()
        {
            soundVolumeNMOptionsMenu.Value = InitializationSettings.Instance.SoundVolume;
            muteSoundOptionsMenu.Checked = InitializationSettings.Instance.IsSoundMuted;
            musicVolumeNMOptionsMenu.Value = InitializationSettings.Instance.MusicVolume;
            muteMusicOptionsMenu.Checked = InitializationSettings.Instance.IsMusicMuted;

            loginTBLoginMenu.Text = InitializationSettings.Instance.Login;

            serverLoginMenu.Items.AddRange(InitializationSettings.Instance.ServerIPs);
            if(serverLoginMenu.Items.Count > 0)
                serverLoginMenu.SelectedItem = serverLoginMenu.Items[serverLoginMenu.Items.Count - 1];
        }

        public void ResetMapFileNames(ListBox listBox)
        {
            listBox.Items.Clear();

            DirectoryInfo directoryInfo = new DirectoryInfo(Settings.Default.Maps);
            listBox.Items.AddRange(directoryInfo.GetFiles("*.map"));
            if(listBox.Items.Count > 0)
                listBox.SelectedIndex = 0;
        }

        public void ManageControlState(Control[] control, bool state)
        {
            for (int i = 0; i < control.Length; i++)
                control[i].Enabled = state;
        }

        public void ManageListBox(ListBox listBox, object[] objects, bool reset)
        {
            if (reset)
                listBox.Items.Clear();

            for (int i = 0; i < objects.Length; i++)
                listBox.Items.Add(objects[i]);
        }

        public void RemoveListBoxMono(ListBox listBox, object removeObject)
        {
            listBox.Items.Remove(removeObject);
        }

        public void RemoveListBox(ListBox listBox, object[] removeObject)
        {
            for(int i = 0; i < removeObject.Length; i++)
                listBox.Items.Remove(removeObject[i]);
        }

        public object GetListBoxSelectedItem(ListBox listBox)
        {
            return listBox.SelectedItem;
        }

        public void ManageDataGridView(DataGridView gridView, object[] objects, bool reset)
        {
            if (reset)
                dataGridViewPlayers.Rows.Clear();

            for (int i = 0; i < objects.Length; i++)
            {
                int index = dataGridViewPlayers.Rows.Add();
                DataGridViewRow row = dataGridViewPlayers.Rows[index];
                PlayerInfo playerInfoObject = objects[i] as PlayerInfo;
                row.Cells[0].Value = playerInfoObject.Id;
                row.Cells[1].Style.BackColor = playerInfoObject.Color;
                row.Cells[2].Value = playerInfoObject.Name;
                row.Cells[3].Value = GlobalSettings.Instance.GetHouseName(playerInfoObject.House);
                row.Cells[4].Value = playerInfoObject.TeamID.ToString();

                row.Cells[1].Selected = false;
                row.ReadOnly = true;
            }
        }

        public void RemoveDataGridView(DataGridView gridView, object[] removeObjects)
        {
            for (int i = 0; i < removeObjects.Length; i++)
            {
                PlayerInfo playerInfoObject = removeObjects[i] as PlayerInfo;

                for (int j = 0; i < gridView.Rows.Count; j++)
                {
                    DataGridViewRow row = dataGridViewPlayers.Rows[j];

                    if ((short)row.Cells[0].Value == playerInfoObject.Id)
                    {
                        gridView.Rows.RemoveAt(j);
                        break;
                    }
                }
            }
        }

        public void UpdateDataGridView(DataGridView gridView, object updateObject)
        {
            PlayerInfo playerInfoObject = updateObject as PlayerInfo;

            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridViewPlayers.Rows[i];

                if ((short)row.Cells[0].Value == playerInfoObject.Id)
                {
                    row.Cells[1].Style.BackColor = playerInfoObject.Color;
                    row.Cells[3].Value = GlobalSettings.Instance.GetHouseName(playerInfoObject.House);
                    row.Cells[4].Value = playerInfoObject.TeamID.ToString();
                    break;
                }
            }
        }

        public void ManagePictureBoxHouse(PictureBox pictureBox, object objectPictureName)
        {
            string pictureName = objectPictureName as string;
            try
            { pictureBox.Image = Image.FromFile(Settings.Default.UI + "/UI_" + pictureName + ".png"); }
            catch
            { pictureBox.Image = null; }
        }

        public void AddComboBoxItem(ComboBox comboBox, object item)
        {
            if (!comboBox.Items.Contains(item))
                comboBox.Items.Add(item);
        }

        public void ManageComboBoxItems(ComboBox comboBox, Array array)
        {
            comboBox.Items.Clear();
            for(int i = 0; i < array.Length; i++)
                comboBox.Items.Add(array.GetValue(i));
        }

        public void ManageComboBoxItems(ComboBox comboBox, Array array, object defaultItem)
        {
            ManageComboBoxItems(comboBox, array);
            comboBox.SelectedItem = defaultItem;
        }

        public void UpdateComboBox(ComboBox comboBox, object updateObject)
        {
            comboBox.SelectedItem = updateObject;
        }

        public void ManageControlText(Control control, string text)
        {
            control.Text = text;
        }

        public void ManageControlBackColor(Control control, Color backColor)
        {
            control.BackColor = backColor;
        }

        void ConnectionInstance_ConnectionLost(object sender, EventArgs e)
        {
            MessageBoxEx.Show(this, "Connection with server lost", "Connection lost", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if(this.InvokeRequired)
                this.Invoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.MainMenu });
            else
                OnMenuOptionChange(MenuOption.MainMenu);
        }
        #endregion

        #region Main menu
        #region Control Events
        private void NewGameMainMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.NewGame);
        }

        private void OptionsMainMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Options);
        }

        private void exitMainMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Exit);
        }

        private void creditsMainMenu_Click(object sender, EventArgs e)
        {
            MessageBoxEx.Show(this, "Yet Another Dune II" + Environment.NewLine
                                    + "Authors (alphabetical order):" + Environment.NewLine
                                    + "Adam Nowacki, Rados³aw Stankiewicz, Kamil Œlesiñski, Pawe³ Rokoszny, Piotr Witos³awski" + Environment.NewLine + Environment.NewLine
                                    + "Graphics library: Tao Framework for .NET" + Environment.NewLine
                                    + "Sound library: FMOD Sound System, copyright © Firelight Technologies",
                                    "Credits", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
        #endregion

        #region Login menu
        #region Control Events
        private void cancelLoginMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Cancel);
        }

        private void registerLoginMenu_Click(object sender, EventArgs e)
        {
            loginTBRegisterMenu.Text = string.Empty;
            passwordTBRegisterMenu.Text = string.Empty;
            repeatPasswordTBRegisterMenu.Text = string.Empty;
            emailTBRegisterMenu.Text = string.Empty;

            OnMenuOptionChange(MenuOption.Registration);
        }

        private void loginBTLoginMenu_Click(object sender, EventArgs e)
        {
            CancelEventArgs cancel = new CancelEventArgs(false);
            loginMenuCB_Validating(serverLoginMenu, cancel);
            loginMenuTB_Validating(loginTBLoginMenu, cancel);
            loginMenuTB_Validating(passwordLoginMenu, cancel);
            if (cancel.Cancel)
                return;

            InitializationSettings.Instance.Login = loginTBLoginMenu.Text;
            InitializationSettings.Instance.AddServerIP(serverLoginMenu.Text);
            AddComboBoxItem(serverLoginMenu, serverLoginMenu.Text);

            ManageControlState(new Control[] { loginBTLoginMenu, registerLoginMenu, cancelLoginMenu, remindPasswordLoginMenu }, false);

            try
            { Connection.Instance.InitConnection(serverLoginMenu.Text); }
            catch (Exception ex)
            {
                MessageBoxEx.Show(this, ex.Message, "Login error");
                ManageControlState(new Control[] { loginBTLoginMenu, registerLoginMenu, cancelLoginMenu, remindPasswordLoginMenu }, true);
                return;
            }

            LoginMessage loginMessage = (LoginMessage)Utils.CreateMessageWithSenderId(MessageType.Login);
            loginMessage.Login = loginTBLoginMenu.Text;
            loginMessage.Password = passwordLoginMenu.Text;
            ClientPlayerInfo.Player.Name = loginTBLoginMenu.Text;
            Connection.Instance.SendMessage(loginMessage);
        }

        private void remindPasswordLoginMenu_Click(object sender, EventArgs e)
        {
            CancelEventArgs cancel = new CancelEventArgs(false);
            loginMenuCB_Validating(serverLoginMenu, cancel);
            loginMenuTB_Validating(loginTBLoginMenu, cancel);
            if (cancel.Cancel)
                return;

            InitializationSettings.Instance.Login = loginTBLoginMenu.Text;
            InitializationSettings.Instance.AddServerIP(serverLoginMenu.Text);
            AddComboBoxItem(serverLoginMenu, serverLoginMenu.Text);

            ManageControlState(new Control[] { loginBTLoginMenu, registerLoginMenu, cancelLoginMenu, remindPasswordLoginMenu }, false);

            try
            { Connection.Instance.InitConnection(serverLoginMenu.Text); }
            catch (Exception ex)
            {
                MessageBoxEx.Show(this, ex.Message, "Remind error");
                ManageControlState(new Control[] { loginBTLoginMenu, registerLoginMenu, cancelLoginMenu, remindPasswordLoginMenu }, true);
                return;
            }

			TextMessage remindMessage = (TextMessage)Utils.CreateMessageWithSenderId(MessageType.Remind);
            remindMessage.Text = loginTBLoginMenu.Text;
            Connection.Instance.SendMessage(remindMessage);
        }

        private void loginMenuTB_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            bool validated = true;
            string errorText = string.Empty;

            validated = (textBox.Text.Length > 0);
            errorText = "Empty field";

            if (!validated)
            {
                errorProvider.SetError(textBox, errorText);
                e.Cancel = true;
            }
            else
                errorProvider.SetError(textBox, string.Empty);

        }

        private void loginMenuCB_Validating(object sender, CancelEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            bool validated = true;
            string errorText = string.Empty;

            if (comboBox == serverLoginMenu)
            {
                Regex ipAddressPattern = new Regex(@"^(?:(?:25[0-5]|2[0-4]\d|[01]\d\d|\d?\d)(?(?=\.?\d)\.)){4}\b:\d{4}\b$");
                validated = ipAddressPattern.IsMatch(comboBox.Text);
                errorText =  "Not proper ip address and port , e.g. 127.0.0.1:1734";
            }
            else
            {
                validated = (comboBox.Text.Length > 0);
                errorText =  "Empty field";
            }

            if (!validated)
            {
                errorProvider.SetError(comboBox, errorText);
                e.Cancel = true;
            }
            else
                errorProvider.SetError(comboBox, string.Empty);

        }
        #endregion
        #region MenuMessageHandler Events
        void menuMessageHandler_LoginRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Login Event", EPrefix.UIManager);

            Control[] controls = new Control[] { loginBTLoginMenu, registerLoginMenu, cancelLoginMenu, remindPasswordLoginMenu };

            if (InvokeRequired)
                this.Invoke(new ManageControlStateEventHandler(ManageControlState), new object[] { controls, true });
            else
                ManageControlState(controls, true);

            if (e.successful)
            {
                if (InvokeRequired)
                    this.Invoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.Login });
                else
                    OnMenuOptionChange(MenuOption.Login);

                Connection.Instance.SendMessage(Utils.CreateMessageWithSenderId(MessageType.ChatEntry));
            }
            else
            {
                MessageBoxEx.Show(this, e.reason, "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Connection.Instance.CloseConnection();
            }
        }

        void menuMessageHandler_RemindRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Remind Event", EPrefix.UIManager);
            MessageBoxEx.Show(this, e.reason, "Remind", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Control[] controls = new Control[] { loginBTLoginMenu, registerLoginMenu, cancelLoginMenu, remindPasswordLoginMenu };
            if (InvokeRequired) this.Invoke(new ManageControlStateEventHandler(ManageControlState), new object[] { controls, true });
            else ManageControlState(controls, true);

            Connection.Instance.CloseConnection();
        }
        #endregion
        #endregion

        #region Register menu
        #region Control Events
        private void registerRegisterMenu_Click(object sender, EventArgs e)
        {
            CancelEventArgs cancel = new CancelEventArgs(false);
            registerMenu_Validating(loginTBRegisterMenu, cancel);
            registerMenu_Validating(passwordTBRegisterMenu, cancel);
            registerMenu_Validating(repeatPasswordTBRegisterMenu, cancel);
            registerMenu_Validating(emailTBRegisterMenu, cancel);
            if (cancel.Cancel)
                return;

            if (passwordTBRegisterMenu.Text == repeatPasswordTBRegisterMenu.Text)
            {
                InitializationSettings.Instance.AddServerIP(serverLoginMenu.Text);
                AddComboBoxItem(serverLoginMenu, serverLoginMenu.Text);

                ManageControlState(new Control[] { registerRegisterMenu, backRegisterMenu }, false);

                try
                { Connection.Instance.InitConnection(serverLoginMenu.Text); }
                catch (Exception)
                {
                    ManageControlState(new Control[] { registerRegisterMenu, backRegisterMenu }, true);
                    return;
                }

                RegisterMessage registerMessage = new RegisterMessage();
                registerMessage.Login = loginTBRegisterMenu.Text;
                registerMessage.Password = passwordTBRegisterMenu.Text;
                registerMessage.Mail = emailTBRegisterMenu.Text;
                Connection.Instance.SendMessage(registerMessage);
            }
            else
                MessageBoxEx.Show(this, "Repeated password does not match", "Repeat", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void backRegisterMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Back);
        }

        private void registerMenu_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            bool validated = true;
            string errorText = string.Empty;

            if (textBox == emailTBRegisterMenu)
            {
                Regex mailPattern = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
                validated = mailPattern.IsMatch(textBox.Text);
                errorText = "Not proper e-mail address";
            }
            else
            {
                validated = (textBox.Text.Length > 0);
                errorText = "Empty field";
            }

            if (!validated)
            {
                errorProvider.SetError(textBox, errorText);
                e.Cancel = true;
            }
            else
                errorProvider.SetError(textBox, string.Empty);
        }
        #endregion
        #region MenuMessageHandler Events
        void menuMessageHandler_RegisterRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Register Event", EPrefix.UIManager);
            MessageBoxEx.Show(this, e.reason, "Register", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Control[] controls = new Control[] { registerRegisterMenu, backRegisterMenu };
            if (InvokeRequired) this.Invoke(new ManageControlStateEventHandler(ManageControlState), new object[] { controls, true });
            else ManageControlState(controls, true);

            Connection.Instance.CloseConnection();
        }
        #endregion
        #endregion

        #region Chat menu
        #region Control Events
        private void backChatMenu_Click(object sender, EventArgs e)
        {
            Connection.Instance.SendMessage(Utils.CreateMessageWithSenderId(MessageType.Logout));
            Connection.Instance.CloseConnection();

            OnMenuOptionChange(MenuOption.Back);
        }

        private void sendChatMenu_Click(object sender, EventArgs e)
        {
            if (chatInputTBChatMenu.Text == string.Empty)
                return;

            TextMessage chatTextMessage = (TextMessage)Utils.CreateMessageWithSenderId(MessageType.ChatText);
            chatTextMessage.Text = chatInputTBChatMenu.Text;

            chatInputTBChatMenu.Text = string.Empty;
            chatListChatMenu.Items.Add(ClientPlayerInfo.ChatPrefix + chatTextMessage.Text);

            Connection.Instance.SendMessage(chatTextMessage);
        }

        private void userListChatMenu_DoubleClick(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;

            int index = listBox.IndexFromPoint(((MouseEventArgs)e).Location);
            if (index != -1)
            {
                ChatUser chatUser = listBox.Items[index] as ChatUser;
                NumericMessage numericMessage = (NumericMessage)Utils.CreateMessageWithSenderId(MessageType.PlayerInfo);
                numericMessage.Number = chatUser.Id;
                Connection.Instance.SendMessage(numericMessage);
            }
        }

        private void gameChatMenu_Click(object sender, EventArgs e)
        {
            EntryMessage entryMessage = (EntryMessage)Utils.CreateMessageWithSenderId(MessageType.ChooseGameEntry);
            Connection.Instance.SendMessage(entryMessage);

            ManageControlText(textBoxTBGameName, string.Empty);
            ManageControlText(textBoxTBGameDescription, string.Empty);
            OnMenuOptionChange(MenuOption.Game);
        }

        private void chatInputTBChatMenu_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                sendChatMenu_Click(sendChatMenu, EventArgs.Empty);
        }
        #endregion
        #region MenuMessageHandler Events
        void menuMessageHandler_ResetChatUsers(object sender, ChatEventArgs e)
        {
            InfoLog.WriteInfo("Chat Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.Invoke(new ManageListBoxEventHandler(ManageListBox), new object[] { userListChatMenu, e.chatUsers, true });
            else
                ManageListBox(userListChatMenu, e.chatUsers, true);
        }

        void menuMessageHandler_AddChatUsers(object sender, ChatEventArgs e)
        {
            InfoLog.WriteInfo("Chat Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.Invoke(new ManageListBoxEventHandler(ManageListBox), new object[] { userListChatMenu, e.chatUsers, false });
            else
                ManageListBox(userListChatMenu, e.chatUsers, false);

        }

        void menuMessageHandler_DeleteChatUsers(object sender, ChatEventArgs e)
        {
            InfoLog.WriteInfo("Chat Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.Invoke(new RemoveListBoxEventHandler(RemoveListBox), new object[] { userListChatMenu, e.chatUsers });
            else
                RemoveListBox(chatListChatMenu, e.chatUsers);
        }

        void menuMessageHandler_ChatTextReceive(object sender, ChatEventArgs e)
        {
            InfoLog.WriteInfo("Chat Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.Invoke(new ManageListBoxEventHandler(ManageListBox), new object[] { chatListChatMenu, new object[] { e.text }, false });
            else
                ManageListBox(chatListChatMenu, new object[] { e.text }, false);
        }
        #endregion
        #endregion

        #region Player info menu
        #region Control Events
        private void backInfoMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Back);
        }
        #endregion
        #region MenuMessageHandler Events
        void menuMessageHandler_PlayerInfoRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Player Info Event", EPrefix.UIManager);

            if (e.successful)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new ManageControlTextEventHandler(ManageControlText), new object[] { playerInfoLInfoMenu, e.reason });
                    this.Invoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.UserName });
                }
                else
                {
                    ManageControlText(playerInfoLInfoMenu, e.reason);
                    OnMenuOptionChange(MenuOption.UserName);
                }
            }
        }
        #endregion
        #endregion

        #region Choose game menu
        #region Control Events
        private void backChooseGameMenu_Click(object sender, EventArgs e)
        {
            Connection.Instance.SendMessage(Utils.CreateMessageWithSenderId(MessageType.ChatEntry));
            OnMenuOptionChange(MenuOption.Back);
        }

        private void joinChooseGameMenu_Click(object sender, EventArgs e)
        {
            if (textBoxTBGameName.Text == string.Empty)
                return;

            TextMessage textMessage = (TextMessage)Utils.CreateMessageWithSenderId(MessageType.JoinGame);
            textMessage.Text = textBoxTBGameName.Text;
            Connection.Instance.SendMessage(textMessage);
        }

        private void createChooseGameMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Create);
        }

        private void listOfGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            int index = listBox.SelectedIndex;
            if (index != -1)
            {
                GameInfo gameInfo = listBox.Items[index] as GameInfo;
                textBoxTBGameDescription.Text = gameInfo.Description;
                textBoxTBGameName.Text = gameInfo.ToString();
            }
        }
        #endregion
        #region MenuMessageHandler Events
        void menuMessageHandler_ResetGamesInfo(object sender, GameEventArgs e)
        {
            InfoLog.WriteInfo("Games Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.Invoke(new ManageListBoxEventHandler(ManageListBox), new object[] { listOfGames, e.games, true });
            else
                ManageListBox(listOfGames, e.games, true);
        }

        void menuMessageHandler_NewGamesInfo(object sender, GameEventArgs e)
        {
            InfoLog.WriteInfo("New Game Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.Invoke(new ManageListBoxEventHandler(ManageListBox), new object[] { listOfGames, e.games, false });
            else
                ManageListBox(listOfGames, e.games, true);
        }

        void menuMessageHandler_DeleteGamesInfo(object sender, GameEventArgs e)
        {
            InfoLog.WriteInfo("Delete Game Event", EPrefix.UIManager);
            /* Check if not deleting currently selected game */
            GameInfo gameInfo = null;

            if (InvokeRequired) gameInfo = (GameInfo)this.Invoke(new GetListBoxSelectedItemEventHandler(GetListBoxSelectedItem), new object[] { listOfGames });
            else gameInfo = (GameInfo)GetListBoxSelectedItem(listOfGames);

            for (int i = 0; i < e.games.Length; i++)
            {
                if (gameInfo == e.games[i])
                {
                    if (InvokeRequired)
                    {
                        this.Invoke(new ManageControlTextEventHandler(ManageControlText), new object[] { textBoxTBGameName, string.Empty });
                        this.Invoke(new ManageControlTextEventHandler(ManageControlText), new object[] { textBoxTBGameDescription, string.Empty });

                    }
                    else
                    {
                        ManageControlText(textBoxTBGameName, string.Empty);
                        ManageControlText(textBoxTBGameDescription, string.Empty);
                    }
                }

                if (InvokeRequired)
                    this.Invoke(new RemoveListBoxMonoEventHandler(RemoveListBoxMono), new object[] { listOfGames, e.games[i] });
                else
                    RemoveListBoxMono(listOfGames, e.games[i]);
            }
        }

        void menuMessageHandler_JoinGameRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Join Game Event", EPrefix.UIManager);

            if (e.successful)
            {
                Control[] controls = new Control[] { houseCBWaitingForPlayersMenu, teamCBWaitingForPlayersMenu, colorWaitingForPlayersMenu, changeWaitingForPlayersMenu, startWaitingForPlayersMenu };
                if (InvokeRequired)
                {
                    /* Block the possibility to change tha race and team before receiving them from server */
                    this.Invoke(new ManageControlStateEventHandler(ManageControlState), new object[] { controls, false });
                    this.Invoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.Join });
                }
                else
                {
                    /* Block the possibility to change tha race and team before receiving them from server */
                    ManageControlState(controls, false);
                    OnMenuOptionChange(MenuOption.Join);
                }
            }
            else
                MessageBoxEx.Show(this, e.reason, "Join Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
        #endregion

        #region Create game menu
        #region Control Events
        private void cancelCreateGameMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Cancel);
        }

        private void createCreateGameMenu_Click(object sender, EventArgs e)
        {
            CancelEventArgs cancel = new CancelEventArgs(false);
            createGameMenu_Validating(gameNameTBCreateGameMenu, cancel);
            if(cancel.Cancel)
                return;

            GameInfoMessage createGameMessage = (GameInfoMessage)Utils.CreateMessageWithSenderId(MessageType.CreateGame);

            GameInfo gameInfo = new GameInfo();
            if (listBoxLBCreateGame.SelectedItem != null)
            {
                gameInfo.MapName = ((FileInfo)listBoxLBCreateGame.SelectedItem).Name;
                gameInfo.MaxPlayerNumber = (short)maxPlayerNumberNUPCreateGameMenu.Value;
                gameInfo.Name = gameNameTBCreateGameMenu.Text;
                if (publicCreateGameMenu.Checked)
                    gameInfo.GameType = GameType.Public;
                else if (privateCreateGameMenu.Checked)
                    gameInfo.GameType = GameType.Private;
                else
                    gameInfo.GameType = GameType.Public;

                createGameMessage.GameInfo = gameInfo;
                Connection.Instance.SendMessage(createGameMessage);
            }
            else
                MessageBoxEx.Show(this, "No map selected", "Create Game error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void createGameMenu_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            bool validated = true;
            string errorText = string.Empty;

            validated = (textBox.Text.Length > 0);
            errorText = "Empty field";

            if (!validated)
            {
                errorProvider.SetError(textBox, errorText);
                e.Cancel = true;
            }
            else
                errorProvider.SetError(textBox, string.Empty);
        }
        #endregion
        #region MenuMessageHandler Events
        void menuMessageHandler_CreateGameRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Create Game Event", EPrefix.UIManager);
            if (e.successful)
            {
                Control[] controls = new Control[] { houseCBWaitingForPlayersMenu, teamCBWaitingForPlayersMenu, colorWaitingForPlayersMenu, changeWaitingForPlayersMenu, startWaitingForPlayersMenu };
                if (InvokeRequired)
                {
                    /* Block the possibility to change tha race and team before receiving them from server */                    
                    this.Invoke(new ManageControlStateEventHandler(ManageControlState), new object[] { controls, false });
                    this.Invoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.Create });
                }
                else
                {
                    /* Block the possibility to change tha race and team before receiving them from server */
                    ManageControlState(controls, false);
                    OnMenuOptionChange(MenuOption.Create);
                }
            }
            else
                MessageBoxEx.Show(this, e.reason, "Create Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
        #endregion

        #region Waiting for players menu
        #region Control Events
        private void cancelWaitingForPlayersMenu_Click(object sender, EventArgs e)
        {
            Connection.Instance.SendMessage(Utils.CreateMessageWithSenderId(MessageType.ChooseGameEntry));

            OnMenuOptionChange(MenuOption.Cancel);

            ManageControlState(new Control[] { startWaitingForPlayersMenu }, true);
        }

        private void startWaitingForPlayersMenu_Click(object sender, EventArgs e)
        {
            ManageControlState(new Control[] { startWaitingForPlayersMenu }, false);

            TextMessage textMessage = (TextMessage)Utils.CreateMessageWithSenderId(MessageType.StartGame);
            textMessage.Text = ClientPlayerInfo.GameInfo.Name;
            Connection.Instance.SendMessage(textMessage);
        }

        private void CBWaitingForPlayersMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
        }

        private void changeWaitingForPlayersMenu_Click(object sender, EventArgs e)
        {
            PlayersMessage playersMessage = (PlayersMessage)Utils.CreateMessageWithSenderId(MessageType.UpdatePlayer);

            playersMessage.PlayerList = new List<PlayerInfo>();
            PlayerInfo playerInfo = new PlayerInfo();

            playerInfo.Id = ClientPlayerInfo.SenderId;
            playerInfo.Name = ClientPlayerInfo.Player.Name;
            playerInfo.House = ((ItemValue)houseCBWaitingForPlayersMenu.SelectedItem).Value;
            playerInfo.TeamID = (short)teamCBWaitingForPlayersMenu.SelectedItem;
            playerInfo.Color = colorWaitingForPlayersMenu.BackColor;
            playersMessage.PlayerList.Add(playerInfo);

            ManageControlState(new Control[] { houseCBWaitingForPlayersMenu, teamCBWaitingForPlayersMenu, colorWaitingForPlayersMenu, changeWaitingForPlayersMenu, startWaitingForPlayersMenu }, false);

            Connection.Instance.SendMessage(playersMessage);
        }

        private void colorWaitingForPlayersMenu_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog(this) == DialogResult.OK)
                colorWaitingForPlayersMenu.BackColor = colorDialog.Color;
        }
        #endregion
        #region MenuMessageHandler Events
        void menuMessageHandler_GameParamsRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Game Params Event", EPrefix.UIManager);

            if (e.successful)
            {
                short[] houseIDs = GlobalSettings.Instance.GetHouseIDs();

                object[] houseObjects = new object[houseIDs.Length];
                short[] teamObjects = new short[ClientPlayerInfo.GameInfo.MaxPlayerNumber];

                for (int i = 0; i < houseObjects.Length; i++)
                    houseObjects[i] = new ItemValue(houseIDs[i], GlobalSettings.Instance.GetHouseName(houseIDs[i]));

                for (int i = 0; i < teamObjects.Length; i++)
                    teamObjects[i] = (short)(i + 1);

                if (InvokeRequired)
                {
                    this.Invoke(new ManageControlTextEventHandler(ManageControlText), new object[] { descriptionWaitingForPlayersMenu, e.reason });
                    this.Invoke(new ManageComboBoxItemsEventHandlerDefaultItem(ManageComboBoxItems), new object[] { teamCBWaitingForPlayersMenu, teamObjects, teamObjects[0] });
                    this.Invoke(new ManageComboBoxItemsEventHandlerDefaultItem(ManageComboBoxItems), new object[] { houseCBWaitingForPlayersMenu, houseObjects, new ItemValue(GlobalSettings.Instance.DefaultHouse, GlobalSettings.Instance.DefaultHouseName) });
                    this.Invoke(new ManagePictureBoxHouseEventHandler(ManagePictureBoxHouse), new object[] { pictureBoxHouse, GlobalSettings.Instance.DefaultHouseName });
                }
                else
                {
                    ManageControlText(playerInfoLInfoMenu, e.reason);
                    ManageComboBoxItems(teamCBWaitingForPlayersMenu, teamObjects);
                    ManageComboBoxItems(houseCBWaitingForPlayersMenu, houseObjects, new ItemValue(GlobalSettings.Instance.DefaultHouse, GlobalSettings.Instance.DefaultHouseName));
                    ManagePictureBoxHouse(pictureBoxHouse, GlobalSettings.Instance.DefaultHouseName);
                }
            }
        }

        void menuMessageHandler_ResetPlayers(object sender, PlayerEventArgs e)
        {
            InfoLog.WriteInfo("Players Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.Invoke(new ManageDataGridViewEventHandler(ManageDataGridView), new object[] { dataGridViewPlayers, e.players, true });
            else
                ManageDataGridView(dataGridViewPlayers, e.players, true);
        }

        void menuMessageHandler_NewPlayers(object sender, PlayerEventArgs e)
        {
            InfoLog.WriteInfo("Players Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.Invoke(new ManageDataGridViewEventHandler(ManageDataGridView), new object[] { dataGridViewPlayers, e.players, false });
            else
                ManageDataGridView(dataGridViewPlayers, e.players, false);
        }

        void menuMessageHandler_DeletePlayers(object sender, PlayerEventArgs e)
        {
            InfoLog.WriteInfo("Players Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.Invoke(new RemoveDataGridViewEventHandler(RemoveDataGridView), new object[] { dataGridViewPlayers, e.players });
            else
                RemoveDataGridView(dataGridViewPlayers, e.players);
        }

        void menuMessageHandler_UpdatePlayers(object sender, PlayerEventArgs e)
        {
            InfoLog.WriteInfo("Players Event", EPrefix.UIManager);

            for (int i = 0; i < e.players.Length; i++)
            {
                if (e.players[i].Id == ClientPlayerInfo.SenderId)
                {
                    Control[] controls = new Control[] { houseCBWaitingForPlayersMenu, teamCBWaitingForPlayersMenu, colorWaitingForPlayersMenu, changeWaitingForPlayersMenu, startWaitingForPlayersMenu };

                    string infoText = "Login: " + e.players[i].Name + Environment.NewLine
                                    + "House: " + GlobalSettings.Instance.GetHouseName(e.players[i].House) + Environment.NewLine
                                    + "TeamID: " + e.players[i].TeamID.ToString();

                    /* Modify current player */
                    if (InvokeRequired)
                    {
                        /* Update controls */
                        this.Invoke(new UpdateComboBoxEventHandler(UpdateComboBox), new object[] { houseCBWaitingForPlayersMenu, new ItemValue(e.players[i].House, string.Empty) });
                        this.Invoke(new UpdateComboBoxEventHandler(UpdateComboBox), new object[] { teamCBWaitingForPlayersMenu, e.players[i].TeamID });
                        this.Invoke(new ManageControlBackColorEventHandler(ManageControlBackColor), new object[] { colorWaitingForPlayersMenu, e.players[i].Color });
                        this.Invoke(new ManagePictureBoxHouseEventHandler(ManagePictureBoxHouse), new object[] { pictureBoxHouse, GlobalSettings.Instance.GetHouseName(e.players[i].House) });
                        /* Set Information */
                        this.Invoke(new ManageControlTextEventHandler(ManageControlText), new object[] { infoLWaitingForPlayersMenu, infoText });
                        this.Invoke(new ManageControlBackColorEventHandler(ManageControlBackColor), new object[] { infoColorWaitingForPlayersMenu, e.players[i].Color });
                        /* Enable controls */
                        this.Invoke(new ManageControlStateEventHandler(ManageControlState), new object[] { controls, true });
                    }
                    else
                    {
                        /* Update controls */
                        UpdateComboBox(houseCBWaitingForPlayersMenu, new ItemValue(e.players[i].House, string.Empty));
                        UpdateComboBox(teamCBWaitingForPlayersMenu, e.players[i].TeamID);
                        ManageControlBackColor(colorWaitingForPlayersMenu, e.players[i].Color);
                        ManagePictureBoxHouse(pictureBoxHouse, GlobalSettings.Instance.GetHouseName(e.players[i].House));
                        /* Set Information */
                        ManageControlText(infoLWaitingForPlayersMenu, infoText);
                        ManageControlBackColor(infoColorWaitingForPlayersMenu, e.players[i].Color);
                        /* Enable controls */
                        ManageControlState(controls, true);
                    }
                }
                else
                {
                    /* Modify different player */
                    if (InvokeRequired)
                        this.Invoke(new UpdateDataGridViewEventHandler(UpdateDataGridView), new object[] { dataGridViewPlayers, e.players[i] });
                    else
                        UpdateDataGridView(dataGridViewPlayers, e.players[i]);
                }
            }
        }

        void menuMessageHandler_StartGameRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Start Game Event", EPrefix.UIManager);

            if(this.InvokeRequired)
                this.Invoke(new ManageControlStateEventHandler(ManageControlState), new object[] { new Control[] { startWaitingForPlayersMenu }, true });
            else
                ManageControlState(new Control[] { startWaitingForPlayersMenu }, true);

            if (e.successful)
            {   
                /* Change message handler to GameMessageHandler */
                GameMessageHandler.Instance.Suspend();
                Connection.Instance.MessageHandler = GameMessageHandler.Instance;
                if (InvokeRequired)
                    this.Invoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.StartGame });
                else
                    OnMenuOptionChange(MenuOption.StartGame);
            }
            else
                MessageBoxEx.Show(this, e.reason, "Start Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
        #endregion

        #region Options menu
        #region Control Events
        private void cancelOptionsMenu_Click(object sender, EventArgs e)
        {
            /* Return to audio engine settings */
            muteMusicOptionsMenu.Checked = AudioEngine.Instance.Music.IsMuted;
            musicVolumeNMOptionsMenu.Value = AudioEngine.Instance.Music.Volume;

            muteSoundOptionsMenu.Checked = AudioEngine.Instance.Music.IsMuted;
            soundVolumeNMOptionsMenu.Value = AudioEngine.Instance.Sound.Volume;

            if (this.lastView == Views.GameMenuForm)
                OnMenuOptionChange(MenuOption.CancelToGameMenu);
            else if (this.lastView == Views.PauseForm)
                OnMenuOptionChange(MenuOption.CancelToPauseMenu);
            else
                OnMenuOptionChange(MenuOption.Cancel);
        }

        private void okOptionsMenu_Click(object sender, EventArgs e)
        {
            /* Change audio engine settings */
            if (muteMusicOptionsMenu.Checked)
                AudioEngine.Instance.Music.Mute();
            else
                AudioEngine.Instance.Music.Volume = (int)musicVolumeNMOptionsMenu.Value;
            InitializationSettings.Instance.IsMusicMuted = muteMusicOptionsMenu.Checked;
            InitializationSettings.Instance.MusicVolume = (int)musicVolumeNMOptionsMenu.Value;

            if (muteSoundOptionsMenu.Checked)
                InitializationSettings.Instance.IsSoundMuted = true;
            else
                AudioEngine.Instance.Sound.Volume = (int)soundVolumeNMOptionsMenu.Value;
            InitializationSettings.Instance.IsSoundMuted = muteSoundOptionsMenu.Checked;
            InitializationSettings.Instance.SoundVolume = (int)soundVolumeNMOptionsMenu.Value;

            if (this.lastView == Views.GameMenuForm)
                OnMenuOptionChange(MenuOption.OkToGameMenu);
            else if (this.lastView == Views.PauseForm)
                OnMenuOptionChange(MenuOption.OkToPauseMenu);
            else
                OnMenuOptionChange(MenuOption.Ok);
        }

        private void muteMusicOptionsMenu_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            musicVolumeNMOptionsMenu.Enabled = !checkBox.Checked;
        }

        private void muteSoundOptionsMenu_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            soundVolumeNMOptionsMenu.Enabled = !checkBox.Checked;
        }
        #endregion
        #region MenuMessageHandler Events
        #endregion
        #endregion

        #region Game Menu
        #region Control Events
        private void pauseGameMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Pause);
            Connection.Instance.SendMessage(MessageFactory.Create(MessageType.Pause));
        }

        private void optionsGameMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Options);
        }

        private void okGameMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Ok);
        }

        private void exitGameMenu_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        #endregion
        #region MenuMessageHandler Events
        #endregion
        #endregion

        #region Pause menu

        #region Control Events

        private void continuePauseMenu_Click(object sender, EventArgs e)
        {
            Connection.Instance.SendMessage(MessageFactory.Create(MessageType.Resume));
            OnMenuOptionChange(MenuOption.Continue); 
        }

        private void optionsPauseMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Options);
        }

        private void exitPauseMenu_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        #endregion

        #region MenuMessageHandler Events
        #endregion
        #endregion
    }
}