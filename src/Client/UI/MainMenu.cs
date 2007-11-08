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
        }
        #endregion

        #region Tab managment
        public void switchToTab(Views view)
        {
            TabPage page = views[view];
            if (page == null)
                throw new NotImplementedException("View " + view + " not exist");

            this.tabControl.SelectTab(page.Name);
        }

        public Views LastView
        {
            get { return lastView; }
            set { lastView = value; }
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
            LoginMessage loginMessage = new LoginMessage();
            loginMessage.Login = loginBTLoginMenu.Text;
            loginMessage.Password = passwordLoginMenu.Text;
            loginMessage.PlayerId = 6;
            Connection.SendMessage(loginMessage);
        }

        private void registerRegisterMenu_Click(object sender, EventArgs e)
        {
            //OnOptionChoosen(MenuOption.Registration);
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

            OnMenuOptionChange(MenuOption.Back);
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