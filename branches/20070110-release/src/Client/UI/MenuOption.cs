using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Yad.UI.Client {

    public class MenuOptionArg {
        private MenuOption option;
        private bool overrideFrom = false ;

        public bool OverrideActualForm {
            get { return overrideFrom; }
            set { overrideFrom = value; }
        }

        public MenuOption Option {
            get { return option; }
        }

        public Form Sender {
            get { return sender; }
        }


        Form sender;

        public MenuOptionArg(MenuOption option, Form sender) {
            this.sender = sender;
            this.option = option;
        }

        public MenuOptionArg(MenuOption option, Form sender, bool overrideActualForm):this(option,sender) {
            this.overrideFrom = overrideActualForm;
        }
    }

    public enum MenuOption {
        MainMenu,
        Exit,
        Ok,
        OkToGameMenu,
        OkToPauseMenu,
        NewGame,
        Cancel,
        CancelToGameMenu,
        CancelToPauseMenu,
        Login,
        Back,
        Registration,
        Options,
        UserName,
        Game,
        Join,
        Create,
        StartGame,
        Pause,
        Continue,
        GameFormToChat
    }
}
