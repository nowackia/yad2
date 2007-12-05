using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Yad.MapEditor
{
    public partial class CreateDialog : Form
    {
        public CreateDialog()
        {
            InitializeComponent();
        }

        public int MapWidth
        {
            get { return (int)this.width.Value; }
            set { this.width.Value = value; }
        }
        public int MapHeight
        {
            get { return (int)this.height.Value; }
            set { this.height.Value = value; }
        }
    }
}
