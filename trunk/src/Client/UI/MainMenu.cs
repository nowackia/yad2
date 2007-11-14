using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Yad.Config;
using Yad.Log.Common;
using Yad.Net;
using Yad.Net.Client;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.Engine.Common;

namespace Client.UI
{
    public partial class MainMenuForm : UIManageable
    {
        private Dictionary<Views, TabPage> views = new Dictionary<Views, TabPage>();
        private Views lastView;

        public MainMenuForm()
        {
            InitializeComponent();

            #region Views Settings
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
            #endregion

            #region ComboBox DataGridView Settings
            //TODO (AN) change to reading races from xml
            foreach(object obj in Enum.GetValues(typeof(HouseType)))
                House.Items.Add(obj.ToString());

            int index = dataGridViewPlayers.Rows.Add();
            DataGridViewRow row = dataGridViewPlayers.Rows[index];
            row.Cells[0].Value = ClientPlayerInfo.Login;
            row.Cells[1].Value = HouseType.Harkonnen.ToString();
            //TODO (AN) do something with team ID - numericUpdown ?
            row.Cells[2].Value = "1";
            #endregion

            #region MenuMessageHandler Settings
            MenuMessageHandler menuMessageHandler = new MenuMessageHandler();

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

            Connection.MessageHandler = menuMessageHandler;
            #endregion
        }

        #region Controls managment
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

            this.tabControl.SelectTab(page.Name);
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

        public void RemoveListBox(ListBox listBox, object[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
                listBox.Items.Remove(objects[i]);
        }

        public void ManageDataGridView(DataGridView gridView, object[] objects, bool reset)
        {
            if (reset)
            {
                for (int i = dataGridViewPlayers.Rows.Count - 1; i > 0; i--)
                    dataGridViewPlayers.Rows.RemoveAt(i);
            }

            for (int i = 0; i < objects.Length; i++)
            {
                int index = dataGridViewPlayers.Rows.Add();
                DataGridViewRow row = dataGridViewPlayers.Rows[index];
                PlayerInfo playerInfoObject = objects[i] as PlayerInfo;
                row.Cells[0].Value = playerInfoObject.Name;
                row.Cells[1].Value = playerInfoObject.House.ToString();
                row.Cells[2].Value = playerInfoObject.TeamID;
                row.ReadOnly = true;
            }
        }

        public void RemoveDataGridView(DataGridView gridView, object removeObject)
        {
            PlayerInfo playerInfoObject = removeObject as PlayerInfo;

            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridViewPlayers.Rows[i];

                if ((string)row.Cells[0].Value == playerInfoObject.Name)
                {
                    gridView.Rows.RemoveAt(i);
                    break;
                }
            }
        }

        public void UpdateDataGridView(DataGridView gridView, object updateObject)
        {
            PlayerInfo playerInfoObject = updateObject as PlayerInfo;

            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridViewPlayers.Rows[i];

                if ((string)row.Cells[0].Value == playerInfoObject.Name)
                {
                    row.Cells[1].Value = playerInfoObject.House.ToString();
                    row.Cells[2].Value = playerInfoObject.TeamID;
                    break;
                }
            }

            //TODO (AN) chceck if we modify this local user's info
        }

        public void ManageControlText(Control control, string text)
        {
            control.Text = text;
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

        }

        private void haxxx_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Game);
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
            OnMenuOptionChange(MenuOption.Registration);
        }

        private void loginBTLoginMenu_Click(object sender, EventArgs e)
        {
            Connection.InitConnection(serverLoginMenu.Text, 1734);

            LoginMessage loginMessage = (LoginMessage)Utils.CreateMessageWithPlayerId(MessageType.Login);
            loginMessage.Login = loginTBLoginMenu.Text;
            loginMessage.Password = passwordLoginMenu.Text;
            ClientPlayerInfo.Login = loginTBLoginMenu.Text;
            Connection.SendMessage(loginMessage);

            ManageControlState(new Control[] { loginBTLoginMenu }, false);
        }

        private void remindPasswordLoginMenu_Click(object sender, EventArgs e)
        {
            Connection.InitConnection(serverLoginMenu.Text, 1734);

            TextMessage remindMessage = (TextMessage)Utils.CreateMessageWithPlayerId(MessageType.Remind);
            remindMessage.Text = loginTBRegisterMenu.Text;
            Connection.SendMessage(remindMessage);

            ManageControlState(new Control[] { loginBTLoginMenu, registerLoginMenu, cancelLoginMenu, remindPasswordLoginMenu }, false);
        }
        #endregion
        #region MenuMessageHandler Events
        void menuMessageHandler_LoginRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Login Event", EPrefix.UIManager);
            MessageBox.Show(e.reason, "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (InvokeRequired)
                this.BeginInvoke(new ManageControlStateEventHandler(ManageControlState), new object[] { new Control[] { loginBTLoginMenu }, true });
            else
                ManageControlState(new Control[] { loginBTLoginMenu }, true);

            if (e.successful)
            {
                if (InvokeRequired)
                    this.BeginInvoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.Login });
                else
                    OnMenuOptionChange(MenuOption.Login);

                Connection.SendMessage(Utils.CreateMessageWithPlayerId(MessageType.ChatEntry));
            }
        }

        void menuMessageHandler_RemindRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Remind Event", EPrefix.UIManager);
            MessageBox.Show(e.reason, "Remind", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (e.successful)
            {
                Control[] controls = new Control[] { loginBTLoginMenu, registerLoginMenu, cancelLoginMenu, remindPasswordLoginMenu };
                if (InvokeRequired) this.BeginInvoke(new ManageControlStateEventHandler(ManageControlState), new object[] { controls, true });
                else ManageControlState(controls, true);
            }

            Connection.CloseConnection();
        }
        #endregion
        #endregion

        #region Register menu
        #region Control Events
        private void registerRegisterMenu_Click(object sender, EventArgs e)
        {
            Connection.InitConnection(serverLoginMenu.Text, 1734);

            RegisterMessage registerMessage = new RegisterMessage();
            registerMessage.Login = loginTBRegisterMenu.Text;
            registerMessage.Password = passwordTBRegisterMenu.Text;
            registerMessage.Mail = emailTBRegisterMenu.Text;
            Connection.SendMessage(registerMessage);

            ManageControlState(new Control[] { registerRegisterMenu, backRegisterMenu }, false);
        }

        private void backRegisterMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Back);
        }
        #endregion
        #region MenuMessageHandler Events
        void menuMessageHandler_RegisterRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Register Event", EPrefix.UIManager);
            MessageBox.Show(e.reason, "Register", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (e.successful)
            {
                Control[] controls = new Control[] { registerRegisterMenu, backRegisterMenu };
                if (InvokeRequired) this.BeginInvoke(new ManageControlStateEventHandler(ManageControlState), new object[] { controls, true });
                else ManageControlState(controls, true);
            }

            Connection.CloseConnection();
        }
        #endregion
        #endregion

        #region Chat menu
        #region Control Events
        private void backChatMenu_Click(object sender, EventArgs e)
        {
            Connection.SendMessage(Utils.CreateMessageWithPlayerId(MessageType.Logout));
            Connection.CloseConnection();

            OnMenuOptionChange(MenuOption.Back);
        }

        private void sendChatMenu_Click(object sender, EventArgs e)
        {
            TextMessage chatTextMessage = (TextMessage)Utils.CreateMessageWithPlayerId(MessageType.ChatText);

            chatTextMessage.Text = chatInputTBChatMenu.Text;

            chatInputTBChatMenu.Text = string.Empty;
            chatListChatMenu.Items.Add(ClientPlayerInfo.ChatPrefix + chatTextMessage.Text);

            Connection.SendMessage(chatTextMessage);
        }

        private void userListChatMenu_DoubleClick(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;

            int index = listBox.IndexFromPoint(((MouseEventArgs)e).Location);
            if (index != -1)
            {
                ChatUser chatUser = listBox.Items[index] as ChatUser;
                NumericMessage numericMessage = (NumericMessage)Utils.CreateMessageWithPlayerId(MessageType.PlayerInfo);
                numericMessage.Number = chatUser.Id;
                Connection.SendMessage(numericMessage);
            }
        }

        private void gameChatMenu_Click(object sender, EventArgs e)
        {
            EntryMessage entryMessage = (EntryMessage)Utils.CreateMessageWithPlayerId(MessageType.ChooseGameEntry);
            Connection.SendMessage(entryMessage);

            OnMenuOptionChange(MenuOption.Game);
        }
        #endregion
        #region MenuMessageHandler Events
        void menuMessageHandler_ResetChatUsers(object sender, ChatEventArgs e)
        {
            InfoLog.WriteInfo("Chat Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new ManageListBoxEventHandler(ManageListBox), new object[] { userListChatMenu, e.chatUsers, true });
            else
                ManageListBox(userListChatMenu, e.chatUsers, true);
        }

        void menuMessageHandler_AddChatUsers(object sender, ChatEventArgs e)
        {
            InfoLog.WriteInfo("Chat Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new ManageListBoxEventHandler(ManageListBox), new object[] { userListChatMenu, e.chatUsers, false });
            else
                ManageListBox(userListChatMenu, e.chatUsers, false);

        }

        void menuMessageHandler_DeleteChatUsers(object sender, ChatEventArgs e)
        {
            InfoLog.WriteInfo("Chat Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new RemoveListBoxEventHandler(RemoveListBox), new object[] { userListChatMenu, e.chatUsers });
            else
                RemoveListBox(chatListChatMenu, e.chatUsers);
        }

        void menuMessageHandler_ChatTextReceive(object sender, ChatEventArgs e)
        {
            InfoLog.WriteInfo("Chat Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new ManageListBoxEventHandler(ManageListBox), new object[] { chatListChatMenu, new object[] { e.text }, false });
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
                if (InvokeRequired) this.BeginInvoke(new ManageControlTextEventHandler(ManageControlText), new object[] { playerInfoLInfoMenu, e.reason });
                else ManageControlText(playerInfoLInfoMenu, e.reason);
            }
        }
        #endregion
        #endregion

        #region Choose game menu
        #region Control Events
        private void backChooseGameMenu_Click(object sender, EventArgs e)
        {
            Connection.SendMessage(Utils.CreateMessageWithPlayerId(MessageType.ChatEntry));
            OnMenuOptionChange(MenuOption.Back);
        }

        private void joinChooseGameMenu_Click(object sender, EventArgs e)
        {
            TextMessage textMessage = (TextMessage)Utils.CreateMessageWithPlayerId(MessageType.JoinGame);
            textMessage.Text = textBoxTBGameName.Text;
            ClientPlayerInfo.GameName = gameNameTBCreateGameMenu.Text;
            Connection.SendMessage(textMessage);
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
                this.BeginInvoke(new ManageListBoxEventHandler(ManageListBox), new object[] { listOfGames, e.games, true });
            else
                ManageListBox(listOfGames, e.games, true);
        }

        void menuMessageHandler_NewGamesInfo(object sender, GameEventArgs e)
        {
            InfoLog.WriteInfo("Games Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new ManageListBoxEventHandler(ManageListBox), new object[] { listOfGames, e.games, false });
            else
                ManageListBox(listOfGames, e.games, true);
        }

        void menuMessageHandler_DeleteGamesInfo(object sender, GameEventArgs e)
        {
            InfoLog.WriteInfo("Games Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new RemoveListBoxEventHandler(RemoveListBox), new object[] { listOfGames, e.games });
            else
                RemoveListBox(listOfGames, e.games);
        }

        void menuMessageHandler_JoinGameRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Join Game Event", EPrefix.UIManager);
            MessageBox.Show(e.reason, "Join Game", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (e.successful)
            {
                if (InvokeRequired) this.BeginInvoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.Join });
                else OnMenuOptionChange(MenuOption.Join); ;
            }
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
            GameInfoMessage createGameMessage = (GameInfoMessage)Utils.CreateMessageWithPlayerId(MessageType.CreateGame);

            GameInfo gameInfo = new GameInfo();
            //TODO (AN) Get somehow MapId
            if (listBoxLBCreateGame.SelectedItem != null)
            {
                gameInfo.MapId = short.Parse(listBoxLBCreateGame.SelectedItem.ToString());
                gameInfo.MaxPlayerNumber = (short)maxPlayerNumberNUPCreateGameMenu.Value;
                gameInfo.Name = gameNameTBCreateGameMenu.Text;
                if (publicCreateGameMenu.Checked)
                    gameInfo.GameType = GameType.Public;
                else if (privateCreateGameMenu.Checked)
                    gameInfo.GameType = GameType.Private;
                else
                    gameInfo.GameType = GameType.Public;

                ClientPlayerInfo.GameName = gameNameTBCreateGameMenu.Text;

                createGameMessage.GameInfo = gameInfo;
                Connection.SendMessage(createGameMessage);
            }
            else
                MessageBox.Show("No map selected", "Create Game error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion
        #region MenuMessageHandler Events
        void menuMessageHandler_CreateGameRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Create Game Event", EPrefix.UIManager);
            MessageBox.Show(e.reason, "Create Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (e.successful)
            {
                if (InvokeRequired) this.BeginInvoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.Create });
                else OnMenuOptionChange(MenuOption.Create);
            }
        }
        #endregion
        #endregion

        #region Waiting for players menu
        #region Control Events
        private void cancelWaitingForPlayersMenu_Click(object sender, EventArgs e)
        {
            Connection.SendMessage(Utils.CreateMessageWithPlayerId(MessageType.ChooseGameEntry));

            OnMenuOptionChange(MenuOption.Cancel);

            ManageControlState(new Control[] { startWaitingForPlayersMenu }, true);
        }

        private void startWaitingForPlayersMenu_Click(object sender, EventArgs e)
        {
            ManageControlState(new Control[] { startWaitingForPlayersMenu }, false);

            TextMessage textMessage = (TextMessage)Utils.CreateMessageWithPlayerId(MessageType.StartGame);
            textMessage.Text = ClientPlayerInfo.GameName;
            Connection.SendMessage(textMessage);
        }

        private void dataGridViewPlayers_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox comboBox = e.Control as ComboBox;

            if (comboBox != null)
            {
                comboBox.SelectedIndexChanged -= new EventHandler(comboBox_SelectedIndexChanged);
                comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            }
        }

        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            //TODO (AN) Send Message with changed information
            
        }
        #endregion
        #region MenuMessageHandler Events
        void menuMessageHandler_GameParamsRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Game Params Event", EPrefix.UIManager);

            if (e.successful)
            {
                if (InvokeRequired) this.BeginInvoke(new ManageControlTextEventHandler(ManageControlText), new object[] { descriptionWaitingForPlayersMenu, e.reason });
                else ManageControlText(playerInfoLInfoMenu, e.reason);
            }
        }

        void menuMessageHandler_ResetPlayers(object sender, PlayerEventArgs e)
        {
            InfoLog.WriteInfo("Players Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new ManageDataGridViewEventHandler(ManageDataGridView), new object[] { dataGridViewPlayers, e.players, true });
            else
                ManageDataGridView(dataGridViewPlayers, e.players, true);
        }

        void menuMessageHandler_NewPlayers(object sender, PlayerEventArgs e)
        {
            InfoLog.WriteInfo("Players Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new ManageDataGridViewEventHandler(ManageDataGridView), new object[] { dataGridViewPlayers, e.players, false });
            else
                ManageDataGridView(dataGridViewPlayers, e.players, false);
        }

        void menuMessageHandler_DeletePlayers(object sender, PlayerEventArgs e)
        {
            InfoLog.WriteInfo("Players Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new RemoveDataGridViewEventHandler(RemoveDataGridView), new object[] { dataGridViewPlayers, e.players[0] });
            else
                RemoveDataGridView(dataGridViewPlayers, e.players[0]);
        }

        void menuMessageHandler_UpdatePlayers(object sender, PlayerEventArgs e)
        {
            InfoLog.WriteInfo("Players Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new RemoveDataGridViewEventHandler(UpdateDataGridView), new object[] { dataGridViewPlayers, e.players[0] });
            else
                UpdateDataGridView(dataGridViewPlayers, e.players[0]);
        }

        void menuMessageHandler_StartGameRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Start Game Event", EPrefix.UIManager);
            MessageBox.Show(e.reason, "Start Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (e.successful)
            {
                //TODO (AN) Before starting the game switch to GameMessageHandler
                if (InvokeRequired)
                {
                    this.BeginInvoke(new ManageControlStateEventHandler(ManageControlState), new object[] { new Control[] { startWaitingForPlayersMenu }, true });
                    this.BeginInvoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.StartGame });
                }
                else
                {
                    ManageControlState(new Control[] { startWaitingForPlayersMenu }, true);
                    OnMenuOptionChange(MenuOption.StartGame);
                }
            }
        }
        #endregion
        #endregion

        #region Options menu
        #region Control Events
        private void cancelOptionsMenu_Click(object sender, EventArgs e)
        {
            if (this.lastView == Views.GameMenuForm)
                OnMenuOptionChange(MenuOption.CancelToGameMenu);
            else if (this.lastView == Views.PauseForm)
                OnMenuOptionChange(MenuOption.CancelToPauseMenu);
            else
                OnMenuOptionChange(MenuOption.Cancel);
        }

        private void okOptionsMenu_Click(object sender, EventArgs e)
        {
            if (this.lastView == Views.GameMenuForm)
                OnMenuOptionChange(MenuOption.OkToGameMenu);
            else if (this.lastView == Views.PauseForm)
                OnMenuOptionChange(MenuOption.OkToPauseMenu);
            else
                OnMenuOptionChange(MenuOption.Ok);
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
            OnMenuOptionChange(MenuOption.Exit);
        }
        #endregion
        #region MenuMessageHandler Events
        #endregion
        #endregion

        #region Pause menu
        #region Control Events
        private void continuePauseMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Continue);
        }

        private void optionsPauseMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Options);
        }

        private void exitPauseMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Exit);
        }
        #endregion
        #region MenuMessageHandler Events
        #endregion
        #endregion
    }
}