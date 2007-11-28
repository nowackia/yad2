using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Yad.Log;
using Yad.Log.Common;
using Yad.Net.Common;

namespace Yad.UI.Client
{
    public delegate void FormShowEventHandler(Form form);
    public delegate void SelectTabEventHandler(String tabName);
    public delegate void MenuEventHandler(MenuOption option);
    public delegate void ManageControlTextEventHandler(Control control, string text);
    public delegate void ManageControlBackColorEventHandler(Control control, Color backColor);
    public delegate void ManageControlStateEventHandler(Control[] option, bool state);
    public delegate void ManageComboBoxItemsEventHandler(ComboBox comboBox, Array array);
    public delegate void ManageComboBoxItemsEventHandlerDefaultItem(ComboBox comboBox, Array array, object defaultItem);
    public delegate void UpdateComboBoxEventHandler(ComboBox comboBox, object updateObject);
    public delegate void ManageListBoxEventHandler(ListBox listBox, object[] objects, bool reset);
    public delegate void RemoveListBoxMonoEventHandler(ListBox listBox, object removeObject);
    public delegate void RemoveListBoxEventHandler(ListBox listBox, object[] removeObjects);
    public delegate object GetListBoxSelectedItemEventHandler(ListBox listBox);
    public delegate void ManageDataGridViewEventHandler(DataGridView gridView, object[] objects, bool reset);
    public delegate void RemoveDataGridViewEventHandler(DataGridView gridView, object[] removeObjects);
    public delegate void UpdateDataGridViewEventHandler(DataGridView gridView, object updateObjects);

    public class UIManager
    {
        Views actualView;
        UIManageable actualForm;
        private MenuEventHandler menuEventHandler = null;
        MiniForm mainForm;

        public UIManager(MiniForm mainForm)
        {
            actualView = Views.MainMenuForm;
            this.mainForm = mainForm;
            menuEventHandler = new MenuEventHandler(form_optionChoosed);
        }

        public void Start()
        {
            ThreadStartFunction();
        }

        private void ThreadStartFunction()
        {
            actualForm = FormPool.GetForm(Views.MainMenuForm);
            //reset handler
            actualForm.MenuOptionChange -= menuEventHandler;
            actualForm.MenuOptionChange += menuEventHandler;
            actualForm.Show();
        }

        private void Stop()
        {
            mainForm.Close();
            actualForm.Close();
        }

        void form_optionChoosed(MenuOption option)
        {
            InfoLog.WriteInfo("OptionChoosed - view: " + actualView + ", option: " + option, EPrefix.UIManager);

            if (option == MenuOption.MainMenu)
            {
                switchView(Views.MainMenuForm);
                return;
            }

            switch (actualView)
            {
                case Views.MainMenuForm:
                    ManageMainMenuForm(option);
                    break;

                case Views.LoginForm:
                    ManageLoginForm(option);
                    break;

                case Views.RegistrationForm:
                    ManageRegistrationForm(option);
                    break;

                case Views.OptionsForm:
                    ManageOptionsForm(option);
                    break;

                case Views.ChatForm:
                    ManageChatForm(option);
                    break;

                case Views.UserInfoForm:
                    ManageUserInfoForm(option);
                    break;

                case Views.ChooseGameForm:
                    ManageChooseGameForm(option);
                    break;

                case Views.GameForm:
                    ManageGameForm(option);
                    break;

                case Views.CreateGameForm:
                    ManageCreateGameForm(option);
                    break;

                case Views.WaitingForPlayersForm:
                    ManageWaitingForPlayersForm(option);
                    break;

                case Views.PauseForm:
                    ManagePauseForm(option);
                    break;

                case Views.GameMenuForm:
                    ManageGameMenuForm(option);
                    break;

                default:
                    throw new NotImplementedException("not supported form: " + actualView + ", option: " + option);
            }
            InfoLog.WriteInfo("Switched to View: " + actualView, EPrefix.UIManager);
        }

        private void ManageRegistrationForm(MenuOption option)
        {
            switch (option)
            {
                case MenuOption.Back:
                    switchView(Views.LoginForm);
                    break;

                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
            }
        }

        private void ManageOptionsForm(MenuOption option)
        {
            switch (option)
            {
                case MenuOption.CancelToGameMenu:
                    switchView(Views.GameMenuForm);
                    break;

                case MenuOption.Cancel:
                    switchView(Views.MainMenuForm);
                    break;

                case MenuOption.CancelToPauseMenu:
                    switchView(Views.PauseForm);
                    break;

                case MenuOption.OkToGameMenu:
                    switchView(Views.GameMenuForm);
                    break;

                case MenuOption.Ok:
                    switchView(Views.MainMenuForm);
                    break;

                case MenuOption.OkToPauseMenu:
                    switchView(Views.PauseForm);
                    break;

                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
            }
        }

        private void ManageChatForm(MenuOption option)
        {
            switch (option)
            {
                case MenuOption.Game:
                    switchView(Views.ChooseGameForm);
                    break;

                case MenuOption.Back:
                    switchView(Views.LoginForm);
                    break;

                case MenuOption.UserName:
                    switchView(Views.UserInfoForm);
                    break;

                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
            }
        }

        private void ManageUserInfoForm(MenuOption option)
        {
            switch (option)
            {
                case MenuOption.Back:
                    switchView(Views.ChatForm);
                    break;

                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
            }
        }

        private void ManageChooseGameForm(MenuOption option)
        {
            switch (option)
            {
                case MenuOption.Back:
                    switchView(Views.ChatForm);
                    break;

                case MenuOption.Join:
                    switchView(Views.WaitingForPlayersForm);
                    //TODO RS: inform form that player dont have admin rights.
                    break;

                case MenuOption.Create:
                    switchView(Views.CreateGameForm);
                    break;

                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
            }
        }

        private void ManageGameForm(MenuOption option)
        {
            switch (option)
            {
                case MenuOption.Options:
                    switchView(Views.GameMenuForm, false);
                    break;

                case MenuOption.GameFormToChat:
                    switchView(Views.ChatForm);
                    ((MainMenuForm)(actualForm)).LastView = Views.MainMenuForm;
                    break;

                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
            }
        }

        private void ManageCreateGameForm(MenuOption option)
        {
            switch (option)
            {
                case MenuOption.Cancel:
                    switchView(Views.ChooseGameForm);
                    break;

                case MenuOption.Create:
                    switchView(Views.WaitingForPlayersForm);
                    break;

                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
            }
        }

        private void ManageWaitingForPlayersForm(MenuOption option)
        {
            switch (option)
            {
                case MenuOption.Cancel:
                    switchView(Views.ChooseGameForm);
                    break;

                case MenuOption.StartGame:
                    switchView(Views.GameForm, true);
                    break;

                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
            }
        }

        private void ManagePauseForm(MenuOption option)
        {
            switch (option)
            {
                case MenuOption.Exit:
                    throw new NotImplementedException("System exit from " + actualView + ": " + option);

                case MenuOption.Options:
                    switchView(Views.OptionsForm);
                    ((MainMenuForm)(actualForm)).LastView = Views.PauseForm;
                    break;

                case MenuOption.Continue:
                    switchView(Views.GameForm, true);
                    break;

                default:
                    throw new NotImplementedException("Bad option from " + actualView + ": " + option);
            }
        }

        private void ManageGameMenuForm(MenuOption option)
        {
            switch (option)
            {
                case MenuOption.Exit:
                    throw new NotImplementedException("System exit from " + actualView + ": " + option);

                case MenuOption.Options:
                    switchView(Views.OptionsForm);
                    ((MainMenuForm)(actualForm)).LastView = Views.GameMenuForm;
                    break;

                case MenuOption.Ok:
                    switchView(Views.GameForm, true);
                    break;

                case MenuOption.Pause:
                    switchView(Views.PauseForm);
                    break;

                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
            }
        }

        private void ManageLoginForm(MenuOption option)
        {
            switch (option)
            {
                case MenuOption.Cancel:
                    switchView(Views.MainMenuForm);
                    break;

                case MenuOption.Login:
                    switchView(Views.ChatForm);
                    break;

                case MenuOption.Registration:
                    switchView(Views.RegistrationForm);
                    break;

                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
            }
        }

        private void ManageMainMenuForm(MenuOption option)
        {
            switch (option)
            {
                case MenuOption.Exit:
                    Stop();
                    break;

                case MenuOption.NewGame:
                    switchView(Views.LoginForm);
                    break;

                case MenuOption.Options:
                    switchView(Views.OptionsForm);
                    ((MainMenuForm)(actualForm)).LastView = Views.MainMenuForm;
                    break;

                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
            }
        }

        private void switchView(Views viewToSwitch)
        {
            switchView(viewToSwitch, false);
        }

        private void switchView(Views viewToSwitch, bool hideLast)
        {
            if (hideLast)
                actualForm.Hide();

            actualView = viewToSwitch;
            actualForm = FormPool.GetForm(viewToSwitch);

            //reset handler
            actualForm.MenuOptionChange -= menuEventHandler;
            actualForm.MenuOptionChange += menuEventHandler;

            if (actualForm.Visible == false)
            {
                if(actualForm.InvokeRequired)
                    actualForm.Invoke(new FormShowEventHandler(actualForm.Show), new object[] { mainForm });
                else
                    actualForm.Show(mainForm);
            }
        }
    }
}
