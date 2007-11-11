using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Client.Net;
using System.Text;
using System.Windows.Forms;
using Yad.Log.Common;
using Yad.Net;
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
            views.Add(Views.UserInfoForm, infoMenu);
            views.Add(Views.WaitingForPlayersForm, waitingForPlayersMenu);
            #endregion

            #region ComboBox DataGridView Settings
            foreach(object obj in Enum.GetValues(typeof(HouseType)))
                House.Items.Add(obj.ToString());

            int index = dataGridViewPlayers.Rows.Add();
            DataGridViewRow row = dataGridViewPlayers.Rows[index];
            row.Cells[0].Value = "Test";
            row.Cells[1].Value = HouseType.Harkonnen.ToString();
            row.Cells[2].Value = "Team Test";
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

            menuMessageHandler.JoinGameRequestReply += new RequestReplyEventHandler(menuMessageHandler_JoinGameRequestReply);
            menuMessageHandler.ResetGamesInfo += new GamesEventHandler(menuMessageHandler_ResetGamesInfo);
            menuMessageHandler.NewGamesInfo += new GamesEventHandler(menuMessageHandler_NewGamesInfo);
            menuMessageHandler.DeleteGamesInfo += new GamesEventHandler(menuMessageHandler_DeleteGamesInfo);

            menuMessageHandler.NewPlayers += new PlayersEventHandler(menuMessageHandler_NewPlayers);
            menuMessageHandler.DeletePlayers += new PlayersEventHandler(menuMessageHandler_DeletePlayers);
            menuMessageHandler.ResetPlayers += new PlayersEventHandler(menuMessageHandler_ResetPlayers);
            menuMessageHandler.UpdatePlayers += new PlayersEventHandler(menuMessageHandler_UpdatePlayers);

            menuMessageHandler.StartGameRequestReply += new RequestReplyEventHandler(menuMessageHandler_StartGameRequestReply);

            Connection.MessageHandler = menuMessageHandler;
            #endregion
        }

        #region MenuMessageHandler Events
        void menuMessageHandler_StartGameRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Start Game Event", EPrefix.UIManager);
            MessageBox.Show(e.reason, "Start Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (e.successful)
            {
                if (InvokeRequired) this.BeginInvoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.Game });
                else OnMenuOptionChange(MenuOption.Game);
            }
        }


        void menuMessageHandler_UpdatePlayers(object sender, PlayerEventArgs e)
        {
            throw new NotImplementedException("Not implemented yet");
        }

        void menuMessageHandler_ResetPlayers(object sender, PlayerEventArgs e)
        {
            InfoLog.WriteInfo("Players Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new ManageDataGridViewEventHandler(ManageDataGridView), new object[] { dataGridViewPlayers, e.players, true });
            else
                ManageDataGridView(dataGridViewPlayers, e.players, true);
        }

        void menuMessageHandler_DeletePlayers(object sender, PlayerEventArgs e)
        {
            InfoLog.WriteInfo("Players Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new RemoveDataGridViewEventHandler(RemoveDataGridView), new object[] { dataGridViewPlayers, e.players });
            else
                RemoveDataGridView(dataGridViewPlayers, e.players);
        }

        void menuMessageHandler_NewPlayers(object sender, PlayerEventArgs e)
        {
            InfoLog.WriteInfo("Players Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new ManageDataGridViewEventHandler(ManageDataGridView), new object[] { dataGridViewPlayers, e.players, false });
            else
                ManageDataGridView(dataGridViewPlayers, e.players, true);
        }

        void menuMessageHandler_DeleteGamesInfo(object sender, GameEventArgs e)
        {
            InfoLog.WriteInfo("Games Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new RemoveListBoxEventHandler(RemoveListBox), new object[] { listOfGames, e.games });
            else
                RemoveListBox(listOfGames, e.games);
        }

        void menuMessageHandler_NewGamesInfo(object sender, GameEventArgs e)
        {
            InfoLog.WriteInfo("Games Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new ManageListBoxEventHandler(ManageListBox), new object[] { listOfGames, e.games, false });
            else
                ManageListBox(listOfGames, e.games, true);
        }

        void menuMessageHandler_ResetGamesInfo(object sender, GameEventArgs e)
        {
            InfoLog.WriteInfo("Games Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new ManageListBoxEventHandler(ManageListBox), new object[] { listOfGames, e.games, true });
            else
                ManageListBox(listOfGames, e.games, true);
        }

        void menuMessageHandler_LoginRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Login Event", EPrefix.UIManager);
            MessageBox.Show(e.reason, "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (e.successful)
            {
                if (InvokeRequired) this.BeginInvoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.Login });
                else OnMenuOptionChange(MenuOption.Login);

                Connection.SendMessage(MessageFactory.Create(MessageType.ChatEntry));
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

        void menuMessageHandler_ChatTextReceive(object sender, ChatEventArgs e)
        {
            InfoLog.WriteInfo("Chat Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new ManageListBoxEventHandler(ManageListBox), new object[] { chatListChatMenu, new object[] { e.text }, false });
            else
                ManageListBox(chatListChatMenu, new object[] { e.text }, false);
        }

        void menuMessageHandler_DeleteChatUsers(object sender, ChatEventArgs e)
        {
            InfoLog.WriteInfo("Chat Event", EPrefix.UIManager);
            if (InvokeRequired)
                this.BeginInvoke(new RemoveListBoxEventHandler(RemoveListBox), new object[] { userListChatMenu, e.chatUsers });
            else
                RemoveListBox(chatListChatMenu, e.chatUsers);
        }

        void menuMessageHandler_PlayerInfoRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Player Info Event", EPrefix.UIManager);

            if (e.successful)
            {
                if (InvokeRequired) this.BeginInvoke(new ManageControlTextEventHandler(ManageControlText), new object[] { userListChatMenu, e.reason });
                else ManageControlText(chatListChatMenu, e.reason);
            }
        }


        void menuMessageHandler_JoinGameRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Join Game Event", EPrefix.UIManager);
            MessageBox.Show(e.reason, "Join Game", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (e.successful)
            {
                if (InvokeRequired) this.BeginInvoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.Join });
                else OnMenuOptionChange(MenuOption.Join); ;

                Connection.SendMessage(MessageFactory.Create(MessageType.PlayersList));
            }
        }

        #endregion

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
                listBox.Items.Add(objects);
        }

        public void RemoveListBox(ListBox listBox, object[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
                listBox.Items.Remove(objects);
        }

        public void ManageDataGridView(DataGridView gridView, object[] objects, bool reset)
        {
            if (reset)
                dataGridViewPlayers.Rows.Clear();

            for (int i = 0; i < objects.Length; i++)
            {
                int index = dataGridViewPlayers.Rows.Add();
                DataGridViewRow row = dataGridViewPlayers.Rows[index];
                row.Cells[0].Value = objects[0];
                row.Cells[1].Value = objects[1];
                row.Cells[2].Value = objects[2];
                row.ReadOnly = true;
            }
        }

        public void RemoveDataGridView(DataGridView gridView, object[] objects)
        {
            throw new NotImplementedException("Not implemented yet");
        }

        public void ManageControlText(Control control, string text)
        {
            control.Text = text;
        }
        #endregion

        #region Button Menu clicks
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

            LoginMessage loginMessage = new LoginMessage();
            loginMessage.Login = loginBTLoginMenu.Text;
            loginMessage.Password = passwordLoginMenu.Text;
            Connection.SendMessage(loginMessage);
        }

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

        private void remindPasswordLoginMenu_Click(object sender, EventArgs e)
        {
            Connection.InitConnection(serverLoginMenu.Text, 1734);

            TextMessage remindMessage = new TextMessage(MessageType.Remind);
            remindMessage.Text = loginTBRegisterMenu.Text;
            Connection.SendMessage(remindMessage);

            ManageControlState(new Control[] { loginBTLoginMenu, registerLoginMenu, cancelLoginMenu, remindPasswordLoginMenu }, false);
        }

        private void backRegisterMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Back);
        }

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

        private void backChatMenu_Click(object sender, EventArgs e)
        {
            Connection.SendMessage(MessageFactory.Create(MessageType.Logout));
            Connection.CloseConnection();

            OnMenuOptionChange(MenuOption.Back);
        }

        private void sendChatMenu_Click(object sender, EventArgs e)
        {
            TextMessage chatTextMessage = (TextMessage)MessageFactory.Create(MessageType.ChatText);
            //TODO: (AN) safe player id
            chatTextMessage.PlayerId = 4;
            chatTextMessage.Text = chatInputTBChatMenu.Text;

            Connection.SendMessage(chatTextMessage);
        }


        private void userListChatMenu_DoubleClick(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;

            int index = listBox.IndexFromPoint(((MouseEventArgs)e).Location);
            if (index != -1)
            {
                ChatUser chatUser = listBox.Items[index] as ChatUser;

                MessageBox.Show(chatUser.Id + " " + chatUser.Name);
                //TODO: (AN) Sending PlayerInfoMessage
            }
        }

        private void gameChatMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Game);
        }

        private void haxxx_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Game);
        }

        private void backInfoMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Back);
        }

        private void backChooseGameMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Back);
        }

        private void joinChooseGameMenu_Click(object sender, EventArgs e)
        {
            TextMessage textMessage = (TextMessage)MessageFactory.Create(MessageType.JoinGameEntry);
            textMessage.Text = textBoxTBGameName.Text;
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
            if(index != -1)
            {
                GameInfo gameInfo = listBox.Items[index] as GameInfo;
                textBoxTBGameDescription.Text = gameInfo.Description;
                textBoxTBGameName.Text = gameInfo.ToString();
            }
        }

        private void cancelCreateGameMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Cancel);
        }

        private void createCreateGameMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Create);
        }

        private void startWaitingForPlayersMenu_Click(object sender, EventArgs e)
        {
            TextMessage textMessage = MessageFactory.Create(MessageType.StartGame) as TextMessage;
            textMessage.Text = "FALSE GAME";
            Connection.SendMessage(textMessage);

            OnMenuOptionChange(MenuOption.StartGame);
        }

        private void cancelWaitingForPlayersMenu_Click(object sender, EventArgs e)
        {
            Connection.SendMessage(MessageFactory.Create(MessageType.ChooseGameEntry));

            OnMenuOptionChange(MenuOption.Cancel);
        }

        #endregion
    }
}