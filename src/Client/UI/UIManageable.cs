using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Client.UI {
    public class UIManageable : Form {
        public event optionChoosen optionChoosen;

        protected void OnOptionChoosen(MenuOption option) {
            if (optionChoosen != null) {
                optionChoosen(option);
            }
        }
    }
}
