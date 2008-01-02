using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Yad.UI.Client {
    public class UIManageable : Form
    {
        public event MenuEventHandler MenuOptionChange;
        public event MessageBoxEventHandler MessageBoxShow;
        protected void OnMenuOptionChange(MenuOptionArg option) {
            if (MenuOptionChange != null) {
                MenuOptionChange(new MenuOptionArg(option.Option, this,option.OverrideActualForm));
            }
        }

        protected void OnMenuOptionChange(MenuOption option) {
            if (MenuOptionChange != null) {
                MenuOptionChange(new MenuOptionArg( option,this));
            }
        }

        protected DialogResult OnMessageBoxShow(String msg, String cap,MessageBoxButtons buttons, MessageBoxIcon icons) {
            if (MessageBoxShow != null) {
                return MessageBoxShow(msg,cap,buttons,icons);
            }
            return DialogResult.None;
        }

        public virtual void Shutdown() {

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
