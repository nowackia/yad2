using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI {
    class FormPool {

        public static Dictionary<Views, UIManageable> pool = new Dictionary<Views, UIManageable>();
        /// <summary>
        /// creates form. it is initialized in 100% - proper groupbox is set.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>

        public static UIManageable createForm(Views view) {
            UIManageable form = null;
            if (pool.ContainsKey(view)) {
                form = pool[view];
                form = initForm(form, view);
                return form;
            }
            switch (view) {
                case Views.MainMenuForm:
                    form = new MenuForm("groupBoxMainMenu");
                    pool.Add(view, form);
                    break;
                case Views.LoginForm:
                    form = new LoginForm("groupBoxLogin");
                    pool.Add(view, form);
                    break;
                case Views.RegistrationForm:
                    form = new LoginForm("groupBoxRegister");
                    pool.Add(view, form);
                    break;
                case Views.OptionsForm:
                    form = new MenuForm("groupBoxOptions");
                    pool.Add(view, form);
                    break;
                case Views.ChatForm:
                    form = new ChatForm("groupBoxChat");
                    pool.Add(view, form);
                    break;
                case Views.UserInfoForm:
                    form = new ChatForm("groupBoxInfo");
                    pool.Add(view, form);
                    break;
                case Views.ChooseGameForm:
                    form = new ChooseGameForm("groupBoxJoin");
                    pool.Add(view, form);
                    break;
                case Views.GameForm:
                    form = new GameForm();
                    pool.Add(view, form);
                    break;
                case Views.CreateGameForm:
                    form = new ChooseGameForm("groupBoxCreate");
                    pool.Add(view, form);
                    break;
                case Views.WaitingForPlayersForm:
                    form = new ChooseGameForm("groupBoxStart");
                    pool.Add(view, form);
                    break;
                case Views.PauseForm:
                    form = new MenuForm("groupBoxPause");
                    pool.Add(view, form);
                    break;
                case Views.GameMenuForm:
                    form = new MenuForm("groupBoxInGame");
                    pool.Add(view, form);
                    break;
                default:
                    break;
            }
            return form;
        }

        /// <summary>
        /// inits form into proper form - set groupbox name
        /// </summary>
        /// <param name="form"></param>
        /// <param name="initInto"></param>
        /// <returns></returns>
        public static UIManageable initForm(UIManageable form, Views initInto) {
            switch (initInto) {
                case Views.MainMenuForm:
                    ((MenuForm)form).GroupBoxName = "groupBoxMainMenu";
                    break;
                case Views.LoginForm:
                    ((LoginForm)form).GroupBoxName = "groupBoxLogin";
                    break;
                case Views.RegistrationForm:
                    ((LoginForm)form).GroupBoxName = "groupBoxRegister";
                    break;
                case Views.OptionsForm:
                    ((MenuForm)form).GroupBoxName = "groupBoxOptions";
                    break;
                case Views.ChatForm:
                    ((ChatForm)form).GroupBoxName = "groupBoxChat";
                    break;
                case Views.UserInfoForm:
                    ((ChatForm)form).GroupBoxName = "groupBoxInfo";
                    break;
                case Views.ChooseGameForm:
                    ((ChooseGameForm)form).GroupBoxName = "groupBoxJoin";
                    break;
                case Views.GameForm:
                    break;
                case Views.CreateGameForm:
                    ((ChooseGameForm)form).GroupBoxName = "groupBoxCreate";
                    break;
                case Views.WaitingForPlayersForm:
                    ((ChooseGameForm)form).GroupBoxName = "groupBoxStart";
                    break;
                case Views.PauseForm:
                    ((MenuForm)form).GroupBoxName = "groupBoxPause";
                    break;
                case Views.GameMenuForm:
                    ((MenuForm)form).GroupBoxName = "groupBoxInGame";
                    break;
                default:
                    break;
            }
            return form;
        }
    }
}
