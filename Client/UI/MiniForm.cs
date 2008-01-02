using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Yad.UI.Client
{
    public partial class MiniForm : Form
    {
        public MiniForm()
        {
            InitializeComponent();
            this.Hide();
            this.Visible = false;
            this.Shown += new EventHandler(MiniForm_Shown);
            this.VisibleChanged += new EventHandler(MiniForm_VisibleChanged);
        }

        void MiniForm_Shown(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        void MiniForm_VisibleChanged(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}