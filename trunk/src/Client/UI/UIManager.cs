using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Client.Log;

namespace Client.UI {

    public delegate void optionChoosen(MenuOption option);
    public class UIManager {
        Views actualView;
        UIManageable actualForm;
        private optionChoosen optionChoosed = null;
        MiniForm mainForm;
        public UIManager(MiniForm mainForm) {
            actualView = Views.MainMenuForm;
            this.mainForm = mainForm;
            optionChoosed = new optionChoosen(form_optionChoosed);
        }

        public void Start() {
            ThreadStartFunction();
        }

        private void ThreadStartFunction() {
            actualForm = FormPool.createForm(Views.MainMenuForm);
            //reset handler
            actualForm.optionChoosen -= optionChoosed;
            actualForm.optionChoosen += optionChoosed;
            actualForm.Show();
        }

        private void Stop() {
            mainForm.Close();
            actualForm.Close();
        }



        void form_optionChoosed(MenuOption option) {
            InfoLog.WriteInfo("OptionChoosed - view: " + actualView + ", option: " + option, EPrefix.UIManager);
            switch (actualView) {
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
                    break;
            }
            InfoLog.WriteInfo("Switched to View: " + actualView, EPrefix.UIManager);
        }

        private void ManageRegistrationForm(MenuOption option) {
            switch (option) {
                case MenuOption.Back:
                    switchView(Views.LoginForm);
                    break;
                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
                    break;
            }
        }

        private void ManageOptionsForm(MenuOption option) {
            switch (option) {
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
                    break;
            }
        }

        private void ManageChatForm(MenuOption option) {
            switch (option) {
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
                    break;
            }
        }

        private void ManageUserInfoForm(MenuOption option) {
            switch (option) {
                case MenuOption.Back:
                    switchView(Views.ChatForm);
                    break;
                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
                    break;
            }
        }

        private void ManageChooseGameForm(MenuOption option) {
            switch (option) {
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
                    break;
            }
        }

        private void ManageGameForm(MenuOption option) {
            switch (option) {
                case MenuOption.Options:
                    switchView(Views.GameMenuForm, false);
                    break;
                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
                    break;
            }
        }

        private void ManageCreateGameForm(MenuOption option) {
            switch (option) {
                case MenuOption.Cancel:
                    switchView(Views.ChooseGameForm);
                    break;
                case MenuOption.Create:
                    switchView(Views.WaitingForPlayersForm);
                    //TODO RS: inform form that player has admin rights.
                    break;
                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
                    break;
            }
        }

        private void ManageWaitingForPlayersForm(MenuOption option) {
            switch (option) {
                case MenuOption.Cancel:
                    switchView(Views.ChooseGameForm);
                    break;
                case MenuOption.StartGame:
                    switchView(Views.GameForm);
                    break;
                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
                    break;
            }
        }

        private void ManagePauseForm(MenuOption option) {
            switch (option) {
                case MenuOption.Exit:
                    throw new NotImplementedException("System exit from " + actualView + ": " + option);
                    break;
                case MenuOption.Options:
                    switchView(Views.OptionsForm);
                    ((MenuForm)(actualForm)).LastView = Views.PauseForm;
                    break;
                case MenuOption.Continue:
                    switchView(Views.GameForm);
                    break;
                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
                    break;
            }
        }

        private void ManageGameMenuForm(MenuOption option) {
            switch (option) {
                case MenuOption.Exit:
                    throw new NotImplementedException("System exit from " + actualView + ": " + option);
                    break;
                case MenuOption.Options:
                    switchView(Views.OptionsForm);
                    ((MenuForm)(actualForm)).LastView = Views.GameMenuForm;
                    break;
                case MenuOption.Back:
                    switchView(Views.GameForm);
                    break;
                case MenuOption.Pause:
                    switchView(Views.PauseForm);
                    break;
                default:
                    throw new NotImplementedException("bad option from " + actualView + ": " + option);
                    break;
            }
        }

        private void ManageLoginForm(MenuOption option) {
            switch (option) {
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
                    break;
            }
        }

        private void ManageMainMenuForm(MenuOption option) {
            switch (option) {
                case MenuOption.Exit:
                    Stop();
                    break;
                case MenuOption.NewGame:
                    switchView(Views.LoginForm);
                    break;
                case MenuOption.Options:
                    switchView(Views.OptionsForm);
                    ((MenuForm)(actualForm)).LastView = Views.MainMenuForm;
                    break;
                default:
                    throw new NotImplementedException("bad option from "+ actualView + ": " + option);
                    break;
            }
        }

        private void switchView(Views viewToSwitch) {
            switchView(viewToSwitch, true);
        }

        private void switchView(Views viewToSwitch, bool hideLast) {
            if(hideLast)
                actualForm.Hide();
            actualView = viewToSwitch;
            actualForm = FormPool.createForm(viewToSwitch);
            //reset handler
            actualForm.optionChoosen -= optionChoosed;
            actualForm.optionChoosen += optionChoosed;
            if(actualForm.Visible == false)
                actualForm.Show(mainForm);
            
        }
    }

}
