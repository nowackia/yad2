using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Client.UI {
    public class UIManageable : Form
    {
        public event MenuEventHandler MenuOptionChange;

        protected void OnMenuOptionChange(MenuOption option) {
            if (MenuOptionChange != null) {
                MenuOptionChange(option);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UIManageable
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "UIManageable";
            this.ResumeLayout(false);
        }
    }
}
