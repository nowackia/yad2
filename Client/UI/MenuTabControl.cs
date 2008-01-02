using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Yad.UI {
	class MenuTabControl : TabControl {

		protected override bool IsInputKey(Keys key) {
			return false;
		}
	}
}
