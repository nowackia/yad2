using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Yad.UI.Client {
    public class UIManageable : Form
    {
        public event MenuEventHandler MenuOptionChange;

        protected void OnMenuOptionChange(MenuOptionArg option) {
            if (MenuOptionChange != null) {
                MenuOptionChange(new MenuOptionArg(option.Option, this));
            }
        }

        protected void OnMenuOptionChange(MenuOption option) {
            if (MenuOptionChange != null) {
                MenuOptionChange(new MenuOptionArg( option,this));
            }
        }

        private void InitializeComponent()
        {
            //this.SuspendLayout();
            // 
            // UIManageable
            // 
            //this.ClientSize = new System.Drawing.Size(292, 273);
            //this.Name = "UIManageable";
            //this.ResumeLayout(false);
        }
    }
}
