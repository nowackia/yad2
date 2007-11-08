using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Yad.Net.General.Messaging;
using Yad.Net;

namespace Client.UI
{
    public partial class MainMenuForm : UIManageable
    {

        Dictionary<Views, TabPage> views = new Dictionary<Views, TabPage>();
        public MainMenuForm()
        {
            InitializeComponent();
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

            Connection.MenuMessageHandler.LoginRequestReply += new Yad.Net.General.RequestReplyEventHandler(MenuMessageHandler_LoginRequestReply);
        }

        void MenuMessageHandler_LoginRequestReply(object sender, Yad.Net.General.RequestReplyEventArgs e)
        {
            MessageBox.Show(e.reason);

            if (InvokeRequired)
                this.BeginInvoke(new MenuOptionDelegate(OnOptionChoosen), new object[] { MenuOption.Login });
            else
                OnOptionChoosen(MenuOption.Login);
        }

        public void switchToTab(Views view)
        {
            TabPage page = views[view];
            if (page == null)
                throw new NotImplementedException("View " + view + " not exist");

            this.tabControl.SelectTab(page.Name);
        }

        private void NewGameMainMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.NewGame);
        }

        private void OptionsMainMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Options);
        }

        private void exitMainMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Exit);
        }

        private void cancelLoginMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Cancel);
        }

        private void registerLoginMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Registration);
        }

        private void loginBTLoginMenu_Click(object sender, EventArgs e)
        {
            LoginMessage loginMessage = new LoginMessage();
            loginMessage.Login = "test";
            loginMessage.Password = "testpsw";
            loginMessage.UserId = 6;
            Connection.SendMessage(loginMessage);
        }

        private void registerRegisterMenu_Click(object sender, EventArgs e)
        {
            //OnOptionChoosen(MenuOption.Registration);
        }

        private void backRegisterMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Back);
        }

        private void continuePauseMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Continue);
        }

        private void optionsPauseMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Options);
        }

        private void exitPauseMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Exit);
        }

        private void pauseGameMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Pause);
        }

        private void optionsGameMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Options);
        }

        private void okGameMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Ok);
        }

        private void exitGameMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Exit);
        }


        private Views lastView;

        public Views LastView
        {
            get { return lastView; }
            set { lastView = value; }
        }


        private void cancelOptionsMenu_Click(object sender, EventArgs e)
        {
            if (this.lastView == Views.GameMenuForm)
            {
                OnOptionChoosen(MenuOption.CancelToGameMenu);
            }
            else if (this.lastView == Views.PauseForm)
            {
                OnOptionChoosen(MenuOption.CancelToPauseMenu);
            }
            else
            {
                OnOptionChoosen(MenuOption.Cancel);
            }
        }

        private void okOptionsMenu_Click(object sender, EventArgs e)
        {
            if (this.lastView == Views.GameMenuForm)
            {
                OnOptionChoosen(MenuOption.OkToGameMenu);
            }
            else if (this.lastView == Views.PauseForm)
            {
                OnOptionChoosen(MenuOption.OkToPauseMenu);
            }
            else
            {
                OnOptionChoosen(MenuOption.Ok);
            }
        }

        private void backChatMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Back);
        }

        private void gameChatMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Game);
        }

        private void haxxx_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Game);
        }

        private void backInfoMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Back);
        }

        private void backChooseGameMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Back);
        }

        private void joinChooseGameMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Join);
        }

        private void createChooseGameMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Create);
        }

        private void listOfGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO RS: wyswietlenie description
        }

        private void cancelCreateGameMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Cancel);
        }

        private void createCreateGameMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Create);
        }

        private void startWaitingForPlayersMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.StartGame);
        }

        private void cancelWaitingForPlayersMenu_Click(object sender, EventArgs e)
        {
            OnOptionChoosen(MenuOption.Cancel);
        }
    }
}