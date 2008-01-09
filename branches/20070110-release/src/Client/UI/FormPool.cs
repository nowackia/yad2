using System;
using System.Collections.Generic;
using System.Text;
using Yad.UI.Client;

namespace Yad.UI.Client
{
    class FormPool
    {

        public static Dictionary<Views, UIManageable> pool = new Dictionary<Views, UIManageable>();
        /// <summary>
        /// Creates form.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>


        public static UIManageable[] Forms {
            get {
                Dictionary<Views, UIManageable>.ValueCollection coll = pool.Values;
                UIManageable [] views = new UIManageable[coll.Count];
                pool.Values.CopyTo(views,0);

                return views;
            }
        }

        static FormPool()
        {
            UIManageable form = null;

            form = new MainMenuForm();
            InitMainMenu(form);

        }

        private static void InitMainMenu(UIManageable form)
        {
            pool.Add(Views.MainMenuForm, form);
            pool.Add(Views.LoginForm, form);
            pool.Add(Views.RegistrationForm, form);
            pool.Add(Views.OptionsForm, form);
            pool.Add(Views.ChatForm, form);
            pool.Add(Views.UserInfoForm, form);
            pool.Add(Views.ChooseGameForm, form);
            pool.Add(Views.CreateGameForm, form);
            pool.Add(Views.WaitingForPlayersForm, form);
            pool.Add(Views.PauseForm, form);
            pool.Add(Views.GameMenuForm, form);
        }

        public static UIManageable GetForm(Views view)
        {
            UIManageable form = null;
            if (pool.ContainsKey(view))
            {
                form = pool[view];

                if (form.IsDisposed)
                    pool.Remove(view);
                else
                {
                    form = InitForm(form, view);
                    return form;
                }
            }

            switch (view)
            {
                case Views.MainMenuForm:
                case Views.LoginForm:
                case Views.RegistrationForm:
                case Views.OptionsForm:
                case Views.ChatForm:
                case Views.UserInfoForm:
                case Views.ChooseGameForm:
                case Views.CreateGameForm:
                case Views.WaitingForPlayersForm:
                case Views.PauseForm:
                case Views.GameMenuForm:
                    form = new MainMenuForm();
                    form.Hide();
                    InitMainMenu(form);
                    break;

                case Views.GameForm:
                    form = new GameForm();
                    pool.Add(view, form);
                    break;

                default:
                    break;
            }
            return form;
        }

        /// <summary>
        /// Inits form into proper form - set groupbox name
        /// </summary>
        /// <param name="form"></param>
        /// <param name="initInto"></param>
        /// <returns></returns>
        public static UIManageable InitForm(UIManageable form, Views initInto)
        {
            switch (initInto)
            {
                case Views.MainMenuForm:
                    ((MainMenuForm)form).SwitchToTab(initInto);
                    break;
                case Views.LoginForm:
                    ((MainMenuForm)form).SwitchToTab(initInto);
                    break;
                case Views.RegistrationForm:
                    ((MainMenuForm)form).SwitchToTab(initInto);
                    break;
                case Views.OptionsForm:
                    ((MainMenuForm)form).SwitchToTab(initInto);
                    break;
                case Views.ChatForm:
                    ((MainMenuForm)form).SwitchToTab(initInto);
                    break;
                case Views.UserInfoForm:
                    ((MainMenuForm)form).SwitchToTab(initInto);
                    break;
                case Views.ChooseGameForm:
                    ((MainMenuForm)form).SwitchToTab(initInto);
                    break;
                case Views.CreateGameForm:
                    ((MainMenuForm)form).SwitchToTab(initInto);
                    break;
                case Views.WaitingForPlayersForm:
                    ((MainMenuForm)form).SwitchToTab(initInto);
                    break;
                case Views.PauseForm:
                    ((MainMenuForm)form).SwitchToTab(initInto);
                    break;
                case Views.GameMenuForm:
                    ((MainMenuForm)form).SwitchToTab(initInto);
                    break;
                case Views.GameForm:
                    break;
                default:
                    break;
            }
            return form;
        }
    }
}
