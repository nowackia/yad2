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

            #region MenuMessageHandler Settings
            MenuMessageHandler menuMessageHandler = new MenuMessageHandler();

            menuMessageHandler.LoginRequestReply += new RequestReplyEventHandler(menuMessageHandler_LoginRequestReply);
            menuMessageHandler.RegisterRequestReply += new RequestReplyEventHandler(menuMessageHandler_RegisterRequestReply);
            menuMessageHandler.RemindRequestReply += new RequestReplyEventHandler(menuMessageHandler_RemindRequestReply);
            menuMessageHandler.ResetChatUsers += new ChatEventHandler(menuMessageHandler_ResetChatUsers);
            menuMessageHandler.NewChatUsers += new ChatEventHandler(menuMessageHandler_AddChatUsers);
            //menuMessageHandler.DeleteChatUsers
            menuMessageHandler.ChatTextReceive += new ChatEventHandler(menuMessageHandler_ChatTextReceive);

            Connection.MessageHandler = menuMessageHandler;
            #endregion
        }

        #region MenuMessageHandler Events
        void menuMessageHandler_LoginRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Login Event", EPrefix.UIManager);
            MessageBox.Show(e.reason, "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (InvokeRequired)
                this.BeginInvoke(new MenuEventHandler(OnMenuOptionChange), new object[] { MenuOption.Login });
            else
                OnMenuOptionChange(MenuOption.Login);

            Connection.SendMessage(MessageFactory.Create(MessageType.ChatEntry));
        }

        void menuMessageHandler_RemindRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Remind Event", EPrefix.UIManager);
            MessageBox.Show(e.reason, "Remind", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Control[] controls = new Control[] { loginBTLoginMenu, registerLoginMenu, cancelLoginMenu, remindPasswordLoginMenu };
            if (InvokeRequired)
                this.BeginInvoke(new ControlStateEventHandler(ControlState), new object[] { controls, true });
            else
                ControlState(controls, true);

            Connection.CloseConnection();
        }

        void menuMessageHandler_RegisterRequestReply(object sender, RequestReplyEventArgs e)
        {
            InfoLog.WriteInfo("Register Event", EPrefix.UIManager);
            MessageBox.Show(e.reason, "Register", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Control[] controls = new Control[] { registerRegisterMenu, backRegisterMenu };
            if (InvokeRequired)
                this.BeginInvoke(new ControlStateEventHandler(ControlState), new object[] { controls, true });
            else
                ControlState(controls, true);

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

        public void ControlState(Control[] control, bool state)
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

            ControlState(new Control[] { registerRegisterMenu, backRegisterMenu }, false);
        }

        private void remindPasswordLoginMenu_Click(object sender, EventArgs e)
        {
            Connection.InitConnection(serverLoginMenu.Text, 1734);

            TextMessage remindMessage = new TextMessage(MessageType.Remind);
            remindMessage.Text = loginTBRegisterMenu.Text;
            Connection.SendMessage(remindMessage);

            ControlState(new Control[] { loginBTLoginMenu, registerLoginMenu, cancelLoginMenu, remindPasswordLoginMenu }, false);
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
            OnMenuOptionChange(MenuOption.Join);
        }

        private void createChooseGameMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Create);
        }

        private void listOfGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO RS: wyswietlenie description
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
            OnMenuOptionChange(MenuOption.StartGame);
        }

        private void cancelWaitingForPlayersMenu_Click(object sender, EventArgs e)
        {
            OnMenuOptionChange(MenuOption.Cancel);
        }

        private void descriptionWaitingForPlayersMenu_TextChanged(object sender, EventArgs e)
        { }
        #endregion
    }
}